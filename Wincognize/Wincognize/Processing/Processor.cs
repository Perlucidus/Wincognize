namespace Wincognize.Processing
{
    public abstract class Processor
    {
        public int Match { get; protected set; }
        private bool m_enabled;

        public Processor()
        {
            Match = 0;
            m_enabled = false;
        }

        public void Enable()
        {
            if (!m_enabled)
            {
                PEnable();
                m_enabled = true;
            }
        }

        public void Disable()
        {
            if (m_enabled)
            {
                PDisable();
                m_enabled = false;
            }
        }

        protected abstract void PEnable();
        protected abstract void PDisable();
    }
}
