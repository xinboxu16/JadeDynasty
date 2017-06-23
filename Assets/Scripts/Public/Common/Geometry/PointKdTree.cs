using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed class PointKdTree
    {
        private struct KdTreeNode
        {
            public int m_Begin;
            public int m_End;
            public int m_Left;
            public float m_MaxX;
            public float m_MaxZ;
            public float m_MinX;
            public float m_MinZ;
            public int m_Right;
        }

        public const int c_MaxLeafSize = 4;
        private int m_PointNum = 0;
        private Vector3[] m_Points = null;
        private KdTreeNode[] m_KdTree = null;
        private int m_MaxNodeNum = 0;
        private Stack<int> m_BuildStack = new Stack<int>(4096);
        private Stack<int> m_QueryStack = new Stack<int>(4096);

        public void Build(IList<Vector3> pts)
        {
            if(null == m_Points || m_Points.Length < pts.Count)
            {
                m_PointNum = 0;
                m_Points = new Vector3[pts.Count * 2];
                foreach(Vector3 pt in pts)
                {
                    m_Points[m_PointNum++] = pt;
                }
            }
            else
            {
                m_PointNum = 0;
                foreach(Vector3 pt in pts)
                {
                    m_Points[m_PointNum] = pt;
                    ++m_PointNum;
                }
            }

            if(m_PointNum > 0)
            {
                if(null == m_KdTree || m_KdTree.Length < 3 * m_PointNum)
                {
                    m_KdTree = new KdTreeNode[3 * m_PointNum];
                    for(int i = 0; i < m_KdTree.Length; i++)
                    {
                        m_KdTree[i] = new KdTreeNode();
                    }
                }
                m_MaxNodeNum = 2 * m_PointNum;
                BuildImpl();
            }
        }

        private void BuildImpl()
        {
            m_BuildStack.Push(0);
            m_BuildStack.Push(m_PointNum);
            m_BuildStack.Push(0);

            while(m_BuildStack.Count >= 3)
            {
                int begin = m_BuildStack.Pop();
                int end = m_BuildStack.Pop();
                int node = m_BuildStack.Pop();

                m_KdTree[node].m_Begin = begin;
                m_KdTree[node].m_End = end;

                float minX = m_Points[begin].x;
                float maxX = minX;
                float minZ = m_Points[begin].z;
                float maxZ = minZ;
                for(int i = begin + 1; i < end; ++i)
                {
                    float newX = m_Points[i].x;
                    float newZ = m_Points[i].z;
                    if (minX > newX)
                        minX = newX;
                    else if (maxX < newX)
                        maxX = newX;
                    if (minZ > newZ)
                        minZ = newZ;
                    else if (maxZ < newZ)
                        maxZ = newZ;
                }
                m_KdTree[node].m_MinX = minX;
                m_KdTree[node].m_MaxX = maxX;
                m_KdTree[node].m_MinZ = minZ;
                m_KdTree[node].m_MaxZ = maxZ;

                if (end - begin > c_MaxLeafSize)
                {
                    bool isVertical = (maxX - minX > maxZ - minZ);//判断以x轴还是y轴分割树
                    float splitValue = (isVertical ? 0.5f * (maxX + minX) : 0.5f * (maxZ + minZ));//取中间值

                    int left = begin;
                    int right = end;
                    //code 不懂
                    while(left < right)
                    {
                        while(left < right && (isVertical ? m_Points[left].x : m_Points[left].z) < splitValue)
                        {
                            ++left;
                        }

                        while (right > left && (isVertical ? m_Points[right - 1].x : m_Points[right - 1].z) >= splitValue)
                        {
                            --right;
                        }

                        //交换
                        if (left < right)
                        {
                            Vector3 tmp = m_Points[left];
                            m_Points[left] = m_Points[right - 1];
                            m_Points[right - 1] = tmp;
                            ++left;
                            --right;
                        }
                    }

                    if (left == end)
                    {
                        --left;
                    }

                    int leftSize = left - begin;

                    if (leftSize == 0)
                    {
                        ++leftSize;
                        ++left;
                    }

                    m_KdTree[node].m_Left = node + 1;
                    m_KdTree[node].m_Right = node + 1 + (2 * leftSize - 1);

                    if (m_KdTree[node].m_Left >= m_MaxNodeNum || m_KdTree[node].m_Right >= m_MaxNodeNum)
                    {
                        LogSystem.Error("PointKdTree Error, node:{0} left:{1} right:{2} leftSize:{3} begin:{4} end:{5} maxNodeNum:{6}", node, left, right, leftSize, begin, end, m_MaxNodeNum);
                    }

                    m_BuildStack.Push(m_KdTree[node].m_Left);
                    m_BuildStack.Push(left);
                    m_BuildStack.Push(begin);

                    m_BuildStack.Push(m_KdTree[node].m_Right);
                    m_BuildStack.Push(end);
                    m_BuildStack.Push(left);
                }
            }
        }

        public void Clear()
        {
            m_PointNum = 0;
        }
    }
}
