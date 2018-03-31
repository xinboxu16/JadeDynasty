using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public interface IImpactLogic
    {
        void StartImpact(CharacterInfo obj, int impactId);

        void Tick(CharacterInfo obj, int impactId);

        void OnInterrupted(CharacterInfo obj, int impactId);

        void OnAddImpact(CharacterInfo obj, int impactId, int addImpactId);

        int RefixHpDamage(CharacterInfo obj, int impactId, int hpDamage, int senderId);
    }

    public abstract class AbstractImpactLogic : IImpactLogic
    {
        public delegate void ImpactLogicDamageDelegate(CharacterInfo entity, int attackerId, int damage, bool isKiller, bool isCritical, bool isOrdinary);
        public delegate void ImpactLogicRageDelegate(CharacterInfo entity, int rage);
        public static ImpactLogicRageDelegate EventImpactLogicRage;
        public static ImpactLogicDamageDelegate EventImpactLogicDamage;

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

        //发生伤害
        protected void ApplyDamage(CharacterInfo obj, int impactId)
        {
            if (null != obj && !obj.IsDead())
            {
                //表示pvp或pve
                if (GlobalVariables.Instance.IsClient && obj.SceneContext.IsRunWithRoomServer)
                {
                    return;
                }

                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
                if(null != impactId)
                {
                    CharacterInfo sender = obj.SceneContext.GetCharacterInfoById(impactInfo.m_ImpactSenderId);
                    int skillLevel = 0;
                    bool isCritical = false;//重要的
                    bool isOrdinary = false;//普通的
                    bool isKiller = false;

                    if(null != sender)
                    {
                        SkillInfo skillInfo = sender.GetSkillStateInfo().GetSkillInfoById(impactInfo.m_SkillId);
                        if (null != skillInfo)
                        {
                            skillLevel = skillInfo.SkillLevel;
                            if (skillInfo.ConfigData.Category == SkillCategory.kAttack)
                            {
                                isOrdinary = true;
                            }
                        }
                        //伤害
                        int curDamage = DamageCalculator.CalcImpactDamage(
                          sender,
                          obj,
                          (SkillDamageType)impactInfo.ConfigData.DamageType,
                          ElementDamageType.DC_None == (ElementDamageType)impactInfo.ConfigData.ElementType ? sender.GetEquipmentStateInfo().WeaponDamageType : (ElementDamageType)impactInfo.ConfigData.ElementType,
                          impactInfo.ConfigData.DamageRate + skillLevel * impactInfo.ConfigData.LevelRate,
                          impactInfo.ConfigData.DamageValue,
                          out isCritical);

                        foreach (ImpactInfo ii in obj.GetSkillStateInfo().GetAllImpact())
                        {
                            IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(ii.ConfigData.ImpactLogicId);
                            if (null != logic)
                            {
                                curDamage = logic.RefixHpDamage(obj, ii.m_ImpactId, curDamage, sender.GetId());
                            }
                        }

                        // 计算出的伤害小于1时， 不处理
                        if (curDamage < 1)
                        {
                            return;
                        }

                        UserInfo user = obj as UserInfo;
                        if (null != user)
                        {
                            user.GetCombatStatisticInfo().AddTotalDamageToMyself(curDamage);
                        }
                        if(curDamage == 2)
                        {
                            Debug.Log("curDamage");
                        }

                        if (curDamage > 2)
                        {
                            Debug.Log("ApplyDamage" + curDamage);
                        }

                        curDamage = curDamage * -1;
                        int realDamage = curDamage;
                        if (obj.Hp + curDamage < 0)
                        {
                            realDamage = 0 - obj.Hp;
                        }

                        obj.SetHp(Operate_Type.OT_Relative, realDamage);
                        if (obj.Hp <= 0)
                        {
                            isKiller = true;
                        }

                        if (null != EventImpactLogicDamage)
                        {
                            EventImpactLogicDamage(obj, sender.GetId(), curDamage, isKiller, isCritical, isOrdinary);
                        }
                    }
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

        public virtual int RefixHpDamage(CharacterInfo obj, int impactId, int hpDamage, int senderId)
        {
            return hpDamage;
        }
    }
}
