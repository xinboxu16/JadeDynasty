using System;
using System.Collections.Generic;
using StorySystem;

namespace StorySystem.CommonValues
{
  internal sealed class AddOperator : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "+") {
        if (callData.GetParamNum() == 1) {
          m_X.InitFromDsl(new ScriptableData.ValueData("0"));
          m_Y.InitFromDsl(callData.GetParam(0));
        } else if (callData.GetParamNum() == 2) {
          m_X.InitFromDsl(callData.GetParam(0));
          m_Y.InitFromDsl(callData.GetParam(1));
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      AddOperator val = new AddOperator();
      val.m_X = m_X.Clone();
      val.m_Y = m_Y.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_X.Evaluate(iterator, args);
      m_Y.Evaluate(iterator, args);
      TryUpdateValue();
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      m_Y.Evaluate(instance);
      TryUpdateValue();
    }
    public void Analyze(StoryInstance instance)
    {
      m_X.Analyze(instance);
      m_Y.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue && m_Y.HaveValue) {
        m_HaveValue = true;
        object objX = m_X.Value;
        object objY = m_Y.Value;
        if (objX is string || objY is string) {
          string x = StoryValueHelper.CastTo<string>(objX);
          string y = StoryValueHelper.CastTo<string>(objY);
          m_Value = x + y;
        } else {
          if (objX is int && objY is int) {
            int x = StoryValueHelper.CastTo<int>(objX);
            int y = StoryValueHelper.CastTo<int>(objY);
            m_Value = x + y;
          } else {
            float x = StoryValueHelper.CastTo<float>(objX);
            float y = StoryValueHelper.CastTo<float>(objY);
            m_Value = x + y;
          }
        }
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private IStoryValue<object> m_Y = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class SubOperator : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "-") {
        if (callData.GetParamNum() == 1) {
          m_X.InitFromDsl(new ScriptableData.ValueData("0"));
          m_Y.InitFromDsl(callData.GetParam(0));
        } else if (callData.GetParamNum() == 2) {
          m_X.InitFromDsl(callData.GetParam(0));
          m_Y.InitFromDsl(callData.GetParam(1));
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      SubOperator val = new SubOperator();
      val.m_X = m_X.Clone();
      val.m_Y = m_Y.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_X.Evaluate(iterator, args);
      m_Y.Evaluate(iterator, args);
      TryUpdateValue();
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      m_Y.Evaluate(instance);
      TryUpdateValue();
    }
    public void Analyze(StoryInstance instance)
    {
      m_X.Analyze(instance);
      m_Y.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue && m_Y.HaveValue) {
        m_HaveValue = true;
        object objX = m_X.Value;
        object objY = m_Y.Value;
        if (objX is int && objY is int) {
          int x = StoryValueHelper.CastTo<int>(objX);
          int y = StoryValueHelper.CastTo<int>(objY);
          m_Value = x - y;
        } else {
          float x = StoryValueHelper.CastTo<float>(objX);
          float y = StoryValueHelper.CastTo<float>(objY);
          m_Value = x - y;
        }
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private IStoryValue<object> m_Y = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class MulOperator : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "*" && callData.GetParamNum() == 2) {
        m_X.InitFromDsl(callData.GetParam(0));
        m_Y.InitFromDsl(callData.GetParam(1));
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      MulOperator val = new MulOperator();
      val.m_X = m_X.Clone();
      val.m_Y = m_Y.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_X.Evaluate(iterator, args);
      m_Y.Evaluate(iterator, args);
      TryUpdateValue();
    }
    public void Evaluate(StoryInstance instance)
    {
      m_X.Evaluate(instance);
      m_Y.Evaluate(instance);
      TryUpdateValue();
    }
    public void Analyze(StoryInstance instance)
    {
      m_X.Analyze(instance);
      m_Y.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue()
    {
      if (m_X.HaveValue && m_Y.HaveValue) {
        m_HaveValue = true;
        object objX = m_X.Value;
        object objY = m_Y.Value;
        if (objX is int && objY is int) {
          int x = StoryValueHelper.CastTo<int>(objX);
          int y = StoryValueHelper.CastTo<int>(objY);
          m_Value = x * y;
        } else {
          float x = StoryValueHelper.CastTo<float>(objX);
          float y = StoryValueHelper.CastTo<float>(objY);
          m_Value = x * y;
        }
      }
    }

    private IStoryValue<object> m_X = new StoryValue();
    private IStoryValue<object> m_Y = new StoryValue();
    private bool m_HaveValue;
    private object m_Value;
  }
}
