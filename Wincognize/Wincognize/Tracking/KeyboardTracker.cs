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
        }

        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged()
        {
            m_hook.Unhook();
        }

        protected override void PDisable() { }

        protected override void PEnable() { }

        private void KeyboardProcCallback(KeyboardAction wParam, KeyboardProc lParam)
        {
            Database.Instance.ExecuteNonQuery($@"
                INSERT INTO Keyboard (action, vkCode, hwsCode, flags, timestamp, extrainfo)
                VALUES ({wParam}, {lParam.VirtualKeyCode}, {lParam.HardwareScanCode}, {lParam.Flags}, {lParam.Timestamp}, {lParam.ExtraInfo})
            ");
        }
    }
}
