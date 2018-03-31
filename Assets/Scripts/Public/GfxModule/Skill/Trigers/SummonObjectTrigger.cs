using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class SummonObjectTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      SummonObjectTrigger copy = new SummonObjectTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_NpcTypeId = m_NpcTypeId;
      copy.m_ModelPrefab = m_ModelPrefab;
      copy.m_SkillId = m_SkillId;
      copy.m_LocalPostion = m_LocalPostion;
      copy.m_LocalRotate = m_LocalRotate;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 4) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_NpcTypeId = int.Parse(callData.GetParamId(1));
        m_ModelPrefab = callData.GetParamId(2);
        if (m_ModelPrefab == " ") {
          m_ModelPrefab = "";
        }
        m_SkillId = int.Parse(callData.GetParamId(3));
      }
      if (callData.GetParamNum() >= 5) {
        m_LocalPostion = ScriptableDataUtility.CalcVector3(callData.GetParam(4) as ScriptableData.CallData);
      }
      if (callData.GetParamNum() >= 6) {
        m_LocalRotate = ScriptableDataUtility.CalcVector3(callData.GetParam(5) as ScriptableData.CallData);
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
      Vector3 position = obj.transform.TransformPoint(m_LocalPostion);
      DashFire.LogicSystem.NotifyGfxSummonNpc(obj, instance.SkillId, m_NpcTypeId, m_ModelPrefab, m_SkillId,
                                              position.x, position.y, position.z);
      return false;
    }

    private int m_NpcTypeId;
    private string m_ModelPrefab;
    private int m_SkillId;
    private Vector3 m_LocalPostion;
    private Vector3 m_LocalRotate;
  }
}
