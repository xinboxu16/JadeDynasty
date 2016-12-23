using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    /**
     * 属性表格填表规则：
     * 1、0～1000000000，表示此值为直接添加到属性上的值，用整数表示2位精度浮点数，实际值为表格中的值/100（如9999表示99.99，这个值直接加到对应属性值）
     * 2、1000000000~2000000000，表示此值为百分比加成，用整数表示2位精度浮点百分数值，实际值为表格中的值/10000（如:9999表示99.99%，实际值为0.9999，这个值将乘以对应属性后再加到对应属性值）
     * 3、2000000000~，表示此值为按角色等级加成系数，用整数表示2位精度浮点值，实际值为表格中的值/100（如9999表示99.99，这个值将乘以角色等级后再加到对应属性值）
     * 4、规则1～3中对应范围值的负值表示与1～3规则相同，只不过最后会从对应属性值扣除相应加成数
     */
    [Serializable]
    public class AttrDataConfig
    {
        public enum ValueType : int
        {
            AbsoluteValue = 0,
            PercentValue,
            LevelRateValue,
        }

        public const int c_MaxAbsoluteValue = 1000000000;
        public const int c_MaxPercentValue = 2000000000;
        public const float c_Rate = 100.0f;

        private float m_AddHpMax = 0;
        private float m_AddRageMax = 0;
        private int m_HpMaxType = 0;
        private float m_AddEpMax = 0;
        private int m_EpMaxType = 0;
        private float m_AddAd = 0;
        private int m_AdType = 0;
        private float m_AddADp = 0;
        private int m_ADpType = 0;
        private float m_AddMDp = 0;
        private int m_MDpType = 0;
        private float m_AddCri = 0;
        private int m_CriType = 0;
        private float m_AddPow = 0;
        private int m_PowType = 0;
        private float m_AddBackHitPow = 0;
        private int m_BackHitPowType = 0;
        private float m_AddCrackPow = 0;
        private int m_CrackPowType = 0;
        private float m_AddHpRecover = 0;
        private int m_HpRecoverType = 0;
        private float m_AddEpRecover = 0;
        private int m_EpRecoverType = 0;
        private float m_AddSpd = 0;
        private float m_AddWalkSpd = 0;
        private float m_AddRunSpd = 0;
        private int m_SpdType = 0;
        private int m_WalkSpdType = 0;
        private int m_RunSpdType = 0;
        private float m_AddFireDam = 0;
        private int m_FireDamType = 0;
        private float m_AddFireErd = 0;
        private int m_FireErdType = 0;
        private float m_AddIceDam = 0;
        private int m_IceDamType = 0;
        private float m_AddIceErd = 0;
        private int m_IceErdType = 0;
        private float m_AddPoisonDam = 0;
        private int m_PoisonDamType = 0;
        private float m_AddPoisonErd = 0;
        private int m_PoisonErdType = 0;
        private float m_AddWeight = 0;
        private int m_WeightType = 0;
        private float m_AddRps = 0;
        private int m_RpsType = 0;
        private float m_AddAttackRange = 0;
        private int m_AttackRangeType = 0;

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
            m_AddWalkSpd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddWalkSpd", 0, false), out m_WalkSpdType);
            m_AddRunSpd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddSpd", 0, false), out m_RunSpdType);
            m_AddFireDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireDam", 0, false), out m_FireDamType);
            m_AddFireErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireErd", 0, false), out m_FireErdType);
            m_AddIceDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceDam", 0, false), out m_IceDamType);
            m_AddIceErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceErd", 0, false), out m_IceErdType);
            m_AddPoisonDam = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonDam", 0, false), out m_PoisonDamType);
            m_AddPoisonErd = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonErd", 0, false), out m_PoisonErdType);
            m_AddWeight = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddWeight", 0, false), out m_WeightType);
            m_AddRps = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRps", 0, false), out m_RpsType);
            m_AddAttackRange = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAttackRange", 0, false), out m_AttackRangeType);
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
            //格式化
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
}
