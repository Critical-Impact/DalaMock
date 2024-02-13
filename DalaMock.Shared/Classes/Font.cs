using DalaMock.Shared.Interfaces;
using Dalamud.Interface;
using ImGuiNET;

namespace DalaMock.Shared.Classes;

public class Font : IFont
{
    public ImFontPtr DefaultFont => UiBuilder.DefaultFont;
    public ImFontPtr IconFont => UiBuilder.IconFont;
    public ImFontPtr MonoFont => UiBuilder.MonoFont;
}