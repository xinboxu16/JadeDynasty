using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class ShakeCamera2Trigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      ShakeCamera2Trigger copy = new ShakeCamera2Trigger();
      copy.m_StartTime = m_StartTime;
      copy.m_RemainTime = m_RemainTime;
      if (m_XShakeInfo != null && m_YShakeInfo != null && m_ZShakeInfo != null) {
        copy.m_XShakeInfo = m_XShakeInfo.Clone();
        copy.m_YShakeInfo = m_YShakeInfo.Clone();
        copy.m_ZShakeInfo = m_ZShakeInfo.Clone();
      }
      copy.m_IsFollow = m_IsFollow;
      copy.m_IsNeedCollide = m_IsNeedCollide;
      copy.m_IsRelativeUser = m_IsRelativeUser;
      return copy;
    }

    public override void Reset()
    {
      m_IsInited = false;
      if (TriggerUtil.IsControledCamera(m_CameraControlId)) {
        TriggerUtil.ControlCamera(false);
      }
      m_CameraControlId = TriggerUtil.CAMERA_CONTROL_FAILED;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 8) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_RemainTime = long.Parse(callData.GetParamId(1));
        m_IsNeedCollide = bool.Parse(callData.GetParamId(2));
        m_IsRelativeUser = bool.Parse(callData.GetParamId(3));
        ScriptableData.CallData vect_param1 = callData.GetParam(4) as ScriptableData.CallData;
        ScriptableData.CallData vect_param2 = callData.GetParam(5) as ScriptableData.CallData;
        ScriptableData.CallData vect_param3 = callData.GetParam(6) as ScriptableData.CallData;
        ScriptableData.CallData vect_param4 = callData.GetParam(7) as ScriptableData.CallData;
        Vector3 amplitude, perShakeTime, shakeStartSpeed, amplitudeDecayPercent;
        if (null == vect_param1 || null == vect_param2 || null == vect_param3 || null == vect_param4) {
          return;
        }
        amplitude = ScriptableDataUtility.CalcVector3(vect_param1);
        perShakeTime = ScriptableDataUtility.CalcVector3(vect_param2);
        shakeStartSpeed = ScriptableDataUtility.CalcVector3(vect_param3);
        amplitudeDecayPercent = ScriptableDataUtility.CalcVector3(vect_param4);

        m_XShakeInfo = new AxisShaker(m_RemainTime, amplitude.x, perShakeTime.x / 1000.0f, shakeStartSpeed.x, amplitudeDecayPercent.x);
        m_YShakeInfo = new AxisShaker(m_RemainTime, amplitude.y, perShakeTime.y / 1000.0f, shakeStartSpeed.y, amplitudeDecayPercent.y);
        m_ZShakeInfo = new AxisShaker(m_RemainTime, amplitude.z, perShakeTime.z / 1000.0f, shakeStartSpeed.z, amplitudeDecayPercent.z);
      }
      if (callData.GetParamNum() >= 9) {
        m_IsFollow = bool.Parse(callData.GetParamId(8));
      }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (curSectionTime < m_StartTime) {
        return true;
      }
      if (curSectionTime > m_StartTime + m_RemainTime) {
        if (TriggerUtil.IsControledCamera(m_CameraControlId)) {
          TriggerUtil.ControlCamera(false);
        }
        return false;
      }

      GameObject obj = sender as GameObject;
      if (obj == null) {
        return false;
      }

      if (!m_IsInited) {
        if (!Init(instance)) {
          return false;
        }
      }
      if (!TriggerUtil.IsControledCamera(m_CameraControlId)) {
        return false;
      }
      float now = instance.CurTime / 1000.0f;
      m_XShakeInfo.Tick(now);
      m_YShakeInfo.Tick(now);
      m_ZShakeInfo.Tick(now);
      Vector3 new_pos = new Vector3(m_XShakeInfo.CurPos, m_YShakeInfo.CurPos, m_ZShakeInfo.CurPos);
      if (m_IsRelativeUser) {
        new_pos = new_pos - m_CameraOriginPos;
        new_pos = obj.transform.TransformPoint(new_pos);
        new_pos = new_pos - obj.transform.position;
        new_pos = m_CameraOriginPos + new_pos;
      }
      if (m_IsFollow) {
        Camera.main.transform.position = m_LastCameraFollowPos;
        if (m_CameraObject != null) {
          m_CameraObject.SendMessage("Apply");
        }
        m_LastCameraFollowPos = Camera.main.transform.position;
        new_pos = Camera.main.transform.position + (new_pos - m_CameraOriginPos);
      }
      Camera.main.transform.position = new_pos;
      return true;
    }

    private bool Init(SkillInstance instance)
    {
      if (m_IsNeedCollide) {
        SkillDamageManager damage_manager = instance.CustomDatas.GetData<SkillDamageManager>();
        if (damage_manager == null || !damage_manager.IsDamagedEnemy) {
          return false;
        }
      }
      if (m_XShakeInfo == null || m_YShakeInfo == null || m_ZShakeInfo == null) {
        return false;
      }
      m_IsInited = true;
      m_CameraOriginPos = Camera.main.transform.position;
      m_LastCameraFollowPos = m_CameraOriginPos;
      m_CameraControlId = TriggerUtil.ControlCamera(true);
      if (m_CameraControlId == TriggerUtil.CAMERA_CONTROL_FAILED) {
        return false;
      }
      float now = instance.CurTime / 1000.0f;
      m_XShakeInfo.Init(m_CameraOriginPos.x, now);
      m_YShakeInfo.Init(m_CameraOriginPos.y, now);
      m_ZShakeInfo.Init(m_CameraOriginPos.z, now);
      m_CameraObject = TriggerUtil.GetCameraObj();
      return true;
    }
 
    private long m_RemainTime = 0;
    public bool m_IsNeedCollide;
    public bool m_IsRelativeUser;
    private bool m_IsFollow = true;
    private AxisShaker m_XShakeInfo;
    private AxisShaker m_YShakeInfo;
    private AxisShaker m_ZShakeInfo;

    private bool m_IsInited = false;
    private Vector3 m_CameraOriginPos;
    private Vector3 m_LastCameraFollowPos;
    private GameObject m_CameraObject;
    private int m_CameraControlId = TriggerUtil.CAMERA_CONTROL_FAILED;
  }

  public class AxisShaker
  {
    public AxisShaker Clone()
    {
      AxisShaker copy = new AxisShaker(RemainTime, Amplitude, PerShakeTime, ShakeStartSpeed, AmplitudeDecayPercent);
      return copy;
    }

    public AxisShaker(float remaintime, float amplitude, float pershaketime, float speed, float percent)
    {
      RemainTime = remaintime;
      Amplitude = amplitude;
      PerShakeTime = pershaketime;
      ShakeStartSpeed = speed;
      AmplitudeDecayPercent = percent;
    }

    public void Init(float startpos, float now) {
      StartPos = startpos;
      StartTime = now;
      m_CurAmplitude = Amplitude;
      CurPos = StartPos;
      InitShakeStartInfo(now);
    }

    public void InitShakeStartInfo(float now)
    {
      if (m_CurAmplitude == 0) {
        m_IsOver = true;
      }
      CalcStartPos = StartPos;
      CalcSpeed = ShakeStartSpeed;
      CalcAccelSpeed = ComputeAccelSpeed(m_CurAmplitude * (ShakeStartSpeed / Math.Abs(ShakeStartSpeed)) / 2.0f, PerShakeTime / 4.0f, ShakeStartSpeed);
      CalcTime = now;
      m_NextSpeedRevertTime = now + PerShakeTime / 4.0f;
      m_NextAccelRevertTime = now + PerShakeTime / 2.0f;
      m_NextDecayTime = now + PerShakeTime;
      //Debug.Log("--------shake starttime=" + StartTime + " speed=" + ShakeStartSpeed + " nextRevertTime="
      //          + m_NextSpeedRevertTime + " nextAccelRevertTime=" + m_NextAccelRevertTime);
    }

    public void Reset()
    {
      m_IsOver = false;
    }

    public void Tick(float now)
    {
      if (m_IsOver) {
        return;
      }

      if (now > StartTime + RemainTime) {
        now = StartTime + RemainTime;
        m_IsOver = true;
      }
      float deltaTime = now - CalcTime;
      if (deltaTime > PerShakeTime / 4) {
        deltaTime = PerShakeTime / 4;
      }
      CurPos = CalcStartPos + CalcSpeed * deltaTime + CalcAccelSpeed * deltaTime * deltaTime / 2;

      if (now >= m_NextDecayTime) {
        Decay();
        InitShakeStartInfo(m_NextDecayTime);
      } else if (now >= m_NextSpeedRevertTime || now >= m_NextAccelRevertTime) {
        if (m_NextAccelRevertTime < m_NextSpeedRevertTime) {
          UpdateCalcInfo(m_NextAccelRevertTime);
          CalcAccelSpeed = -CalcAccelSpeed;
          m_NextAccelRevertTime += PerShakeTime / 2;
        } else {
          UpdateCalcInfo(m_NextSpeedRevertTime);
          CalcSpeed = -CalcSpeed;
          m_NextSpeedRevertTime += PerShakeTime / 2;
        }
      }
      //Debug.Log("------now=" + now + " curpos=" + CurPos + "  startpos=" + CalcStartPos + " speed=" + CalcSpeed + " accel=" + CalcAccelSpeed);
    }

    private void Decay()
    {
      float temp = m_CurAmplitude * AmplitudeDecayPercent / 100.0f;
      m_CurAmplitude = temp;
      if (Math.Abs(m_CurAmplitude) <= 0.0001) {
        m_IsOver = true;
      }
    }

    private void UpdateCalcInfo(float now) {
      float move_time = now - CalcTime;
      CalcStartPos = CalcStartPos + CalcSpeed * move_time + CalcAccelSpeed * move_time * move_time / 2;
      CalcSpeed = CalcSpeed + move_time * CalcAccelSpeed;
      CalcTime = now;
    }

    private float ComputeAccelSpeed(float distance, float seconds, float speed)
    {
      float accel = 0;
      if (seconds * seconds <= 0.0000001) {
        return accel;
      }
      accel = 2 * (distance - speed * seconds) / (seconds * seconds);
      return accel;
    }

    public float RemainTime;
    public float Amplitude;
    public float PerShakeTime;
    public float ShakeStartSpeed;
    public float AmplitudeDecayPercent;

    public float CurPos;
    public float StartPos;
    public float StartTime;

    private float m_CurAmplitude;
    public float CalcStartPos;
    public float CalcSpeed;
    public float CalcAccelSpeed;
    public float CalcTime;

    private float m_NextSpeedRevertTime;
    private float m_NextAccelRevertTime;
    private float m_NextDecayTime;
    private bool m_IsOver = false;

  }

  public class SetCameraFollowSpeed : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      SetCameraFollowSpeed copy = new SetCameraFollowSpeed();
      copy.m_StartTime = m_StartTime;
      copy.m_MaxDistance = m_MaxDistance;
      copy.m_MinDistance = m_MinDistance;
      copy.m_MaxSpeed = m_MaxSpeed;
      copy.m_MinSpeed = m_MinSpeed;
      copy.m_Power = m_Power;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 6) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_MaxDistance = float.Parse(callData.GetParamId(1));
        m_MinDistance = float.Parse(callData.GetParamId(2));
        m_MaxSpeed = float.Parse(callData.GetParamId(3));
        m_MinSpeed = float.Parse(callData.GetParamId(4));
        m_Power = int.Parse(callData.GetParamId(5));
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

      if (!TriggerUtil.IsPlayerSelf(obj)) {
        return false;
      }
      GameObject camera_obj = TriggerUtil.GetCameraObj();
      if (camera_obj != null) {
        string str_param = "" + m_MaxDistance + "|" + m_MinDistance + "|" +
          m_MaxSpeed + "|" + m_MinSpeed + "|" + m_Power;
        camera_obj.SendMessage("SetFollowSpeedByStr", str_param);
      }
      return false;
    }

    private float m_MaxDistance;
    private float m_MinDistance;
    private float m_MaxSpeed;
    private float m_MinSpeed;
    private int m_Power;
  }

  public class ResetCameraFollowSpeed : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      ResetCameraFollowSpeed copy = new ResetCameraFollowSpeed();
      copy.m_StartTime = m_StartTime;
      return copy;
    }

    public override void Reset()
    {
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      if (callData.GetParamNum() >= 1) {
        m_StartTime = long.Parse(callData.GetParamId(0));
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
      if (!TriggerUtil.IsPlayerSelf(obj)) {
        return false;
      }
      GameObject camera_obj = TriggerUtil.GetCameraObj();
      if (camera_obj != null) {
        camera_obj.SendMessage("ResetFollowSpeed");
      }
      return false;
    }
  }
}
