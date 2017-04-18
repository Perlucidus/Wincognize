using System;

namespace Wincognize.Hooking.Mouse
{
    [Flags]
    public enum MouseState : Int32
    {
        Injected = 0x1,
        LowerInjected = 0x2
    }
}
