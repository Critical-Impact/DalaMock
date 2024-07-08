namespace DalaMock.Core.Windows;

using Dalamud.Interface.Windowing;

public class MockWindowSystem : WindowSystem
{
    public MockWindowSystem(string imNamespace = "DalaMock") : base(imNamespace)
    {
    }
}