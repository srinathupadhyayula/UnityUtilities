using System;

namespace Utilities.Async
{
    public sealed class WaitUntilPooled : PooledYieldInstruction
    {
        private Func<bool> m_predicate;
        private bool       m_waiting;

        public override bool keepWaiting
        {
            get
            {
                m_waiting = !m_predicate();
                if (!m_waiting)
                    YieldPool.BackToPool(this);
                return m_waiting;
            }
        }

        public WaitUntilPooled Wait(Func<bool> predicate)
        {
            m_predicate = predicate;
            m_waiting = false;
            return this;
        }
    }
}
