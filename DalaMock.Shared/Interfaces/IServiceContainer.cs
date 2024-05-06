namespace DalaMock.Shared.Interfaces;

public interface IServiceContainer : IDisposable
{
    IPluginInterfaceService PluginInterfaceService { get; set; }
}