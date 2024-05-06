using DalaMock.Mock;
using DalaMock.Shared.Interfaces;
using Dalamud;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Plugin.Services;
using Lumina;
using ILogger = Serilog.ILogger;

namespace DalaMock.Dalamud;

public class MockService
{
    private MockContainer? _mockContainer;
    private readonly IServiceContainer _serviceContainer;
    private readonly MockProgram _mockProgram;
    private readonly MockPluginInterfaceService _mockPluginInterfaceService;
    private readonly GameData _gameData;
    private readonly ClientLanguage _clientLanguage;
    private readonly ILogger _log;
    private readonly IPluginLog _pluginLog;
    
    private MockPluginLog _mockPluginLog;
    private MockClientState _mockClientState;
    private MockDataManager _mockDataManager;
    private MockFramework _mockFramework;
    private MockKeyState _mockKeyState;
    private MockCommandManager _mockCommandManager;
    private MockTextureManager _mockTextureManger;
    private MockTextureProvider _textureProvider;
    private MockGameGui _mockGameGui;
    private MockChatGui _mockChatGui;
    private MockCondition _mockCondition;
    private MockAddonLifecycle _mockAddonLifecycle;
    private MockGameInteropProvider _mockGameInteropProvider;
    private MockLibcFunction _mockLibcFunction;
    private MockGameNetwork _mockGameNetwork;
    private MockObjectTable _mockObjectTable;
    private MockTargetManager _mockTargetManager;
    private MockToastGui _mockToastGui;
    private MockContextMenu _mockContextMenu;
    private MockTitleScreenMenu _mockTitleScreenMenu;

    public MockPluginLog MockPluginLog => _mockPluginLog;
    public MockClientState MockClientState => _mockClientState;
    public MockDataManager MockDataManager => _mockDataManager;
    public MockFramework MockFramework => _mockFramework;
    public MockKeyState MockKeyState => _mockKeyState;
    public MockCommandManager MockCommandManager => _mockCommandManager;
    public MockTextureManager MockTextureManger => _mockTextureManger;
    public MockTextureProvider TextureProvider => _textureProvider;
    public MockGameGui MockGameGui => _mockGameGui;
    public MockCondition MockCondition => _mockCondition;
    public MockAddonLifecycle MockAddonLifecycle => _mockAddonLifecycle;
    public MockContextMenu MockContextMenu => _mockContextMenu;

    public MockService(MockProgram mockProgram, IServiceContainer serviceContainer, MockPluginInterfaceService pluginInterfaceService, MockFramework mockFramework, GameData gameData, ClientLanguage clientLanguage, ILogger log)
    {
        _mockPluginInterfaceService = pluginInterfaceService;
        _mockFramework = mockFramework;
        _serviceContainer = serviceContainer;
        _mockProgram = mockProgram;
        _gameData = gameData;
        _clientLanguage = clientLanguage;
        _log = log;
    }
    
    public MockService(IServiceContainer serviceContainer, MockPluginInterfaceService pluginInterfaceService, MockFramework mockFramework, GameData gameData, ClientLanguage clientLanguage, ILogger log)
    {
        _mockPluginInterfaceService = pluginInterfaceService;
        _mockFramework = mockFramework;
        _serviceContainer = serviceContainer;
        _gameData = gameData;
        _clientLanguage = clientLanguage;
        _log = log;
    }

    public void BuildMockServices(Dictionary<Type, object>? extraServices = null)
    {
        _mockPluginLog = new MockPluginLog(_log);
        _mockClientState = new MockClientState();
        _mockDataManager = new MockDataManager(_gameData, _clientLanguage);
        if (_mockProgram.Window != null)
        {
            _mockKeyState = new MockKeyState(_mockProgram.Window);
        }

        _mockCommandManager = new MockCommandManager(_mockPluginLog,_clientLanguage);
        if (_mockProgram.GraphicsDevice != null && _mockProgram.Controller != null)
        {
            _mockTextureManger = new MockTextureManager(_mockProgram.GraphicsDevice, _mockProgram.Controller,
                _mockFramework, _mockDataManager, _clientLanguage, _mockPluginLog);

            _textureProvider = new MockTextureProvider(_mockTextureManger);
        }

        _mockGameGui = new MockGameGui();
        _mockChatGui = new MockChatGui();
        _mockCondition = new MockCondition();
        _mockAddonLifecycle = new MockAddonLifecycle();
        _mockGameInteropProvider = new MockGameInteropProvider();
        _mockLibcFunction = new MockLibcFunction();
        _mockGameNetwork = new MockGameNetwork();
        _mockObjectTable = new MockObjectTable();
        _mockTargetManager = new MockTargetManager();
        _mockToastGui = new MockToastGui();
        _mockContextMenu = new MockContextMenu();
        _mockTitleScreenMenu = new MockTitleScreenMenu();

        _mockContainer = new MockContainer(_mockPluginLog);
        _mockContainer.AddInstance(typeof(IClientState), _mockClientState);
        _mockContainer.AddInstance(typeof(IDataManager), _mockDataManager);
        _mockContainer.AddInstance(typeof(IFramework), _mockFramework);
        if (_mockProgram.Window != null)
        {
            _mockContainer.AddInstance(typeof(IKeyState), _mockKeyState);
        }

        _mockContainer.AddInstance(typeof(ICommandManager), _mockCommandManager);
        if (_mockProgram.GraphicsDevice != null && _mockProgram.Controller != null)
        {
            _mockContainer.AddInstance(typeof(ITextureProvider), _textureProvider);
        }

        _mockContainer.AddInstance(typeof(IGameGui), _mockGameGui);
        _mockContainer.AddInstance(typeof(IChatGui), _mockChatGui);
        _mockContainer.AddInstance(typeof(IPluginLog), _mockPluginLog);
        _mockContainer.AddInstance(typeof(ICondition), _mockCondition);
        _mockContainer.AddInstance(typeof(IAddonLifecycle), _mockAddonLifecycle);
        _mockContainer.AddInstance(typeof(IGameInteropProvider), _mockGameInteropProvider);
        _mockContainer.AddInstance(typeof(ILibcFunction), _mockLibcFunction);
        _mockContainer.AddInstance(typeof(IGameNetwork), _mockGameNetwork);
        _mockContainer.AddInstance(typeof(IObjectTable), _mockObjectTable);
        _mockContainer.AddInstance(typeof(ITargetManager), _mockTargetManager);
        _mockContainer.AddInstance(typeof(IPluginInterfaceService), _mockPluginInterfaceService);
        _mockContainer.AddInstance(typeof(IToastGui), _mockToastGui);
        _mockContainer.AddInstance(typeof(IContextMenu), _mockContextMenu);
        _mockContainer.AddInstance(typeof(ITitleScreenMenu), _mockTitleScreenMenu);
        if (extraServices != null)
        {
            foreach (var extraService in extraServices)
            {
                _mockContainer.AddInstance(extraService.Key, extraService.Value);
            }
        }
    }

    public void InjectMockServices()
    {
        _mockContainer?.Create<IServiceContainer>(_serviceContainer);
    }
}