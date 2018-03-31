using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class MoveSectionInfo
  {
    public MoveSectionInfo Clone()
    {
      MoveSectionInfo copy = new MoveSectionInfo();
      copy.moveTime = moveTime;
      copy.speedVect = speedVect;
      copy.accelVect = accelVect;
      return copy;
    }

    public float moveTime;
    public Vector3 speedVect;
    public Vector3 accelVect;

    public float startTime = 0;
    public float lastUpdateTime = 0;
    public Vector3 curSpeedVect = Vector3.zero;
  }

  public class CurveMovementTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      CurveMovementTrigger copy = new CurveMovementTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_IsLockRotate = m_IsLockRotate;
      copy.m_SectionList.AddRange(m_SectionList);
      copy.m_IsCurveMoving = true;
      return copy;
    }

    public override void Reset()
    {
      m_IsCurveMoving = true;
      m_IsInited = false;
      GameObject.Destroy(m_StartTransform);
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() > 1) {
        m_StartTime = int.Parse(callData.GetParamId(0));
        m_IsLockRotate = bool.Parse(callData.GetParamId(1));
      }
      m_SectionList.Clear();
      int section_num = 0;
      while (callData.GetParamNum() >= 7 * (section_num + 1) + 2) {
        MoveSectionInfo section = new MoveSectionInfo();
        section.moveTime = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 2));
        section.speedVect.x = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 3));
        section.speedVect.y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 4));
        section.speedVect.z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 5));
        section.accelVect.x = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 6));
        section.accelVect.y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 7));
        section.accelVect.z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 8));
        m_SectionList.Add(section);
        section_num++;
      }
      if (m_SectionList.Count == 0) {
        return;
      }
      m_IsCurveMoving = true;
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
      if (!m_IsCurveMoving) {
        return false;
      }
      if (!m_IsInited) {
        Init(obj, instance);
      }
      if (m_SectionListCopy.Count == 0 || !instance.IsCurveMoveEnable) {
        m_IsCurveMoving = false;
        return false;
      }

      m_Now += TriggerUtil.ConvertToSecond((long)(instance.OrigDelta * instance.MoveScale));
      MoveSectionInfo cur_section = m_SectionListCopy[0];
      if (m_Now - cur_section.startTime > cur_section.moveTime) {
        float end_time = cur_section.startTime + cur_section.moveTime;
        float used_time = end_time - cur_section.lastUpdateTime;
        cur_section.curSpeedVect = Move(obj, cur_section.curSpeedVect, cur_section.accelVect, used_time);
        m_SectionListCopy.RemoveAt(0);
        if (m_SectionListCopy.Count > 0) {
          cur_section = m_SectionListCopy[0];
          cur_section.startTime = end_time;
          cur_section.lastUpdateTime = end_time;
          cur_section.curSpeedVect = cur_section.speedVect;
        } else {
          m_IsCurveMoving = false;
        }
      } else {
        cur_section.curSpeedVect = Move(obj, cur_section.curSpeedVect, cur_section.accelVect, m_Now - cur_section.lastUpdateTime);
        cur_section.lastUpdateTime = m_Now;
      }
      return true;
    }

    private void Init(GameObject obj, SkillInstance instance)
    {
      CopySectionList();
      CalNewSpeedWithTarget(obj, instance);
      m_Now = instance.CurTime / 1000.0f;
      m_SectionListCopy[0].startTime = m_Now;
      m_SectionListCopy[0].lastUpdateTime = m_Now;
      m_SectionListCopy[0].curSpeedVect = m_SectionListCopy[0].speedVect;
      m_StartTransform = new GameObject();
      m_StartTransform.transform.position = obj.transform.position;
      m_StartTransform.transform.rotation = obj.transform.rotation;
      m_IsInited = true;
    }

    private void CopySectionList()
    {
      m_SectionListCopy.Clear();
      foreach (MoveSectionInfo sect in m_SectionList) {
        m_SectionListCopy.Add(sect.Clone());
      }
    }

    private void CalNewSpeedWithTarget(GameObject obj, SkillInstance instance)
    {
      MoveTargetInfo ss = instance.CustomDatas.GetData<MoveTargetInfo>();
      if (ss == null) {
        return;
      }
      GameObject target = ss.Target;
      if (target == null) {
        return;
      }
      if (!ss.IsAdjustMove) {
        return;
      }
      GfxSkillSystem.ChangeDir(obj, (target.transform.position - obj.transform.position));
      float cur_distance_z = 0;
      foreach (MoveSectionInfo section in m_SectionListCopy) {
        cur_distance_z += (section.speedVect.z * section.moveTime +
                           section.accelVect.z * section.moveTime * section.moveTime / 2.0f);
      }
      Vector3 target_motion = (target.transform.position - obj.transform.position);
      target_motion.y = 0;
      float target_distance_z = target_motion.magnitude;
      target_distance_z = target_distance_z * (1 + ss.ToTargetDistanceRatio) + ss.ToTargetConstDistance;
      float speed_ratio = 1;
      if (cur_distance_z != 0) {
        speed_ratio = target_distance_z / cur_distance_z;
      }
      foreach (MoveSectionInfo section in m_SectionListCopy) {
        section.speedVect.z *= speed_ratio;
        section.accelVect.z *= speed_ratio;
      }
      instance.CustomDatas.RemoveData<MoveTargetInfo>();
    }

    private Vector3 Move(GameObject obj, Vector3 speed_vect, Vector3 accel_vect, float time)
    {
      m_StartTransform.transform.position = obj.transform.position;
      if (!m_IsLockRotate) {
        m_StartTransform.transform.rotation = obj.transform.rotation;
      }
      Vector3 local_motion = speed_vect * time + accel_vect * time * time / 2;
      Vector3 word_target_pos;
      while(local_motion.magnitude > m_MaxMoveStep) {
        Vector3 child = Vector3.ClampMagnitude(local_motion, m_MaxMoveStep);
        local_motion = local_motion - child;
        word_target_pos = m_StartTransform.transform.TransformPoint(child);
        TriggerUtil.MoveObjTo(obj, word_target_pos);
        m_StartTransform.transform.position = obj.transform.position;
      }
      word_target_pos = m_StartTransform.transform.TransformPoint(local_motion);
      TriggerUtil.MoveObjTo(obj, word_target_pos);
      TriggerUtil.UpdateObjPosition(obj);
      return (speed_vect + accel_vect * time);
    }

    private bool m_IsLockRotate = true;
    private List<MoveSectionInfo> m_SectionList = new List<MoveSectionInfo>();
    private List<MoveSectionInfo> m_SectionListCopy = new List<MoveSectionInfo>();
    private bool m_IsCurveMoving = false;

    private GameObject m_StartTransform = null;
    private bool m_IsInited = false;
    private float m_Now;
    private static float m_MaxMoveStep = 3;
  }

  /// <summary>
  /// charge(duration,velocity[,start_time]);
  /// </summary>
  internal class ChargeTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      ChargeTriger triger = new ChargeTriger();
      triger.m_Duration = m_Duration;
      triger.m_Velocity = m_Velocity;
      triger.m_StartTime = m_StartTime;
      return triger;
    }
    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj) {
        if (curSectionTime <= m_Duration) {
          float dist = (float)(int)delta / 1000.0f * m_Velocity;
          Vector3 targetPos = obj.transform.position + obj.transform.forward * dist;
          CharacterController ctrl = obj.GetComponent<CharacterController>();
          if (null != ctrl) {
            ctrl.Move(obj.transform.forward * dist);
          } else {
            obj.transform.position = targetPos;
          }
          DashFire.LogicSystem.NotifyGfxUpdatePosition(obj, targetPos.x, targetPos.y, targetPos.z);
          return true;
        } else {
          return true;
        }
      } else {
        return false;
      }
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Duration = long.Parse(callData.GetParamId(0));
      }
      if (num > 1) {
        m_Velocity = float.Parse(callData.GetParamId(1));
      }
      if (num > 2) {
        m_StartTime = long.Parse(callData.GetParamId(2));
      }
    }

    private long m_Duration = 0;
    private float m_Velocity = 1;
  }
  /// <summary>
  /// jump(duration,height,velocity[,start_time]);
  /// </summary>
  internal class JumpTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      JumpTriger triger = new JumpTriger();
      triger.m_Duration = m_Duration;
      triger.m_Height = m_Height;
      triger.m_Velocity = m_Velocity;
      triger.m_StartTime = m_StartTime;
      triger.m_VelocityY = m_VelocityY;
      triger.m_G = m_G;
      return triger;
    }
    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj) {
        if (m_InitY < 0) {
          m_InitY = obj.transform.position.y;
        }
        if (curSectionTime <= m_Duration) {
          float t = (float)(int)(curSectionTime - m_StartTime) / 1000.0f;
          float disty = m_VelocityY * t - m_G * t * t / 2;
          float dist = (float)(int)delta / 1000.0f * m_Velocity;
          Vector3 targetPos = obj.transform.position + obj.transform.forward * dist;
          targetPos.y = m_InitY + disty;

          CharacterController ctrl = obj.GetComponent<CharacterController>();
          if (null != ctrl) {
            ctrl.Move(targetPos - obj.transform.position);
          } else {
            obj.transform.position = targetPos;
          }
          DashFire.LogicSystem.NotifyGfxUpdatePosition(obj, targetPos.x, targetPos.y, targetPos.z);
          return true;
        } else {
          return true;
        }
      } else {
        return false;
      }
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Duration = long.Parse(callData.GetParamId(0));
      } else {
        m_Duration = 0;
      }
      if (num > 1) {
        m_Height = float.Parse(callData.GetParamId(1));
      }
      if (num > 2) {
        m_Velocity = float.Parse(callData.GetParamId(2));
      }
      if (num > 3) {
        m_StartTime = long.Parse(callData.GetParamId(3));
      }

      float time_div = (float)(int)m_Duration / 1000.0f;
      m_VelocityY = m_Height * 2.0f / time_div;
      m_G = m_Height * 4.0f / (time_div * time_div);
    }

    private float m_Duration = 0;
    private float m_Height = 1;
    private float m_Velocity = 1;

    private float m_InitY = -1;
    private float m_VelocityY = 1;
    private float m_G = 10;
  }
}