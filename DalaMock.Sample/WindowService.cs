// <copyright file="TestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Dalamud.Interface;

namespace DalaMock.Sample;

using System.Threading;
using System.Threading.Tasks;
using DalaMock.Host.Factories;
using DalaMock.Shared.Interfaces;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Hosting;

public class WindowService : IHostedService
{
    private readonly WindowSystemFactory windowSystemFactory;
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IUiBuilder uiBuilder;
    private readonly SampleWindow sampleWindow;
    private readonly DtrBarSampleWindow dtrBarSampleWindow;
    private readonly IWindowSystem windowSystem;

    public WindowService(WindowSystemFactory windowSystemFactory, IDalamudPluginInterface pluginInterface, IUiBuilder uiBuilder, SampleWindow sampleWindow, DtrBarSampleWindow dtrBarSampleWindow)
    {
        this.windowSystemFactory = windowSystemFactory;
        this.pluginInterface = pluginInterface;
        this.uiBuilder = uiBuilder;
        this.sampleWindow = sampleWindow;
        this.dtrBarSampleWindow = dtrBarSampleWindow;
        this.windowSystem = this.windowSystemFactory.Create("DalaMock.Sample");
        this.windowSystem.AddWindow(sampleWindow);
        this.windowSystem.AddWindow(dtrBarSampleWindow);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.sampleWindow.IsOpen = true;
        this.dtrBarSampleWindow.IsOpen = true;
        this.uiBuilder.Draw += this.Draw;
        return Task.CompletedTask;
    }

    private void Draw()
    {
        this.windowSystem.Draw();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.uiBuilder.Draw -= this.Draw;
        return Task.CompletedTask;
    }
}
