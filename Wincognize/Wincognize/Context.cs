using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Wincognize.Processing;
using Wincognize.Tracking;

namespace Wincognize
{
    public class Context : ApplicationContext
    {
        private List<Tracker> m_trackers;
        private List<Processor> m_processors;

        public Context()
        {
            Application.ApplicationExit += OnApplicationExit;
            Application.ThreadException += OnThreadException;
            Initialize();
        }

        private void Initialize()
        {
            (m_trackers = new List<Tracker>
            {
                //new MouseTracker(),
                new KeyboardTracker(),
                //new BrowsingHistoryTracker()
            }).ForEach(t => t.Enable());
            (m_processors = new List<Processor>
            {
                new KeyboardProcessor()
            }).ForEach(p => p.Enable());
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
