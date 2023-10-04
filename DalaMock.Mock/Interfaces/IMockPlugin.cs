using DalaMock.Dalamud;
using DalaMock.Mock;

namespace DalaMock.Interfaces;

public interface IMockPlugin : IDisposable
{
    public bool IsStarted { get; }
    public void Start(MockProgram program, MockService mockService, MockPluginInterfaceService mockPluginInterfaceService);
    public void Stop(MockProgram program, MockService mockService, MockPluginInterfaceService mockPluginInterfaceService);
}