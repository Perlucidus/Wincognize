using Wincognize.Data;
using Wincognize.Hooking.Keyboard;

namespace Wincognize.Tracking
{
    public class KeyboardTracker : Tracker
    {
        private KeyboardHook m_hook;

        public KeyboardTracker()
        {
            m_hook = new KeyboardHook(KeyboardProcCallback);
            m_hook.Hook();
        }

        #region Tracker Implementation

        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged()
        {
            m_hook.Unhook();
        }

        protected override void PDisable() { }

        protected override void PEnable() { }

        #endregion

        private void KeyboardProcCallback(KeyboardAction wParam, KeyboardProc lParam)
        {
            if (!m_enabled)
                return;
            lock (DataContext.Main)
            {
                DataContext.Main.Keyboard.Add(
                    new Keyboard
                    {
                        Action = (int)wParam,
                        VkCode = lParam.VirtualKeyCode,
                        HwsCode = lParam.HardwareScanCode,
                        Flags = (int)lParam.Flags,
                        Timestamp = lParam.Timestamp,
                        ExtraInfo = lParam.ExtraInfo.ToInt32()
                    });
                DataContext.Main.SaveChanges();
            }
        }
    }
}
