namespace DalaMock.Sample.Services;

using System;

using DalaMock.Core.Mocks;
using Dalamud.Plugin.Services;
using Serilog.Events;

using ILogger = Serilog.ILogger;

public class MockPluginLogReplacement : IPluginLog, IMockService
{
    public ILogger Logger { get; }

    public MockPluginLogReplacement(ILogger logger)
    {
        this.Logger = logger;
    }

    public string ServiceName => "Replacement Plugin Log";

    public void Fatal(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Fatal("[CustomLogger] " + messageTemplate, values);
    }

    public void Fatal(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Fatal(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Error(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Error("[CustomLogger] " + messageTemplate, values);
    }

    public void Error(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Error(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Warning(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Warning("[CustomLogger] " + messageTemplate, values);
    }

    public void Warning(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Warning(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Information(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Information("[CustomLogger] " + messageTemplate, values);
    }

    public void Information(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Information(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Info(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Information("[CustomLogger] " + messageTemplate, values);
    }

    public void Info(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Information(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Debug(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Debug("[CustomLogger] " + messageTemplate, values);
    }

    public void Debug(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Debug(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Verbose(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Verbose("[CustomLogger] " + messageTemplate, values);
    }

    public void Verbose(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Verbose(exception, "[CustomLogger] " + messageTemplate, values);
    }

    public void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }

        this.Logger.Write(level, exception, "[CustomLogger] " + messageTemplate, values);
    }

    public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Verbose;
}
