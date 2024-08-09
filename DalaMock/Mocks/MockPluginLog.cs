using Microsoft.Extensions.Logging;

namespace DalaMock.Core.Mocks;

using System;
using Dalamud.Plugin.Services;
using Serilog;
using Serilog.Events;

public class MockPluginLog : IPluginLog, IMockService
{
    public ILogger<MockPluginLog> Logger { get; }

    public MockPluginLog(ILogger<MockPluginLog> logger)
    {
        this.Logger = logger;
    }

    public string ServiceName => "Plugin Log";

    public void Fatal(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogCritical(messageTemplate, values);
    }

    public void Fatal(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogCritical(exception, messageTemplate, values);
    }

    public void Error(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogError(messageTemplate, values);
    }

    public void Error(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogError(exception, messageTemplate, values);
    }

    public void Warning(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogWarning(messageTemplate, values);
    }

    public void Warning(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogWarning(exception, messageTemplate, values);
    }

    public void Information(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogInformation(messageTemplate, values);
    }

    public void Information(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogInformation(exception, messageTemplate, values);
    }

    public void Info(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogInformation(messageTemplate, values);
    }

    public void Info(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogInformation(exception, messageTemplate, values);
    }

    public void Debug(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogDebug(messageTemplate, values);
    }

    public void Debug(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogDebug(exception, messageTemplate, values);
    }

    public void Verbose(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogTrace(messageTemplate, values);
    }

    public void Verbose(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.LogTrace(exception, messageTemplate, values);
    }

    public void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        LogLevel logLevel = LogLevel.Trace;

        switch (level)
        {
            case LogEventLevel.Verbose:
                break;
            case LogEventLevel.Debug:
                logLevel = LogLevel.Debug;
                break;
            case LogEventLevel.Information:
                logLevel = LogLevel.Information;
                break;
            case LogEventLevel.Warning:
                logLevel = LogLevel.Warning;
                break;
            case LogEventLevel.Error:
                logLevel = LogLevel.Error;
                break;
            case LogEventLevel.Fatal:
                logLevel = LogLevel.Critical;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }

        this.Logger.Log(logLevel, exception, messageTemplate, values);
    }

    public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Verbose;
}
