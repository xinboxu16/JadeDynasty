using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class BuyMoneyConfig : IData
    {
        public int m_BuyCount = 0;
        public int m_CostGold = 0;
        public int m_GainMoney = 0;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_BuyCount = DBCUtil.ExtractNumeric<int>(node, "BuyCount", 0, true);
            m_CostGold = DBCUtil.ExtractNumeric<int>(node, "CostGold", 0, true);
            m_GainMoney = DBCUtil.ExtractNumeric<int>(node, "GainMoney", 0, true);
            return true;
        }
        public int GetId()
        {
            return m_BuyCount;
        }
    }

    public class BuyMoneyConfigProvider
    {
        private DataDictionaryMgr<BuyMoneyConfig> m_BuyMoneyConfigMgr = new DataDictionaryMgr<BuyMoneyConfig>();

        public void Load(string file, string root)
        {
            m_BuyMoneyConfigMgr.CollectDataFromDBC(file, root);
        }

        public static BuyMoneyConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static BuyMoneyConfigProvider s_Instance = new BuyMoneyConfigProvider();
    }
}
