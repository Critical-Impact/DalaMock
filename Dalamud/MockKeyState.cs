using DalaMock.Extensions;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Plugin.Services;
using Veldrid;
using Veldrid.Sdl2;

namespace DalaMock.Dalamud;

public class MockKeyState : IKeyState, IDisposable
{
    private readonly Sdl2Window _window;

    public MockKeyState(Sdl2Window window)
    {
        _window = window;
        _window.KeyDown += WindowOnKeyDown;
        _window.KeyUp += WindowOnKeyUp;
    }

    private void WindowOnKeyUp(KeyEvent keyEvent)
    {
        var keyState = keyEvent.ToKeyState();
        this[keyState] = false;
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Shift))
        {
            this[VirtualKey.SHIFT] = false;
        }
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Control))
        {
            this[VirtualKey.CONTROL] = false;
        }
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Alt))
        {
            this[VirtualKey.MENU] = false;
        }        
    }

    private void WindowOnKeyDown(KeyEvent keyEvent)
    {
        var keyState = keyEvent.ToKeyState();
        this[keyState] = true;
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Shift))
        {
            this[VirtualKey.SHIFT] = true;
        }
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Control))
        {
            this[VirtualKey.CONTROL] = true;
        }
        if (keyEvent.Modifiers.HasFlag(ModifierKeys.Alt))
        {
            this[VirtualKey.MENU] = true;
        }
    }

    private HashSet<VirtualKey> _activeKeys { get; } = new HashSet<VirtualKey>();
    public bool this[int vkCode]
    {
        get => _activeKeys.Contains((VirtualKey)vkCode);
        set
        {
            if (value)
            {
                _activeKeys.Add((VirtualKey)vkCode);
            }
            else
            {
                _activeKeys.Remove((VirtualKey)vkCode);
            }
        }
    }

    public bool this[VirtualKey vkCode]
    {
        get => this[(int)vkCode];
        set => this[(int)vkCode] = value;
    }

    public int GetRawValue(int vkCode)
    {
        return 0;
    }

    public int GetRawValue(VirtualKey vkCode)
    {
        return 0;
    }

    public void SetRawValue(int vkCode, int value)
    {
    }

    public void SetRawValue(VirtualKey vkCode, int value)
    {
    }

    public bool IsVirtualKeyValid(int vkCode)
    {
        return true;
    }

    public bool IsVirtualKeyValid(VirtualKey vkCode)
    {
        return true;
    }

    public IEnumerable<VirtualKey> GetValidVirtualKeys() => (VirtualKey[])Enum.GetValuesAsUnderlyingType<VirtualKey>();


    public void ClearAll()
    {
        _activeKeys.Clear();
    }

    public void Dispose()
    {
        _window.KeyDown -= WindowOnKeyDown;
    }
}