namespace DalaMock.Sample;

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Autofac;
using DalaMock.Host.Hosting;
using DalaMock.Sample.Services;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;

public class DalamudPluginTest : HostedPlugin
{
    private readonly IPluginLog pluginLog;

    public DalamudPluginTest(
        IDalamudPluginInterface pluginInterface,
        IPluginLog pluginLog)
        : base(pluginInterface)
    {
        this.pluginLog = pluginLog;
    }

    /// <summary>
    /// Configures the optional services to register automatically for use in your plugin.
    /// </summary>
    /// <returns>A HostedPluginOptions configured with the options you require.</returns>
    public override HostedPluginOptions ConfigureOptions()
    {
        return new HostedPluginOptions()
        {
            UseMediatorService = true,
        };
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        // While you can register services in the service collection, as long as you register a service as IHostedService(the AsImplementedInterfaces call) it will automatically be picked up by the host. This also avoids potential double registrations.
        containerBuilder.RegisterType<WindowService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        containerBuilder.RegisterType<ConfigurationService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        containerBuilder.RegisterType<CommandService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        containerBuilder.RegisterType<InstallerWindowService>().AsSelf().AsImplementedInterfaces().SingleInstance();

        // Register every class ending in Window inside our assembly with the container
        containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                        .Where(t => t.Name.EndsWith("Window"))
                        .As<Window>()
                        .AsSelf()
                        .AsImplementedInterfaces();

        // Register the configuration with the container so that it's loaded/created when requested.
        containerBuilder.Register(s =>
        {
            var configurationLoaderService = s.Resolve<ConfigurationService>();
            return configurationLoaderService.GetConfiguration();
        }).SingleInstance();
    }

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
    }

    public override async Task StartingAsync(CancellationToken cancellationToken)
    {
        this.pluginLog.Verbose("Plugin starting!");
    }

    public override Task StartedAsync(CancellationToken cancellationToken)
    {
        this.pluginLog.Verbose("Plugin started!");
        return Task.CompletedTask;
    }

    public override Task StoppingAsync()
    {
        this.pluginLog.Verbose("Plugin stopping!");
        return Task.CompletedTask;
    }

    public override Task StoppedAsync()
    {
        this.pluginLog.Verbose("Plugin stopped!");
        return Task.CompletedTask;
    }
}
