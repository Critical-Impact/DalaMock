namespace DalaMock.Core;

using System.IO;
using DalaMock.Core.Configuration;
using DalaMock.Core.DI;
using Mocks;
using Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        var mockContainer = new MockContainer();
        var mockDalamudUi = mockContainer.GetMockUi();
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(DalamudMockPluginTest));
        mockDalamudUi.Run();
    }
}