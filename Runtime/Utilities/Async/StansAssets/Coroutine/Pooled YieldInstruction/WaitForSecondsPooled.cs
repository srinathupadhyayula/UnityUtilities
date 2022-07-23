#if UNITY_2020_1_OR_NEWER
using UnityEngine;

namespace Utilities.Async
{
    public class WaitForSecondsPooled : PooledYieldInstruction
    {
        private bool  m_waiting;
        private float m_waitTime;

        public override bool keepWaiting
        {
            get
            {
                m_waiting = m_waitTime > 0f;
                if (m_waiting)
                    m_waitTime -= Time.deltaTime;
                else
                    Reset();
                return m_waiting;
            }
        }

        public override void Reset()
        {
            m_waitTime = -1f;
            YieldPool.BackToPool(this);
        }

        public WaitForSecondsPooled Wait(float time)
        {
            m_waitTime = time;
            return this;
        }
    }
}
#endif