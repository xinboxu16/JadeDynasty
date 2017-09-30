using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class SkillController : ISkillController
    {
        public static List<int> m_UnlockedSkills = new List<int>();
        protected Dictionary<SkillCategory, SkillNode> m_SkillCategoryDict = new Dictionary<SkillCategory, SkillNode>();
        protected bool m_IsAttacking = false;
        protected List<SkillNode> m_WaiteSkillBuffer = new List<SkillNode>();
        protected SkillNode m_LastSkillNode = null;
        protected SkillNode m_CurSkillNode = null;
        protected SkillQECanInputHandler m_SkillQECanInputHandler = null;
        protected SkillStartHandler m_SkillStartHandler = null;
        protected bool m_IsHaveBreakSkillTask = false;

        public virtual void Init()
        {
        }

        public virtual void PushSkill(SkillCategory category, Vector3 targetpos)
        {
            
        }

        public virtual void OnTick()
        {

        }

        public virtual void ForceInterruptCurSkill()
        {
        }

        public virtual bool ForceStartSkill(int skillid)
        {
            return true;
        }

        public virtual void AddBreakSkillTask()
        {

        }

        public void CancelBreakSkillTask()
        {
            
        }
    }
}
