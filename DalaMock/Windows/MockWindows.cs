namespace DalaMock.Core.Windows;

using System.Collections.Generic;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Mocks;

public class MockWindows : Window
{
    private readonly MockUiBuilder mockUiBuilder;
    private readonly IEnumerable<IMockWindow> mockWindows;

    public MockWindows(
        MockUiBuilder uiBuilder,
        IEnumerable<IMockWindow> mockWindows,
        string name = "Mock Windows",
        ImGuiWindowFlags flags = ImGuiWindowFlags.None,
        bool forceMainWindow = false) : base(name, flags, forceMainWindow)
    {
        this.mockUiBuilder = uiBuilder;
        this.mockWindows = mockWindows;
    }

    public override void Draw()
    {
        if (ImGui.Button("Open Main Window"))
        {
            this.mockUiBuilder.FireOpenMainUiEvent();
        }

        if (ImGui.Button("Open Config Window"))
        {
            this.mockUiBuilder.FireOpenConfigUiEvent();
        }

        foreach (var mockWindow in this.mockWindows)
        {
            if (ImGui.Button(mockWindow.MockService.ServiceName))
            {
                mockWindow.Toggle();
            }
        }
    }
}