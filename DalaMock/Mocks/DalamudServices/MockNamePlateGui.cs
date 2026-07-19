namespace DalaMock.Core.Mocks.DalamudServices;

public class MockNamePlateGui : INamePlateGui, IMockService
{
    private INamePlateGui.OnPlateUpdateDelegate? onDataUpdate;
    private INamePlateGui.OnPlateUpdateDelegate? onNamePlateUpdate;
    private INamePlateGui.OnPlateUpdateDelegate? onPostDataUpdate;
    private INamePlateGui.OnPlateUpdateDelegate? onPostNamePlateUpdate;

    public void RequestRedraw()
    {
        this.RedrawRequestCount++;
    }

    public event INamePlateGui.OnPlateUpdateDelegate? OnNamePlateUpdate
    {
        add => this.onNamePlateUpdate += value;
        remove => this.onNamePlateUpdate -= value;
    }

    public event INamePlateGui.OnPlateUpdateDelegate? OnPostNamePlateUpdate
    {
        add => this.onPostNamePlateUpdate += value;
        remove => this.onPostNamePlateUpdate -= value;
    }

    public event INamePlateGui.OnPlateUpdateDelegate? OnDataUpdate
    {
        add => this.onDataUpdate += value;
        remove => this.onDataUpdate -= value;
    }

    public event INamePlateGui.OnPlateUpdateDelegate? OnPostDataUpdate
    {
        add => this.onPostDataUpdate += value;
        remove => this.onPostDataUpdate -= value;
    }

    public string ServiceName => "Nameplate GUI";

    public int RedrawRequestCount { get; private set; }

    public int NamePlateUpdateSubscriberCount => GetSubscriberCount(this.onNamePlateUpdate);

    public int RaiseNamePlateUpdate(
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        return InvokeSubscribers(this.onNamePlateUpdate, context, handlers);
    }

    public int RaisePostNamePlateUpdate(
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        return InvokeSubscribers(this.onPostNamePlateUpdate, context, handlers);
    }

    public int RaiseDataUpdate(
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        return InvokeSubscribers(this.onDataUpdate, context, handlers);
    }

    public int RaisePostDataUpdate(
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        return InvokeSubscribers(this.onPostDataUpdate, context, handlers);
    }

    private static int GetSubscriberCount(INamePlateGui.OnPlateUpdateDelegate? subscribers)
    {
        return subscribers?.GetInvocationList().Length ?? 0;
    }

    private static int InvokeSubscribers(
        INamePlateGui.OnPlateUpdateDelegate? subscribers,
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        var callbacks = subscribers?.GetInvocationList()
            .Cast<INamePlateGui.OnPlateUpdateDelegate>()
            .ToList() ?? [];

        foreach (var callback in callbacks)
        {
            callback(context, handlers);
        }

        return callbacks.Count;
    }
}
