using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class CombatStatisticInfo
    {
        private int m_DeadCount = 0;
        private int m_KillHeroCount = 0;
        private int m_AssitKillCount = 0;
        private int m_KillNpcCount = 0;
        private int m_ContinueKillCount = 0;
        private int m_ContinueDeadCount = 0;
        private int m_MultiKillCount = 0;
        private int m_MaxContinueKillCount = 0;
        private int m_MaxMultiKillCount = 0;
        private int m_KillTowerCount = 0;
        private int m_TotalDamageFromMyself = 0;
        private int m_TotalDamageToMyself = 0;
        private bool m_DataChanged = false;

        private long m_LastKillHeroTime = 0;

        private int m_MultiHitCount = 1;
        private int m_MaxMultiHitCount = 0;
        private int m_HitCount = 0;
        private long m_LastHitTime = 0;

        public void Reset()
        {
            m_DeadCount = 0;
            m_KillHeroCount = 0;
            m_AssitKillCount = 0;
            m_KillNpcCount = 0;
            m_DataChanged = false;

            //TODO 未实现
            //m_ContinueKillCount = 0;
            //m_ContinueDeadCount = 0;
            //m_MultiKillCount = 0;
            //m_MaxContinueKillCount = 0;
            //m_MaxMultiKillCount = 0;
            //m_KillTowerCount = 0;
            m_TotalDamageFromMyself = 0;
            //m_TotalDamageToMyself = 0;
            

            //m_LastKillHeroTime = 0;

            m_MaxMultiHitCount = 0;
            
            m_HitCount = 0;

            m_MultiHitCount = 1;
            m_LastHitTime = 0;
        }

        public int HitCount
        {
            get { return m_HitCount; }
            set { m_HitCount = value; }
        }

        public int MaxMultiHitCount
        {
            get { return m_MaxMultiHitCount; }
            set { m_MaxMultiHitCount = value; }
        }

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

        public void AddTotalDamageFromMyself(int val)
        {
            m_TotalDamageFromMyself += val;
            m_DataChanged = true;
        }

        public void AddTotalDamageToMyself(int val)
        {
            m_TotalDamageToMyself += val;
            m_DataChanged = true;
        }

        public int TotalDamageToMyself
        {
            get { return m_TotalDamageToMyself; }
            set { m_TotalDamageToMyself = value; m_DataChanged = true; }
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
