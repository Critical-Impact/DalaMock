namespace DalaMock.Core.Mocks;

using System.IO;
using System.Linq;
using Dalamud.Game;
using Newtonsoft.Json;

public class MockDalamudConfiguration
{
    public ClientLanguage ClientLanguage { get; set; } = ClientLanguage.English;

    [JsonIgnore]
    public DirectoryInfo? GamePath { get; set; }

    public string? GamePathString
    {
        get => this.GamePath?.FullName;
        set => this.GamePath = value == null ? null : new DirectoryInfo(value);
    }

    /// <summary>
    /// Gets a value indicating that the game path provided is valid.
    /// </summary>
    public bool GamePathValid
    {
        get
        {
            return this.GamePath != null && Directory.Exists(this.GamePath.FullName) && this.GamePath.EnumerateDirectories().Any(c => c.Name == "ffxiv");
        }
    }

    [JsonIgnore]
    public DirectoryInfo? PluginSavePath { get; set; }

    public string? PluginSavePathString
    {
        get => this.PluginSavePath?.FullName;
        set => this.PluginSavePath = value == null ? null : new DirectoryInfo(value);
    }

    /// <summary>
    /// Gets a value indicating that the plugin path provided is valid.
    /// </summary>
    public bool PluginSavePathValid
    {
        get
        {
            return this.PluginSavePath != null && Directory.Exists(this.PluginSavePath.FullName);
        }
    }
}