namespace DalaMock.Core.Mocks.Textures;

using System;
using System.Numerics;
using Dalamud.Interface.Textures.TextureWraps;

public class MockTextureManagerTextureWrap : IDalamudTextureWrap
{
    private readonly bool keepAlive;
    private readonly MockTextureManager manager;
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InventoryToolsMock.TextureManagerTextureWrap" /> class.
    /// </summary>
    /// <param name="path">The path to the texture.</param>
    /// <param name="extents">The extents of the texture.</param>
    /// <param name="keepAlive">Keep alive or not.</param>
    /// <param name="manager">Manager that we obtained this from.</param>
    public MockTextureManagerTextureWrap(
        string path,
        Vector2 extents,
        bool keepAlive,
        MockTextureManager manager)
    {
        this.path = path;
        this.keepAlive = keepAlive;
        this.manager = manager;
        this.Width = (int)extents.X;
        this.Height = (int)extents.Y;
    }

    /// <summary>
    /// Gets a value indicating whether or not this wrap has already been disposed.
    /// If true, the handle may be invalid.
    /// </summary>
    internal bool IsDisposed { get; private set; }

    /// <inheritdoc />
    public IntPtr ImGuiHandle
    {
        get
        {
            if (this.IsDisposed)
            {
                throw new InvalidOperationException("Texture already disposed. You may not render it.");
            }

            return this.manager.GetInfo(this.path).Wrap!.ImGuiHandle;
        }
    }

    /// <inheritdoc />
    public int Width { get; private set; }

    /// <inheritdoc />
    public int Height { get; private set; }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (this)
        {
            if (!this.IsDisposed)
            {
                this.manager.NotifyTextureDisposed(this.path, this.keepAlive);
            }

            this.IsDisposed = true;
        }
    }
}