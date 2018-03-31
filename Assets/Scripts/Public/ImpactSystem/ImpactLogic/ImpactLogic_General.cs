using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class ImpactLogic_General : AbstractImpactLogic
    {
        public override void StartImpact(CharacterInfo obj, int impactId)
        {
            if (null != obj)
            {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
                if (null != impactInfo)
                {
                    if (impactInfo.m_IsActivated)
                    {
                        float damageDelayTime = float.Parse(impactInfo.ConfigData.ExtraParams[0]);
                        if (damageDelayTime < 0.01f)
                        {
                            //不是无敌状态
                            if (!obj.IsHaveStateFlag(CharacterState_Type.CST_Invincible))
                            {
                                ApplyDamage(obj, impactId);
                            }
                            ApplyRage(obj, impactInfo);
                            impactInfo.m_HasEffectApplyed = true;
                        }
                    }
                }
            }
            base.StartImpact(obj, impactId);
        }
        public override void Tick(CharacterInfo character, int impactId)
        {
            ImpactInfo impactInfo = character.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != impactInfo)
            {
                if (impactInfo.m_IsActivated)
                {
                    float damageDelayTime = float.Parse(impactInfo.ConfigData.ExtraParams[0]);
                    if (damageDelayTime > 0.01f && TimeUtility.GetServerMilliseconds() > impactInfo.m_StartTime + damageDelayTime * 1000 && !impactInfo.m_HasEffectApplyed)
                    {
                        int damage = int.Parse(impactInfo.ConfigData.ExtraParams[1]);
                        if (!character.IsHaveStateFlag(CharacterState_Type.CST_Invincible))
                        {
                            ApplyDamage(character, impactId);
                        }
                        impactInfo.m_HasEffectApplyed = true;
                        ApplyRage(character, impactInfo);
                    }

                    if (TimeUtility.GetServerMilliseconds() > impactInfo.m_StartTime + impactInfo.m_ImpactDuration)
                    {
                        impactInfo.m_IsActivated = false;
                    }
                }
            }
        }

        //愤怒值
        private void ApplyRage(CharacterInfo target, ImpactInfo impactInfo)
        {
            if (impactInfo.ConfigData.ExtraParams.Count >= 3)
            {
                int rage = int.Parse(impactInfo.ConfigData.ExtraParams[2]);
                if (target.IsUser)
                {
                    target.SetRage(Operate_Type.OT_Relative, rage);
                    if (null != EventImpactLogicRage)
                    {
                        EventImpactLogicRage(target, target.Rage);
                    }
                }
                else
                {
                    CharacterInfo user = target.SceneContext.GetCharacterInfoById(impactInfo.m_ImpactSenderId);
                    if (user != null && user.IsUser)
                    {
                        user.SetRage(Operate_Type.OT_Relative, rage);
                        if (null != EventImpactLogicRage)
                        {
                            EventImpactLogicRage(user, user.Rage);
                        }
                    }
                }
            }
        }
    }
}
