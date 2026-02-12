namespace DalaMock.Core.Imgui;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Dalamud.Bindings.ImGui;
using Dalamud.Game.Text;
using Dalamud.Interface.Utility;
using Newtonsoft.Json;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

using static System.Reflection.BindingFlags;

/// <summary>
/// Simple class to wrap everything necessary to use ImGui inside a window.
/// </summary>
public partial class ImGuiScene : IDisposable
{
    private const string StateFilePath = "dalamock_ui.json";
    private const int DebounceDelayMs = 500;
    private static readonly char SeIconCharMin = (char)Enum.GetValues<SeIconChar>().Min();
    private static readonly char SeIconCharMax = (char)Enum.GetValues<SeIconChar>().Max();
    private readonly AssertHandler assertHandler;
    private readonly Dictionary<Texture, TextureView> autoViewsByTexture = new();
    private readonly List<IDisposable> ownedResources = new();
    private readonly Dictionary<TextureView, ResourceSetInfo> setsByView = new();
    private readonly Dictionary<IntPtr, ResourceSetInfo> viewsById = new();
    private readonly Vector3 backgroundColour = new(0.45f, 0.55f, 0.6f);
    private readonly byte[] gameFontData;
    private readonly unsafe ushort* gameGlyphRanges;
    private readonly bool pauseRendering = false;
    private readonly Vector2 scaleFactor = Vector2.One;

    private MockWindowState currentWindowState;
    private CancellationTokenSource debounceCts;

    private bool disposedValue;
    private int lastAssignedId = 100;

    /// <summary>
    /// User methods invoked every ImGui frame to construct custom UIs.
    /// </summary>
    public event BuildUiDelegate OnBuildUi;

    public delegate void BuildUiDelegate();

    /// <summary>
    /// Initializes a new instance of the <see cref="ImGuiScene"/> class.
    /// Creates a new window and a new renderer of the specified type, and initializes ImGUI.
    /// </summary>
    /// <param name="createInfo">Creation details for the window.</param>
    /// <param name="assertHandler"></param>
    public unsafe ImGuiScene(WindowCreateInfo createInfo, AssertHandler assertHandler)
    {
        this.assertHandler = assertHandler;
        GraphicsDevice graphicsDevice;
        Sdl2Window window;
        VeldridStartup.CreateWindowAndGraphicsDevice(
            createInfo,
            new GraphicsDeviceOptions(false, null, false, ResourceBindingModel.Improved, true, true),
            out window,
            out graphicsDevice);
        this.Window = new RawSdl2Window(window);
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

        this.gameFontData = this.LoadEmbeddedResource("gf.ttf");
        this.gameGlyphRanges = this.GetGameGlyphRanges();

        this.BuildDefaultFonts();

        ImGui.GetIO().BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        ImGui.GetIO().FontGlobalScale = 1;
        var field = typeof(ImGuiHelpers).GetProperty(
            "GlobalScale",
            Static | Public);
        field?.SetValue(null, 1);

        this.CreateDeviceResources(
            this.GraphicsDevice,
            this.GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription);
        this.SetKeyMappings();
        this.SetPerFrameImGuiData(0);
        ImGui.NewFrame();
        ImGui.EndFrame();

        this.currentWindowState = this.CaptureWindowState();
    }

    private unsafe void BuildDefaultFonts()
    {
        var baseFontSize = (12 * 4.0f) / 3.0f;
        var gameGlyphSize = baseFontSize * 2.0f;

        ImGui.GetIO().Fonts.AddFontDefault(null);
        this.AddFontFromMemory(this.gameFontData, gameGlyphSize, this.gameGlyphRanges, mergeMode: true);

        this.LoadFontFromEmbeddedResource(
            "NotoSansCJKjp-Medium.otf",
            baseFontSize,
            ImGui.GetIO().Fonts.GetGlyphRangesJapanese());
        this.AddFontFromMemory(this.gameFontData, gameGlyphSize, this.gameGlyphRanges, mergeMode: true);

        this.LoadFontFromEmbeddedResource(
            "Inconsolata-Regular.ttf",
            baseFontSize,
            ImGui.GetIO().Fonts.GetGlyphRangesDefault());
        this.AddFontFromMemory(this.gameFontData, gameGlyphSize, this.gameGlyphRanges, mergeMode: true);

        this.LoadFontFromEmbeddedResource(
            "FontAwesomeFreeSolid.otf",
            baseFontSize,
            GetFontAwesomeRanges());
        this.AddFontFromMemory(this.gameFontData, gameGlyphSize, this.gameGlyphRanges, mergeMode: true);

        ImGui.GetIO().Fonts.Build();
    }

    private unsafe ushort* GetGameGlyphRanges()
    {
        var builder = ImGuiNative.ImFontGlyphRangesBuilder();

        for (char c = SeIconCharMin; c <= SeIconCharMax; c++)
        {
            ImGuiNative.AddChar(builder, c);
        }

        ImVector<ushort> ranges;
        ImGuiNative.BuildRanges(builder, &ranges);

        return ranges.Data;
    }

