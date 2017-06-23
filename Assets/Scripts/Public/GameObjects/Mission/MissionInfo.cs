using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class MissionInfo
    {
        public enum MissionType
        {
            MAIN_LINE,
            CHALLENGE,
        }

        private int m_MissionId;
        private MissionType m_MissionType;
        private MissionConfig m_Config;
        private int m_Param0;
        private int m_Param1;
        private string m_Progress;

        public MissionInfo(int id)
        {
            m_MissionId = id;
            m_Config = MissionConfigProvider.Instance.GetDataById(m_MissionId);
            if (null != m_Config)
            {
                m_MissionType = (MissionType)m_Config.MissionType;
            }
            else
            {
                LogSystem.Warn("MissionInfo:: can not find mission {0}", id);
            }
        }

        public string Progress
        {
            get { return m_Progress; }
            set { m_Progress = value; }
        }
    }
}
