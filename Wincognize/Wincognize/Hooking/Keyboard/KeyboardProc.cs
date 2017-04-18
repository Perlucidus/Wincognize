using System;
using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Keyboard
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardProc
    {
        public int VirtualKeyCode;
        public int HardwareScanCode;
        public KeyState Flags;
        public int Timestamp;
        public IntPtr ExtraInfo;
    }
}
