using System.Diagnostics;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockGameInteropProvider : IGameInteropProvider
{
    public void InitializeFromAttributes(object self)
    {
        
    }

    public Hook<T> HookFromFunctionPointerVariable<T>(nint address, T detour) where T : Delegate
    {
        return null!;
    }

    public Hook<T> HookFromImport<T>(ProcessModule? module, string moduleName, string functionName, uint hintOrOrdinal, T detour) where T : Delegate
    {
        return null!;
    }

    public Hook<T> HookFromSymbol<T>(string moduleName, string exportName, T detour, IGameInteropProvider.HookBackend backend = IGameInteropProvider.HookBackend.Automatic) where T : Delegate
    {
        return null!;
    }

    public Hook<T> HookFromAddress<T>(nint procAddress, T detour, IGameInteropProvider.HookBackend backend = IGameInteropProvider.HookBackend.Automatic) where T : Delegate
    {
        return null!;
    }

    public Hook<T> HookFromSignature<T>(string signature, T detour, IGameInteropProvider.HookBackend backend = IGameInteropProvider.HookBackend.Automatic) where T : Delegate
    {
        return null!;
    }
}