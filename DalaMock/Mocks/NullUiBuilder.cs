namespace DalaMock.Core.Mocks;

using System;
using System.Threading.Tasks;

using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ManagedFontAtlas;

using SharpDX.Direct3D11;

public class NullUiBuilder : IUiBuilder, IMockService
{
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

    public IFontHandle DefaultFontHandle { get; set; }

    public IFontHandle IconFontHandle { get; set; }

    public IFontHandle MonoFontHandle { get; set; }

    public IFontHandle IconFontFixedWidthHandle { get; set; }

    public IFontSpec DefaultFontSpec { get; set; }

    public float FontDefaultSizePt { get; set; }

    public float FontDefaultSizePx { get; set; }

    public ImFontPtr FontDefault { get; set; }

    public ImFontPtr FontIcon { get; set; }

    public ImFontPtr FontMono { get; set; }

    public Device Device { get; set; }

    public IntPtr DeviceHandle { get; set; }

    public IntPtr WindowHandlePtr { get; set; }

    public bool DisableAutomaticUiHide { get; set; }

    public bool DisableUserUiHide { get; set; }

    public bool DisableCutsceneUiHide { get; set; }

    public bool DisableGposeUiHide { get; set; }

    public bool OverrideGameCursor { get; set; }

    public ulong FrameCount { get; set; }

    public bool CutsceneActive { get; set; }

    public bool ShouldModifyUi { get; set; }

    public bool UiPrepared { get; set; }

    public IFontAtlas FontAtlas { get; set; }

    public bool ShouldUseReducedMotion { get; set; }

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

    public string ServiceName { get; set; } = "Ui Builder";
}
