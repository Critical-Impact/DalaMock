namespace SamplePlugin;

using Autofac;
using DalaMock.Core.Mocks;
using DalaMock.Core.Windows;
using DalaMock.Shared.Interfaces;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

public class MockPlugin : Plugin
{
    private readonly MockReplacementContainer mockReplacementContainer;

    public MockPlugin(MockReplacementContainer mockReplacementContainer, IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
        : base(pluginInterface, pluginLog)
    {
        this.mockReplacementContainer = mockReplacementContainer;
    }

    public override IReplacementContainer ReplacementContainer => mockReplacementContainer;
}
