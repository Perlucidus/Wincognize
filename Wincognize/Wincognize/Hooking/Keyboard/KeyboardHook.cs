using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Wincognize.Hooking.HookAPI;

namespace Wincognize.Hooking.Keyboard
{
    public class KeyboardHook
    {
        public delegate void KeyboardProcCallback(KeyboardAction wParam, KeyboardProc lParam);

        private IntPtr m_hhook;
        private HookProc m_hookProc;
        private KeyboardProcCallback m_callback;

        public KeyboardHook(KeyboardProcCallback callback)
        {
            m_hhook = IntPtr.Zero;
            m_hookProc = LLKeyboardProc;
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
                return SetWindowsHookEx((int)WindowsHook.WH_KEYBOARD_LL, lpfn, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr LLKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                try
                {
                    m_callback((KeyboardAction)wParam.ToInt32(), Marshal.PtrToStructure<KeyboardProc>(lParam));
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
