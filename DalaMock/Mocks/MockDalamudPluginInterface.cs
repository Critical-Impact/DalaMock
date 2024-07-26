namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using DalaMock.Core.Plugin;
using Dalamud.Configuration;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Internal.Types.Manifest;
using Dalamud.Plugin.Ipc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

public class MockDalamudPluginInterface : IDalamudPluginInterface, IDisposable
{
    private readonly string internalName;
    private readonly IComponentContext componentContext;

    // private readonly MockProgram _mockProgram;
    private MockUiBuilder mockUiBuilder;
    private PluginConfiguration pluginConfiguration;
    private PluginLoadReason pluginLoadReason;

    public MockDalamudPluginInterface(
        MockUiBuilder mockUiBuilder,
        PluginLoadSettings pluginLoadSettings,
        string internalName,
        IComponentContext componentContext)
    {
        // _mockProgram = mockProgram;
        this.internalName = internalName;
        this.componentContext = componentContext;
        this.ConfigFile = pluginLoadSettings.ConfigFile;
        this.ConfigDirectory = pluginLoadSettings.ConfigDir;
        this.pluginLoadReason = pluginLoadSettings.PluginLoadReason;
        this.AssemblyLocation = new FileInfo((pluginLoadSettings.AssemblyLocation ?? Environment.ProcessPath)!);
        this.pluginConfiguration = new PluginConfiguration(pluginLoadSettings.ConfigDir.FullName);
        this.mockUiBuilder = mockUiBuilder;
    }

    public event IDalamudPluginInterface.LanguageChangedDelegate? LanguageChanged;

    public event IDalamudPluginInterface.ActivePluginsChangedDelegate? ActivePluginsChanged;

    /// <summary>
    /// Provides access the mock ui builder.
    /// </summary>
    public MockUiBuilder MockUiBuilder => this.mockUiBuilder;

    public Task InjectAsync(object instance, params object[] scopedObjects)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public PluginLoadReason Reason => this.pluginLoadReason;

    public bool IsAutoUpdateComplete => true;

    public string SourceRepository => "DevPlugin";

    public string InternalName => this.internalName;

    public IPluginManifest Manifest { get; }

    public bool IsDev => true;

    public bool IsTesting => false;

    public DateTime LoadTime => DateTime.Now;

    public DateTime LoadTimeUTC => DateTime.Now;

    public TimeSpan LoadTimeDelta => TimeSpan.Zero;

    public DirectoryInfo DalamudAssetDirectory => new("");

    public FileInfo ConfigFile { get; }

    public IUiBuilder UiBuilder => this.mockUiBuilder;

    public bool IsDevMenuOpen => true;

    public bool IsDebugging => true;

    public string UiLanguage => "English";

    public ISanitizer Sanitizer => new Sanitizer(ClientLanguage.English);

    public XivChatType GeneralChatType => XivChatType.Debug;

    public IEnumerable<IExposedPlugin> InstalledPlugins => new List<IExposedPlugin>();

    public bool OpenPluginInstallerTo(
        PluginInstallerOpenKind openTo = PluginInstallerOpenKind.AllPlugins,
        string? searchText = null)
    {
        throw new NotImplementedException();
    }

    public bool OpenDalamudSettingsTo(SettingsOpenKind openTo = SettingsOpenKind.General, string? searchText = null)
    {
        return false;
    }

    public bool OpenDeveloperMenu()
    {
        return false;
    }

    public T GetOrCreateData<T>(string tag, Func<T> dataGenerator) where T : class
    {
        return null!;
    }

    public void RelinquishData(string tag)
    {
    }

    public bool TryGetData<T>(string tag, out T? data) where T : class
    {
        data = null;
        return false;
    }

    public T? GetData<T>(string tag) where T : class
    {
        return null!;
    }

    public DirectoryInfo ConfigDirectory { get; }

    public FileInfo AssemblyLocation { get; }

