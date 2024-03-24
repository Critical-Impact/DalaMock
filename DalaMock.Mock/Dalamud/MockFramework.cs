using System.Diagnostics;
using System.Runtime.InteropServices;
using DalaMock.Extensions;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Serilog;

namespace DalaMock.Dalamud;

public class MockFramework : IDisposable, IFramework
{
    private static readonly Stopwatch StatsStopwatch = new();
    
    private readonly Stopwatch updateStopwatch = new();
    private readonly HitchDetector hitchDetector;

    private readonly object runOnNextTickTaskListSync = new();
    private List<RunOnNextTickTaskBase> runOnNextTickTaskList = new();
    private List<RunOnNextTickTaskBase> runOnNextTickTaskList2 = new();

    private Thread? frameworkUpdateThread;

    public MockFramework()
    {
        this.hitchDetector = new HitchDetector("FrameworkUpdate", 200);
    }

    public void FireUpdate()
    {
        Update?.Invoke(this);
        HandleFrameworkUpdate();
    }

    /// <summary>
    /// A delegate type used during the native Framework::destroy.
    /// </summary>
    /// <param name="framework">The native Framework address.</param>
    /// <returns>A value indicating if the call was successful.</returns>
    public delegate bool OnRealDestroyDelegate(IntPtr framework);

    /// <summary>
    /// A delegate type used during the native Framework::free.
    /// </summary>
    /// <returns>The native Framework address.</returns>
    public delegate IntPtr OnDestroyDelegate();

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate bool OnUpdateDetour(IntPtr framework);

    private delegate IntPtr OnDestroyDetour(); // OnDestroyDelegate

    /// <inheritdoc/>
    public event IFramework.OnUpdateDelegate Update;

    /// <summary>
    /// Gets or sets a value indicating whether the collection of stats is enabled.
    /// </summary>
    public static bool StatsEnabled { get; set; }

    /// <summary>
    /// Gets the stats history mapping.
    /// </summary>
    public static Dictionary<string, List<double>> StatsHistory { get; } = new();

    /// <inheritdoc/>
    public DateTime LastUpdate { get; private set; } = DateTime.MinValue;

    /// <inheritdoc/>
    public DateTime LastUpdateUTC { get; private set; } = DateTime.MinValue;

    public TaskFactory FrameworkThreadTaskFactory { get; }

    /// <inheritdoc/>
    public TimeSpan UpdateDelta { get; private set; } = TimeSpan.Zero;

    /// <inheritdoc/>
    public bool IsInFrameworkUpdateThread => Thread.CurrentThread == this.frameworkUpdateThread;

