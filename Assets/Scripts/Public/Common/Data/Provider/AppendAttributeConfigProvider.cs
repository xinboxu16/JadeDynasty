using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    [Serializable]
    public class AppendAttributeConfig : IData
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

        private float m_AddRageMax1 = 0;
        public int m_RageMaxType1 = 0;
        private float m_AddHpMax1 = 0;
        public int m_HpMaxType1 = 0;
        private float m_AddEpMax1 = 0;
        public int m_EpMaxType1 = 0;
        private float m_AddAd1 = 0;
        public int m_AdType1 = 0;
        private float m_AddADp1 = 0;
        public int m_ADpType1 = 0;
        private float m_AddMDp1 = 0;
        public int m_MDpType1 = 0;
        private float m_AddCri1 = 0;
        public int m_CriType1 = 0;
        private float m_AddPow1 = 0;
        public int m_PowType1 = 0;
        private float m_AddBackHitPow1 = 0;
        public int m_BackHitPowType1 = 0;
        private float m_AddCrackPow1 = 0;
        public int m_CrackPowType1 = 0;
        private float m_AddHpRecover1 = 0;
        public int m_HpRecoverType1 = 0;
        private float m_AddEpRecover1 = 0;
        public int m_EpRecoverType1 = 0;
        private float m_AddSpd1 = 0;
        public int m_SpdType1 = 0;
        private float m_AddFireDam1 = 0;
        public int m_FireDamType1 = 0;
        private float m_AddFireErd1 = 0;
        public int m_FireErdType1 = 0;
        private float m_AddIceDam1 = 0;
        public int m_IceDamType1 = 0;
        private float m_AddIceErd1 = 0;
        public int m_IceErdType1 = 0;
        private float m_AddPoisonDam1 = 0;
        public int m_PoisonDamType1 = 0;
        private float m_AddPoisonErd1 = 0;
        public int m_PoisonErdType1 = 0;
        private float m_AddWeight1 = 0;
        public int m_WeightType1 = 0;
        private float m_AddRps1 = 0;
        public int m_RpsType1 = 0;
        private float m_AddAttackRange1 = 0;
        public int m_AttackRangeType1 = 0;
        private float m_AddRageMax2 = 0;
        public int m_RageMaxType2 = 0;
        private float m_AddHpMax2 = 0;
        public int m_HpMaxType2 = 0;
        private float m_AddEpMax2 = 0;
        public int m_EpMaxType2 = 0;
        private float m_AddAd2 = 0;
        public int m_AdType2 = 0;
        private float m_AddADp2 = 0;
        public int m_ADpType2 = 0;
        private float m_AddMDp2 = 0;
        public int m_MDpType2 = 0;
        private float m_AddCri2 = 0;
        public int m_CriType2 = 0;
        private float m_AddPow2 = 0;
        public int m_PowType2 = 0;
        private float m_AddBackHitPow2 = 0;
        public int m_BackHitPowType2 = 0;
        private float m_AddCrackPow2 = 0;
        public int m_CrackPowType2 = 0;
        private float m_AddHpRecover2 = 0;
        public int m_HpRecoverType2 = 0;
        private float m_AddEpRecover2 = 0;
        public int m_EpRecoverType2 = 0;
        private float m_AddSpd2 = 0;
        public int m_SpdType2 = 0;
        private float m_AddFireDam2 = 0;
        public int m_FireDamType2 = 0;
        private float m_AddFireErd2 = 0;
        public int m_FireErdType2 = 0;
        private float m_AddIceDam2 = 0;
        public int m_IceDamType2 = 0;
        private float m_AddIceErd2 = 0;
        public int m_IceErdType2 = 0;
        private float m_AddPoisonDam2 = 0;
        public int m_PoisonDamType2 = 0;
        private float m_AddPoisonErd2 = 0;
        public int m_PoisonErdType2 = 0;
        private float m_AddWeight2 = 0;
        public int m_WeightType2 = 0;
        private float m_AddRps2 = 0;
        public int m_RpsType2 = 0;
        private float m_AddAttackRange2 = 0;
        public int m_AttackRangeType2 = 0;

        public int m_Id;
        public string m_Describe;
        public int m_Color;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_AddHpMax1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpMax1", 0, false), out m_HpMaxType1);
            m_AddHpRecover1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpRecover1", 0, false), out m_HpRecoverType1);
            m_AddRageMax1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRageMax1", 0, false), out m_RageMaxType1);
            m_AddEpMax1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpMax1", 0, false), out m_EpMaxType1);
            m_AddEpRecover1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpRecover1", 0, false), out m_EpRecoverType1);
            m_AddAd1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAd1", 0, false), out m_AdType1);
            m_AddADp1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddADp1", 0, false), out m_ADpType1);
            m_AddMDp1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddMDp1", 0, false), out m_MDpType1);
            m_AddCri1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCri1", 0, false), out m_CriType1);
            m_AddPow1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPow1", 0, false), out m_PowType1);
            m_AddBackHitPow1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddBackHitPow1", 0, false), out m_BackHitPowType1);
            m_AddCrackPow1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCrackPow1", 0, false), out m_CrackPowType1);
            m_AddSpd1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddSpd1", 0, false), out m_SpdType1);
            m_AddFireDam1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireDam1", 0, false), out m_FireDamType1);
            m_AddFireErd1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireErd1", 0, false), out m_FireErdType1);
            m_AddIceDam1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceDam1", 0, false), out m_IceDamType1);
            m_AddIceErd1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceErd1", 0, false), out m_IceErdType1);
            m_AddPoisonDam1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonDam1", 0, false), out m_PoisonDamType1);
            m_AddPoisonErd1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonErd1", 0, false), out m_PoisonErdType1);
            m_AddWeight1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddWeight1", 0, false), out m_WeightType1);
            m_AddRps1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRps1", 0, false), out m_RpsType1);
            m_AddAttackRange1 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAttackRange1", 0, false), out m_AttackRangeType1);
            m_AddHpMax2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpMax2", 0, false), out m_HpMaxType2);
            m_AddHpRecover2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddHpRecover2", 0, false), out m_HpRecoverType2);
            m_AddRageMax2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRageMax2", 0, false), out m_RageMaxType2);
            m_AddEpMax2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpMax2", 0, false), out m_EpMaxType2);
            m_AddEpRecover2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddEpRecover2", 0, false), out m_EpRecoverType2);
            m_AddAd2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAd2", 0, false), out m_AdType2);
            m_AddADp2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddADp2", 0, false), out m_ADpType2);
            m_AddMDp2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddMDp2", 0, false), out m_MDpType2);
            m_AddCri2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCri2", 0, false), out m_CriType2);
            m_AddPow2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPow2", 0, false), out m_PowType2);
            m_AddBackHitPow2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddBackHitPow2", 0, false), out m_BackHitPowType2);
            m_AddCrackPow2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddCrackPow2", 0, false), out m_CrackPowType2);
            m_AddSpd2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddSpd2", 0, false), out m_SpdType2);
            m_AddFireDam2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireDam2", 0, false), out m_FireDamType2);
            m_AddFireErd2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddFireErd2", 0, false), out m_FireErdType2);
            m_AddIceDam2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceDam2", 0, false), out m_IceDamType2);
            m_AddIceErd2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddIceErd2", 0, false), out m_IceErdType2);
            m_AddPoisonDam2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonDam2", 0, false), out m_PoisonDamType2);
            m_AddPoisonErd2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddPoisonErd2", 0, false), out m_PoisonErdType2);
            m_AddWeight2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddWeight2", 0, false), out m_WeightType2);
            m_AddRps2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddRps2", 0, false), out m_RpsType2);
            m_AddAttackRange2 = CalcRealValue(DBCUtil.ExtractNumeric<int>(node, "AddAttackRange2", 0, false), out m_AttackRangeType2);

            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_Describe = DBCUtil.ExtractString(node, "Describe", "", false);
            m_Color = DBCUtil.ExtractNumeric<int>(node, "Color", 0, false);
            return true;
        }

        public float GetAddHpMax1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddHpMax1, m_HpMaxType1);
        }
        public float GetAddRageMax1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRageMax1, m_RageMaxType1);
        }
        public float GetAddEpMax1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddEpMax1, m_EpMaxType1);
        }
        public float GetAddAd1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddAd1, m_AdType1);
        }
        public float GetAddADp1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddADp1, m_ADpType1);
        }
        public float GetAddMDp1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddMDp1, m_MDpType1);
        }
        public float GetAddCri1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCri1, m_CriType1);
        }
        public float GetAddPow1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPow1, m_PowType1);
        }
        public float GetAddBackHitPow1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddBackHitPow1, m_BackHitPowType1);
        }
        public float GetAddCrackPow1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCrackPow1, m_CrackPowType1);
        }
        public float GetAddHpRecover1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddHpRecover1, m_HpRecoverType1);
        }
        public float GetAddEpRecover1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddEpRecover1, m_EpRecoverType1);
        }
        public float GetAddSpd1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddSpd1, m_SpdType1);
        }
        public float GetAddFireDam1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireDam1, m_FireDamType1);
        }
        public float GetAddFireErd1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireErd1, m_FireErdType1);
        }
        public float GetAddIceDam1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceDam1, m_IceDamType1);
        }
        public float GetAddIceErd1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceErd1, m_IceErdType1);
        }
        public float GetAddPoisonDam1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonDam1, m_PoisonDamType1);
        }
        public float GetAddPoisonErd1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonErd1, m_PoisonErdType1);
        }
        public float GetAddWeight1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddWeight1, m_WeightType1);
        }
        public float GetAddRps1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRps1, m_RpsType1);
        }
        public float GetAddAttackRange1(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddAttackRange1, m_AttackRangeType1);
        }
        public float GetAddHpMax2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddHpMax2, m_HpMaxType2);
        }
        public float GetAddRageMax2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRageMax2, m_RageMaxType2);
        }
        public float GetAddEpMax2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddEpMax2, m_EpMaxType2);
        }
        public float GetAddAd2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddAd2, m_AdType2);
        }
        public float GetAddADp2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddADp2, m_ADpType2);
        }
        public float GetAddMDp2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddMDp2, m_MDpType2);
        }
        public float GetAddCri2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCri2, m_CriType2);
        }
        public float GetAddPow2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPow2, m_PowType2);
        }
        public float GetAddBackHitPow2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddBackHitPow2, m_BackHitPowType2);
        }
        public float GetAddCrackPow2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddCrackPow2, m_CrackPowType2);
        }
        public float GetAddHpRecover2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddHpRecover2, m_HpRecoverType2);
        }
        public float GetAddEpRecover2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddEpRecover2, m_EpRecoverType2);
        }
        public float GetAddSpd2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddSpd2, m_SpdType2);
        }
        public float GetAddFireDam2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireDam2, m_FireDamType2);
        }
        public float GetAddFireErd2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddFireErd2, m_FireErdType2);
        }
        public float GetAddIceDam2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceDam2, m_IceDamType2);
        }
        public float GetAddIceErd2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddIceErd2, m_IceErdType2);
        }
        public float GetAddPoisonDam2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonDam2, m_PoisonDamType2);
        }
        public float GetAddPoisonErd2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddPoisonErd2, m_PoisonErdType2);
        }
        public float GetAddWeight2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddWeight2, m_WeightType2);
        }
        public float GetAddRps2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddRps2, m_RpsType2);
        }
        public float GetAddAttackRange2(float refVal, int refLevel)
        {
            return CalcAddedAttrValue(refVal, refLevel, m_AddAttackRange2, m_AttackRangeType2);
        }
        public float GetAddHpMax(float refVal, int refLevel)
        {
            return GetAddHpMax1(refVal, refLevel) + GetAddHpMax2(refVal, refLevel);
        }
        public float GetAddRageMax(float refVal, int refLevel)
        {
            return GetAddRageMax1(refVal, refLevel) + GetAddRageMax2(refVal, refLevel);
        }
        public float GetAddEpMax(float refVal, int refLevel)
        {
            return GetAddEpMax1(refVal, refLevel) + GetAddEpMax2(refVal, refLevel);
        }
        public float GetAddAd(float refVal, int refLevel)
        {
            return GetAddAd1(refVal, refLevel) + GetAddAd2(refVal, refLevel);
        }
        public float GetAddADp(float refVal, int refLevel)
        {
            return GetAddADp1(refVal, refLevel) + GetAddADp2(refVal, refLevel);
        }
        public float GetAddMDp(float refVal, int refLevel)
        {
            return GetAddMDp1(refVal, refLevel) + GetAddMDp2(refVal, refLevel);
        }
        public float GetAddCri(float refVal, int refLevel)
        {
            return GetAddCri1(refVal, refLevel) + GetAddCri2(refVal, refLevel);
        }
        public float GetAddPow(float refVal, int refLevel)
        {
            return GetAddPow1(refVal, refLevel) + GetAddPow2(refVal, refLevel);
        }
        public float GetAddBackHitPow(float refVal, int refLevel)
        {
            return GetAddBackHitPow1(refVal, refLevel) + GetAddBackHitPow2(refVal, refLevel);
        }
        public float GetAddCrackPow(float refVal, int refLevel)
        {
            return GetAddCrackPow1(refVal, refLevel) + GetAddCrackPow2(refVal, refLevel);
        }
        public float GetAddHpRecover(float refVal, int refLevel)
        {
            return GetAddHpRecover1(refVal, refLevel) + GetAddHpRecover2(refVal, refLevel);
        }
        public float GetAddEpRecover(float refVal, int refLevel)
        {
            return GetAddEpRecover1(refVal, refLevel) + GetAddEpRecover2(refVal, refLevel);
        }
        public float GetAddSpd(float refVal, int refLevel)
        {
            return GetAddSpd1(refVal, refLevel) + GetAddSpd2(refVal, refLevel);
        }
        public float GetAddFireDam(float refVal, int refLevel)
        {
            return GetAddFireDam1(refVal, refLevel) + GetAddFireDam2(refVal, refLevel);
        }
        public float GetAddFireErd(float refVal, int refLevel)
        {
            return GetAddFireErd1(refVal, refLevel) + GetAddFireErd2(refVal, refLevel);
        }
        public float GetAddIceDam(float refVal, int refLevel)
        {
            return GetAddIceDam1(refVal, refLevel) + GetAddIceDam2(refVal, refLevel);
        }
        public float GetAddIceErd(float refVal, int refLevel)
        {
            return GetAddIceErd1(refVal, refLevel) + GetAddIceErd2(refVal, refLevel);
        }
        public float GetAddPoisonDam(float refVal, int refLevel)
        {
            return GetAddPoisonDam1(refVal, refLevel) + GetAddPoisonDam2(refVal, refLevel);
        }
        public float GetAddPoisonErd(float refVal, int refLevel)
        {
            return GetAddPoisonErd1(refVal, refLevel) + GetAddPoisonErd2(refVal, refLevel);
        }
        public float GetAddWeight(float refVal, int refLevel)
        {
            return GetAddWeight1(refVal, refLevel) + GetAddWeight2(refVal, refLevel);
        }
        public float GetAddRps(float refVal, int refLevel)
        {
            return GetAddRps1(refVal, refLevel) + GetAddRps2(refVal, refLevel);
        }
        public float GetAddAttackRange(float refVal, int refLevel)
        {
            return GetAddAttackRange1(refVal, refLevel) + GetAddAttackRange2(refVal, refLevel);
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

        public int GetId()
        {
            return m_Id;
        }
    }

    public class AppendAttributeConfigProvider
    {
        private DataDictionaryMgr<AppendAttributeConfig> m_AppendAttributeConfigMgr = new DataDictionaryMgr<AppendAttributeConfig>();

        public void Load(string file, string root)
        {
            m_AppendAttributeConfigMgr.CollectDataFromDBC(file, root);
        }

        public AppendAttributeConfig GetDataById(int id)
        {
            return m_AppendAttributeConfigMgr.GetDataById(id);
        }

        public static AppendAttributeConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static AppendAttributeConfigProvider s_Instance = new AppendAttributeConfigProvider();
    }
}
