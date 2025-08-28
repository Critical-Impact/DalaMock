namespace DalaMock.Core.Imgui;

using System;
using System.Numerics;

using Veldrid;
using Veldrid.Sdl2;

public class NullSdl2Window : ISdl2Window
{
    public bool LimitPollRate { get; set; }

    public float PollIntervalInMs { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public IntPtr Handle { get; set; }

    public string Title { get; set; }

    public WindowState WindowState { get; set; }

    public bool Exists { get; set; }

    public bool Visible { get; set; }

    public Vector2 ScaleFactor { get; set; }

    public Rectangle Bounds { get; set; }

    public bool CursorVisible { get; set; }

    public unsafe float Opacity { get; set; }

    public bool Focused { get; set; }

    public bool Resizable { get; set; }

    public bool BorderVisible { get; set; }

    public IntPtr SdlWindowHandle { get; set; }

    public Vector2 MouseDelta { get; set; }

    public event Action? Resized;

    public event Action? Closing;

    public event Action? Closed;

    public event Action? FocusLost;

    public event Action? FocusGained;

    public event Action? Shown;

    public event Action? Hidden;

    public event Action? MouseEntered;

    public event Action? MouseLeft;

    public event Action? Exposed;

    public event Action<Point>? Moved;

    public event Action<MouseWheelEventArgs>? MouseWheel;

    public event Action<MouseMoveEventArgs>? MouseMove;

    public event Action<MouseEvent>? MouseDown;

    public event Action<MouseEvent>? MouseUp;

    public event Action<KeyEvent>? KeyDown;

    public event Action<KeyEvent>? KeyUp;

    public event Action<DragDropEvent>? DragDrop;

    public Point ClientToScreen(Point p)
    {
        throw new NotImplementedException();
    }

    public void SetMousePosition(Vector2 position)
    {
        throw new NotImplementedException();
    }

    public void SetMousePosition(int x, int y)
    {
        throw new NotImplementedException();
    }

    public void SetCloseRequestedHandler(Func<bool> handler)
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public InputSnapshot PumpEvents()
    {
        throw new NotImplementedException();
    }

    public void PumpEvents(SDLEventHandler eventHandler)
    {
        throw new NotImplementedException();
    }

    public Point ScreenToClient(Point p)
    {
        throw new NotImplementedException();
    }
}
