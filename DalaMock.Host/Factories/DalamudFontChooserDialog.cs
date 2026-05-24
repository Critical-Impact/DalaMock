namespace DalaMock.Host.Factories;

using System;
using System.Numerics;
using System.Threading.Tasks;

using DalaMock.Shared.Interfaces;

using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ImGuiFontChooserDialog;

/// <summary>
/// Wraps Dalamud's real <see cref="SingleFontChooserDialog"/> as an <see cref="IFontChooserDialog"/>.
/// </summary>
public class DalamudFontChooserDialog : IFontChooserDialog
{
    private readonly SingleFontChooserDialog inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="DalamudFontChooserDialog"/> class.
    /// </summary>
    /// <param name="inner">The underlying Dalamud font chooser dialog.</param>
    public DalamudFontChooserDialog(SingleFontChooserDialog inner)
    {
        this.inner = inner;
    }

    /// <inheritdoc/>
    public event Action<SingleFontSpec>? SelectedFontSpecChanged
    {
        add => this.inner.SelectedFontSpecChanged += value;
        remove => this.inner.SelectedFontSpecChanged -= value;
    }

    /// <inheritdoc/>
    public Task<SingleFontSpec> ResultTask => this.inner.ResultTask;

    /// <inheritdoc/>
    public string Title
    {
        get => this.inner.Title;
        set => this.inner.Title = value;
    }

    /// <inheritdoc/>
    public string PreviewText
    {
        get => this.inner.PreviewText;
        set => this.inner.PreviewText = value;
    }

    /// <inheritdoc/>
    public SingleFontSpec SelectedFont
    {
        get => this.inner.SelectedFont;
        set => this.inner.SelectedFont = value;
    }

    /// <inheritdoc/>
    public Predicate<IFontFamilyId>? FontFamilyExcludeFilter
    {
        get => this.inner.FontFamilyExcludeFilter;
        set => this.inner.FontFamilyExcludeFilter = value;
    }

    /// <inheritdoc/>
    public void Draw() => this.inner.Draw();

    /// <inheritdoc/>
    public void Cancel() => this.inner.Cancel();

    /// <inheritdoc/>
    public void SetPopupPositionAndSizeToCurrentWindowCenter(Vector2 preferredPopupSize) =>
        this.inner.SetPopupPositionAndSizeToCurrentWindowCenter(preferredPopupSize);

    /// <inheritdoc/>
    public void SetPopupPositionAndSizeToCurrentWindowCenter() =>
        this.inner.SetPopupPositionAndSizeToCurrentWindowCenter();

    /// <inheritdoc/>
    public void Dispose() => this.inner.Dispose();
}
