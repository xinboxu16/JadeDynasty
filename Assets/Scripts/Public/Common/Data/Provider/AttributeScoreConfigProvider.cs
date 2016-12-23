using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class AttributeScoreConfig : IData
    {
        public int m_Id = 0;
        public string m_Name = "";
        public float m_Score = 0;
        public float m_BaseValue = 0;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_Name = DBCUtil.ExtractString(node, "Name", "", true);
            m_Score = DBCUtil.ExtractNumeric<float>(node, "Score", 0, false);
            m_BaseValue = DBCUtil.ExtractNumeric<float>(node, "BaseValue", 0, false);
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class AttributeScoreConfigProvider
    {
        private DataDictionaryMgr<AttributeScoreConfig> m_AttributeScoreConfigMgr = new DataDictionaryMgr<AttributeScoreConfig>();

        public void Load(string file, string root)
        {
            m_AttributeScoreConfigMgr.CollectDataFromDBC(file, root);
        }

        public static AttributeScoreConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static AttributeScoreConfigProvider s_Instance = new AttributeScoreConfigProvider();
    }
}
