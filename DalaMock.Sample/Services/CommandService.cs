namespace DalaMock.Sample.Services;

using System.Threading;
using System.Threading.Tasks;

using DalaMock.Host.Mediator;
using DalaMock.Sample.Mediator;
using DalaMock.Sample.Windows;

using Dalamud.Game.Command;
using Dalamud.Plugin.Services;

using Microsoft.Extensions.Hosting;

public class CommandService(ICommandManager commandManager, MediatorService mediatorService) : IHostedService
{
    private readonly MediatorService mediatorService = mediatorService;
    private readonly string[] commandName = { "/sampleplugin", "/samplepluginalias" };

    public ICommandManager CommandManager { get; } = commandManager;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < this.commandName.Length; i++)
        {
            this.CommandManager.AddHandler(
            this.commandName[i],
            new CommandInfo(this.OnCommand)
            {
                HelpMessage = "Shows the sample plugin main window.",
            });
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < this.commandName.Length; i++)
        {
            this.CommandManager.RemoveHandler(this.commandName[i]);
        }

        return Task.CompletedTask;
    }

    private void OnCommand(string command, string arguments)
    {
        this.mediatorService.Publish(new ToggleWindowMessage(typeof(MainWindow)));
    }
}
