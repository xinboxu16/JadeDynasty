using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    public sealed class StoryMessageHandler
    {
        private class MessageInfo
        {
            public string m_MsgId = null;
            public object[] m_Args = null;
        }

        private bool m_IsTriggered = false;
        private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();

        public void Reset()
        {
            m_IsTriggered = false;
            foreach (IStoryCommand cmd in m_CommandQueue)
            {
                cmd.Reset();
            }
            m_CommandQueue.Clear();
        }
    }
    public sealed class StoryInstance
    {
        private class MessageInfo
        {
            public string m_MsgId = null;
            public object[] m_Args = null;
        }

        private bool m_IsTerminated = false;
        private Queue<MessageInfo> m_MessageQueue = new Queue<MessageInfo>();
        private List<StoryMessageHandler> m_MessageHandlers = new List<StoryMessageHandler>();

        private TypedDataCollection m_CustomDatas = new TypedDataCollection();

        public void Reset()
        {
            m_IsTerminated = false;
            m_MessageQueue.Clear();
            int ct = m_MessageHandlers.Count;
            for (int i = ct - 1; i >= 0; --i)
            {
                StoryMessageHandler handler = m_MessageHandlers[i];
                handler.Reset();
            }
            m_CustomDatas.Clear();
        }
    }
}
