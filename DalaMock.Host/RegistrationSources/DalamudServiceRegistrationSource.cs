using System;
using System.Collections.Generic;

using Autofac.Builder;
using Autofac.Core;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

public sealed class DalamudServiceRegistrationSource : IRegistrationSource
{
    private readonly IDalamudPluginInterface pluginInterface;

    public DalamudServiceRegistrationSource(IDalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public bool IsAdapterForIndividualComponents => false;

    public IEnumerable<IComponentRegistration> RegistrationsFor(
        Service service,
        Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
    {
        if (service is not TypedService typedService)
        {
            yield break;
        }

        var type = typedService.ServiceType;

        if (!typeof(IDalamudService).IsAssignableFrom(type) || type.GetInterface("IMockService") != null)
        {
            yield break;
        }

        var wrapperType = typeof(DalamudServiceWrapper<>).MakeGenericType(type);

        var wrapper = Activator.CreateInstance(
            wrapperType,
            this.pluginInterface);
        var prop = wrapperType.GetProperty("Service")!;
        var value = prop.GetValue(wrapper)!;

        if (value == null!)
        {
            yield break;
        }

        var rb = RegistrationBuilder
                 .ForDelegate((_, _) => value)
                 .SingleInstance()
                 .As(value.GetType())
                 .As(type)
                 .ExternallyOwned()
                 .CreateRegistration();

        yield return rb;
    }
}
