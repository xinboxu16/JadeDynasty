using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class ExpeditionTollgateConfig : IData
    {
        public int m_TollgateNum = 0;
        public float m_RelativeScore = 0f;
        public int m_DropOutMoney = 0;
        public float m_MoneyFactor = 0f;
        public const int c_DropOutItemNum = 2;
        public List<int> m_ItemId = new List<int>();
        public List<float> m_ItemProbality = new List<float>();
        public List<float> m_ItemFactor = new List<float>();
        public int m_ImageAiLogic = 0;
        public const int c_FlushPosNum = 5;
        public List<string> m_Pos = new List<string>();

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_TollgateNum = DBCUtil.ExtractNumeric<int>(node, "TollgateNum", 0, true);
            m_RelativeScore = DBCUtil.ExtractNumeric<float>(node, "RelativeScore", 0, true);
            m_DropOutMoney = DBCUtil.ExtractNumeric<int>(node, "DropOutMoney", 0, false);
            m_MoneyFactor = DBCUtil.ExtractNumeric<float>(node, "MoneyFactor", 0, false);
            for (int i = 0; i < c_DropOutItemNum; ++i)
            {
                string key = "DropOutItemId_" + (i + 1).ToString();
                m_ItemId.Add(DBCUtil.ExtractNumeric<int>(node, key, 0, false));
                key = "ItemProbality_" + (i + 1).ToString();
                m_ItemProbality.Add(DBCUtil.ExtractNumeric<float>(node, key, 0, false));
                key = "ItemFactor_" + (i + 1).ToString();
                m_ItemFactor.Add(DBCUtil.ExtractNumeric<float>(node, key, 0, false));
            }
            m_ImageAiLogic = DBCUtil.ExtractNumeric<int>(node, "ImageAiLogic", 0, false);
            for (int i = 0; i < c_FlushPosNum; ++i)
            {
                string key = "FlushPos_" + (i + 1).ToString();
                m_Pos.Add(DBCUtil.ExtractString(node, key, "0.0,0.0,0.0", false));
            }
            return true;
        }
        public int GetId()
        {
            return m_TollgateNum;
        }
    }

    public class ExpeditionTollgateConfigProvider
    {
        private DataDictionaryMgr<ExpeditionTollgateConfig> m_ExpeditionTollgateConfigMgr = new DataDictionaryMgr<ExpeditionTollgateConfig>();

        public void Load(string file, string root)
        {
            m_ExpeditionTollgateConfigMgr.CollectDataFromDBC(file, root);
        }

        public static ExpeditionTollgateConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ExpeditionTollgateConfigProvider s_Instance = new ExpeditionTollgateConfigProvider();
    }
}
