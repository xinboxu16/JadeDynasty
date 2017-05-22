using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class UserManager
    {
        private int m_UserPoolSize = 1024;
        private const int c_StartId = 1;
        private int m_NextInfoId = c_StartId;

        private LinkedListDictionary<int, UserInfo> m_Users = new LinkedListDictionary<int, UserInfo>();
        private Queue<UserInfo> m_UnusedUsers = new Queue<UserInfo>();

        public UserManager(int poolSize)
        {
            m_UserPoolSize = poolSize;
        }

        public void Reset()
        {
            m_Users.Clear();
            m_UnusedUsers.Clear();
            m_NextInfoId = c_StartId;
        }

        public UserInfo GetUserInfo(int id)
        {
            UserInfo info;
            Users.TryGetValue(id, out info);
            return info;
        }

        public LinkedListDictionary<int, UserInfo> Users
        {
            get { return m_Users; }
        }
    }
}
