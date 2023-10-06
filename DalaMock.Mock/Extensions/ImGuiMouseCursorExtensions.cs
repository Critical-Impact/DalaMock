using ImGuiNET;
using Veldrid.Sdl2;

namespace DalaMock.Extensions;

public static class ImGuiMouseCursorExtensions
{
    public static SDL_SystemCursor ToSdlCursor(this ImGuiMouseCursor cursor)
    {
        switch (cursor)
        {
            case ImGuiMouseCursor.Arrow:
                return SDL_SystemCursor.Arrow;
            case ImGuiMouseCursor.Hand:
                return SDL_SystemCursor.Hand;
            case ImGuiMouseCursor.NotAllowed:
                return SDL_SystemCursor.No;
            case ImGuiMouseCursor.ResizeAll:
                return SDL_SystemCursor.SizeAll;
            case ImGuiMouseCursor.TextInput:
                return SDL_SystemCursor.IBeam;
            case ImGuiMouseCursor.ResizeEW:
                return SDL_SystemCursor.SizeWE;
            case ImGuiMouseCursor.ResizeNS:
                return SDL_SystemCursor.SizeNS;
            case ImGuiMouseCursor.ResizeNESW:
                return SDL_SystemCursor.SizeNESW;
            case ImGuiMouseCursor.ResizeNWSE:
                return SDL_SystemCursor.SizeNWSE;
        }

        return SDL_SystemCursor.WaitArrow;
    }
}