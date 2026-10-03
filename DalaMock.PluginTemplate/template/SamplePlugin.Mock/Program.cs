namespace SamplePlugin;

using System.Threading.Tasks;

using DalaMock.Core.Plugin;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var mockContainer = new MockContainer();
        var mockDalamudUi = mockContainer.GetMockUi();
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(MockPlugin));
        await pluginLoader.StartPlugin(mockPlugin);
        mockDalamudUi.Run();
    }
}
