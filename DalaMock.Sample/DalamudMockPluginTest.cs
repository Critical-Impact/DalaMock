// <copyright file="DalamudMockPluginTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace DalaMock.Sample;

using Autofac;
using DalaMock.Core.Mocks;
using DalaMock.Core.Windows;
using DalaMock.Shared.Interfaces;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

public class DalamudMockPluginTest : DalamudPluginTest
{
    private readonly ILogger<DalamudMockPluginTest> logger;

    public DalamudMockPluginTest(IDalamudPluginInterface pluginInterface, ILogger<DalamudMockPluginTest> logger,  IPluginLog pluginLog, IDataManager dataManager, ITextureProvider textureProvider, IChatGui chatGui, IDtrBar dtrBar)
        : base(pluginInterface, pluginLog, dataManager, textureProvider, chatGui, dtrBar)
    {
        this.logger = logger;
        this.logger.LogInformation("Plugin test started");
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        base.ConfigureContainer(containerBuilder);
        containerBuilder.RegisterType<MockWindowSystem>().As<IWindowSystem>();
        containerBuilder.RegisterType<MockFileDialogManager>().As<IFileDialogManager>();
        containerBuilder.RegisterType<MockFont>().As<IFont>().SingleInstance();
    }
}
