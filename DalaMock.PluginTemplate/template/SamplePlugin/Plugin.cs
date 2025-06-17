namespace SamplePlugin;

using System.Reflection;

using Autofac;
using DalaMock.Host.Hosting;
using SamplePlugin.Services;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;

public class Plugin : HostedPlugin
{
    public Plugin(
        IDalamudPluginInterface pluginInterface,
        IPluginLog pluginLog,
        IFramework framework,
        ICommandManager commandManager,
        IDataManager dataManager,
        ITextureProvider textureProvider,
        IChatGui chatGui,
        IDtrBar dtrBar)
        : base(pluginInterface, pluginLog, framework, commandManager, dataManager, textureProvider, chatGui, dtrBar)
    {
        this.CreateHost();
        this.Start();
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
}
