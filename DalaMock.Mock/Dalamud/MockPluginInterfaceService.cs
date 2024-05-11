using DalaMock.Mock;
using DalaMock.Shared.Interfaces;
using Dalamud;
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

public abstract class MockCallGatePubSubBase
{
    public Delegate? Func { get; set; }
    public Delegate? Action { get; set; }
    public string Name { get; }

    protected MockCallGatePubSubBase(string name)
    {
       Name = name;
    }


    /// <summary>
    /// Removes a registered Action from inter-plugin communication.
    /// </summary>
    public void UnregisterAction()
    {
       Action = null;
    }

    /// <summary>
    /// Removes a registered Func from inter-plugin communication.
    /// </summary>
    public void UnregisterFunc()
    {
       Func = null;
    }

    /// <summary>
    /// Registers an Action for inter-plugin communication.
    /// </summary>
    /// <param name="action">Action to register.</param>
    private protected void RegisterAction(Delegate action)
    {
       Action = action;
    }

    /// <summary>
    /// Registers a Func for inter-plugin communication.
    /// </summary>
    /// <param name="func">Func to register.</param>
    private protected void RegisterFunc(Delegate func)
    {
       Func = func;
    }

    /// <summary>
    /// Subscribe an expression to this registration.
    /// </summary>
    /// <param name="action">Action to subscribe.</param>
    private protected void Subscribe(Delegate action)
    {
       
    }

    /// <summary>
    /// Unsubscribe an expression from this registration.
    /// </summary>
    /// <param name="action">Action to unsubscribe.</param>
    private protected void Unsubscribe(Delegate action)
    {
       
    }

    /// <summary>
    /// Invoke an action registered for inter-plugin communication.
    /// </summary>
    /// <param name="args">Action arguments.</param>
    /// <exception cref="IpcNotReadyError">This is thrown when the IPC publisher has not registered an action for calling yet.</exception>
    private protected void InvokeAction(params object?[]? args)
    {
       
    }



    /// <summary>
    /// Invoke all actions that have subscribed to this IPC.
    /// </summary>
    /// <param name="args">Delegate arguments.</param>
    private protected void SendMessage(params object?[]? args)
    {
       
    }
}

public class MockCallGateProvider<TRet> : MockCallGatePubSubBase, ICallGateProvider<TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage()
   {
      
   }
}
public class MockCallGateProvider<T1, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1)
   {
   }
}
public class MockCallGateProvider<T1, T2, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2)
   {
   }
}
public class MockCallGateProvider<T1, T2, T3, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3)
   {
   }
}
public class MockCallGateProvider<T1, T2, T3, T4, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, T4, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3, T4> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, T4, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
   {
   }
}
public class MockCallGateProvider<T1, T2, T3, T4, T5, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, T4, T5, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3, T4, T5> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, T4, T5, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
   {
   }
}

public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3, T4, T5, T6> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, T4, T5, T6, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
   {
   }
}

public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3, T4, T5, T6, T7> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, T4, T5, T6, T7, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
   {
   }
}


public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> : MockCallGatePubSubBase, ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>
{
   public MockCallGateProvider(string name) : base(name)
   {
   }

   public void RegisterAction(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
   {
      base.RegisterAction(action);
   }

   public void RegisterFunc(Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> func)
   {
      base.RegisterFunc(func);
   }

   public void SendMessage(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
   {
   }
}

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

    public PluginLoadReason Reason => PluginLoadReason.Boot;

    public bool IsAutoUpdateComplete => true;

    public string SourceRepository => "";

    public string InternalName => "";

    public bool IsDev => true;

    public bool IsTesting => false;

    public DateTime LoadTime => DateTime.Now;

    public DateTime LoadTimeUTC => DateTime.Now;

    public TimeSpan LoadTimeDelta => TimeSpan.Zero;

    public DirectoryInfo DalamudAssetDirectory => new DirectoryInfo("");

    public FileInfo ConfigFile { get; }

    public bool IsDevMenuOpen => true;

    public bool IsDebugging => true;

    public string UiLanguage => "English";

    public ISanitizer Sanitizer => new Sanitizer(ClientLanguage.English);

    public XivChatType GeneralChatType => XivChatType.Debug;

    public IEnumerable<InstalledPluginState> InstalledPlugins => new List<InstalledPluginState>();

    public bool OpenPluginInstaller()
    {
       return false!;
    }

    public T GetOrCreateData<T>(string tag, Func<T> dataGenerator) where T : class
    {
       return null!;
    }

    public void RelinquishData(string tag)
    {
    }

    public bool TryGetData<T>(string tag, out T? data) where T : class
    {
       data = null;
       return false;
    }

    public T? GetData<T>(string tag) where T : class
    {
       return null!;
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
       return new MockCallGateProvider<TRet>(name);
    }

    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
       return new MockCallGateProvider<T1,TRet>(name);
    }

    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,T4,TRet>(name);

    }

    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,T4,T5,TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,T4,T5,T6,TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,T4,T5,T6,T7,TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
       return new MockCallGateProvider<T1,T2,T3,T4,T5,T6,T7,T8,TRet>(name);
    }

    public ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(string name)
    {
       return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name)
    {
       return null!;
    }

    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
    }

    public IPluginConfiguration? GetPluginConfig()
    {
       return null!;
    }

    public string GetPluginConfigDirectory()
    {
       return null!;
    }

    public string GetPluginLocDirectory()
    {
       return null!;
    }

    public DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction)
    {
       return null!;
    }

    public void RemoveChatLinkHandler(uint commandId)
    {
    }

    public void RemoveChatLinkHandler()
    {
    }

    public T? Create<T>(params object[] scopedObjects) where T : class
    {
       return null!;
    }

    public bool Inject(object instance, params object[] scopedObjects)
    {
       return false;
    }

    public void Dispose()
    {

    }
}