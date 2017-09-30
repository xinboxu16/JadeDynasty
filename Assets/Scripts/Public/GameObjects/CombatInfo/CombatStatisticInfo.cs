using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class CombatStatisticInfo
    {
        private long m_LastHitTime = 0;
        private int m_MultiHitCount = 1;
        private int m_DeadCount = 0;
        private bool m_DataChanged = false;

        private int m_KillHeroCount = 0;
        private int m_AssitKillCount = 0;
        private int m_KillNpcCount = 0;

        public long LastHitTime
        {
            get { return m_LastHitTime; }
            set { m_LastHitTime = value; }
        }

        public int MultiHitCount
        {
            get { return m_MultiHitCount; }
            set { m_MultiHitCount = value; }
        }

        public void AddDeadCount(int count)
        {
            m_DeadCount += count;
            m_DataChanged = true;
        }

        public int DeadCount
        {
            get { return m_DeadCount; }
            set { m_DeadCount = value; m_DataChanged = true; }
        }
        public int KillHeroCount
        {
            get { return m_KillHeroCount; }
            set { m_KillHeroCount = value; m_DataChanged = true; }
        }
        public int AssitKillCount
        {
            get { return m_AssitKillCount; }
            set { m_AssitKillCount = value; m_DataChanged = true; }
        }
        public int KillNpcCount
        {
            get { return m_KillNpcCount; }
            set { m_KillNpcCount = value; m_DataChanged = true; }
        }
    }
}
