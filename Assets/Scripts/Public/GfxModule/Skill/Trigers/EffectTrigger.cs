using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class EffectManager
  {
    public void AddEffect(GameObject obj)
    {
      m_EffectObject.Add(obj);
    }

    public void SetParticleSpeed(float speed)
    {
      foreach (GameObject obj in m_EffectObject) {
        if (obj.activeSelf) {
          ParticleSystem[] ps = obj.GetComponentsInChildren<ParticleSystem>();
          foreach (ParticleSystem p in ps) {
              var main = p.main;
              main.simulationSpeed = speed;//特效播放速度
          }
        }
      }
    }

    public void StopEffects()
    {
      foreach (GameObject effect in m_EffectObject) {
        DashFire.ResourceSystem.RecycleObject(effect);
      }
      m_EffectObject.Clear();
    }
    private List<GameObject> m_EffectObject = new List<GameObject>();
  }
  /// <summary>
  /// charactereffect(effect_path,delete_time,attach_bone[,start_time]);
  /// 
  /// or
  /// 
  /// charactereffect(effect_path,delete_time,attach_bone[,start_time])
  /// {
  ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
  /// };
  /// </summary>
  internal class CharacterEffectTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      CharacterEffectTriger triger = new CharacterEffectTriger();
      triger.m_EffectPath = m_EffectPath;
      triger.m_AttachPath = m_AttachPath;
      triger.m_DeleteTime = m_DeleteTime;
      triger.m_StartTime = m_StartTime;
      triger.m_IsAttach = m_IsAttach;
      triger.m_Pos = m_Pos;
      triger.m_Dir = m_Dir;
      triger.m_Scale = m_Scale;
      return triger;
    }
    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj) {
        if (curSectionTime >= m_StartTime) {
          GameObject effectObj = DashFire.ResourceSystem.NewObject(m_EffectPath, m_DeleteTime) as GameObject;
          if (null != effectObj) {
            TriggerUtil.SetObjVisible(effectObj, true);
            Transform bone = DashFire.LogicSystem.FindChildRecursive(obj.transform, m_AttachPath);
            if (null != bone) {
              effectObj.transform.parent = bone;
              effectObj.transform.localPosition = m_Pos;
              effectObj.transform.localRotation = m_Dir;
              effectObj.transform.localScale = m_Scale;
              if (!m_IsAttach) {
                effectObj.transform.parent = null;
              }
              EffectManager em = instance.CustomDatas.GetData<EffectManager>();
              if (em == null) {
                em = new EffectManager();
                instance.CustomDatas.AddData<EffectManager>(em);
              }
              em.AddEffect(effectObj);
              em.SetParticleSpeed(instance.EffectScale);
            }
          }

          //DashFire.LogSystem.Debug("CharacterEffectTriger:{0}", m_EffectPath);
          return false;
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
        m_EffectPath = callData.GetParamId(0);
      }
      if (num > 1) {
        m_DeleteTime = float.Parse(callData.GetParamId(1)) / 1000.0f;
      }
      if (num > 2) {
        m_AttachPath = callData.GetParamId(2);
      }
      if (num > 3) {
        m_StartTime = long.Parse(callData.GetParamId(3));
      }
      if (num > 4) {
        m_IsAttach = bool.Parse(callData.GetParamId(4));
      }
    }

    protected override void Load(ScriptableData.FunctionData funcData)
    {
      ScriptableData.CallData callData = funcData.Call;
      if (null != callData) {
        Load(callData);

        ScriptableData.ISyntaxComponent statement = funcData.Statements.Find(st => st.GetId() == "transform");
        if (null != statement) {
          ScriptableData.CallData stCall = statement as ScriptableData.CallData;
          if (null != stCall) {
            if (stCall.GetParamNum() > 0) {
              ScriptableData.CallData param0 = stCall.GetParam(0) as ScriptableData.CallData;
              if (null != param0)
                m_Pos = ScriptableDataUtility.CalcVector3(param0);
            }
            if (stCall.GetParamNum() > 1) {
              ScriptableData.CallData param1 = stCall.GetParam(1) as ScriptableData.CallData;
              if (null != param1)
                m_Dir = ScriptableDataUtility.CalcEularRotation(param1);
            }
            if (stCall.GetParamNum() > 2) {
              ScriptableData.CallData param2 = stCall.GetParam(2) as ScriptableData.CallData;
              if (null != param2)
                m_Scale = ScriptableDataUtility.CalcVector3(param2);
            }
          }
        }
      }
    }

    private string m_EffectPath = "";
    private string m_AttachPath = "";
    private float m_DeleteTime = 0;
    private bool m_IsAttach = true;

    private Vector3 m_Pos = Vector3.zero;
    private Quaternion m_Dir = Quaternion.identity;
    private Vector3 m_Scale = Vector3.one;
  }
  /// <summary>
  /// sceneeffect(effect_path,delete_time[,vector3(x,y,z)[,start_time[,eular(rx,ry,rz)[,vector3(sx,sy,sz)]]]]);
  /// </summary>
  internal class SceneEffectTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      SceneEffectTriger triger = new SceneEffectTriger();
      triger.m_EffectPath = m_EffectPath;
      triger.m_Pos = m_Pos;
      triger.m_Dir = m_Dir;
      triger.m_Scale = m_Scale;
      triger.m_DeleteTime = m_DeleteTime;
      triger.m_StartTime = m_StartTime;
      triger.m_IsRotateRelativeUser = m_IsRotateRelativeUser;
      return triger;
    }
    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj) {
        if (curSectionTime >= m_StartTime) {
          GameObject effectObj = DashFire.ResourceSystem.NewObject(m_EffectPath, m_DeleteTime) as GameObject;
          if (null != effectObj) {
            TriggerUtil.SetObjVisible(effectObj, true);
            Vector3 pos = obj.transform.position + obj.transform.localRotation * m_Pos;
            effectObj.transform.position = pos;
            effectObj.transform.localScale = m_Scale;
            if (m_IsRotateRelativeUser) {
              effectObj.transform.parent = obj.transform;
              effectObj.transform.localRotation = m_Dir;
              effectObj.transform.parent = null;
            } else {
              effectObj.transform.localRotation = m_Dir;
            }
            EffectManager em = instance.CustomDatas.GetData<EffectManager>();
            if (em == null) {
              em = new EffectManager();
              instance.CustomDatas.AddData<EffectManager>(em);
            }
            em.AddEffect(effectObj);
            em.SetParticleSpeed(instance.EffectScale);
          }
          return false;
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
        m_EffectPath = callData.GetParamId(0);
      }
      if (num > 1) {
        m_DeleteTime = float.Parse(callData.GetParamId(1)) / 1000.0f;
      }
      if (num > 2) {
        m_Pos = ScriptableDataUtility.CalcVector3(callData.GetParam(2) as ScriptableData.CallData);
      }
      if (num > 3) {
        m_StartTime = long.Parse(callData.GetParamId(3));
      }
      if (num > 4) {
        m_Dir = ScriptableDataUtility.CalcEularRotation(callData.GetParam(4) as ScriptableData.CallData);
      }
      if (num > 5) {
        m_Scale = ScriptableDataUtility.CalcVector3(callData.GetParam(5) as ScriptableData.CallData);
      }
      if (num > 6) {
        m_IsRotateRelativeUser = bool.Parse(callData.GetParamId(6));
      }
    }

    private string m_EffectPath = "";
    private Vector3 m_Pos = Vector3.zero;
    private Quaternion m_Dir = Quaternion.identity;
    private Vector3 m_Scale = Vector3.one;
    private float m_DeleteTime = 0;
    private long m_StartTime = 0;
    private bool m_IsRotateRelativeUser = false;
  }
}
