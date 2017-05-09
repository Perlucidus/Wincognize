using System;
using Wincognize.Data;
using Wincognize.Hooking.Mouse;

namespace Wincognize.Tracking
{
    public class MouseTracker : Tracker
    {
        private MouseHook m_hook;
        private DateTime m_lastMove; //Last mouse movement

        public MouseTracker()
        {
            m_hook = new MouseHook(MouseProcCallback);
            m_lastMove = DateTime.Now;
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

        private void MouseProcCallback(MouseAction wParam, MouseProc lParam)
        {
            if (!m_enabled)
                return;
            if (wParam == MouseAction.WM_MOUSEMOVE && (DateTime.Now - m_lastMove).TotalMilliseconds < 10)
                return;
            lock (DataContext.Main)
            {
                //Insert mouse action into database
                DataContext.Main.Mouse.Add(
                    new Mouse
                    {
                        Action = (int)wParam,
                        X = lParam.Location.x,
                        Y = lParam.Location.y,
                        Data = lParam.Data,
                        Flags = (int)lParam.Flags,
                        Timestamp = lParam.Timestamp,
                        ExtraInfo = lParam.ExtraInfo.ToInt32()
                    });
                DataContext.Main.SaveChanges();
            }
        }
    }
}
