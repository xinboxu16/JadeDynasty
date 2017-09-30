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
    }
}
