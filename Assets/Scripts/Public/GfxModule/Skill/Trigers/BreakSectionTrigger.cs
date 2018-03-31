using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class BreakSectionTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      BreakSectionTrigger copy = new BreakSectionTrigger();
      copy.m_BreakType = this.m_BreakType;
      copy.m_StartTime = this.m_StartTime;
      copy.m_EndTime = this.m_EndTime;
      copy.m_IsInterrupt = this.m_IsInterrupt;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() > 0) {
        m_BreakType = int.Parse(callData.GetParamId(0));
      }
      if (callData.GetParamNum() > 1) {
        m_StartTime = int.Parse(callData.GetParamId(1));
      }
      if (callData.GetParamNum() > 2) {
        m_EndTime = long.Parse(callData.GetParamId(2));
      }
      if (callData.GetParamNum() > 3) {
        m_IsInterrupt = bool.Parse(callData.GetParamId(3));
      }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (obj == null) {
        return false;
      }
      long start_time = m_StartTime + instance.CurTime;
      long end_time = m_EndTime + instance.CurTime;
      DashFire.LogicSystem.NotifyGfxSkillBreakSection(obj, instance.SkillId, m_BreakType, (int)start_time, (int)end_time, m_IsInterrupt);
      return false;
    }

    private int m_BreakType = 0;
    private long m_EndTime = 0;
    private bool m_IsInterrupt = true;
  }
}
