using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class SetEnableTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      SetEnableTrigger copy = new SetEnableTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_AttributeName = m_AttributeName;
      copy.m_IsEnable = m_IsEnable;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 3) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_AttributeName = callData.GetParamId(1);
        m_IsEnable = bool.Parse(callData.GetParamId(2));
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
      switch (m_AttributeName) {
        case "CurveMove":
          instance.IsCurveMoveEnable = m_IsEnable;
          break;
        case "Rotate":
          instance.IsRotateEnable = m_IsEnable;
          break;
        case "Damage":
          instance.IsDamageEnable = m_IsEnable;
          break;
        case "Visible":
          TriggerUtil.SetObjVisible(obj, m_IsEnable);
          break;
        case "CameraFollow":
          TriggerUtil.SetFollowEnable(m_IsEnable);
          break;
      }
      return false;
    }

    private string m_AttributeName;
    private bool m_IsEnable;
  }
}
