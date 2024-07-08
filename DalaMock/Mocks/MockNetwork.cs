namespace DalaMock.Core.Mocks;

using Dalamud.Plugin.Services;

public class MockGameNetwork : IGameNetwork, IMockService
{
    public event IGameNetwork.OnNetworkMessageDelegate? NetworkMessage;

    public string ServiceName => "Game Network";
}