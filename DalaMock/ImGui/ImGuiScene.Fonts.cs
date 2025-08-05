// <copyright file="ImGuiScene.Fonts.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Dalamud.Bindings.ImGui;

namespace DalaMock.Core.Imgui;

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
