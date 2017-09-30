using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class AiActionInfo
    {
        private AiActionConfig m_Config;
        private long m_LastTriggerTime = 0;

        public AiActionInfo(AiActionConfig config)
        {
            m_Config = config;
            m_LastTriggerTime = 0;
        }
    }
}
