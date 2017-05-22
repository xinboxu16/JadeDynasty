using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public NpcManager(int poolSize)
        {
            m_NpcPoolSize = poolSize;
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

        public NpcInfo GetNpcInfo(int id)
        {
            NpcInfo npc;
            m_Npcs.TryGetValue(id, out npc);
            return npc;
        }

        public LinkedListDictionary<int, NpcInfo> Npcs
        {
            get { return m_Npcs; }
        }
    }
}
