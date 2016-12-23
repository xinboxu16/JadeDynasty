using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class LegacyLevelupConfig : IData
    {
        public int m_Level = 0;
        public const int LegacyCount = 4;
        public List<int> m_CostItemList = new List<int>();
        public int m_CostNum = 0;
        public float m_Rate = 1f;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Level = DBCUtil.ExtractNumeric<int>(node, "Level", 0, true);
            m_CostItemList.Clear();
            if (LegacyCount > 0)
            {
                for (int i = 0; i < LegacyCount; ++i)
                {
                    int index = i + 1;
                    string key = "CostItemID_" + index.ToString();
                    m_CostItemList.Insert(i, DBCUtil.ExtractNumeric<int>(node, key, 0, false));
                }
            }
            m_CostNum = DBCUtil.ExtractNumeric<int>(node, "CostNum", 0, false);
            m_Rate = DBCUtil.ExtractNumeric<float>(node, "Rate", 1.0f, false);
            return true;
        }
        public int GetId()
        {
            return m_Level;
        }
    }

    public class LegacyLevelupConfigProvider
    {
        private DataDictionaryMgr<LegacyLevelupConfig> m_LegacyLevelupConfigMgr = new DataDictionaryMgr<LegacyLevelupConfig>();

        public void Load(string file, string root)
        {
            m_LegacyLevelupConfigMgr.CollectDataFromDBC(file, root);
        }

        public static LegacyLevelupConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static LegacyLevelupConfigProvider s_Instance = new LegacyLevelupConfigProvider();
    }
}
