namespace DalaMock.Host.Mediator;

using Dalamud.Plugin.Services;

public abstract class MediatorSubscriberBase : IMediatorSubscriber
{
    protected MediatorSubscriberBase(IPluginLog logger, MediatorService mediatorService)
    {
        this.Logger = logger;
        this.Logger.Verbose("Creating {type} ({this})", this.GetType().Name, this);
        this.MediatorService = mediatorService;
    }

    protected IPluginLog Logger { get; }

    public MediatorService MediatorService { get; }

    protected void UnsubscribeAll()
    {
        this.Logger.Verbose("Unsubscribing from all for {type} ({this})", this.GetType().Name, this);
        this.MediatorService.UnsubscribeAll(this);
    }
}