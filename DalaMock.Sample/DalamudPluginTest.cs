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
        ITextureProvider textureProvider)
        : base(pluginInterface, pluginLog, dataManager, textureProvider)
    {
        this.CreateHost();
        this.Start();
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<WindowService>().SingleInstance();
        containerBuilder.RegisterType<SampleWindow>().SingleInstance();
    }

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService(c => c.GetRequiredService<WindowService>());
    }
}