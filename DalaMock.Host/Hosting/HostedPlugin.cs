using DalaMock.Host.LoggingProviders;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DalaMock.Host.Hosting;

using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DalaMock.Host.Factories;
using DalaMock.Shared.Classes;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// A hosted plugin, will automatically inject any services provided. Will handle the startup and shutdown of the plugin.
/// </summary>
public abstract class HostedPlugin : IDalamudPlugin
{
    private readonly List<object> interfaces;
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IPluginLog pluginLog;
    private IHost? host;

    /// <summary>
    /// Initializes a new instance of the <see cref="HostedPlugin"/> class.
    /// </summary>
    /// <param name="pluginInterface">The dalamud plugin interface.</param>
    /// <param name="pluginLog">The plugin log.</param>
    /// <param name="interfaces">A list of dalamud services you want to inject into the container.</param>
    public HostedPlugin(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog, params object[] interfaces)
    {
        this.pluginInterface = pluginInterface;
        this.pluginLog = pluginLog;
        this.interfaces = new List<object>();
        foreach (var potentialInterface in interfaces)
        {
            if (potentialInterface.GetType().GetInterfaces().Length != 0)
            {
                this.interfaces.Add(potentialInterface);
            }
            else
            {
                pluginLog.Error(
                    $"Object {potentialInterface.GetType()} was passed into HostedPlugin but does not implement any interfaces.");
            }
        }
    }

    /// <summary>
    /// Has the plugin started?
    /// </summary>
    public bool IsStarted { get; private set; }

    /// <summary>
    /// Can the plugin be disposed?
    /// </summary>
    public void Dispose()
    {
        if (this.IsStarted)
        {
            this.Stop();
        }

        this.host?.Dispose();
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
    /// Builds the host and starts the plugin.
    /// </summary>
    /// <returns>A built host.</returns>
    public IHost CreateHost()
    {
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
            .ConfigureContainer<ContainerBuilder>(collection =>
            {
                this.interfaces.Add(this.pluginLog);

                collection.RegisterInstance(this.pluginInterface).As<IDalamudPluginInterface>().AsSelf();
                collection.RegisterType<DalamudWindowSystem>().As<IWindowSystem>();
                collection.RegisterType<WindowSystemFactory>().As<IWindowSystemFactory>().AsSelf().SingleInstance();
                foreach (var potentialInterface in this.interfaces)
                {
                    var registrationBuilder = collection.RegisterInstance(potentialInterface).AsSelf();
                    var serviceInterfaces = potentialInterface.GetType().GetInterfaces();
                    if (serviceInterfaces.Length != 0)
                    {
                        registrationBuilder.As(serviceInterfaces);
                    }
                }

                collection.Register<IUiBuilder>(c =>
                {
                    var pluginInterface = c.Resolve<IDalamudPluginInterface>();
                    return pluginInterface.UiBuilder;
                });
            });
        hostBuilder.ConfigureContainer<ContainerBuilder>(this.ConfigureContainer);
        hostBuilder.ConfigureServices(this.ConfigureServices);
        this.PreBuild(hostBuilder);

        this.host = hostBuilder.Build();

        return this.host;
    }

    /// <summary>
    /// Override this function if you need to access the host builder while it is building.
    /// </summary>
    /// <param name="hostBuilder">The host builder.</param>
    public virtual void PreBuild(IHostBuilder hostBuilder)
    {
    }

    public void Start()
    {
        if (this.host == null)
        {
            this.pluginLog.Error("You attempted to start the plugin before the container has been built.");
            return;
        }

        var startTask = this.host.StartAsync();
        if (startTask.IsFaulted)
        {
            this.pluginLog.Error(startTask.Exception, "Plugin startup faulted.");
            throw startTask.Exception;
        }

        this.IsStarted = true;
    }

    public void Stop()
    {
        this.host?.StopAsync().GetAwaiter().GetResult();
        this.IsStarted = false;
    }
}
