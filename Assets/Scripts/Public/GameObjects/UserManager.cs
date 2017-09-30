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

        private UserInfo NewUserInfo(int id)
        {
            UserInfo user = null;
            if (m_UnusedUsers.Count > 0)
            {
                user = m_UnusedUsers.Dequeue();
                user.Reset();
                user.InitId(id);
            }
            else
            {
                user = new UserInfo(id);
            }
            return user;
        }

        public UserInfo AddUser(int id, int resId)
        {
            UserInfo user = NewUserInfo(id);
            user.LoadData(resId);
            m_Users.AddLast(user.GetId(), user);
            return user;
        }

        public void RemoveUser(int id)
        {
            UserInfo user = GetUserInfo(id);
            if(null != user)
            {
                m_Users.Remove(id);
                user.SceneContext = null;
                RecycleUserInfo(user);
            }
        }

        private void RecycleUserInfo(UserInfo userInfo)
        {
            if (null != userInfo && m_UnusedUsers.Count < m_UserPoolSize)
            {
                userInfo.Reset();
                m_UnusedUsers.Enqueue(userInfo);
            }
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
