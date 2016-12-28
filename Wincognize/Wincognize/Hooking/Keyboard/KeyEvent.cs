using System;
using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Keyboard
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyEvent
    {
        public uint VirtualKeyCode;
        public uint HardwareScanCode;
        public KeyState Flags;
        public uint Timestamp;
        public IntPtr ExtraInfo;
    }
}
