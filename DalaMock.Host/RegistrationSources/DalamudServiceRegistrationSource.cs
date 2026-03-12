using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Builder;

using Dalamud.IoC;
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

        if (!typeof(IDalamudService).IsAssignableFrom(type))
        {
            yield break;
        }

        var rb = RegistrationBuilder
                 .ForDelegate(type, (ctx, p) =>
                 {
                     var wrapperType = typeof(DalamudServiceWrapper<>).MakeGenericType(type);

                     var wrapper = Activator.CreateInstance(
                         wrapperType,
                         this.pluginInterface);
                     var prop = wrapperType.GetProperty("Service")!;
                     return prop.GetValue(wrapper)!;
                 })
                 .SingleInstance()
                 .ExternallyOwned()
                 .CreateRegistration();

        yield return rb;
    }
}
