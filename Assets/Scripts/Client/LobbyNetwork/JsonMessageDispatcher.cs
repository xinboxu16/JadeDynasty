using System;
using System.Text;
using LitJson;
using DashFire;
using DashFire.Network;

namespace DashFire.Network
{
    internal class JsonMessageHandlerInfo
    {
        public Type m_ProtoType = null;
        public JsonMessageHandlerDelegate m_Handler = null;
    }
    public class JsonMessageDispatcher
    {
        public static void Init()
        {
            if (!s_Inited)
            {
                s_MessageHandlers = new JsonMessageHandlerInfo[(int)JsonMessageID.MaxNum];
                for (int i = (int)JsonMessageID.Zero; i < (int)JsonMessageID.MaxNum; ++i)
                {
                    s_MessageHandlers[i] = new JsonMessageHandlerInfo();
                }
                s_Inited = true;
            }
        }

        public static bool Inited
        {
            get
            {
                return s_Inited;
            }
        }

        public static void RegisterMessageHandler(int id, Type protoType, JsonMessageHandlerDelegate handler)
        {
            if (s_Inited)
            {
                if (id >= (int)JsonMessageID.Zero && id < (int)JsonMessageID.MaxNum)
                {
                    s_MessageHandlers[id].m_ProtoType = protoType;
                    s_MessageHandlers[id].m_Handler = handler;
                }
            }
        }

        public static unsafe void HandleNodeMessage(string msgStr)
        {
            if (s_Inited)
            {
                JsonMessage msg = DecodeJsonMessage(msgStr);
                if (null != msg)
                {
                    HandleNodeMessage(msg);
                }
            }
        }

        private static void HandleNodeMessage(JsonMessage msg)
        {
            if (s_Inited && msg != null)
            {
                JsonMessageHandlerDelegate handler = s_MessageHandlers[msg.m_ID].m_Handler;
                if (handler != null)
                {
                    try
                    {
                        handler(msg);
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error("[Exception] HandleNodeMessage:{0} throw:{1}\n{2}", msg.m_ID, ex.Message, ex.StackTrace);
                    }
                }
            }
        }

        private static unsafe JsonMessage DecodeJsonMessage(string msgStr)
        {
            JsonMessage msg = null;
            if (s_Inited)
            {
                try
                {
                    //LogSystem.Info("DecodeJsonMessage:{0}", msgStr);

                    int ix = msgStr.IndexOf('|');
                    if (ix > 0)
                    {
                        int id = int.Parse(msgStr.Substring(0, ix));
                        int ix2 = msgStr.IndexOf('|', ix + 1);
                        msg = new JsonMessage(id);
                        if (ix2 > 0)
                        {
                            string jsonStr = msgStr.Substring(ix + 1, ix2 - ix - 1);
                            string protoStr = msgStr.Substring(ix2 + 1);
                            msg.m_JsonData = JsonMapper.ToObject(jsonStr);
                            Type t = s_MessageHandlers[id].m_ProtoType;
                            if (null != t)
                            {
                                byte[] bytes = Convert.FromBase64String(protoStr);
                                msg.m_ProtoData = Encoding.Decode(t, bytes);
                            }
                        }
                        else
                        {
                            string jsonStr = msgStr.Substring(ix + 1);
                            msg.m_JsonData = JsonMapper.ToObject(jsonStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error("[Exception] DecodeJsonMessage:{0} throw:{1}\n{2}", msgStr, ex.Message, ex.StackTrace);
                }
            }
            return msg;
        }

        public static bool SendMessage(int id, JsonData jsonData)
        {
            JsonMessage msg = new JsonMessage(id);
            msg.m_JsonData = jsonData;
            return SendMessage(msg);
        }

        public static bool SendMessage(JsonMessage msg)
        {
            string msgStr = BuildNodeMessage(msg);
            return LobbyNetworkSystem.Instance.SendMessage(msgStr);
        }

        private static string BuildNodeMessage(JsonMessage msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(msg.m_ID);
            sb.Append('|');
            sb.Append(JsonMapper.ToJson(msg.m_JsonData));
            if (null != msg.m_ProtoData)
            {
                byte[] bytes = Encoding.Encode(msg.m_ProtoData);
                sb.Append('|');
                sb.Append(Convert.ToBase64String(bytes));
            }
            return sb.ToString();
        }

        private static ProtoNetEncoding Encoding
        {
            get
            {
                if (null == s_Encoding)
                {
                    s_Encoding = new ProtoNetEncoding();
                }
                return s_Encoding;
            }
        }

        private static bool s_Inited = false;
        private static JsonMessageHandlerInfo[] s_MessageHandlers = null;
        [ThreadStatic]
        private static ProtoNetEncoding s_Encoding = null;
    }
}

