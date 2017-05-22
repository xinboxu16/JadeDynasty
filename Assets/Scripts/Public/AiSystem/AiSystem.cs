using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class AiSystem
    {
        private long m_LastTickTime = 0;

        public void Reset()
        {
            m_LastTickTime = 0;
        }
    }
}
