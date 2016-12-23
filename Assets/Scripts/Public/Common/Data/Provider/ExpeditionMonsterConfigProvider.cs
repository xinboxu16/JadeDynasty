using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum MonsterType : int
    {
        MT_Normal = 0,
        MT_Boss,
        MT_Other,
    }
    [Serializable]
    public class ExpeditionMonsterConfig : IData
    {
        public int m_Id = 0;
        public List<int> m_LinkId = new List<int>();
        public int m_FightingScore = 0;
        public MonsterType m_Type = MonsterType.MT_Other;
        public List<int> m_AttributeId = new List<int>();
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_LinkId = DBCUtil.ExtractNumericList<int>(node, "LinkId", 0, true);
            m_FightingScore = DBCUtil.ExtractNumeric<int>(node, "FightingScore", 0, true);
            m_Type = (MonsterType)DBCUtil.ExtractNumeric<int>(node, "MonsterType", 0, true);
            m_AttributeId = DBCUtil.ExtractNumericList<int>(node, "AttributeId", 0, true);
            return true;
        }
        public int GetId()
        {
            return m_Id;
        }
    }

    public class ExpeditionMonsterConfigProvider
    {
        private DataDictionaryMgr<ExpeditionMonsterConfig> m_ExpeditionMonsterConfigMgr = new DataDictionaryMgr<ExpeditionMonsterConfig>();

        public void Load(string file, string root)
        {
            m_ExpeditionMonsterConfigMgr.CollectDataFromDBC(file, root);
        }

        public static ExpeditionMonsterConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ExpeditionMonsterConfigProvider s_Instance = new ExpeditionMonsterConfigProvider();
    }
}
