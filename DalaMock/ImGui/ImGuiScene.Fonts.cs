namespace DalaMock.Core.Imgui;

using System;
using System.Runtime.InteropServices;

using Dalamud.Bindings.ImGui;

public partial class ImGuiScene
{
    public unsafe ImFontPtr LoadFont(string fontPath, float fontSize, int fontNo = 0, ushort[]? iconRanges = null)
    {
        ImGuiIOPtr io = ImGui.GetIO();

        GCHandle iconRangeHandle = GCHandle.Alloc(iconRanges, GCHandleType.Pinned);
        IntPtr iconRangePtr = iconRangeHandle.AddrOfPinnedObject();

        ImFontConfigPtr fontConfig = ImGui.ImFontConfig();
        fontConfig.FontNo = fontNo;
        fontConfig.GlyphRanges = (ushort*)iconRangePtr;

        ImFontPtr newFont = io.Fonts.AddFontFromFileTTF(fontPath, fontSize, fontConfig.Handle, (ushort*)iconRangePtr);
        ImGui.GetIO().Fonts.Build();

        this.RecreateFontDeviceTexture(this.GraphicsDevice);

        fontConfig.Destroy();
        if (iconRangeHandle.IsAllocated)
        {
            iconRangeHandle.Free();
        }

        return newFont;
    }
}
