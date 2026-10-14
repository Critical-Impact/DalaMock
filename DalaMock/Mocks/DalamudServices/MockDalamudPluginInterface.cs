namespace DalaMock.Core.Mocks.DalamudServices;

public class MockDalamudPluginInterface : IDalamudPluginInterface, IDisposable
{
    private readonly IUiBuilder uiBuilder;
    private readonly IComponentContext componentContext;
    private readonly MockDalamudVersionInfo mockDalamudVersionInfo;
    private readonly MockDataShare dataShare;
    private readonly ILogger<MockDalamudPluginInterface> logger;
    private readonly MockPlugin mockPlugin;

    // private readonly MockProgram _mockProgram;
    private readonly IPluginManifest pluginManifest;
    private readonly DirectoryInfo pluginConfigsDirectory;
    private PluginConfiguration pluginConfiguration;
    private PluginLoadReason pluginLoadReason;
    private readonly Guid pluginGuid;

    public MockDalamudPluginInterface(
        IUiBuilder uiBuilder,
        PluginLoadSettings pluginLoadSettings,
        IPluginManifest pluginManifest,
        IComponentContext componentContext,
        MockDalamudVersionInfo mockDalamudVersionInfo,
        MockDataShare dataShare,
        ILogger<MockDalamudPluginInterface> logger,
        MockPlugin mockPlugin,
        MockDalamudConfiguration mockDalamudConfiguration)
    {
        // _mockProgram = mockProgram;
        this.uiBuilder = uiBuilder;
        this.componentContext = componentContext;
        this.mockDalamudVersionInfo = mockDalamudVersionInfo;
        this.dataShare = dataShare;
        this.logger = logger;
        this.mockPlugin = mockPlugin;
        this.ConfigFile = pluginLoadSettings.ConfigFile;
        this.pluginConfigsDirectory = pluginLoadSettings.ConfigDir;
        this.pluginLoadReason = pluginLoadSettings.PluginLoadReason;
        this.LoadTime = DateTime.Now;
        this.LoadTimeUTC = DateTime.UtcNow;
        this.UiLanguage = mockDalamudConfiguration.EffectiveLanguage;
        this.Sanitizer = new Sanitizer(mockDalamudConfiguration.ClientLanguage);
        this.DalamudAssetDirectory = new DirectoryInfo(pluginLoadSettings.DalamudAssetDirectory ?? Path.Combine(AppContext.BaseDirectory, "dalamudAssets"));
        this.Assembly = mockPlugin.PluginType.Assembly;

        var assemblyLocation = pluginLoadSettings.AssemblyLocation;
        if (string.IsNullOrEmpty(assemblyLocation))
        {
            assemblyLocation = this.Assembly.Location;
        }

        if (string.IsNullOrEmpty(assemblyLocation))
        {
            assemblyLocation = Environment.ProcessPath!;
        }

        this.AssemblyLocation = new FileInfo(assemblyLocation);
        this.pluginConfiguration = new PluginConfiguration(pluginLoadSettings.ConfigDir.FullName);
        this.pluginManifest = pluginManifest;
        //TODO: Move this GUID into a LocalPluginManifest record
        this.pluginGuid = Guid.NewGuid();
    }

    /// <inheritdoc/>
    public event IDalamudPluginInterface.LanguageChangedDelegate? LanguageChanged;

    /// <inheritdoc/>
    public event IDalamudPluginInterface.ActivePluginsChangedDelegate? ActivePluginsChanged;

    /// <inheritdoc/>
    public Task InjectAsync(object instance, params object[] scopedObjects)
    {
        this.Inject(instance, scopedObjects);
        return Task.CompletedTask;
    }

    public Task<PluginUpdate?> CheckForUpdateAsync()
    {
        return Task.FromResult((PluginUpdate?)null);
    }

    /// <inheritdoc/>
    public PluginLoadReason Reason => this.pluginLoadReason;

    /// <inheritdoc/>
    public bool IsAutoUpdateComplete => true;

    /// <inheritdoc/>
    public string SourceRepository => "DevPlugin";

    /// <inheritdoc/>
    public string InternalName => this.pluginManifest.InternalName;

    /// <inheritdoc/>
    public Guid WorkingPluginId => this.pluginGuid;

    /// <inheritdoc/>
    public IPluginManifest Manifest => this.pluginManifest;

    /// <inheritdoc/>
    public bool IsDev => true;

    /// <inheritdoc/>
    public bool IsTesting => false;

    public bool IsInProfile => false;

    /// <inheritdoc/>
    public DateTime LoadTime { get; }

    /// <inheritdoc/>
    public DateTime LoadTimeUTC { get; }

    /// <inheritdoc/>
    public TimeSpan LoadTimeDelta => DateTime.Now - this.LoadTime;

    /// <inheritdoc/>
    public DirectoryInfo DalamudAssetDirectory { get; }

    /// <inheritdoc/>
    public FileInfo ConfigFile { get; }

    /// <inheritdoc/>
    public IUiBuilder UiBuilder => this.uiBuilder;

