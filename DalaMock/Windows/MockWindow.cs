namespace DalaMock.Core.Windows;

using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mocks;

public abstract class MockWindow<T> : Window, IMockWindow where T : IMockService
{
    protected MockWindow(
        T mockService,
        string name,
        ImGuiWindowFlags flags = ImGuiWindowFlags.None,
        bool forceMainWindow = false) : base(name, flags, forceMainWindow)
    {
        this.MockService = mockService;
    }

    public IMockService MockService { get; }
}

public interface IMockWindow
{
    public IMockService MockService { get; }

    public void Toggle();

    public void Draw();
}