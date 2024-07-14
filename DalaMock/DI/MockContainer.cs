using System;
using DalaMock.Core.Imgui.Auto;

namespace DalaMock.Core.DI;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using DalaMock.Core.Configuration;
using DalaMock.Core.Imgui;
using DalaMock.Core.Interface;
using DalaMock.Core.Mocks;
using DalaMock.Core.Plugin;
using DalaMock.Core.Windows;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface.Windowing;
using Lumina;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Veldrid;
using Veldrid.Sdl2;

/// <summary>
/// The main service container that provides plugin loaders, loggers, and UI if required.
/// </summary>
public class MockContainer
{
    private readonly MockDalamudConfiguration dalamudConfiguration;
    private readonly LoggingLevelSwitch levelSwitch;
    private readonly IContainer container;
    private readonly Logger seriLog;
    private readonly ConfigurationManager configurationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockContainer"/> class.
    /// Creates a new mock container with the given configuration.
    /// </summary>
    /// <param name="dalamudConfiguration">The configuration to use.</param>
    /// <param name="containerBuildHook">Allows you to alter the services registered by the container.</param>
    public MockContainer(MockDalamudConfiguration? dalamudConfiguration = null, Action<ContainerBuilder>? containerBuildHook = null)
    {
        this.configurationManager = new ConfigurationManager();
        this.dalamudConfiguration = dalamudConfiguration ?? this.configurationManager.LoadConfiguration();
        this.levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = LogEventLevel.Verbose,
        };
        this.seriLog = new LoggerConfiguration()
            .WriteTo.Console(standardErrorFromLevel: LogEventLevel.Verbose)
            .MinimumLevel.ControlledBy(this.levelSwitch)
            .CreateLogger();

        var builder = new ContainerBuilder();
        this.RegisterMockServices(builder);
        containerBuildHook?.Invoke(builder);
        this.container = builder.Build();
    }

    private void RegisterMockServices(ContainerBuilder builder)
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Find all types that implement IMockService and are not abstract
        var mockServiceTypes = assembly.GetTypes()
            .Where(t => typeof(IMockService).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        var mockWindows = assembly.GetTypes()
            .Where(t => typeof(IMockWindow).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        // ImGui.Auto
        builder.RegisterType<ImGuiElementGenerator>();
        var imGuiElements = assembly.GetTypes()
            .Where(t => typeof(ImGuiBaseElement).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var imGuiElement in imGuiElements)
        {
            builder.RegisterType(imGuiElement).AsSelf().As<IImGuiElement>();
        }

        builder.RegisterInstance(this.seriLog).As<Serilog.ILogger>();
        builder.RegisterInstance(this.levelSwitch);
        builder.RegisterInstance(this.configurationManager);
        builder.Register<GameData>(
            c => new GameData(
                this.dalamudConfiguration.GamePath!.FullName,
                new LuminaOptions
                {
                    PanicOnSheetChecksumMismatch = false,
                }));
        builder.RegisterInstance(this.dalamudConfiguration);
        builder.RegisterType<MockDalamudUi>().SingleInstance();
        builder.RegisterType<PluginLoader>().SingleInstance();
        builder.RegisterType<MockFileDialogManager>().As<IFileDialogManager>().SingleInstance();
        builder.RegisterInstance(this).SingleInstance();
        builder.RegisterInstance(new MockWindowSystem("DalaMock"));
        builder.RegisterType<MockMockWindow>().AsSelf().As<Window>().SingleInstance();
        builder.RegisterType<MockSettingsWindow>().AsSelf().As<Window>().SingleInstance();
        builder.RegisterType<LocalPlayersWindow>().AsSelf().As<Window>().SingleInstance();
        builder.RegisterType<LocalPlayerEditWindow>().AsSelf().As<Window>().SingleInstance();
        builder.Register<ImGuiScene>(_ => ImGuiScene.CreateWindow()).SingleInstance();

        builder.Register<GraphicsDevice>(
            c =>
            {
                var context = c.Resolve<ImGuiScene>();
                return context.GraphicsDevice;
            }).SingleInstance();

        builder.Register<CommandList>(
            c =>
            {
                var context = c.Resolve<ImGuiScene>();
                return context.CommandList;
            }).SingleInstance();

        builder.Register<Sdl2Window>(
            c =>
            {
                var context = c.Resolve<ImGuiScene>();
                return context.Window;
            }).SingleInstance();

        // Register each type as implementing IMockService
        foreach (var type in mockServiceTypes)
        {
            builder.RegisterType(type).AsSelf().As(type.GetInterfaces()).SingleInstance();
        }

        // Register each type as implementing IMockService
        foreach (var type in mockWindows)
        {
            builder.RegisterType(type).AsSelf().As<IMockWindow>().SingleInstance();
        }
    }

    /// <summary>
    /// Gets a mock UI, allowing you to render the imgui windows of plugins.
    /// </summary>
    /// <returns>A mock dalamud ui class.</returns>
    public MockDalamudUi GetMockUi()
    {
        return this.container.Resolve<MockDalamudUi>();
    }

    /// <summary>
    /// Returns a plugin loader, allows for multiple plugins to be loaded, then started and stopped.
    /// Each plugin will get it's own DI container.
    /// </summary>
    /// <returns>The plugin loader.</returns>
    public PluginLoader GetPluginLoader()
    {
        return this.container.Resolve<PluginLoader>();
    }

    /// <summary>
    /// Returns a list of all the mocked services that are available.
    /// </summary>
    /// <returns>An enumerable list of all mock services.</returns>
    public IEnumerable<IMockService> GetMockServices()
    {
        return this.container.Resolve<IEnumerable<IMockService>>().ToList();
    }

    /// <summary>
    /// Returns a list of all windows that are considered to be global to DalaMock.
    /// </summary>
    /// <returns>An enumerable list of all windows that are considered to be global to DalaMock.</returns>
    public IEnumerable<Window> GetWindows()
    {
        return this.container.Resolve<IEnumerable<Window>>().ToList();
    }

    /// <summary>
    /// Returns a mock window system.
    /// The mock window system extends the normal window system to get around the issue of not being able to initialize any dalamud services.
    /// </summary>
    /// <returns>A mock window system.</returns>
    public MockWindowSystem GetWindowSystem()
    {
        return this.container.Resolve<MockWindowSystem>();
    }

    /// <summary>
    /// Returns a serilog logger. Will log to console.
    /// </summary>
    /// <returns>Returns a serilog logger.</returns>
    public Serilog.ILogger GetLogger()
    {
        return this.container.Resolve<Serilog.ILogger>();
    }

    /// <summary>
    /// Gets the service container, can be used to request services.
    /// </summary>
    /// <returns>The service container.</returns>
    public IContainer GetContainer()
    {
        return this.container;
    }

    /// <summary>
    /// Gets lumina's GameData object.
    /// </summary>
    /// <returns>A lumina GameData object.</returns>
    public GameData GetGameData()
    {
        return this.container.Resolve<GameData>();
    }
}