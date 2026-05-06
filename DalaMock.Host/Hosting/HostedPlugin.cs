using System.Threading;
using System.Threading.Tasks;

using Dalamud.Interface.Windowing;

namespace DalaMock.Host.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using DalaMock.Host.Factories;
using DalaMock.Host.LoggingProviders;
using DalaMock.Host.Mediator;
using DalaMock.Shared.Classes;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// A hosted plugin, will automatically inject any services provided. Will handle the startup and shutdown of the plugin.
/// </summary>
public abstract class HostedPlugin : IAsyncDalamudPlugin
{
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IPluginLog pluginLog;
    private readonly HostedEvents hostedEvents;
    private Dictionary<Type, Type> hostedServices;
    private IHost? host;

    /// <summary>
    /// Initializes a new instance of the <see cref="HostedPlugin"/> class.
    /// </summary>
    /// <param name="pluginInterface">The dalamud plugin interface.</param>
    /// <param name="pluginLog">The plugin log.</param>
    public HostedPlugin(IDalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
        this.pluginLog = new DalamudServiceWrapper<IPluginLog>(pluginInterface).Service;
        this.hostedEvents = new HostedEvents();
        this.hostedServices = new Dictionary<Type, Type>();
        this.ReplacementContainer = new DalamudReplacementContainer();
    }

    /// <summary>
    /// Allows configuration of plugin-specific options.
    /// Called before ConfigureServices and ConfigureContainer.
    /// </summary>
    /// <returns></returns>
    public abstract HostedPluginOptions ConfigureOptions();

    /// <summary>
    /// Gets a value indicating whether has the plugin started?.
    /// </summary>
    public bool IsStarted { get; private set; }

    /// <summary>
    /// Gets the options the plugin was started with.
    /// </summary>
    public HostedPluginOptions HostedPluginOptions { get; private set; }

    public HostedEvents HostedEvents => this.hostedEvents;

    /// <summary>
    /// Can the plugin be disposed?.
    /// </summary>
    public virtual void Dispose()
    {
        if (this.IsStarted)
        {
            this.StoppingAsync().GetAwaiter().GetResult();
            this.Stop().GetAwaiter().GetResult();
            this.StoppedAsync().GetAwaiter().GetResult();
        }

        this.Host?.Dispose();
    }

    /// <summary>
    /// Configure the container in this function.
    /// </summary>
    /// <param name="containerBuilder">A container builder provided by autofac.</param>
    public abstract void ConfigureContainer(ContainerBuilder containerBuilder);

    /// <summary>
    /// Configure your hosted services in this function.
    /// </summary>
    /// <param name="serviceCollection">A service collection.</param>
    public abstract void ConfigureServices(IServiceCollection serviceCollection);

    /// <summary>
    /// Register a service that implements IHostedService. You can override a given type with a mock type using <see cref="ReplaceHostedService"/>.
    /// </summary>
    /// <param name="hostedServiceType">The service you want to register.</param>
    public void RegisterHostedService(Type hostedServiceType)
    {
        if (this.hostedServices.ContainsKey(hostedServiceType))
        {
            return;
        }

        this.hostedServices[hostedServiceType] = hostedServiceType;
    }

    /// <summary>
    /// Replace a service that implements IHostedService with a mock version. If the service was not registered with RegisterHostedService this will have no effect.
    /// </summary>
    /// <param name="hostedServiceType">The concrete implementation you want to replace.</param>
    /// <param name="mockServiceType">The mock implementation you want to use.</param>
    public void ReplaceHostedService(Type hostedServiceType, Type mockServiceType)
    {
        this.hostedServices[hostedServiceType] = mockServiceType;
    }

    /// <summary>
    /// A container for all DalaMock provided services.
    /// </summary>
    public virtual IReplacementContainer ReplacementContainer { get; }

    public IHost? Host => this.host;

