using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class AiSystem
    {
        private long m_LastTickTime = 0;
        private NpcManager m_NpcMgr = null;
        private UserManager m_UserMgr = null;

        private AiCommandDispatcher m_AiCommandDispatcher = new AiCommandDispatcher();

        public void Reset()
        {
            m_LastTickTime = 0;
        }

        public void Tick()
        {
            if (0 == m_LastTickTime)
            {
                m_LastTickTime = TimeUtility.GetServerMilliseconds();
            }
            else
            {
                long delta = TimeUtility.GetServerMilliseconds() - m_LastTickTime;
                m_LastTickTime = TimeUtility.GetServerMilliseconds();
                if(null != m_NpcMgr)
                {
                    for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
                    {
                        NpcInfo npc = linkNode.Value;
                        TickNpc(npc, delta);
                    }
                }

                if (null != m_UserMgr)
                {
                    for (LinkedListNode<UserInfo> linkNode = m_UserMgr.Users.FirstValue; null != linkNode; linkNode = linkNode.Next)
                    {
                        UserInfo user = linkNode.Value;
                        TickUser(user, delta);
                    }
                }
            }
        }

        private void TickNpc(NpcInfo npc, long delta)
        {
            INpcStateLogic logic = AiLogicManager.Instance.GetNpcStateLogic(npc.GetAiStateInfo().AiLogic);
            if (null != logic)
            {
                logic.Execute(npc, m_AiCommandDispatcher, delta);
            }
        }

        private void TickUser(UserInfo user, long delta)
        {
            IUserStateLogic logic = AiLogicManager.Instance.GetUserStateLogic(user.GetAiStateInfo().AiLogic);
            if (null != logic)
            {
                logic.Execute(user, m_AiCommandDispatcher, delta);
            }
        }

        public void SetNpcManager(NpcManager npcMgr)
        {
            m_NpcMgr = npcMgr;
        }
        public void SetUserManager(UserManager userMgr)
        {
            m_UserMgr = userMgr;
        }
    }
}
