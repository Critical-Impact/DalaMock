using System.Text;
using Dalamud.Game.Libc;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockILibcFunction : ILibcFunction
{
    public OwnedStdString NewString(byte[] content)
    {
        throw new NotImplementedException();
    }

    public OwnedStdString NewString(string content, Encoding? encoding = null)
    {
        throw new NotImplementedException();
    }
}