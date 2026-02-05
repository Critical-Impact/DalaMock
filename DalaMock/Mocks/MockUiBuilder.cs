using DalaMock.Shared.Interfaces;

using FFXIVClientStructs.FFXIV.Client.Graphics.Kernel;

namespace DalaMock.Core.Mocks;

using System;
using System.Threading.Tasks;

using DalaMock.Core.Imgui;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ManagedFontAtlas;
using Veldrid;

using IFontAtlas = Dalamud.Interface.ManagedFontAtlas.IFontAtlas;
using IFontHandle = Dalamud.Interface.ManagedFontAtlas.IFontHandle;
using UldWrapper = Dalamud.Interface.UldWrapper;

public class MockUiBuilder : IUiBuilder, IMockService
{
    private readonly IFont font;

    public float FontDefaultSizePt { get; }

    public float FontDefaultSizePx { get; }

    public ImFontPtr FontDefault => this.font.DefaultFont;

    public ImFontPtr FontIcon => this.font.IconFont;

    public ImFontPtr FontMono => this.font.MonoFont;

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

    public UldWrapper LoadUld(string uldPath)
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

    public IFontAtlas CreateFontAtlas(
        FontAtlasAutoRebuildMode autoRebuildMode,
        bool isGlobalScaled = true,
        string? debugName = null)
    {
        throw new NotImplementedException();
    }

    public IFontHandle DefaultFontHandle { get; }

    public IFontHandle IconFontHandle { get; }

    public IFontHandle MonoFontHandle { get; }

    public IFontHandle IconFontFixedWidthHandle { get; }

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

    public IFontAtlas FontAtlas { get; }

    public bool ShouldUseReducedMotion { get; }

    public bool PluginUISoundEffectsEnabled { get; set; }

    public event Action? Draw;

    public event Action? ResizeBuffers;

    public event Action? OpenConfigUi;

    public event Action? OpenMainUi;

    public event Action? ShowUi;

    public event Action? HideUi;

    public event Action? DefaultGlobalScaleChanged;

    public event Action? DefaultFontChanged;

    public event Action? DefaultStyleChanged;

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
