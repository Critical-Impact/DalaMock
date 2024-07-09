namespace DalaMock.Core.Windows;

using Dalamud.Interface.Windowing;

public class MockWindowSystem : WindowSystem
{
    public MockWindowSystem(string imNamespace = "DalaMock")
        : base(imNamespace)
    {
    }

    /// <inheritdoc cref="WindowSystem.Draw" />
    public new void Draw()
    {
        foreach (var window in this.Windows)
        {
            window.AllowClickthrough = false;
            window.AllowPinning = false;
        }

        base.Draw();
    }
}