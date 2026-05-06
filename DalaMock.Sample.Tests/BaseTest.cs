using System.Threading;
using DalaMock.Host.Mediator;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DalaMock.Sample.Tests;

/// <summary>
/// Abstract base class for DalaMock unit tests. Bootstraps the full DI host
/// via <see cref="TestBoot"/>, starts the <see cref="MediatorService"/>, and
/// exposes the most commonly needed services so derived test classes can focus
/// on their own setup and assertions.
/// </summary>
public class BaseTest : IMediatorSubscriber
{
    public BaseTest()
    {
        Host = new TestBoot().CreateHost().Result;
        MediatorService = Host.Services.GetRequiredService<MediatorService>();
        MediatorService.StartAsync(CancellationToken.None);
        PluginLog = Host.Services.GetRequiredService<IPluginLog>();
    }

    public IPluginLog PluginLog { get; set; }
    public IHost Host { get; set; }
    public MediatorService MediatorService { get; }
}