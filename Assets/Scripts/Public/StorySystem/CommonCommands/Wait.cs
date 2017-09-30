using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// sleep(milliseconds);
  /// </summary>
  internal class SleepCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      SleepCommand cmd = new SleepCommand();
      cmd.m_Time = m_Time.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
      m_CurTime = 0;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Time.Evaluate(iterator, args);    
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Time.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int curTime = m_CurTime;
      m_CurTime += (int)delta;
      if (curTime <= m_Time.Value)
        return true;
      else
        return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Time.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_Time = new StoryValue<int>();
    private int m_CurTime = 0;
  }
}
