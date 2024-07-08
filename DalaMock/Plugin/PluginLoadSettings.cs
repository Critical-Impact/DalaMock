namespace DalaMock.Core.Plugin;

using System.IO;

/// <summary>
/// The settings to use when loading the plugin.
/// </summary>
public class PluginLoadSettings(DirectoryInfo configDir, FileInfo configFile)
{
    /// <summary>
    /// Gets the configuration directory to provide to the plugin.
    /// </summary>
    public DirectoryInfo ConfigDir { get; private set; } = configDir;

    /// <summary>
    /// Gets the configuration file to provide to the plugin.
    /// </summary>
    public FileInfo ConfigFile { get; private set; } = configFile;
}