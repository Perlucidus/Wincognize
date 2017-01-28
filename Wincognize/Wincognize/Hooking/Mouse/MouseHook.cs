using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Wincognize.Hooking.HookAPI;

namespace Wincognize.Hooking.Mouse
{
    public class MouseHook
    {
        public delegate void MouseProcCallback(MouseAction wParam, MouseProc lParam);

        private IntPtr m_hhook;
        private HookProc m_hookProc;
        private MouseProcCallback m_callback;

        public MouseHook(MouseProcCallback callback)
        {
            m_hhook = IntPtr.Zero;
            m_hookProc = LLMouseProc;
            m_callback = callback;
        }

        public void Hook()
        {
            if (m_hhook == IntPtr.Zero)
                m_hhook = SetHook(m_hookProc);
        }

        public void Unhook()
        {
            if (m_hhook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(m_hhook);
                m_hhook = IntPtr.Zero;
            }
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
                try
                {
                    m_callback((MouseAction)wParam.ToInt32(), Marshal.PtrToStructure<MouseProc>(lParam));
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
