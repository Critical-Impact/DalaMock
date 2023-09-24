using Dalamud;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;
using Lumina.Data.Files;

namespace DalaMock.Dalamud;

public class MockTextureProvider : ITextureProvider
{
    private readonly MockTextureManager _mockTextureManager;

    public MockTextureProvider(MockTextureManager mockTextureManager)
    {
        _mockTextureManager = mockTextureManager;
    }
    
    public IDalamudTextureWrap? GetIcon(uint iconId, ITextureProvider.IconFlags flags = ITextureProvider.IconFlags.HiRes, ClientLanguage? language = null,
        bool keepAlive = false)
    {
        return _mockTextureManager.GetIcon(iconId, flags, language);
    }

    public string? GetIconPath(uint iconId, ITextureProvider.IconFlags flags = ITextureProvider.IconFlags.HiRes, ClientLanguage? language = null)
    {
        return _mockTextureManager.GetIconPath(iconId, flags, language);
    }

    public IDalamudTextureWrap? GetTextureFromGame(string path, bool keepAlive = false)
    {
        return _mockTextureManager.GetTextureFromGame(path, keepAlive);
    }

    public IDalamudTextureWrap? GetTextureFromFile(FileInfo file, bool keepAlive = false)
    {
        return _mockTextureManager.GetTextureFromFile(file, keepAlive);
    }

    public IDalamudTextureWrap GetTexture(TexFile file)
    {
        return _mockTextureManager.GetTexture(file);
    }
}