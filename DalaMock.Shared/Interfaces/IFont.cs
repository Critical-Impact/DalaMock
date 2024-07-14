﻿namespace DalaMock.Shared.Interfaces;

using ImGuiNET;

/// <summary>
/// A interface that provides the default fonts plugins can use.
/// </summary>
public interface IFont
{
    /// <summary>
    /// Gets dalamud's default font.
    /// </summary>
    public ImFontPtr DefaultFont { get; }

    /// <summary>
    /// Gets dalamud's icon font.
    /// </summary>
    public ImFontPtr IconFont { get; }

    /// <summary>
    /// Gets dalamud's mon font.
    /// </summary>
    public ImFontPtr MonoFont { get; }
}