using DalaMock.Shared.Interfaces;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace DalaMock.Sample;

public class SampleWindow(IDataManager dataManager, IChatGui chatGui, IFont font) : Window("Sample Window")
{
    public override void Draw()
    {
        ImGui.Text("A sample window");
        var gilItem = dataManager.GetExcelSheet<Item>()!.GetRow(1)!;
        ImGui.Text(gilItem.Description);
        using (var iconFont = ImRaii.PushFont(font.IconFont))
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }

        if (ImGui.Button("Fire chat message"))
        {
            chatGui.Print("Hello from sample plugin.");
        }
    }
}
