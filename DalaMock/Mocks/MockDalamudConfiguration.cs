namespace DalaMock.Core.Mocks;

using System.IO;
using System.Linq;

using Dalamud.Game;

using Newtonsoft.Json;

public class MockDalamudConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether should a Veldrid window be created.
    /// </summary>
    public bool CreateWindow { get; set; } = true;

    /// <summary>
    /// Gets or sets what language should be loaded.
    /// </summary>
    public ClientLanguage ClientLanguage { get; set; } = ClientLanguage.English;

    [JsonIgnore]
    public DirectoryInfo? GamePath { get; set; }

    public string? GamePathString
    {
        get => this.GamePath?.FullName;
        set => this.GamePath = value == null ? null : new DirectoryInfo(value);
    }

    /// <summary>
    /// Gets a value indicating whether gets a value indicating that the game path provided is valid.
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
    /// Gets a value indicating whether gets a value indicating that the plugin path provided is valid.
    /// </summary>
    public bool PluginSavePathValid
    {
        get
        {
            return this.PluginSavePath != null && Directory.Exists(this.PluginSavePath.FullName);
        }
    }
}
