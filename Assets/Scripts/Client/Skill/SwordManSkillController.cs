using GfxModule.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SwordManSkillController : SkillController
    {
        private static int MOVE_BREAK_TYPE = 1;
        private GfxSkillSystem m_GfxSkillSystem;
        private int m_PlayerSelfId;
        private CharacterInfo m_Owner;

        public SwordManSkillController(CharacterInfo entity, GfxSkillSystem gfxskillsystem)
        {
            m_Owner = entity;
            m_PlayerSelfId = m_Owner.GetId();
            m_GfxSkillSystem = gfxskillsystem;
        }

        public override void Init()
        {
            //配置文件里的就能数据
            List<SkillLogicData> playerSkillConfig = new List<SkillLogicData>();
            //当前人物的所有技能
            List<SkillInfo> playerSkills = m_Owner.GetSkillStateInfo().GetAllSkill();
            //添加到技能列表里
            foreach(SkillInfo info in playerSkills)
            {
                if(info.ConfigData != null && info.ConfigData.Category != SkillCategory.kNone)
                {
                    playerSkillConfig.Add(info.ConfigData);
                }
            }

            InitSkills(playerSkillConfig);
            LogSystem.Debug("-----------------init skill of " + m_Owner.GetId());
        }

        private void InitSkills(List<SkillLogicData> skills)
        {
            m_SkillCategoryDict.Clear();
            foreach(SkillLogicData sd in skills)
            {
                if(IsCategoryContain(sd.SkillId))
                {
                    continue;
                }
                SkillNode firstNode = InitCategorySkillNode(skills, sd);
                m_SkillCategoryDict[sd.Category] = firstNode;
            }

            foreach(int id in m_UnlockedSkills)
            {
                SkillNode node = GetSkillNodeById(id);
                if(null != node)
                {
                    node.IsLocked = false;
                }
            }
        }

        private bool IsCategoryContain(int skillId)
        {
            foreach(SkillNode head in m_SkillCategoryDict.Values)
            {
                if(FindSkillNodeInChildren(head, skillId) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsSkillActive(SkillNode node)
        {
            SkillInfo curSkill = GetSkillInfoByNode(node);
            if(curSkill == null)
            {
                return false;
            }
            return curSkill.IsSkillActivated;
        }

        public override bool IsSkillCanStart(SkillNode node, out SkillCannotCastType reason)
        {
            SkillStateInfo state = m_Owner.GetSkillStateInfo();
            SkillInfo info = state.GetSkillInfoById(node.SkillId);
            reason = SkillCannotCastType.kUnknow;
            if(info == null)
            {
                reason = SkillCannotCastType.kNotFindSkill;
                return false;
            }
            if(m_Owner.IsDead())
            {
                reason = SkillCannotCastType.kOwnerDead;
                return false;
            }
            if (state != null && (state.IsImpactControl() && m_Owner.IsUnderControl()) && !CanStartWhenImpactControl(info))//IsUnderControl在动作中
            {
                reason = SkillCannotCastType.kCannotCtrol;
                return false;
            }
            if(IsSkillInCD(node))
            {
                reason = SkillCannotCastType.kInCD;
                return false;
            }
            if(!IsCostEnough(node))
            {
                reason = SkillCannotCastType.kCostNotEnough;
                return false;
            }
            return true;
        }

        public override bool IsSkillCanBreak(SkillNode node, SkillNode nextNode)
        {
            bool IsInterrupt;//打断
            return IsSkillCanBreak(node, GetSkillNodeBreakType(nextNode), out IsInterrupt);
        }

        public override bool IsSkillCanBreak(SkillNode node, int breakType, out bool isInterrupt)
        {
            isInterrupt = false;
            SkillInfo curSkill = GetSkillInfoByNode(node);
            if(curSkill == null)
            {
                return true;
            }

            return curSkill.CanBreak(breakType, TimeUtility.GetServerMilliseconds(), out isInterrupt);
        }

        private int GetSkillNodeBreakType(SkillNode node)
        {
            int breakType = MOVE_BREAK_TYPE;
            SkillInfo nextSkill = GetSkillInfoByNode(node);
            if(nextSkill != null)
            {
                breakType = nextSkill.ConfigData.BreakType;
            }
            return breakType;
        }

        public SkillInfo GetSkillInfoByNode(SkillNode node)
        {
            SkillInfo result = null;
            if(node != null)
            {
                SkillStateInfo state = m_Owner.GetSkillStateInfo();
                if(state != null)
                {
                    result = state.GetSkillInfoById(node.SkillId);
                }
            }
            return result;
        }

        public override bool StartSkill(SkillNode node)
        {
            if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
            {
                //if (m_Owner.GetId() == WorldSystem.Instance.PlayerSelfId || m_Owner.OwnerId == WorldSystem.Instance.PlayerSelfId)
                //{
                //    Network.NetworkSystem.Instance.SyncPlayerSkill(m_Owner, node.SkillId);
                //}
            }
            else
            {
                if(!MakeSkillCost(node))
                {
                    return false;
                }
            }
            m_Owner.ResetCross2StandRunTime();//切换到站立或跑步状态的时间
            m_Owner.GetSkillStateInfo().SetCurSkillInfo(node.SkillId);
            SkillInfo curSkill = m_Owner.GetSkillStateInfo().GetCurSkillInfo();
            if(null == curSkill)
            {
                LogSystem.Error("----------SkillError: {0} set cur skill {1} failed! it's not exist", m_Owner.GetId(), node.SkillId);
                return false;
            }
            curSkill.IsSkillActivated = true;
            curSkill.IsInterrupted = false;
            curSkill.StartTime = TimeUtility.GetServerMilliseconds() / 1000.0f;
            UpdateSkillCD(node);
            LogSystem.Debug("---start skill at {0}--{1}", curSkill.SkillId, curSkill.StartTime);

            CharacterView view = EntityManager.Instance.GetCharacterViewById(m_Owner.GetId());
            if (view != null)
            {
                GfxSystem.QueueGfxAction(m_GfxSkillSystem.StartSkill, view.Actor, node.SkillId);
            }
            SimulateStartSkill(m_Owner, node.SkillId);
            return true;
        }

        private void SimulateStartSkill(CharacterInfo owner, int skillid)
        {
            SkillNode node = GetSkillNodeById(skillid);
            if (node != null && node.Category == SkillCategory.kEx)
            {
                return;
            }
            List<NpcInfo> summons = owner.GetSkillStateInfo().GetSummonObject();
            foreach (NpcInfo npc in summons)
            {
                if (npc.IsSimulateMove)
                {
                    npc.SkillController.ForceStartSkill(skillid);
                }
            }
        }

        private bool IsCostEnough(SkillNode node)
        {
            SkillInfo info = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
            if (info == null)
            {
                return false;
            }
            if (m_Owner.Energy >= info.ConfigData.CostEnergy
              && m_Owner.Rage >= info.ConfigData.CostRage)
            {
                return true;
            }
            return false;
        }

        private bool MakeSkillCost(SkillNode node)
        {
            SkillInfo info = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
            if(null == info)
            {
                return false;
            }
            if (m_Owner.Energy < info.ConfigData.CostEnergy)//能量值
            {
                return false;
            }
            if (m_Owner.Rage < info.ConfigData.CostRage)//愤怒值
            {
                return false;
            }
            m_Owner.SetEnergy(Operate_Type.OT_Relative, -info.ConfigData.CostEnergy);
            m_Owner.SetRage(Operate_Type.OT_Relative, -info.ConfigData.CostRage);
            return true;
        }

        public override bool StopSkill(SkillNode node, SkillNode nextnode)
        {
            return StopSkill(node, GetSkillNodeBreakType(nextnode));
        }

        private bool StopSkill(SkillNode node, int breaktype)
        {
            if(!IsSkillActive(node))
            {
                return true;
            }

            bool isinterrupt;
            if (!IsSkillCanBreak(node, breaktype, out isinterrupt))
            {
                return false;
            }

            CharacterView view = EntityManager.Instance.GetCharacterViewById(m_Owner.GetId());
            if (view != null)
            {
                //技能打断时逻辑层处理状态（包括技能激活状态与移动控制状态）并通知服务器，Gfx层不再处理状态
                if(isinterrupt)
                {
                    m_Owner.GetMovementStateInfo().IsSkillMoving = false;
                    view.ObjectInfo.IsSkillGfxMoveControl = false;
                    view.ObjectInfo.IsSkillGfxRotateControl = false;
                    view.ObjectInfo.IsSkillGfxAnimation = false;

                    SkillInfo skillInfo = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
                    if(null != skillInfo)
                    {
                        skillInfo.IsSkillActivated = false;
                        LogSystem.Debug("---------StopSkill " + node.SkillId);
                    }
                    //TODO pvp未实现
                    //if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                    //{
                    //    if (m_Owner.GetId() == WorldSystem.Instance.PlayerSelfId || m_Owner.OwnerId == WorldSystem.Instance.PlayerSelfId)
                    //    {
                    //        Network.NetworkSystem.Instance.SyncGfxMoveControlStop(m_Owner, node.SkillId, true);
                    //        Network.NetworkSystem.Instance.SyncPlayerStopSkill(m_Owner, node.SkillId);
                    //    }
                    //}
                }
                //通知Gfx停止技能
                GfxSystem.QueueGfxAction(m_GfxSkillSystem.StopSkill, view.Actor, isinterrupt);
                SimulateStopSkill(m_Owner);
            }
            return true;
        }

        private void SimulateStopSkill(CharacterInfo owner)
        {
            List<NpcInfo> summons = owner.GetSkillStateInfo().GetSummonObject();
            foreach (NpcInfo npc in summons)
            {
                if (npc.IsSimulateMove)
                {
                    npc.SkillController.ForceInterruptCurSkill();
                }
            }
        }

        private void UpdateSkillCD(SkillNode node)
        {
            if(node == null)
            {
                return;
            }
            SkillNode head = GetHead(node.Category);
            SkillInfo headInfo = m_Owner.GetSkillStateInfo().GetSkillInfoById(head.SkillId);
            if(head.SkillId == node.SkillId)
            {
                headInfo.BeginCD();
            }
            else
            {
                SkillInfo nodeInfo = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
                headInfo.AddCD(nodeInfo.ConfigData.CoolDownTime);
            }
        }

        public override float GetSkillCD(SkillNode node)
        {
            if(null == node)
            {
                return 0;
            }
            SkillNode head = GetHead(node.Category);
            if (null == head)
            {
                return 0;
            }
            SkillInfo headInfo = m_Owner.GetSkillStateInfo().GetSkillInfoById(head.SkillId);
            return headInfo.GetCD(TimeUtility.GetServerMilliseconds() / 1000.0f);
        }

        public override void AddBreakSection(int skillid, int breaktpye, int starttime, int endtime, bool isinterrupt)
        {
            SkillInfo cur_skill = m_Owner.GetSkillStateInfo().GetSkillInfoById(skillid);
            if(cur_skill != null)
            {
                cur_skill.AddBreakSection(breaktpye, starttime, endtime, isinterrupt);
            }
        }

        public override float GetLockInputTime(SkillNode node, SkillCategory next_category)
        {
            if(node == null)
            {
                return 0;
            }

            SkillInfo skill = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
            if(null == skill)
            {
                return 0;
            }
            return skill.StartTime + skill.GetLockInputTime(next_category);
        }

        public override float GetWaitInputTime(SkillNode node)
        {
            if (node == null)
            {
                return 0;
            }
            SkillInfo skill = m_Owner.GetSkillStateInfo().GetSkillInfoById(node.SkillId);
            if (skill == null)
            {
                return 0;
            }
            if (skill.IsInterrupted)
            {
                return 0;
            }
            return skill.StartTime + skill.ConfigData.NextInputTime;
        }

        private bool IsSkillInCD(SkillNode node)
        {
            if (node == null)
            {
                return false;
            }
            SkillNode head = GetHead(node.Category);
            if(node.SkillId != head.SkillId)
            {
                return false;
            }
            SkillInfo headInfo = m_Owner.GetSkillStateInfo().GetSkillInfoById(head.SkillId);
            return headInfo.IsInCd(TimeUtility.GetServerMilliseconds() / 1000.0f);
        }

        public override bool IsPlayerSelf()
        {
            if (m_Owner.IsUser && WorldSystem.Instance.PlayerSelfId == m_Owner.GetId())
            {
                return true;
            }
            return false;
        }

        private bool CanStartWhenImpactControl(SkillInfo skillinfo)
        {
            switch (m_Owner.GfxStateFlag)
            {
                case (int)GfxCharacterState_Type.Stiffness:
                    return skillinfo.ConfigData.CanStartWhenStiffness;
                case (int)GfxCharacterState_Type.HitFly:
                    return skillinfo.ConfigData.CanStartWhenHitFly;
                case (int)GfxCharacterState_Type.KnockDown:
                    return skillinfo.ConfigData.CanStartWhenKnockDown;
                default:
                    return false;
            }
        }


        // 强行开始技能
        public override bool ForceStartSkill(int skillId)
        {
            m_Owner.GetSkillStateInfo().SetCurSkillInfo(skillId);
            SkillInfo curSkill = m_Owner.GetSkillStateInfo().GetCurSkillInfo();
            if(null == curSkill)
            {
                LogSystem.Error("----------ForceStartSkillError: {0} set cur skill {1} failed! it's not exist", m_Owner.GetId(), skillId);
                return false;
            }
            if (curSkill.SkillId != skillId)
            {
                LogSystem.Debug("----------ForceStartSkillError: {0} set cur skill {1} failed! it's {2}", m_Owner.GetId(), skillId, curSkill.SkillId);
                return false;
            }

            if(null != curSkill)
            {
                curSkill.IsSkillActivated = true;
                curSkill.IsInterrupted = false;
                curSkill.StartTime = TimeUtility.GetServerMilliseconds() / 1000.0f;

                CharacterView view = EntityManager.Instance.GetCharacterViewById(m_Owner.GetId());
                if (view != null)
                {
                    GfxSystem.QueueGfxAction(m_GfxSkillSystem.StartSkill, view.Actor, skillId);
                }
            }
            SimulateStartSkill(m_Owner, skillId);
            return true;
        }

        //打断技能
        public override void ForceInterruptCurSkill()
        {
            //TODO 未实现
        }

        protected override void BeginSkillCategoryCD(SkillCategory category)
        {
            if (null != m_Owner && WorldSystem.Instance.PlayerSelfId == m_Owner.GetId())
            {
                SkillNode head = null;
                if (m_SkillCategoryDict.TryGetValue(category, out head))
                {
                    GfxSystem.PublishGfxEvent("ge_cast_skill_cd", "ui", GetCategoryName(head.Category), GetSkillCD(head));
                }
            }
        }
    }
}
