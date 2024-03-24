using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Types;

namespace DalaMock.Dalamud;

public class MockTargetManager : ITargetManager
{
    public nint Address { get; }
    public GameObject? Target { get; set; }
    public GameObject? MouseOverTarget { get; set; }
    public GameObject? FocusTarget { get; set; }
    public GameObject? PreviousTarget { get; set; }
    public GameObject? SoftTarget { get; set; }
    public GameObject? GPoseTarget { get; set; }
    public GameObject? MouseOverNameplateTarget { get; set; }
}