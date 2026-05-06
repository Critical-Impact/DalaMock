namespace DalaMock.Core.Mocks;

/// <inheritdoc />
public class MockReplacementContainer : IReplacementContainer
{
    public MockReplacementContainer(IUiBuilder uiBuilder, MockWindowSystem.Factory factory)
    {
        this.ImGuiComponents = new MockImGuiComponents(uiBuilder);
        this.WindowSystemFactory = new MockWindowSystemFactory(factory);
        this.Font = new MockFont();
        this.FileDialogManager = new MockFileDialogManager();
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
        containerBuilder.RegisterType<MockWindowSystem>().As<IWindowSystem>().InstancePerDependency();
    }
}