    /// <inheritdoc/>
    public bool IsFrameworkUnloading { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether to dispatch update events.
    /// </summary>
    internal bool DispatchUpdateEvents { get; set; } = true;

    public TaskFactory GetTaskFactory()
    {
        throw new NotImplementedException();
    }

    public Task DelayTicks(long numTicks, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public Task Run(Action action, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<T> Run<T>(Func<T> action, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task Run(Func<Task> action, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<T> Run<T>(Func<Task<T>> action, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<T> RunOnFrameworkThread<T>(Func<T> func) =>
        this.IsInFrameworkUpdateThread || this.IsFrameworkUnloading ? Task.FromResult(func()) : this.RunOnTick(func);

    /// <inheritdoc/>
    public Task RunOnFrameworkThread(Action action)
    {
        if (this.IsInFrameworkUpdateThread || this.IsFrameworkUnloading)
        {
            try
            {
                action();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
        else
        {
            return this.RunOnTick(action);
        }
    }

    /// <inheritdoc/>
    public Task<T> RunOnFrameworkThread<T>(Func<Task<T>> func) =>
        this.IsInFrameworkUpdateThread || this.IsFrameworkUnloading ? func() : this.RunOnTick(func);

    /// <inheritdoc/>
    public Task RunOnFrameworkThread(Func<Task> func) =>
        this.IsInFrameworkUpdateThread || this.IsFrameworkUnloading ? func() : this.RunOnTick(func);

    /// <inheritdoc/>
    public Task<T> RunOnTick<T>(Func<T> func, TimeSpan delay = default, int delayTicks = default, CancellationToken cancellationToken = default)
    {
        if (this.IsFrameworkUnloading)
        {
            if (delay == default && delayTicks == default)
                return this.RunOnFrameworkThread(func);

            var cts = new CancellationTokenSource();
            cts.Cancel();
            return Task.FromCanceled<T>(cts.Token);
        }

        var tcs = new TaskCompletionSource<T>();
        lock (this.runOnNextTickTaskListSync)
        {
            this.runOnNextTickTaskList.Add(new RunOnNextTickTaskFunc<T>()
            {
                RemainingTicks = delayTicks,
                RunAfterTickCount = Environment.TickCount64 + (long)Math.Ceiling(delay.TotalMilliseconds),
                CancellationToken = cancellationToken,
                TaskCompletionSource = tcs,
                Func = func,
            });
        }

        return tcs.Task;
    }

    /// <inheritdoc/>
    public Task RunOnTick(Action action, TimeSpan delay = default, int delayTicks = default, CancellationToken cancellationToken = default)
    {
        if (this.IsFrameworkUnloading)
        {
            if (delay == default && delayTicks == default)
                return this.RunOnFrameworkThread(action);

            var cts = new CancellationTokenSource();
            cts.Cancel();
            return Task.FromCanceled(cts.Token);
        }

        var tcs = new TaskCompletionSource();
        lock (this.runOnNextTickTaskListSync)
        {
            this.runOnNextTickTaskList.Add(new RunOnNextTickTaskAction()
            {
                RemainingTicks = delayTicks,
                RunAfterTickCount = Environment.TickCount64 + (long)Math.Ceiling(delay.TotalMilliseconds),
                CancellationToken = cancellationToken,
                TaskCompletionSource = tcs,
                Action = action,
            });
        }

        return tcs.Task;
    }

    /// <inheritdoc/>
    public Task<T> RunOnTick<T>(Func<Task<T>> func, TimeSpan delay = default, int delayTicks = default, CancellationToken cancellationToken = default)
    {
        if (this.IsFrameworkUnloading)
        {
            if (delay == default && delayTicks == default)
                return this.RunOnFrameworkThread(func);

            var cts = new CancellationTokenSource();
            cts.Cancel();
            return Task.FromCanceled<T>(cts.Token);
        }

        var tcs = new TaskCompletionSource<Task<T>>();
        lock (this.runOnNextTickTaskListSync)
        {
            this.runOnNextTickTaskList.Add(new RunOnNextTickTaskFunc<Task<T>>()
            {
                RemainingTicks = delayTicks,
                RunAfterTickCount = Environment.TickCount64 + (long)Math.Ceiling(delay.TotalMilliseconds),
                CancellationToken = cancellationToken,
                TaskCompletionSource = tcs,
                Func = func,
            });
        }

        return tcs.Task.ContinueWith(x => x.Result, cancellationToken).Unwrap();
    }

    /// <inheritdoc/>
    public Task RunOnTick(Func<Task> func, TimeSpan delay = default, int delayTicks = default, CancellationToken cancellationToken = default)
    {
        if (this.IsFrameworkUnloading)
        {
            if (delay == default && delayTicks == default)
                return this.RunOnFrameworkThread(func);

            var cts = new CancellationTokenSource();
            cts.Cancel();
            return Task.FromCanceled(cts.Token);
        }

        var tcs = new TaskCompletionSource<Task>();
        lock (this.runOnNextTickTaskListSync)
        {
            this.runOnNextTickTaskList.Add(new RunOnNextTickTaskFunc<Task>()
            {
                RemainingTicks = delayTicks,
                RunAfterTickCount = Environment.TickCount64 + (long)Math.Ceiling(delay.TotalMilliseconds),
                CancellationToken = cancellationToken,
                TaskCompletionSource = tcs,
                Func = func,
            });
        }

        return tcs.Task.ContinueWith(x => x.Result, cancellationToken).Unwrap();
    }

    /// <summary>
    /// Dispose of managed and unmanaged resources.
    /// </summary>
    void IDisposable.Dispose()
    {
        this.updateStopwatch.Reset();
        StatsStopwatch.Reset();
    }


    private void RunPendingTickTasks()
    {
        if (this.runOnNextTickTaskList.Count == 0 && this.runOnNextTickTaskList2.Count == 0)
            return;

        for (var i = 0; i < 2; i++)
        {
            lock (this.runOnNextTickTaskListSync)
                (this.runOnNextTickTaskList, this.runOnNextTickTaskList2) = (this.runOnNextTickTaskList2, this.runOnNextTickTaskList);

            this.runOnNextTickTaskList2.RemoveAll(x => x.Run());
        }
    }

    private bool HandleFrameworkUpdate()
    {
        this.frameworkUpdateThread ??= Thread.CurrentThread;

        this.hitchDetector.Start();

        if (this.DispatchUpdateEvents)
        {
            this.updateStopwatch.Stop();
            this.UpdateDelta = TimeSpan.FromMilliseconds(this.updateStopwatch.ElapsedMilliseconds);
            this.updateStopwatch.Restart();

            this.LastUpdate = DateTime.Now;
            this.LastUpdateUTC = DateTime.UtcNow;

            void AddToStats(string key, double ms)
            {
                if (!StatsHistory.ContainsKey(key))
                    StatsHistory.Add(key, new List<double>());

                StatsHistory[key].Add(ms);

                if (StatsHistory[key].Count > 1000)
                {
                    StatsHistory[key].RemoveRange(0, StatsHistory[key].Count - 1000);
                }
            }

            if (StatsEnabled)
            {
                StatsStopwatch.Restart();
                this.RunPendingTickTasks();
                StatsStopwatch.Stop();

                AddToStats(nameof(this.RunPendingTickTasks), StatsStopwatch.Elapsed.TotalMilliseconds);
            }
            else
            {
                this.RunPendingTickTasks();
            }

            if (StatsEnabled && this.Update != null)
            {
                // Stat Tracking for Framework Updates
                var invokeList = this.Update.GetInvocationList();
                var notUpdated = StatsHistory.Keys.ToList();

                // Individually invoke OnUpdate handlers and time them.
                foreach (var d in invokeList)
                {
                    StatsStopwatch.Restart();
                    try
                    {
                        d.Method.Invoke(d.Target, new object[] { this });
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Exception while dispatching Framework::Update event.");
                    }

                    StatsStopwatch.Stop();

                    var key = $"{d.Target}::{d.Method.Name}";
                    if (notUpdated.Contains(key))
                        notUpdated.Remove(key);

                    AddToStats(key, StatsStopwatch.Elapsed.TotalMilliseconds);
                }

                // Cleanup handlers that are no longer being called
                foreach (var key in notUpdated)
                {
                    if (key == nameof(this.RunPendingTickTasks))
                        continue;

                    if (StatsHistory[key].Count > 0)
                    {
                        StatsHistory[key].RemoveAt(0);
                    }
                    else
                    {
                        StatsHistory.Remove(key);
                    }
                }
            }
            else
            {
                this.Update?.InvokeSafely(this);
            }
        }

        this.hitchDetector.Stop();

        return true;
    }

    private abstract class RunOnNextTickTaskBase
    {
        internal int RemainingTicks { get; set; }

        internal long RunAfterTickCount { get; init; }

        internal CancellationToken CancellationToken { get; init; }

        internal bool Run()
        {
            if (this.CancellationToken.IsCancellationRequested)
            {
                this.CancelImpl();
                return true;
            }

            if (this.RemainingTicks > 0)
                this.RemainingTicks -= 1;
            if (this.RemainingTicks > 0)
                return false;

            if (this.RunAfterTickCount > Environment.TickCount64)
                return false;

            this.RunImpl();

            return true;
        }

        protected abstract void RunImpl();

        protected abstract void CancelImpl();
    }

    private class RunOnNextTickTaskFunc<T> : RunOnNextTickTaskBase
    {
        internal TaskCompletionSource<T> TaskCompletionSource { get; init; }

        internal Func<T> Func { get; init; }

        protected override void RunImpl()
        {
            try
            {
                this.TaskCompletionSource.SetResult(this.Func());
            }
            catch (Exception ex)
            {
                this.TaskCompletionSource.SetException(ex);
            }
        }

        protected override void CancelImpl()
        {
            this.TaskCompletionSource.SetCanceled();
        }
    }

    private class RunOnNextTickTaskAction : RunOnNextTickTaskBase
    {
        internal TaskCompletionSource TaskCompletionSource { get; init; }

        internal Action Action { get; init; }

        protected override void RunImpl()
        {
            try
            {
                this.Action();
                this.TaskCompletionSource.SetResult();
            }
            catch (Exception ex)
            {
                this.TaskCompletionSource.SetException(ex);
            }
        }

        protected override void CancelImpl()
        {
            this.TaskCompletionSource.SetCanceled();
        }
    }
}
