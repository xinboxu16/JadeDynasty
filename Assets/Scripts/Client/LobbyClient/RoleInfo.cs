using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class RoleInfo
    {
        // 角色GUID
        private ulong m_Guid = 0;

        // 远征信息
        private ExpeditionPlayerInfo m_Expeditioninfo = new ExpeditionPlayerInfo();

        public ulong Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
        }

        public ExpeditionPlayerInfo GetExpeditionInfo()
        {
            return m_Expeditioninfo;
        }
    }
}
