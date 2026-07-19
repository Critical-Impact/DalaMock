namespace DalaMock.Core.Mocks.DalamudServices;

public class MockAddonLifecycle : IAddonLifecycle, IMockService
{
    private readonly object syncRoot = new();
    private readonly List<MockAddonLifecycleRegistration> listeners = [];

    public IReadOnlyList<MockAddonLifecycleRegistration> RegisteredListeners
    {
        get
        {
            lock (this.syncRoot)
            {
                return this.listeners.ToList();
            }
        }
    }

    public void RegisterListener(
        AddonEvent eventType,
        IEnumerable<string> addonNames,
        IAddonLifecycle.AddonEventDelegate handler)
    {
        foreach (var addonName in addonNames)
        {
            this.RegisterListener(eventType, addonName, handler);
        }
    }

    public void RegisterListener(AddonEvent eventType, string addonName, IAddonLifecycle.AddonEventDelegate handler)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(addonName);
        ArgumentNullException.ThrowIfNull(handler);

        lock (this.syncRoot)
        {
            this.listeners.Add(new MockAddonLifecycleRegistration(eventType, addonName, handler));
        }
    }

    public void RegisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (this.syncRoot)
        {
            this.listeners.Add(new MockAddonLifecycleRegistration(eventType, null, handler));
        }
    }

    public void UnregisterListener(
        AddonEvent eventType,
        IEnumerable<string> addonNames,
        IAddonLifecycle.AddonEventDelegate? handler = null)
    {
        var names = addonNames.ToHashSet(StringComparer.Ordinal);
        this.RemoveListeners(listener =>
            listener.EventType == eventType &&
            listener.AddonName is not null &&
            names.Contains(listener.AddonName) &&
            MatchesHandler(listener, handler));
    }

    public void UnregisterListener(
        AddonEvent eventType,
        string addonName,
        IAddonLifecycle.AddonEventDelegate? handler = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(addonName);
        this.RemoveListeners(listener =>
            listener.EventType == eventType &&
            string.Equals(listener.AddonName, addonName, StringComparison.Ordinal) &&
            MatchesHandler(listener, handler));
    }

    public void UnregisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate? handler = null)
    {
        this.RemoveListeners(listener =>
            listener.EventType == eventType &&
            MatchesHandler(listener, handler));
    }

    public void UnregisterListener(params IAddonLifecycle.AddonEventDelegate[] handlers)
    {
        var handlerSet = handlers.ToHashSet();
        this.RemoveListeners(listener => handlerSet.Contains(listener.Handler));
    }

    public IntPtr GetOriginalVirtualTable(IntPtr virtualTableAddress)
    {
        return IntPtr.Zero;
    }

    public string ServiceName => "Addon Lifecycle";

    public int Raise(AddonEvent eventType, string addonName)
    {
        return this.Raise(eventType, addonName, CreateGenericAddonArgs());
    }

    public int Raise(AddonEvent eventType, string addonName, AddonArgs args)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(addonName);
        ArgumentNullException.ThrowIfNull(args);

        var targets = this.GetMatchingListeners(eventType, addonName);
        foreach (var target in targets)
        {
            target.Handler(eventType, args);
        }

        return targets.Count;
    }

    private static AddonArgs CreateGenericAddonArgs()
    {
        return (AddonArgs)RuntimeHelpers.GetUninitializedObject(typeof(AddonArgs));
    }

    private static bool MatchesHandler(
        MockAddonLifecycleRegistration listener,
        IAddonLifecycle.AddonEventDelegate? handler)
    {
        return handler is null || listener.Handler == handler;
    }

    private List<MockAddonLifecycleRegistration> GetMatchingListeners(
        AddonEvent eventType,
        string addonName)
    {
        lock (this.syncRoot)
        {
            return this.listeners
                .Where(listener => listener.Matches(eventType, addonName))
                .ToList();
        }
    }

    private void RemoveListeners(Func<MockAddonLifecycleRegistration, bool> predicate)
    {
        lock (this.syncRoot)
        {
            this.listeners.RemoveAll(listener => predicate(listener));
        }
    }
}

public sealed record MockAddonLifecycleRegistration(
    AddonEvent EventType,
    string? AddonName,
    IAddonLifecycle.AddonEventDelegate Handler)
{
    public bool Matches(AddonEvent eventType, string addonName)
    {
        return this.EventType == eventType &&
               (this.AddonName is null ||
                string.Equals(this.AddonName, addonName, StringComparison.Ordinal));
    }
}
