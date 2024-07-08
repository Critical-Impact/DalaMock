using IMenuItem = Dalamud.Game.Gui.ContextMenu.IMenuItem;

namespace DalaMock.Core.Mocks;

using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Plugin.Services;

public class MockContextMenu : IContextMenu, IMockService
{
    public void AddMenuItem(ContextMenuType menuType, IMenuItem item)
    {
    }

    public bool RemoveMenuItem(ContextMenuType menuType, IMenuItem item)
    {
        return true;
    }

    public event IContextMenu.OnMenuOpenedDelegate? OnMenuOpened;

    public string ServiceName => "Context Menu";
}