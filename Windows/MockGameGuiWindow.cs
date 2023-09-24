using DalaMock.Dalamud;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;

namespace DalaMock.Windows;

public class MockGameGuiWindow : Window
{
    private readonly MockGameGui _mockGameGui;

    public MockGameGuiWindow(MockGameGui mockGameGui, string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow)
    {
        _mockGameGui = mockGameGui;
    }

    private int _hoveredItemId;
    private bool _hoveredItemIsHq;
    public override void Draw()
    {
        var hoveredItemId = _hoveredItemId;
        var hoveredItemIsHq = _hoveredItemIsHq;
        ImGui.InputInt("Hovered Item ID: ", ref hoveredItemId);
        ImGui.Checkbox("Hovered Item Is HQ?: ", ref hoveredItemIsHq);
        if (hoveredItemId != _hoveredItemId || hoveredItemIsHq != _hoveredItemIsHq)
        {
            _hoveredItemId = hoveredItemId;
            _hoveredItemIsHq = hoveredItemIsHq;
            _mockGameGui.SetHoveredItem((uint)_hoveredItemId, _hoveredItemIsHq ? InventoryItem.ItemFlags.HQ : InventoryItem.ItemFlags.None);
        }
    }
}