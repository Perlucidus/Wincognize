using System;

namespace Wincognize.Tracking
{
    public abstract class Tracker : IDisposable
    {
        protected bool m_enabled = false;

        #region IDisposable Support

        private bool disposedValue = false;

        protected abstract void DisposeManaged();
        protected abstract void DisposeUnmanaged();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    DisposeManaged();
                DisposeUnmanaged();
                disposedValue = true;
            }
        }

        ~Tracker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void Enable()
        {
            if (!m_enabled)
            {
                m_enabled = true;
                PEnable();
            }
        }

        public void Disable()
        {
            if (m_enabled)
            {
                m_enabled = false;
                PDisable();
            }
        }

        protected abstract void PEnable();
        protected abstract void PDisable();
    }
}
