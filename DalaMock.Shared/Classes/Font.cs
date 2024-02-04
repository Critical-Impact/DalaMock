using DalaMock.Shared.Interfaces;
using Dalamud.Interface;
using ImGuiNET;

namespace DalaMock.Shared.Classes;

public class Font : IFont
{
    public ImFontPtr DefaultFont { get; } = UiBuilder.DefaultFont;
    public ImFontPtr IconFont { get; } = UiBuilder.IconFont;
    public ImFontPtr MonoFont { get; } = UiBuilder.MonoFont;
}