namespace DalaMock.Sample.Mock;

using DalaMock.Core.DI;

internal static class Program
{
    private static void Main(string[] args)
    {
        var mockContainer = new MockContainer();
        var mockDalamudUi = mockContainer.GetMockUi();
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(DalamudMockPluginTest));
        pluginLoader.StartPlugin(mockPlugin);
        mockDalamudUi.Run();
    }
}
