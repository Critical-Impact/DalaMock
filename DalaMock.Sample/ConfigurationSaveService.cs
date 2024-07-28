// <copyright file="ConfigurationSaveService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

using Dalamud.Plugin;

using Microsoft.Extensions.Hosting;

namespace DalaMock.Sample;

public class ConfigurationSaveService : IHostedService
{
    private readonly IDalamudPluginInterface dalamudPluginInterface;
    private SampleConfiguration sampleConfiguration;

    public ConfigurationSaveService(IDalamudPluginInterface dalamudPluginInterface)
    {
        this.dalamudPluginInterface = dalamudPluginInterface;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.sampleConfiguration = this.dalamudPluginInterface.GetPluginConfig() as SampleConfiguration ?? new SampleConfiguration();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.dalamudPluginInterface.SavePluginConfig(this.sampleConfiguration);
        return Task.CompletedTask;
    }
}
