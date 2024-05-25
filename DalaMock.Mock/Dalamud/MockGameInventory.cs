using Dalamud.Game.Inventory.InventoryEventArgTypes;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockGameInventory : IGameInventory
{
    public event IGameInventory.InventoryChangelogDelegate? InventoryChanged;
    public event IGameInventory.InventoryChangelogDelegate? InventoryChangedRaw;
    public event IGameInventory.InventoryChangedDelegate? ItemAdded;
    public event IGameInventory.InventoryChangedDelegate? ItemRemoved;
    public event IGameInventory.InventoryChangedDelegate? ItemChanged;
    public event IGameInventory.InventoryChangedDelegate? ItemMoved;
    public event IGameInventory.InventoryChangedDelegate? ItemSplit;
    public event IGameInventory.InventoryChangedDelegate? ItemMerged;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemAddedArgs>? ItemAddedExplicit;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemRemovedArgs>? ItemRemovedExplicit;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemChangedArgs>? ItemChangedExplicit;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemMovedArgs>? ItemMovedExplicit;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemSplitArgs>? ItemSplitExplicit;
    public event IGameInventory.InventoryChangedDelegate<InventoryItemMergedArgs>? ItemMergedExplicit;
}