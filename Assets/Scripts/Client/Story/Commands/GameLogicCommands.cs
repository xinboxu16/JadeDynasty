using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;

namespace DashFire.Story.Commands
{
  /// <summary>
  /// reconnectlobby();
  /// </summary>
  internal class ReconnectLobbyCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ReconnectLobbyCommand cmd = new ReconnectLobbyCommand();
      return cmd;
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      DashFire.Network.LobbyNetworkSystem.Instance.ConnectIfNotOpen();
      GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, true);
      return false;
    }
  }
  /// <summary>
  /// publishfilterevent(ev,group,args);
  /// </summary>
  internal class PublishFilterEventCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PublishFilterEventCommand cmd = new PublishFilterEventCommand();
      cmd.m_EventName = m_EventName.Clone();
      cmd.m_Group = m_Group.Clone();
      cmd.m_Args = m_Args.Clone();
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_EventName.Evaluate(iterator, args);
      m_Group.Evaluate(iterator, args);
      m_Args.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_EventName.Evaluate(instance);
      m_Group.Evaluate(instance);
      m_Args.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string evname = m_EventName.Value;
      string group = m_Group.Value;
      object[] args = m_Args.Value as object[];
      GfxSystem.EventChannelForLogic.Publish(evname, group, args);
      return false;
    }
    
    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 2) {
        m_EventName.InitFromDsl(callData.GetParam(0));
        m_Group.InitFromDsl(callData.GetParam(1));
        m_Args.InitFromDsl(callData.GetParam(2));
      }
    }
    
    private IStoryValue<string> m_EventName = new StoryValue<string>();
    private IStoryValue<string> m_Group = new StoryValue<string>();
    private IStoryValue<object> m_Args = new StoryValue();
  }
  /// <summary>
  /// publishlogicevent(ev_name,group,arg1,arg2,...);
  /// </summary>
  internal class PublishLogicEventCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PublishLogicEventCommand cmd = new PublishLogicEventCommand();
      cmd.m_EventName = m_EventName.Clone();
      cmd.m_Group = m_Group.Clone();
      foreach (StoryValue val in m_Args) {
        cmd.m_Args.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_EventName.Evaluate(iterator, args);
      m_Group.Evaluate(iterator, args);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_EventName.Evaluate(instance);
      m_Group.Evaluate(instance);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string evname = m_EventName.Value;
      string group = m_Group.Value;
      ArrayList arglist = new ArrayList();
      foreach (StoryValue val in m_Args) {
        arglist.Add(val.Value);
      }
      object[] args = arglist.ToArray();
      GfxSystem.EventChannelForLogic.Publish(evname, group, args);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_EventName.InitFromDsl(callData.GetParam(0));
        m_Group.InitFromDsl(callData.GetParam(1));
      }
      for (int i = 2; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_Args.Add(val);
      }
    }

    private IStoryValue<string> m_EventName = new StoryValue<string>();
    private IStoryValue<string> m_Group = new StoryValue<string>();
    private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
  }
  /// <summary>
  /// publishgfxevent(ev_name,group,arg1,arg2,...);
  /// </summary>
  internal class PublishGfxEventCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PublishGfxEventCommand cmd = new PublishGfxEventCommand();
      cmd.m_EventName = m_EventName.Clone();
      cmd.m_Group = m_Group.Clone();
      foreach (StoryValue val in m_Args) {
        cmd.m_Args.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_EventName.Evaluate(iterator, args);
      m_Group.Evaluate(iterator, args);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_EventName.Evaluate(instance);
      m_Group.Evaluate(instance);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string evname = m_EventName.Value;
      string group = m_Group.Value;
      ArrayList arglist = new ArrayList();
      foreach (StoryValue val in m_Args) {
        arglist.Add(val.Value);
      }
      object[] args = arglist.ToArray();
      GfxSystem.PublishGfxEvent(evname, group, args);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_EventName.InitFromDsl(callData.GetParam(0));
        m_Group.InitFromDsl(callData.GetParam(1));
      }
      for (int i = 2; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_Args.Add(val);
      }
    }

    private IStoryValue<string> m_EventName = new StoryValue<string>();
    private IStoryValue<string> m_Group = new StoryValue<string>();
    private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
  }
  /// <summary>
  /// sendgfxmessage(objname,msg,arg1,arg2,...);
  /// </summary>
  internal class SendGfxMessageCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      SendGfxMessageCommand cmd = new SendGfxMessageCommand();
      cmd.m_ObjName = m_ObjName.Clone();
      cmd.m_Msg = m_Msg.Clone();
      foreach (StoryValue val in m_Args) {
        cmd.m_Args.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjName.Evaluate(iterator, args);
      m_Msg.Evaluate(iterator, args);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjName.Evaluate(instance);
      m_Msg.Evaluate(instance);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string objname = m_ObjName.Value;
      string msg = m_Msg.Value;
      ArrayList arglist = new ArrayList();
      foreach (StoryValue val in m_Args) {
        arglist.Add(val.Value);
      }
      object[] args = arglist.ToArray();
      if(args.Length==1)
        GfxSystem.SendMessage(objname, msg, args[0]);
      else
        GfxSystem.SendMessage(objname, msg, args);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjName.InitFromDsl(callData.GetParam(0));
        m_Msg.InitFromDsl(callData.GetParam(1));
      }
      for (int i = 2; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_Args.Add(val);
      }
    }

    private IStoryValue<string> m_ObjName = new StoryValue<string>();
    private IStoryValue<string> m_Msg = new StoryValue<string>();
    private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
  }
  /// <summary>
  /// sendgfxmessagewithtag(objname,msg,arg1,arg2,...);
  /// </summary>
  internal class SendGfxMessageWithTagCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      SendGfxMessageWithTagCommand cmd = new SendGfxMessageWithTagCommand();
      cmd.m_ObjTag = m_ObjTag.Clone();
      cmd.m_Msg = m_Msg.Clone();
      foreach (StoryValue val in m_Args) {
        cmd.m_Args.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjTag.Evaluate(iterator, args);
      m_Msg.Evaluate(iterator, args);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjTag.Evaluate(instance);
      m_Msg.Evaluate(instance);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      string objtag = m_ObjTag.Value;
      string msg = m_Msg.Value;
      ArrayList arglist = new ArrayList();
      foreach (StoryValue val in m_Args) {
        arglist.Add(val.Value);
      }
      object[] args = arglist.ToArray();
      if (args.Length == 1)
        GfxSystem.SendMessageWithTag(objtag, msg, args[0]);
      else
        GfxSystem.SendMessageWithTag(objtag, msg, args);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjTag.InitFromDsl(callData.GetParam(0));
        m_Msg.InitFromDsl(callData.GetParam(1));
      }
      for (int i = 2; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_Args.Add(val);
      }
    }

    private IStoryValue<string> m_ObjTag = new StoryValue<string>();
    private IStoryValue<string> m_Msg = new StoryValue<string>();
    private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
  }
  /// <summary>
  /// sendgfxmessagebyid(objname,msg,arg1,arg2,...);
  /// </summary>
  internal class SendGfxMessageByIdCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      SendGfxMessageByIdCommand cmd = new SendGfxMessageByIdCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_Msg = m_Msg.Clone();
      foreach (StoryValue val in m_Args) {
        cmd.m_Args.Add(val.Clone());
      }
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_Msg.Evaluate(iterator, args);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_Msg.Evaluate(instance);
      foreach (StoryValue val in m_Args) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objid = m_ObjId.Value;
      CharacterView view = EntityManager.Instance.GetCharacterViewById(objid);
      if (null != view) {
        string msg = m_Msg.Value;
        ArrayList arglist = new ArrayList();
        foreach (StoryValue val in m_Args) {
          arglist.Add(val.Value);
        }
        object[] args = arglist.ToArray();
        if (args.Length == 1)
          GfxSystem.SendMessage(view.Actor, msg, args[0]);
        else
          GfxSystem.SendMessage(view.Actor, msg, args);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_Msg.InitFromDsl(callData.GetParam(1));
      }
      for (int i = 2; i < callData.GetParamNum(); ++i) {
        StoryValue val = new StoryValue();
        val.InitFromDsl(callData.GetParam(i));
        m_Args.Add(val);
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<string> m_Msg = new StoryValue<string>();
    private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
  }
}
