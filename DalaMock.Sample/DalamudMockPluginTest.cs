﻿// <copyright file="DalamudMockPluginTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    public DalamudMockPluginTest(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog, IDataManager dataManager, ITextureProvider textureProvider, IChatGui chatGui, IDtrBar dtrBar)
        : base(pluginInterface, pluginLog, dataManager, textureProvider, chatGui, dtrBar)
    {
    }

    public override void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        base.ConfigureContainer(containerBuilder);
        containerBuilder.RegisterType<MockWindowSystem>().As<IWindowSystem>();
        containerBuilder.RegisterType<MockFileDialogManager>().As<IFileDialogManager>();
        containerBuilder.RegisterType<MockFont>().As<IFont>().SingleInstance();
    }
}
