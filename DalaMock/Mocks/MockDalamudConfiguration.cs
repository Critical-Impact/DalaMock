namespace DalaMock.Core.Mocks;

using System.IO;
using Dalamud.Game;

public class MockDalamudConfiguration
{
    public bool UseTexturePluginTracking { get; set; } = true;

    public ClientLanguage ClientLanguage { get; set; } = ClientLanguage.English;

    public DirectoryInfo? GamePath { get; set; }
}