namespace SamplePlugin;

using Autofac;
using DalaMock.Core.Mocks;
using DalaMock.Core.Windows;
using DalaMock.Shared.Interfaces;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

public class MockPlugin : Plugin
{
    private readonly ILogger<MockPlugin> logger;

    public MockPlugin(IDalamudPluginInterface pluginInterface, ILogger<MockPlugin> logger, IPluginLog pluginLog, IFramework framework, ICommandManager commandManager, IDataManager dataManager, ITextureProvider textureProvider, IChatGui chatGui, IDtrBar dtrBar)
        : base(pluginInterface, pluginLog, framework, commandManager, dataManager, textureProvider, chatGui, dtrBar)
    {
        this.logger = logger;
        this.logger.LogInformation("Plugin mock started");
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        base.ConfigureContainer(containerBuilder);
        containerBuilder.RegisterType<MockWindowSystem>().As<IWindowSystem>();
        containerBuilder.RegisterType<MockFileDialogManager>().As<IFileDialogManager>();
        containerBuilder.RegisterType<MockFont>().As<IFont>().SingleInstance();
    }
}
