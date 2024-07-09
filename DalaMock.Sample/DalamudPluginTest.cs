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
        ICommandManager commandManager,
        ITextureProvider textureProvider) : base(pluginInterface, pluginLog)
    {
        this.CreateHost();
        this.Start();
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        // Replace your interfaces/implementations here
    }

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Register any services here
    }
}