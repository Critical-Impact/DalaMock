using Dalamud;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;
using Action = System.Action;
using ClientLanguage = Dalamud.Game.ClientLanguage;
using ConditionFlag = Dalamud.Game.ClientState.Conditions.ConditionFlag;

namespace DalaMock.Dalamud;

public class MockClientState : IClientState
{
    public bool IsClientIdle(out ConditionFlag blockingFlag)
    {
        blockingFlag = ConditionFlag.None;
        return false;
    }

    public ClientLanguage ClientLanguage { get; }
    public ushort TerritoryType { get; }
    public uint MapId { get; }
    public IPlayerCharacter? LocalPlayer { get; }
    public ulong LocalContentId { get; }
    public bool IsLoggedIn { get; }
    public bool IsPvP { get; }
    public bool IsPvPExcludingDen { get; }
    public bool IsGPosing { get; }
    public event Action<ushort>? TerritoryChanged;
    public event Action? Login;
    public event Action? Logout;
    public event Action? EnterPvP;
    public event Action? LeavePvP;
    public event Action<ContentFinderCondition>? CfPop;
}