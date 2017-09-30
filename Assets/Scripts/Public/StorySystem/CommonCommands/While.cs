using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// while($val<10)
  /// {
  ///   createnpc($$);
  ///   wait(100);
  /// };
  /// </summary>
  internal class WhileCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      WhileCommand retCmd = new WhileCommand();
      retCmd.m_Condition = m_Condition.Clone();
      foreach (IStoryCommand cmd in m_LoadedCommands) {
        retCmd.m_LoadedCommands.Add(cmd.Clone());
      }
      retCmd.IsCompositeCommand = true;
      return retCmd;
    }

    protected override void ResetState()
    {
      m_CurCount = 0;
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Arguments = args;
      m_Condition.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Condition.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = true;
      while (ret) {
        if (m_CommandQueue.Count == 0) {
          if (m_Condition.Value != 0) {
            Prepare();
            foreach (IStoryCommand cmd in m_CommandQueue) {
              cmd.Prepare(instance, m_CurCount, m_Arguments);
            }
            ++m_CurCount;
            ret = true;
          } else {
            ret = false;
          }
        } else {
          while (m_CommandQueue.Count > 0) {
            IStoryCommand cmd = m_CommandQueue.Peek();
            if (cmd.Execute(instance, delta)) {
              break;
            } else {
              cmd.Reset();
              m_CommandQueue.Dequeue();
            }
          }
          ret = true;
          if (m_CommandQueue.Count > 0) {
            break;
          }
        }
      }
      return ret;
    }

    protected override void Load(ScriptableData.FunctionData functionData)
    {
      ScriptableData.CallData callData = functionData.Call;
      if (null != callData) {
        if (callData.GetParamNum() > 0) {
          ScriptableData.ISyntaxComponent param = callData.GetParam(0);
          m_Condition.InitFromDsl(param);
        }
        foreach (ScriptableData.ISyntaxComponent statement in functionData.Statements) {
          IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(statement);
          if (null != cmd)
            m_LoadedCommands.Add(cmd);
        }
      }
    }

    private void Prepare()
    {
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
      foreach (IStoryCommand cmd in m_LoadedCommands) {
        m_CommandQueue.Enqueue(cmd);
      }
    }

    private object[] m_Arguments = null;
    private IStoryValue<int> m_Condition = new StoryValue<int>();
    private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
    private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
    private int m_CurCount = 0;
  }
}
