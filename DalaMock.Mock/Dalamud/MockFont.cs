using DalaMock.Shared.Interfaces;
using ImGuiNET;

namespace DalaMock.Dalamud;

public class MockFont : IFont
{
    public ImFontPtr DefaultFont { get; } = null;
    public ImFontPtr IconFont { get; } = null;
    public ImFontPtr MonoFont { get; } = null;
}