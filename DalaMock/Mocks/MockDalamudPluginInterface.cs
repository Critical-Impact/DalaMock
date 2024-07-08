namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface;
using Dalamud.Plugin;
using Dalamud.Plugin.Internal.Types.Manifest;
using Dalamud.Plugin.Ipc;
using Newtonsoft.Json;

public abstract class MockCallGatePubSubBase
{
    protected MockCallGatePubSubBase(string name)
    {
        this.Name = name;
    }

    public Delegate? Func { get; set; }

    public Delegate? Action { get; set; }

    public string Name { get; }

    /// <summary>
    /// Removes a registered Action from inter-plugin communication.
    /// </summary>
    public void UnregisterAction()
    {
        this.Action = null;
    }

    /// <summary>
    /// Removes a registered Func from inter-plugin communication.
    /// </summary>
    public void UnregisterFunc()
    {
        this.Func = null;
    }

    /// <summary>
    /// Registers an Action for inter-plugin communication.
    /// </summary>
    /// <param name="action">Action to register.</param>
    private protected void RegisterAction(Delegate action)
    {
        this.Action = action;
    }

    /// <summary>
    /// Registers a Func for inter-plugin communication.
    /// </summary>
    /// <param name="func">Func to register.</param>
    private protected void RegisterFunc(Delegate func)
    {
        this.Func = func;
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
    /// <exception cref="Dalamud.Plugin.Ipc.Exceptions.IpcNotReadyError">This is thrown when the IPC publisher has not registered an action for calling yet.</exception>
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

public class MockCallGateProvider<T1, T2, T3, T4, TRet> : MockCallGatePubSubBase,
    ICallGateProvider<T1, T2, T3, T4, TRet>
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

public class MockCallGateProvider<T1, T2, T3, T4, T5, TRet> : MockCallGatePubSubBase,
    ICallGateProvider<T1, T2, T3, T4, T5, TRet>
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

public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, TRet> : MockCallGatePubSubBase,
    ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet>
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

public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> : MockCallGatePubSubBase,
    ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet>
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

public class MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> : MockCallGatePubSubBase,
    ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>
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

public class MockDalamudPluginInterface : IDalamudPluginInterface
{
    private readonly string internalName;

    // private readonly MockProgram _mockProgram;
    private MockUiBuilder mockUiBuilder;
    private PluginConfiguration pluginConfiguration;

    public MockDalamudPluginInterface(
        MockUiBuilder mockUiBuilder,
        FileInfo configFile,
        DirectoryInfo configDirectory,
        string internalName)
    {
        // _mockProgram = mockProgram;
        this.internalName = internalName;
        this.ConfigFile = configFile;
        this.ConfigDirectory = configDirectory;
        this.pluginConfiguration = new PluginConfiguration(configDirectory.FullName);
        this.mockUiBuilder = mockUiBuilder;
    }

    public MockUiBuilder MockUiBuilder => this.mockUiBuilder;

    public event IDalamudPluginInterface.LanguageChangedDelegate? LanguageChanged;

    public event IDalamudPluginInterface.ActivePluginsChangedDelegate? ActivePluginsChanged;

    public PluginLoadReason Reason => PluginLoadReason.Boot;

    public bool IsAutoUpdateComplete => true;

    public string SourceRepository => "";

    public string InternalName => this.internalName;

    public IPluginManifest Manifest { get; }

    public bool IsDev => true;

    public bool IsTesting => false;

    public DateTime LoadTime => DateTime.Now;

    public DateTime LoadTimeUTC => DateTime.Now;

    public TimeSpan LoadTimeDelta => TimeSpan.Zero;

    public DirectoryInfo DalamudAssetDirectory => new("");

    public FileInfo ConfigFile { get; }

    public IUiBuilder UiBuilder => this.mockUiBuilder;

    public bool IsDevMenuOpen => true;

    public bool IsDebugging => true;

    public string UiLanguage => "English";

    public ISanitizer Sanitizer => new Sanitizer(ClientLanguage.English);

    public XivChatType GeneralChatType => XivChatType.Debug;

    public IEnumerable<IExposedPlugin> InstalledPlugins => new List<IExposedPlugin>();

    public bool OpenPluginInstallerTo(
        PluginInstallerOpenKind openTo = PluginInstallerOpenKind.AllPlugins,
        string? searchText = null)
    {
        throw new NotImplementedException();
    }

    public bool OpenDalamudSettingsTo(SettingsOpenKind openTo = SettingsOpenKind.General, string? searchText = null)
    {
        return false;
    }

    public bool OpenDeveloperMenu()
    {
        return false;
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

    public FileInfo AssemblyLocation => new(Environment.ProcessPath);

    public ICallGateProvider<TRet> GetIpcProvider<TRet>(string name)
    {
        return new MockCallGateProvider<TRet>(name);
    }

    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
        return new MockCallGateProvider<T1, TRet>(name);
    }

    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(name);
    }

    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(name);
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

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return null!;
    }

    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8,
        TRet>(string name)
    {
        return null!;
    }

    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
        var serializedConfig = SerializeConfig(currentConfig);
        File.WriteAllText(this.ConfigFile.FullName, serializedConfig);
        if (currentConfig != null)
        {
            this.pluginConfiguration.Save(currentConfig, this.InternalName);
        }
    }

    public IPluginConfiguration? GetPluginConfig()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(IPluginConfiguration)))
            {
                var mi = this.pluginConfiguration.GetType().GetMethod("LoadForType");
                var fn = mi.MakeGenericMethod(type);
                return (IPluginConfiguration)fn.Invoke(this.pluginConfiguration, new object[] { this.InternalName });
            }
        }

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

    internal static string SerializeConfig(object? config)
    {
        return JsonConvert.SerializeObject(
            config,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
    }

    public bool OpenPluginInstaller()
    {
        return false;
    }

    public void Dispose()
    {
    }
}