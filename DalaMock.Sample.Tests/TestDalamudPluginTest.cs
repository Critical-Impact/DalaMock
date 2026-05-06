using DalaMock.Core.Mocks;
using DalaMock.Shared.Interfaces;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace DalaMock.Sample.Tests;

/// <summary>
/// Concrete <see cref="DalamudPluginTest"/> implementation used as the plugin under test.
/// Accepts <see cref="MockReplacements"/> via constructor injection so that the mock
/// Dalamud service overrides wired up by the test suite are passed through to the
/// base plugin infrastructure.
/// </summary>
public class TestDalamudPluginTest : DalamudPluginTest
{
    private readonly MockReplacementContainer mockReplacementContainer;

    /// <summary>
    /// Initialises the plugin, forwarding <paramref name="pluginInterface"/> and
    /// <paramref name="pluginLog"/> to the base class and storing
    /// <paramref name="mockReplacementContainer"/> for later exposure through
    /// <see cref="IReplacementContainer"/>.
    /// </summary>
    /// <param name="mockReplacementContainer">
    /// The set of mock Dalamud service replacements provided by the DI container.
    /// </param>
    /// <param name="pluginInterface">The Dalamud plugin interface mock.</param>
    /// <param name="pluginLog">The plugin log mock.</param>
    public TestDalamudPluginTest(MockReplacementContainer mockReplacementContainer, IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
        : base(pluginInterface, pluginLog)
    {
        this.mockReplacementContainer = mockReplacementContainer;
    }

    /// <summary>
    /// Gets the <see cref="IReplacementContainer"/> that provides the mock Dalamud service
    /// replacements for this plugin instance.
    /// </summary>
    public override IReplacementContainer ReplacementContainer => mockReplacementContainer;
}