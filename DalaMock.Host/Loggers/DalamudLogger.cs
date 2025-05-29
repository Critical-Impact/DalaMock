namespace DalaMock.Host.Loggers;

using System;
using System.Text;

using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

internal sealed class DalamudLogger : ILogger
{
    private readonly string name;
    private readonly IPluginLog pluginLog;

    public DalamudLogger(string name, IPluginLog pluginLog)
    {
        this.name = name;
        this.pluginLog = pluginLog;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        return this.BeginScope(state);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel)) return;


        StringBuilder sb = new();
        sb.Append($"[{this.name}]{{{(int)logLevel}}} {state}: {exception?.Message}");
        if (exception != null)
        {
            sb.AppendLine(exception.StackTrace);
            var innerException = exception?.InnerException;
            while (innerException != null)
            {
                sb.AppendLine($"InnerException {innerException}: {innerException.Message}");
                sb.AppendLine(innerException.StackTrace);
                innerException = innerException.InnerException;
            }
        }

        switch (logLevel)
        {
            case LogLevel.Trace:
                this.pluginLog.Verbose(sb.ToString());
                break;
            case LogLevel.Debug:
                this.pluginLog.Debug(sb.ToString());
                break;
            case LogLevel.Information:
                this.pluginLog.Information(sb.ToString());
                break;
            case LogLevel.Warning:
                this.pluginLog.Warning(sb.ToString());
                break;
            case LogLevel.Error:
                this.pluginLog.Error(sb.ToString());
                break;
            case LogLevel.Critical:
                this.pluginLog.Fatal(sb.ToString());
                break;
        }
    }
}
