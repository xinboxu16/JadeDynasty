using System;
using System.Collections.Generic;
using StorySystem;

namespace DashFire.Story.Commands
{
  /// <summary>
  /// enableinput(true_or_false);
  /// </summary>
  internal class EnableInputCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      EnableInputCommand cmd = new EnableInputCommand();
      cmd.m_Enable = m_Enable.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Enable.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Enable.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_Enable.Value != "false") {
        PlayerControl.Instance.EnableMoveInput = true;
        PlayerControl.Instance.EnableRotateInput = true;
      } else {
        PlayerControl.Instance.EnableMoveInput = false;
        PlayerControl.Instance.EnableRotateInput = false;
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Enable.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<string> m_Enable = new StoryValue<string>();
  }
  /// <summary>
  /// showui(true_or_false);
  /// </summary>
  internal class ShowUiCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ShowUiCommand cmd = new ShowUiCommand();
      cmd.m_Enable = m_Enable.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Enable.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Enable.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_Enable.Value != "false") {
        GfxSystem.SendMessage("GfxGameRoot", "ShowUi", true);
      } else {
        GfxSystem.SendMessage("GfxGameRoot", "ShowUi", false);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Enable.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<string> m_Enable = new StoryValue<string>();
  }
  /// <summary>
  /// showwall(name, true_or_false);
  /// </summary>
  internal class ShowWallCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ShowWallCommand cmd = new ShowWallCommand();
      cmd.m_Name = m_Name.Clone();
      cmd.m_Enable = m_Enable.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Name.Evaluate(iterator, args);
      m_Enable.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Name.Evaluate(instance);
      m_Enable.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      if (m_Enable.Value != "false") {
        GfxSystem.SendMessage(m_Name.Value, "OpenDoor", null);
      } else {
        GfxSystem.SendMessage(m_Name.Value, "CloseDoor", null);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_Name.InitFromDsl(callData.GetParam(0));
        m_Enable.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<string> m_Name = new StoryValue<string>();
    private IStoryValue<string> m_Enable = new StoryValue<string>();
  }
  /// <summary>
  /// showdlg(storyDlgId);
  /// </summary>
  internal class ShowDlgCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ShowDlgCommand cmd = new ShowDlgCommand();
      cmd.m_StoryDlgId = m_StoryDlgId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_StoryDlgId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_StoryDlgId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      GfxSystem.SendMessage("GfxGameRoot", "TriggerStory", m_StoryDlgId.Value);
      LogSystem.Info("showdlg {0}", m_StoryDlgId.Value);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_StoryDlgId.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_StoryDlgId = new StoryValue<int>();
  }
  /// <summary>
  /// startcountdown(countdowntime);
  /// </summary>
  internal class StartCountDownCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      StartCountDownCommand cmd = new StartCountDownCommand();
      cmd.m_CountDownTime = m_CountDownTime.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_CountDownTime.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_CountDownTime.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      GfxSystem.SendMessage("GfxGameRoot", "StartCountDown", m_CountDownTime.Value);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_CountDownTime.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_CountDownTime = new StoryValue<int>();
  }
}
