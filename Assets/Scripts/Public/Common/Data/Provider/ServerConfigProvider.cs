using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class ServerConfig : IData
    {
        public int ServerId = -1;
        public string ServerName = "";
        public string NodeIp = "";
        public int NodePort = -1;
        public int LogicServerId = -1;
        public string LogicServerName = "";
        public bool CollectDataFromDBC(DBC_Row node)
        {
            ServerId = DBCUtil.ExtractNumeric<int>(node, "ServerId", -1, true);
            ServerName = DBCUtil.ExtractString(node, "ServerName", "", true);
            NodeIp = DBCUtil.ExtractString(node, "NodeIp", "", true);
            NodePort = DBCUtil.ExtractNumeric<int>(node, "NodePort", -1, true);
            LogicServerId = DBCUtil.ExtractNumeric<int>(node, "LogicServerId", -1, true);
            LogicServerName = DBCUtil.ExtractNumeric(node, "LogicServerName", "", true);
            return true;
        }
        public int GetId()
        {
            return ServerId;
        }
    }

    public class ServerConfigProvider
    {
        private DataDictionaryMgr<ServerConfig> m_ServerConfigMgr = new DataDictionaryMgr<ServerConfig>();

        public void Load(string file, string root)
        {
            m_ServerConfigMgr.CollectDataFromDBC(file, root);
        }

        public static ServerConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ServerConfigProvider s_Instance = new ServerConfigProvider();
    }
}
