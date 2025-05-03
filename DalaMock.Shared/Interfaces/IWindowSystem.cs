namespace DalaMock.Shared.Interfaces;

using System.Collections.Generic;

using Dalamud.Interface.Windowing;

/// <summary>
/// A interface implementing a window system.
/// </summary>
public interface IWindowSystem
{
    /// <summary>
    /// Gets a read-only list of all <see cref="Window"/>s in this <see cref="WindowSystem"/>.
    /// </summary>
    IReadOnlyList<Window> Windows { get; }

    /// <summary>
    /// Gets a value indicating whether any window in this <see cref="WindowSystem"/> has focus and is
    /// not marked to be excluded from consideration.
    /// </summary>
    bool HasAnyFocus { get; }

    /// <summary>
    /// Gets or sets the name/ID-space of this <see cref="WindowSystem"/>.
    /// </summary>
    string? Namespace { get; set; }

    /// <summary>
    /// Add a window to this <see cref="WindowSystem"/>.
    /// The window system doesn't own your window, it just renders it
    /// You need to store a reference to it to use it later.
    /// </summary>
    /// <param name="window">The window to add.</param>
    void AddWindow(Window window);

    /// <summary>
    /// Remove a window from this <see cref="WindowSystem"/>.
    /// Will not dispose your window, if it is disposable.
    /// </summary>
    /// <param name="window">The window to remove.</param>
    void RemoveWindow(Window window);

    /// <summary>
    /// Remove all windows from this <see cref="WindowSystem"/>.
    /// Will not dispose your windows, if they are disposable.
    /// </summary>
    void RemoveAllWindows();

    /// <summary>
    /// Draw all registered windows using ImGui.
    /// </summary>
    void Draw();
}
