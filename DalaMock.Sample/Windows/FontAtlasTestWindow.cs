using System;

using Dalamud.Game.Text;

namespace DalaMock.Sample.Windows;

using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.ManagedFontAtlas;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

/// <summary>
/// Exercises the new <see cref="IUiBuilder.CreateFontAtlas"/> path on <c>MockUiBuilder</c> plus the
/// four pre-built handles (<see cref="IUiBuilder.DefaultFontHandle"/>, etc.).
/// </summary>
public class FontAtlasTestWindow : Window, IDisposable
{
    private readonly IUiBuilder uiBuilder;
    private readonly IFontAtlas customAtlas;
    private readonly IFontHandle bigHandle;
    private readonly IFontHandle biggerHandle;
    private float scaleSlider = 1f;

    public FontAtlasTestWindow(IDalamudPluginInterface pluginInterface)
        : base("Font Atlas Test")
    {
        this.uiBuilder = pluginInterface.UiBuilder;

        this.customAtlas = this.uiBuilder.CreateFontAtlas(
            FontAtlasAutoRebuildMode.OnNewFrame,
            isGlobalScaled: true,
            debugName: "FontAtlasTestWindow");

        this.bigHandle = this.customAtlas.NewDelegateFontHandle(e =>
            e.OnPreBuild(tk => tk.Font = tk.AddDalamudDefaultFont(36f)));

        this.biggerHandle = this.customAtlas.NewDelegateFontHandle(e =>
            e.OnPreBuild(tk => tk.Font = tk.AddDalamudDefaultFont(50f)));
    }

    public override void Draw()
    {
        ImGui.TextUnformatted("Pre-built handles on UiBuilder:");

        using (this.uiBuilder.DefaultFontHandle.Push())
        {
            ImGui.Text("DefaultFontHandle — Hello, world!");
        }

        using (this.uiBuilder.MonoFontHandle.Push())
        {
            ImGui.Text("MonoFontHandle — 0123456789");
        }

        using (this.uiBuilder.IconFontHandle.Push())
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }

        using (this.uiBuilder.IconFontFixedWidthHandle.Push())
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }

        ImGui.Separator();
        ImGui.TextUnformatted($"Custom atlas '{this.customAtlas.Name}': Available={this.bigHandle.Available}");

        if (this.bigHandle.Available)
        {
            using (this.bigHandle.Push())
            {
                ImGui.Text("Custom 36px font from CreateFontAtlas");
            }
        }
        else
        {
            ImGui.TextUnformatted("(custom atlas still building...)");
        }

        ImGui.Separator();
        ImGui.TextUnformatted($"Custom atlas '{this.customAtlas.Name}': Available={this.biggerHandle.Available}");

        if (this.biggerHandle.Available)
        {
            using (this.biggerHandle.Push())
            {
                ImGui.Text("Custom 50px font from CreateFontAtlas");
            }
        }
        else
        {
            ImGui.TextUnformatted("(custom atlas still building...)");
        }
    }

    public void Dispose()
    {
        this.bigHandle.Dispose();
        this.biggerHandle.Dispose();
        this.customAtlas.Dispose();
    }
}
