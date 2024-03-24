using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockToastGui : IToastGui
{
    public void ShowNormal(string message, ToastOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void ShowNormal(SeString message, ToastOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void ShowQuest(string message, QuestToastOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void ShowQuest(SeString message, QuestToastOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void ShowError(string message)
    {
        throw new NotImplementedException();
    }

    public void ShowError(SeString message)
    {
        throw new NotImplementedException();
    }

    public event IToastGui.OnNormalToastDelegate? Toast;
    public event IToastGui.OnQuestToastDelegate? QuestToast;
    public event IToastGui.OnErrorToastDelegate? ErrorToast;
}