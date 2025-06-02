using DalaMock.Shared.Classes;
using DalaMock.Shared.Interfaces;

namespace DalaMock.Sample;

using Autofac;
using DalaMock.Host.Factories;
using DalaMock.Host.Hosting;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Host;
using Microsoft.Extensions.DependencyInjection;

public class DalamudPluginTest : HostedPlugin
{
    public DalamudPluginTest(
        IDalamudPluginInterface pluginInterface,
        IPluginLog pluginLog,
        IDataManager dataManager,
        ITextureProvider textureProvider,
        IChatGui chatGui, IDtrBar dtrBar)
        : base(pluginInterface, pluginLog, dataManager, textureProvider, chatGui, dtrBar)
    {
        this.CreateHost();
        this.Start();
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        // While you can register services in the service collection, as long as you register a service as IHostedService(the AsImplementedInterfaces call) it will automatically be picked up by the host. This also avoids potential double registrations.
        containerBuilder.RegisterType<WindowService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        containerBuilder.RegisterType<ConfigurationSaveService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        containerBuilder.RegisterType<SampleWindow>().SingleInstance();
        containerBuilder.RegisterType<DtrBarSampleWindow>().SingleInstance();
        containerBuilder.RegisterType<Font>().As<IFont>().SingleInstance();
    }


    public override void ConfigureServices(IServiceCollection serviceCollection)
    {

    }
}
