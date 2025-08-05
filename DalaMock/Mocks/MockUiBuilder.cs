using DalaMock.Core.Imgui;
using IFontAtlas = Dalamud.Interface.ManagedFontAtlas.IFontAtlas;
using IFontHandle = Dalamud.Interface.ManagedFontAtlas.IFontHandle;
using UldWrapper = Dalamud.Interface.UldWrapper;

namespace DalaMock.Core.Mocks;

using System;
using System.Threading.Tasks;
using Dalamud.Interface;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ManagedFontAtlas;
using Dalamud.Bindings.ImGui;
using SharpDX.Direct3D11;
using Veldrid;

public class MockUiBuilder : IUiBuilder
{
    public float FontDefaultSizePt { get; }

    public float FontDefaultSizePx { get; }

    public ImFontPtr FontDefault { get; }

    public ImFontPtr FontIcon { get; }

    public ImFontPtr FontMono { get; }

    public GraphicsDevice GraphicsDevice { get; }

    public MockUiBuilder(ImGuiScene scene)
    {
        this.GraphicsDevice = scene.GraphicsDevice;
        if (this.GraphicsDevice.GetD3D11Info(out BackendInfoD3D11 dx11Info))
        {
            this.Device = new Device(dx11Info.Device);
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

    public event Action? Draw;

    public event Action? ResizeBuffers;

    public event Action? OpenConfigUi;

    public event Action? OpenMainUi;

    public event Action? ShowUi;

    public event Action? HideUi;

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
}
