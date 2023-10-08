using System.Diagnostics.CodeAnalysis;
using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Plugin;
using Dalamud.Plugin.Internal.Types.Manifest;
using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Ipc.Exceptions;

namespace DalaMock.Shared.Interfaces;

public interface IPluginInterfaceService
{
    public event Action? Draw;

    public event Action OpenConfigUi;
    
    public event Action OpenMainUi;
    
    /// <summary>
    /// Event that gets fired when loc is changed
    /// </summary>
    event DalamudPluginInterface.LanguageChangedDelegate LanguageChanged;

    /// <summary>
    /// Event that is fired when the active list of plugins is changed.
    /// </summary>
    event DalamudPluginInterface.ActivePluginsChangedDelegate ActivePluginsChanged;

    /// <summary>
    /// Gets the reason this plugin was loaded.
    /// </summary>
    PluginLoadReason Reason { get; }

    /// <summary>
    /// Gets a value indicating whether or not auto-updates have already completed this session.
    /// </summary>
    bool IsAutoUpdateComplete { get; }

    /// <summary>
    /// Gets the repository from which this plugin was installed.
    ///
    /// If a plugin was installed from the official/main repository, this will return the value of
    /// <see cref="SpecialPluginSource.MainRepo"/>. Developer plugins will return the value of
    /// <see cref="SpecialPluginSource.DevPlugin"/>.
    /// </summary>
    string SourceRepository { get; }

    /// <summary>
    /// Gets the current internal plugin name.
    /// </summary>
    string InternalName { get; }

    /// <summary>
    /// Gets a value indicating whether this is a dev plugin.
    /// </summary>
    bool IsDev { get; }

    /// <summary>
    /// Gets a value indicating whether this is a testing release of a plugin.
    /// </summary>
    /// <remarks>
    /// Dev plugins have undefined behavior for this value, but can be expected to return <c>false</c>.
    /// </remarks>
    bool IsTesting { get; }

    /// <summary>
    /// Gets the time that this plugin was loaded.
    /// </summary>
    DateTime LoadTime { get; }

    /// <summary>
    /// Gets the UTC time that this plugin was loaded.
    /// </summary>
    DateTime LoadTimeUTC { get; }

    /// <summary>
    /// Gets the timespan delta from when this plugin was loaded.
    /// </summary>
    TimeSpan LoadTimeDelta { get; }

    /// <summary>
    /// Gets the directory Dalamud assets are stored in.
    /// </summary>
    DirectoryInfo DalamudAssetDirectory { get; }

    /// <summary>
    /// Gets the location of your plugin assembly.
    /// </summary>
    FileInfo AssemblyLocation { get; }

    /// <summary>
    /// Gets the directory your plugin configurations are stored in.
    /// </summary>
    DirectoryInfo ConfigDirectory { get; }

    /// <summary>
    /// Gets the config file of your plugin.
    /// </summary>
    FileInfo ConfigFile { get; }

    /// <summary>
    /// Gets a value indicating whether Dalamud is running in Debug mode or the /xldev menu is open. This can occur on release builds.
    /// </summary>
    bool IsDevMenuOpen { get; }

    /// <summary>
    /// Gets a value indicating whether a debugger is attached.
    /// </summary>
    bool IsDebugging { get; }

    /// <summary>
    /// Gets the current UI language in two-letter iso format.
    /// </summary>
    string UiLanguage { get; }

    /// <summary>
    /// Gets serializer class with functions to remove special characters from strings.
    /// </summary>
    ISanitizer Sanitizer { get; }

    /// <summary>
    /// Gets the chat type used by default for plugin messages.
    /// </summary>
    XivChatType GeneralChatType { get; }

    /// <summary>
    /// Gets a list of installed plugins along with their current state.
    /// </summary>
    IEnumerable<InstalledPluginState> InstalledPlugins { get; }

