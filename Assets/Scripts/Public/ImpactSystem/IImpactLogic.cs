using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public interface IImpactLogic
    {
        void StartImpact(CharacterInfo obj, int impactId);

        void Tick(CharacterInfo obj, int impactId);

        void OnInterrupted(CharacterInfo obj, int impactId);

        void OnAddImpact(CharacterInfo obj, int impactId, int addImpactId);
    }

    public abstract class AbstractImpactLogic : IImpactLogic
    {
        public virtual void StartImpact(CharacterInfo obj, int impactId)
        {
            if (null != obj)
            {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
                if (null != impactInfo)
                {
                    if (impactInfo.ConfigData.BreakSuperArmor)//破甲
                    {
                        obj.SuperArmor = false;
                    }
                }
                if (obj is NpcInfo)
                {
                    NpcInfo npcObj = obj as NpcInfo;
                    NpcAiStateInfo aiInfo = npcObj.GetAiStateInfo();
                    if (null != aiInfo && 0 == aiInfo.HateTarget)
                    {
                        aiInfo.HateTarget = impactInfo.m_ImpactSenderId;
                    }
                }
            }
        }

        public virtual void Tick(CharacterInfo obj, int impactId)
        {
            ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != impactInfo && impactInfo.m_IsActivated)
            {
                long curTime = TimeUtility.GetServerMilliseconds();
                if (curTime > impactInfo.m_StartTime + impactInfo.m_ImpactDuration)
                {
                    impactInfo.m_IsActivated = false;
                }
            }
        }

        public virtual void OnInterrupted(CharacterInfo obj, int impactId)
        {
            StopImpact(obj, impactId);
        }

        public virtual void StopImpact(CharacterInfo obj, int impactId)
        {
        }

        public virtual void OnAddImpact(CharacterInfo obj, int impactId, int addImpactId)
        {
        }
    }
}
