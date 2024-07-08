namespace DalaMock.Core.Windows;

using System.IO;
using System.Numerics;
using Autofac;
using DalaMock.Core.Mocks;
using DalaMock.Core.Plugin;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

/// <summary>
/// Provides a window for configuring
/// </summary>
public class MockSettingsWindow : Window
{
    private readonly MockDalamudConfiguration dalamudConfiguration;
    private readonly PluginLoader pluginLoader;

    public MockSettingsWindow(PluginLoader pluginLoader, MockDalamudConfiguration dalamudConfiguration)
        : base(
        "Mock Settings",
        ImGuiWindowFlags.None,
        true)
    {
        this.pluginLoader = pluginLoader;
        this.dalamudConfiguration = dalamudConfiguration;
    }

    public override void Draw()
    {
        var gamePath = this.dalamudConfiguration.GamePath?.FullName ?? string.Empty;

        ImGui.SetNextWindowSize(new Vector2(700, 300));
        if (ImGui.Begin("Mock Settings", ImGuiWindowFlags.None))
        {
            if (ImGui.InputTextWithHint("Game Path##gp", "Please enter your game path", ref gamePath, 999))
            {
                if (gamePath != (this.dalamudConfiguration.GamePath?.FullName ?? string.Empty))
                {
                    this.dalamudConfiguration.GamePath = gamePath == string.Empty ? null : new DirectoryInfo(gamePath);
                }
            }

            var tooltip = "Must be the game/sqpack directory";
            if (tooltip.Length > 0 && ImGui.IsItemHovered(ImGuiHoveredFlags.None))
            {
                using var tt = ImRaii.Tooltip();
                ImGui.TextUnformatted(tooltip);
            }

            if (gamePath != string.Empty && !Directory.Exists(gamePath))
            {
                ImGui.Text("The configured path does not exist.");
            }

            ImGui.NewLine();
            ImGui.Text("Available Plugins: ");
            ImGui.Separator();
            using (var table = ImRaii.Table("Loaded Plugins", 4))
            {
                if (table)
                {
                    foreach (var plugin in this.pluginLoader.LoadedPlugins)
                    {
                        ImGui.TableNextRow();
                        var push = ImRaii.PushId(plugin.Key.Name);
                        ImGui.TableNextColumn();
                        ImGui.TextWrapped(plugin.Value.PluginType.ToString());
                        ImGui.TableNextColumn();
                        ImGui.Text(plugin.Value.IsLoaded ? "Loaded" : "Not Loaded");
                        ImGui.TableNextColumn();
                        if (ImGui.Button(plugin.Value.IsLoaded ? "Unload" : "Load"))
                        {
                            if (plugin.Value.IsLoaded)
                            {
                                this.pluginLoader.StopPlugin(plugin.Value);
                            }
                            else
                            {
                                this.pluginLoader.StartPlugin(
                                    plugin.Value,
                                    new PluginLoadSettings(
                                        new DirectoryInfo("C:\\Users\\Blair\\RiderProjects\\FookOff"),
                                        new FileInfo("C:\\Users\\Blair\\RiderProjects\\FookOff\\FookOff.json")));

                                // TODO: Need to make this configurable.
                            }
                        }

                        ImGui.TableNextColumn();
                        if (plugin.Value.IsLoaded && ImGui.Button("Settings"))
                        {
                            if (plugin.Value.Container != null)
                            {
                                var uiBuilder = plugin.Value.Container.Resolve<MockUiBuilder>();
                                uiBuilder.FireOpenConfigUiEvent();
                            }
                        }

                        if (plugin.Value.IsLoaded && ImGui.Button("Main UI"))
                        {
                            if (plugin.Value.Container != null)
                            {
                                var uiBuilder = plugin.Value.Container.Resolve<MockUiBuilder>();
                                uiBuilder.FireOpenMainUiEvent();
                            }
                        }

                        push.Pop();
                    }
                }
            }
        }

        ImGui.End();
    }
}