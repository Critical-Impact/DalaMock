namespace DalaMock.Core.Mocks.MockServices;

public class MockUiBuilder : IUiBuilder, IMockService
{
    private readonly IFont font;

    public float FontDefaultSizePt { get; }

    public float FontDefaultSizePx { get; }

    public ImFontPtr FontDefault => this.font.DefaultFont;

    public ImFontPtr FontIcon => this.font.IconFont;

    public ImFontPtr FontMono => this.font.MonoFont;

    public ImFontPtr FontIconFixedWidth => this.font.IconFixedWidth;

    public GraphicsDevice GraphicsDevice { get; }

    public MockUiBuilder(ImGuiScene scene, IFont font)
    {
        this.font = font;
        this.GraphicsDevice = scene.GraphicsDevice;
        if (this.GraphicsDevice.GetD3D11Info(out BackendInfoD3D11 dx11Info))
        {
            this.DeviceHandle = dx11Info.Device;
        }

        this.WindowHandlePtr = scene.Window.SdlWindowHandle;
    }

    public DalamudUldWrapper LoadUld(string uldPath)
    {
        throw new NotImplementedException();
    }

    public Task WaitForUi()
    {
        throw new NotImplementedException();
    }

    public Task<T> RunWhenUiPrepared<T>(Func<T> func, bool runInFrameworkThread = false)
    {
        throw new NotImplementedException();
    }

    public Task<T> RunWhenUiPrepared<T>(Func<Task<T>> func, bool runInFrameworkThread = false)
    {
        throw new NotImplementedException();
    }

    public DalamudIFontAtlas CreateFontAtlas(
        FontAtlasAutoRebuildMode autoRebuildMode,
        bool isGlobalScaled = true,
        string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public DalamudIFontHandle DefaultFontHandle { get; }

    public DalamudIFontHandle IconFontHandle { get; }

    public DalamudIFontHandle MonoFontHandle { get; }

    public DalamudIFontHandle IconFontFixedWidthHandle { get; }

    public IFontSpec DefaultFontSpec { get; }

    public Device Device { get; }

    public IntPtr DeviceHandle { get; set; }

    public IntPtr WindowHandlePtr { get; }

    public bool DisableAutomaticUiHide { get; set; }

    public bool DisableUserUiHide { get; set; }

    public bool DisableCutsceneUiHide { get; set; }

    public bool DisableGposeUiHide { get; set; }

    public bool OverrideGameCursor { get; set; }

    public ulong FrameCount { get; }

    public bool CutsceneActive { get; }

    public bool ShouldModifyUi { get; }

    public bool UiPrepared { get; }

    public DalamudIFontAtlas FontAtlas { get; }

    public bool ShouldUseReducedMotion { get; }

    public bool PluginUISoundEffectsEnabled { get; set; }

    public event SysAction? Draw;

    public event SysAction? ResizeBuffers;

    public event SysAction? OpenConfigUi;

    public event SysAction? OpenMainUi;

    public event SysAction? ShowUi;

    public event SysAction? HideUi;

    public event SysAction? DefaultGlobalScaleChanged;

    public event SysAction? DefaultFontChanged;

    public event SysAction? DefaultStyleChanged;

    public void FireOpenMainUiEvent()
    {
        this.OpenMainUi?.Invoke();
    }

    public void FireOpenConfigUiEvent()
    {
        this.OpenConfigUi?.Invoke();
    }

    public void FireDraw()
    {
        this.Draw?.Invoke();
    }

    public string ServiceName { get; set; } = "Ui Builder";
}
