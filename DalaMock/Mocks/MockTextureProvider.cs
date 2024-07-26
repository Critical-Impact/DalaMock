using IBitmapCodecInfo = Dalamud.Interface.Textures.IBitmapCodecInfo;
using ImGuiViewportTextureArgs = Dalamud.Interface.Textures.ImGuiViewportTextureArgs;
using ISharedImmediateTexture = Dalamud.Interface.Textures.ISharedImmediateTexture;
using RawImageSpecification = Dalamud.Interface.Textures.RawImageSpecification;
using TextureModificationArgs = Dalamud.Interface.Textures.TextureModificationArgs;

namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Plugin.Services;
using Lumina.Data.Files;

public class MockTextureProvider : ITextureProvider, IMockService
{
    private readonly MockTextureManager mockTextureManager;

    public MockTextureProvider(MockTextureManager mockTextureManager)
    {
        this.mockTextureManager = mockTextureManager;
    }

    public string ServiceName { get; } = "Texture Provider";

    public IDalamudTextureWrap CreateEmpty(
        RawImageSpecification specs,
        bool cpuRead,
        bool cpuWrite,
        string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromExistingTextureAsync(
        IDalamudTextureWrap wrap,
        TextureModificationArgs args,
        bool leaveWrapOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImGuiViewportAsync(
        ImGuiViewportTextureArgs args,
        string? debugName = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImageAsync(
        ReadOnlyMemory<byte> bytes,
        string? debugName = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromImageAsync(
        Stream stream,
        bool leaveOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public IDalamudTextureWrap CreateFromRaw(
        RawImageSpecification specs,
        ReadOnlySpan<byte> bytes,
        string? debugName = null)
    {
        var pixelFormat = specs.DxgiFormat switch
        {
            // DXGI_FORMAT_R8G8B8A8_UNORM
            28 => Veldrid.PixelFormat.R8_G8_B8_A8_UNorm,

            // DXGI_FORMAT_B8G8R8A8_UNORM
            87 => Veldrid.PixelFormat.B8_G8_R8_A8_UNorm,
            _ => throw new ArgumentOutOfRangeException(),
        };
        return this.mockTextureManager.LoadImageRaw(bytes.ToArray(), specs.Width, specs.Height, pixelFormat);
    }

    public Task<IDalamudTextureWrap> CreateFromRawAsync(
        RawImageSpecification specs,
        ReadOnlyMemory<byte> bytes,
        string? debugName = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public Task<IDalamudTextureWrap> CreateFromRawAsync(
        RawImageSpecification specs,
        Stream stream,
        bool leaveOpen = false,
        string? debugName = null,
        CancellationToken cancellationToken = new())
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
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IBitmapCodecInfo> GetSupportedImageDecoderInfos()
    {
        throw new NotImplementedException();
    }

    public ISharedImmediateTexture GetFromGameIcon(in GameIconLookup lookup)
    {
        var gamePath = this.mockTextureManager.GetFromGameIcon(lookup);
        if (gamePath == null)
        {
            throw new Exception($"Attempted to load a invalid game icon path: {gamePath}");
        }

        var textureWrap = this.mockTextureManager.GetTextureFromGame(gamePath, true);
        if (textureWrap == null)
        {
            throw new Exception($"Texture wrap created from game icon was invalid: {gamePath}");
        }

        return new ForwardingSharedImmediateTexture(textureWrap);
    }

    public bool TryGetFromGameIcon(in GameIconLookup lookup, out ISharedImmediateTexture texture)
    {
        texture = null!;
        var gamePath = this.mockTextureManager.GetFromGameIcon(lookup);
        if (gamePath == null)
        {
            return false;
        }

        var textureWrap = this.mockTextureManager.GetTextureFromGame(gamePath, true);
        if (textureWrap == null)
        {
            return false;
        }

        texture = new ForwardingSharedImmediateTexture(textureWrap);
        return true;
    }

    public ISharedImmediateTexture GetFromGame(string path)
    {
        var textureFile = this.mockTextureManager.GetTextureFromGame(path);
        if (textureFile == null)
        {
            throw new Exception($"Failed to create texture {textureFile}");
        }

        return new ForwardingSharedImmediateTexture(textureFile);
    }

    public ISharedImmediateTexture GetFromFile(string path)
    {
        return this.GetFromFile(new FileInfo(path));
    }

    public ISharedImmediateTexture GetFromFile(FileInfo file)
    {
        var textureFile = this.mockTextureManager.GetTextureFromFile(file);
        if (textureFile == null)
        {
            throw new Exception($"Failed to create texture {textureFile}");
        }

        return new ForwardingSharedImmediateTexture(textureFile);
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

    public bool TryGetIconPath(in GameIconLookup lookup, out string? path)
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
}