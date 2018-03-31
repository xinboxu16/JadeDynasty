using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class AiLogic_Npc_CommonMelee : AbstractNpcStateLogic
    {
        private const long m_IntervalTime = 100;
        private const float m_AttackRange = 8.0f;
        private const long m_ChaseWalkMaxTime = 1000;
        private const long m_TauntTime = 3000;
        private const long m_ChaseStandMaxTime = 1000;

        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Pursuit, this.PursuitHandler);
            //SetStateHandler((int)AiStateId.Patrol, this.PatrolHandler);
            //SetStateHandler((int)AiStateId.MoveCommand, this.MoveCommandHandler);
            //SetStateHandler((int)AiStateId.PursuitCommand, this.PursuitCommandHandler);
            //SetStateHandler((int)AiStateId.PatrolCommand, this.PatrolCommandHandler);
        }
        
        //等待
        private void IdleHandler(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            AiLogicUtility.CommonIdleHandler(npc, aiCmdDispatcher, deltaTime, this);
        }

        
        //追赶
        private void PursuitHandler(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            //是否死亡或者释放技能
            if(!CanAiControl(npc))
            {
                npc.GetMovementStateInfo().IsMoving = false;
                return;
            }
            NpcAiStateInfo info = npc.GetAiStateInfo();
            AiData_Npc_CommonMelee data = GetAiData(npc);
            if(null != data)
            {
                //获取目标是否死亡
                CharacterInfo target = AiLogicUtility.GetLivingCharacterInfoHelper(npc, info.Target);
                if(null != target)
                {
                    //攻击范围
                    float dist = (float)npc.GetActualProperty().AttackRange;
                    //回家范围
                    float distGoHome = (float)npc.GohomeRange;
                    //目标位置
                    Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                    //攻击方位置
                    Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
                    //两方距离
                    float powDist = Geometry.DistanceSquare(srcPos, targetPos);
                    //离出生点距离
                    float powDistToHome = Geometry.DistanceSquare(srcPos, info.HomePos);
                    // 遇敌是播放特效， 逗留两秒。
                    if(data.WaitTime <= npc.MeetEnemyStayTime)
                    {
                        if(!data.HasMeetEnemy)
                        {
                            //通知遇到敌人 AiViewManager:OnNpcMeetEnemy 播放一个叹号动画
                            NotifyNpcMeetEnemy(npc, Animation_Type.AT_Attack);
                            data.HasMeetEnemy = true;
                        }
                        //尝试看向目标
                        TrySeeTarget(npc, target);
                        data.WaitTime += deltaTime;
                        return;
                    }

                    // 走向目标1.5秒
                    if (data.MeetEnemyWalkTime < npc.MeetEnemyWalkTime)
                    {
                        data.MeetEnemyWalkTime += deltaTime;
                        //设置移动速度
                        NotifyNpcWalk(npc);
                        info.Time += deltaTime;
                        if (info.Time > m_IntervalTime)
                        {
                            //无障碍路径到目标
                            info.Time = 0;
                            AiLogicUtility.PathToTargetWithoutObstacle(npc, data.FoundPath, targetPos, m_IntervalTime, true, this);
                        }
                        return;
                    }

                    // 大于攻击距离 跑向目标
                    if(powDist > m_AttackRange * m_AttackRange && 0 == data.CurAiAction)
                    {
                        npc.IsTaunt = false;
                        NotifyNpcRun(npc);
                        info.Time += deltaTime;
                        if (info.Time > m_IntervalTime)
                        {
                            info.Time = 0;
                            AiLogicUtility.PathToTargetWithoutObstacle(npc, data.FoundPath, targetPos, m_IntervalTime, true, this);
                        }
                    }
                    else
                    {
                        //小于攻击距离
                        if (data.CurAiAction == (int)AiAction.NONE)
                        {
                            npc.GetMovementStateInfo().IsMoving = false;
                            NotifyNpcMove(npc);
                            //切到下一个动作 随机
                            data.CurAiAction = (int)GetNextAction();
                        }
                        switch(data.CurAiAction)
                        {
                            case (int)AiAction.SKILL:
                                if(0 >= data.SkillToCast)
                                {
                                    data.SkillToCast = GetCanCastSkillId(npc);
                                }
                                //如果没有技能或者技能不在攻击范围内
                                if (0 >= data.SkillToCast || !AiLogicUtility.CanCastSkill(npc, data.SkillToCast, target))
                                {
                                    info.Time += deltaTime;
                                    if(info.Time > m_IntervalTime)
                                    {
                                        info.Time = 0;
                                        NotifyNpcRun(npc);
                                        AiLogicUtility.PathToTargetWithoutObstacle(npc, data.FoundPath, targetPos, m_IntervalTime, true, this);
                                    }
                                }
                                else
                                {
                                    //尝试触发技能
                                    npc.GetMovementStateInfo().IsMoving = false;
                                    NotifyNpcMove(npc);
                                    //尝试触发技能
                                    if(TryCastSkill(npc, data.SkillToCast, target))
                                    {
                                        data.CurAiAction = 0;
                                        data.SkillToCast = -1;
                                    }
                                }
                                break;
                            case (int)AiAction.STAND:
                                data.ChaseStandTime += deltaTime;
                                TrySeeTarget(npc, target);
                                if(data.ChaseStandTime > m_ChaseWalkMaxTime)
                                {
                                    data.ChaseStandTime = 0;
                                    data.CurAiAction = 0;
                                }
                                break;
                            case (int)AiAction.TAUNT:
                                npc.GetMovementStateInfo().IsMoving = false;
                                NotifyNpcMove(npc);
                                npc.IsTaunt = true;
                                data.TauntTime += deltaTime;
                                TrySeeTarget(npc, target);
                                if (data.TauntTime > m_TauntTime)
                                {
                                    npc.IsTaunt = false;
                                    data.TauntTime = 0;
                                    data.CurAiAction = 0;
                                }
                                break;
                            case (int)AiAction.WALK:
                                data.ChaseWalkTime += deltaTime;
                                info.Time += deltaTime;
                                if (info.Time > m_IntervalTime) 
                                {
                                  info.Time = 0;
                                  NotifyNpcWalk(npc);
                                  AiLogicUtility.PathToTargetWithoutObstacle(npc, data.FoundPath, targetPos, m_IntervalTime, true, this);
                                }
                                if (data.ChaseWalkTime > m_ChaseStandMaxTime)
                                {
                                    data.ChaseWalkTime = 0;
                                    data.CurAiAction = 0;
                                }
                                break;
                        }
                    }

                }
            }
        }

        private int GetCanCastSkillId(NpcInfo npc)
        {
            List<SkillInfo> skills = npc.GetSkillStateInfo().GetAllSkill();
            List<int> canCastSkills = new List<int>();
            foreach(SkillInfo skill in skills)
            {
                if(skill.ConfigData.SkillRangeMin >= 0 && skill.ConfigData.SkillRangeMax >= 0)
                {
                    canCastSkills.Add(skill.SkillId);
                }
            }

            if(canCastSkills.Count > 0)
            {
                return canCastSkills[Helper.Random.Next(canCastSkills.Count)];
            }
            else
            {
                return -1;
            }
        }

        private AiAction GetNextAction()
        {
            int roll = Helper.Random.Next(100);
            if (roll <= 20)
            {
                return AiAction.WALK;
            }
            else if (roll <= 30)
            {
                return AiAction.TAUNT;
            }
            else if (roll <= 85)
            {
                return AiAction.SKILL;
            }
            else
            {
                return AiAction.STAND;
            }
        }

        private AiData_Npc_CommonMelee  GetAiData(NpcInfo npc)
        {
            AiData_Npc_CommonMelee data = npc.GetAiStateInfo().AiDatas.GetData<AiData_Npc_CommonMelee>();
            if(null == data)
            {
                data = new AiData_Npc_CommonMelee();
                npc.GetAiStateInfo().AiDatas.AddData(data);
            }
            return data;
        }
    }
}