    /// <summary>
    /// Opens the <see cref="PluginInstallerWindow"/> with the plugin name set as search target.
    /// </summary>
    /// <returns>Returns false if the DalamudInterface was null.</returns>
    bool OpenPluginInstaller();

    /// <inheritdoc cref="DataShare.GetOrCreateData{T}"/>
    T GetOrCreateData<T>(string tag, Func<T> dataGenerator) where T : class;

    /// <inheritdoc cref="DataShare.RelinquishData"/>
    void RelinquishData(string tag);

    /// <inheritdoc cref="DataShare.TryGetData{T}"/>
    bool TryGetData<T>(string tag, [NotNullWhen(true)] out T? data) where T : class;

    /// <inheritdoc cref="DataShare.GetData{T}"/>
    T? GetData<T>(string tag) where T : class;

    /// <summary>
    /// Gets an IPC provider.
    /// </summary>
    /// <typeparam name="TRet">The return type for funcs. Use object if this is unused.</typeparam>
    /// <param name="name">The name of the IPC registration.</param>
    /// <returns>An IPC provider.</returns>
    /// <exception cref="IpcTypeMismatchError">This is thrown when the requested types do not match the previously registered types are different.</exception>
    ICallGateProvider<TRet> GetIpcProvider<TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(string name);

    /// <inheritdoc cref="ICallGateProvider{TRet}"/>
    ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name);

    /// <summary>
    /// Gets an IPC subscriber.
    /// </summary>
    /// <typeparam name="TRet">The return type for funcs. Use object if this is unused.</typeparam>
    /// <param name="name">The name of the IPC registration.</param>
    /// <returns>An IPC subscriber.</returns>
    ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(string name);

    /// <inheritdoc cref="ICallGateSubscriber{TRet}"/>
    ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(string name);

    /// <summary>
    /// Save a plugin configuration(inheriting IPluginConfiguration).
    /// </summary>
    /// <param name="currentConfig">The current configuration.</param>
    void SavePluginConfig(IPluginConfiguration? currentConfig);

    /// <summary>
    /// Get a previously saved plugin configuration or null if none was saved before.
    /// </summary>
    /// <returns>A previously saved config or null if none was saved before.</returns>
    IPluginConfiguration? GetPluginConfig();

    /// <summary>
    /// Get the config directory.
    /// </summary>
    /// <returns>directory with path of AppData/XIVLauncher/pluginConfig/PluginInternalName.</returns>
    string GetPluginConfigDirectory();

    /// <summary>
    /// Get the loc directory.
    /// </summary>
    /// <returns>directory with path of AppData/XIVLauncher/pluginConfig/PluginInternalName/loc.</returns>
    string GetPluginLocDirectory();

    /// <summary>
    /// Register a chat link handler.
    /// </summary>
    /// <param name="commandId">The ID of the command.</param>
    /// <param name="commandAction">The action to be executed.</param>
    /// <returns>Returns an SeString payload for the link.</returns>
    DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction);

    /// <summary>
    /// Remove a chat link handler.
    /// </summary>
    /// <param name="commandId">The ID of the command.</param>
    void RemoveChatLinkHandler(uint commandId);

    /// <summary>
    /// Removes all chat link handlers registered by the plugin.
    /// </summary>
    void RemoveChatLinkHandler();

    /// <summary>
    /// Create a new object of the provided type using its default constructor, then inject objects and properties.
    /// </summary>
    /// <param name="scopedObjects">Objects to inject additionally.</param>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <returns>The created and initialized type.</returns>
    T? Create<T>(params object[] scopedObjects) where T : class;

    /// <summary>
    /// Inject services into properties on the provided object instance.
    /// </summary>
    /// <param name="instance">The instance to inject services into.</param>
    /// <param name="scopedObjects">Objects to inject additionally.</param>
    /// <returns>Whether or not the injection succeeded.</returns>
    bool Inject(object instance, params object[] scopedObjects);

    /// <summary>
    /// Obsolete implicit dispose implementation. Should not be used.
    /// </summary>
    void Dispose();
}