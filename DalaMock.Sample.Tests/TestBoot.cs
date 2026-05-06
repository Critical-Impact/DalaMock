using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using DalaMock.Core.Configuration;
using DalaMock.Core.Plugin;
using DalaMock.Host.Mediator;
using Microsoft.Extensions.Hosting;

namespace DalaMock.Sample.Tests;

/// <summary>
/// Responsible for constructing the <see cref="IHost"/> used by all tests.
/// Configures a <see cref="MockContainer"/> with a minimal Dalamud environment
/// (no window creation), registers the <see cref="MediatorService"/>, loads
/// <see cref="TestDalamudPluginTest"/> as the plugin under test, and returns
/// the host that was built inside that plugin.
/// </summary>
public class TestBoot
{
    /// <summary>
    /// Builds and returns the <see cref="IHost"/> owned by the test plugin.
    /// </summary>
    /// <remarks>
    /// The method creates a <see cref="MockContainer"/> configured to suppress any
    /// real Dalamud window, registers <see cref="MediatorService"/> into Autofac,
    /// then loads and starts <see cref="TestDalamudPluginTest"/>. Plug-in load
    /// settings point at the running test assembly so that DalaMock can locate the
    /// correct binaries. Throws <see cref="Exception"/> if the container, plugin,
    /// or host were not constructed successfully.
    /// </remarks>
    /// <returns>The <see cref="IHost"/> built during plugin startup.</returns>
    public async Task<IHost> CreateHost()
    {
        //Creating the mock container with no window will replace ITextureProvider, IUiBuilder, IKeyState with empty versions and create no imgui window.
        var mockContainer = new MockContainer(new MockDalamudConfiguration()
        {
            CreateWindow = false,
        }, builder =>
        {
            builder.RegisterType<MediatorService>();
        },[], false);
        var pluginLoader = mockContainer.GetPluginLoader();
        var mockPlugin = pluginLoader.AddPlugin(typeof(TestDalamudPluginTest));
        var pluginLoadSettings = new PluginLoadSettings(new DirectoryInfo(Environment.CurrentDirectory), new FileInfo(Path.Combine(Environment.CurrentDirectory, "test.json")));
        pluginLoadSettings.AssemblyLocation = this.GetType().Assembly.Location;
        await pluginLoader.StartPlugin(mockPlugin, pluginLoadSettings);
        if (mockPlugin.Container == null)
        {
            throw new Exception("Container was not built.");
        }

        if (mockPlugin.DalamudPlugin is not TestDalamudPluginTest testingPlugin)
        {
            throw new Exception("Plugin was not built.");
        }

        if (testingPlugin.Host == null)
        {
            throw new Exception("Host was not built.");
        }
        return testingPlugin.Host;
    }
}