namespace DalaMock.Core.Windows;

using Dalamud.Interface.Windowing;
using ImGuiNET;

public class MockMockWindow() : Window("Service Mocks")
{
    public override void Draw()
    {
        ImGui.Text("Sample Plugin Window");
    }
}