using System;
using System.Collections;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// foreach(v1,v2,v3)
  /// {
  ///   createnpc($$);
  ///   wait(100);
  /// };
  /// </summary>
  internal class ForeachCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ForeachCommand retCmd = new ForeachCommand();
      foreach (IStoryValue<object> val in m_LoadedIterators) {
        retCmd.m_LoadedIterators.Add(val.Clone());
      }
      foreach (IStoryCommand cmd in m_LoadedCommands) {
        retCmd.m_LoadedCommands.Add(cmd.Clone());
      }
      retCmd.IsCompositeCommand = true;
      return retCmd;
    }

    protected override void ResetState()
    {
      m_Iterators.Clear();
      foreach (IStoryValue<object> val in m_LoadedIterators) {
        m_Iterators.Enqueue(val);
      }
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Arguments = args;
      foreach (IStoryValue<object> val in m_Iterators) {
        val.Evaluate(iterator, args);
      }
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      foreach (IStoryValue<object> val in m_Iterators) {
        val.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = true;
      while(ret) {
        if (m_CommandQueue.Count == 0) {
          if (m_Iterators.Count > 0) {
            Prepare();
            IStoryValue<object> val = m_Iterators.Dequeue();
            foreach (IStoryCommand cmd in m_CommandQueue) {
              cmd.Prepare(instance, val.Value, m_Arguments);
            }
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
        for (int i = 0; i < callData.GetParamNum(); ++i) {
          ScriptableData.ISyntaxComponent param = callData.GetParam(i);
          StoryValue val = new StoryValue();
          val.InitFromDsl(param);
          m_LoadedIterators.Add(val);
          m_Iterators.Enqueue(val);
        }
        foreach (ScriptableData.ISyntaxComponent statement in functionData.Statements) {
          IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(statement);
          if(null!=cmd)
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
    private Queue<IStoryValue<object>> m_Iterators = new Queue<IStoryValue<object>>();
    private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
    private List<IStoryValue<object>> m_LoadedIterators = new List<IStoryValue<object>>();
    private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
  }
  /// <summary>
  /// looplist(list)
  /// {
  ///   createnpc($$);
  ///   wait(100);
  /// };
  /// </summary>
  internal class LoopListCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      LoopListCommand retCmd = new LoopListCommand();
      retCmd.m_LoadedList = m_LoadedList.Clone();
      foreach (IStoryCommand cmd in m_LoadedCommands) {
        retCmd.m_LoadedCommands.Add(cmd.Clone());
      }
      retCmd.IsCompositeCommand = true;
      return retCmd;
    }

    protected override void ResetState()
    {
      m_Iterators.Clear();
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Arguments = args;
      m_LoadedList.Evaluate(iterator, args);
      TryUpdateValue();
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_LoadedList.Evaluate(instance);
      TryUpdateValue();
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = true;
      while (ret) {
        if (m_CommandQueue.Count == 0) {
          if (m_Iterators.Count > 0) {
            Prepare();
            object val = m_Iterators.Dequeue();
            foreach (IStoryCommand cmd in m_CommandQueue) {
              cmd.Prepare(instance, val, m_Arguments);
            }
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
          m_LoadedList.InitFromDsl(callData.GetParam(0));
          TryUpdateValue();
        }
        foreach (ScriptableData.ISyntaxComponent statement in functionData.Statements) {
          IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(statement);
          if (null != cmd)
            m_LoadedCommands.Add(cmd);
        }
      }
    }

    private void TryUpdateValue()
    {
      if (m_LoadedList.HaveValue && m_Iterators.Count == 0) {
        foreach (object obj in m_LoadedList.Value) {
          m_Iterators.Enqueue(obj);
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
    private Queue<object> m_Iterators = new Queue<object>();
    private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
    private IStoryValue<IEnumerable> m_LoadedList = new StoryValue<IEnumerable>();
    private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
  }
  /// <summary>
  /// loop(count)
  /// {
  ///   createnpc($$);
  ///   wait(100);
  /// };
  /// </summary>
  internal class LoopCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      LoopCommand retCmd = new LoopCommand();
      retCmd.m_Count = m_Count.Clone();
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
      m_Count.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Count.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = true;
      while (ret) {
        if (m_CommandQueue.Count == 0) {
          if (m_CurCount < m_Count.Value) {
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
        if(callData.GetParamNum()>0) {
          ScriptableData.ISyntaxComponent param = callData.GetParam(0);
          m_Count.InitFromDsl(param);
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
    private IStoryValue<int> m_Count = new StoryValue<int>();
    private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
    private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();

    private int m_CurCount = 0;
  }
}
