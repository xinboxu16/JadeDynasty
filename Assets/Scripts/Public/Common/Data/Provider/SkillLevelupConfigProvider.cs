using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class SkillLevelupConfig : IData
    {
        public int m_Level = 0;
        public const int TypeNum = 10;
        public List<int> m_TypeList = new List<int>();
        public float m_Rate = 0;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Level = DBCUtil.ExtractNumeric<int>(node, "Level", 0, true);
            for (int i = 0; i < TypeNum; ++i)
            {
                string key = "SkillType_" + (i + 1).ToString();
                m_TypeList.Add(DBCUtil.ExtractNumeric<int>(node, key, 0, false));
            }
            m_Rate = DBCUtil.ExtractNumeric<float>(node, "Rate", 1.0f, false);
            return true;
        }
        public int GetId()
        {
            return m_Level;
        }
    }

    public class SkillLevelupConfigProvider
    {
        private DataDictionaryMgr<SkillLevelupConfig> m_SkillLevelupConfigMgr = new DataDictionaryMgr<SkillLevelupConfig>();

        public void Load(string file, string root)
        {
            m_SkillLevelupConfigMgr.CollectDataFromDBC(file, root);
        }

        public static SkillLevelupConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static SkillLevelupConfigProvider s_Instance = new SkillLevelupConfigProvider();
    }
}
