namespace DalaMock.Sample.Mock;

using Autofac;

using DalaMock.Core.Mocks;
using DalaMock.Core.Windows;
using DalaMock.Shared.Interfaces;

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using Microsoft.Extensions.Logging;

public class DalamudMockPluginTest : DalamudPluginTest
{
    public DalamudMockPluginTest(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
        : base(pluginInterface, pluginLog)
    {
        pluginLog.Info("Plugin mock started");
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        base.ConfigureContainer(containerBuilder);
        containerBuilder.RegisterType<MockWindowSystem>().As<IWindowSystem>();
        containerBuilder.RegisterType<MockFileDialogManager>().As<IFileDialogManager>();
        containerBuilder.RegisterType<MockFont>().As<IFont>().SingleInstance();
    }
}
