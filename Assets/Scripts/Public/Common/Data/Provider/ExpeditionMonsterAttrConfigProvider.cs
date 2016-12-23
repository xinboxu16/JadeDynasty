using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class ExpeditionMonsterAttrConfig : IData
    {
        public int m_Id = 0;
        public string m_Description = "";
        public AttrDataConfig m_AttrData = new AttrDataConfig();
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_Description = DBCUtil.ExtractString(node, "Description", "", false);
            m_AttrData.CollectDataFromDBC(node);
            return true;
        }
        public int GetId()
        {
            return m_Id;
        }
    }

    public class ExpeditionMonsterAttrConfigProvider
    {
        private DataDictionaryMgr<ExpeditionMonsterAttrConfig> m_ExpeditionMonsterAttrConfigMgr = new DataDictionaryMgr<ExpeditionMonsterAttrConfig>();

        public void Load(string file, string root)
        {
            m_ExpeditionMonsterAttrConfigMgr.CollectDataFromDBC(file, root);
        }

        public static ExpeditionMonsterAttrConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ExpeditionMonsterAttrConfigProvider s_Instance = new ExpeditionMonsterAttrConfigProvider();
    }
}
