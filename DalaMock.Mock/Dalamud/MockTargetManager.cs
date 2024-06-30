using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Types;

namespace DalaMock.Dalamud;

public class MockTargetManager : ITargetManager
{
    public nint Address { get; }
    public IGameObject? Target { get; set; }
    public IGameObject? MouseOverTarget { get; set; }
    public IGameObject? FocusTarget { get; set; }
    public IGameObject? PreviousTarget { get; set; }
    public IGameObject? SoftTarget { get; set; }
    public IGameObject? GPoseTarget { get; set; }
    public IGameObject? MouseOverNameplateTarget { get; set; }
}