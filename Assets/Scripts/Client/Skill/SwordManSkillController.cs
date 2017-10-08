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
    }
}
