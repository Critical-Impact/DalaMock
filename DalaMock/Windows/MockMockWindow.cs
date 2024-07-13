using System.Collections.Generic;
using Dalamud.Interface.Utility.Raii;

namespace DalaMock.Core.Windows;

using Dalamud.Interface.Windowing;
using ImGuiNET;

public class MockMockWindow : Window
{
    private readonly IEnumerable<IMockWindow> mockWindows;

    public MockMockWindow(IEnumerable<IMockWindow> mockWindows)
        : base("Service Mocks")
    {
        this.mockWindows = mockWindows;
        this.IsOpen = false;
    }

    public override void Draw()
    {
        using var tabBar = ImRaii.TabBar("Mocks");
        if (tabBar)
        {
            foreach (var mockWindow in this.mockWindows)
            {
                using (var tabItem = ImRaii.TabItem(mockWindow.MockService.ServiceName))
                {
                    if (tabItem)
                    {
                        mockWindow.Draw();
                    }
                }
            }
        }
    }
}