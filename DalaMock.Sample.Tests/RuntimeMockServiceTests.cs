using DalaMock.Core.Mocks.DalamudServices;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Gui.NamePlate;
using NUnit.Framework;
using System;

namespace DalaMock.Sample.Tests;

/// <summary>
/// Tests runtime-service mocks that need to be directly drivable from hosted tests.
/// </summary>
[TestFixture]
public class RuntimeMockServiceTests
{
    /// <summary>
    /// Verifies that addon lifecycle registrations are recorded and dispatchable.
    /// </summary>
    [Test]
    public void MockAddonLifecycleCanRecordAndDispatchListeners()
    {
        var lifecycle = new MockAddonLifecycle();
        var received = false;

        lifecycle.RegisterListener(
            AddonEvent.PreDraw,
            "MockSurface",
            (evt, args) =>
            {
                Assert.That(evt, Is.EqualTo(AddonEvent.PreDraw));
                Assert.That(args, Is.Not.Null);
                received = true;
            });

        Assert.That(
            lifecycle.RegisteredListeners,
            Has.Some.Matches<MockAddonLifecycleRegistration>(listener =>
                listener.EventType == AddonEvent.PreDraw &&
                listener.AddonName == "MockSurface"));

        var invoked = lifecycle.Raise(AddonEvent.PreDraw, "MockSurface");

        Assert.That(invoked, Is.EqualTo(1));
        Assert.That(received, Is.True);
    }

    /// <summary>
    /// Verifies that nameplate mock subscriptions are counted and dispatchable.
    /// </summary>
    [Test]
    public void MockNamePlateGuiCanRecordAndDispatchSubscribers()
    {
        var gui = new MockNamePlateGui();
        var received = false;

        gui.OnNamePlateUpdate += (_, handlers) =>
        {
            Assert.That(handlers, Is.Empty);
            received = true;
        };

        Assert.That(gui.NamePlateUpdateSubscriberCount, Is.EqualTo(1));

        var invoked = gui.RaiseNamePlateUpdate(
            context: null!,
            Array.Empty<INamePlateUpdateHandler>());

        Assert.That(invoked, Is.EqualTo(1));
        Assert.That(received, Is.True);
    }

    /// <summary>
    /// Verifies that the game GUI mock can resolve registered addon addresses.
    /// </summary>
    [Test]
    public void MockGameGuiCanResolveRegisteredAddons()
    {
        var gui = new MockGameGui();
        nint addonAddress = 0x230;

        gui.RegisterAddon("JournalAccept", addonAddress);

        Assert.That(gui.GetAddonByName("JournalAccept"), Is.EqualTo(addonAddress));
        Assert.That(gui.GetAddonByName("JournalAccept", index: 2), Is.EqualTo(nint.Zero));
    }
}
