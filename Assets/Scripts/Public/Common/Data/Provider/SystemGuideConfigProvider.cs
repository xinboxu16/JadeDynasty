using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SystemGuideConfig : IData
    {
        public int Id;
        public List<int> Operations;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            Operations = DBCUtil.ExtractNumericList<int>(node, "Operation", 0, true);
            return true;
        }
        public int GetId()
        {
            return Id;
        }
    }

    public class SystemGuideConfigProvider
    {
        private DataDictionaryMgr<SystemGuideConfig> m_SystemGuideConfigMgr = new DataDictionaryMgr<SystemGuideConfig>();

        public void Load(string file, string root)
        {
            m_SystemGuideConfigMgr.CollectDataFromDBC(file, root);
        }

        public static SystemGuideConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static SystemGuideConfigProvider s_Instance = new SystemGuideConfigProvider();

        public SystemGuideConfig GetDataById(int id)
        {
            return m_SystemGuideConfigMgr.GetDataById(id);
        }
    }
}
