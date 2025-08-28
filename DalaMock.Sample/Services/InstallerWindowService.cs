namespace DalaMock.Sample.Services;

using System.Threading;
using System.Threading.Tasks;

using DalaMock.Host.Mediator;
using DalaMock.Sample.Mediator;
using DalaMock.Sample.Windows;

using Dalamud.Plugin;

using Microsoft.Extensions.Hosting;

public class InstallerWindowService(
    IDalamudPluginInterface pluginInterface,
    MediatorService mediatorService) : IHostedService
{
    private readonly MediatorService mediatorService = mediatorService;

    public IDalamudPluginInterface PluginInterface { get; } = pluginInterface;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.PluginInterface.UiBuilder.OpenConfigUi += this.ToggleConfigUi;
        this.PluginInterface.UiBuilder.OpenMainUi += this.ToggleMainUi;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.PluginInterface.UiBuilder.OpenConfigUi -= this.ToggleConfigUi;
        this.PluginInterface.UiBuilder.OpenMainUi -= this.ToggleMainUi;
        return Task.CompletedTask;
    }

    private void ToggleMainUi()
    {
        this.mediatorService.Publish(new ToggleWindowMessage(typeof(MainWindow)));
    }

    private void ToggleConfigUi()
    {
        this.mediatorService.Publish(new ToggleWindowMessage(typeof(ConfigWindow)));
    }
}
