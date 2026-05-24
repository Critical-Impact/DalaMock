namespace DalaMock.Host.Factories;

using DalaMock.Shared.Interfaces;

using Dalamud.Interface;
using Dalamud.Interface.ImGuiFontChooserDialog;

/// <summary>
/// Creates font chooser dialogs backed by Dalamud's real <see cref="SingleFontChooserDialog"/>,
/// using the active plugin's <see cref="UiBuilder"/>.
/// </summary>
public class DalamudFontChooserFactory : IFontChooserFactory
{
    private readonly IUiBuilder uiBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DalamudFontChooserFactory"/> class.
    /// </summary>
    /// <param name="uiBuilder">The plugin's UI builder. Must be the concrete Dalamud <see cref="UiBuilder"/>.</param>
    public DalamudFontChooserFactory(IUiBuilder uiBuilder)
    {
        this.uiBuilder = uiBuilder;
    }

    /// <inheritdoc/>
    public IFontChooserDialog Create() =>
        new DalamudFontChooserDialog(new SingleFontChooserDialog((UiBuilder)this.uiBuilder));

    /// <inheritdoc/>
    public IFontChooserDialog CreateAuto() =>
        new DalamudFontChooserDialog(SingleFontChooserDialog.CreateAuto((UiBuilder)this.uiBuilder));
}
