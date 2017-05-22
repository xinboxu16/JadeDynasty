using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SkillStateInfo
    {
        private List<NpcInfo> m_SummonObjects;

        public void RecyleSummonObject(NpcInfo npc)
        {
            m_SummonObjects.Remove(npc);
        }
    }
}
