using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockAddonLifecycle : IAddonLifecycle
{
    public void RegisterListener(AddonEvent eventType, IEnumerable<string> addonNames, IAddonLifecycle.AddonEventDelegate handler)
    {
        
    }

    public void RegisterListener(AddonEvent eventType, string addonName, IAddonLifecycle.AddonEventDelegate handler)
    {
    }

    public void RegisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate handler)
    {
    }

    public void UnregisterListener(AddonEvent eventType, IEnumerable<string> addonNames, IAddonLifecycle.AddonEventDelegate handler = null)
    {
    }

    public void UnregisterListener(AddonEvent eventType, string addonName, IAddonLifecycle.AddonEventDelegate handler = null)
    {
    }

    public void UnregisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate handler = null)
    {
    }

    public void UnregisterListener(params IAddonLifecycle.AddonEventDelegate[] handlers)
    {
    }
}