using DashFireSpatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public enum NpcTypeEnum
    {
        Normal = 0,
        Skill,
        Mecha,
        Horse,
        InteractiveNpc,
        PvpTower,
        AutoPickItem,
        Task,
        BigBoss,
        LittleBoss,
        SceneObject,
        Friend,
    }

    public class NpcInfo : CharacterInfo
    {
        private int m_NpcType = 0;
        private float m_Scale = 1.0f;
        private bool m_CanMove = true;
        private bool m_CanRotate = true;
        private bool m_CanHitMove = true;

        private bool m_NeedDelete = false;

        private int m_DropMoney = 0;
        private int m_SummonOwnerId = -1;
        private bool m_IsSimulateMove = false;
        private int m_CreatorId = 0;

        private long m_LifeEndTime = -1;
        private long m_BornTime = 0;
        private int m_BornAnimTimeMs = 0;
        private bool m_IsAttachControler = false;
        private string m_AttachNodeName = "";
        private long m_MeetEnemyStayTime = 0;
        private long m_MeetEnemyWalkTime = 0;
        private bool m_IsTaunt = false;

        private NpcAiStateInfo m_AiStateInfo = new NpcAiStateInfo();

        public NpcInfo(int id):base(id)
        {
            m_SpaceObject = new SpaceObjectImpl(this, SpatialObjType.kNPC);
            m_CastNpcInfo = this;
        }

        public void InitId(int id)
        {
            m_Id = id;
        }

        public void LoadData(Data_Unit unit)
        {
            SetUnitId(unit.m_Id);
            SetCampId(unit.m_CampId);//设置阵营 用于敌对关系
            GetAiStateInfo().AiLogic = unit.m_AiLogic;
            for(int i = 0; i < Data_Unit.c_MaxAiParamNum; ++i)
            {
                GetAiStateInfo().AiParam[i] = unit.m_AiParam[i];
            }
            if (null != unit.m_AiActions)
            {
                for (int i = 0; i < unit.m_AiActions.Count; ++i)
                {
                    GetAiStateInfo().AiActions.Add(unit.m_AiActions[i]);
                }
            }
            
            GetMovementStateInfo().SetPosition(unit.m_Pos);
            GetMovementStateInfo().SetFaceDir(unit.m_RotAngle);
            LoadData(unit.m_LinkId);
        }

        public void LoadData(int resId)
        {
            SetLinkId(resId);
            m_LevelupConfig = NpcConfigProvider.Instance.GetNpcLevelupConfigById(resId);
            Data_NpcConfig npcCfg = NpcConfigProvider.Instance.GetNpcConfigById(resId);
            if(null != npcCfg)
            {
                m_NpcType = npcCfg.m_NpcType;
                switch(m_NpcType)
                {
                    case (int)NpcTypeEnum.Mecha:
                        m_IsMecha = true;
                        break;
                    case (int)NpcTypeEnum.Horse:
                        m_IsHorse = true;
                        break;
                    case (int)NpcTypeEnum.Task:
                        m_IsTask = true;
                        break;
                    case (int)NpcTypeEnum.PvpTower:
                        m_IsPvpTower = true;
                        break;
                }
                m_CanMove = npcCfg.m_CanMove;
                m_CanHitMove = npcCfg.m_CanHitMove;
                m_CanRotate = npcCfg.m_CanRotate;
                m_Scale = npcCfg.m_Scale;
                m_CauseStiff = npcCfg.m_CauseStiff;
                m_AcceptStiff = npcCfg.m_AcceptStiff;
                m_AcceptStiffEffect = npcCfg.m_AcceptStiffEffect;

                m_IsAttachControler = npcCfg.m_IsAttachControler;
                m_AttachNodeName = npcCfg.m_AttachNodeName;

                SetName(npcCfg.m_Name);
                SetLevel(npcCfg.m_Level);
                SetModel(npcCfg.m_Model);
                SetBornEffect(npcCfg.m_BornEffect);
                SetActionList(npcCfg.m_ActionList);

                AvoidanceRadius = npcCfg.m_AvoidanceRadius;
                if(null != npcCfg.m_Shape)
                {
                    Shape = (Shape)npcCfg.m_Shape.Clone();
                }
                else
                {
                    Shape = new Circle(new Vector3(0, 0, 0), 1);
                }

                ViewRange = npcCfg.m_ViewRange;
                GohomeRange = npcCfg.m_GohomeRange;
                ReleaseTime = npcCfg.m_ReleaseTime;
                SuperArmor = npcCfg.m_SuperArmor;
                m_MeetEnemyImpact = npcCfg.m_MeetEnemyImpact;
                m_MeetEnemyStayTime = npcCfg.m_MeetEnemyStayTime;
                m_MeetEnemyWalkTime = npcCfg.m_MeetEnemyWalkTime;

                int hp = (int)npcCfg.m_AttrData.GetAddHpMax(0, npcCfg.m_Level);
                int energy = (int)npcCfg.m_AttrData.GetAddEpMax(0, npcCfg.m_Level);
                float moveSpeed = npcCfg.m_AttrData.GetAddSpd(0, npcCfg.m_Level);
                float walkSpeed = npcCfg.m_AttrData.GetAddWalkSpd(0, npcCfg.m_Level);
                float runSpeed = npcCfg.m_AttrData.GetAddRunSpd(0, npcCfg.m_Level);
                int hpMax = (int)npcCfg.m_AttrData.GetAddHpMax(0, npcCfg.m_Level);
                int energyMax = (int)npcCfg.m_AttrData.GetAddEpMax(0, npcCfg.m_Level);
                float hpRecover = npcCfg.m_AttrData.GetAddHpRecover(0, npcCfg.m_Level);
                float energyRecover = npcCfg.m_AttrData.GetAddEpRecover(0, npcCfg.m_Level);
                int attackBase = (int)npcCfg.m_AttrData.GetAddAd(0, npcCfg.m_Level);
                int aDefenceBase = (int)npcCfg.m_AttrData.GetAddADp(0, npcCfg.m_Level);
                int mDefenceBase = (int)npcCfg.m_AttrData.GetAddMDp(0, npcCfg.m_Level);
                float critical = npcCfg.m_AttrData.GetAddCri(0, npcCfg.m_Level);
                float criticalPow = npcCfg.m_AttrData.GetAddPow(0, npcCfg.m_Level);
                float criticalBackHitPow = npcCfg.m_AttrData.GetAddBackHitPow(0, npcCfg.m_Level);
                float criticalCrackPow = npcCfg.m_AttrData.GetAddCrackPow(0, npcCfg.m_Level);
                float fireDam = npcCfg.m_AttrData.GetAddFireDam(0, npcCfg.m_Level);
                float fireErd = npcCfg.m_AttrData.GetAddFireErd(0, npcCfg.m_Level);
                float iceDam = npcCfg.m_AttrData.GetAddIceDam(0, npcCfg.m_Level);
                float iceErd = npcCfg.m_AttrData.GetAddIceErd(0, npcCfg.m_Level);
                float poisonDam = npcCfg.m_AttrData.GetAddPoisonDam(0, npcCfg.m_Level);
                float poisonErd = npcCfg.m_AttrData.GetAddPoisonErd(0, npcCfg.m_Level);
                float weight = npcCfg.m_AttrData.GetAddWeight(0, npcCfg.m_Level);
                float rps = npcCfg.m_AttrData.GetAddRps(0, npcCfg.m_Level);
                float attackRange = npcCfg.m_AttrData.GetAddAttackRange(0, npcCfg.m_Level);

                GetBaseProperty().SetMoveSpeed(Operate_Type.OT_Absolute, moveSpeed);
                GetBaseProperty().SetWalkSpeed(Operate_Type.OT_Absolute, walkSpeed);
                GetBaseProperty().SetRunSpeed(Operate_Type.OT_Absolute, runSpeed);
                GetBaseProperty().SetHpMax(Operate_Type.OT_Absolute, hpMax);
                GetBaseProperty().SetEnergyMax(Operate_Type.OT_Absolute, energyMax);
                GetBaseProperty().SetHpRecover(Operate_Type.OT_Absolute, hpRecover);
                GetBaseProperty().SetEnergyRecover(Operate_Type.OT_Absolute, energyRecover);
                GetBaseProperty().SetAttackBase(Operate_Type.OT_Absolute, attackBase);
                GetBaseProperty().SetADefenceBase(Operate_Type.OT_Absolute, aDefenceBase);
                GetBaseProperty().SetMDefenceBase(Operate_Type.OT_Absolute, mDefenceBase);
                GetBaseProperty().SetCritical(Operate_Type.OT_Absolute, critical);
                GetBaseProperty().SetCriticalPow(Operate_Type.OT_Absolute, criticalPow);
                GetBaseProperty().SetCriticalBackHitPow(Operate_Type.OT_Absolute, criticalBackHitPow);
                GetBaseProperty().SetCriticalCrackPow(Operate_Type.OT_Absolute, criticalCrackPow);
                GetBaseProperty().SetFireDamage(Operate_Type.OT_Absolute, fireDam);
                GetBaseProperty().SetFireERD(Operate_Type.OT_Absolute, fireErd);
                GetBaseProperty().SetIceDamage(Operate_Type.OT_Absolute, iceDam);
                GetBaseProperty().SetIceERD(Operate_Type.OT_Absolute, iceErd);
                GetBaseProperty().SetPoisonDamage(Operate_Type.OT_Absolute, poisonDam);
                GetBaseProperty().SetPoisonERD(Operate_Type.OT_Absolute, poisonErd);
                GetBaseProperty().SetWeight(Operate_Type.OT_Absolute, weight);
                GetBaseProperty().SetRps(Operate_Type.OT_Absolute, rps);
                GetBaseProperty().SetAttackRange(Operate_Type.OT_Absolute, attackRange);

                // 技能数据
                for(int i = 0; i < npcCfg.m_SkillList.Count; i++)
                {
                    SkillInfo skillInfo = new SkillInfo(npcCfg.m_SkillList[i]);
                    GetSkillStateInfo().AddSkill(skillInfo);
                }

                NpcAttrCalculator.Calc(this);
                SetHp(Operate_Type.OT_Absolute, GetActualProperty().HpMax);
                SetEnergy(Operate_Type.OT_Absolute, GetActualProperty().EnergyMax);
                m_Cross2StandTime = npcCfg.m_Cross2StandTime;
                m_Cross2RunTime = npcCfg.m_Cross2Runtime;
                m_DeadAnimTime = npcCfg.m_DeadAnimTime;
            }
        }

        public float Scale
        {
            get { return m_Scale; }
        }

        public bool CanMove
        {
            get { return m_CanMove; }
        }

        public bool CanHitMove
        {
            get { return m_CanHitMove; }
        }

        public NpcAiStateInfo GetAiStateInfo()
        {
            return m_AiStateInfo;
        }

        public int SummonOwnerId
        {
            get { return m_SummonOwnerId; }
            set { m_SummonOwnerId = value; }
        }

        public bool IsSimulateMove
        {
            get { return m_IsSimulateMove; }
            set { m_IsSimulateMove = value; }
        }

        public long LifeEndTime
        {
            get { return m_LifeEndTime; }
            set { m_LifeEndTime = value; }
        }

        public int BornAnimTimeMs
        {
            get { return m_BornAnimTimeMs; }
        }

        public long BornTime
        {
            set { m_BornTime = value; }
            get { return m_BornTime; }
        }

        public bool IsBorning { set; get; }

        public bool IsTaunt
        {
            get { return m_IsTaunt; }
            set { m_IsTaunt = value; }
        }

        public bool NeedDelete
        {
            get { return m_NeedDelete; }
            set { m_NeedDelete = value; }
        }

        public long MeetEnemyStayTime
        {
            get { return m_MeetEnemyStayTime; }
            set { m_MeetEnemyStayTime = value; }
        }

        public long MeetEnemyWalkTime
        {
            get { return m_MeetEnemyWalkTime; }
            set { m_MeetEnemyWalkTime = value; }
        }

        public bool IsCombatNpc()
        {
            if ((int)NpcTypeEnum.BigBoss == m_NpcType || (int)NpcTypeEnum.LittleBoss == m_NpcType || (int)NpcTypeEnum.Normal == m_NpcType)
            {
                return true;
            }
            return false;
        }

        public int NpcType
        {
            get { return m_NpcType; }
        }

        public void Reset()
        {
            m_NeedDelete = false;
            m_SummonOwnerId = -1;
            m_IsSimulateMove = false;
            m_LifeEndTime = -1;

            ResetCharacterInfo();
            GetAiStateInfo().Reset();
            GetAiStateInfo().AiDatas.Clear();
        }
    }
}
