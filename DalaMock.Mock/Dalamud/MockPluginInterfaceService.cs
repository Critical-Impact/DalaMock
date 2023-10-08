using DalaMock.Mock;
using DalaMock.Shared.Interfaces;
using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface.Internal;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;
using StbiSharp;
using Veldrid;

namespace DalaMock.Dalamud;

public class MockPluginInterfaceService : IPluginInterfaceService
{
    private readonly MockProgram _mockProgram;

    public MockPluginInterfaceService(MockProgram mockProgram, FileInfo configFile, DirectoryInfo configDirectory)
    {
        _mockProgram = mockProgram;
        ConfigFile = configFile;
        ConfigDirectory = configDirectory;
    }
    public event Action? Draw;
    public event Action? OpenConfigUi;
    public event Action? OpenMainUi;
    public event DalamudPluginInterface.LanguageChangedDelegate? LanguageChanged;
    public event DalamudPluginInterface.ActivePluginsChangedDelegate? ActivePluginsChanged;

    public PluginLoadReason Reason => throw new NotImplementedException();

    public bool IsAutoUpdateComplete => throw new NotImplementedException();

    public string SourceRepository => throw new NotImplementedException();

    public string InternalName => throw new NotImplementedException();

    public bool IsDev => throw new NotImplementedException();

    public bool IsTesting => throw new NotImplementedException();

    public DateTime LoadTime => throw new NotImplementedException();

    public DateTime LoadTimeUTC => throw new NotImplementedException();

    public TimeSpan LoadTimeDelta => throw new NotImplementedException();

    public DirectoryInfo DalamudAssetDirectory => throw new NotImplementedException();

    public FileInfo ConfigFile { get; }

    public bool IsDevMenuOpen => throw new NotImplementedException();

    public bool IsDebugging => throw new NotImplementedException();

    public string UiLanguage => throw new NotImplementedException();

    public ISanitizer Sanitizer => throw new NotImplementedException();

    public XivChatType GeneralChatType => throw new NotImplementedException();

    public IEnumerable<InstalledPluginState> InstalledPlugins => throw new NotImplementedException();

    public bool OpenPluginInstaller()
    {
        throw new NotImplementedException();
    }

    public T GetOrCreateData<T>(string tag, Func<T> dataGenerator) where T : class
    {
        throw new NotImplementedException();
    }

    public void RelinquishData(string tag)
    {
        throw new NotImplementedException();
    }

    public bool TryGetData<T>(string tag, out T? data) where T : class
    {
        throw new NotImplementedException();
    }

    public T? GetData<T>(string tag) where T : class
    {
        throw new NotImplementedException();
    }

    public DirectoryInfo ConfigDirectory { get; }

    public FileInfo AssemblyLocation
    {
        get
        {
            return new FileInfo(System.Environment.ProcessPath);
        }
    }
    public IDalamudTextureWrap LoadImageRaw(byte[] imageData, int width, int height, int numChannels)
    {
        var texture = _mockProgram.GraphicsDevice.ResourceFactory.CreateTexture(TextureDescription.Texture2D((uint)width, (uint)height, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
        var CPUframeBufferTextureId = _mockProgram.Controller.GetOrCreateImGuiBinding(_mockProgram.GraphicsDevice.ResourceFactory, texture);
        _mockProgram.GraphicsDevice.UpdateTexture(texture, imageData, 0,0,0, (uint)width, (uint)height, 1, 0,0);
        var veldridTextureWrap =
            new MockTextureMap(CPUframeBufferTextureId, width, height);
        return veldridTextureWrap;        
    }

    public IDalamudTextureWrap LoadImage(string filePath)
    {
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var ms = new MemoryStream())
        {
            fs.CopyTo(ms);
            var image = Stbi.LoadFromMemory(ms, 4);
            var texture = _mockProgram.GraphicsDevice.ResourceFactory.CreateTexture(TextureDescription.Texture2D((uint)image.Width,
                (uint)image.Height, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            var CPUframeBufferTextureId =
                _mockProgram.Controller.GetOrCreateImGuiBinding(_mockProgram.GraphicsDevice.ResourceFactory, texture);
            _mockProgram.GraphicsDevice.UpdateTexture(texture, image.Data, 0, 0, 0, (uint)image.Width, (uint)image.Height, 1, 0, 0);
            var veldridTextureWrap =
                new MockTextureMap(CPUframeBufferTextureId, image.Width, image.Height);
            return veldridTextureWrap;        
        }

    }

    public ICallGateProvider<TRet> GetIpcProvider<TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
        throw new NotImplementedException();
    }

    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
        throw new NotImplementedException();
    }

    public IPluginConfiguration? GetPluginConfig()
    {
        throw new NotImplementedException();
    }

    public string GetPluginConfigDirectory()
    {
        throw new NotImplementedException();
    }

    public string GetPluginLocDirectory()
    {
        throw new NotImplementedException();
    }

    public DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction)
    {
        throw new NotImplementedException();
    }

    public void RemoveChatLinkHandler(uint commandId)
    {
        throw new NotImplementedException();
    }

    public void RemoveChatLinkHandler()
    {
        throw new NotImplementedException();
    }

    public T? Create<T>(params object[] scopedObjects) where T : class
    {
        throw new NotImplementedException();
    }

    public bool Inject(object instance, params object[] scopedObjects)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {

    }
}