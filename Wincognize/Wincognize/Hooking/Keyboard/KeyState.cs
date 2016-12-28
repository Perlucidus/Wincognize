using System;

namespace Wincognize.Hooking.Keyboard
{
    [Flags]
    public enum KeyState : UInt32
    {
        ExtendedKey = 0x1,
        LowerILInjected = 0x2,
        Injected = 0x10,
        Alt = 0x20,
        Transition = 0x80
    }
}
