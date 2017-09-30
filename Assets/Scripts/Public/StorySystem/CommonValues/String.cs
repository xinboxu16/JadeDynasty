using System;
using System.Collections;
using System.Collections.Generic;

namespace StorySystem.CommonValues
{
  internal sealed class FormatValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "format") {
        int num = callData.GetParamNum();
        if (num > 0) {
          m_Format.InitFromDsl(callData.GetParam(0));
        }
        for (int i = 1; i < callData.GetParamNum(); ++i) {
          StoryValue val = new StoryValue();
          val.InitFromDsl(callData.GetParam(i));
          m_FormatArgs.Add(val);
        }
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      FormatValue val = new FormatValue();
      val.m_Format = m_Format.Clone();
      foreach (StoryValue v in m_FormatArgs) {
        val.m_FormatArgs.Add(v.Clone());
      }
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Format.Evaluate(iterator, args);
      foreach (StoryValue val in m_FormatArgs) {
        val.Evaluate(iterator, args);
      }
      TryUpdateValue();
    }
    public void Evaluate(StoryInstance instance)
    {
      m_Format.Evaluate(instance);
      foreach (StoryValue val in m_FormatArgs) {
        val.Evaluate(instance);
      }
      TryUpdateValue();
    }
    public void Analyze(StoryInstance instance)
    {
      m_Format.Analyze(instance);
      foreach (StoryValue val in m_FormatArgs) {
        val.Analyze(instance);
      }
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
      bool canCalc = true;
      if (!m_Format.HaveValue) {
        canCalc = false;
      } else {
        foreach (StoryValue val in m_FormatArgs) {
          if (!val.HaveValue) {
            canCalc = false;
            break;
          }
        }
      }
      if (canCalc) {
        m_HaveValue = true;
        string format = m_Format.Value;
        ArrayList arglist = new ArrayList();
        foreach (StoryValue val in m_FormatArgs) {
          arglist.Add(val.Value);
        }
        object[] args = arglist.ToArray();
        m_Value = string.Format(format, args);
      }
    }

    private IStoryValue<string> m_Format = new StoryValue<string>();
    private List<IStoryValue<object>> m_FormatArgs = new List<IStoryValue<object>>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class SubstringValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "substring" && callData.GetParamNum() > 0) {
        m_ParamNum = callData.GetParamNum();
        m_Start.InitFromDsl(callData.GetParam(0));
        if (m_ParamNum > 1)
          m_Start.InitFromDsl(callData.GetParam(1));
        if (m_ParamNum > 2)
          m_Length.InitFromDsl(callData.GetParam(2));
        TryUpdateValue();
      }
    }
    public IStoryValue<object> Clone()
    {
      SubstringValue val = new SubstringValue();
      val.m_ParamNum = m_ParamNum;
      val.m_String = m_String.Clone();
      val.m_Start = m_Start.Clone();
      val.m_Length = m_Length.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_String.Evaluate(iterator, args);
      if (m_ParamNum > 1)
        m_Start.Evaluate(iterator, args);
      if (m_ParamNum > 2)
        m_Length.Evaluate(iterator, args);
      TryUpdateValue();
    }
    public void Evaluate(StoryInstance instance)
    {
      m_String.Evaluate(instance);
      if (m_ParamNum > 1)
        m_Start.Evaluate(instance);
      if (m_ParamNum > 2)
        m_Length.Evaluate(instance);
      TryUpdateValue();
    }
    public void Analyze(StoryInstance instance)
    {
      if (m_ParamNum > 1)
        m_Start.Analyze(instance);
      if (m_ParamNum > 2)
        m_Length.Analyze(instance);
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
      if (m_String.HaveValue) {
        bool canCalc = true;
        string str = m_String.Value;
        int start = 0;
        int len = 0;
        if (m_ParamNum == 1 && m_String.HaveValue) {
          len = str.Length;
        }
        if (m_ParamNum == 2 && !m_Start.HaveValue) {
          canCalc = false;
        } else {
          start = m_Start.Value;
          len = str.Length - start;
        }
        if (m_ParamNum == 3 && (!m_Start.HaveValue || !m_Length.HaveValue)) {
          canCalc = false;
        } else {
          start = m_Start.Value;
          len = m_Length.Value;
        }
        if (canCalc) {
          m_HaveValue = true;
          m_Value = str.Substring(start, len);
        }
      }
    }

    private int m_ParamNum = 0;
    private IStoryValue<string> m_String = new StoryValue<string>();
    private IStoryValue<int> m_Start = new StoryValue<int>();
    private IStoryValue<int> m_Length = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
}
