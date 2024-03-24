using System.Text;
using Dalamud.Game.Libc;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockLibcFunction : ILibcFunction
{
    public OwnedStdString NewString(byte[] content)
    {
        return null!;
    }

    public OwnedStdString NewString(string content, Encoding? encoding = null)
    {
        return null!;
    }
}