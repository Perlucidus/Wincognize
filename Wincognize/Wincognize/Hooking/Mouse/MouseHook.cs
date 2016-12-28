using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Wincognize.Hooking.HookAPI;

namespace Wincognize.Hooking.Mouse
{
    public class MouseHook
    {
        public class MouseEventArgs : EventArgs
        {
            public MouseAction Action;
            public MouseEvent Data;
        }
        public event EventHandler<MouseEventArgs> MouseAction;

        private IntPtr m_hhook;
        private HookProc m_hookProc;

        public MouseHook()
        {
            m_hhook = IntPtr.Zero;
            m_hookProc = LLMouseProc;
        }

        public void Start()
        {
            if (m_hhook != IntPtr.Zero)
                return;
            m_hhook = SetHook(m_hookProc);
        }

        public void Stop()
        {
            if (m_hhook == IntPtr.Zero)
                return;
            UnhookWindowsHookEx(m_hhook);
            m_hhook = IntPtr.Zero;
        }

        private IntPtr SetHook(HookProc lpfn)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
                return SetWindowsHookEx((int)WindowsHook.WH_MOUSE_LL, lpfn, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr LLMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseEventArgs eventArgs = new MouseEventArgs
                {
                    Action = (MouseAction)wParam.ToInt32(),
                    Data = Marshal.PtrToStructure<MouseEvent>(lParam)
                };
                try
                {
                    MouseAction?.Invoke(this, eventArgs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return CallNextHookEx(m_hhook, nCode, wParam, lParam);
        }
    }
}
