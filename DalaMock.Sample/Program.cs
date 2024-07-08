namespace DalaMock.Core;

using System.IO;
using DalaMock.Core.DI;
using Mocks;
using Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        var dalamudConfiguration = new MockDalamudConfiguration();
        var mockContainer = new MockContainer(dalamudConfiguration);
        var mockDalamudUi = mockContainer.GetMockUi();
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(DalamudPluginTest));
        mockDalamudUi.Run();
    }
}