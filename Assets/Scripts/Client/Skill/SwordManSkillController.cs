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
            foreach(SkillLogicData ss in skills)
            {
                if(IsCategoryContain(ss.SkillId))
                {
                    continue;
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

        private SkillNode FindSkillNodeInChildren(SkillNode head, int targetId)
        {
            if(head.SkillId == targetId)
            {
                return head;
            }
            if (null != head.NextSkillNode)
            {
                SkillNode node = FindSkillNodeInChildren(head.NextSkillNode, targetId);
                if(null != node)
                {
                    return node;
                }
            }
            if(null != head.SkillQ)
            {
                SkillNode node = FindSkillNodeInChildren(head.SkillQ, targetId);
                if(null != node)
                {
                    return node;
                }
            }
            if (head.SkillE != null)
            {
                SkillNode node = FindSkillNodeInChildren(head.SkillE, targetId);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
