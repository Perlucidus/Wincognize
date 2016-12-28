using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Wincognize.Hooking.HookAPI;

namespace Wincognize.Hooking.Keyboard
{
    public class KeyboardHook
    {
        public class KeyEventArgs : EventArgs
        {
            public KeyboardAction Action;
            public KeyEvent Data;
        }

        public event EventHandler<KeyEventArgs> KeyboardAction;

        private IntPtr m_hhook;
        private HookProc m_hookProc;

        public KeyboardHook()
        {
            m_hhook = IntPtr.Zero;
            m_hookProc = LLKeyboardProc;
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
                return SetWindowsHookEx((int)WindowsHook.WH_KEYBOARD_LL, lpfn, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr LLKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyEventArgs eventArgs = new KeyEventArgs
                {
                    Action = (KeyboardAction)wParam.ToInt32(),
                    Data = Marshal.PtrToStructure<KeyEvent>(lParam)
                };
                try
                {
                    KeyboardAction?.Invoke(this, eventArgs);
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
