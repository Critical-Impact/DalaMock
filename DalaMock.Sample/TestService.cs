// <copyright file="TestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DalaMock.Sample;

using System.Threading;
using System.Threading.Tasks;
using DalaMock.Host.Factories;
using DalaMock.Shared.Interfaces;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Hosting;

public class TestService : IHostedService
{
    private readonly WindowSystemFactory windowSystemFactory;
    private readonly IDalamudPluginInterface pluginInterface;

    public TestService(WindowSystemFactory windowSystemFactory, IDalamudPluginInterface pluginInterface)
    {
        this.windowSystemFactory = windowSystemFactory;
        this.pluginInterface = pluginInterface;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}