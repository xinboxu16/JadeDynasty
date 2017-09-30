using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public delegate void ImpactEventHandler(CharacterInfo sender, int targetId, int impactId);
    public delegate void SendImpactEventHandler(CharacterInfo sender, int targetId, int impactId, Vector3 srcPos, float srcDir);

    public sealed class ImpactSystem
    {
        public static SendImpactEventHandler EventSendImpact;
        public static ImpactEventHandler EventStopImpact;

        public void Tick(CharacterInfo obj)
        {
            List<ImpactInfo> impactInfos = obj.GetSkillStateInfo().GetAllImpact();
            int ct = impactInfos.Count;
            for(int i = ct - 1; i >= 0; --i)
            {
                ImpactInfo info = impactInfos[i];
                IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(info.ConfigData.ImpactLogicId);
                if(info.m_IsActivated)
                {
                    if(null != logic)
                    {
                        logic.Tick(obj, info.m_ImpactId);
                    }
                }
                else if(!info.m_IsGfxControl)
                {
                    logic.OnInterrupted(obj, info.m_ImpactId);
                    obj.GetSkillStateInfo().RemoveImpact(info.m_ImpactId);
                }
            }
            obj.GetSkillStateInfo().CleanupImpactInfoForCheck(TimeUtility.GetServerMilliseconds(), 5000);//用于校验的impact比正常时间晚5秒清除
        }

        private bool SendImpactImpl(CharacterInfo sender, int impactId, int targetId, int skillId, int duration)
        {
            if (null != sender)
            {
                CharacterInfo target = sender.SceneContext.GetCharacterInfoById(targetId);
                if (null != target)
                {
                    if (target.IsUser && target.IsDead()) return false;
                    if (target.IsNpc && target.IsDead())
                    {
                        NpcInfo npc = target.CastNpcInfo();
                        if ((int)NpcTypeEnum.SceneObject == npc.NpcType)
                        {
                            return false;
                        }
                    }
                    ImpactLogicData impactLogicData = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_IMPACT, impactId) as ImpactLogicData;
                    if (null != impactLogicData)
                    {
                        IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(impactLogicData.ImpactLogicId);
                        if (null != logic)
                        {
                            ImpactInfo oldImpactInfo = target.GetSkillStateInfo().GetImpactInfoById(impactId);
                            if (null != oldImpactInfo)
                            {
                                logic.OnInterrupted(target, impactId);
                                target.GetSkillStateInfo().RemoveImpact(impactId);
                            }
                            ImpactInfo impactInfo = new ImpactInfo();
                            impactInfo.m_IsActivated = true;
                            impactInfo.m_SkillId = skillId;
                            impactInfo.m_ImpactId = impactLogicData.ImpactId;
                            impactInfo.m_ImpactType = impactLogicData.ImpactType;
                            impactInfo.m_BuffDataId = impactLogicData.BuffDataId;
                            impactInfo.ConfigData = impactLogicData;
                            impactInfo.m_StartTime = TimeUtility.GetServerMilliseconds();
                            impactInfo.m_ImpactDuration = impactLogicData.ImpactTime;
                            if (-1 == duration || duration > impactLogicData.ImpactTime)
                            {
                                impactInfo.m_ImpactDuration = impactLogicData.ImpactTime;
                            }
                            else
                            {
                                impactInfo.m_ImpactDuration = duration;
                            }
                            impactInfo.m_HasEffectApplyed = false;
                            if (0 == impactInfo.ConfigData.ImpactGfxLogicId)
                            {
                                impactInfo.m_IsGfxControl = false;
                            }
                            else
                            {
                                impactInfo.m_IsGfxControl = true;
                            }
                            impactInfo.m_ImpactSenderId = sender.GetId();
                            impactInfo.m_MaxMoveDistanceSqr = impactLogicData.CalcMaxMoveDistanceSqr();
                            if (impactLogicData.ImpactGfxLogicId == 0)
                            {
                                impactInfo.m_LeftEnableMoveCount = 0;//禁止位移
                            }
                            else
                            {
                                impactInfo.m_LeftEnableMoveCount = 1;//允许位移
                            }

                            target.GetSkillStateInfo().AddImpact(impactInfo);
                            logic.StartImpact(target, impactId);

                            if ((int)ImpactType.INSTANT == impactInfo.m_ImpactType)
                            {
                                impactInfo.m_IsActivated = false;
                            }
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        public bool SendImpactToCharacter(CharacterInfo sender, int impactId, int targetId, int skillId, int duration, Vector3 srcPos, float srcDir)
        {
            bool ret = SendImpactImpl(sender, impactId, targetId, skillId, duration);
            if (ret)
            {
                CharacterInfo target = sender.SceneContext.GetCharacterInfoById(targetId);

                if (null != target)
                {
                    OnAddImpact(target, impactId);
                }
                if (null != EventSendImpact)
                {
                    EventSendImpact(sender, targetId, impactId, srcPos, srcDir);
                }
            }
            return ret;
        }

        public bool StopImpactById(CharacterInfo target, int impactId)
        {
            bool ret = StopImpactImpl(target, impactId);
            if (ret)
            {
                if (null != EventStopImpact)
                {
                    EventStopImpact(null, target.GetId(), impactId);
                }
            }
            return ret;
        }

        private bool StopImpactImpl(CharacterInfo target, int impactId)
        {
            ImpactInfo impactInfo = target.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != impactInfo)
            {
                IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(impactInfo.ConfigData.ImpactLogicId);
                if (null != logic)
                {
                    logic.OnInterrupted(target, impactId);
                }
                impactInfo.m_IsActivated = false;
                return true;
            }
            return false;
        }

        private void OnAddImpact(CharacterInfo target, int impactId)
        {
            foreach (ImpactInfo info in target.GetSkillStateInfo().GetAllImpact())
            {
                if (info.m_ImpactId != impactId)
                {
                    IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(info.ConfigData.ImpactLogicId);
                    if (null != logic)
                    {
                        logic.OnAddImpact(target, info.m_ImpactId, impactId);
                    }
                }
            }
        }

        private ImpactSystem() { }

        public static ImpactSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static ImpactSystem s_Instance = new ImpactSystem();
    }
}
