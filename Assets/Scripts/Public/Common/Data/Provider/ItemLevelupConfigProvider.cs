using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class ItemLevelupConfig : IData
    {
        public int m_Id = 0;
        public int m_Level = 0;
        public const int PartsNum = 8;
        public List<int> m_PartsList = new List<int>();
        public int m_ChangeEquipCost = 0;
        public float m_Rate = 0;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_Level = DBCUtil.ExtractNumeric<int>(node, "Level", 0, true);
            for (int i = 0; i < PartsNum; ++i)
            {
                string key = "WearPart_" + i.ToString();
                m_PartsList.Add(DBCUtil.ExtractNumeric<int>(node, key, 0, false));
            }
            m_ChangeEquipCost = DBCUtil.ExtractNumeric<int>(node, "ChangeEquipCost", 0, false);
            m_Rate = DBCUtil.ExtractNumeric<float>(node, "Rate", 1.0f, false);
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class ItemLevelupConfigProvider
    {
        private DataDictionaryMgr<ItemLevelupConfig> m_ItemLevelupConfigMgr = new DataDictionaryMgr<ItemLevelupConfig>();

        public void Load(string file, string root)
        {
            m_ItemLevelupConfigMgr.CollectDataFromDBC(file, root);
        }

        public static ItemLevelupConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ItemLevelupConfigProvider s_Instance = new ItemLevelupConfigProvider();
    }
}
