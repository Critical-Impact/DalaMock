namespace DalaMock.Core.Windows;

using DalaMock.Shared.Classes;

public class MockWindowSystem : DalamudWindowSystem
{
    public MockWindowSystem(string? imNamespace = null)
        : base(imNamespace)
    {
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        foreach (var window in this.Windows)
        {
            window.AllowClickthrough = false;
            window.AllowPinning = false;
        }

        base.Draw();
    }
}