    /// <inheritdoc/>
    public bool IsDevMenuOpen => true;

    /// <inheritdoc/>
    public bool IsDebugging => Debugger.IsAttached;

    public bool AllowSeasonalEvents => true;

    /// <inheritdoc/>
    public string UiLanguage { get; private set; }

    /// <inheritdoc/>
    public ISanitizer Sanitizer { get; }

    /// <inheritdoc/>
    public XivChatType GeneralChatType => XivChatType.Debug;

    /// <inheritdoc/>
    public IEnumerable<IExposedPlugin> InstalledPlugins => new List<IExposedPlugin>();

    /// <inheritdoc/>
    public bool OpenPluginInstallerTo(
        PluginInstallerOpenKind openTo = PluginInstallerOpenKind.AllPlugins,
        string? searchText = null)
    {
        return false;
    }

    /// <inheritdoc/>
    public bool OpenDalamudSettingsTo(SettingsOpenKind openTo = SettingsOpenKind.General, string? searchText = null)
    {
        return false;
    }

    /// <inheritdoc/>
    public bool OpenDeveloperMenu()
    {
        return false;
    }

    public IExposedPlugin? GetPlugin(Assembly assembly)
    {
        return null;
    }

    public IExposedPlugin? GetPlugin(AssemblyLoadContext context)
    {
        return null;
    }

    public IDalamudVersionInfo GetDalamudVersion()
    {
        return this.mockDalamudVersionInfo;
    }

    /// <inheritdoc/>
    public T GetOrCreateData<T>(string tag, Func<T> dataGenerator)
        where T : class
    {
        return this.dataShare.GetOrCreateData(tag, new DataCachePluginId(this.InternalName, this.pluginGuid), dataGenerator);
    }

    /// <inheritdoc/>
    public void RelinquishData(string tag)
    {

        this.dataShare.RelinquishData(tag, new DataCachePluginId(this.InternalName, this.pluginGuid));
    }

    /// <inheritdoc/>
    public bool TryGetData<T>(string tag, out T? data)
        where T : class
    {
        return this.dataShare.TryGetData(tag, new DataCachePluginId(this.InternalName, this.pluginGuid), out data);
    }

    /// <inheritdoc/>
    public T? GetData<T>(string tag)
        where T : class
    {
        return this.dataShare.GetData<T>(tag, new DataCachePluginId(this.InternalName, this.pluginGuid));
    }

    /// <inheritdoc/>
    public DirectoryInfo ConfigDirectory => new(this.GetPluginConfigDirectory());

    /// <inheritdoc/>
    public Assembly Assembly { get; }

    /// <inheritdoc/>
    public FileInfo AssemblyLocation { get; }

    /// <inheritdoc/>
    public ICallGateProvider<TRet> GetIpcProvider<TRet>(string name)
    {
        return new MockCallGateProvider<TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, TRet> GetIpcProvider<T1, TRet>(string name)
    {
        return new MockCallGateProvider<T1, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, TRet> GetIpcProvider<T1, T2, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, TRet> GetIpcProvider<T1, T2, T3, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, T4, TRet> GetIpcProvider<T1, T2, T3, T4, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, T4, T5, TRet> GetIpcProvider<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, T4, T5, T6, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(
        string name)
    {
        return new MockCallGateProvider<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(name);
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<TRet> GetIpcSubscriber<TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, TRet> GetIpcSubscriber<T1, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, TRet> GetIpcSubscriber<T1, T2, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, TRet> GetIpcSubscriber<T1, T2, T3, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, T4, TRet> GetIpcSubscriber<T1, T2, T3, T4, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, T4, T5, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, TRet>(
        string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public ICallGateSubscriber<T1, T2, T3, T4, T5, T6, T7, T8, TRet> GetIpcSubscriber<T1, T2, T3, T4, T5, T6, T7, T8,
        TRet>(string name)
    {
        return null!;
    }

    /// <inheritdoc/>
    public void SavePluginConfig(IPluginConfiguration? currentConfig)
    {
        if (currentConfig == null)
        {
            return;
        }

        var serializedConfig = SerializeConfig(currentConfig);
        File.WriteAllText(this.ConfigFile.FullName, serializedConfig);
    }

    /// <inheritdoc/>
    public IPluginConfiguration? GetPluginConfig()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(IPluginConfiguration)))
            {
                var mi = this.pluginConfiguration.GetType().GetMethod("LoadForType");
                var fn = mi!.MakeGenericMethod(type);
                return (IPluginConfiguration)fn.Invoke(this.pluginConfiguration, new object[] { this.InternalName });
            }
        }

        return null!;
    }

    /// <inheritdoc/>
    public string GetPluginConfigDirectory()
    {
        return this.GetOrCreateConfigDirectory(this.InternalName);
    }

    /// <inheritdoc/>
    public string GetPluginLocDirectory()
    {
        return this.GetOrCreateConfigDirectory(Path.Combine(this.InternalName, "loc"));
    }

