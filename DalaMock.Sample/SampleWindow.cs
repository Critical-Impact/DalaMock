using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace DalaMock.Sample;

public class SampleWindow(IDataManager dataManager) : Window("Sample Window")
{
    public override void Draw()
    {
        ImGui.Text("A sample window");
        var gilItem = dataManager.GetExcelSheet<Item>()!.GetRow(1)!;
        ImGui.Text(gilItem.Description);
    }
}