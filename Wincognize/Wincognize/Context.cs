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
        public List<Tracker> Trackers;
        public List<Processor> Processors;

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
            (Trackers = new List<Tracker>
            {
                new StatusTracker(),
                new MouseTracker(),
                new KeyboardTracker(),
                new BrowsingHistoryTracker()
            }).ForEach(t => t.Enable());
            (Processors = new List<Processor>
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
            Trackers.ForEach(t => t.Disable());
            Trackers.Clear();
        }
    }
}
