using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// if(@val>0)
  /// {
  ///   createnpc(123);
  /// };
  /// 
  /// or
  /// 
  /// if(@val>0)
  /// {
  ///   createnpc(123);
  /// }
  /// else
  /// {
  ///   missioncomplete();
  /// };
  /// </summary>
  internal class IfElseCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      IfElseCommand retCmd = new IfElseCommand();
      retCmd.m_Condition = m_Condition.Clone();
      foreach (IStoryCommand cmd in m_LoadedIfCommands) {
        retCmd.m_LoadedIfCommands.Add(cmd.Clone());
      }
      foreach (IStoryCommand cmd in m_LoadedElseCommands) {
        retCmd.m_LoadedElseCommands.Add(cmd.Clone());
      }
      retCmd.IsCompositeCommand = true;
      return retCmd;
    }

    protected override void ResetState()
    {
      m_AlreadyExecute = false;
      foreach (IStoryCommand cmd in m_IfCommandQueue) {
        cmd.Reset();
      }
      m_IfCommandQueue.Clear();
      foreach (IStoryCommand cmd in m_ElseCommandQueue) {
        cmd.Reset();
      }
      m_ElseCommandQueue.Clear();
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Arguments = args;
      m_Condition.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Condition.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = false;
      if (m_IfCommandQueue.Count == 0 && m_ElseCommandQueue.Count==0 && !m_AlreadyExecute) {
        if (m_Condition.Value != 0) {
          PrepareIf();
          foreach (IStoryCommand cmd in m_IfCommandQueue) {
            cmd.Prepare(instance, m_Iterator, m_Arguments);
          }
          ret = true;
        } else {
          PrepareElse();
          foreach (IStoryCommand cmd in m_ElseCommandQueue) {
            cmd.Prepare(instance, m_Iterator, m_Arguments);
          }
          ret = true;
        }
        m_AlreadyExecute = true;
      } else {
        if (m_IfCommandQueue.Count > 0) {
          while (m_IfCommandQueue.Count > 0) {
            IStoryCommand cmd = m_IfCommandQueue.Peek();
            if (cmd.Execute(instance, delta)) {
              break;
            } else {
              cmd.Reset();
              m_IfCommandQueue.Dequeue();
            }
          }
          ret = true;
        }
        if (m_ElseCommandQueue.Count > 0) {
          while (m_ElseCommandQueue.Count > 0) {
            IStoryCommand cmd = m_ElseCommandQueue.Peek();
            if (cmd.Execute(instance, delta)) {
              break;
            } else {
              cmd.Reset();
              m_ElseCommandQueue.Dequeue();
            }
          }
          ret = true;
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
            m_LoadedIfCommands.Add(cmd);
        }
      }
    }

    protected override void Load(ScriptableData.StatementData statementData)
    {
      Load(statementData.First);
      ScriptableData.FunctionData functionData = statementData.Second;
      if (null != functionData && functionData.GetId() == "else") {
        foreach (ScriptableData.ISyntaxComponent statement in functionData.Statements) {
          IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(statement);
          if (null != cmd)
            m_LoadedElseCommands.Add(cmd);
        }
      }
    }

    private void PrepareIf()
    {
      foreach (IStoryCommand cmd in m_IfCommandQueue) {
        cmd.Reset();
      }
      m_IfCommandQueue.Clear();
      foreach (IStoryCommand cmd in m_LoadedIfCommands) {
        m_IfCommandQueue.Enqueue(cmd);
      }
    }

    private void PrepareElse()
    {
      foreach (IStoryCommand cmd in m_ElseCommandQueue) {
        cmd.Reset();
      }
      m_ElseCommandQueue.Clear();
      foreach (IStoryCommand cmd in m_LoadedElseCommands) {
        m_ElseCommandQueue.Enqueue(cmd);
      }
    }

    private object m_Iterator = null;
    private object[] m_Arguments = null;
    private IStoryValue<int> m_Condition = new StoryValue<int>();
    private Queue<IStoryCommand> m_IfCommandQueue = new Queue<IStoryCommand>();
    private List<IStoryCommand> m_LoadedIfCommands = new List<IStoryCommand>();
    private Queue<IStoryCommand> m_ElseCommandQueue = new Queue<IStoryCommand>();
    private List<IStoryCommand> m_LoadedElseCommands = new List<IStoryCommand>();

    private bool m_AlreadyExecute = false;
  }
}
