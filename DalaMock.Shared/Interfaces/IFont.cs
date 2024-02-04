using ImGuiNET;

namespace DalaMock.Shared.Interfaces;

public interface IFont
{
    public ImFontPtr DefaultFont { get; }
    public ImFontPtr IconFont { get; }
    public ImFontPtr MonoFont { get; }
}