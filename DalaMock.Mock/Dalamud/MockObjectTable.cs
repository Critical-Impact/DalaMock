using System.Collections;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockObjectTable : IObjectTable
{
    public IEnumerator<IGameObject> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }
    public IGameObject? SearchById(ulong objectId)
    {
        throw new NotImplementedException();
    }

    public nint GetObjectAddress(int index)
    {
        throw new NotImplementedException();
    }

    public IGameObject? CreateObjectReference(nint address)
    {
        throw new NotImplementedException();
    }

    public nint Address { get; }
    public int Length { get; }

    public IGameObject? this[int index] => throw new NotImplementedException();
}