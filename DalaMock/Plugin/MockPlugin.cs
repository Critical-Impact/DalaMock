namespace DalaMock.Core.Plugin;

using System;

using Autofac;

using Dalamud.Plugin;

/// <summary>
/// A mock plugin, managed by <see cref="IPluginLoader"/>.
/// </summary>
/// <param name="pluginType">The type of the plugin being loaded.</param>
public class MockPlugin(Type pluginType) : IMockPlugin
{
    private IContainer? container;
    private IDalamudPlugin? dalamudPlugin;
    private PluginLoadSettings? pluginLoadSettings;

    public MockPlugin(Type pluginType, PluginLoadSettings pluginLoadSettings)
        : this(pluginType)
    {
        this.pluginLoadSettings = pluginLoadSettings;
    }

    /// <inheritdoc/>
    public bool IsLoaded => this.DalamudPlugin != null && this.PluginLoadSettings != null && this.Container != null;

    /// <inheritdoc/>
    public IDalamudPlugin? DalamudPlugin => this.dalamudPlugin;

    /// <inheritdoc/>
    public PluginLoadSettings? PluginLoadSettings => this.pluginLoadSettings;

    /// <inheritdoc/>
    public IContainer? Container => this.container;

    /// <inheritdoc/>
    public Type PluginType => pluginType;

    /// <inheritdoc/>
    public void Startup(IDalamudPlugin dalamudPlugin, PluginLoadSettings pluginLoadSettings, IContainer container)
    {
        this.dalamudPlugin = dalamudPlugin;
        this.pluginLoadSettings = pluginLoadSettings;
        this.container = container;
    }

    /// <inheritdoc/>
    public void Teardown()
    {
        this.dalamudPlugin = null;
    }
}
