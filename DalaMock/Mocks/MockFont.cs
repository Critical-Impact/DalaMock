namespace DalaMock.Core.Mocks;

using DalaMock.Shared.Interfaces;
using ImGuiNET;

public class MockFont : IFont, IMockService
{
    /// <inheritdoc/>
    public ImFontPtr DefaultFont => ImGui.GetIO().FontDefault;

    /// <inheritdoc/>
    public ImFontPtr IconFont => ImGui.GetIO().FontDefault;

    /// <inheritdoc/>
    public ImFontPtr MonoFont => ImGui.GetIO().FontDefault;

    public string ServiceName => "Font";
}