    /// <summary>
    /// Builds the host and starts the plugin.
    /// </summary>
    /// <returns>A built host.</returns>
    public IHost CreateHost()
    {
        this.HostedPluginOptions = this.ConfigureOptions();
        var hostBuilder = new HostBuilder()
            .UseContentRoot(this.pluginInterface.ConfigDirectory.FullName)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging(lb =>
            {
                lb.ClearProviders();
                lb.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DalamudLoggingProvider>(b =>
                                                 new DalamudLoggingProvider(b.GetRequiredService<IPluginLog>())));
                lb.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureContainer<ContainerBuilder>(this.ReplacementContainer.Register)
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterInstance(this.pluginLog).AsSelf().AsImplementedInterfaces().ExternallyOwned();
                containerBuilder.RegisterInstance(this.HostedEvents).AsSelf();
                containerBuilder.RegisterInstance(this.pluginInterface).As<IDalamudPluginInterface>().AsSelf();
                containerBuilder.RegisterSource(new DalamudServiceRegistrationSource(this.pluginInterface));

                if (this.HostedPluginOptions.UseMediatorService)
                {
                    containerBuilder.RegisterType<MediatorService>().AsImplementedInterfaces().AsSelf().SingleInstance();
                }

                containerBuilder.Register<IUiBuilder>(c =>
                {
                    var pluginInterface = c.Resolve<IDalamudPluginInterface>();
                    return pluginInterface.UiBuilder;
                });
            });
        hostBuilder.ConfigureContainer<ContainerBuilder>(this.ConfigureContainer);
        hostBuilder.ConfigureServices(this.ConfigureServices);
        this.PreBuild(hostBuilder);
        hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            foreach (var service in this.hostedServices)
            {
                containerBuilder.RegisterType(service.Value).AsImplementedInterfaces().AsSelf().SingleInstance();
            }
        });

        this.host = hostBuilder.Build();
        this.HostedEvents.OnPluginEvent(HostedEventType.PluginBuilt);
        this.HostedEvents.OnPluginBuilt();

        return this.host;
    }

    /// <summary>
    /// Override this function if you need to access the host builder while it is building.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    public virtual void PreBuild(IHostBuilder hostBuilder)
    {
    }

    private async Task Start(CancellationToken cancellationToken)
    {
        if (this.Host == null)
        {
            this.pluginLog.Error("You attempted to start the plugin before the container has been built.");
            return;
        }

        try
        {
            await this.Host.StartAsync(cancellationToken);
        }
        catch (Exception startTask)
        {
            this.pluginLog.Error(startTask, "Plugin startup faulted.");
            throw;
        }

        if (this.HostedPluginOptions.UseMediatorService)
        {
            this.Host.Services.GetRequiredService<MediatorService>().Publish(new PluginStartedMessage());
        }

        this.HostedEvents.OnPluginEvent(HostedEventType.PluginStarted);
        this.HostedEvents.OnPluginStarted();

        this.IsStarted = true;
    }

    private async Task Stop()
    {
        if (this.HostedPluginOptions.UseMediatorService)
        {
            this.Host?.Services.GetRequiredService<MediatorService>().Publish(new PluginStoppingMessage());
        }

        this.HostedEvents.OnPluginEvent(HostedEventType.PluginStopping);
        this.HostedEvents.OnPluginStopping();
        if (this.Host != null)
        {
            try
            {
                await this.Host.StopAsync();
            }
            catch (Exception startTask)
            {
                this.pluginLog.Error(startTask, "Plugin stop faulted.");
                throw;
            }
        }

        this.IsStarted = false;
        this.HostedEvents.OnPluginEvent(HostedEventType.PluginStopped);
        this.HostedEvents.OnPluginStopped();
    }

    public async ValueTask DisposeAsync()
    {
        if (this.IsStarted)
        {
            await this.StoppingAsync();
            await this.Stop();
            await this.StoppedAsync();
        }

        this.Host?.Dispose();
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        this.host = this.CreateHost();
        await this.StartingAsync(cancellationToken);
        await this.Start(cancellationToken);
        await this.StartedAsync(cancellationToken);
    }

    public virtual Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual Task StoppingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual Task StoppedAsync()
    {
        return Task.CompletedTask;
    }
}
