namespace DalaMock.Core.Fonts.Atlas;

/// <summary>
/// Maps <see cref="DalamudAsset"/> values to bundled embedded resources shipped with DalaMock.
/// </summary>
internal static class MockFontAtlasResources
{
    /// <summary>
    /// Reads the embedded resource bytes for the given <see cref="DalamudAsset"/> font.
    /// Falls back to <c>gf.ttf</c> for unrecognized values so plugin builds never throw.
    /// </summary>
    /// <returns>The asset as a byte array.</returns>
    public static byte[] LoadAssetBytes(DalamudAsset asset)
    {
        var resourceName = asset switch
        {
            DalamudAsset.NotoSansCjkMedium => "NotoSansCJK-Medium.ttc",
            DalamudAsset.NotoSansCjkRegular => "NotoSansCJK-Regular.ttc",
            DalamudAsset.InconsolataRegular => "Inconsolata-Regular.ttf",
            DalamudAsset.FontAwesomeFreeSolid => "FontAwesome710FreeSolid.otf",
            DalamudAsset.LodestoneGameSymbol => "gf.ttf",
            _ => "gf.ttf",
        };

        return LoadEmbeddedResource(resourceName);
    }

    /// <summary>
    /// Reads an embedded font resource from the DalaMock assembly by its logical name.
    /// </summary>
    /// <returns>The embedded resource as a byte array.</returns>
    public static byte[] LoadEmbeddedResource(string resourceName)
    {
        var assembly = typeof(MockFontAtlasResources).Assembly;
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException(
                $"Embedded resource '{resourceName}' not found in {assembly.GetName().Name}.");
        }

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
