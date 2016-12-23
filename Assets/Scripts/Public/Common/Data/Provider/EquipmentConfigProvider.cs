using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class EquipmentConfig : IData
    {
        public int m_EquipmentId = 0;
        public string m_EquipmentName = "";
        public string m_UiModel = "";
        public AttrDataConfig m_AttrData = new AttrDataConfig();

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_EquipmentId = DBCUtil.ExtractNumeric<int>(node, "EquipmentId", 0, true);
            m_EquipmentName = DBCUtil.ExtractString(node, "EquipmentName", "", true);
            m_UiModel = DBCUtil.ExtractString(node, "UiModel", "", false);
            m_AttrData.CollectDataFromDBC(node);
            return true;
        }

        public int GetId()
        {
            return m_EquipmentId;
        }
    }

    public class EquipmentConfigProvider
    {
        private DataDictionaryMgr<EquipmentConfig> m_EquipmentConfigMgr = new DataDictionaryMgr<EquipmentConfig>();

        public void LoadEquipmentConfig(string file, string root)
        {
            m_EquipmentConfigMgr.CollectDataFromDBC(file, root);
        }

        public static EquipmentConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static EquipmentConfigProvider s_Instance = new EquipmentConfigProvider();
    }
}
