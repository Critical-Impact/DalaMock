using System.Threading.Tasks;

using DalaMock.Core.Plugin;

namespace DalaMock.Sample.Mock;

using System;
using System.Collections.Generic;

using DalaMock.Core.Mocks;
using DalaMock.Sample.Services;
using Dalamud.Plugin.Services;

internal static class Program
{
    private async static Task Main(string[] args)
    {
        var mockContainer = new MockContainer(
            serviceReplacements: new Dictionary<Type, Type>()
            {
                { typeof(IPluginLog), typeof(MockPluginLogReplacement) },
                { typeof(ISigScanner), typeof(MockSigScanner) },
            });
        var mockDalamudUi = mockContainer.GetMockUi();
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(MockDalamudPluginTest));
        await pluginLoader.StartPlugin(mockPlugin);
        mockDalamudUi.Run();
    }
}