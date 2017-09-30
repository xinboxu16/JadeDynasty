using DashFireSpatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public enum AiTargetType : int
    {
        USER = 0,
        NPC,
        ALL,
        TOWER,
        SOLDIER,
    }

    public sealed class AiLogicUtility
    {
        public const int c_MaxViewRange = 30;
        public const int c_MaxViewRangeSqr = c_MaxViewRange * c_MaxViewRange;

        private static float MoveDirection(Vector3 from, Vector3 to)
        {
            return (float)Math.Atan2(to.x - from.x, to.z - from.z);
        }

        #region npc motion
        
        //通用idle, 寻路 休闲  遇敌
        public static void CommonIdleHandler(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime, AbstractNpcStateLogic logic)
        {
            //是否技能激活
            if (npc.GetSkillStateInfo().IsSkillActivated()) return;
            if (npc.IsDead()) return;
            if (npc.GetSkillStateInfo().IsImpactControl())
            {
                // 被动进入战斗
            }

            NpcAiStateInfo info = npc.GetAiStateInfo();

            //执行时间 100ms一次
            info.Time += deltaTime;
            if(info.Time > 100)
            {
                //时间清0
                info.Time = 0; 
                //是否巡逻
                if(null != logic.GetAiPatrolData(npc))
                {
                    //转为巡逻状态
                    npc.GetMovementStateInfo().IsMoving = false;
                    logic.ChangeToState(npc, (int)AiStateId.Patrol);
                }
                else//攻击
                {
                    //使用kd树查找攻击圆半径内的敌人
                    CharacterInfo target = AiLogicUtility.GetNearstTargetHelper(npc, CharacterRelation.RELATION_ENEMY, AiTargetType.ALL);
                    if(null != target)
                    {
                        //更改攻击目标 原版就没有实现方法
                        info.Target = target.GetId();
                        logic.NotifyNpcTargetChange(npc);

                        //通知移动 原版就没有实现方法
                        npc.GetMovementStateInfo().IsMoving = false;
                        logic.NotifyNpcMove(npc);

                        //追赶目标
                        info.Time = 0;
                        logic.ChangeToState(npc, (int)AiStateId.Pursuit);
                    }
                }
            }
        }

        #endregion

        public static void DoMoveCommandState(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime, AbstractUserStateLogic logic)
        {
            //执行状态处理
            AiData_ForMoveCommand data = GetAiDataForMoveCommand(user);
            if (null == data) return;

            if(!data.IsFinish)
            {
                if(WayPointArrived(user, data))
                {
                    Vector3 targetPos = new Vector3();
                    MoveToNext(user, data, ref targetPos);
                    if(!data.IsFinish)
                    {
                        logic.NotifyUserMove(user);
                    }
                }
            }

            //判断是否状态结束并执行相应处理
            if(data.IsFinish)
            {
                if(GlobalVariables.Instance.IsClient)
                {
                    logic.UserSendStoryMessage(user, "playerselfarrived", user.GetId());
                }
                logic.UserSendStoryMessage(user, "objarrived", user.GetId());
                user.GetMovementStateInfo().IsMoving = false;
                logic.ChangeToState(user, (int)AiStateId.Idle);
            }
        }

        //移动到下一个位置
        private static void MoveToNext(CharacterInfo charObj, AiData_ForMoveCommand data, ref Vector3 targetPos)
        {
            if(++data.Index >= data.WayPoints.Count)
            {
                data.IsFinish = true;
                return;
            }

            var moveInfo = charObj.GetMovementStateInfo();
            Vector3 from = moveInfo.GetPosition3D();
            targetPos = data.WayPoints[data.Index];
            float moveDir = MoveDirection(from, targetPos);

            float now = TimeUtility.GetServerMilliseconds();
            float distance = Geometry.Distance(from, targetPos);
            float speed = charObj.GetActualProperty().MoveSpeed;
            data.EstimateFinishTime = now + 1000 * (distance / speed);

            moveInfo.IsMoving = true;
            moveInfo.SetMoveDir(moveDir);
            moveInfo.SetFaceDir(moveDir);
        }

        private static bool WayPointArrived(CharacterInfo charObj, AiData_ForMoveCommand data)
        {
            if (TimeUtility.GetServerMilliseconds() >= data.EstimateFinishTime)//估计完成时间
            {
                var moveInfo = charObj.GetMovementStateInfo();
                Vector3 to = data.WayPoints[data.Index];
                Vector3 now = moveInfo.GetPosition3D();
                float distance = Geometry.Distance(now, to);
                return true;
            }
            return false;
        }

        
        public static void PathToTargetWithoutObstacle(NpcInfo npc, AiPathData data, Vector3 pathTargetPos, long deltaTime, bool faceIsMoveFir, AbstractNpcStateLogic logic)
        {
            NpcAiStateInfo info = npc.GetAiStateInfo();
            Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
            if(null != data)
            {
                data.Clear();
                data.UpdateTime += deltaTime;
                Vector3 targetPos = pathTargetPos;//目标位置
                //是否可以通过
                bool canGo = true;
                //障碍躲避半径
                ICellMapView cellMapView = npc.SpatialSystem.GetCellMapView(npc.AvoidanceRadius);
                //不能通过障碍 获取障碍目标位置
                if(!cellMapView.CanPass(targetPos))
                {
                    //ref targetPos 获取障碍目标位置
                    if(!AiLogicUtility.GetWalkablePosition(cellMapView, targetPos, srcPos, ref targetPos))
                    {
                        //向前走两米目标位置
                        if (!AiLogicUtility.GetForwardTargetPos(npc, targetPos, 2.0f, ref targetPos))
                        {
                            canGo = false;
                        }
                    }
                }
                if(canGo)
                {
                    List<Vector3> posList = null;
                    //里面的看不懂
                    bool canPass = npc.SpatialSystem.CanPass(npc.SpaceObject, targetPos);
                    if(canPass)
                    {
                        posList = new List<Vector3>();
                        posList.Add(srcPos);
                        posList.Add(targetPos);
                    }
                    else
                    {
                        long stTime = TimeUtility.GetElapsedTimeUs();
                        posList = npc.SpatialSystem.FindPath(srcPos, targetPos, npc.AvoidanceRadius);
                        long endTime = TimeUtility.GetElapsedTimeUs();
                        long calcTime = endTime - stTime;
                        if (calcTime > 10000)
                        {
                            LogSystem.Warn("*** pve FindPath consume {0} us,npc:{1} from:{2} to:{3} radius:{4} pos:{5}", calcTime, npc.GetId(), srcPos.ToString(), targetPos.ToString(), npc.AvoidanceRadius, npc.GetMovementStateInfo().GetPosition3D().ToString());
                        }
                    }
                    if(posList.Count >= 2)
                    {
                        data.SetPathPoints(posList[0], posList, 1);
                    }
                    else
                    {
                        npc.GetMovementStateInfo().IsMoving = false;
                        logic.NotifyNpcMove(npc);
                        data.IsUsingAvoidanceVelocity = false;//使用回避速度
                    }

                    //沿路点列表移动的逻辑
                    bool havePathPoint = data.HavePathPoint;
                    if(havePathPoint)
                    {
                        targetPos = data.CurPathPoint;
                        //向指定路点移动（避让移动过程）是否已经到达
                        if (!data.IsReached(srcPos))
                        {
                            float angle = Geometry.GetYAngle(new Vector2(srcPos.x, srcPos.z), new Vector2(targetPos.x, targetPos.z));
                            Vector3 prefVelocity = (float)npc.GetActualProperty().MoveSpeed * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                            Vector3 v = new Vector3(targetPos.x - srcPos.x, 0, targetPos.z - srcPos.z);
                            //实现规范化，让一个向量保持相同的方向，但它的长度为1.0
                            v.Normalize();
                            //获取乘以速度系数的速度
                            Vector3 velocity = npc.SpaceObject.GetVelocity();
                            long stTime = TimeUtility.GetElapsedTimeUs();
                            //RVO算法 求避让速度
                            Vector3 newVelocity = npc.SpatialSystem.ComputeVelocity(npc.SpaceObject, v, (float)deltaTime / 1000, (float)npc.GetActualProperty().MoveSpeed, (float)npc.GetRadius(), data.IsUsingAvoidanceVelocity);
                            long endTime = TimeUtility.GetElapsedTimeUs();
                            long calcTime = endTime - stTime;
                            if (calcTime > 10000)
                            {
                                LogSystem.Warn("*** pve ComputeVelocity consume {0} us,npc:{1} velocity:{2} newVelocity:{3} deltaTime:{4} speed:{5} pos:{6}", calcTime, npc.GetId(), velocity.ToString(), newVelocity.ToString(), deltaTime, npc.GetActualProperty().MoveSpeed, npc.GetMovementStateInfo().GetPosition3D().ToString());
                            }

                            if(data.UpdateTime > 100)
                            {
                                data.UpdateTime = 0;
                                float newAngle = Geometry.GetYAngle(new Vector2(0, 0), new Vector2(newVelocity.x, newVelocity.z));
                                npc.GetMovementStateInfo().SetMoveDir(newAngle);
                                if(faceIsMoveFir)
                                {
                                    logic.NotifyNpcFace(npc, newAngle);
                                }
                                newVelocity.Normalize();
                                npc.GetMovementStateInfo().TargetPosition = srcPos + newVelocity * Geometry.Distance(srcPos, targetPos);
                                npc.GetMovementStateInfo().IsMoving = true;
                                logic.NotifyNpcMove(npc);
                            }
                            else
                            {
                                data.UpdateTime += deltaTime;
                            }
                        }
                        else//改变路点或结束沿路点移动
                        {
                            data.UseNextPathPoint();
                            if(data.HavePathPoint)
                            {
                                targetPos = data.CurPathPoint;
                                npc.GetMovementStateInfo().TargetPosition = targetPos;
                                float angle = Geometry.GetYAngle(new Vector2(srcPos.x, srcPos.z), new Vector2(targetPos.x, targetPos.z));
                                npc.GetMovementStateInfo().SetMoveDir(angle);
                                if (faceIsMoveFir)
                                {
                                    logic.NotifyNpcFace(npc, angle);
                                }
                                npc.GetMovementStateInfo().IsMoving = true;
                                logic.NotifyNpcMove(npc);
                                data.IsUsingAvoidanceVelocity = false;
                            }
                            else//没找到目标点或者到达目标
                            {
                                npc.GetMovementStateInfo().IsMoving = false;
                                data.Clear();
                            }
                        }
                    }
                }
                else
                {
                    npc.GetMovementStateInfo().IsMoving = false;
                    logic.NotifyNpcMove(npc);
                }
            }
        }
        


        private static AiData_ForMoveCommand GetAiDataForMoveCommand(UserInfo user)
        {
            AiData_ForMoveCommand data = user.GetAiStateInfo().AiDatas.GetData<AiData_ForMoveCommand>();
            return data;
        }

        public static CharacterInfo GetLivingCharacterInfoHelper(CharacterInfo srcObj, int id)
        {
            CharacterInfo target = srcObj.NpcManager.GetNpcInfo(id);
            if(null == target)
            {
                target = srcObj.UserManager.GetUserInfo(id);
            }
            if(target != null)
            {
                if(target.IsDead())
                {
                    target = null;
                }
            }
            return target;
        }

        public static CharacterInfo GetNearstTargetHelper(CharacterInfo srcObj, CharacterRelation relation)
        {
            return GetNearstTargetHelper(srcObj, relation, AiTargetType.ALL);
        }

        public static CharacterInfo GetNearstTargetHelper(CharacterInfo srcObj, CharacterRelation relation, AiTargetType type)
        {
            CharacterInfo nearstTarget = null;
            ISpatialSystem spatialSys = srcObj.SpatialSystem;//不懂
            if(null != spatialSys)
            {
                Vector3 srcPos = srcObj.GetMovementStateInfo().GetPosition3D();
                float minPowDist = 999999;
                //从树中查找
                spatialSys.VisitObjectInCircle(srcPos, srcObj.ViewRange, (float distSqr, ISpaceObject obj) =>
                {
                    StepCalcNearstTarget(srcObj, relation, type, distSqr, obj, ref minPowDist, ref nearstTarget);//TODO不懂
                });
            }
            return nearstTarget;
        }

        private static void StepCalcNearstTarget(CharacterInfo srcObj, CharacterRelation relation, AiTargetType type, float powDist, ISpaceObject obj, ref float minPowDist, ref CharacterInfo nearstTarget)
        {
            if (type == AiTargetType.USER && obj.GetObjType() != SpatialObjType.kUser) return;
            if (type == AiTargetType.NPC && obj.GetObjType() != SpatialObjType.kNPC) return;
            CharacterInfo target = GetSeeingLivingCharacterInfoHelper(srcObj, (int)obj.GetID());
            if (null != target && !target.IsDead())
            {
                if (target.IsControlMecha)
                {
                    return;
                }
                NpcInfo npcTarget = target.CastNpcInfo();
                if (null != npcTarget)
                {
                    if (npcTarget.NpcType == (int)NpcTypeEnum.Skill || npcTarget.NpcType == (int)NpcTypeEnum.AutoPickItem)
                    {
                        return;
                    }
                }
                if (relation == CharacterInfo.GetRelation(srcObj, target))
                {
                    if (powDist < minPowDist)
                    {
                        if (powDist > c_MaxViewRangeSqr || CanSee(srcObj, target))
                        {
                            nearstTarget = target;
                            minPowDist = powDist;
                        }
                    }
                }
            }
        }

        public static CharacterInfo GetSeeingLivingCharacterInfoHelper(CharacterInfo srcObj, int id)
        {
            CharacterInfo target = srcObj.NpcManager.GetNpcInfo(id);
            if (null == target)
            {
                target = srcObj.UserManager.GetUserInfo(id);
            }
            if (null != target)
            {
                if (target.IsHaveStateFlag(CharacterState_Type.CST_Hidden))
                    target = null;
                else if (target.IsDead())
                    target = null;
                else if (!CanSee(srcObj, target))
                    target = null;
            }
            return target;
        }

        public static bool GetForwardTargetPos(CharacterInfo character, Vector3 endPos, float dis, ref Vector3 pos)
        {
            Vector3 targetPos = Vector3.zero;
            Vector3 sourcePos = character.GetMovementStateInfo().GetPosition3D();
            float angle = Geometry.GetYAngle(character.GetMovementStateInfo().GetPosition2D(), new Vector2(endPos.x, endPos.z));
            targetPos.x = sourcePos.x + dis * (float)Math.Sin(angle);
            targetPos.y = sourcePos.y;
            targetPos.z = sourcePos.z + dis * (float)Math.Cos(angle);
            bool isFind = GetWalkablePosition(character.SpatialSystem.GetCellMapView(character.AvoidanceRadius), targetPos, sourcePos, ref pos);
            return isFind;
        }

        public static bool GetWalkablePosition(ICellMapView view, Vector3 targetPos, Vector3 srcPos, ref Vector3 pos)
        {
            bool ret = false;
            const int c_MaxCheckCells = 3;
            int row = 0;
            int col = 0;
            view.GetCell(targetPos, out row, out col);
            float radian = Geometry.GetYAngle(new Vector2(targetPos.x, targetPos.z), new Vector2(srcPos.x, srcPos.y));

            //右边 45度到135度
            if (radian >= Math.PI / 4 && radian < Math.PI * 3 / 4)
            {
                //3行3列
                for(int ci = 1; ci <= c_MaxCheckCells; ++ci)
                {
                    for(int ri = 0; ri <= c_MaxCheckCells; ++ri)
                    {
                        int row_ = row + ri;
                        int col_ = col + ci;
                        if(view.IsCellValid(row_, col_))
                        {
                            if(view.CanPass(row_, col_))
                            {
                                pos = view.GetCellCenter(row_, col_);
                                ret = true;
                                goto exit;
                            }
                        }
                        if(ri > 0)
                        {
                            row_ = row - ri;
                            if(view.IsCellValid(row_, col_))
                            {
                                if (view.CanPass(row_, col_))
                                {
                                    pos = view.GetCellCenter(row_, col_);
                                    ret = true;
                                    goto exit;
                                }
                            }
                        }
                    }
                }
            }
            else if (radian >= Math.PI * 3 / 4 && radian < Math.PI * 5 / 4)//下边 135度到225度
            {
                for (int ri = 1; ri <= c_MaxCheckCells; ++ri)
                {
                    for (int ci = 0; ci <= c_MaxCheckCells; ++ci)
                    {
                        int row_ = row - ri;
                        int col_ = col + ci;
                        if (view.IsCellValid(row_, col_))
                        {
                            if (view.CanPass(row_, col_))
                            {
                                pos = view.GetCellCenter(row_, col_);
                                ret = true;
                                goto exit;
                            }
                        }
                        if (ci > 0)
                        {
                            col_ = col - ci;
                            if (view.IsCellValid(row_, col_))
                            {
                                if (view.CanPass(row_, col_))
                                {
                                    pos = view.GetCellCenter(row_, col_);
                                    ret = true;
                                    goto exit;
                                }
                            }
                        }
                    }
                }
            }
            else if (radian >= Math.PI * 5 / 4 && radian < Math.PI * 7 / 4) //左边 225度到270度
            {
                for (int ci = 1; ci <= c_MaxCheckCells; ++ci)
                {
                    for (int ri = 0; ri <= c_MaxCheckCells; ++ri)
                    {
                        int row_ = row + ri;
                        int col_ = col - ci;
                        if (view.IsCellValid(row_, col_))
                        {
                            if (view.CanPass(row_, col_))
                            {
                                pos = view.GetCellCenter(row_, col_);
                                ret = true;
                                goto exit;
                            }
                        }
                        if (ri > 0)
                        {
                            row_ = row - ri;
                            if (view.IsCellValid(row_, col_))
                            {
                                if (view.CanPass(row_, col_))
                                {
                                    pos = view.GetCellCenter(row_, col_);
                                    ret = true;
                                    goto exit;
                                }
                            }
                        }
                    }
                }
            }
            else//上边
            {
                for (int ri = 1; ri <= c_MaxCheckCells; ++ri)
                {
                    for (int ci = 0; ci <= c_MaxCheckCells; ++ci)
                    {
                        int row_ = row + ri;
                        int col_ = col + ci;
                        if (view.IsCellValid(row_, col_))
                        {
                            if (view.CanPass(row_, col_))
                            {
                                pos = view.GetCellCenter(row_, col_);
                                ret = true;
                                goto exit;
                            }
                        }
                        if (ci > 0)
                        {
                            col_ = col - ci;
                            if (view.IsCellValid(row_, col_))
                            {
                                if (view.CanPass(row_, col_))
                                {
                                    pos = view.GetCellCenter(row_, col_);
                                    ret = true;
                                    goto exit;
                                }
                            }
                        }
                    }
                }
            }

        exit:
            return ret;
        }

        private static bool CanSee(CharacterInfo src, CharacterInfo target)
        {
            int srcCampId = src.GetCampId();
            int targetCampId = target.GetCampId();
            if (srcCampId == targetCampId)
                return true;
            else if (srcCampId == (int)CampIdEnum.Hostile || targetCampId == (int)CampIdEnum.Hostile)//Hostile有敌意的
            {
                return CharacterInfo.CanSee(src, target);
            }
            else
            {
                bool isBlue = (targetCampId == (int)CampIdEnum.Blue);
                if (isBlue && target.CurRedCanSeeMe || !isBlue && target.CurBlueCanSeeMe)
                    return true;
                else
                    return false;
            }
        }

        public static AiActionInfo CreateAiAction(int id)
        {
            AiActionInfo result = null;
            AiActionConfig config = AiActionConfigProvider.Instance.GetDataById(id);
            if(null != config)
            {
                result = new AiActionInfo(config);
            }
            else
            {
                LogSystem.Warn("CreateAiAction:: can't find AiActionConfig {0}", id);
            }
            return result;
        }

        //Patrol巡逻
        public static void InitPatrolData(NpcInfo npc, AbstractNpcStateLogic logic)
        {
            AiData_ForPatrol data = new AiData_ForPatrol();
            data.IsLoopPatrol = true;
            List<Vector3> path = new List<Vector3>();
            NpcAiStateInfo info = npc.GetAiStateInfo();
            path = Converter.ConvertVector3DList(info.AiParam[1]);
            path.Add(npc.GetAiStateInfo().HomePos);
            data.PatrolPath.SetPathPoints(npc.GetAiStateInfo().HomePos, path);
            npc.GetAiStateInfo().AiDatas.AddData<AiData_ForPatrol>(data);
        }

        public static bool IsSkillInRange(CharacterInfo character, int skillId, float dis)
        {
            SkillInfo skillInfo = character.GetSkillStateInfo().GetSkillInfoById(skillId);
            if(null != skillInfo)
            {
                if(skillInfo.ConfigData.SkillRangeMin < dis && skillInfo.ConfigData.SkillRangeMax > dis)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSkillInCD(CharacterInfo character, int skillId)
        {
            if (null != character.SkillController)
            {
                SkillInfo skillInfo = character.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo)
                {
                    long curTime = TimeUtility.GetServerMilliseconds();
                    if (!skillInfo.IsInCd(curTime / 1000.0f))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //是否在攻击范围
        public static bool CanCastSkill(CharacterInfo sender, int skillId, CharacterInfo target)
        {
            if(null != sender && null != target)
            {
                float dis = Geometry.Distance(sender.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                //在攻击范围和不是cd时间
                if(IsSkillInRange(sender, skillId, dis) && !IsSkillInCD(sender, skillId))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class AiLogic_User_Client : AbstractUserStateLogic
    {
        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Move, this.MoveHandler);
            SetStateHandler((int)AiStateId.Wait, this.WaitHandler);
        }

        protected override void OnStateLogicInit(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            UserAiStateInfo info = user.GetAiStateInfo();
            info.Time = 0;
            info.Target = 0;
        }

        private void IdleHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            UserAiStateInfo info = user.GetAiStateInfo();
            user.GetMovementStateInfo().IsMoving = false;
            ChangeToState(user, (int)AiStateId.Wait);
        }

        private void MoveHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            if(user.IsDead())
            {
                user.GetMovementStateInfo().IsMoving = false;
                ChangeToState(user, (int)AiStateId.Wait);
                return;
            }
            UserAiStateInfo info = user.GetAiStateInfo();
            info.Time += deltaTime;
            if(info.Time > 10)
            {
                info.Time = 0;
                Vector3 srcPos = user.GetMovementStateInfo().GetPosition3D();
                Vector3 targetPos = user.GetMovementStateInfo().TargetPosition;
                if(!IsReached(srcPos, targetPos))
                {
                    float angle = Geometry.GetYAngle(new Vector2(srcPos.x, srcPos.z), new Vector2(targetPos.x, targetPos.z));
                    user.GetMovementStateInfo().SetMoveDir(angle);
                    user.GetMovementStateInfo().IsMoving = true;
                }
            }
        }

        private void WaitHandler(UserInfo user, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
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
    }
}
