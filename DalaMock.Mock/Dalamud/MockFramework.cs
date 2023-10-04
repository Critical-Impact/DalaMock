using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockFramework : IFramework
{
    public Task<T> RunOnFrameworkThread<T>(Func<T> func)
    {
        throw new NotImplementedException();
    }

    public Task RunOnFrameworkThread(Action action)
    {
        throw new NotImplementedException();
    }

    public Task<T> RunOnFrameworkThread<T>(Func<Task<T>> func)
    {
        throw new NotImplementedException();
    }

    public Task RunOnFrameworkThread(Func<Task> func)
    {
        throw new NotImplementedException();
    }

    public Task<T> RunOnTick<T>(Func<T> func, TimeSpan delay = new TimeSpan(), int delayTicks = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task RunOnTick(Action action, TimeSpan delay = new TimeSpan(), int delayTicks = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<T> RunOnTick<T>(Func<Task<T>> func, TimeSpan delay = new TimeSpan(), int delayTicks = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task RunOnTick(Func<Task> func, TimeSpan delay = new TimeSpan(), int delayTicks = 0,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public DateTime LastUpdate { get; }
    public DateTime LastUpdateUTC { get; }
    public TimeSpan UpdateDelta { get; }
    public bool IsInFrameworkUpdateThread { get; }
    public bool IsFrameworkUnloading { get; }
    public event IFramework.OnUpdateDelegate? Update;
}