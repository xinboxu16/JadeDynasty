using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum ElementDamageType : int
    {
        DC_None = 0,
        DC_Fire = 1,//火
        DC_Ice = 2,//冰
        DC_Poison = 3//毒
    }

    public enum EquipmentType : int
    {
        E_Weapon = 0,
        E_Clothes = 1,
        E_Hat = 2,
        E_Shoes = 3,
        E_Jewelry = 4,
        E_Ring = 5,
        E_Fashion = 6,
        E_Wing = 7,
        MaxNum
    }

    [Serializable]
    public class ItemAttrDataConfig
    {
        public const int c_MaxAbsoluteValue = 1000000000;
        public const int c_MaxPercentValue = 2000000000;
        public const float c_Rate = 100.0f;
        public enum ValueType : int
        {
            AbsoluteValue = 0,
            PercentValue,
            LevelRateValue,
        }

        private float m_AddHpMax = 0;
        private float m_AddRageMax = 0;
        public int m_HpMaxType = 0;
        private float m_AddEpMax = 0;
        public int m_EpMaxType = 0;
        private float m_AddAd = 0;
        public int m_AdType = 0;
        private float m_AddADp = 0;
        public int m_ADpType = 0;
        private float m_AddMDp = 0;
        public int m_MDpType = 0;
        private float m_AddCri = 0;
        public int m_CriType = 0;
        private float m_AddPow = 0;
        public int m_PowType = 0;
        private float m_AddBackHitPow = 0;
        public int m_BackHitPowType = 0;
        private float m_AddCrackPow = 0;
        public int m_CrackPowType = 0;
        private float m_AddHpRecover = 0;
        public int m_HpRecoverType = 0;
        private float m_AddEpRecover = 0;
        public int m_EpRecoverType = 0;
        private float m_AddSpd = 0;
        public int m_SpdType = 0;
        private float m_AddFireDam = 0;
        public int m_FireDamType = 0;
        private float m_AddFireErd = 0;
        public int m_FireErdType = 0;
        private float m_AddIceDam = 0;
        public int m_IceDamType = 0;
        private float m_AddIceErd = 0;
        public int m_IceErdType = 0;
        private float m_AddPoisonDam = 0;
        public int m_PoisonDamType = 0;
        private float m_AddPoisonErd = 0;
        public int m_PoisonErdType = 0;
        private float m_AddWeight = 0;
        public int m_WeightType = 0;
        private float m_AddRps = 0;
        public int m_RpsType = 0;
        private float m_AddAttackRange = 0;
        public int m_AttackRangeType = 0;
        public int m_ItemLevel = 1;
        public int m_LevelUpAddHpMax = 0;
        public int m_LevelUpAddEpMax = 0;
        public int m_LevelUpAddAd = 0;
        public int m_LevelUpAddADp = 0;
        public int m_LevelUpAddMDp = 0;

        public void CollectDataFromDBC(DBC_Row node)
        {
            m_AddHpMax = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpMax", 0, false), out m_HpMaxType);
            m_AddHpRecover = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpRecover", 0, false), out m_HpRecoverType);
            m_AddRageMax = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRageMax", 0, false), out m_HpMaxType);
            m_AddEpMax = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpMax", 0, false), out m_EpMaxType);
            m_AddEpRecover = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpRecover", 0, false), out m_EpRecoverType);
            m_AddAd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAd", 0, false), out m_AdType);
            m_AddADp = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddADp", 0, false), out m_ADpType);
            m_AddMDp = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddMDp", 0, false), out m_MDpType);
            m_AddCri = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCri", 0, false), out m_CriType);
            m_AddPow = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPow", 0, false), out m_PowType);
            m_AddBackHitPow = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddBackHitPow", 0, false), out m_BackHitPowType);
            m_AddCrackPow = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCrackPow", 0, false), out m_CrackPowType);
            m_AddSpd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddSpd", 0, false), out m_SpdType);
            m_AddFireDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireDam", 0, false), out m_FireDamType);
            m_AddFireErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireErd", 0, false), out m_FireErdType);
            m_AddIceDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceDam", 0, false), out m_IceDamType);
            m_AddIceErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceErd", 0, false), out m_IceErdType);
            m_AddPoisonDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonDam", 0, false), out m_PoisonDamType);
            m_AddPoisonErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonErd", 0, false), out m_PoisonErdType);
            m_AddWeight = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddWeight", 0, false), out m_WeightType);
            m_AddRps = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRps", 0, false), out m_RpsType);
            m_AddAttackRange = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAttackRange", 0, false), out m_AttackRangeType);
            ///
            m_LevelUpAddHpMax = DBCUtil.ExtractNumeric<int>(node, "LevelUpAddHpMax", 0, false);
            m_LevelUpAddEpMax = DBCUtil.ExtractNumeric<int>(node, "LevelUpAddEpMax", 0, false);
            m_LevelUpAddAd = DBCUtil.ExtractNumeric<int>(node, "LevelUpAddAd", 0, false);
            m_LevelUpAddADp = DBCUtil.ExtractNumeric<int>(node, "LevelUpAddADp", 0, false);
            m_LevelUpAddMDp = DBCUtil.ExtractNumeric<int>(node, "LevelUpAddMDp", 0, false);
        }

        public float GetAddHpMax(float refVal, int refLevel, int itemLevel = 1)
        {
            float base_value = CalcAddedAttrValue(refVal, refLevel, m_AddHpMax, m_HpMaxType);
            return base_value + itemLevel * (float)(m_LevelUpAddHpMax / 100.0);
        }
        public float GetAddRageMax(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRageMax, m_HpMaxType);
        }
        public float GetAddEpMax(float refVal, int refLevel, int itemLevel = 1)
        {
            float base_value = CalcAddedAttrValue(refVal, refLevel, m_AddEpMax, m_EpMaxType);
            return base_value + itemLevel * (float)(m_LevelUpAddEpMax / 100.0);
        }
        public float GetAddAd(float refVal, int refLevel, int itemLevel = 1)
        {
            float base_value = CalcAddedAttrValue(refVal, refLevel, m_AddAd, m_AdType);
            return base_value + itemLevel * (float)(m_LevelUpAddAd / 100.0);
        }
        public float GetAddADp(float refVal, int refLevel, int itemLevel = 1)
        {
            float base_value = CalcAddedAttrValue(refVal, refLevel, m_AddADp, m_ADpType);
            return base_value + itemLevel * (float)(m_LevelUpAddADp / 100.0);
        }
        public float GetAddMDp(float refVal, int refLevel, int itemLevel = 1)
        {
            float base_value = CalcAddedAttrValue(refVal, refLevel, m_AddMDp, m_MDpType);
            return base_value + itemLevel * (float)(m_LevelUpAddMDp / 100.0);
        }
        public float GetAddCri(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCri, m_CriType);
        }
        public float GetAddPow(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPow, m_PowType);
        }
        public float GetAddBackHitPow(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddBackHitPow, m_BackHitPowType);
        }
        public float GetAddCrackPow(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCrackPow, m_CrackPowType);
        }
        public float GetAddHpRecover(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddHpRecover, m_HpRecoverType);
        }
        public float GetAddEpRecover(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddEpRecover, m_EpRecoverType);
        }
        public float GetAddSpd(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddSpd, m_SpdType);
        }
        public float GetAddFireDam(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireDam, m_FireDamType);
        }
        public float GetAddFireErd(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireErd, m_FireErdType);
        }
        public float GetAddIceDam(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceDam, m_IceDamType);
        }
        public float GetAddIceErd(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceErd, m_IceErdType);
        }
        public float GetAddPoisonDam(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonDam, m_PoisonDamType);
        }
        public float GetAddPoisonErd(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonErd, m_PoisonErdType);
        }
        public float GetAddWeight(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddWeight, m_WeightType);
        }
        public float GetAddRps(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRps, m_RpsType);
        }
        public float GetAddAttackRange(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddAttackRange, m_AttackRangeType);
        }

        private float CalcAddedAttrValue(float refVal, int refLevel, float addVal, int type)
        {
            float retVal = 0;
            switch (type)
            {
                case (int)ValueType.AbsoluteValue:
                    retVal = addVal;
                    break;
                case (int)ValueType.PercentValue:
                    retVal = refVal * addVal;
                    break;
                case (int)ValueType.LevelRateValue:
                    retVal = refLevel * addVal;
                    break;
            }
            return retVal;
        }

        private float CalcRealValue(int tableValue, out int type)
        {
            float retVal = 0;
            int val = tableValue;
            bool isNegative = false;
            if (tableValue < 0)
            {
                isNegative = true;
                val = -val;
            }
            if (val < c_MaxAbsoluteValue)
            {
                retVal = val / c_Rate;
                type = (int)ValueType.AbsoluteValue;
            }
            else if (val < c_MaxPercentValue)
            {
                retVal = (val - c_MaxAbsoluteValue) / c_Rate / 100;
                type = (int)ValueType.PercentValue;
            }
            else
            {
                retVal = (val - c_MaxPercentValue) / c_Rate;
                type = (int)ValueType.LevelRateValue;
            }
            if (isNegative)
                retVal = -retVal;
            return retVal;
        }
    }

    [Serializable]
    public class ItemConfig : IData
    {
        public int m_ItemId = 0;
        public string m_ItemName = "";
        public string m_ItemTrueName = "";
        public string m_ItemType = "";
        public string m_Description = "";
        public int m_Grade = 0;
        public int m_UseLogicId = 0;
        public string[] m_UseResultData = null;
        public int[] m_AddBuffOnEquiping = null;
        public int[] m_AddSkillOnEquiping = null;
        public bool m_ShowInShop = false;
        public string m_Model = "";
        public string m_UiModel = "";
        public int m_MaxStack = 1;
        public int[] m_ConsumeItems = null;
        public int m_ConsumeMoney = 0;
        public int m_PropertyRank = 0;
        public int m_AddExp = 0;
        public int m_AddMoney = 0;
        public int m_AddBuff = 0;
        public string m_Introduce = "";
        public int m_ItemSkillFirst = 0;
        public int m_ItemSkillSecond = 0;
        public int m_ItemSkillThird = 0;
        public string m_NormalIcon = "";
        public string m_BigIcon = "";
        public ItemAttrDataConfig m_AttrData = new ItemAttrDataConfig();
        public bool m_CanWear = false;
        public int m_WearParts = -1;
        public bool m_CanUpgrade = false;
        public bool m_CanDeve = false;
        public List<int> m_AttachedProperty = new List<int>();
        public string m_Inlay = "";
        public int m_Weight = 0;
        public int m_SellingPrice = 0;
        public float m_SellGainGoldProb = 0;
        public int m_SellGainGoldNum = 0;
        public ElementDamageType m_DamageType = ElementDamageType.DC_None;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_ItemId = DBCUtil.ExtractNumeric<int>(node, "ItemId", 0, true);
            m_ItemName = DBCUtil.ExtractString(node, "ItemName", "", true);
            m_ItemTrueName = DBCUtil.ExtractString(node, "ItemTrueName", "", true);
            m_ItemType = DBCUtil.ExtractString(node, "ItemType", "", true);
            m_Description = DBCUtil.ExtractString(node, "Description", "", false);
            m_Grade = DBCUtil.ExtractNumeric<int>(node, "Grade", 0, false);
            m_UseLogicId = DBCUtil.ExtractNumeric<int>(node, "UseLogicId", 0, false);
            List<string> strList = DBCUtil.ExtractStringList(node, "UseResultData", "", false);
            if (strList.Count > 0)
                m_UseResultData = strList.ToArray();
            List<int> list = DBCUtil.ExtractNumericList<int>(node, "AddBuffOnEquiping", 0, false);
            if (list.Count > 0)
            {
                m_AddBuffOnEquiping = list.ToArray();
            }
            list = DBCUtil.ExtractNumericList<int>(node, "AddSkillOnEquiping", 0, false);
            if (list.Count > 0)
            {
                m_AddSkillOnEquiping = list.ToArray();
            }
            m_ShowInShop = (0 != DBCUtil.ExtractNumeric<int>(node, "ShowInShop", 0, false));
            m_Model = DBCUtil.ExtractString(node, "Model", "", false);
            m_UiModel = DBCUtil.ExtractString(node, "UiModel", "", false);
            m_MaxStack = DBCUtil.ExtractNumeric<int>(node, "MaxStackNum", 1, false);
            list = DBCUtil.ExtractNumericList<int>(node, "ConsumeItems", 0, false);
            if (list.Count > 0)
            {
                m_ConsumeItems = list.ToArray();
            }
            else
            {
                m_ConsumeItems = new int[] { 0, 0, 0 };
            }
            m_ConsumeMoney = DBCUtil.ExtractNumeric<int>(node, "ConsumeMoney", 0, false);
            m_PropertyRank = DBCUtil.ExtractNumeric<int>(node, "PropertyRank", 0, true);
            m_AddExp = DBCUtil.ExtractNumeric<int>(node, "AddExp", 0, false);
            m_AddMoney = DBCUtil.ExtractNumeric<int>(node, "AddMoney", 0, false);
            m_AddBuff = DBCUtil.ExtractNumeric<int>(node, "AddBuffer", 0, false);
            m_Introduce = DBCUtil.ExtractString(node, "Introduce", "", false);
            m_ItemSkillFirst = DBCUtil.ExtractNumeric<int>(node, "ItemSkillFirst", 0, false);
            m_ItemSkillSecond = DBCUtil.ExtractNumeric<int>(node, "ItemSkillSecond", 0, false);
            m_ItemSkillThird = DBCUtil.ExtractNumeric<int>(node, "ItemSkillThird", 0, false);
            m_NormalIcon = DBCUtil.ExtractString(node, "NormalIcon", "", false);
            m_BigIcon = DBCUtil.ExtractString(node, "BigIcon", "", false);
            m_AttrData.CollectDataFromDBC(node);
            m_CanWear = DBCUtil.ExtractNumeric<bool>(node, "CanWear", false, false);
            m_WearParts = DBCUtil.ExtractNumeric<int>(node, "WearParts", -1, false);
            m_CanUpgrade = DBCUtil.ExtractNumeric<bool>(node, "CanUpgrade", false, false);
            m_CanDeve = DBCUtil.ExtractNumeric<bool>(node, "CanDeve", false, false);
            m_AttachedProperty = DBCUtil.ExtractNumericList(node, "AttachedProperty", 0, false);
            m_Inlay = DBCUtil.ExtractString(node, "Inlay", "", false);
            m_Weight = DBCUtil.ExtractNumeric<int>(node, "Weight", -1, false);
            m_SellingPrice = DBCUtil.ExtractNumeric<int>(node, "SellingPrice", 0, false);
            m_SellGainGoldProb = DBCUtil.ExtractNumeric<float>(node, "SellGainGoldProb", 0f, false);
            m_SellGainGoldNum = DBCUtil.ExtractNumeric<int>(node, "SellGainGoldNum", 0, false);
            m_DamageType = (ElementDamageType)DBCUtil.ExtractNumeric<int>(node, "ElementType", 0, false);
            return true;
        }

        public int GetId()
        {
            return m_ItemId;
        }
    }

    public class ItemConfigProvider
    {
        private DataDictionaryMgr<ItemConfig> m_ItemConfigMgr = new DataDictionaryMgr<ItemConfig>();

        public void Load(string file, string root)
        {
            m_ItemConfigMgr.CollectDataFromDBC(file, root);
        }

        public ItemConfig GetDataById(int id)
        {
            return m_ItemConfigMgr.GetDataById(id);
        }

        public static ItemConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ItemConfigProvider s_Instance = new ItemConfigProvider();
    }
}
