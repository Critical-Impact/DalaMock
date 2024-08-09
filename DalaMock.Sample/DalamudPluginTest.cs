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
        containerBuilder.RegisterType<WindowService>().SingleInstance();
        containerBuilder.RegisterType<ConfigurationSaveService>().SingleInstance();
        containerBuilder.RegisterType<SampleWindow>().SingleInstance();
        containerBuilder.RegisterType<DtrBarSampleWindow>().SingleInstance();
        containerBuilder.RegisterType<Font>().As<IFont>().SingleInstance();
    }

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService(c => c.GetRequiredService<WindowService>());
        serviceCollection.AddHostedService(c => c.GetRequiredService<ConfigurationSaveService>());
    }
}
