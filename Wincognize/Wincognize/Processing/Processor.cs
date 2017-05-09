using System.Threading;
using System.Threading.Tasks;

namespace Wincognize.Processing
{
    public abstract class Processor
    {
        public double Approximation { get; protected set; } //Approximation error of data comparison
        private Task m_task;
        private CancellationTokenSource m_cts;
        private int m_interval; //Task execute interval
        private bool m_enabled;

        public Processor(int interval)
        {
            Approximation = 0;
            m_cts = new CancellationTokenSource();
            m_task = new Task(Trigger, m_cts.Token);
            m_interval = interval;
            m_enabled = false;
        }

        public void Enable()
        {
            if (!m_enabled)
            {
                m_task.Start();
                m_enabled = true;
            }
        }

        public void Disable()
        {
            if (m_enabled)
            {
                m_cts.Cancel();
                m_enabled = false;
            }
        }

        private async void Trigger(object obj)
        {
            CancellationToken cts = (CancellationToken)obj;
            while (!cts.IsCancellationRequested)
            {
                Process();
                await Task.Delay(m_interval);
            }
        }

        protected abstract void Process();
    }
}
