using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  /// <summary>
  /// animation(anim_name[,start_time]);
  /// 
  /// or
  /// 
  /// animation(anim_name[,start_time])
  /// {
  ///   speed(0.6);
  ///   weight(0.8);
  ///   playmode(1, 50);
  ///   blendmode(0);
  ///   mixingnode("bone");
  /// };
  /// </summary>
  internal class AnimationTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      AnimationTriger triger = new AnimationTriger();
      triger.m_AnimName = m_AnimName;
      triger.m_StartTime = m_StartTime;
      triger.m_Speed = m_Speed;
      triger.m_Weight = m_Weight;
      triger.m_Layer = m_Layer;
      triger.m_WrapMode = m_WrapMode;
      triger.m_PlayMode = m_PlayMode;
      triger.m_BlendMode = m_BlendMode;
      triger.m_MixingNode = m_MixingNode;
      triger.m_CrossFadeTime = m_CrossFadeTime;
      return triger;
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      GameObject obj = sender as GameObject;
      if (null != obj && null != obj.GetComponent<Animation>()) {
        if (curSectionTime >= m_StartTime) {
          AnimationState state = obj.GetComponent<Animation>()[m_AnimName];
          if (null != state) {
            state.speed = m_Speed;
            if (m_IsEffectSkillTime) {
              instance.TimeScale = m_Speed;
            }
            state.weight = m_Weight;
            state.layer = m_Layer;
            state.wrapMode = (WrapMode)m_WrapMode;
            state.normalizedTime = 0;
            state.blendMode = (m_BlendMode == 0 ? AnimationBlendMode.Blend : AnimationBlendMode.Additive);

            if (!string.IsNullOrEmpty(m_MixingNode)) {
              //todo:支持部分骨骼分离的动画播放
            }
          }
          if (m_PlayMode == 0) {
            obj.GetComponent<Animation>().Play(m_AnimName);
          } else {
            obj.GetComponent<Animation>().CrossFade(m_AnimName, m_CrossFadeTime / 1000.0f);
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
        m_AnimName = callData.GetParamId(0);
      }
      if (num > 1) {
        m_StartTime = long.Parse(callData.GetParamId(1));
      }
    }

    protected override void Load(ScriptableData.FunctionData funcData)
    {
      ScriptableData.CallData callData = funcData.Call;
      if (null != callData) {
        Load(callData);

        foreach (ScriptableData.ISyntaxComponent statement in funcData.Statements) {
          ScriptableData.CallData stCall = statement as ScriptableData.CallData;
          if (null != stCall && stCall.GetParamNum() > 0) {
            string id = stCall.GetId();
            string param = stCall.GetParamId(0);

            if (id == "speed") {
              m_Speed = float.Parse(param);
              if (stCall.GetParamNum() >= 2) {
                m_IsEffectSkillTime = bool.Parse(stCall.GetParamId(1));
              }
            } else if (id == "weight") {
              m_Weight = float.Parse(param);
            } else if (id == "layer") {
              m_Layer = int.Parse(param);
            } else if (id == "playmode") {
              m_PlayMode = int.Parse(param);
              if (stCall.GetParamNum() >= 2) {
                m_CrossFadeTime = long.Parse(stCall.GetParamId(1));
              }
            } else if (id == "blendmode") {
              m_BlendMode = int.Parse(param);
            } else if (id == "mixingnode") {
              m_MixingNode = param;
            } else if (id == "wrapmode") {
              m_WrapMode = int.Parse(param);
            }
          }
        }
      }
    }

    private string m_AnimName = "";

    private float m_Speed = 1.0f;
    private bool m_IsEffectSkillTime = false;
    private float m_Weight = 1.0f;
    private int m_Layer = 0;
    private int m_WrapMode = (int)WrapMode.ClampForever;//播放动画。当播放到结尾的时候，动画总是处于最后一帧的采样状态。
    private int m_PlayMode = 0;
    private int m_BlendMode = 0;
    private string m_MixingNode = "";
    private long m_CrossFadeTime = 300;
  }

  public class AnimationSpeedTrigger : AbstractSkillTriger {
    public override ISkillTriger Clone()
    {
      AnimationSpeedTrigger copy = new AnimationSpeedTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_AnimName = m_AnimName;
      copy.m_Speed = m_Speed;
      copy.m_IsEffectSkillTime = m_IsEffectSkillTime;
      return copy;
    }

    public override void Reset()
    {
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
      Animation animation = obj.GetComponent<Animation>();
      AnimationState state = animation[m_AnimName];
      if (state != null) {
        float passed_ms = curSectionTime - m_StartTime;
        if (passed_ms > 0) {
          float old_speed = state.speed;
          state.time -= old_speed * passed_ms / 1000.0f;
          state.time += m_Speed * passed_ms / 1000.0f;
          if (state.time < 0) {
            state.time = 0;
          }
        }
        state.speed = m_Speed;
        if (m_IsEffectSkillTime) {
          instance.TimeScale = m_Speed;
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num >= 3) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_AnimName = callData.GetParamId(1);
        m_Speed = float.Parse(callData.GetParamId(2));
      }
      if (num >= 4) {
        m_IsEffectSkillTime = bool.Parse(callData.GetParamId(3));
      }
    }

    private string m_AnimName = "";
    private float m_Speed = 1.0f;
    private bool m_IsEffectSkillTime = false;
  }

  /// <summary>
  /// setcross2othertime(100, "stand", 3000);
  /// </summary>
  public class SetCrossFadeTimeTrigger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      SetCrossFadeTimeTrigger copy = new SetCrossFadeTimeTrigger();
      copy.m_StartTime = m_StartTime;
      copy.m_TargetAnimType = m_TargetAnimType;
      copy.m_CrossFadeTime = m_CrossFadeTime;
      return copy;
    }

    public override void Reset()
    {
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
      DashFire.LogicSystem.NotifyGfxSetCrossFadeTime(obj, m_TargetAnimType, m_CrossFadeTime);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num >= 3) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_TargetAnimType = callData.GetParamId(1);
        m_CrossFadeTime = long.Parse(callData.GetParamId(2)) / 1000.0f;
      }
    }

    private string m_TargetAnimType = "";
    private float m_CrossFadeTime = 0;
  }
}