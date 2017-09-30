using System;
using System.Collections.Generic;
using DashFire;

namespace StorySystem.CommonValues
{
  internal sealed class TimeValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "time") {
      }
    }
    public IStoryValue<object> Clone()
    {
      TimeValue val = new TimeValue();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {}
    public void Evaluate(StoryInstance instance)
    {
      m_Value = (int)TimeUtility.GetLocalMilliseconds();
      m_HaveValue = true;
    }
    public void Analyze(StoryInstance instance)
    {
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
    
    private bool m_HaveValue;
    private object m_Value;
  }
}
