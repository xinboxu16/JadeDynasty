using System;
using System.Collections.Generic;
using StorySystem;
using UnityEngine;

namespace DashFire.Story.Commands
{
  /// <summary>
  /// playerselfface(dir);
  /// </summary>
  internal class PlayerselfFaceCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PlayerselfFaceCommand cmd = new PlayerselfFaceCommand();
      cmd.m_Dir = m_Dir.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Dir.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Dir.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      float dir = m_Dir.Value;
      UserInfo user = WorldSystem.Instance.GetPlayerSelf();
      if (null != user) {
        user.GetMovementStateInfo().SetFaceDir(dir);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Dir.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<float> m_Dir = new StoryValue<float>();
  }
  /// <summary>
  /// playerselfmove(vector3(x,y,z));
  /// </summary>
  internal class PlayerselfMoveCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PlayerselfMoveCommand cmd = new PlayerselfMoveCommand();
      cmd.m_Pos = m_Pos.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_Pos.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_Pos.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      Vector3 pos = m_Pos.Value;
      UserInfo user = WorldSystem.Instance.GetPlayerSelf();
      if (null != user) {
        List<Vector3> waypoints = user.SpatialSystem.FindPath(user.GetMovementStateInfo().GetPosition3D(), pos, 1);
        waypoints.Add(pos);
        UserAiStateInfo aiInfo = user.GetAiStateInfo();
        AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
        if (null == data) {
          data = new AiData_ForMoveCommand(waypoints);
          aiInfo.AiDatas.AddData(data);
        }
        data.WayPoints = waypoints;
        data.Index = 0;
        data.EstimateFinishTime = 0;
        data.IsFinish = false;
        aiInfo.ChangeToState((int)AiStateId.MoveCommand);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_Pos.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
  }
  /// <summary>
  /// playerselfmovewithwaypoints(vector2list("1 2 3 4 5 6 7"));
  /// </summary>
  internal class PlayerselfMoveWithWaypointsCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PlayerselfMoveWithWaypointsCommand cmd = new PlayerselfMoveWithWaypointsCommand();
      cmd.m_WayPoints = m_WayPoints.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_WayPoints.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_WayPoints.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      List<Vector2> poses = m_WayPoints.Value;
      UserInfo user = WorldSystem.Instance.GetPlayerSelf();
      if (null != user) {
        List<Vector3> waypoints = new List<Vector3>();
        foreach (Vector2 pt in poses) {
          waypoints.Add(new Vector3(pt.x, 0, pt.y));
        }
        UserAiStateInfo aiInfo = user.GetAiStateInfo();
        AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
        if (null == data) {
          data = new AiData_ForMoveCommand(waypoints);
          aiInfo.AiDatas.AddData(data);
        }
        data.WayPoints = waypoints;
        data.Index = 0;
        data.EstimateFinishTime = 0;
        data.IsFinish = false;
        aiInfo.ChangeToState((int)AiStateId.MoveCommand);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_WayPoints.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<List<Vector2>> m_WayPoints = new StoryValue<List<Vector2>>();
  }
  /// <summary>
  /// playerselfpursuit(target_obj_id);
  /// </summary>
  internal class PlayerselfPursuitCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PlayerselfPursuitCommand cmd = new PlayerselfPursuitCommand();
      cmd.m_TargetId = m_TargetId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_TargetId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_TargetId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int targetId = m_TargetId.Value;
      UserInfo user = WorldSystem.Instance.GetPlayerSelf();
      if (null != user) {
        UserAiStateInfo aiInfo = user.GetAiStateInfo();
        AiData_ForPursuitCommand data = aiInfo.AiDatas.GetData<AiData_ForPursuitCommand>();
        if (null == data) {
          data = new AiData_ForPursuitCommand();
          aiInfo.AiDatas.AddData(data);
        }
        aiInfo.Target = targetId;
        aiInfo.ChangeToState((int)AiStateId.PursuitCommand);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_TargetId.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_TargetId = new StoryValue<int>();
  }
  /// <summary>
  /// playerselfstop();
  /// </summary>
  internal class PlayerselfStopCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      PlayerselfStopCommand cmd = new PlayerselfStopCommand();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      UserInfo user = WorldSystem.Instance.GetPlayerSelf();
      if (null != user) {
        UserAiStateInfo aiInfo = user.GetAiStateInfo();
        if (aiInfo.CurState == (int)AiStateId.MoveCommand || aiInfo.CurState == (int)AiStateId.PursuitCommand || aiInfo.CurState == (int)AiStateId.PatrolCommand) {
          aiInfo.Time = 0;
          aiInfo.Target = 0;
          aiInfo.AiDatas.RemoveData<AiData_ForMoveCommand>();
          aiInfo.AiDatas.RemoveData<AiData_ForPursuitCommand>();
          aiInfo.AiDatas.RemoveData<AiData_ForPatrolCommand>();
          aiInfo.ChangeToState((int)AiStateId.Idle);
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
      }
    }
  }
}
