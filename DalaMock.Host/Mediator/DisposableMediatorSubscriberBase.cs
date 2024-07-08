namespace DalaMock.Host.Mediator;

using System;
using Dalamud.Plugin.Services;

public abstract class DisposableMediatorSubscriberBase : MediatorSubscriberBase, IDisposable
{
    protected DisposableMediatorSubscriberBase(
        IPluginLog logger,
        MediatorService
            mediatorService)
        : base(logger, mediatorService)
    {
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        this.Logger.Verbose("Disposing {type} ({this})", this.GetType().Name, this);
        this.UnsubscribeAll();
    }
}