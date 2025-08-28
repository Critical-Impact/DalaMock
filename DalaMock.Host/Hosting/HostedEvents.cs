namespace DalaMock.Host.Hosting;

using System;

public enum HostedEventType
{
    PluginBuilt,
    PluginStarted,
    PluginStopping,
    PluginStopped,
}

/// <summary>
/// Plugins can subscribe to these events to perform logic during initialization, start, stop, and other lifecycle transitions.
/// </summary>
public class HostedEvents
{
    /// <summary>
    /// Represents a delegate that is called when a generic plugin event occurs.
    /// </summary>
    /// <param name="eventType">The type of hosted plugin event that occurred.</param>
    public delegate void PluginEventDelegate(HostedEventType eventType);

    /// <summary>
    /// Represents a delegate that is called after the plugin has completed its dependency injection and has been built.
    /// </summary>
    public delegate void PluginBuiltDelegate();

    /// <summary>
    /// Represents a delegate that is called after the plugin has started.
    /// </summary>
    public delegate void PluginStartedDelegate();

    /// <summary>
    /// Represents a delegate that is called when the plugin is about to be stopped.
    /// </summary>
    public delegate void PluginStoppingDelegate();

    /// <summary>
    /// Represents a delegate that is called after the plugin has been stopped.
    /// </summary>
    public delegate void PluginStoppedDelegate();

    /// <summary>
    /// Raised when a generic plugin event occurs.
    /// Use this to react to arbitrary events signaled during plugin hosting.
    /// </summary>
    public event PluginEventDelegate? PluginEvent;

    /// <summary>
    /// Raised after the plugin has completed its build phase (e.g., services have been configured).
    /// </summary>
    public event PluginBuiltDelegate? PluginBuilt;

    /// <summary>
    /// Raised after the plugin has been started.
    /// </summary>
    public event PluginStartedDelegate? PluginStarted;

    /// <summary>
    /// Raised just before the plugin begins stopping.
    /// </summary>
    public event PluginStoppingDelegate? PluginStopping;

    /// <summary>
    /// Raised after the plugin has been fully stopped.
    /// </summary>
    public event PluginStoppedDelegate? PluginStopped;

    /// <summary>
    /// Invokes the <see cref="PluginEvent"/> event with the specified event type.
    /// </summary>
    /// <param name="eventType">The type of event to broadcast to listeners.</param>
    internal virtual void OnPluginEvent(HostedEventType eventType)
    {
        this.PluginEvent?.Invoke(eventType);
    }

    /// <summary>
    /// Invokes the <see cref="PluginBuilt"/> event to notify that the plugin has been built.
    /// </summary>
    internal virtual void OnPluginBuilt()
    {
        this.PluginBuilt?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="PluginStarted"/> event to notify that the plugin has started.
    /// </summary>
    internal virtual void OnPluginStarted()
    {
        this.PluginStarted?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="PluginStopping"/> event to notify that the plugin is beginning the stop process.
    /// </summary>
    internal virtual void OnPluginStopping()
    {
        this.PluginStopping?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="PluginStopped"/> event to notify that the plugin has completed the stop process.
    /// </summary>
    internal virtual void OnPluginStopped()
    {
        this.PluginStopped?.Invoke();
    }
}
