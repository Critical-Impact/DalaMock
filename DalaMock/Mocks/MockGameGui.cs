using Dalamud.Game.NativeWrapper;

namespace DalaMock.Core.Mocks;

using System;
using System.Numerics;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;

public class MockGameGui : IGameGui, IMockService
{
    private ulong hoveredItemId;

    public bool OpenMapWithMapLink(MapLinkPayload mapLink)
    {
        throw new NotImplementedException();
    }

    public bool WorldToScreen(Vector3 worldPos, out Vector2 screenPos)
    {
        throw new NotImplementedException();
    }

    public bool WorldToScreen(Vector3 worldPos, out Vector2 screenPos, out bool inView)
    {
        throw new NotImplementedException();
    }

    public bool ScreenToWorld(Vector2 screenPos, out Vector3 worldPos, float rayDistance = 100000)
    {
        throw new NotImplementedException();
    }
    
    UIModulePtr IGameGui.GetUIModule()
    {
        return this.GetUIModule();
    }
    
    AtkUnitBasePtr IGameGui.GetAddonByName(string name, int index)
    {
        return this.GetAddonByName(name, index);
    }
    
    public AgentInterfacePtr GetAgentById(int id)
    {
        throw new NotImplementedException();
    }
    
    AgentInterfacePtr IGameGui.FindAgentInterface(string addonName)
    {
        return this.FindAgentInterface(addonName);
    }
    
    public AgentInterfacePtr FindAgentInterface(AtkUnitBasePtr addon)
    {
        throw new NotImplementedException();
    }
    
    public nint GetUIModule()
    {
        return 0;
    }

    public nint GetAddonByName(string name, int index = 1)
    {
        return 0;
    }

    public nint FindAgentInterface(string addonName)
    {
        return 0;
    }

    public unsafe nint FindAgentInterface(void* addon)
    {
        return 0;
    }

    public nint FindAgentInterface(nint addonPtr)
    {
        return 0;
    }

    public bool GameUiHidden { get; }

    public ulong HoveredItem
    {
        get => this.hoveredItemId;
        set => this.hoveredItemId = value;
    }

    public HoveredAction HoveredAction { get; }

    public event EventHandler<bool>? UiHideToggled;

    public event EventHandler<ulong>? HoveredItemChanged;

    public event EventHandler<HoveredAction>? HoveredActionChanged;

    public string ServiceName => "Game Gui";

    public void SetHoveredItem(uint itemId, InventoryItem.ItemFlags flags)
    {
        if (flags == InventoryItem.ItemFlags.HighQuality)
        {
            itemId += 1_000_000;
        }

        this.hoveredItemId = itemId;
    }
}
