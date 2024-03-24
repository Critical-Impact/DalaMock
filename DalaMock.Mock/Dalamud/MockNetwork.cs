using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockGameNetwork : IGameNetwork
{
    public event IGameNetwork.OnNetworkMessageDelegate? NetworkMessage;
}