using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class MovementStateInfo
    {
        private Vector3 m_Position;
        private bool m_IsMoving = false;
        private bool m_IsMoveMeetObstacle = false;
        private float m_MoveDir = 0;

        public Vector3 GetPosition3D()
        {
            return m_Position;
        }

        //是否移动
        public bool IsMoving
        {
            get { return m_IsMoving; }
            set
            {
                m_IsMoving = value;
                if (m_IsMoving)
                    m_IsMoveMeetObstacle = false;
            }
        }

        public float GetMoveDir()
        {
            return m_MoveDir;
        }

        public Vector3 GetMoveDir3D()
        {
            float dir = GetMoveDir();
            return new Vector3((float)Math.Sin(dir), 0, (float)Math.Cos(dir));
        }
    }
}
