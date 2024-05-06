using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockTitleScreenMenu : ITitleScreenMenu
{
    public TitleScreenMenuEntry AddEntry(string text, IDalamudTextureWrap texture, Action onTriggered)
    {
        return null!;
    }

    public TitleScreenMenuEntry AddEntry(ulong priority, string text, IDalamudTextureWrap texture, Action onTriggered)
    {
        return null!;
    }

    public void RemoveEntry(TitleScreenMenuEntry entry)
    {
        
    }

    public IReadOnlyList<TitleScreenMenuEntry> Entries { get; } = null!;
}