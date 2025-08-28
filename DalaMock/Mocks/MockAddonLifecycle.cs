namespace DalaMock.Core.Mocks;

using System.Collections.Generic;

using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Plugin.Services;

public class MockAddonLifecycle : IAddonLifecycle, IMockService
{
    public void RegisterListener(
        AddonEvent eventType,
        IEnumerable<string> addonNames,
        IAddonLifecycle.AddonEventDelegate handler)
    {
    }

    public void RegisterListener(AddonEvent eventType, string addonName, IAddonLifecycle.AddonEventDelegate handler)
    {
    }

    public void RegisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate handler)
    {
    }

    public void UnregisterListener(
        AddonEvent eventType,
        IEnumerable<string> addonNames,
        IAddonLifecycle.AddonEventDelegate? handler = null)
    {
    }

    public void UnregisterListener(
        AddonEvent eventType,
        string addonName,
        IAddonLifecycle.AddonEventDelegate? handler = null)
    {
    }

    public void UnregisterListener(AddonEvent eventType, IAddonLifecycle.AddonEventDelegate? handler = null)
    {
    }

    public void UnregisterListener(params IAddonLifecycle.AddonEventDelegate[] handlers)
    {
    }

    public string ServiceName => "Addon Lifecycle";
}
