using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class AiPathData
    {
        private Queue<Vector3> m_PathPoints = new Queue<Vector3>();
        private Vector3 m_LastPos = new Vector3();
        private Vector3 m_StartPos = new Vector3();
        private IList<Vector3> m_Path = null;
        private int m_PathStart = 0;
        private int m_PathLength = 0;
        private long m_UpdateTime = 0;
        private bool m_IsUsingAvoidanceVelocity = false;

        public void SetPathPoints(Vector3 startPos, IList<Vector3> pts)
        {
            SetPathPoints(startPos, pts, 0);
        }

        public void SetPathPoints(Vector3 startPos, IList<Vector3> pts, int start)
        {
            SetPathPoints(startPos, pts, start, pts.Count - start);
        }

        public void SetPathPoints(Vector3 startPos, IList<Vector3> pts, int start, int len)
        {
            m_PathPoints.Clear();
            m_LastPos = startPos;
            for(int ix = start; ix < pts.Count && ix < start + len; ++ix)
            {
                m_PathPoints.Enqueue(pts[ix]);
            }
            m_StartPos = startPos;
            m_Path = pts;
            m_PathStart = start;
            m_PathLength = len;
        }

        public bool IsReached(Vector3 curPos)
        {
            bool ret = false;
            if (m_PathPoints.Count > 0)
            {
                Vector3 targetPos = m_PathPoints.Peek();
                float powDistDest = Geometry.DistanceSquare(curPos, targetPos);
                if (powDistDest <= 0.25f)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public void UseNextPathPoint()
        {
            m_LastPos = CurPathPoint;
            if (m_PathPoints.Count > 0)
                m_PathPoints.Dequeue();
        }

        public long UpdateTime
        {
            get { return m_UpdateTime; }
            set { m_UpdateTime = value; }
        }

        public bool IsUsingAvoidanceVelocity
        {
            get { return m_IsUsingAvoidanceVelocity; }
            set { m_IsUsingAvoidanceVelocity = value; }
        }

        public bool HavePathPoint
        {
            get
            {
                return m_PathPoints.Count > 0;
            }
        }

        public Vector3 CurPathPoint
        {
            get
            {
                Vector3 pos;
                if (m_PathPoints.Count > 0)
                    pos = m_PathPoints.Peek();
                else
                    pos = new Vector3();
                return pos;
            }
        }

        public void Restart()
        {
            m_PathPoints.Clear();
            m_LastPos = m_StartPos;
            for (int ix = m_PathStart; ix < m_Path.Count && ix < m_PathStart + m_PathLength; ++ix)
            {
                m_PathPoints.Enqueue(m_Path[ix]);
            }
        }

        public void Clear()
        {
            m_PathPoints.Clear();
            m_Path = null;
            m_PathStart = 0;
            m_PathLength = 0;
        }
    }
}
