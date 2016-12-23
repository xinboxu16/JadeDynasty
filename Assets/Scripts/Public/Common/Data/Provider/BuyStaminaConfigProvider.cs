using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class BuyStaminaConfig : IData
    {
        public int m_BuyCount = 0;
        public int m_CostGold = 0;
        public int m_GainStamina = 0;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_BuyCount = DBCUtil.ExtractNumeric<int>(node, "BuyCount", 0, true);
            m_CostGold = DBCUtil.ExtractNumeric<int>(node, "CostGold", 0, true);
            m_GainStamina = DBCUtil.ExtractNumeric<int>(node, "GainStamina", 0, true);
            return true;
        }
        public int GetId()
        {
            return m_BuyCount;
        }
    }

    //Stamina体力
    public class BuyStaminaConfigProvider
    {
        private DataDictionaryMgr<BuyStaminaConfig> m_BuyStaminaConfigMgr = new DataDictionaryMgr<BuyStaminaConfig>();

        public void Load(string file, string root)
        {
            m_BuyStaminaConfigMgr.CollectDataFromDBC(file, root);
        }

        public static BuyStaminaConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static BuyStaminaConfigProvider s_Instance = new BuyStaminaConfigProvider();
    }
}
