using System.Runtime.InteropServices;

namespace Wincognize.Hooking.Mouse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
}
