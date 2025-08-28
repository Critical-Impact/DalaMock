namespace DalaMock.Core.Plugin;

using System;
using System.Collections.Generic;

/// <summary>
/// An interface describing a plugin loader.
/// </summary>
public interface IPluginLoader
{
    public Dictionary<Type, MockPlugin> LoadedPlugins { get; }

    MockPlugin AddPlugin(Type dalamudPluginType);

    bool StartPlugin(MockPlugin plugin, PluginLoadSettings pluginLoadSettings);

    bool StopPlugin(MockPlugin plugin);

    bool HasPluginsLoaded { get; }

    bool HasPluginsStarted { get; }
}
