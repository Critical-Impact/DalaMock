using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockContextMenu : IContextMenu
{
    public void AddMenuItem(ContextMenuType menuType, MenuItem item)
    {
        
    }

    public bool RemoveMenuItem(ContextMenuType menuType, MenuItem item)
    {
        return true;
    }

    public event IContextMenu.OnMenuOpenedDelegate? OnMenuOpened;
}