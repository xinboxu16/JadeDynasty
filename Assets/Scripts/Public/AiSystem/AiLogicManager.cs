using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed class AiLogicManager
    {
        private Dictionary<int, INpcStateLogic> m_NpcStateLogics = new Dictionary<int, INpcStateLogic>();
        private Dictionary<int, IUserStateLogic> m_UserStateLogics = new Dictionary<int, IUserStateLogic>();

        private AiLogicManager()
        {
            //这里初始化所有的Ai状态逻辑，并记录到对应的列表(客户端的逻辑因为通常比较简单，很多会使用通用的ai逻辑)
            if(GlobalVariables.Instance.IsClient)
            {
                AiLogic_User_Client userLogic = new AiLogic_User_Client();
                //m_UserStateLogics.Add((int)AiStateLogicId.UserMirror_General, new AiLogic_UserMirror_General());
                //m_UserStateLogics.Add((int)AiStateLogicId.PvpUser_General, userLogic);
                m_UserStateLogics.Add((int)AiStateLogicId.UserSelf_General, new AiLogic_UserSelf_General());
                //m_UserStateLogics.Add((int)AiStateLogicId.UserSelfRange_General, new AiLogic_UserSelfRange_General());


                m_NpcStateLogics.Add((int)AiStateLogicId.Npc_CommonMelee, new AiLogic_Npc_CommonMelee());
            }
        }

        public INpcStateLogic GetNpcStateLogic(int id)
        {
            INpcStateLogic logic = null;
            if (m_NpcStateLogics.ContainsKey(id))
                logic = m_NpcStateLogics[id];
            return logic;
        }

        public IUserStateLogic GetUserStateLogic(int id)
        {
            //Debug.Log("GetUserStateLogic_id_" + id.ToString());

            IUserStateLogic logic = null;
            if (m_UserStateLogics.ContainsKey(id))
                logic = m_UserStateLogics[id];
            return logic;
        }

        public static AiLogicManager Instance
        {
            get { return s_AiLogicManager; }
        }
        private static AiLogicManager s_AiLogicManager = new AiLogicManager();
    }
}
