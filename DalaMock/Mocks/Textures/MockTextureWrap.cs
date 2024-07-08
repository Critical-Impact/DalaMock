namespace DalaMock.Core.Mocks.Textures;

using Dalamud.Interface.Textures.TextureWraps;

public class MockTextureMap : IDalamudTextureWrap
{
    public MockTextureMap(nint handle, int width, int height)
    {
        this.ImGuiHandle = handle;
        this.Width = width;
        this.Height = height;
    }

    public void Dispose()
    {
    }

    public nint ImGuiHandle { get; }

    public int Width { get; }

    public int Height { get; }
}