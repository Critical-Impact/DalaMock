// using System.Numerics;
// using Dalamud.Interface.Utility.Raii;
// using Dalamud.Interface.Windowing;
// using ImGuiNET;
//
// namespace DalaMock.Core.Windows;
//
// using System.IO;
//
// public class MockSettingsWindow : Window
// {
//     private readonly MockProgram _mockProgram;
//
//     public MockSettingsWindow(MockProgram mockProgram, string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow)
//     {
//         _mockProgram = mockProgram;
//     }
//
//     public MockSettingsWindow(MockProgram mockProgram) : base("Mock Settings", ImGuiWindowFlags.None, true)
//     {
//         _mockProgram = mockProgram;
//     }
//
//     public override void Draw()
//     {
//         var gamePath = AppSettings.Default.GamePath ?? "";
//         var pluginConfigPath = AppSettings.Default.PluginConfigPath ?? "";
//         var pluginInternalName = AppSettings.Default.PluginInternalName ?? "";
//         var autoStart = AppSettings.Default.AutoStart;
//
//
//
//         ImGui.SetNextWindowSize(new Vector2(500,200));
//         if (ImGui.Begin("Mock Settings", ImGuiWindowFlags.None))
//         {
//             if (ImGui.InputTextWithHint("Game Path##gp", "Please enter your game path", ref gamePath, 999))
//             {
//                 if (gamePath != AppSettings.Default.GamePath)
//                 {
//                     AppSettings.Default.GamePath = gamePath;
//                 }
//             }
//
//             var tooltip = "Must be the game/sqpack directory";
//             if (tooltip.Length > 0 && ImGui.IsItemHovered(ImGuiHoveredFlags.None))
//             {
//                 using var tt = ImRaii.Tooltip();
//                 ImGui.TextUnformatted(tooltip);
//             }
//
//             if (gamePath != "" && !Directory.Exists(gamePath))
//             {
//                 ImGui.Text("The configured path does not exist.");
//             }
//
//             if (ImGui.InputTextWithHint("Plugin Config Path##pcp", "Please enter your plugin config path", ref pluginConfigPath, 999))
//             {
//                 if (pluginConfigPath != AppSettings.Default.PluginConfigPath)
//                 {
//                     AppSettings.Default.PluginConfigPath = pluginConfigPath;
//                 }
//             }
//
//             if (pluginConfigPath != "" && !Directory.Exists(pluginConfigPath))
//             {
//                 ImGui.Text("The configured path does not exist.");
//             }
//
//             if (ImGui.InputTextWithHint("Plugin Internal Name##pin", "Please enter your plugin's internal name", ref pluginInternalName, 999))
//             {
//                 if (pluginInternalName != AppSettings.Default.PluginInternalName)
//                 {
//                     AppSettings.Default.PluginInternalName = pluginInternalName;
//                 }
//             }
//
//             if (pluginConfigPath != "" && !Directory.Exists(pluginConfigPath))
//             {
//                 ImGui.Text("The configured path does not exist.");
//             }
//
//
//             if (ImGui.Checkbox("Auto-start?", ref autoStart))
//             {
//                 if (autoStart != AppSettings.Default.AutoStart)
//                 {
//                     AppSettings.Default.AutoStart = autoStart;
//                 }
//             }
//
//             if(AppSettings.Dirty && ImGui.Button("Save"))
//             {
//                 AppSettings.Default.Save();
//             }
//
//             if (Directory.Exists(gamePath) && Directory.Exists(pluginConfigPath))
//             {
//                 if (ImGui.Button("Start Plugin"))
//                 {
//                     _mockProgram.StartPlugin();
//                 }
//                 else if (ImGui.Button("Stop Plugin"))
//                 {
//                     _mockProgram.StopPlugin();
//                 }
//             }
//         }
//
//         ImGui.End();
//     }
// }

