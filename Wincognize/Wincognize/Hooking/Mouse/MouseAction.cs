using System;

namespace Wincognize.Hooking.Mouse
{
    public enum MouseAction : Int32
    {
        WM_MOUSEFIRST = 0x200,
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_LBUTTONDBLCLK = 0x203,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_RBUTTONDBLCLK = 0x206,
        WM_MBUTTONDOWN = 0x207,
        WM_MBUTTONUP = 0x208,
        WM_MBUTTONDBLCLK = 0x209,
        WM_MOUSEWHEEL = 0x20A,
        WM_XBUTTONDOWN = 0x20B,
        WM_XBUTTONUP = 0x20C,
        WM_XBUTTONDBLCLK = 0x20D,
        WM_MOUSEHWHEEL = 0x20E
    }
}
