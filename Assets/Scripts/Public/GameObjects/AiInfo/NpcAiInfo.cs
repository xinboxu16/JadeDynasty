using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class AiData_ForPatrol
    {
        private bool m_IsLoopPatrol = false;
        private AiPathData m_PatrolPath = new AiPathData();
        private AiPathData m_FoundPath = new AiPathData();

        public bool IsLoopPatrol
        {
            get { return m_IsLoopPatrol; }
            set { m_IsLoopPatrol = value; }
        }

        public AiPathData PatrolPath
        {
            get
            {
                return m_PatrolPath;
            }
        }

        public AiPathData FoundPath
        {
            get { return m_FoundPath; }
        }
    }

    public class AiData_ForMoveCommand
    {
        public List<Vector3> WayPoints { get; set; }
        public int Index { get; set; }
        public bool IsFinish { get; set; }
        public float EstimateFinishTime { get; set; }//估计完成时间

        public AiData_ForMoveCommand(List<Vector3> way_points)
        {
            WayPoints = way_points;
            Index = 0;
            EstimateFinishTime = 0;
            IsFinish = false;
        }
    }

    public class AiData_ForPursuitCommand
    {
        private AiPathData m_FoundPath = new AiPathData();

        public AiPathData FoundPath
        {
            get { return m_FoundPath; }
        }
    }

    public class AiData_ForPatrolCommand
    {
        private AiPathData m_PatrolPath = new AiPathData();
        private AiPathData m_FoundPath = new AiPathData();
        private bool m_IsLoopPatrol = false;

        public bool IsLoopPatrol
        {
            get { return m_IsLoopPatrol; }
            set { m_IsLoopPatrol = value; }
        }
        public DashFire.AiPathData PatrolPath
        {
            get { return m_PatrolPath; }
        }
        public DashFire.AiPathData FoundPath
        {
            get { return m_FoundPath; }
        }
    }

    public class AiData_PveNpc_General
    {
        public long Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }
        public long FindPathTime
        {
            get { return m_FindPathTime; }
            set { m_FindPathTime = value; }
        }
        public DashFire.AiPathData FoundPath
        {
            get { return m_FoundPath; }
        }

        private long m_Time = 0;
        private long m_FindPathTime = 0;
        private AiPathData m_FoundPath = new AiPathData();
    }

    public class AiData_Npc_CommonMelee : AiData_PveNpc_General
    {
        private long m_WaitTime = 0;
        private bool m_HasMeetEnemy = false;
        private long m_MeetEnemyWalkTime = 0;
        private long m_ChaseStandTime = 0;
        private long m_TauntTime = 0;
        private long m_ChaseWalkTime = 0;
        private int m_CurAiAction = 0;
        private int m_SkillToCast = 0;

        public long WaitTime
        {
            get { return m_WaitTime; }
            set { m_WaitTime = value; }
        }

        public bool HasMeetEnemy
        {
            get { return m_HasMeetEnemy; }
            set { m_HasMeetEnemy = value; }
        }

        public long MeetEnemyWalkTime
        {
            get { return m_MeetEnemyWalkTime; }
            set { m_MeetEnemyWalkTime = value; }
        }

        public long ChaseStandTime
        {
            get { return m_ChaseStandTime; }
            set { m_ChaseStandTime = value; }
        }

        public long TauntTime
        {
            get { return m_TauntTime; }
            set { m_TauntTime = value; }
        }

        public long ChaseWalkTime
        {
            get { return m_ChaseWalkTime; }
            set { m_ChaseWalkTime = value; }
        }

        public int CurAiAction
        {
            get { return m_CurAiAction; }
            set { m_CurAiAction = value; }
        }

        public int SkillToCast
        {
            get { return m_SkillToCast; }
            set { m_SkillToCast = value; }
        }
    }
}
