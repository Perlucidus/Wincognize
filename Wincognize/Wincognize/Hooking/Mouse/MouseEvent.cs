using System;
using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Mouse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseEvent
    {
        public POINT Location;
        public uint Data;
        public MouseState Flags;
        public uint Timestamp;
        public IntPtr ExtraInfo;
    }
}
