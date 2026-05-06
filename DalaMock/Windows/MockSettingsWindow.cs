namespace DalaMock.Core.Windows;

/// <summary>Represents the validity state of a configured path.</summary>
public enum PathState
{
    /// <summary>The path is configured and valid.</summary>
    Valid,

    /// <summary>The path does not exist on disk.</summary>
    DoesNotExist,

    /// <summary>The path exists but is not valid for its intended purpose.</summary>
    Invalid,
}

/// <summary>
/// Provides a tab-based settings window for configuring DalaMock.
/// </summary>
public class MockSettingsWindow : Window
{
    private readonly MockDalamudConfiguration dalamudConfiguration;
    private readonly IFileDialogManager fileDialogManager;
    private readonly MockConfigurationManager configurationManager;
    private readonly MockKeyState mockKeyState;

    private int clientLanguageIndex;
    private bool createWindow;
    private string languageOverride = string.Empty;

    private static readonly string[] LanguageNames = ["English", "Japanese", "German", "French"];
    private static readonly ClientLanguage[] LanguageValues =
    [
        ClientLanguage.English,
        ClientLanguage.Japanese,
        ClientLanguage.German,
        ClientLanguage.French,
    ];

    public MockSettingsWindow(
        MockDalamudConfiguration dalamudConfiguration,
        IFileDialogManager fileDialogManager,
        MockConfigurationManager configurationManager,
        MockKeyState mockKeyState)
        : base("DalaMock Settings", ImGuiWindowFlags.None, false)
    {
        this.dalamudConfiguration = dalamudConfiguration;
        this.fileDialogManager = fileDialogManager;
        this.configurationManager = configurationManager;
        this.mockKeyState = mockKeyState;

        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(520, 350),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue),
        };

        if (this.GetGamePathState() != PathState.Valid || this.GetPluginPathState() != PathState.Valid)
        {
            this.IsOpen = true;
        }
    }

    public override void OnOpen()
    {
        this.clientLanguageIndex = System.Array.IndexOf(LanguageValues, this.dalamudConfiguration.ClientLanguage);
        if (this.clientLanguageIndex < 0)
        {
            this.clientLanguageIndex = 0;
        }

        this.createWindow = this.dalamudConfiguration.CreateWindow;
        this.languageOverride = this.dalamudConfiguration.LanguageOverride ?? string.Empty;
    }

    public override void Draw()
    {
        using (var tabBar = ImRaii.TabBar("##SettingsTabs"))
        {
            if (tabBar)
            {
                using (var tab = ImRaii.TabItem("General"))
                {
                    if (tab)
                    {
                        this.DrawGeneralTab();
                    }
                }

                using (var tab = ImRaii.TabItem("Paths"))
                {
                    if (tab)
                    {
                        this.DrawPathsTab();
                    }
                }
            }
        }

        ImGui.Separator();
        this.DrawSaveButton();
    }

    private void DrawGeneralTab()
    {
        using var child = ImRaii.Child("##GeneralTabContent", new Vector2(0, -ImGui.GetFrameHeightWithSpacing() - ImGui.GetStyle().ItemSpacing.Y), false);

        ImGui.SetNextItemWidth(200);
        ImGui.Combo("Client Language##clientLang", ref this.clientLanguageIndex, LanguageNames, LanguageNames.Length);

        ImGui.NewLine();

        ImGui.Checkbox("Create ImGui Window##createWindow", ref this.createWindow);
        using (ImRaii.PushColor(ImGuiCol.Text, new Vector4(0.6f, 0.6f, 0.6f, 1f)))
        {
            ImGui.TextUnformatted("Requires restart to take effect.");
        }

        ImGui.NewLine();

        ImGui.SetNextItemWidth(120);
        ImGui.InputTextWithHint("Language Override##langOverride", "e.g. de, fr, ja", ref this.languageOverride, 8);
        using (ImRaii.PushColor(ImGuiCol.Text, new Vector4(0.6f, 0.6f, 0.6f, 1f)))
        {
            ImGui.TextUnformatted("Leave empty to use system locale.");
        }
    }

    private void DrawPathsTab()
    {
        this.fileDialogManager.Draw();
        this.DrawGamePathSelector();
        this.DrawPluginPathSelector();
    }

    private void DrawSaveButton()
    {
        if (ImGui.Button("Save"))
        {
            this.dalamudConfiguration.ClientLanguage = LanguageValues[this.clientLanguageIndex];
            this.dalamudConfiguration.CreateWindow = this.createWindow;
            this.dalamudConfiguration.LanguageOverride = string.IsNullOrEmpty(this.languageOverride) ? null : this.languageOverride;
            this.configurationManager.SaveConfiguration(this.dalamudConfiguration);
        }
    }

    private void DrawGamePathSelector()
    {
        var gamePath = this.dalamudConfiguration.GamePath?.FullName ?? string.Empty;
        using var gamePathDisabled = ImRaii.Disabled(false);

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

        var tooltip = "Must be the game/sqpack directory";
        if (tooltip.Length > 0 && ImGui.IsItemHovered(ImGuiHoveredFlags.None))
        {
            using var tt = ImRaii.Tooltip();
            ImGui.TextUnformatted(tooltip);
        }

        ImGui.TextUnformatted(this.GetGamePathStatus());

        ImGui.NewLine();
    }

    private PathState GetGamePathState()
    {
        var path = this.dalamudConfiguration.GamePath?.FullName ?? string.Empty;
        if (path != string.Empty && !Directory.Exists(path))
        {
            return PathState.DoesNotExist;
        }

        if (!this.dalamudConfiguration.GamePathValid)
        {
            return PathState.Invalid;
        }

        return PathState.Valid;
    }

    private PathState GetPluginPathState()
    {
        var path = this.dalamudConfiguration.PluginSavePath?.FullName ?? string.Empty;
        if (path != string.Empty && !Directory.Exists(path))
        {
            return PathState.DoesNotExist;
        }

        if (!this.dalamudConfiguration.PluginSavePathValid)
        {
            return PathState.Invalid;
        }

        return PathState.Valid;
    }

    private string GetGamePathStatus() => this.GetGamePathState() switch
    {
        PathState.DoesNotExist => "The configured path does not exist.",
        PathState.Invalid => "The configured path is not valid.",
        _ => "The configured path is valid.",
    };

    private string GetPluginPathStatus() => this.GetPluginPathState() switch
    {
        PathState.DoesNotExist => "The configured path does not exist.",
        PathState.Invalid => "The configured path is not valid.",
        _ => "The configured path is valid.",
    };

    private void DrawPluginPathSelector()
    {
        var pluginSavePath = this.dalamudConfiguration.PluginSavePath?.FullName ?? string.Empty;

        if (ImGui.InputTextWithHint("Plugin Save Path##psp", "Please enter the default plugin save path", ref pluginSavePath, 999))
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

        ImGui.TextUnformatted(this.GetPluginPathStatus());

        ImGui.NewLine();
    }
}
