using System;
using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Keyboard
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardProc
    {
        public uint VirtualKeyCode;
        public uint HardwareScanCode;
        public KeyState Flags;
        public uint Timestamp;
        public IntPtr ExtraInfo;
    }
}
