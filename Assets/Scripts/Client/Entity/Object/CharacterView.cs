using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed class CharacterAnimationInfo
    {
        public bool IsPlayChangeWeapon;
        public bool IsPlayDead;
        public bool IsPlayTaunt;

        public bool IsMoving;
        public bool CanMove;
        public MovementMode MoveMode;
        public bool IsCombat;
        public float m_Speed;

        public void Reset()
        {
            IsPlayChangeWeapon = false;
            IsPlayDead = false;
            IsPlayTaunt = false;
            IsMoving = false;
            CanMove = true;
            IsCombat = false;
            m_Speed = 0;
        }

        public bool IsIdle()
        {
            return !IsMoving && !IsPlayChangeWeapon && !IsPlayDead && !IsPlayTaunt;
        }
    }

    public enum IdleState
    {
        kNotIdle, //未进入
        kReady, //空闲开始计时
        kBegin, //开始播放空闲动画
    }

    public class CharacterView
    {
        private int m_Actor = 0;
        private int m_ObjId = 0;
        private bool m_Visible = true;
        private Dictionary<string, uint> effect_map_ = new Dictionary<string, uint>();

        //动画相关
        protected Data_ActionConfig m_CurActionConfig = null;
        private bool m_CanAffectPlayerSelf = true;
        protected CharacterAnimationInfo m_CharacterAnimationInfo = new CharacterAnimationInfo();
        protected IdleState m_IdleState = IdleState.kNotIdle;
        protected long m_BeginIdleTime = 0;
        protected long m_IdleInterval = 0;

        protected long m_LastLeaveCombatTime = 0;
        protected bool m_IsCombat2IdleChanging = true;
        protected bool m_IsCombatState = false;
        protected bool m_IsWeaponMoved = false;

        private Vector4 m_NormalColor = new Vector4(1, 1, 1, 1);
        private Vector4 m_BurnColor = new Vector4(0.75f, 0.2f, 0.2f, 1);
        private Vector4 m_FrozonColor = new Vector4(0.2f, 0.2f, 0.75f, 1);
        private Vector4 m_ShineColor = new Vector4(0.2f, 0.75f, 0.2f, 1);

        protected virtual CharacterInfo GetOwner() { return null; }

        protected void DestroyActor()
        {
            GfxSystem.DestroyGameObject(m_Actor);
            Release();
        }

        private void Release()
        {
            List<string> keyList = effect_map_.Keys.ToList();
            if (keyList != null && keyList.Count > 0)
            {
                foreach (string model in keyList)
                {
                    //DetachActor(model);
                }
            }
            CurWeaponList.Clear();
        }

        private List<string> m_CurWeaponName = new List<string>();

        public List<string> CurWeaponList
        {
            get
            {
                return m_CurWeaponName;
            }
        }

        private SharedGameObjectInfo m_ObjectInfo = new SharedGameObjectInfo();

        public SharedGameObjectInfo ObjectInfo
        {
            get { return m_ObjectInfo; }
        }

        public int Actor
        {
            get { return m_Actor; }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = UpdateVisible(value);
            }
        }

        public bool IsCombatState
        {
            get { return m_IsCombatState; }
        }

        public bool CanAffectPlayerSelf
        {
            get { return m_CanAffectPlayerSelf || WorldSystem.Instance.IsObserver; }
            set { m_CanAffectPlayerSelf = value; }
        }

        private void Init()
        {
            m_NormalColor = new Vector4(1, 1, 1, 1);
            m_BurnColor = new Vector4(0.75f, 0.2f, 0.2f, 1);
            m_FrozonColor = new Vector4(0.2f, 0.2f, 0.75f, 1);
            m_ShineColor = new Vector4(0.2f, 0.75f, 0.2f, 1);
            m_Actor = 0;

            m_CurActionConfig = null;
        }

        protected void CreateActor(int objId, string model, Vector3 pos, float dir, float scale = 1.0f)
        {
            Init();

            m_ObjId = objId;
            m_Actor = GameObjectIdManager.Instance.GenNextId();
            m_ObjectInfo.m_ActorId = m_Actor;
            m_ObjectInfo.m_LogicObjectId = objId;
            m_ObjectInfo.X = pos.x;
            m_ObjectInfo.Y = pos.y;
            m_ObjectInfo.Z = pos.z;
            m_ObjectInfo.FaceDir = dir;
            m_ObjectInfo.Sx = scale;
            m_ObjectInfo.Sy = scale;
            m_ObjectInfo.Sz = scale;
            GfxSystem.CreateGameObject(m_Actor, model, m_ObjectInfo);
        }

        protected void CreateBornEffect(int parentActor, string effect)
        {
            if(!String.IsNullOrEmpty(effect))
            {
                GfxSystem.CreateAndAttachGameObject(effect, parentActor, "");
            }
        }

        protected void InitAnimationSets()
        {
            List<int> actionList = GetOwner().GetActionList();
            for(int i = 0; i < actionList.Count; i++)
            {
                m_CurActionConfig = ActionConfigProvider.Instance.GetDataById(actionList[i]);
                m_ObjectInfo.AnimConfigId = actionList[i];
            }
        }

        protected virtual void UpdateIdle()
        {
            if (!GetOwner().IsDead() && m_CharacterAnimationInfo.IsIdle())
            {
                if(m_IdleState == IdleState.kNotIdle)
                {
                    Animation_Type at = m_IsCombatState ? Animation_Type.AT_CombatStand : Animation_Type.AT_Stand;
                    string name = GetAnimationNameByType(at);
                    if(!string.IsNullOrEmpty(name))
                    {
                        float fadeTime = 0.5f;
                        if(GetOwner() != null && GetOwner().GetSkillStateInfo() != null)
                        {
                            fadeTime = GetOwner().GetSkillStateInfo().CrossToStandTime;
                            GetOwner().ResetCross2StandRunTime();
                        }
                        GfxSystem.CrossFadeAnimation(m_Actor, name, fadeTime);
                    }
                    m_BeginIdleTime = TimeUtility.GetServerMilliseconds();
                    m_IdleState = IdleState.kReady;
                    m_IdleInterval = new System.Random().Next(1, 3) * 1000;
                }
                else if (m_IdleState == IdleState.kReady)
                {
                    if (TimeUtility.GetServerMilliseconds() - m_BeginIdleTime > m_IdleInterval)
                    {
                        Animation_Type at = m_IsCombatState ? Animation_Type.AT_CombatStand : Animation_Type.AT_Stand;
                        string name = GetAnimationNameByType(at);
                        if (!string.IsNullOrEmpty(name))
                        {
                            float fadetime = 0.5f;
                            if (GetOwner() != null && GetOwner().GetSkillStateInfo() != null)
                            {
                                fadetime = GetOwner().GetSkillStateInfo().CrossToStandTime;
                            }
                            GfxSystem.CrossFadeAnimation(m_Actor, name, fadetime);
                        }
                        m_BeginIdleTime = TimeUtility.GetServerMilliseconds();
                    }
                }
            }
            else
            {
                m_IdleState = IdleState.kNotIdle;
            }
        }

        protected void UpdateDead()
        {
            CharacterInfo charObj = GetOwner();
            if (null == charObj) return;
            if (charObj.IsDead() && !charObj.GetSkillStateInfo().IsImpactControl() && charObj.DeadTime > 0 && !charObj.IsHaveGfxStateFlag(GfxCharacterState_Type.KnockDown))
            {
                if (!m_CharacterAnimationInfo.IsPlayDead)
                {
                    m_CharacterAnimationInfo.IsPlayDead = true;
                    string name = GetAnimationNameByType(Animation_Type.AT_Dead);
                    if (!string.IsNullOrEmpty(name))
                    {
                        GfxSystem.CrossFadeAnimation(m_Actor, name);
                    }
                }
            }
            else
            {
                if (m_CharacterAnimationInfo.IsPlayDead)
                {
                    m_CharacterAnimationInfo.IsPlayDead = false;
                }
            }
        }

        protected void UpdateMoveAnimation()
        {
            CharacterInfo charObj = GetOwner();
            if (null == charObj) return;
            if(charObj.GetMovementStateInfo().IsMoving && !charObj.GetMovementStateInfo().IsSkillMoving)
            {
                if(!m_CharacterAnimationInfo.IsMoving)
                {
                    m_CharacterAnimationInfo.IsMoving = true;
                    m_CharacterAnimationInfo.MoveMode = charObj.GetMovementStateInfo().MovementMode;
                    StartMoveAnimation();
                }
                else if (IsMoveStateChange())
                {
                    Debug.Log("MovementMode" + m_CharacterAnimationInfo.MoveMode);
                    StopMoveAnimation();
                    UpdateAnimInfo();
                    StartMoveAnimation();
                }
            }
            else
            {
                if (m_CharacterAnimationInfo.IsMoving)
                {
                    Debug.Log("StopMoveAnimation m_CharacterAnimationInfo.IsMoving prveMoveState" + m_CharacterAnimationInfo.IsMoving);
                    m_CharacterAnimationInfo.IsMoving = false;
                    StopMoveAnimation();
                }
            }
        }

        private void StopMoveAnimation()
        {
            FadeToStand();
        }

        private void StartMoveAnimation()
        {
            Animation_Type type = Animation_Type.AT_None;
            float speedFactor;
            float moveSpeed = GetOwner().GetActualProperty().MoveSpeed;
            GetAnimationDirAndSpeed(m_CharacterAnimationInfo.MoveMode, moveSpeed, out type, out speedFactor);
            if(type == Animation_Type.AT_RunForward && IsCombatState)
            {
                type = Animation_Type.AT_CombatRun;
                if (m_CurActionConfig != null)
                {
                    speedFactor = moveSpeed / m_CurActionConfig.m_CombatStdSpeed;
                }
            }
            string name = GetAnimationNameByType(type);
            if(!string.IsNullOrEmpty(name))
            {
                float fadetime = 0.3f;
                if(null != GetOwner() && GetOwner().GetSkillStateInfo() != null)
                {
                    fadetime = GetOwner().GetSkillStateInfo().CrossToRunTime;
                    GetOwner().ResetCross2StandRunTime();
                }
                GfxSystem.SetAnimationSpeed(m_Actor, name, speedFactor);
                GfxSystem.CrossFadeAnimation(m_Actor, name, fadetime);
            }
        }

        private void UpdateAnimInfo()
        {
            m_CharacterAnimationInfo.MoveMode = GetOwner().GetMovementStateInfo().MovementMode;
            m_CharacterAnimationInfo.IsCombat = IsCombatState;
            m_CharacterAnimationInfo.m_Speed = GetOwner().GetActualProperty().MoveSpeed;
        }

        private void FadeToStand()
        {
            string name = GetAnimationNameByType(Animation_Type.AT_Stand);
            if (!string.IsNullOrEmpty(name))
            {
                GfxSystem.CrossFadeAnimation(m_Actor, name);
            }
        }

        private bool IsMoveStateChange()
        {
            if (m_CharacterAnimationInfo.MoveMode != GetOwner().GetMovementStateInfo().MovementMode)
            {
                return true;
            }
            if (m_CharacterAnimationInfo.IsCombat != IsCombatState)
            {
                return true;
            }
            if (m_CharacterAnimationInfo.m_Speed != GetOwner().GetActualProperty().MoveSpeed)
            {
                return true;
            }
            return false;
        }

        protected void GetAnimationDirAndSpeed(MovementMode mode, float move_speed, out Animation_Type at, out float speed_factor)
        {
            Data_ActionConfig actionConfig = m_CurActionConfig;
            if (mode == MovementMode.LowSpeed)
            {
                at = Animation_Type.AT_SlowMove;
            }
            else if (mode == MovementMode.HighSpeed)
            {
                at = Animation_Type.AT_FastMove;
            }
            else
            {
                at = Animation_Type.AT_RunForward;
            }

            if (actionConfig != null)
            {
                if(mode == MovementMode.LowSpeed)
                {
                    speed_factor = move_speed / actionConfig.m_SlowStdSpeed;
                }
                else if (mode == MovementMode.HighSpeed)
                {
                    speed_factor = move_speed / actionConfig.m_FastStdSpeed;
                }
                else
                {
                    speed_factor = move_speed / actionConfig.m_ForwardStdSpeed;
                }
            }
            else
            {
                speed_factor = 1.0f;
            }
        }

        protected virtual bool UpdateVisible(bool visible)
        {
            GfxSystem.SetGameObjectVisible(m_Actor, visible);
            return visible;
        }

        protected void UpdateState()
        {
            if(GetOwner().IsDead())
            {
                return;
            }

            long now = TimeUtility.GetServerMilliseconds();
            if(IsInCombatState())
            {
                m_LastLeaveCombatTime = now;
                m_IsCombat2IdleChanging = false;
                m_IsCombatState = true;
            }
            else if(m_IsCombatState)
            {
                if(GetOwner().GetMovementStateInfo().IsMoving)
                {
                    m_LastLeaveCombatTime = now;
                    m_IsCombat2IdleChanging = false;
                }
            }
            if (GetOwner().GetId() == WorldSystem.Instance.GetPlayerSelf().GetId())
            {
                if (m_LastLeaveCombatTime + GetOwner().Combat2IdleTime * 1000 <= now && !m_IsCombat2IdleChanging)
                {
                    GetOwner().SkillController.PushSkill(SkillCategory.kCombat2Idle, Vector3.zero);
                    m_IsCombat2IdleChanging = true;
                }
            }
            if (m_IsCombatState && !m_IsWeaponMoved)
            {
                EnterCombatState();
            }
        }

        public void EnterCombatState()
        {
            m_IsCombatState = true;
            string[] weapon_moves = GetOwner().Idle2CombatWeaponMoves.Split('|');
            for(int i = 1; i < weapon_moves.Length; i+=2)
            {
                string child = weapon_moves[i - 1];
                string node = weapon_moves[i];
                //TODO:未实现
                //GfxSystem.QueueGfxAction(GfxModule.Skill.Trigers.TriggerUtil.MoveChildToNode, Actor, child, node);
            }
            m_IsWeaponMoved = true;
        }

        //是否战斗状态
        protected bool IsInCombatState()
        {
            SkillStateInfo state = GetOwner().GetSkillStateInfo();
            if(state == null)
            {
                return false;
            }
            if(state.IsSkillActivated() && state.GetCurSkillInfo().SkillId != GetOwner().Combat2IdleSkill)
            {
                return true;
            }
            if (state.IsImpactActive())
            {
                return true;
            }
            return false;
        }

        //更新移动
        protected void UpdateMovement()
        {
            CharacterInfo obj = GetOwner();
            if(null != obj && !obj.IsDead() && null != ObjectInfo)
            {
                if (obj.IsNpc && !obj.CastNpcInfo().CanMove) return;
                MovementStateInfo msi = obj.GetMovementStateInfo();
                ObjectInfo.FaceDir = msi.GetFaceDir();
                ObjectInfo.WantFaceDir = msi.GetWantFaceDir();
                if (msi.IsMoving)
                {
                    Vector3 pos = msi.GetPosition3D();
                    ObjectInfo.MoveCos = (float)msi.MoveDirCosAngle;
                    ObjectInfo.MoveSin = (float)msi.MoveDirSinAngle;
                    ObjectInfo.MoveSpeed = (float)obj.GetActualProperty().MoveSpeed * (float)obj.VelocityCoefficient;

                    if (obj is UserInfo)
                    {
                        if(msi.TargetPosition.sqrMagnitude < Geometry.c_FloatPrecision)
                        {
                            ObjectInfo.MoveTargetDistanceSqr = 100.0f;
                        }
                        else
                        {
                            ObjectInfo.MoveTargetDistanceSqr = msi.CalcDistancSquareToTarget();
                        }
                    }
                    else
                    {
                        ObjectInfo.MoveTargetDistanceSqr = msi.CalcDistancSquareToTarget();
                    }

                    ObjectInfo.IsLogicMoving = true;
                }
                else
                {
                    ObjectInfo.IsLogicMoving = false;
                }
            }
            else
            {
                ObjectInfo.IsLogicMoving = false;
            }
        }

        public void StopAnimation(Animation_Type type)
        {
            string name = GetAnimationNameByType(type);
            if(string.IsNullOrEmpty(name))
            {
                return;
            }
            GfxSystem.StopAnimation(m_Actor, name);
        }

        public void PlayQueuedAnimation(Animation_Type type)
        {
            PlayQueuedAnimation(type, 1.0f);
        }

        public void PlayQueuedAnimation(Animation_Type type, float speed)
        {
            string name = GetAnimationNameByType(type);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            GfxSystem.PlayQueuedAnimation(m_Actor, name);
            GfxSystem.SetAnimationSpeed(m_Actor, name, speed);
        }

        public void PlayAnimation(Animation_Type type)
        {
            PlayAnimation(type, 1.0f);
        }

        public void PlayAnimation(Animation_Type type, float speed)
        {
            string name = GetAnimationNameByType(type);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            GfxSystem.CrossFadeAnimation(m_Actor, name);
            GfxSystem.SetAnimationSpeed(m_Actor, name, speed);
        }

        protected string GetAnimationNameByType(Animation_Type type)
        {
            if(m_CurActionConfig != null)
            {
                Data_ActionConfig.Data_ActionInfo action = m_CurActionConfig.GetRandomActionByType(type);
                if (action != null)
                {
                    return action.m_AnimName;
                }
            }
            return null;
        }
    }
}
