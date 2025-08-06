namespace DalaMock.Sample.Windows;

using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

public class ConfigWindow(Configuration config) : Window("SamplePlugin Config")
{
    public override void Draw()
    {
        var configOption = config.ConfigOption;

        if (ImGui.Checkbox("Config Option", ref configOption))
        {
            config.ConfigOption = configOption;
        }
    }
}
