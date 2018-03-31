using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class GameLogicNotification : IGameLogicNotification
    {
        public void OnGfxStartStory(int id)
        {
            if(WorldSystem.Instance.IsPveScene())
            {
                ClientStorySystem.Instance.StartStory(id);
            }
            else
            {
                // OnGfxStartStory 只会在单人pve场景中被调用
            }
        }

        public void OnGfxSendStoryMessage(string msgId, object[] args)
        {
            if (WorldSystem.Instance.IsPureClientScene() || WorldSystem.Instance.IsPveScene())
            {
                ClientStorySystem.Instance.SendMessage(msgId, args);
            }
            else
            {
                //通知服务器
                string msgIdPrefix = "dialogover:";
                if (msgId.StartsWith(msgIdPrefix))
                {
                    DashFireMessage.Msg_CR_DlgClosed msg = new DashFireMessage.Msg_CR_DlgClosed();
                    msg.dialog_id = int.Parse(msgId.Substring(msgIdPrefix.Length));
                    Network.NetworkSystem.Instance.SendMessage(msg);
                }
            }
        }

        public void OnGfxControlMoveStart(int objId, int id, bool isSkill)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(objId);
            if(null != charObj)
            {
                charObj.GetMovementStateInfo().IsSkillMoving = true;
                if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                {
                    if (objId == WorldSystem.Instance.PlayerSelfId || charObj.OwnerId == WorldSystem.Instance.PlayerSelfId)
                    {
                        //TODO未实现
                        //Network.NetworkSystem.Instance.SyncGfxMoveControlStart(charObj, id, isSkill);
                    }
                }
            }
        }

        public void OnGfxControlMoveStop(int objId, int id, bool isSkill)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(objId);
            if (null != charObj)
            {
                charObj.GetMovementStateInfo().IsSkillMoving = false;
                if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                {
                    if (objId == WorldSystem.Instance.PlayerSelfId || charObj.OwnerId == WorldSystem.Instance.PlayerSelfId)
                    {
                        //TODO未实现
                        //Network.NetworkSystem.Instance.SyncGfxMoveControlStop(charObj, id, isSkill);
                    }
                }
            }
        }

        public void OnGfxMoveMeetObstacle(int id)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if(null != charObj)
            {
                charObj.GetMovementStateInfo().IsMoveMeetObstacle = true;
                WorldSystem.Instance.NotifyMoveMeetObstacle(false);
            }
        }

        public void OnGfxStartSkill(int id, SkillCategory category, float x, float y, float z)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (null != charObj)
            {
                if (charObj.SkillController != null)
                {
                    charObj.SkillController.PushSkill(category, new Vector3(x, y, z));
                }
            }
        }

        public void OnGfxStartAttack(int id, float x, float y, float z)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if(null != charObj)
            {
                charObj.SkillController.StartAttack(new Vector3(x, y, z));
            }
        }

        public void OnGfxStopAttack(int id)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (null != charObj)
            {
                charObj.SkillController.StopAttack();
            }
        }

        public void OnGfxHitTarget(int id, int impactId, int targetId, int hitCount, int skillId, int duration, float x, float y, float z, float dir)
        {
            CharacterInfo sender = WorldSystem.Instance.GetCharacterById(id);
            CharacterInfo target = WorldSystem.Instance.GetCharacterById(targetId);

            UserInfo playerSelf = WorldSystem.Instance.GetPlayerSelf();
            bool hitCountChanged = false;
            //用于对敌人的时候
            if(id == WorldSystem.Instance.PlayerSelfId && null != playerSelf)
            {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if(hitCount > 0)
                {
                    CombatStatisticInfo combatInfo = playerSelf.GetCombatStatisticInfo();
                    long lastHitTime = combatInfo.LastHitTime;
                    if(0 == lastHitTime || lastHitTime + 1500 > curTime)
                    {
                        combatInfo.MultiHitCount = combatInfo.MultiHitCount + hitCount;
                    }
                    if(combatInfo.MaxMultiHitCount < combatInfo.MultiHitCount)
                    {
                        combatInfo.MaxMultiHitCount = combatInfo.MultiHitCount;
                        hitCountChanged = true;
                    }
                    combatInfo.LastHitTime = curTime;
                    if (combatInfo.MultiHitCount > 1)
                    {
                        GfxSystem.PublishGfxEvent("ge_hitcount", "ui", combatInfo.MultiHitCount);
                    }
                }
            }

            //精英场景 连击
            if(targetId == WorldSystem.Instance.PlayerSelfId && null != playerSelf)
            {
                if(hitCount > 0)
                {
                    CombatStatisticInfo combatInfo = playerSelf.GetCombatStatisticInfo();
                    combatInfo.HitCount += hitCount;
                    hitCountChanged = true;
                    //精英场景
                    if(WorldSystem.Instance.IsELiteScene())
                    {
                        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
                        SceneResource curScene = WorldSystem.Instance.GetCurScene();
                        if (null != roleInfo && null != curScene && roleInfo.GetSceneInfo(WorldSystem.Instance.GetCurSceneId()) == 2)
                        {   //当前在挑战3星通关
                            GfxSystem.PublishGfxEvent("ge_pve_fightinfo", "ui", 0, combatInfo.HitCount, curScene.SceneConfig.m_CompletedHitCount, 0);
                        }
                    }
                }
            }

            //pvp或多人pve
            if (hitCountChanged && null != playerSelf && (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene()))
            {
                CombatStatisticInfo combatInfo = playerSelf.GetCombatStatisticInfo();
                DashFireMessage.Msg_CR_HitCountChanged msg = new DashFireMessage.Msg_CR_HitCountChanged();
                msg.max_multi_hit_count = combatInfo.MaxMultiHitCount;
                msg.hit_count = combatInfo.HitCount;
                Network.NetworkSystem.Instance.SendMessage(msg);
            }

            if(null != sender && null != target)
            {
                OnGfxStartImpact(sender.GetId(), impactId, target.GetId(), skillId, duration, new Vector3(x, y, z), dir);
            }
        }

        public void OnGfxStartImpact(int sender, int impactId, int target, int skillId, int duration, Vector3 srcPos, float dir)
        {
            CharacterInfo senderObj = WorldSystem.Instance.GetCharacterById(sender);
            if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
            {
                if (null != senderObj)
                {
                    bool isSend = false;
                    if (senderObj.GetId() == WorldSystem.Instance.PlayerSelfId)
                    {
                        isSend = true;
                    }
                    if (senderObj is NpcInfo)
                    {
                        if (senderObj.OwnerId == WorldSystem.Instance.GetPlayerSelf().GetId())
                        {
                            isSend = true;
                        }
                    }
                    if (isSend)
                    {
                        bool ret = ImpactSystem.Instance.SendImpactToCharacter(senderObj, impactId, target, skillId, duration, srcPos, dir);
                        if (ret)
                        {
                            //Network.NetworkSystem.Instance.SyncSendImpact(senderObj, impactId, target, skillId, duration, srcPos, dir);TODO未实现
                        }
                    }
                }
            }
            else
            {
                bool ret = ImpactSystem.Instance.SendImpactToCharacter(senderObj, impactId, target, skillId, duration, srcPos, dir);
            }
        }

        public void OnGfxForceStartSkill(int id, int skillId)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (null != charObj)
            {
                if (charObj.SkillController != null)
                {
                    charObj.SkillController.ForceStartSkill(skillId);
                }
            }
        }

        public void OnGfxStopSkill(int id, int skillId)
        {
            //TODO未实现
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (null != charObj)
            {
                if ((WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene()) && skillId != charObj.Combat2IdleSkill)
                {
                    //if (charObj.GetId() == WorldSystem.Instance.PlayerSelfId || charObj.OwnerId == WorldSystem.Instance.PlayerSelfId)
                    //{
                    //    LogSystem.Debug("---ongfxstopskill id={0}, skillid=", id, skillId);
                    //    Network.NetworkSystem.Instance.SyncPlayerStopSkill(charObj, skillId);
                    //}
                }
            }

            //TODO未懂
            if (skillId == charObj.Combat2IdleSkill)
            {
                CharacterView userview = EntityManager.Instance.GetCharacterViewById(id);
                if (userview != null)
                {
                    userview.OnCombat2IdleSkillOver();
                }
            }

            SkillInfo skillInfo = charObj.GetSkillStateInfo().GetSkillInfoById(skillId);
            if(null != skillInfo)
            {
                skillInfo.IsSkillActivated = false;
                LogSystem.Debug("-----OnGfxStopSkill " + skillId);
            }
        }

        public void OnGfxSkillBreakSection(int objid, int skillid, int breaktype, int startime, int endtime, bool isinterrupt)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(objid);
            if (null != charObj)
            {
                charObj.SkillController.AddBreakSection(skillid, breaktype, startime, endtime, isinterrupt);
            }
        }

        public void OnGfxStopImpact(int id, int impactId)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (null != charObj)
            {
                ImpactSystem.Instance.OnGfxStopImpact(charObj, impactId);
            }
        }

        public void OnGfxSetCrossFadeTime(int id, string fadeTargetAnim, float crossTime)
        {
            //TODO未实现
        }

        public void OnGfxAddLockInputTime(int id, SkillCategory category, float lockinputtime)
        {
            //TODO未实现
        }

        public void OnGfxSummonNpc(int owner_id, int owner_skill_id, int npc_type_id, string modelPrefab, int skillid, float x, float y, float z)
        {
            //TODO未实现
        }

        public void OnGfxDestroyObj(int id)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if (charObj == null)
            {
                return;
            }
            EntityManager.Instance.DestroyNpcView(charObj.GetId());
            WorldSystem.Instance.DestroyCharacterById(charObj.GetId());
        }

        public void OnGfxDestroySummonObject(int id)
        {
            //TODO未实现
        }

        public void OnGfxSetObjLifeTime(int id, long life_remaint_time)
        {
            //TODO未实现
        }

        public void OnGfxSimulateMove(int id)
        {
            //TODO未实现
        }

        public static GameLogicNotification Instance
        {
            get { return s_Instance; }
        }
        private static GameLogicNotification s_Instance = new GameLogicNotification();
    }
}
