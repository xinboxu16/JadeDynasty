using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed class NpcManager
    {
        private int m_NpcPoolSize = 1024;
        private const int c_StartId = 20000;
        private const int c_MaxIdNum = 10000;
        private const int c_StartId_Client = 30000;
        private int m_NextInfoId = c_StartId;

        private SceneContextInfo m_SceneContext = null;

        private LinkedListDictionary<int, NpcInfo> m_Npcs = new LinkedListDictionary<int, NpcInfo>();
        private List<NpcInfo> m_DelayAdd = new List<NpcInfo>();
        private Queue<NpcInfo> m_UnusedNpcs = new Queue<NpcInfo>();
        private Heap<int> m_UnusedIds = new Heap<int>(new DefaultReverseComparer<int>());
        private Heap<int> m_UnusedClientIds = new Heap<int>(new DefaultReverseComparer<int>());

        public NpcManager(int poolSize)
        {
            m_NpcPoolSize = poolSize;
        }

        public void SetSceneContext(SceneContextInfo context)
        {
            m_SceneContext = context;
        }

        public NpcInfo AddNpc(int id, Data_Unit unit)
        {
            NpcInfo npc = NewNpcInfo(id);
            npc.SceneContext = m_SceneContext;
            npc.LoadData(unit);
            npc.IsBorning = true;
            npc.SetAIEnable(false);
            npc.SetStateFlag(Operate_Type.OT_AddBit, CharacterState_Type.CST_Invincible);
            npc.BornTime = TimeUtility.GetServerMilliseconds();
            m_Npcs.AddLast(npc.GetId(), npc);
            if (null != m_SceneContext && null != m_SceneContext.SpatialSystem)
            {
                m_SceneContext.SpatialSystem.AddObj(npc.SpaceObject);
            }
            if (null != m_SceneContext && null != m_SceneContext.SightManager)
            {
                m_SceneContext.SightManager.AddObject(npc);
            }
            return npc;
        }

        public NpcInfo AddNpc(Data_Unit unit)
        {
            NpcInfo npc = NewNpcInfo();
            npc.SceneContext = m_SceneContext;
            npc.LoadData(unit);
            // born
            npc.IsBorning = true;
            npc.SetAIEnable(false);
            npc.SetStateFlag(Operate_Type.OT_AddBit, CharacterState_Type.CST_Invincible);
            npc.BornTime = TimeUtility.GetServerMilliseconds();
            m_Npcs.AddLast(npc.GetId(), npc);
            if (null != m_SceneContext && null != m_SceneContext.SpatialSystem)
            {
                m_SceneContext.SpatialSystem.AddObj(npc.SpaceObject);
            }
            if(null != m_SceneContext && null != m_SceneContext.SightManager)
            {
                m_SceneContext.SightManager.AddObject(npc);
            }
            return npc;
        }

        private NpcInfo NewNpcInfo(int id)
        {
            NpcInfo npc = null;
            if (m_UnusedNpcs.Count > 0)
            {
                npc = m_UnusedNpcs.Dequeue();
                npc.Reset();
                npc.InitId(id);
            }
            else
            {
                npc = new NpcInfo(id);
            }
            return npc;
        }

        private NpcInfo NewNpcInfo()
        {
            NpcInfo npc = null;
            int id = GenNextId();
            if(m_UnusedNpcs.Count > 0)
            {
                npc = m_UnusedNpcs.Dequeue();
                npc.Reset();
                npc.InitId(id);
            }
            else
            {
                npc = new NpcInfo(id);
            }
            return npc;
        }

        private int GenNextId()
        {
            int id = 0;
            int startId = 0;
            if(GlobalVariables.Instance.IsClient)
            {
                startId = c_StartId_Client;
                while(m_UnusedClientIds.Count > 0)
                {
                    int t = m_UnusedClientIds.Pop();
                    if(!m_Npcs.Contains(t))
                    {
                        id = t;
                        break;
                    }
                }
            }

            if (id <= 0)
            {
                for(int i = 0; i < c_MaxIdNum; ++i)
                {
                    int t = (m_NextInfoId + i - startId) % c_MaxIdNum + startId;
                    if(!m_Npcs.Contains(t))
                    {
                        id = t;
                        break;
                    }
                }
                if (id > 0)
                {
                    m_NextInfoId = (id + 1 - startId) % c_MaxIdNum + startId;
                }
            }
            return id;
        }

        public void Reset()
        {
            for (LinkedListNode<NpcInfo> linkNode = m_Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo npc = linkNode.Value;
                if (null != npc)
                {
                    if (null != m_SceneContext && null != m_SceneContext.SpatialSystem)
                    {
                        m_SceneContext.SpatialSystem.RemoveObj(npc.SpaceObject);
                    }
                    if (null != m_SceneContext && null != m_SceneContext.SightManager)
                    {
                        m_SceneContext.SightManager.RemoveObject(npc);
                    }
                }
            }
            foreach (NpcInfo npc in m_DelayAdd)
            {
                if (null != npc)
                {
                    if (null != m_SceneContext && null != m_SceneContext.SpatialSystem)
                    {
                        m_SceneContext.SpatialSystem.RemoveObj(npc.SpaceObject);
                    }
                    if (null != m_SceneContext && null != m_SceneContext.SightManager)
                    {
                        m_SceneContext.SightManager.RemoveObject(npc);
                    }
                }
            }
            m_Npcs.Clear();
            m_DelayAdd.Clear();
            m_UnusedNpcs.Clear();
            m_NextInfoId = c_StartId;
        }

        public NpcInfo GetNearest(Vector3 pos, ref float minPowDist)
        {
            NpcInfo result = null;
            float powDist = 0.0f;
            for (LinkedListNode<NpcInfo> linkNode = m_Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo npc = linkNode.Value;
                if (null != npc && npc.IsCombatNpc())
                {
                    powDist = Geometry.DistanceSquare(pos, npc.GetMovementStateInfo().GetPosition3D());
                    if (minPowDist > powDist)
                    {
                        result = npc;
                        minPowDist = powDist;
                    }
                }
            }
            return result;
        }

        public void RemoveNpc(int id)
        {
            NpcInfo npc = GetNpcInfo(id);
            if (null != npc)
            {
                if (null != m_SceneContext && null != m_SceneContext.SpatialSystem)
                {
                    m_SceneContext.SpatialSystem.RemoveObj(npc.SpaceObject);
                }
                if (null != m_SceneContext && null != m_SceneContext.SightManager)
                {
                    m_SceneContext.SightManager.RemoveObject(npc);
                }
                m_Npcs.Remove(id);
                npc.SceneContext = m_SceneContext;
                RecycleNpcInfo(npc);
            }
        }

        private void RecycleNpcInfo(NpcInfo npcInfo)
        {
            if (null != npcInfo)
            {
                int id = npcInfo.GetId();
                if (id >= c_StartId && id < c_StartId + c_MaxIdNum)
                {
                    m_UnusedIds.Push(id);
                }
                if (id >= c_StartId_Client && id < c_StartId_Client + c_MaxIdNum)
                {
                    m_UnusedClientIds.Push(id);
                }
                if (m_UnusedNpcs.Count < m_NpcPoolSize)
                {
                    npcInfo.Reset();
                    m_UnusedNpcs.Enqueue(npcInfo);
                }
            }
        }

        public NpcInfo GetNpcInfo(int id)
        {
            NpcInfo npc;
            m_Npcs.TryGetValue(id, out npc);
            return npc;
        }

        public NpcInfo GetNpcInfoByUnitId(int id)
        {
            NpcInfo npc = null;
            for (LinkedListNode<NpcInfo> linkNode = m_Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo info = linkNode.Value;
                if (info.GetUnitId() == id)
                {
                    npc = info;
                    break;
                }
            }
            return npc;
        }

        public LinkedListDictionary<int, NpcInfo> Npcs
        {
            get { return m_Npcs; }
        }
    }
}
