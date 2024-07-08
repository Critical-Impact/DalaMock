namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using Dalamud.Interface;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Plugin.Services;

public class MockTitleScreenMenu : ITitleScreenMenu, IMockService
{
    public string ServiceName => "Title Screen Menu";

    public IReadOnlyTitleScreenMenuEntry AddEntry(string text, IDalamudTextureWrap texture, Action onTriggered)
    {
        return null!;
    }

    public IReadOnlyTitleScreenMenuEntry AddEntry(
        ulong priority,
        string text,
        IDalamudTextureWrap texture,
        Action onTriggered)
    {
        return null!;
    }

    public void RemoveEntry(IReadOnlyTitleScreenMenuEntry entry)
    {
    }

    public IReadOnlyList<IReadOnlyTitleScreenMenuEntry> Entries { get; } = null!;

    public IReadOnlyTitleScreenMenuEntry AddEntry(string text, ISharedImmediateTexture texture, Action onTriggered)
    {
        return null!;
    }

    public IReadOnlyTitleScreenMenuEntry AddEntry(
        ulong priority,
        string text,
        ISharedImmediateTexture texture,
        Action onTriggered)
    {
        return null!;
    }
}