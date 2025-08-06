using Dalamud.Game.Addon.Events.EventDataTypes;

using Microsoft.Extensions.Logging;

namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

/// <summary>
/// Currently this is a global dtr bar store which does not mimic what dalamud provides, scoped services will be implemented at some point
/// </summary>
public class MockDtrBar : IDtrBar, IMockService
{
    private readonly Func<MockDtrBarEntry> entryFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockDtrBar"/> class.
    /// </summary>
    /// <param name="entryFactory"></param>
    public MockDtrBar(Func<MockDtrBarEntry> entryFactory)
    {
        this.entryFactory = entryFactory;
    }

    private readonly Dictionary<string, MockDtrBarEntry> dtrBarEntries = new();

    /// <inheritdoc/>
    public IDtrBarEntry Get(string title, SeString? text = null)
    {
        if (this.dtrBarEntries.ContainsKey(title))
        {
            throw new ArgumentException($"The Mock DTR bar already contains an entry with the title {title}");
        }

        var mockDtrBarEntry = this.entryFactory.Invoke();
        mockDtrBarEntry.Title = title;
        if (text != null)
        {
            mockDtrBarEntry.Text = text;
        }

        this.dtrBarEntries[title] = mockDtrBarEntry;
        return this.dtrBarEntries[title];
    }

    /// <inheritdoc/>
    public void Remove(string title)
    {
        this.dtrBarEntries.Remove(title);
    }

    /// <inheritdoc/>
    public IReadOnlyList<IReadOnlyDtrBarEntry> Entries =>
        this.dtrBarEntries.Select(c => (IReadOnlyDtrBarEntry)c.Value).ToList();

    public string ServiceName { get; set; } = "DTR Bar";
}

public class MockReadOnlyDtrEntryBar : IReadOnlyDtrBarEntry
{
    public ILogger<MockReadOnlyDtrEntryBar> Logger { get; }

    public MockReadOnlyDtrEntryBar(ILogger<MockReadOnlyDtrEntryBar> logger)
    {
        this.Logger = logger;
    }

    public virtual bool TriggerClickAction()
    {
        this.Logger.LogInformation("You cannot trigger an action on a read only dtr entry bar.");
        return false;
    }

    /// <inheritdoc/>
    public string Title { get; set; }

    /// <inheritdoc/>
    public bool HasClickAction { get; set; }

    /// <inheritdoc/>
    public SeString Text { get; set; }

    /// <inheritdoc/>
    public SeString Tooltip { get; set; }

    /// <inheritdoc/>
    public bool Shown { get; set; }

    /// <inheritdoc/>
    public bool UserHidden { get; set; }
}

public class MockDtrBarEntry : MockReadOnlyDtrEntryBar, IDtrBarEntry
{
    private readonly MockDtrBar mockDtrBar;

    public MockDtrBarEntry(MockDtrBar mockDtrBar, ILogger<MockReadOnlyDtrEntryBar> logger)
        : base(logger)
    {
        this.mockDtrBar = mockDtrBar;
    }

    /// <inheritdoc/>
    public void Remove()
    {
        this.mockDtrBar.Remove(this.Title);
    }

    public Action<AddonMouseEventData>? OnClick { get; set; }
}
