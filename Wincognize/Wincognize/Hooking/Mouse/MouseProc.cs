using System;
using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Mouse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseProc
    {
        public POINT Location;
        public int Data;
        public MouseState Flags;
        public int Timestamp;
        public IntPtr ExtraInfo;
    }
}
