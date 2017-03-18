using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Wincognize.Data;
using Wincognize.Tracking;

namespace Wincognize
{
    public class Context : ApplicationContext
    {
        private List<Tracker> m_trackers;

        public Context()
        {
            Application.ApplicationExit += OnApplicationExit;
            Application.ThreadException += OnThreadException;
            InitializeTrackers();
        }

        private void InitializeTrackers()
        {
            m_trackers = new List<Tracker>();
            //m_trackers.Add(new MouseTracker());
            //m_trackers.Add(new KeyboardTracker());
            m_trackers.Add(new BrowsingHistoryTracker());
            m_trackers.ForEach(t => t.Enable());
        }

        private void OnThreadException(Object sender, ThreadExceptionEventArgs e)
        {
            Console.WriteLine($"Thread exception: {e}");
        }

        private void OnApplicationExit(Object sender, EventArgs e)
        {
            Console.WriteLine("Application Exit");
            m_trackers.ForEach(t => t.Disable());
            m_trackers.Clear();
        }
    }
}
