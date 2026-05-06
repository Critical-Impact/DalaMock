using Autofac;

using Dalamud.Interface.Windowing;

namespace DalaMock.Host.Hosting;

using DalaMock.Host.Factories;
using DalaMock.Shared.Classes;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface.ImGuiFileDialog;

/// <inheritdoc />
public class DalamudReplacementContainer : IReplacementContainer
{
    public DalamudReplacementContainer()
    {
        this.ImGuiComponents = new DalamudImGuiComponents();
        this.WindowSystemFactory = new WindowSystemFactory();
        this.Font = new Font();
        this.FileDialogManager = new DalamudFileDialogManager(new FileDialogManager());
    }

    public IImGuiComponents ImGuiComponents { get; }

    public IWindowSystemFactory WindowSystemFactory { get; }

    public IFont Font { get; }

    public IFileDialogManager FileDialogManager { get; }

    /// <inheritdoc/>
    public void Register(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterInstance(this.ImGuiComponents).AsImplementedInterfaces().AsSelf().SingleInstance();
        containerBuilder.RegisterInstance(this.WindowSystemFactory).AsImplementedInterfaces().AsSelf().SingleInstance();
        containerBuilder.RegisterInstance(this.Font).AsImplementedInterfaces().AsSelf().SingleInstance();
        containerBuilder.RegisterInstance(this.FileDialogManager).AsImplementedInterfaces().AsSelf().SingleInstance();
        containerBuilder.RegisterType<WindowSystem>().As<IWindowSystem>();
    }
}
