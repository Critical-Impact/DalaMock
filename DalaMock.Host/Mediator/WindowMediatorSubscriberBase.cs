namespace DalaMock.Host.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

public abstract class WindowMediatorSubscriberBase : Window, IMediatorSubscriber, IDisposable
{
    protected WindowMediatorSubscriberBase(
        IPluginLog logger,
        MediatorService mediator,
        string name,
        ImGuiWindowFlags flags = ImGuiWindowFlags.None,
        bool forceMainWindow = false)
        : base(name, flags, forceMainWindow)
    {
        this.Logger = logger;
        this.MediatorService = mediator;
    }

    public IPluginLog Logger { get; set; }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public MediatorService MediatorService { get; set; }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        this.Logger.Debug("Disposing {type}", this.GetType());

        this.MediatorService.UnsubscribeAll(this);
    }
}