using Dalamud.Game.Gui.Dtr;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;

using ImGuiNET;

namespace DalaMock.Sample;

public class DtrBarSampleWindow(IDtrBar dtrBar) : Window("Dtr Bar Window")
{
    private IDtrBarEntry? sampleEntry;

    public override void Draw()
    {
        ImGui.Text("Current DTR Bar Entries:");
        ImGui.Separator();
        foreach (var dtrBarEntry in dtrBar.Entries)
        {
            ImGui.Text(dtrBarEntry.Title);
        }

        if (ImGui.Button("Add Sample DTR Bar"))
        {
            if (this.sampleEntry == null)
            {
                this.sampleEntry = dtrBar.Get("Sample DTR Bar Entry");
            }
        }

        if (ImGui.Button("Remove Sample DTR Bar"))
        {
            if (this.sampleEntry != null)
            {
                dtrBar.Remove("Sample DTR Bar Entry");
                this.sampleEntry = null;
            }
        }
    }
}
