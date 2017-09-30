using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class AiData_UserSelf_General
    {
        private long m_Time = 0;
        private AiPathData m_FoundPath = new AiPathData();

        public long Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        public DashFire.AiPathData FoundPath
        {
            get { return m_FoundPath; }
        }
    }
}
