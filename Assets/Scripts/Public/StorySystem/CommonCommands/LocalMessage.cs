using System;
using System.Collections.Generic;
using System.Collections;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// firemessage(msgid,arg1,arg2,...);
  /// </summary>
  internal class LocalMessageCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      LocalMessageCommand cmd = new LocalMessageCommand();
      cmd.m_MsgId = m_MsgId.Clone();
      foreach (StoryValue val in m_MsgArgs) {
        cmd.m_MsgArgs.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_MsgId.Evaluate(iterator, args);
      foreach (StoryValue val in m_MsgArgs) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_MsgId.Evaluate(instance);
      foreach (StoryValue val in m_MsgArgs) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string msgId = m_MsgId.Value;
      ArrayList arglist = new ArrayList();
      foreach (StoryValue val in m_MsgArgs) {
        arglist.Add(val.Value);
      }
      object[] args = arglist.ToArray();
      instance.SendMessage(msgId, args);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_MsgId.InitFromDsl(callData.GetParam(0));
      }
      for (int i = 1; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_MsgArgs.Add(val);
      }
    }

    private IStoryValue<string> m_MsgId = new StoryValue<string>();
    private List<IStoryValue<object>> m_MsgArgs = new List<IStoryValue<object>>();
  }
}
