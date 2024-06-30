using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;
using IDalamudTextureWrap = Dalamud.Interface.Textures.TextureWraps.IDalamudTextureWrap;

namespace DalaMock.Dalamud;

public class MockTitleScreenMenu : ITitleScreenMenu
{
    public IReadOnlyTitleScreenMenuEntry AddEntry(string text, IDalamudTextureWrap texture, Action onTriggered)
    {
        return null!;
    }

    public IReadOnlyTitleScreenMenuEntry AddEntry(ulong priority, string text, IDalamudTextureWrap texture, Action onTriggered)
    {
        return null!;
    }

    public void RemoveEntry(IReadOnlyTitleScreenMenuEntry entry)
    {
        
    }

    public IReadOnlyList<IReadOnlyTitleScreenMenuEntry> Entries { get; } = null!;
}