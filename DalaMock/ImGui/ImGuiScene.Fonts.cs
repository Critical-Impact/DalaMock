// <copyright file="ImGuiScene.Fonts.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace DalaMock.Core.Imgui;

public partial class ImGuiScene
{
    public unsafe ImFontPtr LoadFont(string fontPath, float fontSize, int fontNo = 0, ushort[]? iconRanges = null)
    {
        ImGuiIOPtr io = ImGui.GetIO();

        GCHandle iconRangeHandle = GCHandle.Alloc(iconRanges, GCHandleType.Pinned);
        IntPtr iconRangePtr = iconRangeHandle.AddrOfPinnedObject();

        // Create a new font configuration
        ImFontConfigPtr fontConfig = ImGuiNative.ImFontConfig_ImFontConfig();
        fontConfig.FontNo = fontNo;
        fontConfig.GlyphRanges = iconRangePtr;

        ImFontPtr newFont = io.Fonts.AddFontFromFileTTF(fontPath, fontSize, fontConfig, iconRangePtr);
        ImGui.GetIO().Fonts.Build();

        this.RecreateFontDeviceTexture(this.GraphicsDevice);

        ImGuiNative.ImFontConfig_destroy(fontConfig);
        if (iconRangeHandle.IsAllocated)
        {
            iconRangeHandle.Free();
        }

        return newFont;
    }
}