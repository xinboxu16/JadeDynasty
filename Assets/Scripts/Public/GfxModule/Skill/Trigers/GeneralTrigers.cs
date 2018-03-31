using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  /// <summary>
  /// movecontrol(is_skill_control_move);
  /// </summary>
  internal class MoveControlTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      MoveControlTriger triger = new MoveControlTriger();
      triger.m_IsControlMove = m_IsControlMove;
      return triger;
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj) {
        if (m_IsControlMove)
          DashFire.LogicSystem.NotifyGfxMoveControlStart(obj, instance.SkillId, true);
        else
          DashFire.LogicSystem.NotifyGfxMoveControlFinish(obj, instance.SkillId, true);
        instance.IsControlMove = m_IsControlMove;//更改移动状态

        //DashFire.LogSystem.Debug("movecontrol:{0}", m_IsControlMove);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_IsControlMove = (callData.GetParamId(0) == "true");
      }
    }

    private bool m_IsControlMove = false;
  }

  /// <summary>
  /// timescale(start_time, scale[, end_time]);
  /// </summary>
  internal class TimeScaleTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      TimeScaleTriger triger = new TimeScaleTriger();
      triger.m_TimeScale = m_TimeScale;
      triger.m_StartTime = m_StartTime;
      triger.m_EndTime = m_EndTime;
      triger.m_FixedDeltaTime = m_FixedDeltaTime;
      return triger;
    }

    public override void Reset()
    {
      if (m_IsSet && !m_IsReset) {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = m_FixedDeltaTime;
      }
      m_IsSet = false;
      m_IsReset = false;
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (curSectionTime >= m_StartTime && curSectionTime < m_EndTime) {
        if (!m_IsSet) {
          m_IsSet = true;

          Time.timeScale = m_TimeScale;
          Time.fixedDeltaTime = m_FixedDeltaTime * m_TimeScale;

          //DashFire.LogSystem.Debug("timescale:{0} {1} {2}", m_TimeScale, m_StartTime, m_EndTime);
        }
      }
      if (curSectionTime >= m_EndTime) {
        if (!m_IsReset) {
          m_IsReset = true;

          Time.timeScale = 1.0f;
          Time.fixedDeltaTime = m_FixedDeltaTime;

          //DashFire.LogSystem.Debug("timescale reset");
        }
        return false;
      } else {
        return true;
      }
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() > 0) {
        m_StartTime = long.Parse(callData.GetParamId(0));
      }
      if (callData.GetParamNum() > 1) {
        m_TimeScale = float.Parse(callData.GetParamId(1));
      }
      if (callData.GetParamNum() > 2) {
        m_EndTime = long.Parse(callData.GetParamId(2));
      }
      m_FixedDeltaTime = Time.fixedDeltaTime;
    }

    private float m_TimeScale = 1.0f;
    private float m_EndTime = 0;
    private float m_FixedDeltaTime = 0.1f;

    private bool m_IsSet = false;
    private bool m_IsReset = false;
  }

  class MoveChildTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      MoveChildTrigger copy = new MoveChildTrigger();
      copy.m_StartTime = this.m_StartTime;
      copy.m_ChildName = this.m_ChildName;
      copy.m_NodeName = this.m_NodeName;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() > 0) {
        m_StartTime = long.Parse(callData.GetParamId(0));
      }
      if (callData.GetParamNum() > 1) {
        m_ChildName = callData.GetParamId(1);
      }
      if (callData.GetParamNum() > 2) {
        m_NodeName = callData.GetParamId(2);
      }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (m_StartTime <= curSectionTime) {
        GameObject obj = sender as GameObject;
        if (obj == null) {
          return false;
        }
        TriggerUtil.MoveChildToNode(obj, m_ChildName, m_NodeName);
        return false;
      }
      return true;
    }

    private string m_ChildName = "";
    private string m_NodeName = "";
  }
}
