// <copyright file="SampleConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Dalamud.Configuration;

namespace DalaMock.Sample;

public class SampleConfiguration : IPluginConfiguration
{
    public string Test { get; set; }
    public int Version { get; set; }
}
