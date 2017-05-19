using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    //Lobby 休息室; 游说团; 门厅，大厅; 投票厅;
    public class LobbyClient
    {
        private RoleInfo m_CurrentRole = null;                    //当前游戏的玩家角色
        public RoleInfo CurrentRole
        {
            get { return m_CurrentRole; }
            set { m_CurrentRole = value; }
        }

        public static LobbyClient Instance
        {
            get { return s_Instance; }
        }
        private static LobbyClient s_Instance = new LobbyClient();
    }
}
