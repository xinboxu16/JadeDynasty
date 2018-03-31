using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class AddImpactToSelfTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      AddImpactToSelfTrigger copy = new AddImpactToSelfTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_ImpactId = m_ImpactId;
      copy.m_RemainTime = m_RemainTime;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 2) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_ImpactId = int.Parse(callData.GetParamId(1));
      }
      if (callData.GetParamNum() >= 3) {
        m_RemainTime = long.Parse(callData.GetParamId(2));
      }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (curSectionTime < m_StartTime) {
        return true;
      }
      GameObject obj = sender as GameObject;
      if (obj == null) {
        return false;
      }
      DashFire.LogicSystem.NotifyGfxHitTarget(obj, m_ImpactId, obj, 0, instance.SkillId,
                                              (int)m_RemainTime, obj.transform.position,
                                              TriggerUtil.GetObjFaceDir(obj));
      return false;
    }

    private int m_ImpactId;
    private long m_RemainTime = -1;
  }

  public class AddImpactToTargetTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      AddImpactToTargetTrigger copy = new AddImpactToTargetTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_ImpactId = m_ImpactId;
      copy.m_RemainTime = m_RemainTime;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 2) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_ImpactId = int.Parse(callData.GetParamId(1));
      }
      if (callData.GetParamNum() >= 3) {
        m_RemainTime = long.Parse(callData.GetParamId(2));
      }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (curSectionTime < m_StartTime) {
        return true;
      }
      GameObject obj = sender as GameObject;
      if (obj == null) {
        return false;
      }
      MoveTargetInfo target_info = instance.CustomDatas.GetData<MoveTargetInfo>();
      if (target_info == null) {
        return false;
      }
      GameObject target_obj = target_info.Target;
      if (target_obj == null) {
        return false;
      }
      DashFire.LogicSystem.NotifyGfxHitTarget(obj, m_ImpactId, target_obj, 0, instance.SkillId,
                                              (int)m_RemainTime, obj.transform.position,
                                              TriggerUtil.GetObjFaceDir(obj));
      return false;
    }

    private int m_ImpactId;
    private long m_RemainTime = -1;
  }
}