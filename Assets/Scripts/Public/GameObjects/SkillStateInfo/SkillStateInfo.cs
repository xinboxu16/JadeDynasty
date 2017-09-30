using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SkillStateInfo
    {
        private List<NpcInfo> m_SummonObjects;
        private List<SkillInfo> m_SkillList;   // 技能容器
        private List<ImpactInfo> m_ImpactList; // 效果容器
        private Dictionary<int, ImpactInfo> m_ImpactListForCheck;  //用于校验的impact列表（保留时间会比正常impact长）
        private SkillInfo m_CurSkillInfo;      // 当前所释放的点技能
        private float m_CrossToRunTime = 0.3f;
        private float m_CrossToStandTime = 0.5f;

        private bool m_BuffChanged;            // BUFF状态是否改变
        private bool m_IsInCombat;             // 表示当前是否在战斗中

        public SkillStateInfo()
        {
            m_SkillList = new List<SkillInfo>();
            m_CurSkillInfo = null;
            m_ImpactList = new List<ImpactInfo>();
            m_ImpactListForCheck = new Dictionary<int, ImpactInfo>();
            m_SummonObjects = new List<NpcInfo>();
        }

        public void Reset()
        {
            m_SkillList.Clear();
            m_ImpactList.Clear();
            m_ImpactListForCheck.Clear();
            m_SummonObjects.Clear();
            m_CurSkillInfo = null;
            m_BuffChanged = false;
            m_IsInCombat = false;
        }

        public float CrossToRunTime
        {
            get { return m_CrossToRunTime; }
            set { m_CrossToRunTime = value; }
        }

        public float CrossToStandTime
        {
            get { return m_CrossToStandTime; }
            set { m_CrossToStandTime = value; }
        }

        public bool BuffChanged
        {
            get { return m_BuffChanged; }
            set { m_BuffChanged = value; }
        }

        public SkillInfo GetCurSkillInfo()
        {
            return m_CurSkillInfo;
        }

        public bool IsImpactActive()
        {
            int ct = m_ImpactList.Count;
            for(int i = ct - 1; i >= 0; --i)
            {
                ImpactInfo info = m_ImpactList[i];
                if(info.m_IsActivated)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsImpactControl()
        {
            int ct = m_ImpactList.Count;
            for(int i = ct - 1; i >= 0; --i)
            {
                ImpactInfo info = m_ImpactList[i];
                if(info.m_IsGfxControl)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddSummonObject(NpcInfo npc)
        {
            m_SummonObjects.Add(npc);
        }

        public List<NpcInfo> GetSummonObject()
        {
            return m_SummonObjects;
        }

        public void RecyleSummonObject(NpcInfo npc)
        {
            m_SummonObjects.Remove(npc);
        }

        public bool IsSkillActivated()
        {
            return (null == m_CurSkillInfo) ? false : m_CurSkillInfo.IsSkillActivated;
        }

        public void RemoveAllSkill()
        {
            m_SkillList.Clear();
        }

        public void RemoveAllImpact()
        {
            m_ImpactList.Clear();
            m_BuffChanged = true;
        }

        public void AddImpact(ImpactInfo info)
        {
            ImpactInfo oriImpact = GetImpactInfoById(info.m_ImpactId);
            if (oriImpact == null)
            {
                m_ImpactList.Add(info);
            }
            else
            {
                m_ImpactList.Remove(oriImpact);
                m_ImpactList.Add(info);
            }
            if ((int)ImpactType.BUFF == info.m_ImpactType)
            {
                m_BuffChanged = true;
            }
            if (m_ImpactListForCheck.ContainsKey(info.m_ImpactId))
            {
                m_ImpactListForCheck[info.m_ImpactId] = info;
            }
            else
            {
                m_ImpactListForCheck.Add(info.m_ImpactId, info);
            }
        }

        public void RemoveImpact(int impactId)
        {
            ImpactInfo oriImpact = GetImpactInfoById(impactId);
            if(oriImpact != null)
            {
                if((int)ImpactType.BUFF == oriImpact.m_ImpactType)
                {
                    m_BuffChanged = true;
                }
                m_ImpactList.Remove(oriImpact);
            }
        }

        //用于校验的impact比正常时间晚5秒清除
        public void CleanupImpactInfoForCheck(long curTime, long additionalLifeTime)
        {
            List<int> deletes = new List<int>();
            foreach (KeyValuePair<int, ImpactInfo> pair in m_ImpactListForCheck)
            {
                if(null != pair.Value && null != pair.Value.ConfigData)
                {
                    if((pair.Value.m_StartTime + pair.Value.ConfigData.ImpactTime + additionalLifeTime) < curTime)
                    {
                        deletes.Add(pair.Key);
                    }
                }
                else
                {
                    deletes.Add(pair.Key);
                }
            }
            foreach (int id in deletes)
            {
                m_ImpactListForCheck.Remove(id);
            }
        }

        public ImpactInfo GetImpactInfoById(int impactId)
        {
            return m_ImpactList.Find(
                delegate(ImpactInfo info)
                {
                    return info.m_ImpactId == impactId;
                });
        }

        public void AddSkill(SkillInfo info)
        {
            m_SkillList.Add(info);
        }

        public void RemoveSkill(int skillId)
        {
            SkillInfo oriSkill = GetSkillInfoById(skillId);
            if (oriSkill != null)
            {
                m_SkillList.Remove(oriSkill);
            }
        }

        public SkillInfo GetSkillInfoById(int skillId)
        {
            return m_SkillList.Find(delegate(SkillInfo info)
            {
                if (info == null) return false;
                return info.SkillId == skillId;
            });
        }

        public List<SkillInfo> GetAllSkill()
        {
            return m_SkillList;
        }

        public List<ImpactInfo> GetAllImpact()
        {
            return m_ImpactList;
        }
    }
}
