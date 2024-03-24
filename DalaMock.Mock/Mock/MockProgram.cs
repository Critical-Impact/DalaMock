using System.Numerics;
using System.Reflection;
using DalaMock.Configuration;
using DalaMock.Dalamud;
using DalaMock.Interfaces;
using DalaMock.Shared.Interfaces;
using Dalamud;
using Dalamud.Interface.Utility;
using ImGuiNET;
using Lumina;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Vector3 = System.Numerics.Vector3;

namespace DalaMock.Mock;

public class MockProgram : IDisposable
{
    private readonly IServiceContainer _serviceContainer;
    private GameData? _gameData;
    private Logger _seriLog;
    private Sdl2Window _window;
    private GraphicsDevice _graphicsDevice;
    private CommandList _commandList;
    private ImGuiController _controller;
    private Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);
    private MockPluginInterfaceService _mockPluginInterfaceService;
    private IPluginInterfaceService _pluginInterfaceService;
    private MockService? _mockService;
    private IMockPlugin? _mockPlugin;
    private MockFramework _mockFramework;
    
    public Sdl2Window Window => _window;
    public GraphicsDevice GraphicsDevice => _graphicsDevice;
    public CommandList CommandList => _commandList;
    public ImGuiController Controller => _controller;
    public MockService MockService => _mockService;

    public Logger SeriLog => _seriLog;

    public MockFramework Framework => _mockFramework;

    private Dictionary<Type, object> _extraServices = new Dictionary<Type, object>();
    
    public MockProgram(IServiceContainer serviceContainer)
    {
        _serviceContainer = serviceContainer;
        _mockFramework = new MockFramework();
        Framework.FireUpdate();
        var levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = LogEventLevel.Verbose,
        };
        
        _seriLog = new LoggerConfiguration()
            .WriteTo.Console(standardErrorFromLevel: LogEventLevel.Verbose)
            .MinimumLevel.ControlledBy(levelSwitch)
            .CreateLogger();            

        var field = typeof(ImGuiHelpers).GetProperty("GlobalScale", 
            BindingFlags.Static | 
            BindingFlags.Public);
        field.SetValue(null, 1);
            
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "DalaMock"),
            new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
            out _window,
            out _graphicsDevice);
        _window.Resized += () =>
        {
            _graphicsDevice.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
            _controller.WindowResized(_window.Width, _window.Height);
        };
            
        _commandList = _graphicsDevice.ResourceFactory.CreateCommandList();

        _controller = new ImGuiController(_graphicsDevice, _graphicsDevice.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);
        var property = typeof(ImGuiHelpers).GetProperty("MainViewport", 
            BindingFlags.Static | 
            BindingFlags.Public);
        property.SetValue(null, ImGui.GetMainViewport());
    }

    public bool PumpEvents(Action preUpdate, Action postUpdate)
    {
        InputSnapshot snapshot = _window.PumpEvents();
        if (!_window.Exists) { return false; }
        preUpdate.Invoke();
        _controller.Update(1f / 60f, snapshot);
        postUpdate.Invoke();
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.MainSwapchain.Framebuffer);
        _commandList.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
        _controller.Render(_graphicsDevice, _commandList);
        _commandList.End();
        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.SwapBuffers(_graphicsDevice.MainSwapchain);
        return true;
    }

    public void SetPlugin(IMockPlugin mockPlugin)
    {
        _mockPlugin = mockPlugin;
    }

    public void AddService(Type interfaceType, object mockObject)
    {
        _extraServices.Add(interfaceType, mockObject);
    }
    
    
    public bool StartPlugin()
    {
        var gameLocation = AppSettings.Default.GamePath;
        var configDirectory = AppSettings.Default.PluginConfigPath;
        var internalName = AppSettings.Default.PluginInternalName;
        if (gameLocation != null && configDirectory != null && internalName != null && _mockPlugin != null)
        {
            _gameData ??= new GameData(gameLocation, new LuminaOptions()
            {
                PanicOnSheetChecksumMismatch = false
            });
            var configFile = Path.Combine(configDirectory, internalName + ".json");
            var configFolder = Path.Combine(configDirectory, internalName);
            _mockPluginInterfaceService = new MockPluginInterfaceService(this, new FileInfo(configFile),
                new DirectoryInfo(configFolder));
            _pluginInterfaceService = _mockPluginInterfaceService;
            if (_mockService == null)
            {
                _mockService = new MockService(this, _serviceContainer, _mockPluginInterfaceService, Framework, _gameData,
                    ClientLanguage.English, SeriLog);
                _mockService.BuildMockServices(_extraServices);
                _mockService.InjectMockServices();
            }
            else
            {
                _serviceContainer.PluginInterfaceService = _pluginInterfaceService;
            }
            _mockPlugin.Start(this, _mockService, _mockPluginInterfaceService);
            return true;
        }
        return false;
    }

    public void StopPlugin()
    {
        _mockPlugin?.Stop(this, _mockService!, _mockPluginInterfaceService);
    }

    public void Dispose()
    {
        _graphicsDevice.WaitForIdle();
        _controller.Dispose();
        _commandList.Dispose();
        _graphicsDevice.Dispose();
    }
}