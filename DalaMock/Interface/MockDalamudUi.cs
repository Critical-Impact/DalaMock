namespace DalaMock.Core.Interface;

using System;
using System.Collections.Generic;
using Autofac;
using DalaMock.Core.DI;
using DalaMock.Core.Imgui;
using DalaMock.Core.Mocks;
using DalaMock.Core.Plugin;
using DalaMock.Core.Windows;
using ImGuiNET;

/// <summary>
/// Provides a mock dalamud UI allowing for plugins to be loaded/unloaded at will.
/// </summary>
public class MockDalamudUi : IDisposable
{
    private readonly ImGuiScene imGuiScene;
    private readonly PluginLoader pluginLoader;
    private readonly MockWindowSystem windowSystem;
    private Dictionary<MockPlugin, MockFramework> frameworks;
    private Dictionary<MockPlugin, MockUiBuilder> uiBUilders;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockDalamudUi"/> class.
    /// Keeps track of each loaded plugin, and draws it's windows as required. Also draws any mock windows that have been registered by DalaMock.
    /// </summary>
    /// <param name="mockContainer">The autofac container for the primary mock application.</param>
    /// <param name="pluginLoader">The plugin loader.</param>
    public MockDalamudUi(MockContainer mockContainer, PluginLoader pluginLoader)
    {
        this.pluginLoader = pluginLoader;
        this.frameworks = new Dictionary<MockPlugin, MockFramework>();
        this.uiBUilders = new Dictionary<MockPlugin, MockUiBuilder>();
        var windows = mockContainer.GetWindows();
        this.windowSystem = mockContainer.GetWindowSystem();
        this.imGuiScene = mockContainer.GetContainer().Resolve<ImGuiScene>();

        foreach (var window in windows)
        {
            window.IsOpen = true;
            this.windowSystem.AddWindow(window);
        }

        this.pluginLoader.PluginStarted += this.PluginLoaderOnPluginStarted;
        this.pluginLoader.PluginStopped += this.PluginLoaderOnPluginStopped;
    }


    /// <inheritdoc/>
    public void Dispose()
    {
        this.pluginLoader.PluginStarted -= this.PluginLoaderOnPluginStarted;
        this.pluginLoader.PluginStopped -= this.PluginLoaderOnPluginStopped;
    }

    /// <summary>
    /// Starts the ui's render and update loop.
    /// </summary>
    public void Run()
    {
        this.imGuiScene.OnBuildUi += () =>
        {
            ImGui.ShowDemoWindow();

            // Move the updates elsewhere
            foreach (var framework in this.frameworks)
            {
                framework.Value.FireUpdate();
            }

            foreach (var uiBuilder in this.uiBUilders)
            {
                uiBuilder.Value.FireDraw();
            }

            foreach (var window in this.windowSystem.Windows)
            {
                window.AllowClickthrough = false;
                window.AllowPinning = false;
                window.ForceMainWindow = false;
            }

            this.windowSystem.Draw();
        };
        this.imGuiScene.Run();
    }

    private void PluginLoaderOnPluginStopped(MockPlugin mockPlugin)
    {
        this.uiBUilders.Remove(mockPlugin);
        this.frameworks.Remove(mockPlugin);
    }

    private void PluginLoaderOnPluginStarted(MockPlugin mockPlugin)
    {
        if (mockPlugin.Container != null)
        {
            this.uiBUilders.TryAdd(mockPlugin, mockPlugin.Container.Resolve<MockUiBuilder>());
            if (mockPlugin.Container.TryResolve<MockFramework>(out var framework))
            {
                this.frameworks.TryAdd(mockPlugin, framework);
            }
        }
    }
}