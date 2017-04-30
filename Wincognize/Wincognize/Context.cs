using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Context()
        {
            MainForm = new Form();
            AllocConsole();
            Application.ApplicationExit += OnApplicationExit;
            Application.ThreadException += OnThreadException;
            Initialize();
        }

        private void Initialize()
        {
            (m_trackers = new List<Tracker>
            {
#if !DEBUG
                new MouseTracker(),
#endif
                new KeyboardTracker(),
                new BrowsingHistoryTracker()
            }).ForEach(t => t.Enable());
            (m_processors = new List<Processor>
            {
                new MouseProcessor(),
                new KeyboardProcessor(),
                new BrowsingHistoryProcessor()
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
