namespace DalaMock.Core.Plugin;

using System;
using Autofac;
using Dalamud.Plugin;

/// <summary>
/// An interface describing a mock plugin.
/// </summary>
public interface IMockPlugin
{
    /// <summary>
    /// Gets a value indicating whether the plugin is loaded?
    /// </summary>
    public bool IsLoaded { get; }

    /// <summary>
    /// Gets the loaded plugin
    /// </summary>
    public IDalamudPlugin? DalamudPlugin { get; }

    /// <summary>
    /// Gets the settings that were used when the plugin was loaded
    /// </summary>
    public PluginLoadSettings? PluginLoadSettings { get; }

    /// <summary>
    /// Gets the DI container for the plugin
    /// </summary>
    public IContainer? Container { get; }

    /// <summary>
    /// The type of the plugin being loaded
    /// </summary>
    public Type PluginType { get; }

    /// <summary>
    /// Mark the plugin as started
    /// </summary>
    /// <param name="dalamudPlugin">The loaded plugin.</param>
    /// <param name="pluginLoadSettings">The settings that were used when the plugin was loaded.</param>
    /// <param name="container">The DI container for the plugin.</param>
    public void Startup(IDalamudPlugin dalamudPlugin, PluginLoadSettings pluginLoadSettings, IContainer container);

    /// <summary>
    /// Mark the plugin as unloaded
    /// </summary>
    public void Teardown();
}