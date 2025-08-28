namespace DalaMock.Core.Imgui;

using System;
using System.Numerics;

using Veldrid;
using Veldrid.Sdl2;

public interface ISdl2Window
{
    bool LimitPollRate { get; set; }

    float PollIntervalInMs { get; set; }

    int X { get; set; }

    int Y { get; set; }

    int Width { get; set; }

    int Height { get; set; }

    IntPtr Handle { get; }

    string Title { get; set; }

    WindowState WindowState { get; set; }

    bool Exists { get; }

    bool Visible { get; set; }

    Vector2 ScaleFactor { get; }

    Rectangle Bounds { get; }

    bool CursorVisible { get; set; }

    unsafe float Opacity { get; set; }

    bool Focused { get; }

    bool Resizable { get; set; }

    bool BorderVisible { get; set; }

    IntPtr SdlWindowHandle { get; }

    Vector2 MouseDelta { get; }

    event Action Resized;

    event Action Closing;

    event Action Closed;

    event Action FocusLost;

    event Action FocusGained;

    event Action Shown;

    event Action Hidden;

    event Action MouseEntered;

    event Action MouseLeft;

    event Action Exposed;

    event Action<Point> Moved;

    event Action<MouseWheelEventArgs> MouseWheel;

    event Action<MouseMoveEventArgs> MouseMove;

    event Action<MouseEvent> MouseDown;

    event Action<MouseEvent> MouseUp;

    event Action<KeyEvent> KeyDown;

    event Action<KeyEvent> KeyUp;

    event Action<DragDropEvent> DragDrop;

    Point ClientToScreen(Point p);

    void SetMousePosition(Vector2 position);

    void SetMousePosition(int x, int y);

    void SetCloseRequestedHandler(Func<bool> handler);

    void Close();

    InputSnapshot PumpEvents();

    void PumpEvents(SDLEventHandler eventHandler);

    Point ScreenToClient(Point p);
}
