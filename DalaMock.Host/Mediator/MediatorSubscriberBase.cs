namespace DalaMock.Host.Mediator;

using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

public abstract class MediatorSubscriberBase : IMediatorSubscriber
{
    protected MediatorSubscriberBase(ILogger<MediatorSubscriberBase> logger, MediatorService mediatorService)
    {
        this.Logger = logger;
        this.Logger.LogTrace("Creating {type} ({this})", this.GetType().Name, this);
        this.MediatorService = mediatorService;
    }

    protected ILogger Logger { get; }

    public MediatorService MediatorService { get; }

    protected void UnsubscribeAll()
    {
        this.Logger.LogTrace("Unsubscribing from all for {type} ({this})", this.GetType().Name, this);
        this.MediatorService.UnsubscribeAll(this);
    }
}
