using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class AiLogic_UserSelf_General : AbstractUserStateLogic
    {
        //有父类构造函数调用
        protected override void OnInitStateHandlers()
        {
            //TODO 未实现
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Pursuit, this.PursuitHandler);
            SetStateHandler((int)AiStateId.Move, this.MoveHandler);
            //SetStateHandler((int)AiStateId.Combat, this.CombatHandler);
            SetStateHandler((int)AiStateId.MoveCommand, this.MoveCommandHandler);
            //SetStateHandler((int)AiStateId.PursuitCommand, this.PursuitCommandHandler);
        }

        protected override void OnStateLogicInit(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            UserAiStateInfo info = user.GetAiStateInfo();
            info.HomePos = user.GetMovementStateInfo().GetPosition3D();
            info.Time = 0;
            user.GetMovementStateInfo().IsMoving = false;
        }

        private void IdleHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
        }

        private void MoveCommandHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            AiLogicUtility.DoMoveCommandState(user, aiCmdDispatcher, deltaTime, this);
        }

        private void MoveHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            if(user.IsDead())
            {
                return;
            }
            UserAiStateInfo info = user.GetAiStateInfo();
            AiData_UserSelf_General data = GetAiData(user);
            Vector3 targetPos = info.TargetPos;
            Vector3 srcPos = user.GetMovementStateInfo().GetPosition3D();

            if(null != data && !IsReached(srcPos, targetPos))
            {
                if(info.IsTargetPosChanged)
                {
                    info.IsTargetPosChanged = false;
                    data.FoundPath.Clear();
                }
                //TODO未实现
                //PathToTarget(user, data.FoundPath, targetPos, deltaTime);
            }
            else
            {
                user.GetMovementStateInfo().StopMove();
                NotifyUserMove(user);
                info.Time = 0;
                data.Time = 0;
                data.FoundPath.Clear();
                ChangeToState(user, (int)AiStateId.Idle);
            }
        }

        //Pursuit追求
        private void PursuitHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            if (user.IsDead())
                return;
            UserAiStateInfo info = user.GetAiStateInfo();
            AiData_UserSelf_General data = GetAiData(user);

             if (null != data)
             {
                 if(info.Target > 0)
                 {
                     CharacterInfo target = AiLogicUtility.GetLivingCharacterInfoHelper(user, info.Target);
                     if(null != target)
                     {
                         float dist = info.AttackRange - 1.0f;
                         Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                         Vector3 srcPos = user.GetMovementStateInfo().GetPosition3D();
                         float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                         if (powDist < Math.Pow(dist, 2.0))
                         {
                             user.GetMovementStateInfo().IsMoving = false;
                             info.Time = 0;
                             data.Time = 0;
                             ChangeToState(user, (int)AiStateId.Combat);
                             NotifyUserMove(user);
                         }
                         else
                         {
                             info.Time += deltaTime;
                             if (info.Time > 100)
                             {
                                 info.Time = 0;
                                 CharacterInfo target2 = GetCanAttackUserTarget(user);//是否可攻击
                                 if (null == target2)
                                 {
                                     AiLogicUtility.GetNearstTargetHelper(user, CharacterRelation.RELATION_ENEMY);
                                     if(null == target2 || target == target2)
                                     {
                                         //TODO未实现
                                         //PathToTarget(user, data.FoundPath, targetPos, deltaTime);
                                     }
                                     else
                                     {
                                         info.Target = target2.GetId();
                                         return;
                                     }
                                 }
                             }
                         }
                     }
                     else
                     {
                         user.GetMovementStateInfo().StopMove();
                         NotifyUserMove(user);
                         info.Time = 0;
                         data.Time = 0;
                         data.FoundPath.Clear();
                         ChangeToState(user, (int)AiStateId.Idle);
                     }
                 }
                 else
                 {
                     float dist = info.AttackRange;
                     Vector3 targetPos = info.TargetPos;
                     Vector3 srcPos = user.GetMovementStateInfo().GetPosition3D();
                     float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                     if (powDist < dist * dist)
                     {
                         user.GetMovementStateInfo().IsMoving = false;
                         info.Time = 0;
                         data.Time = 0;
                         ChangeToState(user, (int)AiStateId.Combat);
                         NotifyUserMove(user);
                     }
                     else
                     {
                         info.Time += deltaTime;
                         if (info.Time > 100)
                         {
                             info.Time = 0;
                             //TODO未实现
                             //PathToTarget(user, data.FoundPath, targetPos, deltaTime);
                         }
                     }
                 }
             }
        }

        private bool IsReached(Vector3 src, Vector3 target)
        {
            bool ret = false;
            float powDist = Geometry.DistanceSquare(src, target);
            if (powDist <= 0.25f)
            {
                ret = true;
            }
            return ret;
        }

        private AiData_UserSelf_General GetAiData(UserInfo user)
        {
            AiData_UserSelf_General data = user.GetAiStateInfo().AiDatas.GetData<AiData_UserSelf_General>();
            if(null ==data)
            {
                data = new AiData_UserSelf_General();
                user.GetAiStateInfo().AiDatas.AddData<AiData_UserSelf_General>(data);
            }
            return data;
        }

        private CharacterInfo GetCanAttackUserTarget(UserInfo user)
        {
            float dist = 3.0f;
            LinkedListDictionary<int, UserInfo> list = user.SceneContext.UserManager.Users;
            for(LinkedListNode<UserInfo> node = list.FirstValue; null != node; node = node.Next)
            {
                UserInfo other = node.Value;
                if(null != other && CharacterRelation.RELATION_ENEMY == CharacterInfo.GetRelation(user, other))
                {
                    if(Geometry.DistanceSquare(user.GetMovementStateInfo().GetPosition3D(), other.GetMovementStateInfo().GetPosition3D()) < dist * dist)
                    {
                        return other;
                    }
                }
            }
            return null;
        }
    }
}
