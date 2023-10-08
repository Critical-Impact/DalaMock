using DalaMock.Shared.Interfaces;
using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface.Internal;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;

namespace DalaMock.Shared.Classes;

public class PluginInterfaceService : IPluginInterfaceService, IDisposable
{
    private DalamudPluginInterface _dalamudPluginInterface;
    public PluginInterfaceService(DalamudPluginInterface dalamudPluginInterface)
    {
        _dalamudPluginInterface = dalamudPluginInterface;
        _dalamudPluginInterface.UiBuilder.Draw += UiBuilderOnDraw;
        _dalamudPluginInterface.UiBuilder.OpenConfigUi += UiBuilderOnOpenConfigUi;
        _dalamudPluginInterface.UiBuilder.OpenMainUi += UiBuilderOnOpenMainUi;
    }

    private void UiBuilderOnOpenMainUi()
    {
        OpenMainUi?.Invoke();
    }

    private void UiBuilderOnOpenConfigUi()
    {
        OpenConfigUi?.Invoke();
    }

    private void UiBuilderOnDraw()
    {
        Draw?.Invoke();
    }

    public event Action? Draw;
    public event Action? OpenConfigUi;
    public event Action? OpenMainUi;
    public event DalamudPluginInterface.LanguageChangedDelegate? LanguageChanged;
    public event DalamudPluginInterface.ActivePluginsChangedDelegate? ActivePluginsChanged;

    public PluginLoadReason Reason => _dalamudPluginInterface.Reason;

    public bool IsAutoUpdateComplete => _dalamudPluginInterface.IsAutoUpdateComplete;

    public string SourceRepository => _dalamudPluginInterface.SourceRepository;

    public string InternalName => _dalamudPluginInterface.InternalName;

    public bool IsDev => _dalamudPluginInterface.IsDev;

    public bool IsTesting => _dalamudPluginInterface.IsTesting;

    public DateTime LoadTime => _dalamudPluginInterface.LoadTime;

    public DateTime LoadTimeUTC => _dalamudPluginInterface.LoadTimeUTC;

    public TimeSpan LoadTimeDelta => _dalamudPluginInterface.LoadTimeDelta;

    public DirectoryInfo DalamudAssetDirectory => _dalamudPluginInterface.DalamudAssetDirectory;

    public FileInfo ConfigFile
    {
        get
        {
            return _dalamudPluginInterface.ConfigFile;
        }
    }

    public bool IsDevMenuOpen => _dalamudPluginInterface.IsDevMenuOpen;

    public bool IsDebugging => _dalamudPluginInterface.IsDebugging;

    public string UiLanguage => _dalamudPluginInterface.UiLanguage;

    public ISanitizer Sanitizer => _dalamudPluginInterface.Sanitizer;

    public XivChatType GeneralChatType => _dalamudPluginInterface.GeneralChatType;

    public IEnumerable<InstalledPluginState> InstalledPlugins => _dalamudPluginInterface.InstalledPlugins;

    public bool OpenPluginInstaller()
    {
        return _dalamudPluginInterface.OpenPluginInstaller();
    }

    public T GetOrCreateData<T>(string tag, Func<T> dataGenerator) where T : class
    {
        return _dalamudPluginInterface.GetOrCreateData<T>(tag, dataGenerator);
    }

    public void RelinquishData(string tag)
    {
        _dalamudPluginInterface.RelinquishData(tag);
    }

    public bool TryGetData<T>(string tag, out T? data) where T : class
    {
        return _dalamudPluginInterface.TryGetData(tag, out data);
    }

    public T? GetData<T>(string tag) where T : class
    {
        return _dalamudPluginInterface.GetData<T>(tag);
    }

    public DirectoryInfo ConfigDirectory
    {
        get
        {
            return _dalamudPluginInterface.ConfigDirectory;
        }
    }

    public FileInfo AssemblyLocation
    {
        get
        {
            return _dalamudPluginInterface.AssemblyLocation;
        }
    }

    public IDalamudTextureWrap LoadImageRaw(byte[] imageData, int width, int height, int numChannels)
    {
        return _dalamudPluginInterface.UiBuilder.LoadImageRaw(imageData, width, height, numChannels);
    }

    public IDalamudTextureWrap LoadImage(string filePath)
    {
        return _dalamudPluginInterface.UiBuilder.LoadImage(filePath);
    }

    public ICallGateProvider<TRet> GetIpcProvider<TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<TRet>(name);
    }

    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, TRet>(name);
    }

    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, T4, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, T4, T5, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(name);
    }

    public ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<TRet>(name);
    }

    public ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, T4, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(name);
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
        return _dalamudPluginInterface.GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(name);
    }

    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
        _dalamudPluginInterface.SavePluginConfig(currentConfig);
    }

    public IPluginConfiguration? GetPluginConfig()
    {
        return _dalamudPluginInterface.GetPluginConfig();
    }

    public string GetPluginConfigDirectory()
    {
        return _dalamudPluginInterface.GetPluginConfigDirectory();
    }

    public string GetPluginLocDirectory()
    {
        return _dalamudPluginInterface.GetPluginLocDirectory();
    }

    public DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction)
    {
        return _dalamudPluginInterface.AddChatLinkHandler(commandId, commandAction);
    }

    public void RemoveChatLinkHandler(uint commandId)
    {
        _dalamudPluginInterface.RemoveChatLinkHandler(commandId);
    }

    public void RemoveChatLinkHandler()
    {
        _dalamudPluginInterface.RemoveChatLinkHandler();
    }

    public T? Create<T>(params object[] scopedObjects) where T : class
    {
        return _dalamudPluginInterface.Create<T>(scopedObjects);
    }

    public bool Inject(object instance, params object[] scopedObjects)
    {
        return _dalamudPluginInterface.Inject(instance, scopedObjects);
    }


    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed && disposing)
        {
            _dalamudPluginInterface.UiBuilder.Draw -= UiBuilderOnDraw;
            _dalamudPluginInterface.UiBuilder.OpenConfigUi -= UiBuilderOnOpenConfigUi;
            _dalamudPluginInterface.UiBuilder.OpenMainUi -= UiBuilderOnOpenMainUi;
        }
        _disposed = true;         
    }
}