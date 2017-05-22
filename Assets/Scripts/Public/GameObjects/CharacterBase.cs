using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DashFireSpatial;
using UnityEngine;

namespace DashFire
{
    public interface IShootTarget
    {
        uint GetActorID();
    }

    /**
     * @brief 角色基类
     */
    public class CharacterInfo : IShootTarget
    {
        public class SpaceObjectImpl : ISpaceObject
        {
            private CharacterInfo m_CharacterInfo = null;
            private SpatialObjType m_ObjType = SpatialObjType.kNPC;
            private List<ISpaceObject> m_CollideObjects = new List<ISpaceObject>(); // 与当前物体碰撞的物体

            public SpaceObjectImpl(CharacterInfo info, SpatialObjType objType)
            {
                m_CharacterInfo = info;
                m_ObjType = objType;
            }

            public uint GetID() { return (uint)m_CharacterInfo.GetId(); }
            public SpatialObjType GetObjType() { return m_ObjType; }
            public Vector3 GetPosition()
            {
                Vector3 v = m_CharacterInfo.GetMovementStateInfo().GetPosition3D();
                return v;
            }
            public float GetRadius() { return m_CharacterInfo.GetRadius(); }
            public Vector3 GetVelocity()
            {
                Vector3 ret;
                if (!m_CharacterInfo.GetMovementStateInfo().IsMoving)
                {
                    ret = new Vector3();
                }
                else
                {
                    ret = m_CharacterInfo.GetMovementStateInfo().GetMoveDir3D() * (float)(m_CharacterInfo.GetActualProperty().MoveSpeed * m_CharacterInfo.VelocityCoefficient);
                }
                return ret;
            }
            public bool IsAvoidable()
            {
                bool ret = true;
                if (SpatialObjType.kNPC == m_ObjType)
                {
                    NpcInfo npc = m_CharacterInfo.CastNpcInfo();
                    if (null != npc)
                    {
                        ret = (npc.NpcType != (int)NpcTypeEnum.Skill &&
                          npc.NpcType != (int)NpcTypeEnum.AutoPickItem);
                    }
                }
                return ret;
            }
            public Shape GetCollideShape()
            {
                return m_CharacterInfo.Shape;
            }
            public List<ISpaceObject> GetCollideObjects() { return m_CollideObjects; }

            public void OnCollideObject(ISpaceObject obj)
            {
                m_CollideObjects.Add(obj);
            }
            public void OnDepartObject(ISpaceObject obj)
            {
                m_CollideObjects.Remove(obj);
            }
            public object RealObject
            {
                get
                {
                    return m_CharacterInfo;
                }
            }
        }

        protected int m_Id = 0;
        private int m_CampId = 0;
        private Shape m_Shape = null;
        private float m_VelocityCoefficient = 1;

        protected ISpaceObject m_SpaceObject = null;
        protected UserInfo m_CastUserInfo = null;
        private CellPos m_SightCell;
        private CharacterInfo m_ControllerObject = null;
        private CharacterInfo m_ControlledObject = null;
        protected NpcInfo m_CastNpcInfo = null;
        /**
         * @brief 当前属性值
         */
        protected CharacterProperty m_ActualProperty;

        private SkillStateInfo m_SkillStateInfo = new SkillStateInfo();
        private MovementStateInfo m_MovementStateInfo = new MovementStateInfo();

        /**
         * @brief 构造函数
         *
         * @param id
         *
         * @return 
         */
        public CharacterInfo(int id)
        {
            m_Id = id;
            //m_UnitId = 0;
            //m_LinkId = 0;
            //m_AIEnable = true;
            //m_BaseProperty = new CharacterProperty();
            m_ActualProperty = new CharacterProperty();

            //m_ReleaseTime = 0;
            //IsFlying = false;
        }

        public UserInfo CastUserInfo()
        {
            return m_CastUserInfo;
        }

        public DashFireSpatial.CellPos SightCell
        {
            get { return m_SightCell; }
            set { m_SightCell = value; }
        }

        /**
         * @brief 获取id
         *
         * @return 
         */
        public int GetId()
        {
            return m_Id;
        }

        /**
         * @brief 阵营ID	
         */
        public int GetCampId()
        {
            if (null != m_ControllerObject)
            {
                return m_ControllerObject.GetCampId();
            }
            return m_CampId;
        }

        public SkillStateInfo GetSkillStateInfo()
        {
            return m_SkillStateInfo;
        }

        public DashFireSpatial.ISpaceObject SpaceObject
        {
            get { return m_SpaceObject; }
        }

        //获取移动状态
        public MovementStateInfo GetMovementStateInfo()
        {
            return m_MovementStateInfo;
        }

        //获取角度
        public float GetRadius()
        {
            float radius = 0;
            if (null != m_Shape)
                radius = m_Shape.GetRadius();
            return radius;
        }

        //临时加的，不要调用
        public uint GetActorID()
        {
            return 0;
        }

        /**
         * @brief 当前属性值
         */
        public CharacterProperty GetActualProperty()
        {
            return m_ActualProperty;
        }

        /**
         * 对象的速度系数（在0~1之间）
         */
        public float VelocityCoefficient
        {
            get { return m_VelocityCoefficient; }
            set { m_VelocityCoefficient = value; }
        }

        public NpcInfo CastNpcInfo()
        {
            return m_CastNpcInfo;
        }

        public Shape Shape
        {
            get { return m_Shape; }
            set { m_Shape = value; }
        }
    }
}
