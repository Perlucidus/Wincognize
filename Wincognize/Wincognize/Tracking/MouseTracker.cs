using System.Text;
using Wincognize.Data;
using Wincognize.Hooking.Mouse;

namespace Wincognize.Tracking
{
    public class MouseTracker : Tracker
    {
        private MouseHook m_hook;

        public MouseTracker()
        {
            m_hook = new MouseHook(MouseProcCallback);
        }

        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged()
        {
            m_hook.Unhook();
        }

        protected override void PDisable() { }

        protected override void PEnable() { }

        private void MouseProcCallback(MouseAction wParam, MouseProc lParam)
        {
            if (!m_enabled)
                return;
            if (wParam != MouseAction.WM_MOUSEMOVE)
            {
                Database.Instance.ExecuteNonQuery($@"
                INSERT INTO Mouse (action, location, data, flags, timestamp, extrainfo)
                VALUES ({wParam}, {lParam.Location}, {lParam.Data}, {lParam.Flags}, {lParam.Timestamp}, {lParam.ExtraInfo})
                ");
            }
        }
    }
}
