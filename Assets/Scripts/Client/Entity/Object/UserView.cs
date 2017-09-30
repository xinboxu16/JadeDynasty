using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class UserView : CharacterView
    {
        private UserInfo m_User = null;
        private Vector4 old_color_;
        private int m_IndicatorActor = 0;
        private float m_IndicatorDir = 0;
        private bool m_IndicatorVisible = false;
        private int m_IndicatorTargetType = 1;

        private void Init()
        {
            m_User = null;
            old_color_ = Vector4.zero;
        }

        public void Create(UserInfo user)
        {
            Init();
            if (null != user)
            {
                m_User = user;
                ObjectInfo.CampId = m_User.GetCampId();
                MovementStateInfo msi = m_User.GetMovementStateInfo();
                Vector3 pos = msi.GetPosition3D();
                float dir = msi.GetFaceDir();
                //TODO 未实现
                CreateActor(m_User.GetId(), m_User.GetModel(), pos, dir, m_User.Scale);
                CreateIndicatorActor(m_User.GetId(), m_User.GetIndicatorModel());
                InitAnimationSets();
                ObjectInfo.IsPlayer = true;
                if (user.GetId() == WorldSystem.Instance.PlayerSelfId)
                {
                    GfxSystem.MarkPlayerSelf(Actor);
                }
            }
        }

        private void CreateIndicatorActor(int objId, string model)
        {
            m_IndicatorActor = GameObjectIdManager.Instance.GenNextId();
            GfxSystem.CreateGameObject(m_IndicatorActor, model, 0, 0, 0, 0, 0, 0, false);
            //GfxSystem.CreateGameObjectForAttach(m_IndicatorActor, model);
            //GfxSystem.AttachGameObject(m_IndicatorActor, Actor);
            GfxSystem.SetGameObjectVisible(m_IndicatorActor, false);
            GfxSystem.SendMessage(m_IndicatorActor, "SetOwner", Actor);
            //GfxSystem.UpdateGameObjectLocalRotateY(m_IndicatorActor, m_IndicatorDir);
        }

        public void Destroy()
        {
            DestroyActor();
            Release();
        }

        protected override CharacterInfo GetOwner()
        {
            return m_User;
        }

        public void SetIndicatorInfo(bool visible, float dir, int targetType)
        {
            m_IndicatorVisible = visible;
            m_IndicatorDir = dir;
            m_IndicatorTargetType = targetType;
        }

        public void Update()
        {
            UpdateAttr();
            UpdateSpatial();
            //更新动画
            UpdateAnimation();
            UpdateIndicator();
        }

        private void UpdateIndicator()
        {
            if (null != m_User)
            {
                GfxSystem.SetGameObjectVisible(m_IndicatorActor, m_IndicatorVisible);
                GfxSystem.SendMessage(m_IndicatorActor, "SetIndicatorDir", m_IndicatorDir);
                GfxSystem.SendMessage(m_IndicatorActor, "SetIndicatorTarget", m_IndicatorTargetType);
            }
        }

        private void UpdateAnimation()
        {
            if (!CanAffectPlayerSelf) return;
            if(null != m_User)
            {
                UpdateState();
                if(ObjectInfo.IsGfxAnimation)//是否技能特效
                {
                    m_CharacterAnimationInfo.Reset();
                    m_IdleState = IdleState.kNotIdle;
                    return;
                }
                UpdateMoveAnimation();
                UpdateDead();
                UpdateIdle();
            }
        }

        private void UpdateAttr()
        {
            if (null != m_User)
            {
                ObjectInfo.Blood = m_User.Hp;
                ObjectInfo.MaxBlood = m_User.GetActualProperty().HpMax;
                ObjectInfo.Energy = m_User.Energy;
                ObjectInfo.MaxEnergy = m_User.GetActualProperty().EnergyMax;
                ObjectInfo.Rage = m_User.Rage;
                ObjectInfo.MaxRage = m_User.GetActualProperty().RageMax;
                ObjectInfo.IsSuperArmor = (m_User.SuperArmor || m_User.UltraArmor);
                m_User.GfxStateFlag = ObjectInfo.GfxStateFlag;
            }
        }

        //修改移动
        private void UpdateSpatial()
        {
            if(null != m_User)
            {
                MovementStateInfo msi = m_User.GetMovementStateInfo();
                if (ObjectInfo.IsGfxMoveControl)
                {
                    if (ObjectInfo.DataChangedByGfx)
                    {
                        msi.PositionX = ObjectInfo.X;
                        msi.PositionY = ObjectInfo.Y;
                        msi.PositionZ = ObjectInfo.Z;
                        msi.SetFaceDir(ObjectInfo.FaceDir);
                        ObjectInfo.DataChangedByGfx = false;
                    }
                    if (ObjectInfo.WantDirChangedByGfx)
                    {
                        msi.SetWantFaceDir(ObjectInfo.WantFaceDir);
                        ObjectInfo.WantDirChangedByGfx = false;
                    }
                }
                else
                {
                    if (ObjectInfo.DataChangedByGfx)
                    {
                        msi.PositionX = ObjectInfo.X;
                        msi.PositionY = ObjectInfo.Y;
                        msi.PositionZ = ObjectInfo.Z;
                        ObjectInfo.DataChangedByGfx = false;
                    }
                    UpdateMovement();
                }
                ObjectInfo.WantFaceDir = msi.GetWantFaceDir();
                SimulateDir(ObjectInfo.WantFaceDir);
            }
        }

        private void SimulateDir(float dir)
        {
            List<NpcInfo> summons = m_User.GetSkillStateInfo().GetSummonObject();
            foreach(NpcInfo npc in summons)
            {
                if(npc.IsSimulateMove)
                {
                    npc.GetMovementStateInfo().SetWantFaceDir(dir);
                }
            }
        }

        private void Release()
        {

        }
    }
}
