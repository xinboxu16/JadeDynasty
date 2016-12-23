using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire.Network
{
    public sealed partial class LobbyNetworkSystem
    {
        private IActionQueue m_AsyncActionQueue;

        public void Init(IActionQueue asyncQueue)
        {
            //WebSocket的事件不是在当前线程触发的，我们需要自己进行线程调整
            m_AsyncActionQueue = asyncQueue;
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