    private unsafe void AddFontFromMemory(byte[] fontData, float size, ushort* glyphRanges, bool mergeMode)
    {
        var fontConfig = ImGui.ImFontConfig();
        fontConfig.MergeMode = mergeMode;
        fontConfig.GlyphMinAdvanceX = 13.0f;
        fontConfig.FontDataOwnedByAtlas = true;

        ImGui.GetIO().Fonts.AddFontFromMemoryTTF(
            fontData,
            size,
            fontConfig,
            glyphRanges);

        fontConfig.Destroy();
    }

    /// <summary>
    /// Gets the main application container window where we do all our rendering and input processing.
    /// </summary>
    public ISdl2Window Window { get; init; }

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
    public static unsafe ImGuiScene CreateWindow(AssertHandler assertHandler)
    {
        var savedState = LoadWindowState();

        var createInfo = new WindowCreateInfo
        {
            WindowTitle = "DalaMock",
            WindowWidth = savedState?.Width ?? 1024,
            WindowHeight = savedState?.Height ?? 768,
            X = savedState?.X ?? 0,
            Y = savedState?.Y ?? 0,
            WindowInitialState = WindowState.Maximized,
        };
        if (savedState != null)
        {
            createInfo.WindowInitialState = savedState.IsMaximized
                                                ? Veldrid.WindowState.Maximized
                                                : Veldrid.WindowState.Normal;
            int numDisplays = Sdl2Native.SDL_GetNumVideoDisplays();
            if (savedState.MonitorIndex >= numDisplays)
            {
                savedState.MonitorIndex = 0;
            }

            Rectangle displayBounds;
            Sdl2Native.SDL_GetDisplayBounds(savedState.MonitorIndex, &displayBounds);

            int clampedX = Math.Max(displayBounds.X, Math.Min(savedState.X, (displayBounds.X + displayBounds.Width) - 100));
            int clampedY = Math.Max(displayBounds.Y, Math.Min(savedState.Y, (displayBounds.Y + displayBounds.Height) - 100));

            createInfo.X = clampedX;
            createInfo.Y = clampedY;
        }

        var scene = new ImGuiScene(createInfo, assertHandler);
        scene.Window.Opacity = 1;

        return scene;
    }

    /// <summary>
    /// Simple method to run the scene in a loop until the window is closed or the application
    /// requests an exit (via <see cref="ShouldQuit"/>).
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

        this.TrackWindowState();

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

    private unsafe MockWindowState CaptureWindowState()
    {
        var handle = this.Window.SdlWindowHandle;

        int x, y, width, height;
        Sdl2Native.SDL_GetWindowPosition(handle, &x, &y);
        Sdl2Native.SDL_GetWindowSize(handle, &width, &height);

        bool isMaximized = this.Window.WindowState == WindowState.Maximized;

        int monitorIndex = this.GetMonitorIndexForWindow(handle);

        return new MockWindowState
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            IsMaximized = isMaximized,
            MonitorIndex = monitorIndex,
        };
    }

    private void TrackWindowState()
    {
        var newState = this.CaptureWindowState();

        if (!this.WindowStatesEqual(newState, this.currentWindowState))
        {
            this.currentWindowState = newState;
            this.DebounceSaveWindowState();
        }
    }

    private bool WindowStatesEqual(MockWindowState a, MockWindowState b)
    {
        return a.X == b.X
            && a.Y == b.Y
            && a.Width == b.Width
            && a.Height == b.Height
            && a.IsMaximized == b.IsMaximized
            && a.MonitorIndex == b.MonitorIndex;
    }

    private void DebounceSaveWindowState()
    {
        this.debounceCts?.Cancel();
        this.debounceCts = new CancellationTokenSource();

        var token = this.debounceCts.Token;

        Task.Run(
            async () =>
        {
            try
            {
                await Task.Delay(DebounceDelayMs, token);

                if (!token.IsCancellationRequested)
                {
                    SaveWindowState(this.currentWindowState);
                }
            }
            catch (TaskCanceledException)
            {
            }
        },
            token);
    }

    private static void SaveWindowState(MockWindowState state)
    {
        try
        {
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(StateFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save window state: {ex.Message}");
        }
    }

    private static MockWindowState? LoadWindowState()
    {
        try
        {
            if (File.Exists(StateFilePath))
            {
                var json = File.ReadAllText(StateFilePath);
                return JsonConvert.DeserializeObject<MockWindowState>(json);
            }
        }
        catch (Exception ex)
        {
            // Log error or handle gracefully
            Console.WriteLine($"Failed to load window state: {ex.Message}");
        }

        return null;
    }

    private unsafe int GetMonitorIndexForWindow(IntPtr windowHandle)
    {
        int displayIndex = Sdl2Native.SDL_GetWindowDisplayIndex(windowHandle);
        return displayIndex >= 0 ? displayIndex : 0;
    }


    private static unsafe ushort* GetFontAwesomeRanges()
    {
        var ranges = (ushort*)ImGui.MemAlloc(sizeof(ushort) * 3);
        ranges[0] = 0xF000;
        ranges[1] = 0xF8FF;
        ranges[2] = 0;
        return ranges;
    }

    private void WindowOnClosed()
    {
        this.debounceCts?.Cancel();
        SaveWindowState(this.currentWindowState);
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