    public ICallGateProvider<TRet> GetIpcProvider<TRet>(string name)
    {
        return new MockCallGateProvider<TRet>(name);
    }

    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
        return new MockCallGateProvider<T1, TRet>(name);
    }

    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(name);
    }

    public ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8,
        TRet>(string name)
    {
        return null!;
    }

    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
        var serializedConfig = SerializeConfig(currentConfig);
        File.WriteAllText(this.ConfigFile.FullName, serializedConfig);
        if (currentConfig != null)
        {
            this.pluginConfiguration.Save(currentConfig, this.InternalName);
        }
    }

    public IPluginConfiguration? GetPluginConfig()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(IPluginConfiguration)))
            {
                var mi = this.pluginConfiguration.GetType().GetMethod("LoadForType");
                var fn = mi.MakeGenericMethod(type);
                return (IPluginConfiguration)fn.Invoke(this.pluginConfiguration, new object[] { this.InternalName });
            }
        }

        return null!;
    }

    public string GetPluginConfigDirectory()
    {
        return Path.Combine(this.ConfigDirectory.FullName, this.InternalName);
    }

    public string GetPluginLocDirectory()
    {
        return Path.Combine(Path.Combine(this.ConfigDirectory.FullName, this.InternalName), "loc");
    }

    public DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction)
    {
        return null!;
    }

    public void RemoveChatLinkHandler(uint commandId)
    {
    }

    public void RemoveChatLinkHandler()
    {
    }

    /// <inheritdoc/>
    public T? Create<T>(params object[] scopedObjects) where T : class
    {
        var logger = this.componentContext.Resolve<ILogger>();
        var mockServices = this.componentContext.Resolve<IEnumerable<IMockService>>();
        Dictionary<Type, object> mockServicesDict = new();
        foreach (var mockService in mockServices)
        {
            foreach (var interfaceType in mockService.GetType().GetInterfaces())
            {
                mockServicesDict.TryAdd(interfaceType, mockService);
            }
        }

        foreach (var scopedObject in scopedObjects)
        {
            foreach (var interfaceType in scopedObject.GetType().GetInterfaces())
            {
                mockServicesDict.TryAdd(interfaceType, scopedObject);
            }
        }

        // Get the type of the class to instantiate
        var typeToCreate = typeof(T);

        // Get all constructors of the class
        var constructors = typeToCreate.GetConstructors();
        T? newInstance = null;

        // Try to find a suitable constructor
        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var parameterValues = new List<object>();

            foreach (var parameter in parameters)
            {
                // Try to resolve each parameter from the mock services
                if (mockServicesDict.TryGetValue(parameter.ParameterType, out var mockService))
                {
                    parameterValues.Add(mockService);
                }
                else
                {
                    parameterValues = null;
                    break;
                }
            }

            // If all parameters are resolved, create an instance and return it
            if (parameterValues != null)
            {
                newInstance = (T)constructor.Invoke(parameterValues.ToArray());
            }
        }

        // If no suitable constructor is found, try to use a parameterless constructor
        var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);
        if (parameterlessConstructor != null)
        {
            newInstance = (T)parameterlessConstructor.Invoke(null);
        }

        if (newInstance == null)
        {
            // If no suitable constructor is found, return null
            logger.Error($"No suitable constructor found for type '{typeToCreate.FullName}' that can be resolved with the provided mock services.");
            return null;
        }

        var properties = newInstance.GetType().GetProperties()
            .Where(prop => prop.PropertyType.IsPublic && Attribute.IsDefined(prop, typeof(PluginServiceAttribute)) && prop.CanWrite && prop.CanRead);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (mockServicesDict.TryGetValue(propertyType, out var mockService))
            {
                property.SetValue(newInstance, mockService);
            }
            else
            {
                logger.Error($"No matching mock service found for property '{property.Name}' of type '{propertyType.FullName}' in object of type '{newInstance.GetType().FullName}'.");
            }
        }

        return newInstance;
    }

    public Task<T> CreateAsync<T>(params object[] scopedObjects) where T : class
    {
        throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public bool Inject(object instance, params object[] scopedObjects)
    {
        var logger = this.componentContext.Resolve<ILogger>();
        var mockServices = this.componentContext.Resolve<IEnumerable<IMockService>>();
        Dictionary<Type, object> mockServicesDict = new();
        foreach (var mockService in mockServices)
        {
            foreach (var interfaceType in mockService.GetType().GetInterfaces())
            {
                mockServicesDict.TryAdd(interfaceType, mockService);
            }
        }

        foreach (var scopedObject in scopedObjects)
        {
            foreach (var interfaceType in scopedObject.GetType().GetInterfaces())
            {
                mockServicesDict.TryAdd(interfaceType, scopedObject);
            }
        }

        var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => Attribute.IsDefined(prop, typeof(PluginServiceAttribute)) && prop.CanWrite && prop.CanRead);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (mockServicesDict.TryGetValue(propertyType, out var mockService))
            {
                property.SetValue(instance, mockService);
            }
            else
            {
                logger.Error($"No matching mock service found for property '{property.Name}' of type '{propertyType.FullName}' in object of type '{instance.GetType().FullName}'.");
            }
        }

        return true;
    }

    internal static string SerializeConfig(object? config)
    {
        return JsonConvert.SerializeObject(
            config,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
    }

    public bool OpenPluginInstaller()
    {
        return false;
    }

    public void Dispose()
    {
    }

    public virtual void OnLanguageChanged(string langcode)
    {
        this.LanguageChanged?.Invoke(langcode);
    }

    public virtual void OnActivePluginsChanged(PluginListInvalidationKind kind, bool affectedthisplugin)
    {
        this.ActivePluginsChanged?.Invoke(kind, affectedthisplugin);
    }
}