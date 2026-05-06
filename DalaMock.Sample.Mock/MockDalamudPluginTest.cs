namespace DalaMock.Sample;

using DalaMock.Core.Mocks;
using DalaMock.Shared.Interfaces;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

public class MockDalamudPluginTest : DalamudPluginTest
{
    private readonly MockReplacementContainer mockReplacementContainer;

    public MockDalamudPluginTest(MockReplacementContainer mockReplacementContainer, IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
        : base(pluginInterface, pluginLog)
    {
        this.mockReplacementContainer = mockReplacementContainer;
    }

    public override IReplacementContainer ReplacementContainer => mockReplacementContainer;
}
