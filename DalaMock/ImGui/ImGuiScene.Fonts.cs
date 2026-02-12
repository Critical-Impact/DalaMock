namespace DalaMock.Core.Imgui;

using System;
using System.IO;
using System.Reflection;

using Dalamud.Bindings.ImGui;

public partial class ImGuiScene
{
    public unsafe ImFontPtr LoadFontFromEmbeddedResource(
        string resourceName,
        float fontSize,
        ushort* glyphRanges = null,
        bool mergeMode = false)
    {
        var io = ImGui.GetIO();

        using var stream = typeof(ImGuiScene).Assembly
                                             .GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException($"Missing resource {resourceName}");

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var fontData = ms.ToArray();

        var fontConfig = ImGui.ImFontConfig();
        fontConfig.FontNo = 0;
        fontConfig.GlyphRanges = glyphRanges;
        fontConfig.FontDataOwnedByAtlas = true;
        fontConfig.MergeMode = mergeMode;

        var font = io.Fonts.AddFontFromMemoryTTF(
            fontData,
            fontSize,
            fontConfig,
            glyphRanges);

        fontConfig.Destroy();
        return font;
    }

    private byte[] LoadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var fullResourceName = typeof(ImGuiScene).Assembly.GetManifestResourceStream(resourceName);

        if (fullResourceName == null)
        {
            throw new FileNotFoundException(
                $"Embedded resource '{resourceName}' not found. Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");
        }

        using var stream = typeof(ImGuiScene).Assembly
                                             .GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException($"Missing resource {resourceName}");
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not load embedded resource '{resourceName}'");
        }

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
