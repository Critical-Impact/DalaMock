namespace DalaMock.Host.Mediator;

using System;

using Microsoft.Extensions.Logging;

public abstract class DisposableMediatorSubscriberBase : MediatorSubscriberBase, IDisposable
{
    protected DisposableMediatorSubscriberBase(
        ILogger<DisposableMediatorSubscriberBase> logger,
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
        this.Logger.LogTrace("Disposing {type} ({this})", this.GetType().Name, this);
        this.UnsubscribeAll();
    }
}
