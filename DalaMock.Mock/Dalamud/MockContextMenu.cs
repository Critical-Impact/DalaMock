using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockContextMenu : IContextMenu
{
    public void AddMenuItem(ContextMenuType menuType, IMenuItem item)
    {
        
    }

    public bool RemoveMenuItem(ContextMenuType menuType, IMenuItem item)
    {
        return true;
    }

    public event IContextMenu.OnMenuOpenedDelegate? OnMenuOpened;
}