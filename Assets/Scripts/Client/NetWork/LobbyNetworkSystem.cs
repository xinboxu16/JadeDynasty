using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire.Network
{
    public sealed partial class LobbyNetworkSystem
    {
        private IActionQueue m_AsyncActionQueue;

        private ulong m_Guid;
        private string m_Account;
        private bool m_IsWaitStart = true;
        private bool m_IsLogged = false;

        private WebSocket4Net.WebSocket m_WebSocket;

        public void Init(IActionQueue asyncQueue)
        {
            //WebSocket的事件不是在当前线程触发的，我们需要自己进行线程调整
            m_AsyncActionQueue = asyncQueue;
        }

        public void QuitClient()
        {
            if (m_Guid != 0)
            {
                JsonData msg = new JsonData();
                msg.SetJsonType(JsonType.Object);
                msg.Set("m_Guid", m_Guid);
                SendMessage(JsonMessageID.Logout, msg);
            }
            if (LobbyClient.Instance.AccountInfo.Account != string.Empty)
            {
                JsonData msg = new JsonData();
                msg.SetJsonType(JsonType.Object);
                msg.Set("m_Account", m_Account);
                SendMessage(JsonMessageID.AccountLogout, msg);
            }
            m_IsWaitStart = true;
            m_IsLogged = false;
            if (IsConnected)
            {
                m_WebSocket.Close();
            }
        }

        private void SendMessage(JsonMessage msg)
        {
            try
            {
                JsonMessageDispatcher.SendMessage(msg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("LobbyNetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void SendMessage(JsonMessageID id, JsonData msg)
        {
            try
            {
                JsonMessageDispatcher.SendMessage((int)id, msg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("LobbyNetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal bool SendMessage(string msgStr)
        {
            bool ret = false;
            try
            {
                if (IsConnected)
                {
                    m_WebSocket.Send(msgStr);
                    LogSystem.Info("SendToLobby {0}", msgStr);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("SendMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return ret;
        }

        public bool IsConnected
        {
            get
            {
                bool ret = false;
                if (null != m_WebSocket)
                    ret = (m_WebSocket.State == WebSocket4Net.WebSocketState.Open && m_WebSocket.IsSocketConnected);
                return ret;
            }
        }

        #region Sington
        public static LobbyNetworkSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static LobbyNetworkSystem s_Instance = new LobbyNetworkSystem();
        #endregion
    }
}
