namespace DalaMock.Core.Mocks;

using System;

using DalaMock.Shared.Interfaces;

using Dalamud.Bindings.ImGui;

public class MockFont : IFont, IMockService
{
    private ImFontPtr defaultFont = null;
    private ImFontPtr monoFont = null;
    private ImFontPtr iconFont = null;

    /// <inheritdoc/>
    public unsafe ImFontPtr DefaultFont
    {
        get
        {
            if ((IntPtr)this.iconFont.Handle == IntPtr.Zero)
            {
                this.defaultFont = ImGui.GetIO().Fonts.Fonts[1];
            }

            return this.defaultFont;
        }
    }

    /// <inheritdoc/>
    public unsafe ImFontPtr IconFont
    {
        get
        {
            if ((IntPtr)this.iconFont.Handle == IntPtr.Zero)
            {
                this.iconFont = ImGui.GetIO().Fonts.Fonts[3];
            }

            return this.iconFont;
        }
    }

    /// <inheritdoc/>
    public unsafe ImFontPtr MonoFont
    {
        get
        {
            if ((IntPtr)this.monoFont.Handle == IntPtr.Zero)
            {
                this.monoFont = ImGui.GetIO().Fonts.Fonts[2];
            }

            return this.monoFont;
        }
    }

    public string ServiceName => "Font";
}
