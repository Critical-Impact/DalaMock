using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DalaMock.Core.Configuration;
using DalaMock.Core.Plugin;
using Dalamud.Plugin;
using NUnit.Framework;

namespace DalaMock.Sample.Tests;

/// <summary>
/// Tests plugin loader behavior that affects hosted plugin adapters.
/// </summary>
[TestFixture]
public class PluginLoaderTests
{
    /// <summary>
    /// Verifies that an explicit assembly location wins over adapter base-type inference.
    /// </summary>
    /// <returns>A task that completes when the plugin startup check finishes.</returns>
    [Test]
    public async Task StartPluginUsesExplicitAssemblyLocationForManifestIdentity()
    {
        var tempRoot = CreateTempDirectory();
        var mockContainer = new MockContainer(
            new MockDalamudConfiguration
            {
                CreateWindow = false,
                GamePath = ResolveGamePath(),
                PluginSavePath = tempRoot,
            },
            containerBuildHook: null,
            serviceReplacements: null,
            askPath: false);

        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(AssemblyLocationAdapterPlugin));
        var assemblyLocation = typeof(PluginLoaderTests).Assembly.Location;
        var pluginLoadSettings = new PluginLoadSettings(
            tempRoot,
            new FileInfo(Path.Combine(tempRoot.FullName, "AssemblyLocationAdapterPlugin.json")))
        {
            AssemblyLocation = assemblyLocation,
        };

        var started = await pluginLoader.StartPlugin(mockPlugin, pluginLoadSettings).ConfigureAwait(false);

        Assert.That(started, Is.True);
        Assert.That(mockPlugin.Container, Is.Not.Null);

        var manifest = mockPlugin.Container!.Resolve<MockPluginManifest>();
        Assert.That(manifest.InternalName, Is.EqualTo(Path.GetFileNameWithoutExtension(assemblyLocation)));

        await pluginLoader.StopPlugin(mockPlugin).ConfigureAwait(false);
    }

    private static DirectoryInfo CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "DalaMock.Sample.Tests", Guid.NewGuid().ToString("N"));
        return Directory.CreateDirectory(path);
    }

    private static DirectoryInfo ResolveGamePath()
    {
        var candidates = new[]
        {
            Environment.GetEnvironmentVariable("EXD_DATA_DIR"),
            TryReadLauncherGamePath() is { } launcherGamePath
                ? Path.Combine(launcherGamePath, "game", "sqpack")
                : null,
            @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game\sqpack",
            @"C:\Program Files\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game\sqpack",
            @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY XIV Online\game\sqpack",
        };

        foreach (var candidate in candidates)
        {
            if (IsValidGamePath(candidate))
            {
                return new DirectoryInfo(candidate!);
            }
        }

        Assert.Inconclusive("No local FFXIV sqpack directory is available for DalaMock container tests.");
        throw new InvalidOperationException("No local FFXIV sqpack directory is available for DalaMock container tests.");
    }

    private static bool IsValidGamePath(string? path)
    {
        return !string.IsNullOrWhiteSpace(path)
            && Directory.Exists(path)
            && Directory.EnumerateDirectories(path).Any(directory => Path.GetFileName(directory) == "ffxiv");
    }

    private static string? TryReadLauncherGamePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var launcherConfigPath = Path.Combine(appDataPath, "XIVLauncher", "launcherConfigV3.json");
        if (!File.Exists(launcherConfigPath))
        {
            return null;
        }

        try
        {
            using var launcherConfigDocument = JsonDocument.Parse(File.ReadAllText(launcherConfigPath));
            return launcherConfigDocument.RootElement.TryGetProperty("GamePath", out var gamePathElement)
                ? gamePathElement.GetString()
                : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed class AssemblyLocationAdapterPlugin : IAsyncDalamudPlugin
    {
        public Task LoadAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
