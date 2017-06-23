using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class VersionConfig : IData
    {
        public string m_MainNum = "";
        public string m_SubNum = "";
        public string m_StepNum = "";
        public string m_DateNum = "";
        public string m_GreekSymbols = "";
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_MainNum = DBCUtil.ExtractString(node, "MainNum", "1", true);
            m_SubNum = DBCUtil.ExtractString(node, "SubNum", "0", true);
            m_StepNum = DBCUtil.ExtractString(node, "StepNum", "0", true);
            m_DateNum = DBCUtil.ExtractString(node, "DateNum", "0", true);
            m_GreekSymbols = DBCUtil.ExtractString(node, "GreekSymbols", "base", true);
            return true;
        }
        public int GetId()
        {
            return int.Parse(m_MainNum);
        }
    }

    public class VersionConfigProvider
    {
        private DataDictionaryMgr<VersionConfig> m_VersionConfigMgr = new DataDictionaryMgr<VersionConfig>();

        public void Load(string file, string root)
        {
            m_VersionConfigMgr.CollectDataFromDBC(file, root);
        }

        public string GetVersionNum()
        {
            string game_version = "0";
            MyDictionary<int, object> dic = m_VersionConfigMgr.GetData();
            foreach(object element in dic.Values)
            {
                VersionConfig version = element as VersionConfig;
                if(null != version)
                {
                    game_version = version.m_MainNum + "-" + version.m_SubNum + "-" + version.m_StepNum + "-" + version.m_DateNum + "-" + version.m_GreekSymbols;
                    break;
                }
            }
            return game_version;
        }

        public static VersionConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static VersionConfigProvider s_Instance = new VersionConfigProvider();
    }
}
