namespace DalaMock.Sample.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using DalaMock.Host.Hosting;

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

public class ConfigurationService : HostingAwareService
{
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IPluginLog pluginLog;
    private readonly IFramework framework;
    private Configuration? configuration;

    public ConfigurationService(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog, IFramework framework, HostedEvents hostedEvents)
        : base(hostedEvents)
    {
        this.pluginInterface = pluginInterface;
        this.pluginLog = pluginLog;
        this.framework = framework;
        var configPath = pluginInterface.GetPluginConfigDirectory();
        pluginLog.Info("Configuration service loaded.");
    }

    /// <summary>
    /// Get the configuration of the plugin.
    /// </summary>
    /// <returns>The configuration either loaded from the file system or a new instance of the configuration.</returns>
    public Configuration GetConfiguration()
    {
        if (this.configuration == null)
        {
            try
            {
                this.configuration = this.pluginInterface.GetPluginConfig() as Configuration ??
                                     new Configuration();
            }
            catch (Exception e)
            {
                this.pluginLog.Error(e, "Failed to load configuration");
                this.configuration = new Configuration();
            }
        }

        return this.configuration;
    }

    /// <summary>
    /// Request a manual save of the configuration.
    /// </summary>
    public void Save()
    {
        this.GetConfiguration().IsDirty = false;
        this.pluginInterface.SavePluginConfig(this.GetConfiguration());
    }

    /// <summary>
    /// Automatically saves the plugin once it start's stopping and handles autosaving the configuration when the plugin has started or is stopping.
    /// </summary>
    /// <param name="eventType"></param>
    public override void OnPluginEvent(HostedEventType eventType)
    {
        if (eventType == HostedEventType.PluginStopping)
        {
            this.Save();
        }

        if (eventType == HostedEventType.PluginStarted)
        {
            this.framework.Update += this.HandleAutosave;
        }

        if (eventType == HostedEventType.PluginStopping)
        {
            this.framework.Update -= this.HandleAutosave;
        }
    }

    private void HandleAutosave(IFramework framework1)
    {
        if (this.configuration?.IsDirty ?? false)
        {
            this.pluginLog.Verbose("Configuration is dirty, saving.");
            this.Save();
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this.framework.Update -= this.HandleAutosave;
        return Task.CompletedTask;
    }
}
