using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class GowInfo
    {
        private int m_GowElo = 1400;
        private int m_GowMatches = 0;
        private int m_GowWinMatches = 0;
        private int m_LeftMatchCount = 0;
        private DateTime m_LastBuyTime;
        private int m_LeftBuyCount = 0;

        public int GowElo
        {
            get { return m_GowElo; }
            set { m_GowElo = value; }
        }

        public int GowMatches
        {
            get { return m_GowMatches; }
            set { m_GowMatches = value; }
        }
        public int GowWinMatches
        {
            get { return m_GowWinMatches; }
            set { m_GowWinMatches = value; }
        }
        public int LeftMatchCount
        {
            get { return m_LeftMatchCount; }
            set { m_LeftMatchCount = value; }
        }
    }
}
