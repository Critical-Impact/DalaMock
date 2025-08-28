namespace DalaMock.Shared.Classes;

using System.Collections.Generic;

using DalaMock.Shared.Interfaces;

using Dalamud.Interface.Windowing;

public class DalamudWindowSystem : IWindowSystem
{
    public DalamudWindowSystem(string? imNamespace = null)
    {
        this.WindowSystem = new WindowSystem(imNamespace);
    }

    /// <summary>
    /// Gets the dalamud window system.
    /// </summary>
    public WindowSystem WindowSystem { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyList<Window> Windows => this.WindowSystem.Windows;

    /// <inheritdoc/>
    public bool HasAnyFocus => this.WindowSystem.HasAnyFocus;

    /// <inheritdoc/>
    public string? Namespace
    {
        get => this.WindowSystem.Namespace;
        set => this.WindowSystem.Namespace = value;
    }

    /// <inheritdoc/>
    public void AddWindow(Window window)
    {
        this.WindowSystem.AddWindow(window);
    }

    /// <inheritdoc/>
    public void RemoveWindow(Window window)
    {
        this.WindowSystem.RemoveWindow(window);
    }

    /// <inheritdoc/>
    public void RemoveAllWindows()
    {
        this.WindowSystem.RemoveAllWindows();
    }

    /// <inheritdoc/>
    public virtual void Draw()
    {
        this.WindowSystem.Draw();
    }
}
