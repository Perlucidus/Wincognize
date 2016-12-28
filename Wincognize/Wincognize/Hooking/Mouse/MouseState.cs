using System;

namespace Wincognize.Hooking.Mouse
{
    [Flags]
    public enum MouseState
    {
        Injected = 0x1,
        LowerInjected = 0x2
    }
}
