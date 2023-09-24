using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockCondition : ICondition
{
    public bool Any()
    {
        return false;
    }

    public bool Any(params ConditionFlag[] flags)
    {
        return false;
    }

    public int MaxEntries { get; } = 0;
    public nint Address { get; } = 0;

    public bool this[int flag] => false;

    public event ICondition.ConditionChangeDelegate? ConditionChange;
}