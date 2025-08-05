namespace DalaMock.Core.Windows;

using System;
using System.Collections.Generic;
using System.Linq;

using DalaMock.Shared.Interfaces;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

/// <summary>
/// A mock window system that simulates Dalamud's window system.
/// </summary>
public class MockWindowSystem : IWindowSystem
{
    private readonly Dictionary<Window, WindowState> windowStates = new();

    private readonly List<Window> windows = new();

    private string lastFocusedWindowName = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockWindowSystem"/> class.
    /// A mock window system that implements roughly the same functionality as the real Dalamud window system.
    /// </summary>
    /// <param name="imNamespace">The namespace of the window system.</param>
    public MockWindowSystem(string? imNamespace)
    {
        this.Namespace = imNamespace;
    }

    /// <inheritdoc/>
    public IReadOnlyList<Window> Windows => this.windows;

    /// <inheritdoc/>
    public bool HasAnyFocus { get; set; }

    /// <inheritdoc/>
    public string? Namespace { get; set; }

    /// <inheritdoc/>
    public void AddWindow(Window window)
    {
        if (this.windows.Any(w => w.WindowName == window.WindowName))
        {
            throw new ArgumentException("A window with this name already exists.");
        }

        this.windows.Add(window);
    }

    /// <inheritdoc/>
    public void RemoveWindow(Window window)
    {
        if (!this.windows.Contains(window))
        {
            throw new ArgumentException("Window not found in this WindowSystem.");
        }

        this.windows.Remove(window);
    }

    /// <inheritdoc/>
    public void RemoveAllWindows()
    {
        this.windows.Clear();
    }

    /// <inheritdoc/>
    public void Draw()
    {
        var hasNamespace = !string.IsNullOrEmpty(this.Namespace);

        if (hasNamespace)
        {
            ImGui.PushID(this.Namespace);
        }

        foreach (var window in this.windows.ToArray())
        {
            this.DrawWindow(window);
        }

        var focusedWindow = this.windows.FirstOrDefault(window => window.IsFocused && window.RespectCloseHotkey);
        this.HasAnyFocus = focusedWindow != default;

        if (this.HasAnyFocus)
        {
            if (focusedWindow != null && this.lastFocusedWindowName != focusedWindow.WindowName)
            {
                this.lastFocusedWindowName = focusedWindow.WindowName;
            }
        }
        else
        {
            this.lastFocusedWindowName = string.Empty;
        }

        if (hasNamespace)
        {
            ImGui.PopID();
        }
    }

    private WindowState GetState(Window window)
    {
        if (!this.windowStates.TryGetValue(window, out var state))
        {
            state = new WindowState();
            this.windowStates[window] = state;
        }

        return state;
    }

    private void ApplyConditionals(Window window)
    {
        if (window.Position.HasValue)
        {
            var pos = window.Position.Value;

            if (window.ForceMainWindow)
            {
                pos += ImGuiHelpers.MainViewport.Pos;
            }

            ImGui.SetNextWindowPos(pos, window.PositionCondition);
        }

        if (window.Size.HasValue)
        {
            ImGui.SetNextWindowSize(window.Size.Value * ImGuiHelpers.GlobalScale, window.SizeCondition);
        }

        if (window.Collapsed.HasValue)
        {
            ImGui.SetNextWindowCollapsed(window.Collapsed.Value, window.CollapsedCondition);
        }

        if (window.SizeConstraints.HasValue)
        {
            ImGui.SetNextWindowSizeConstraints(window.SizeConstraints.Value.MinimumSize * ImGuiHelpers.GlobalScale, window.SizeConstraints.Value.MaximumSize * ImGuiHelpers.GlobalScale);
        }

        if (window.BgAlpha.HasValue)
        {
            ImGui.SetNextWindowBgAlpha(window.BgAlpha.Value);
        }
    }

    private void DrawWindow(Window window)
    {
        var state = this.GetState(window);

        window.PreOpenCheck();

        if (!window.IsOpen)
        {
            if (window.IsOpen != state.LastIsOpen)
            {
                state.LastIsOpen = window.IsOpen;
                window.OnClose();
                state.IsFocused = false;
            }

            return;
        }

        window.Update();
        if (!window.DrawConditions())
        {
            return;
        }

        if (!string.IsNullOrEmpty(window.Namespace))
        {
            ImGui.PushID(window.Namespace);
        }

        if (state.LastIsOpen != window.IsOpen && window.IsOpen)
        {
            state.LastIsOpen = window.IsOpen;
            window.OnOpen();
        }

        window.PreDraw();
        this.ApplyConditionals(window);

        if (window.ForceMainWindow)
        {
            ImGuiHelpers.ForceNextWindowMainViewport();
        }

        var wasFocused = state.IsFocused;
        if (wasFocused)
        {
            var style = ImGui.GetStyle();
            var focusedHeaderColor = style.Colors[(int)ImGuiCol.TitleBgActive];
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, focusedHeaderColor);
        }

        if (state.NextFrameBringToFront)
        {
            ImGui.SetNextWindowFocus();
            state.NextFrameBringToFront = false;
        }

        var flags = window.Flags;

        if (state.IsPinned || state.IsClickthrough)
        {
            flags |= ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
        }

        if (state.IsClickthrough)
        {
            flags |= ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoCollapse |
                     ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoMouseInputs;
        }

        var canShowCloseButton = window.ShowCloseButton && !state.IsClickthrough;

        var isOpen = window.IsOpen;
        var beginSuccess = canShowCloseButton
                               ? ImGui.Begin(window.WindowName, ref isOpen, flags)
                               : ImGui.Begin(window.WindowName, flags);

        window.IsOpen = isOpen;

        if (beginSuccess)
        {
            try
            {
                window.Draw();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Draw(): {window.WindowName}\n{ex}");
            }
        }

        if (wasFocused)
        {
            ImGui.PopStyleColor();
        }

        state.IsFocused = ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);

        ImGui.End();

        window.PostDraw();

        if (!string.IsNullOrEmpty(window.Namespace))
        {
            ImGui.PopID();
        }
    }

    /// <summary>
    /// Simulates the internal window state that dalamud windows have and that we can't access.
    /// </summary>
    public class WindowState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the window should be open/closed.
        /// </summary>
        public bool LastIsOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the window is focused.
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window should be brought to the front next frame.
        /// </summary>
        public bool NextFrameBringToFront { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is pinned.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window should allow click through.
        /// </summary>
        public bool IsClickthrough { get; set; }
    }
}
