// <copyright file="HostingAwareService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace DalaMock.Host.Hosting;

public abstract class HostingAwareService : IHostedService, IDisposable
{
    private readonly HostedEvents hostedEvents;

    public HostingAwareService(HostedEvents hostedEvents)
    {
        this.hostedEvents = hostedEvents;
        this.hostedEvents.PluginEvent += this.OnPluginEvent;
    }

    public abstract void OnPluginEvent(HostedEventType eventType);

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync(CancellationToken cancellationToken);

    public void Dispose()
    {
        this.hostedEvents.PluginEvent -= this.OnPluginEvent;
    }
}