    /// <inheritdoc/>
    public DalamudLinkPayload AddChatLinkHandler(uint commandId, Action<uint, SeString> commandAction)
    {
        return null!;
    }

    /// <inheritdoc/>
    public void RemoveChatLinkHandler(uint commandId)
    {
    }

    /// <inheritdoc/>
    public void RemoveChatLinkHandler()
    {
    }

    /// <inheritdoc/>
    public T? Create<T>(params object[] scopedObjects)
        where T : class
    {
        var mockServicesDict = this.BuildServiceMap(scopedObjects);

        // Get the type of the class to instantiate
        var typeToCreate = typeof(T);
        T? newInstance = null;

        // Try to find the most suitable constructor
        foreach (var constructor in typeToCreate.GetConstructors().OrderByDescending(c => c.GetParameters().Length))
        {
            var parameters = constructor.GetParameters();
            var parameterValues = new List<object>();

            foreach (var parameter in parameters)
            {
                // Try to resolve each parameter from the mock services
                if (mockServicesDict.TryGetValue(parameter.ParameterType, out var mockService))
                {
                    parameterValues.Add(mockService);
                }
                else
                {
                    parameterValues = null;
                    break;
                }
            }

            if (parameterValues != null)
            {
                newInstance = (T)constructor.Invoke(parameterValues.ToArray());
                break;
            }
        }

        if (newInstance == null)
        {
            // If no suitable constructor is found, return null
            this.logger.LogError($"No suitable constructor found for type '{typeToCreate.FullName}' that can be resolved with the provided mock services.");
            return null;
        }

        var properties = newInstance.GetType().GetProperties()
            .Where(prop => prop.PropertyType.IsPublic && Attribute.IsDefined(prop, typeof(PluginServiceAttribute)) && prop.CanWrite && prop.CanRead);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (mockServicesDict.TryGetValue(propertyType, out var mockService))
            {
                property.SetValue(newInstance, mockService);
            }
            else
            {
                this.logger.LogError($"No matching mock service found for property '{property.Name}' of type '{propertyType.FullName}' in object of type '{newInstance.GetType().FullName}'.");
            }
        }

        return newInstance;
    }

    /// <inheritdoc/>
    public Task<T> CreateAsync<T>(params object[] scopedObjects)
        where T : class
    {
        var instance = this.Create<T>(scopedObjects);
        if (instance == null)
        {
            return Task.FromException<T>(new InvalidOperationException(
                $"Could not create an instance of '{typeof(T).FullName}' with the provided mock services."));
        }

        return Task.FromResult(instance);
    }

    /// <inheritdoc/>
    public bool Inject(object instance, params object[] scopedObjects)
    {
        var mockServicesDict = this.BuildServiceMap(scopedObjects);

        var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => Attribute.IsDefined(prop, typeof(PluginServiceAttribute)) && prop.CanWrite && prop.CanRead);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (mockServicesDict.TryGetValue(propertyType, out var mockService))
            {
                property.SetValue(instance, mockService);
            }
            else
            {
                this.logger.LogError($"No matching mock service found for property '{property.Name}' of type '{propertyType.FullName}' in object of type '{instance.GetType().FullName}'.");
            }
        }

        return true;
    }

    private string GetOrCreateConfigDirectory(string relativePath)
    {
        try
        {
            var path = new DirectoryInfo(Path.Combine(this.pluginConfigsDirectory.FullName, relativePath));
            if (!path.Exists)
            {
                path.Create();
            }

            return path.FullName;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, $"Could not create the config directory '{relativePath}'.");
            return string.Empty;
        }
    }

    internal static string SerializeConfig(object? config)
    {
        return JsonConvert.SerializeObject(
            config,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });
    }

    public bool OpenPluginInstaller()
    {
        return false;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    public virtual void OnLanguageChanged(string langcode)
    {
        this.UiLanguage = langcode;
        this.LanguageChanged?.Invoke(langcode);
    }

    public virtual void OnActivePluginsChanged(PluginListInvalidationKind kind, bool affectedthisplugin)
    {
        this.ActivePluginsChanged?.Invoke(new MockActivePluginsChangedEventArgs(kind, []));
    }

    public object? GetService(Type serviceType)
    {
        return this.componentContext.ResolveOptional(serviceType);
    }

    private Dictionary<Type, object> BuildServiceMap(object[] scopedObjects)
    {
        Dictionary<Type, object> serviceMap = new();
        foreach (var scopedObject in scopedObjects)
        {
            serviceMap.TryAdd(scopedObject.GetType(), scopedObject);
            foreach (var interfaceType in scopedObject.GetType().GetInterfaces())
            {
                serviceMap.TryAdd(interfaceType, scopedObject);
            }
        }

        serviceMap.TryAdd(typeof(IDalamudPluginInterface), this);

        foreach (var mockService in this.componentContext.Resolve<IEnumerable<IMockService>>())
        {
            foreach (var interfaceType in mockService.GetType().GetInterfaces())
            {
                serviceMap.TryAdd(interfaceType, mockService);
            }
        }

        return serviceMap;
    }
}
