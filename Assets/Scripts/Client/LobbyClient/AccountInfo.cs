using System;
using System.Collections.Generic;

namespace DashFire
{
    public class AccountInfo
    {
        public string Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        public List<RoleInfo> Players
        {
            get { return m_Roles; }
        }
        public RoleInfo FindRole(ulong userGuid)
        {
            RoleInfo ret = null;
            for (int i = 0; i < m_Roles.Count; ++i)
            {
                if (m_Roles[i].Guid == userGuid)
                {
                    ret = m_Roles[i];
                    break;
                }
            }
            return ret;
        }

        private string m_Account;                 //玩家账号
        private string m_AccountId;               //账号平台返回的账号ID
        private List<RoleInfo> m_Roles = new List<RoleInfo>();  // 玩家角色列表
    }
}