using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Wincognize.Tracking
{
    public class StatusTracker : Tracker
    {
        private const int DelayInterval = 10000; //Task execute interval
        private Task m_task;
        private CancellationTokenSource m_cts;

        public StatusTracker()
        {
            m_cts = new CancellationTokenSource();
            m_task = new Task(Update, m_cts.Token);
        }

        #region Tracker Implementation

        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged() { }

        protected override void PDisable()
        {
            m_cts.Cancel();
        }

        protected override void PEnable()
        {
            m_task.Start();
        }

        #endregion

        private async void Update(object obj)
        {
            CancellationToken cts = (CancellationToken)obj;
            while (!cts.IsCancellationRequested)
            {
                //Calculate average of processor approximations
                double avgApprox = Program.MainContext.Processors.Select(p => p.Approximation).Average();
                Console.WriteLine($"Total: {avgApprox}");
                if (avgApprox > 0.6)
                    Console.WriteLine("RIP");
                await Task.Delay(DelayInterval);
            }
        }
    }
}
