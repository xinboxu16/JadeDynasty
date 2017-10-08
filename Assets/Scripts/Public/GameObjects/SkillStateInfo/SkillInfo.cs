using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum SlotPosition:int
    {
        SP_None = 0,
        SP_A,
        SP_B,
        SP_C,
        SP_D,
    }

    public class BreakSection
    {
        public BreakSection(int breaktype, int starttime, int endtime, bool isinterrupt)
        {
            BreakType = breaktype;
            StartTime = starttime;
            EndTime = endtime;
            IsInterrupt = isinterrupt;
        }
        public int BreakType;
        public int StartTime;
        public int EndTime;
        public bool IsInterrupt;
    }

    public class PresetInfo
    {
        public const int PresetNum = 4;
        public SlotPosition[] Presets = new SlotPosition[PresetNum];

        public PresetInfo()
        {
            for (int i = 0; i < PresetNum; i++)
            {
                Presets[i] = SlotPosition.SP_None;
            }
        }

        public void SetCurSkillSlotPos(int preset, SlotPosition pos)
        {
            if (preset >= 0 && preset < PresetNum)
            {
                Presets[preset] = pos;
            }
        }
    }
    public class SkillInfo
    {
        public int SkillId;                // 技能Id
        public int SkillLevel;             // 技能等级
        public bool IsSkillActivated;      // 是否正在释放技能    
        public bool IsItemSkill;
        public bool IsMarkToRemove;
        public PresetInfo Postions;         // 技能挂载位置信息
        public SkillLogicData ConfigData = null;

        public float StartTime;
        public bool IsInterrupted;

        private float m_CDEndTime;

        private List<BreakSection> BreakSections = new List<BreakSection>();
        private MyDictionary<SkillCategory, float> m_CategoryLockinputTime = new MyDictionary<SkillCategory, float>();

        public SkillInfo (int skillId, int level = 0)
        {
            SkillId = skillId;
            SkillLevel = level;
            IsSkillActivated = false;
            IsItemSkill = false;
            IsMarkToRemove = false;
            IsInterrupted = false;
            Postions = new PresetInfo();
            ConfigData = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
        }

        public void BeginCD()
        {
            m_CDEndTime = StartTime + ConfigData.CoolDownTime;
        }

        //当前时间
        public bool IsInCd(float now)
        {
            if(now < m_CDEndTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //是否能打断
        public bool CanBreak(int brealType, long time, out bool isInterrupt)
        {
            isInterrupt = false;
            if (!IsSkillActivated)
                return true;

            foreach(BreakSection section in BreakSections)
            {
                if(section.BreakType == brealType && (StartTime * 1000 + section.StartTime) <= time && time <= (StartTime * 1000 + section.EndTime))
                {
                    isInterrupt = section.IsInterrupt;
                    return true;
                }
            }
            return false;
        }

        public float GetLockInputTime(SkillCategory category)
        {
            if(m_CategoryLockinputTime.ContainsKey(category))
            {
                return m_CategoryLockinputTime[category];
            }
            else
            {
                return ConfigData.LockInputTime;
            }
        }
    }
}
