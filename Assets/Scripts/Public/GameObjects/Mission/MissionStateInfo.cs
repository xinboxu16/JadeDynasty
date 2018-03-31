using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum MissionStateType
    {
        UNCOMPLETED = 0,
        COMPLETED = 1,
    }

    public enum MissionOperationType
    {
        ADD,
        FINISH,
        DELETE,
        UPDATA,
    }
    public class MissionStateInfo
    {
        private Dictionary<int, MissionInfo> m_UnCompletedMissions = new Dictionary<int, MissionInfo>();
        private Dictionary<int, MissionInfo> m_CompletedMissions = new Dictionary<int, MissionInfo>();

        public bool AddMission(int id, MissionStateType type)
        {
            switch (type)
            {
                case MissionStateType.UNCOMPLETED:
                    AddUnCompletedMission(id);
                    return true;
                case MissionStateType.COMPLETED:
                    AddCompletedMission(id);
                    return true;
                default:
                    return false;
            }
        }
        private bool AddUnCompletedMission(int id)
        {
            LogSystem.Debug("Add uncompleted mission {0}", id);
            if (!m_UnCompletedMissions.ContainsKey(id))
            {
                m_UnCompletedMissions.Add(id, new MissionInfo(id));
                return true;
            }
            return false;
        }

        private bool AddCompletedMission(int id)
        {
            LogSystem.Debug("Add completed mission {0}", id);
            if (!m_CompletedMissions.ContainsKey(id))
            {
                m_CompletedMissions.Add(id, new MissionInfo(id));
                return true;
            }
            return false;
        }

        public bool CompletedMission(int id)
        {
            LogSystem.Debug("Completed mission {0}", id);
            if (m_UnCompletedMissions.ContainsKey(id) && !m_CompletedMissions.ContainsKey(id))
            {
                m_CompletedMissions.Add(id, m_UnCompletedMissions[id]);
                m_UnCompletedMissions.Remove(id);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            m_UnCompletedMissions.Clear();
            m_CompletedMissions.Clear();
        }
        public Dictionary<int, MissionInfo> UnCompletedMissions
        {
            get { return m_UnCompletedMissions; }
        }
        public Dictionary<int, MissionInfo> CompletedMissions
        {
            get { return m_CompletedMissions; }
        }
    }
}
