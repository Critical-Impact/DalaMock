namespace DalaMock.Core.Plugin;

using System;
using System.Collections.Generic;
using Autofac;
using DalaMock.Core.DI;
using DalaMock.Core.Mocks;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Serilog;

/// <summary>
/// The plugin loader is responsible for loading/starting and stopping plugins.
/// </summary>
public class PluginLoader : IPluginLoader
{
    private readonly Dictionary<Type, MockPlugin> loadedPlugins;
    private readonly ILogger logger;
    private readonly MockContainer mockContainer;

    /// <summary>
    /// Has the plugin's state changed, has it started or stopped?
    /// <param name="mockPlugin">The plugin whose state has changed.</param>
    /// </summary>
    public delegate void PluginStateChangeDelegate(MockPlugin mockPlugin);

    /// <summary>
    /// An event that fires when a plugin starts.
    /// </summary>
    public event PluginStateChangeDelegate PluginStarted;

    /// <summary>
    /// An event that fires when a plugin stops.
    /// </summary>
    public event PluginStateChangeDelegate PluginStopped;

    /// <summary>
    /// Gets a list of all plugins that have been loaded into the plugin loader.
    /// </summary>
    public Dictionary<Type, MockPlugin> LoadedPlugins => this.loadedPlugins;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginLoader"/> class.
    /// </summary>
    /// <param name="mockContainer">The global DI container.</param>
    /// <param name="logger">The serilog logger.</param>
    public PluginLoader(MockContainer mockContainer, ILogger logger)
    {
        this.mockContainer = mockContainer;
        this.logger = logger;
        this.loadedPlugins = new Dictionary<Type, MockPlugin>();
    }

    /// <summary>
    /// Add a plugin to the plugin loader.
    /// </summary>
    /// <param name="dalamudPluginType">The type of the plugin to add.</param>
    /// <returns>A mock plugin that can be used to keep track of the state of your plugin.</returns>
    public MockPlugin AddPlugin(Type dalamudPluginType)
    {
        if (!this.loadedPlugins.ContainsKey(dalamudPluginType))
        {
            this.loadedPlugins.Add(dalamudPluginType, new MockPlugin(dalamudPluginType));
        }

        return this.loadedPlugins[dalamudPluginType];
    }

    /// <summary>
    /// Starts a loaded plugin.
    /// </summary>
    /// <param name="plugin">The mock plugin to start.</param>
    /// <param name="pluginLoadSettings">The settings that should be used to start the plugin.</param>
    /// <returns>A boolean indicating whether or not the plugin started successfully.</returns>
    public bool StartPlugin(MockPlugin plugin, PluginLoadSettings pluginLoadSettings)
    {
        if (plugin.IsLoaded)
        {
            this.logger.Information(
                $"Could not start plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}, already started.");
            return false;
        }

        this.logger.Information($"Starting plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");

        var builder = new ContainerBuilder();

        builder.RegisterType(plugin.PluginType).AsSelf().As<IDalamudPlugin>();

        foreach (var mockService in this.mockContainer.GetMockServices())
        {
            var registrationBuilder = builder.RegisterInstance(mockService).AsSelf();
            var interfaces = mockService.GetType().GetInterfaces();
            if (interfaces.Length != 0)
            {
                registrationBuilder.As(interfaces);
            }
        }

        foreach (var window in this.mockContainer.GetWindows())
        {
            builder.RegisterInstance(window).AsSelf().As<Window>();
        }

        builder.RegisterInstance(this.mockContainer.GetWindowSystem());

        var uiBuilder = new MockUiBuilder();
        builder.RegisterInstance(uiBuilder);
        builder.Register<MockDalamudPluginInterface>(c =>
            new MockDalamudPluginInterface(
                uiBuilder,
                pluginLoadSettings.ConfigFile,
                pluginLoadSettings.ConfigDir,
                plugin.PluginType.FullName ?? plugin.PluginType.Name,
                c.Resolve<IComponentContext>())).As<IDalamudPluginInterface>();

        builder.RegisterInstance(this.mockContainer.GetLogger());

        var container = builder.Build();

        var builtPlugin = (IDalamudPlugin)container.Resolve(plugin.PluginType);

        plugin.Startup(builtPlugin, pluginLoadSettings, container);

        this.logger.Information($"Started plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");
        this.OnPluginStarted(plugin);
        return true;
    }

    /// <summary>
    /// Stops a loaded plugin.
    /// </summary>
    /// <param name="plugin">The plugin to stop.</param>
    /// <returns>A boolean indicating whether the plugin could be stopped.</returns>
    public bool StopPlugin(MockPlugin plugin)
    {
        if (!plugin.IsLoaded || plugin.DalamudPlugin == null || plugin.Container == null)
        {
            this.logger.Information(
                $"Could not stop plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}, already stopped.");
            return false;
        }

        this.logger.Information($"Stopping plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");

        plugin.DalamudPlugin.Dispose();
        plugin.Container.Dispose();
        plugin.Teardown();
        this.logger.Information($"Stopped plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");
        this.OnPluginStopped(plugin);
        return true;
    }

    /// <summary>
    /// Invokes the plugin started event.
    /// </summary>
    /// <param name="mockPlugin">The plugin that has started.</param>
    protected virtual void OnPluginStarted(MockPlugin mockPlugin)
    {
        this.PluginStarted?.Invoke(mockPlugin);
    }

    /// <summary>
    /// Invokes the plugin stopped event.
    /// </summary>
    /// <param name="mockPlugin">The plugin that has started.</param>
    protected virtual void OnPluginStopped(MockPlugin mockPlugin)
    {
        this.PluginStopped?.Invoke(mockPlugin);
    }
}