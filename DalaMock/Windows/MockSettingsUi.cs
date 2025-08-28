namespace DalaMock.Core.Windows;

using System.IO;
using System.Numerics;

using Autofac;

using DalaMock.Core.Configuration;
using DalaMock.Core.Mocks;
using DalaMock.Core.Plugin;
using DalaMock.Shared.Interfaces;

using Dalamud.Bindings.ImGui;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;

/// <summary>
/// Provides a window for configuring.
/// </summary>
public class MockSettingsWindow : Window
{
    private readonly MockDalamudConfiguration dalamudConfiguration;
    private readonly PluginLoader pluginLoader;
    private readonly IFileDialogManager fileDialogManager;
    private readonly ConfigurationManager configurationManager;

    public MockSettingsWindow(PluginLoader pluginLoader, MockDalamudConfiguration dalamudConfiguration, IFileDialogManager fileDialogManager, ConfigurationManager configurationManager)
        : base(
        "Mock Settings",
        ImGuiWindowFlags.None,
        true)
    {
        this.pluginLoader = pluginLoader;
        this.dalamudConfiguration = dalamudConfiguration;
        this.fileDialogManager = fileDialogManager;
        this.configurationManager = configurationManager;
        this.IsOpen = true;
    }

    public override void Draw()
    {
        ImGui.SetNextWindowSize(new Vector2(700, 300));
        if (ImGui.Begin("Mock Settings", ImGuiWindowFlags.None))
        {
            this.DrawGamePathSelector();
            this.DrawPluginPathSelector();

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
                        var disabled = ImRaii.Disabled(!this.dalamudConfiguration.GamePathValid);
                        if (ImGui.Button(plugin.Value.IsLoaded ? "Unload" : "Load"))
                        {
                            if (plugin.Value.IsLoaded)
                            {
                                this.pluginLoader.StopPlugin(plugin.Value);
                            }
                            else
                            {
                                if (this.dalamudConfiguration.PluginSavePath != null)
                                {
                                    this.pluginLoader.StartPlugin(plugin.Value);
                                }

                                // TODO: Need to make this configurable.
                            }
                        }

                        disabled.Dispose();

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

    private void DrawGamePathSelector()
    {
        var gamePath = this.dalamudConfiguration.GamePath?.FullName ?? string.Empty;
        var gamePathDisabled = ImRaii.Disabled(this.pluginLoader.HasPluginsStarted);
        this.fileDialogManager.Draw();
        if (ImGui.InputTextWithHint("Game Path##gp", "Please enter your game path", ref gamePath, 999))
        {
            if (gamePath != (this.dalamudConfiguration.GamePath?.FullName ?? string.Empty))
            {
                this.dalamudConfiguration.GamePath = gamePath == string.Empty ? null : new DirectoryInfo(gamePath);
                this.configurationManager.SaveConfiguration(this.dalamudConfiguration);
            }
        }

        if (ImGui.Button("Select Folder##gamePathSelector"))
        {
            this.fileDialogManager.OpenFolderDialog(
                "Select Folder",
                (b, s) =>
                {
                    if (b)
                    {
                        if (s != (this.dalamudConfiguration.GamePath?.FullName ?? string.Empty))
                        {
                            this.dalamudConfiguration.GamePath = s == string.Empty ? null : new DirectoryInfo(s);
                            this.configurationManager.SaveConfiguration(this.dalamudConfiguration);
                        }
                    }
                });
        }

        gamePathDisabled.Dispose();

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
        else if (!this.dalamudConfiguration.GamePathValid)
        {
            ImGui.Text("The configured path is not valid.");
        }
        else
        {
            ImGui.Text("The configured path is valid.");
        }

        ImGui.NewLine();
    }

    private void DrawPluginPathSelector()
    {
        var pluginSavePath = this.dalamudConfiguration.PluginSavePath?.FullName ?? string.Empty;
        var pluginSavePathDisabled = ImRaii.Disabled(this.pluginLoader.HasPluginsStarted);
        this.fileDialogManager.Draw();
        if (ImGui.InputTextWithHint("Plugin Save Path##gp", "Please enter the default plugin save path", ref pluginSavePath, 999))
        {
            if (pluginSavePath != (this.dalamudConfiguration.PluginSavePath?.FullName ?? string.Empty))
            {
                this.dalamudConfiguration.PluginSavePath = pluginSavePath == string.Empty ? null : new DirectoryInfo(pluginSavePath);
                this.configurationManager.SaveConfiguration(this.dalamudConfiguration);
            }
        }

        if (ImGui.Button("Select Folder##pluginSelectFolder"))
        {
            this.fileDialogManager.OpenFolderDialog(
                "Select Folder",
                (b, s) =>
                {
                    if (b)
                    {
                        if (s != (this.dalamudConfiguration.PluginSavePath?.FullName ?? string.Empty))
                        {
                            this.dalamudConfiguration.PluginSavePath = s == string.Empty ? null : new DirectoryInfo(s);
                            this.configurationManager.SaveConfiguration(this.dalamudConfiguration);
                        }
                    }
                });
        }

        pluginSavePathDisabled.Dispose();

        if (pluginSavePath != string.Empty && !Directory.Exists(pluginSavePath))
        {
            ImGui.Text("The configured path does not exist.");
        }
        else if (!this.dalamudConfiguration.PluginSavePathValid)
        {
            ImGui.Text("The configured path is not valid.");
        }
        else
        {
            ImGui.Text("The configured path is valid.");
        }

        ImGui.NewLine();
    }
}
