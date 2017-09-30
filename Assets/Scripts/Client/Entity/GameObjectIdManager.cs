using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class GameObjectIdManager
    {
        private GameObjectIdManager() { }

        public int GenNextId()
        {
            int ret = m_NextId;
            ++m_NextId;
            return ret;
        }

        private int m_NextId = 1;

        public static GameObjectIdManager Instance
        {
            get { return s_Instance; }
        }
        private static GameObjectIdManager s_Instance = new GameObjectIdManager();
    }
}
