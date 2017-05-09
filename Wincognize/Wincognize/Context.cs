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
#if DEBUG
            AllocConsole(); //Create console for debugging purposes
#endif
            Application.ApplicationExit += OnApplicationExit;
            Application.ThreadException += OnThreadException;
            Initialize();
        }

        private void Initialize()
        {
            //Create instances of each tracker and enable all of them
            (Trackers = new List<Tracker>
            {
                new StatusTracker(),
                new MouseTracker(),
                new KeyboardTracker(),
                new BrowsingHistoryTracker()
            }).ForEach(t => t.Enable());
            //Create instance of each processor and enable all of them
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
