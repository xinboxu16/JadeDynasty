using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// assign(@local,value);
  /// or
  /// assign(@@global,value);
  /// </summary>
  internal class AssignCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      AssignCommand cmd = new AssignCommand();
      cmd.m_VarName = m_VarName;
      cmd.m_Value = m_Value.Clone();
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Value.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Value.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_Value.HaveValue) {
        if (m_VarName.StartsWith("@@")) {
          if (null != instance.GlobalVariables) {
            if (instance.GlobalVariables.ContainsKey(m_VarName)) {
              instance.GlobalVariables[m_VarName] = m_Value.Value;
            } else {
              instance.GlobalVariables.Add(m_VarName, m_Value.Value);
            }
          }
        } else if (m_VarName.StartsWith("@")) {
          if (instance.LocalVariables.ContainsKey(m_VarName)) {
            instance.LocalVariables[m_VarName] = m_Value.Value;
          } else {
            instance.LocalVariables.Add(m_VarName, m_Value.Value);
          }
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_VarName = callData.GetParamId(0);
        m_Value.InitFromDsl(callData.GetParam(1));
      }
    }

    private string m_VarName = null;
    private IStoryValue<object> m_Value = new StoryValue();
  }
  /// <summary>
  /// inc(@local,value);
  /// or
  /// inc(@@global,value);
  /// </summary>
  internal class IncCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      IncCommand cmd = new IncCommand();
      cmd.m_VarName = m_VarName;
      cmd.m_Value = m_Value.Clone();
      cmd.m_ParamNum = m_ParamNum;
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Value.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Value.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_VarName.StartsWith("@@")) {
        if (null != instance.GlobalVariables) {
          if (instance.GlobalVariables.ContainsKey(m_VarName)) {
            object oval = instance.GlobalVariables[m_VarName];
            if (oval is int) {
              int ov = StoryValueHelper.CastTo<int>(oval);
              if (m_ParamNum > 1 && m_Value.HaveValue) {
                int v = StoryValueHelper.CastTo<int>(m_Value.Value);
                ov += v;
                instance.GlobalVariables[m_VarName] = ov;
              } else {
                ++ov;
                instance.GlobalVariables[m_VarName] = ov;
              }
            } else {
              float ov = StoryValueHelper.CastTo<float>(oval);
              if (m_ParamNum > 1 && m_Value.HaveValue) {
                float v = StoryValueHelper.CastTo<float>(m_Value.Value);
                ov += v;
                instance.GlobalVariables[m_VarName] = ov;
              } else {
                ++ov;
                instance.GlobalVariables[m_VarName] = ov;
              }
            }
          }
        }
      } else if (m_VarName.StartsWith("@")) {
        if (instance.LocalVariables.ContainsKey(m_VarName)) {
          object oval = instance.LocalVariables[m_VarName];
          if (oval is int) {
            int ov = StoryValueHelper.CastTo<int>(oval);
            if (m_ParamNum > 1 && m_Value.HaveValue) {
              int v = StoryValueHelper.CastTo<int>(m_Value.Value);
              ov += v;
              instance.LocalVariables[m_VarName] = ov;
            } else {
              ++ov;
              instance.LocalVariables[m_VarName] = ov;
            }
          } else {
            float ov = StoryValueHelper.CastTo<float>(oval);
            if (m_ParamNum > 1 && m_Value.HaveValue) {
              float v = StoryValueHelper.CastTo<float>(m_Value.Value);
              ov += v;
              instance.LocalVariables[m_VarName] = ov;
            } else {
              ++ov;
              instance.LocalVariables[m_VarName] = ov;
            }
          }
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      m_ParamNum = num;
      if (num > 0) {
        m_VarName = callData.GetParamId(0);
      }
      if (num > 1) {
        m_Value.InitFromDsl(callData.GetParam(1));
      }
    }

    private int m_ParamNum = 0;
    private string m_VarName = null;
    private IStoryValue<object> m_Value = new StoryValue();
  }
  /// <summary>
  /// dec(@local,value);
  /// or
  /// dec(@@global,value);
  /// </summary>
  internal class DecCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      DecCommand cmd = new DecCommand();
      cmd.m_VarName = m_VarName;
      cmd.m_Value = m_Value.Clone();
      cmd.m_ParamNum = m_ParamNum;
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Value.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Value.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_VarName.StartsWith("@@")) {
        if (null != instance.GlobalVariables) {
          if (instance.GlobalVariables.ContainsKey(m_VarName)) {
            object oval = instance.GlobalVariables[m_VarName];
            if (oval is int) {
              int ov = StoryValueHelper.CastTo<int>(oval);
              if (m_ParamNum > 1 && m_Value.HaveValue) {
                int v = StoryValueHelper.CastTo<int>(m_Value.Value);
                ov -= v;
                instance.GlobalVariables[m_VarName] = ov;
              } else {
                --ov;
                instance.GlobalVariables[m_VarName] = ov;
              }
            } else {
              float ov = StoryValueHelper.CastTo<float>(oval);
              if (m_ParamNum > 1 && m_Value.HaveValue) {
                float v = StoryValueHelper.CastTo<float>(m_Value.Value);
                ov -= v;
                instance.GlobalVariables[m_VarName] = ov;
              } else {
                --ov;
                instance.GlobalVariables[m_VarName] = ov;
              }
            }
          }
        }
      } else if (m_VarName.StartsWith("@")) {
        if (instance.LocalVariables.ContainsKey(m_VarName)) {
          object oval = instance.LocalVariables[m_VarName];
          if (oval is int) {
            int ov = StoryValueHelper.CastTo<int>(oval);
            if (m_ParamNum > 1 && m_Value.HaveValue) {
              int v = StoryValueHelper.CastTo<int>(m_Value.Value);
              ov -= v;
              instance.LocalVariables[m_VarName] = ov;
            } else {
              --ov;
              instance.LocalVariables[m_VarName] = ov;
            }
          } else {
            float ov = StoryValueHelper.CastTo<float>(oval);
            if (m_ParamNum > 1 && m_Value.HaveValue) {
              float v = StoryValueHelper.CastTo<float>(m_Value.Value);
              ov -= v;
              instance.LocalVariables[m_VarName] = ov;
            } else {
              --ov;
              instance.LocalVariables[m_VarName] = ov;
            }
          }
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      m_ParamNum = num;
      if (num > 0) {
        m_VarName = callData.GetParamId(0);
      }
      if (num > 1) {
        m_Value.InitFromDsl(callData.GetParam(1));
      }
    }

    private int m_ParamNum = 0;
    private string m_VarName = null;
    private IStoryValue<object> m_Value = new StoryValue();
  }
  /// <summary>
  /// propset(name,value);
  /// </summary>
  internal class PropSetCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PropSetCommand cmd = new PropSetCommand();
      cmd.m_VarName = m_VarName.Clone();
      cmd.m_Value = m_Value.Clone();
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_VarName.Evaluate(iterator, args);
      m_Value.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_VarName.Evaluate(instance);
      m_Value.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_VarName.HaveValue && m_Value.HaveValue) {
        string varName = m_VarName.Value;
        if (varName.StartsWith("@") && !varName.StartsWith("@@")) {
          if (instance.LocalVariables.ContainsKey(varName) && m_Value.HaveValue) {
            instance.LocalVariables[varName] = m_Value.Value;
          }
        } else {
          if (null != instance.GlobalVariables && m_Value.HaveValue) {
            if (instance.GlobalVariables.ContainsKey(varName)) {
              instance.GlobalVariables[varName] = m_Value.Value;
            } else {
              instance.GlobalVariables.Add(varName, m_Value.Value);
            }
          }
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_VarName.InitFromDsl(callData.GetParam(0));
        m_Value.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<string> m_VarName = new StoryValue<string>();
    private IStoryValue<object> m_Value = new StoryValue();
  }
}
