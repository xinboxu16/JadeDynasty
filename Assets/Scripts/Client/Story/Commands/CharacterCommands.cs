using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using UnityEngine;

namespace DashFire.Story.Commands
{
  /// <summary>
  /// objface(obj_id, dir);
  /// </summary>
  internal class ObjFaceCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjFaceCommand cmd = new ObjFaceCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_Dir = m_Dir.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_Dir.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_Dir.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      float dir = m_Dir.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        obj.GetMovementStateInfo().SetFaceDir(dir);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_Dir.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<float> m_Dir = new StoryValue<float>();
  }
  /// <summary>
  /// objmove(obj_id, vector3(x,y,z));
  /// </summary>
  internal class ObjMoveCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjMoveCommand cmd = new ObjMoveCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_Pos = m_Pos.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_Pos.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_Pos.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      Vector3 pos = m_Pos.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      UserInfo user = obj as UserInfo;
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
      } else {
        NpcInfo npc = obj as NpcInfo;
        if (null != npc) {
          List<Vector3> waypoints = npc.SpatialSystem.FindPath(npc.GetMovementStateInfo().GetPosition3D(), pos, 1);
          waypoints.Add(pos);
          NpcAiStateInfo aiInfo = npc.GetAiStateInfo();
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
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_Pos.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
  }
  /// <summary>
  /// objmovewithwaypoints(obj_id, vector2list("1 2 3 4 5 6 7"));
  /// </summary>
  internal class ObjMoveWithWaypointsCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjMoveWithWaypointsCommand cmd = new ObjMoveWithWaypointsCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_WayPoints = m_WayPoints.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_WayPoints.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_WayPoints.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      List<Vector2> poses = m_WayPoints.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      UserInfo user = obj as UserInfo;
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
      } else {
        NpcInfo npc = obj as NpcInfo;
        if (null != npc) {
          List<Vector3> waypoints = new List<Vector3>();
          foreach (Vector2 pt in poses) {
            waypoints.Add(new Vector3(pt.x, 0, pt.y));
          }
          NpcAiStateInfo aiInfo = npc.GetAiStateInfo();
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
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_WayPoints.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<List<Vector2>> m_WayPoints = new StoryValue<List<Vector2>>();
  }
  /// <summary>
  /// objstop(obj_id);
  /// </summary>
  internal class ObjStopCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjStopCommand cmd = new ObjStopCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      NpcInfo npc = obj as NpcInfo;
      if (null != npc) {
        NpcAiStateInfo aiInfo = npc.GetAiStateInfo();
        if (aiInfo.CurState == (int)AiStateId.MoveCommand || aiInfo.CurState == (int)AiStateId.PursuitCommand || aiInfo.CurState == (int)AiStateId.PatrolCommand) {
          aiInfo.Time = 0;
          aiInfo.Target = 0;
          aiInfo.AiDatas.RemoveData<AiData_ForMoveCommand>();
          aiInfo.AiDatas.RemoveData<AiData_ForPursuitCommand>();
          aiInfo.AiDatas.RemoveData<AiData_ForPatrolCommand>();
          aiInfo.ChangeToState((int)AiStateId.Idle);
        }
      } else {
        UserInfo user = obj as UserInfo;
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
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
  }
  /// <summary>
  /// objanimation(obj_id, anim_type);
  /// </summary>
  internal class ObjAnimationCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjAnimationCommand cmd = new ObjAnimationCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_AnimType = m_AnimType.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_AnimType.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_AnimType.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int animType = m_AnimType.Value;
      CharacterView view = EntityManager.Instance.GetCharacterViewById(objId);
      if (null != view) {
        view.PlayAnimation((Animation_Type)animType);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_AnimType.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_AnimType = new StoryValue<int>();
  }
  /// <summary>
  /// objpursuit(obj_id, target_obj_id);
  /// </summary>
  internal class ObjPursuitCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjPursuitCommand cmd = new ObjPursuitCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_TargetId = m_TargetId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_TargetId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_TargetId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int targetId = m_TargetId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      NpcInfo npc = obj as NpcInfo;
      if (null != npc) {
        NpcAiStateInfo aiInfo = npc.GetAiStateInfo();
        AiData_ForPursuitCommand data = aiInfo.AiDatas.GetData<AiData_ForPursuitCommand>();
        if (null == data) {
          data = new AiData_ForPursuitCommand();
          aiInfo.AiDatas.AddData(data);
        }
        aiInfo.Target = targetId;
        aiInfo.ChangeToState((int)AiStateId.PursuitCommand);
      } else {
        UserInfo user = obj as UserInfo;
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
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_TargetId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_TargetId = new StoryValue<int>();
  }
  /// <summary>
  /// objenableai(obj_id, true_or_false);
  /// </summary>
  internal class ObjEnableAiCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjEnableAiCommand cmd = new ObjEnableAiCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_Enable = m_Enable.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_Enable.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_Enable.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      string enable = m_Enable.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        obj.SetAIEnable(m_Enable.Value != "false");
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_Enable.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<string> m_Enable = new StoryValue<string>();
  }
  /// <summary>
  /// objsetai(objid,ai_logic_id,stringlist("param1 param2 param3 ..."));
  /// </summary>
  internal class ObjSetAiCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjSetAiCommand cmd = new ObjSetAiCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_AiLogic = m_AiLogic.Clone();
      cmd.m_AiParams = m_AiParams.Clone();
      return cmd;
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_AiLogic.Evaluate(iterator, args);
      m_AiParams.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_AiLogic.Evaluate(instance);
      m_AiParams.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int aiLogic = m_AiLogic.Value;
      IEnumerable aiParams = m_AiParams.Value;
      CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(objId);
      NpcInfo npc = charObj as NpcInfo;
      if (null != npc) {
        npc.GetAiStateInfo().Reset();
        npc.GetAiStateInfo().AiLogic = aiLogic;
        int ix = 0;
        foreach (string aiParam in aiParams) {
          if (ix < Data_Unit.c_MaxAiParamNum) {
            npc.GetAiStateInfo().AiParam[ix] = aiParam;
            ++ix;
          } else {
            break;
          }
        }
      } else {
        UserInfo user = charObj as UserInfo;
        user.GetAiStateInfo().Reset();
        user.GetAiStateInfo().AiLogic = aiLogic;
        int ix = 0;
        foreach (string aiParam in aiParams) {
          if (ix < Data_Unit.c_MaxAiParamNum) {
            user.GetAiStateInfo().AiParam[ix] = aiParam;
            ++ix;
          } else {
            break;
          }
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 2) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_AiLogic.InitFromDsl(callData.GetParam(1));
        m_AiParams.InitFromDsl(callData.GetParam(2));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_AiLogic = new StoryValue<int>();
    private IStoryValue<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
  }
  /// <summary>
  /// objaddimpact(obj_id, impactid);
  /// </summary>
  internal class ObjAddImpactCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjAddImpactCommand cmd = new ObjAddImpactCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_ImpactId = m_ImpactId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_ImpactId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_ImpactId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int impactId = m_ImpactId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        ImpactSystem.Instance.SendImpactToCharacter(obj, impactId, obj.GetId(), -1, -1, obj.GetMovementStateInfo().GetPosition3D(), obj.GetMovementStateInfo().GetFaceDir());
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_ImpactId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_ImpactId = new StoryValue<int>();
  }
  /// <summary>
  /// objremoveimpact(obj_id, impactid);
  /// </summary>
  internal class ObjRemoveImpactCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjRemoveImpactCommand cmd = new ObjRemoveImpactCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_ImpactId = m_ImpactId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_ImpactId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_ImpactId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int impactId = m_ImpactId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        ImpactSystem.Instance.StopImpactById(obj, impactId);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_ImpactId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_ImpactId = new StoryValue<int>();
  }
  /// <summary>
  /// objcastskill(obj_id, skillid);
  /// </summary>
  internal class ObjCastSkillCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjCastSkillCommand cmd = new ObjCastSkillCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_SkillId = m_SkillId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_SkillId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_SkillId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int skillId = m_SkillId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj && null!=obj.SkillController) {
        obj.SkillController.ForceStartSkill(skillId);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_SkillId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_SkillId = new StoryValue<int>();
  }
  /// <summary>
  /// objstopskill(obj_id);
  /// </summary>
  internal class ObjStopSkillCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjStopSkillCommand cmd = new ObjStopSkillCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj && null != obj.SkillController) {
        obj.SkillController.ForceInterruptCurSkill();
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 0) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_SkillId = new StoryValue<int>();
  }
  /// <summary>
  /// objaddskill(obj_id, skillid);
  /// </summary>
  internal class ObjAddSkillCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjAddSkillCommand cmd = new ObjAddSkillCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_SkillId = m_SkillId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_SkillId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_SkillId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int skillId = m_SkillId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        if(obj.GetSkillStateInfo().GetSkillInfoById(skillId)==null){
          obj.GetSkillStateInfo().AddSkill(new SkillInfo(skillId));
        }
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_SkillId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_SkillId = new StoryValue<int>();
  }
  /// <summary>
  /// objremoveskill(obj_id, skillid);
  /// </summary>
  internal class ObjRemoveSkillCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      ObjRemoveSkillCommand cmd = new ObjRemoveSkillCommand();
      cmd.m_ObjId = m_ObjId.Clone();
      cmd.m_SkillId = m_SkillId.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_ObjId.Evaluate(iterator, args);
      m_SkillId.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_SkillId.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      int objId = m_ObjId.Value;
      int skillId = m_SkillId.Value;
      CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
      if (null != obj) {
        obj.GetSkillStateInfo().RemoveSkill(skillId);
      }
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_SkillId.InitFromDsl(callData.GetParam(1));
      }
    }

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_SkillId = new StoryValue<int>();
  }
  /// <summary>
  /// setblockedshader(rimcolor1,rimpower1,rimcutvalue1,rimcolor2,rimpower2,rimcutvalue2);
  /// </summary>
  internal class SetBlockedShaderCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      SetBlockedShaderCommand cmd = new SetBlockedShaderCommand();
      cmd.m_RimColor1 = m_RimColor1.Clone();
      cmd.m_RimPower1 = m_RimPower1.Clone();
      cmd.m_RimCutValue1 = m_RimCutValue1.Clone();
      cmd.m_RimColor2 = m_RimColor2.Clone();
      cmd.m_RimPower2 = m_RimPower2.Clone();
      cmd.m_RimCutValue2 = m_RimCutValue2.Clone();
      return cmd;
    }

    protected override void ResetState()
    {
    }

    protected override void UpdateArguments(object iterator, object[] args)
    {
      m_RimColor1.Evaluate(iterator, args);
      m_RimPower1.Evaluate(iterator, args);
      m_RimCutValue1.Evaluate(iterator, args);
      m_RimColor2.Evaluate(iterator, args);
      m_RimPower2.Evaluate(iterator, args);
      m_RimCutValue2.Evaluate(iterator, args);
    }

    protected override void UpdateVariables(StoryInstance instance)
    {
      m_RimColor1.Evaluate(instance);
      m_RimPower1.Evaluate(instance);
      m_RimCutValue1.Evaluate(instance);
      m_RimColor2.Evaluate(instance);
      m_RimPower2.Evaluate(instance);
      m_RimCutValue2.Evaluate(instance);
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      uint rimColor1 = m_RimColor1.Value;
      float rimPower1 = m_RimPower1.Value;
      float rimCutValue1 = m_RimCutValue1.Value;
      uint rimColor2 = m_RimColor2.Value;
      float rimPower2 = m_RimPower2.Value;
      float rimCutValue2 = m_RimCutValue2.Value;
      WorldSystem.Instance.SetBlockedShader(rimColor1, rimPower1, rimCutValue1, rimColor2, rimPower2, rimCutValue2);
      return false;
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num > 5) {
        m_RimColor1.InitFromDsl(callData.GetParam(0));
        m_RimPower1.InitFromDsl(callData.GetParam(1));
        m_RimCutValue1.InitFromDsl(callData.GetParam(2));
        m_RimColor2.InitFromDsl(callData.GetParam(3));
        m_RimPower2.InitFromDsl(callData.GetParam(4));
        m_RimCutValue2.InitFromDsl(callData.GetParam(5));
      }
    }

    private IStoryValue<uint> m_RimColor1 = new StoryValue<uint>();
    private IStoryValue<float> m_RimPower1 = new StoryValue<float>();
    private IStoryValue<float> m_RimCutValue1 = new StoryValue<float>();
    private IStoryValue<uint> m_RimColor2 = new StoryValue<uint>();
    private IStoryValue<float> m_RimPower2 = new StoryValue<float>();
    private IStoryValue<float> m_RimCutValue2 = new StoryValue<float>();
  }
}
