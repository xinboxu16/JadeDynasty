using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed class ImpactViewManager
    {
        public void Init()
        {
            ImpactSystem.EventSendImpact += this.OnSendImpact;
            ImpactSystem.EventStopImpact += this.OnStopImpact;
            ImpactSystem.EventGfxStopImpact += this.OnGfxStopImpact;

            AbstractImpactLogic.EventImpactLogicDamage += this.OnImpactDamage;
        }

        #region ImpactSystem && AbstractImpactLogic

        private void OnSendImpact(CharacterInfo sender, int targetId, int impactId, Vector3 srcPos, float srcDir)
        {
            CharacterView senderView = EntityManager.Instance.GetCharacterViewById(sender.GetId());
            CharacterView targetView = EntityManager.Instance.GetCharacterViewById(targetId);

            if(null != senderView && null != targetView)
            {
                CharacterInfo target = WorldSystem.Instance.GetCharacterById(targetId);
                if (null != target)
                {
                    // 施法者能造成硬直且受击方没有霸体
                    ImpactInfo impactInfo = target.GetSkillStateInfo().GetImpactInfoById(impactId);
                    if (null == impactInfo) return;
                    int forceLogicId = -1;
                    if (sender.CauseStiff && !target.SuperArmor && !target.UltraArmor)
                    {
                        // 正常造成硬直
                    }
                    else
                    {
                        forceLogicId = 0;
                        impactInfo.m_IsGfxControl = false;
                    }

                    // 场景破坏物体单独处理
                    if (target.IsNpc)
                    {
                        NpcInfo npcInfo = target.CastNpcInfo();
                        if (npcInfo.NpcType == (int)NpcTypeEnum.SceneObject)
                        {
                            forceLogicId = 1;
                            impactInfo.m_IsGfxControl = true;
                        }
                    }

                    // 打断技能
                    if ((null != impactInfo && 0 != impactInfo.ConfigData.ImpactGfxLogicId && forceLogicId < 0) || forceLogicId > 0)
                    {
                        if (null != target.SkillController)
                        {
                            target.SkillController.ForceInterruptCurSkill();
                        }
                        else
                        {
                            LogSystem.Warn("{0} does't have a skillcontroller", target.GetName());
                        }
                    }

                    GfxSystem.QueueGfxAction(GfxModule.Impact.GfxImpactSystem.Instance.SendImpactToCharacter, senderView.Actor, targetView.Actor, impactId, srcPos.x, srcPos.y, srcPos.z, srcDir, forceLogicId);
                }
            }
        }

        private void OnStopImpact(CharacterInfo sender, int targetId, int impactId)
        {
            //TODO 未实现
            Debug.Log("OnStopImpact");
        }

        private void OnGfxStopImpact(CharacterInfo sender, int targetId, int impactId)
        {
            CharacterInfo target = WorldSystem.Instance.GetCharacterById(targetId);
            if (null != target)
            {
                if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                {
                    if (target.GetId() == WorldSystem.Instance.PlayerSelfId)
                    {
                        //TODO 未实现
                        //Network.NetworkSystem.Instance.SyncStopGfxImpact(sender,targetId,impactId);
                    }
                    if (target.OwnerId == WorldSystem.Instance.PlayerSelfId)
                    {
                        //TODO 未实现
                        //Network.NetworkSystem.Instance.SyncStopGfxImpact(sender,targetId,impactId);
                    }
                }
            }
        }

        private void OnImpactDamage(CharacterInfo entity, int attackerId, int damage, bool isKiller, bool isCritical, bool isOrdinary)
        {
            if (WorldSystem.Instance.IsPureClientScene() || WorldSystem.Instance.IsPveScene())
            {
                if (null != entity)
                {
                    entity.SetAttackerInfo(attackerId, 0, isKiller, isOrdinary, isCritical, damage, 0);
                }
            }
        }

        #endregion

        public static ImpactViewManager Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static ImpactViewManager s_Instance = new ImpactViewManager();
    }
}
