using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public delegate void UserMoveDelegation(UserInfo user);
    public delegate void UserSendStoryMessageDelegation(UserInfo npc, string msgId, object[] args);

    public delegate void NpcTargetDelegation(NpcInfo npc);
    public delegate void NpcMoveDelegation(NpcInfo npc);
    public delegate void NpcMeetEnemy(NpcInfo npc, Animation_Type animType);
    public delegate void NpcFaceDelegation(NpcInfo npc);
    public delegate void NpcFaceClientDelegation(NpcInfo npc, float faceDirection);
    public delegate void NpcSkillDelegation(NpcInfo npc, int skillId);

    public enum AiStateLogicId : int
    {
        Invalid = 0,
        Npc_General = 1,
        Npc_OneSkill = 2,
        DropOut_AutoPick = 1001,
        Npc_Bluelf01 = 10110,
        Npc_Bluelf02 = 10120,
        Npc_Bluelf03 = 10130,
        Npc_BluelfBoss = 10150,
        Npc_Blof01 = 10210,
        Npc_Blof02 = 10220,
        Npc_CommonMelee = 20001,
        Npc_CommonRange = 20002,
        Npc_CommonLittleBoss = 20003,
        Npc_LittleBossWithSuperArmor = 20004,
        UserMirror_General = 30001,
        Npc_SmallMouse = 10530,
        Npc_BossDevilWarrior = 50110,
        Npc_BossCommon = 50121,
        Npc_BossXiLie = 50120,
        Npc_BossHulun = 50130,



        PvpUser_General = 10000,
        UserSelf_General = 10001,
        UserSelfRange_General = 11000,
        MaxNum
    }

    public interface IUserStateLogic
    {
        void Execute(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime);
    }

    public delegate void UserAiStateHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime);

    public abstract class AbstractUserStateLogic : IUserStateLogic
    {
        public static UserMoveDelegation OnUserMove;
        public static UserSendStoryMessageDelegation OnUserSendStoryMessage;

        private Dictionary<int, UserAiStateHandler> m_Handlers = new Dictionary<int, UserAiStateHandler>();

        public AbstractUserStateLogic()
        {
            OnInitStateHandlers();
        }

        public void Execute(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            if(user.GetAIEnable())
            {
                UserAiStateInfo userAi = user.GetAiStateInfo();
                if(userAi.CommandQueue.Count <= 0)
                {
                    int curState = userAi.CurState;
                    if (curState > (int)AiStateId.Invalid && curState < (int)AiStateId.MaxNum)
                    {
                        if (curState > (int)AiStateId.Idle)
                        {
                            Debug.Log("ailogic Execute" + curState);
                        }
                        if(m_Handlers.ContainsKey(curState))
                        {
                            UserAiStateHandler handler = m_Handlers[curState];
                            if(null != handler)
                            {
                                handler(user, aiCmdDispatcher, deltaTime);
                            }
                        }
                        else
                        {
                            LogSystem.Error("Illegal ai state: " + curState + " user:" + user.GetId());
                        }
                    }
                    else
                    {
                        OnStateLogicInit(user, aiCmdDispatcher, deltaTime);
                        ChangeToState(user, (int)AiStateId.Idle);
                    }
                }
                ExecuteCommandQueue(user, deltaTime);
            }
        }

        public void ChangeToState(UserInfo user, int state)
        {
            user.GetAiStateInfo().ChangeToState(state);
        }

        private void ExecuteCommandQueue(UserInfo user, long deltaTime)
        {
            UserAiStateInfo userAi = user.GetAiStateInfo();
            while(userAi.CommandQueue.Count > 0)
            {
                IAiCommand cmd = userAi.CommandQueue.Peek();
                if(cmd.Execute(deltaTime))
                {
                    userAi.CommandQueue.Dequeue();
                }
                else
                {
                    break;
                }
            }
        }

        protected void SetStateHandler(int state, UserAiStateHandler handler)
        {
            if(state > (int)AiStateId.Invalid && state < (int)AiStateId.MaxNum)
            {
                if(null != handler)
                {
                    if(m_Handlers.ContainsKey(state))
                    {
                        m_Handlers[state] = handler;
                    }
                    else
                    {
                        m_Handlers.Add(state, handler);
                    }
                }
                else
                {
                    m_Handlers.Remove(state);
                }
            }
        }

        public void NotifyUserMove(UserInfo user)
        {
            if (null != OnUserMove)
                OnUserMove(user);
        }

        public void UserSendStoryMessage(UserInfo user, string msgId, params object[] args)
        {
            if(null != OnUserSendStoryMessage)
            {
                OnUserSendStoryMessage(user, msgId, args);
            }
        }

        protected abstract void OnInitStateHandlers();
        protected abstract void OnStateLogicInit(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime);
    }




    #region npc

    public delegate void NpcAiStateHandler(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime);

    public interface INpcStateLogic
    {
        void Execute(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime);
    }

    public abstract class AbstractNpcStateLogic:INpcStateLogic
    {
        public enum AiAction
        {
            NONE = 0,
            STAND,
            WALK,
            TAUNT,
            SKILL,
        }

        public static NpcTargetDelegation OnNpcTargetChange;
        public static NpcMoveDelegation OnNpcMove;
        public static NpcMeetEnemy OnNpcMeetEnemy;
        public static NpcFaceDelegation OnNpcFace;
        public static NpcFaceClientDelegation OnNpcFaceClient;
        public static NpcSkillDelegation OnNpcSkill;

        protected abstract void OnInitStateHandlers();
        private Dictionary<int, NpcAiStateHandler> m_Handlers = new Dictionary<int, NpcAiStateHandler>();

        public AbstractNpcStateLogic()
        {
            OnInitStateHandlers();
        }

        public void Execute(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            if(npc.GetAIEnable())
            {
                NpcAiStateInfo npcAi = npc.GetAiStateInfo();
                if(npcAi.CommandQueue.Count <= 0)
                {
                    int curState = npcAi.CurState;
                    if(curState > (int)AiStateId.Invalid && curState < (int)AiStateId.MaxNum)
                    {
                        if(m_Handlers.ContainsKey(curState))
                        {
                            NpcAiStateHandler handler = m_Handlers[curState];
                            if(null != handler)
                            {
                                handler(npc, aiCmdDispatcher, deltaTime);
                            }
                            else
                            {
                                LogSystem.Error("Illegal ai state: " + curState + " npc:" + npc.GetId());
                            }
                        }
                    }
                    else
                    {
                        OnStateLogicInit(npc, aiCmdDispatcher, deltaTime);
                        ChangeToState(npc, (int)AiStateId.Idle);
                    }
                }
                ExecuteCommandQueue(npc, deltaTime);
            }
        }

        public void ChangeToState(NpcInfo npc, int state)
        {
            npc.GetAiStateInfo().ChangeToState(state);
        }

        protected virtual void OnStateLogicInit(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            NpcAiStateInfo info = npc.GetAiStateInfo();
            info.Time = 0;
            npc.GetMovementStateInfo().IsMoving = false;
            info.HomePos = npc.GetMovementStateInfo().GetPosition3D();
            info.Target = 0;
            foreach(int actionId in info.AiActions)
            {
                AiActionInfo action = AiLogicUtility.CreateAiAction(actionId);
                if(null != action)
                {
                    info.ActionInfos.Add(action);
                }
            }

            if(!string.IsNullOrEmpty(info.AiParam[0]))
            {
                if(int.Parse(info.AiParam[0]) == 1)
                {
                    AiLogicUtility.InitPatrolData(npc, this);
                }
            }
        }

        private void ExecuteCommandQueue(NpcInfo npc, long deltaTime)
        {
            NpcAiStateInfo npcAi = npc.GetAiStateInfo();
            while (npcAi.CommandQueue.Count > 0)
            {
                IAiCommand cmd = npcAi.CommandQueue.Peek();
                if (cmd.Execute(deltaTime))
                {
                    npcAi.CommandQueue.Dequeue();
                }
                else
                {
                    break;
                }
            }
        }

        protected void SetStateHandler(int state, NpcAiStateHandler handler)
        {
            if (state > (int)AiStateId.Invalid && state < (int)AiStateId.MaxNum)
            {
                if (null != handler)
                {
                    if (m_Handlers.ContainsKey(state))
                        m_Handlers[state] = handler;
                    else
                        m_Handlers.Add(state, handler);
                }
                else
                {
                    m_Handlers.Remove(state);
                }
            }
        }

        //获取巡逻数据
        public AiData_ForPatrol GetAiPatrolData(NpcInfo npc)
        {
            AiData_ForPatrol data = npc.GetAiStateInfo().AiDatas.GetData<AiData_ForPatrol>();
            return data;
        }

        //是否可以ai控制
        protected bool CanAiControl(NpcInfo npc)
        {
            if(npc.IsDead() || npc.GetSkillStateInfo().IsSkillActivated() || npc.GetSkillStateInfo().IsImpactControl())
            {
                return false;
            }
            return true;
        }

        //看向目标
        public bool TrySeeTarget(NpcInfo npc, CharacterInfo target)
        {
            float angle = Geometry.GetYAngle(npc.GetMovementStateInfo().GetPosition2D(), target.GetMovementStateInfo().GetPosition2D());
            NotifyNpcFace(npc, angle);
            if(Math.Abs(npc.GetMovementStateInfo().GetFaceDir() - angle) < 0.1)
            {
                return true;
            }
            return false;
        }

        public bool TryCastSkill(NpcInfo npc, int skillId, CharacterInfo target, bool needFaceToTarget = true)
        {
            if(AiLogicUtility.CanCastSkill(npc, skillId, target))
            {
                float angle = Geometry.GetYAngle(npc.GetMovementStateInfo().GetPosition2D(), target.GetMovementStateInfo().GetPosition2D());
                NotifyNpcFace(npc, angle);
                if(!needFaceToTarget || Math.Abs(npc.GetMovementStateInfo().GetFaceDir() - angle) < 0.1)
                {
                    NotifyNpcSkill(npc, skillId);
                    return true;
                }
            }
            return false;
        }

        //更改攻击目标
        public void NotifyNpcTargetChange(NpcInfo npc)
        {
            if(null != OnNpcTargetChange)
            {
                OnNpcTargetChange(npc);
            }
        }

        public void NotifyNpcMove(NpcInfo npc)
        {
            if (null != OnNpcMove)
                OnNpcMove(npc);
        }

        public void NotifyNpcMeetEnemy(NpcInfo npc, Animation_Type type)
        {
            if (null != OnNpcMeetEnemy)
            {
                OnNpcMeetEnemy(npc, type);
            }
        }

        public void NotifyNpcFace(NpcInfo npc, float faceDirection = 0.0f)
        {
            if(null != OnNpcFaceClient)
            {
                OnNpcFaceClient(npc, faceDirection);
            }
            if (null != OnNpcFace)
            {
                OnNpcFace(npc);
            } 
        }

        public void NotifyNpcWalk(NpcInfo npc)
        {
            npc.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, npc.GetBaseProperty().WalkSpeed);
            npc.GetMovementStateInfo().MovementMode = MovementMode.LowSpeed;
            if (null != OnNpcMove)
                OnNpcMove(npc);
        }

        public void NotifyNpcRun(NpcInfo npc)
        {
            npc.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, npc.GetBaseProperty().RunSpeed);
            npc.GetMovementStateInfo().MovementMode = MovementMode.HighSpeed;
            if (null != OnNpcMove)
                OnNpcMove(npc);
        }

        public void NotifyNpcSkill(NpcInfo npc, int skillId)
        {
            if (null != OnNpcSkill)
                OnNpcSkill(npc, skillId);
        }
    }

    #endregion
}


