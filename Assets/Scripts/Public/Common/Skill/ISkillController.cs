using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public enum SkillCannotCastType
    {
        kUnknow,
        kNotFindSkill,
        kOwnerDead,
        kCannotCtrol,
        kInCD,
        kCostNotEnough,
    }

    public class SkillNode
    {
        public int SkillId;
        public SkillCategory Category;
        public SkillNode SkillQ = null;
        public SkillNode SkillE = null;
        public SkillNode NextSkillNode = null;
        public float StartTime;
        public Vector3 TargetPos;
        public bool IsLocked = false;
        public bool IsCDChecked = false;
    }

    public delegate void SkillQECanInputHandler(float remaintime, List<SkillNode> skills);
    public delegate void SkillStartHandler();

    public interface ISkillController
    {
        void Init();
        void OnTick();
        void PushSkill(SkillCategory category, Vector3 targetpos);
        void ForceInterruptCurSkill();
        bool ForceStartSkill(int skillid);
        void AddBreakSkillTask();
        void CancelBreakSkillTask();
    }
}
