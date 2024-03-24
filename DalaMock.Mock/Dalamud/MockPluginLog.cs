using Dalamud.Plugin.Services;
using Serilog;
using Serilog.Events;

namespace DalaMock.Dalamud;

public class MockPluginLog : IPluginLog
{
    public readonly ILogger _logger;

    public MockPluginLog(ILogger logger)
    {
        _logger = logger;
    }
    public void Fatal(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Fatal(messageTemplate, values);
    }

    public void Fatal(Exception? exception, string messageTemplate, params object[] values)
    {        
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Fatal(exception, messageTemplate, values);
    }

    public void Error(string messageTemplate, params object[] values)
    {        
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Error(messageTemplate, values);
    }

    public void Error(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Error(exception, messageTemplate, values);
    }

    public void Warning(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Warning(messageTemplate, values);
    }

    public void Warning(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Warning(exception, messageTemplate, values);
    }

    public void Information(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Information(messageTemplate, values);
    }

    public void Information(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Information(exception, messageTemplate, values);
    }

    public void Info(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Information(messageTemplate, values);
    }

    public void Info(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Information(exception, messageTemplate, values);
    }

    public void Debug(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Debug(messageTemplate, values);
    }

    public void Debug(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Debug(exception, messageTemplate, values);
    }

    public void Verbose(string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Verbose(messageTemplate, values);
    }

    public void Verbose(Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Verbose(exception, messageTemplate, values);
    }

    public void Write(LogEventLevel level, Exception? exception, string messageTemplate, params object[] values)
    {
        if (messageTemplate.Contains("Evicting"))
        {
            return;
        }
        _logger.Write(level, exception, messageTemplate, values);
    }

    public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Verbose;
}