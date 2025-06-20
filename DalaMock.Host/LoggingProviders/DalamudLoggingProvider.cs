namespace DalaMock.Host.LoggingProviders;

using System;
using System.Collections.Concurrent;
using System.Linq;

using DalaMock.Host.Loggers;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

[ProviderAlias("Dalamud")]
public sealed class DalamudLoggingProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, DalamudLogger> loggers =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly IPluginLog pluginLog;

    public DalamudLoggingProvider(IPluginLog pluginLog)
    {
        this.pluginLog = pluginLog;
    }

    public ILogger CreateLogger(string categoryName)
    {
        string catName = categoryName.Split(".", StringSplitOptions.RemoveEmptyEntries).Last();
        if (catName.Length > 15)
        {
            catName = string.Join("", catName.Take(6)) + "..." + string.Join("", catName.TakeLast(6));
        }
        else
        {
            catName = string.Join("", Enumerable.Range(0, 15 - catName.Length).Select(_ => " ")) + catName;
        }

        return this.loggers.GetOrAdd(catName, name => new DalamudLogger(name, this.pluginLog));
    }

    public void Dispose()
    {
        this.loggers.Clear();
        GC.SuppressFinalize(this);
    }
}
