using Dalamud.Interface.Internal;

namespace DalaMock.Dalamud;

public class MockTextureMap : IDalamudTextureWrap
{

    public MockTextureMap(nint handle, int width, int height)
    {
        ImGuiHandle = handle;
        Width = width;
        Height = height;
    }
    public void Dispose()
    {
    }

    public nint ImGuiHandle { get; }
    public int Width { get; }
    public int Height { get; }
}