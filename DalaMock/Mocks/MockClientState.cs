namespace DalaMock.Core.Mocks;

using System;
using Dalamud.Game;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;

public class MockClientState : IClientState, IMockService
{
    private ClientLanguage clientLanguage = ClientLanguage.English;
    private bool isGPosing;
    private bool isLoggedIn;
    private bool isPvP;
    private bool isPvPExcludingDen;
    private ulong localContentId;
    private IPlayerCharacter? localPlayer;
    private uint mapId;
    private ushort territoryType;

    public bool IsClientIdle(out ConditionFlag blockingFlag)
    {
        throw new NotImplementedException();
    }

    public ClientLanguage ClientLanguage
    {
        get => this.clientLanguage;
        set => this.clientLanguage = value;
    }

    public ushort TerritoryType
    {
        get => this.territoryType;
        set
        {
            if (this.territoryType != value)
            {
                this.territoryType = value;
                this.TerritoryChanged?.Invoke(this.territoryType);
            }
        }
    }

    public uint MapId
    {
        get => this.mapId;
        set => this.mapId = value;
    }

    public IPlayerCharacter? LocalPlayer
    {
        get => this.localPlayer;
        set => this.localPlayer = value;
    }

    public ulong LocalContentId
    {
        get => this.localContentId;
        set => this.localContentId = value;
    }

    public bool IsLoggedIn
    {
        get => this.isLoggedIn;
        set
        {
            if (this.isLoggedIn != value)
            {
                this.isLoggedIn = value;
                if (this.isLoggedIn)
                {
                    this.Login?.Invoke();
                }
                else
                {
                    this.Logout?.Invoke();
                }
            }
        }
    }

    public bool IsPvP
    {
        get => this.isPvP;
        set
        {
            if (this.isPvP != value)
            {
                this.isPvP = value;
                if (this.isPvP)
                {
                    this.EnterPvP?.Invoke();
                }
                else
                {
                    this.LeavePvP?.Invoke();
                }
            }
        }
    }

    public bool IsPvPExcludingDen
    {
        get => this.isPvPExcludingDen;
        set => this.isPvPExcludingDen = value;
    }

    public bool IsGPosing
    {
        get => this.isGPosing;
        set => this.isGPosing = value;
    }

    public event Action<ushort>? TerritoryChanged;

    public event IClientState.ClassJobChangeDelegate? ClassJobChanged;

    public event IClientState.LevelChangeDelegate? LevelChanged;

    public event System.Action? Login;

    public event System.Action? Logout;

    public event System.Action? EnterPvP;

    public event System.Action? LeavePvP;

    public event Action<ContentFinderCondition>? CfPop;

    public string ServiceName => "Client State";

    // Method to simulate CF Pop event
    public void TriggerCfPop(ContentFinderCondition condition)
    {
        this.CfPop?.Invoke(condition);
    }
}
