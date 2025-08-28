namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Plugin.Services;

using Lumina.Data.Files;

public class NullTextureProvider : ITextureProvider, IMockService
{
    public IDalamudTextureWrap CreateEmpty(RawImageSpecification specs, bool cpuRead, bool cpuWrite, string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public IDrawListTextureWrap CreateDrawListTexture(string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromExistingTextureAsync(
        IDalamudTextureWrap wrap,
        TextureModificationArgs args,
        bool leaveWrapOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImGuiViewportAsync(
        ImGuiViewportTextureArgs args,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImageAsync(
        ReadOnlyMemory<byte> bytes,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImageAsync(
        Stream stream,
        bool leaveOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public IDalamudTextureWrap CreateFromRaw(RawImageSpecification specs, ReadOnlySpan<byte> bytes, string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromRawAsync(
        RawImageSpecification specs,
        ReadOnlyMemory<byte> bytes,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromRawAsync(
        RawImageSpecification specs,
        Stream stream,
        bool leaveOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public IDalamudTextureWrap CreateFromTexFile(TexFile file)
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromTexFileAsync(
        TexFile file,
        string? debugName = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromClipboardAsync(string? debugName = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IBitmapCodecInfo> GetSupportedImageDecoderInfos()
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromGameIcon(in GameIconLookup lookup)
    {
        throw new NotImplementedException();
    }

    public bool HasClipboardImage()
    {
        throw new NotImplementedException();
    }

    public bool TryGetFromGameIcon(in GameIconLookup lookup, [NotNullWhen(true)] out ISharedImmediateTexture? texture)
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromGame(string path)
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromFile(string path)
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromFile(FileInfo file)
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromFileAbsolute(string fullPath)
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromManifestResource(Assembly assembly, string name)
    {
        throw new NotImplementedException();
    }

    public string GetIconPath(in GameIconLookup lookup)
    {
        throw new NotImplementedException();
    }

    public bool TryGetIconPath(in GameIconLookup lookup, [NotNullWhen(true)] out string? path)
    {
        throw new NotImplementedException();
    }

    public bool IsDxgiFormatSupported(int dxgiFormat)
    {
        throw new NotImplementedException();
    }

    public bool IsDxgiFormatSupportedForCreateFromExistingTextureAsync(int dxgiFormat)
    {
        throw new NotImplementedException();
    }

    public IntPtr ConvertToKernelTexture(IDalamudTextureWrap wrap, bool leaveWrapOpen = false)
    {
        throw new NotImplementedException();
    }

    public string ServiceName { get; set; } = "Texture Provider";
}
