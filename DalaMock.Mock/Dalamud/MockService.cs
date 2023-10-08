using DalaMock.Mock;
using DalaMock.Shared.Interfaces;
using Dalamud;
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

    public MockService(MockProgram mockProgram, IServiceContainer serviceContainer, MockPluginInterfaceService pluginInterfaceService, GameData gameData, ClientLanguage clientLanguage, ILogger log)
    {
        _mockPluginInterfaceService = pluginInterfaceService;
        _serviceContainer = serviceContainer;
        _mockProgram = mockProgram;
        _gameData = gameData;
        _clientLanguage = clientLanguage;
        _log = log;
    }

    internal void BuildMockServices(Dictionary<Type, object>? extraServices = null)
    {
        _mockPluginLog = new MockPluginLog(_log);
        _mockClientState = new MockClientState();
        _mockDataManager = new MockDataManager(_gameData, _clientLanguage);
        _mockFramework = new MockFramework();
        _mockKeyState = new MockKeyState(_mockProgram.Window);
        _mockCommandManager = new MockCommandManager(_mockPluginLog,_clientLanguage);
        _mockTextureManger = new MockTextureManager(_mockProgram.GraphicsDevice,_mockProgram.Controller, _mockFramework, _mockDataManager, _clientLanguage, _mockPluginLog );
        _textureProvider = new MockTextureProvider(_mockTextureManger);
        _mockGameGui = new MockGameGui();
        _mockChatGui = new MockChatGui();
        _mockCondition = new MockCondition();

        _mockContainer = new MockContainer(_mockPluginLog);
        _mockContainer.AddInstance(typeof(IClientState), _mockClientState);
        _mockContainer.AddInstance(typeof(IDataManager), _mockDataManager);
        _mockContainer.AddInstance(typeof(IFramework), _mockFramework);
        _mockContainer.AddInstance(typeof(IKeyState), _mockKeyState);
        _mockContainer.AddInstance(typeof(ICommandManager), _mockCommandManager);
        _mockContainer.AddInstance(typeof(ITextureProvider), _textureProvider);
        _mockContainer.AddInstance(typeof(IGameGui), _mockGameGui);
        _mockContainer.AddInstance(typeof(IChatGui), _mockChatGui);
        _mockContainer.AddInstance(typeof(IPluginLog), _mockPluginLog);
        _mockContainer.AddInstance(typeof(ICondition), _mockCondition);
        _mockContainer.AddInstance(typeof(IPluginInterfaceService), _mockPluginInterfaceService);
        if (extraServices != null)
        {
            foreach (var extraService in extraServices)
            {
                _mockContainer.AddInstance(extraService.Key, extraService.Value);
            }
        }
    }

    internal void InjectMockServices()
    {
        _mockContainer?.Create<IServiceContainer>(_serviceContainer);
    }
}