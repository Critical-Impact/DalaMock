using System.IO;

namespace DalaMock.Core.Plugin;

using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly MockDalamudConfiguration mockDalamudConfiguration;

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
    public PluginLoader(MockContainer mockContainer, MockDalamudConfiguration mockDalamudConfiguration, ILogger logger)
    {
        this.mockContainer = mockContainer;
        this.mockDalamudConfiguration = mockDalamudConfiguration;
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

    public bool StartPlugin(MockPlugin mockPlugin)
    {
        if (this.mockDalamudConfiguration.PluginSavePath == null)
        {
            this.logger.Warning("Could not automatically start plugin as no plugin save path was provided. If this is your first time running DalaMock, this path will be saved once provided or you can provide one programatically.");
            return false;
        }

        var assemblyName = mockPlugin.PluginType.BaseType?.Assembly.GetName().Name ?? mockPlugin.PluginType.Assembly.GetName().Name ?? mockPlugin.PluginType.Name;

        var pluginDirectory = new DirectoryInfo(Path.Combine(this.mockDalamudConfiguration.PluginSavePath.FullName, assemblyName));
        return this.StartPlugin(mockPlugin, new PluginLoadSettings(pluginDirectory, new FileInfo(Path.Combine(this.mockDalamudConfiguration.PluginSavePath.FullName, assemblyName + ".json"))));
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
        IEnumerable<IMockService> mockServices;
        try
        {
            mockServices = this.mockContainer.GetMockServices();
        }
        catch (Exception e)
        {
            this.logger.Error(e, "Failed to get mock services.");
            throw;
        }

        foreach (var mockService in mockServices)
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

        var scene = this.mockContainer.GetContainer().Resolve<DalaMock.Core.Imgui.ImGuiScene>();

        var uiBuilder = new MockUiBuilder(scene);
        builder.RegisterInstance(uiBuilder);
        builder.RegisterInstance(pluginLoadSettings);
        builder.RegisterInstance(this);
        builder.Register<MockDalamudPluginInterface>(c =>
            new MockDalamudPluginInterface(
                uiBuilder,
                pluginLoadSettings,
                plugin.PluginType.FullName ?? plugin.PluginType.Name,
                c.Resolve<IComponentContext>())).As<IDalamudPluginInterface>();

        builder.RegisterInstance(this.mockContainer.GetLogger());

        var container = builder.Build();


        if (container.TryResolve(plugin.PluginType, out object? builtPlugin))
        {
            var dalamudPlugin = (IDalamudPlugin)builtPlugin;
            plugin.Startup(dalamudPlugin, pluginLoadSettings, container);

            this.logger.Information($"Started plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");
            this.OnPluginStarted(plugin);
            return true;
        }

        this.logger.Information($"Failed to start plugin {plugin.PluginType.FullName ?? plugin.PluginType.Name}");
        return false;


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

    public bool HasPluginsLoaded => this.LoadedPlugins.Count != 0;
    public bool HasPluginsStarted => this.LoadedPlugins.Count(c => c.Value.IsLoaded) != 0;

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
