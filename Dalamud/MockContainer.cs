using System.Reflection;
using System.Runtime.Serialization;
using DalaMock.Interfaces;
using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockContainer
{
    private readonly IPluginLog _log;

    public MockContainer(IPluginLog log)
    {
        _log = log;
    }
    
    private object[] GetPublicIocScopes(IEnumerable<object> scopedObjects) => scopedObjects.Append<object>((object) this).ToArray<object>();

    private readonly Dictionary<Type, Type> _interfaceToTypeMap = new Dictionary<Type, Type>();
    private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
    
    public T? Create<T>(params object[] scopedObjects) where T : class
    {
        return (T) this.CreateSync(typeof (T), this.GetPublicIocScopes((IEnumerable<object>) scopedObjects));
    }
    public T? Create<T>(IServiceContainer serviceContainer, params object[] scopedObjects) where T : class
    {
        return (T) this.CreateSync(serviceContainer.GetType(), this.GetPublicIocScopes((IEnumerable<object>) scopedObjects));
    }


    public void AddTypeMapping(Type interfaceType, Type mockType)
    {
        _interfaceToTypeMap.Add(interfaceType, mockType);
    }

    public void AddMockInstance(Type interfaceType, object instance)
    {
        _instances.Add(interfaceType, instance);
    }

    public void AddInstance(Type realInterface, object instance)
    {
        AddTypeMapping(realInterface, instance.GetType());
        AddMockInstance(instance.GetType(), instance);
    }
    
    public object? CreateSync(
      Type objectType,
      object[] scopedObjects)
    {
      ConstructorInfo? ctor = this.FindApplicableCtor(objectType, scopedObjects);
      if (ctor == null)
      {
        _log.Error("Failed to create {TypeName}, an eligible ctor with satisfiable services could not be found", (object) objectType.FullName);
        return (object) null;
      }
      List<Type> list = ((IEnumerable<ParameterInfo>) ctor.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => (p.ParameterType))).ToList<Type>();

      object[] resolvedParams = list.Select((Func<Type, object>) (p =>
      {
        object service = this.GetService(p, scopedObjects);
        if (service == null)
          _log.Error("Requested ctor service type {TypeName} was not available (null)", (object) p.FullName);
        return service;
      })).ToArray();
      
      if (((IEnumerable<object>) resolvedParams).Any<object>((Func<object, bool>) (p => p == null)))
      {
        _log.Error("Failed to create {TypeName}, a requested service type could not be satisfied", (object) objectType.FullName);
        return (object) null;
      }
      object instance = FormatterServices.GetUninitializedObject(objectType);
      if (!this.InjectProperties(instance, scopedObjects))
      {
        _log.Error("Failed to create {TypeName}, a requested property service type could not be satisfied", (object) objectType.FullName);
        return (object) null;
      }
      ctor.Invoke(instance, resolvedParams);
      return instance;
    }    
    
    private ConstructorInfo? FindApplicableCtor(Type type, object[] scopedObjects)
    {
        Type[] array = ((IEnumerable<object>) scopedObjects).Select<object, Type>((Func<object, Type>) (o => o.GetType())).Union<Type>((IEnumerable<Type>) this._instances.Keys).ToArray<Type>();
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
        if (type.Assembly == Assembly.GetExecutingAssembly())
            bindingAttr |= BindingFlags.NonPublic;
        foreach (ConstructorInfo constructor in type.GetConstructors(bindingAttr))
        {
            if (this.ValidateCtor(constructor, array))
                return constructor;
        }
        return (ConstructorInfo) null;
    }

    private bool ValidateCtor(ConstructorInfo ctor, Type[] types)
    {
        foreach (ParameterInfo parameter in ctor.GetParameters())
        {
            bool flag = IsTypeValid(parameter.ParameterType);
            Type type;
            if (!flag && this._interfaceToTypeMap.TryGetValue(parameter.ParameterType, out type))
                flag = IsTypeValid(type);
            if (!flag)
            {
                _log.Error("Failed to validate {TypeName}, unable to find any services that satisfy the type", (object) parameter.ParameterType.FullName);
                return false;
            }
        }
        return true;

        bool IsTypeValid(Type type) => ((IEnumerable<Type>) types).Any<Type>((Func<Type, bool>) (x => x.IsAssignableTo(type)));
    }

    public bool InjectProperties(object instance, object[] publicScopes)
    {
        Type type = instance.GetType();
        (PropertyInfo, RequiredVersionAttribute)[] array = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where((Func<PropertyInfo, bool>)(x => x.GetCustomAttributes(typeof(PluginServiceAttribute)).Any())).Select(
                (Func<PropertyInfo, (PropertyInfo, RequiredVersionAttribute)>)(propertyInfo =>
                {
                    RequiredVersionAttribute customAttribute =
                        propertyInfo.GetCustomAttribute(typeof(RequiredVersionAttribute)) as RequiredVersionAttribute;
                    return (propertyInfo, customAttribute);
                })).ToArray();
        (PropertyInfo, RequiredVersionAttribute)[] valueTupleArray = array;
        for (int index = 0; index < valueTupleArray.Length; ++index)
        {
            (PropertyInfo, RequiredVersionAttribute) prop = valueTupleArray[index];
            object service = GetService(prop.Item1.PropertyType, publicScopes);
            if (service == null)
            {
                _log.Error("Requested service type {TypeName} was not available (null)",
                    prop.Item1.PropertyType.FullName);
                //For the sake of mocking, log the error but continue trying to inject other properties
                continue;
            }

            prop.Item1.SetValue(instance, service);
        }

        return true;
    }

    private object? GetService(
        Type serviceType,
        object[] scopedObjects)
    {
        Type type;
        if (_interfaceToTypeMap.TryGetValue(serviceType, out type))
            serviceType = type;
        object singletonService = GetSingletonService(serviceType, false);
        return singletonService == null
            ? scopedObjects.FirstOrDefault((Func<object, bool>)(o => o.GetType().IsAssignableTo(serviceType))) ??
              (object)null
            : singletonService;
    }

    private object? GetSingletonService(Type serviceType, bool tryGetInterface = true)
    {
        Type type;
        if (tryGetInterface && _interfaceToTypeMap.TryGetValue(serviceType, out type))
            serviceType = type;
        object? objectInstance;
        return !_instances.TryGetValue(serviceType, out objectInstance) ? null : objectInstance;
    }
}