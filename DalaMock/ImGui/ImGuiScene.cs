using System.IO;
using System.Runtime.InteropServices;

namespace DalaMock.Core.Imgui;

using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Utility;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using static System.Reflection.BindingFlags;

/// <summary>
/// Simple class to wrap everything necessary to use ImGui inside a window.
/// </summary>
public partial class ImGuiScene : IDisposable
{
    private readonly AssertHandler assertHandler;

    public delegate void BuildUiDelegate();

    private readonly Dictionary<Texture, TextureView> autoViewsByTexture = new();
    private readonly List<IDisposable> ownedResources = new();

    private readonly Dictionary<TextureView, ResourceSetInfo> setsByView = new();
    private readonly Dictionary<IntPtr, ResourceSetInfo> viewsById = new();
    private Vector3 backgroundColour = new(0.45f, 0.55f, 0.6f);
    private bool disposedValue;
    private int lastAssignedId = 100;

    /// <summary>
    /// User methods invoked every ImGui frame to construct custom UIs.
    /// </summary>
    public BuildUiDelegate OnBuildUi;

    private bool pauseRendering = false;
    private Vector2 scaleFactor = Vector2.One;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImGuiScene"/> class.
    /// Creates a new window and a new renderer of the specified type, and initializes ImGUI.
    /// </summary>
    /// <param name="createInfo">Creation details for the window.</param>
    /// <param name="assertHandler"></param>
    public ImGuiScene(WindowCreateInfo createInfo, AssertHandler assertHandler)
    {
        this.assertHandler = assertHandler;
        GraphicsDevice graphicsDevice;
        Sdl2Window window;
        VeldridStartup.CreateWindowAndGraphicsDevice(
            createInfo,
            new GraphicsDeviceOptions(false, null, false, ResourceBindingModel.Improved, true, true),
            out window,
            out graphicsDevice);
        this.Window = window;
        this.GraphicsDevice = graphicsDevice;
        this.Window.Resized += () =>
        {
            this.GraphicsDevice.MainSwapchain.Resize((uint)this.Window.Width, (uint)this.Window.Height);
        };

        this.Window.Closed += this.WindowOnClosed;

        this.CommandList = this.GraphicsDevice.ResourceFactory.CreateCommandList();

        var context = ImGui.CreateContext();
        ImGui.SetCurrentContext(context);
        assertHandler.Setup();
        ImGui.GetIO().Fonts.AddFontDefault();
        ImGui.GetIO().BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        ImGui.GetIO().FontGlobalScale = 1;
        var field = typeof(ImGuiHelpers).GetProperty(
            "GlobalScale",
            Static | Public);
        field?.SetValue(null, 1);

        this.CreateDeviceResources(
            this.GraphicsDevice,
            this.GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription);
        this.LoadFont(Path.Combine("Fonts", "FontAwesomeFreeSolid.otf"), 12.0f, 0, new ushort[] { 0xe005, 0xf8ff, 0x00 });
        this.SetKeyMappings();
        this.SetPerFrameImGuiData(0);
        ImGui.NewFrame();
        ImGui.EndFrame();
    }

    /// <summary>
    /// Gets the main application container window where we do all our rendering and input processing.
    /// </summary>
    public Sdl2Window Window { get; init; }

    /// <summary>
    /// Gets the veldrid graphics device.
    /// </summary>
    public GraphicsDevice GraphicsDevice { get; init; }

    /// <summary>
    /// Gets the veldrid commandlist.
    /// </summary>
    public CommandList CommandList { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the user application has requested the system to terminate.
    /// </summary>
    public bool ShouldQuit { get; set; }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Helper method to create a fullscreen window.
    /// </summary>
    /// <param name="assertHandler">The assert handler.</param>
    /// <returns>Returns a imguiscene.</returns>
    public static ImGuiScene CreateWindow(AssertHandler assertHandler)
    {
        var scene = new ImGuiScene(
            new WindowCreateInfo
            {
                WindowTitle = "DalaMock",
                WindowInitialState = WindowState.Maximized,
                WindowWidth = 500,
                WindowHeight = 500,
            },
            assertHandler);
        scene.Window.Opacity = 1;
        return scene;
    }

    /// <summary>
    /// Simple method to run the scene in a loop until the window is closed or the application
    /// requests an exit (via <see cref="ShouldQuit"/>)
    /// </summary>
    public void Run()
    {
        // For now we consider the window closing to be a quit request
        // while ShouldQuit is used for external/application close requests
        while (!this.ShouldQuit)
        {
            this.Update();
        }
    }

    [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong SDL_GetPerformanceFrequency();

    /// <summary>
    /// Performs a single-frame update of ImGui and renders it to the window.
    /// This method does not check any quit conditions.
    /// </summary>
    public void Update()
    {
        var snapshot = this.Window.PumpEvents();

        if (!this.pauseRendering)
        {
            var deltaSeconds = 1000f / SDL_GetPerformanceFrequency();
            this.SetPerFrameImGuiData(deltaSeconds);
            this.UpdateImGuiInput(snapshot);

            ImGui.NewFrame();
            this.OnBuildUi?.Invoke();
            this.CommandList!.Begin();
            this.CommandList.SetFramebuffer(this.GraphicsDevice!.MainSwapchain.Framebuffer);
            this.CommandList.ClearColorTarget(
                0,
                new RgbaFloat(this.backgroundColour.X, this.backgroundColour.Y, this.backgroundColour.Z, 1f));
            ImGui.Render();
            this.RenderImDrawData(ImGui.GetDrawData(), this.GraphicsDevice, this.CommandList);
            this.CommandList.End();
            this.GraphicsDevice.SubmitCommands(this.CommandList);
            this.GraphicsDevice.SwapBuffers(this.GraphicsDevice.MainSwapchain);
        }
    }

    private void WindowOnClosed()
    {
        this.ShouldQuit = true;
    }

    private void SetPerFrameImGuiData(float deltaSeconds)
    {
        var io = ImGui.GetIO();
        io.DisplaySize = new Vector2(
            this.Window.Width / this.scaleFactor.X,
            this.Window.Height / this.scaleFactor.Y);
        io.DisplayFramebufferScale = this.scaleFactor;
        io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
            }
            this.assertHandler.Dispose();
            this.Window.Closed -= this.WindowOnClosed;

            this.Window.Close();

            ImGui.DestroyContext();

            this.vertexBuffer.Dispose();
            this.indexBuffer.Dispose();
            this.projMatrixBuffer.Dispose();
            this.fontTexture?.Dispose();
            this.fontTextureView.Dispose();
            this.vertexShader.Dispose();
            this.fragmentShader.Dispose();
            this.layout.Dispose();
            this.textureLayout.Dispose();
            this.pipeline.Dispose();
            this.mainResourceSet.Dispose();

            this.ownedResources.ForEach(res => res.Dispose());
            this.ownedResources.Clear();

            this.disposedValue = true;
        }
    }

    ~ImGuiScene()
    {
        this.Dispose(false);
    }
}
