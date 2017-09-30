using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire.Network
{
    class MessageDispatch
    {
        public delegate void MsgHandler(object msg, NetConnection user);
        MyDictionary<Type, MsgHandler> m_DicHandler = new MyDictionary<Type, MsgHandler>();

        public void RegisterHandler(Type t, MsgHandler handler)
        {
            m_DicHandler[t] = handler;
        }

        public bool Dispatch(object msg, NetConnection conn)
        {
            MsgHandler msgHandler;
            if(m_DicHandler.TryGetValue(msg.GetType(), out msgHandler))
            {
                msgHandler.Invoke(msg, conn);
                return true;
            }
            return false;
        }
    }
}
