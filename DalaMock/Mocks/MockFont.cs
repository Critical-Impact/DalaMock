﻿using System;

namespace DalaMock.Core.Mocks;

using DalaMock.Shared.Interfaces;
using ImGuiNET;

public class MockFont : IFont, IMockService
{
    private ImFontPtr iconFont = null;

    /// <inheritdoc/>
    public ImFontPtr DefaultFont => ImGui.GetIO().FontDefault;

    /// <inheritdoc/>
    public unsafe ImFontPtr IconFont
    {
        get
        {
            if ((IntPtr)this.iconFont.NativePtr == IntPtr.Zero)
            {
                this.iconFont = ImGui.GetIO().Fonts.Fonts[1];
            }

            return this.iconFont;
        }
    }

    /// <inheritdoc/>
    public ImFontPtr MonoFont => ImGui.GetIO().FontDefault;

    public string ServiceName => "Font";
}