using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class AttrCalculateUtility
    {
        public static void ResetBaseProperty(CharacterInfo obj)
        {
            obj.CalcBaseAttr();
        }

        public static void RefixFightingScoreByProperty(CharacterInfo obj)
        {

        }

        public static void RefixAttrByImpact(CharacterInfo obj)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            foreach(ImpactInfo impact in impacts)
            {
                impact.RefixCharacterProperty(obj);
            }
        }

        public static void RefixAttrByLegacy(CharacterInfo obj)
        {
            //计算神器影响
            for (int index = 0; index < LegacyStateInfo.c_LegacyCapacity; ++index)
            {
                ItemDataInfo info = obj.GetLegacyStateInfo().LegacyInfo.Legacy[index];
                if (null != info)
                {
                    float aMoveSpeed = obj.GetActualProperty().MoveSpeed;
                    int aHpMax = obj.GetActualProperty().HpMax;
                    int aEnergyMax = obj.GetActualProperty().EnergyMax;
                    float aHpRecover = obj.GetActualProperty().HpRecover;
                    float aEnergyRecover = obj.GetActualProperty().EnergyRecover;
                    int aAttackBase = obj.GetActualProperty().AttackBase;
                    int aADefenceBase = obj.GetActualProperty().ADefenceBase;
                    int aMDefenceBase = obj.GetActualProperty().MDefenceBase;
                    float aCritical = obj.GetActualProperty().Critical;
                    float aCriticalPow = obj.GetActualProperty().CriticalPow;
                    float aCriticalBackHitPow = obj.GetActualProperty().CriticalBackHitPow;
                    float aCriticalCrackPow = obj.GetActualProperty().CriticalCrackPow;
                    float aFireDamage = obj.GetActualProperty().FireDamage;
                    float aFireERD = obj.GetActualProperty().FireERD;
                    float aIceDamage = obj.GetActualProperty().IceDamage;
                    float aIceERD = obj.GetActualProperty().IceERD;
                    float aPoisonDamage = obj.GetActualProperty().PoisonDamage;
                    float aPoisonERD = obj.GetActualProperty().PoisonERD;
                    float aWeight = obj.GetActualProperty().Weight;
                    float aRps = obj.GetActualProperty().Rps;
                    float aAttackRange = obj.GetActualProperty().AttackRange;

                    obj.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Relative, info.GetAddSpd(aMoveSpeed, obj.GetLevel()));
                    obj.GetActualProperty().SetHpMax(Operate_Type.OT_Relative, (int)info.GetAddHpMax(aHpMax, obj.GetLevel()));
                    obj.GetActualProperty().SetEnergyMax(Operate_Type.OT_Relative, (int)info.GetAddEpMax(aEnergyMax, obj.GetLevel()));
                    obj.GetActualProperty().SetHpRecover(Operate_Type.OT_Relative, info.GetAddHpRecover(aHpRecover, obj.GetLevel()));
                    obj.GetActualProperty().SetEnergyRecover(Operate_Type.OT_Relative, info.GetAddEpRecover(aEnergyRecover, obj.GetLevel()));
                    obj.GetActualProperty().SetAttackBase(Operate_Type.OT_Relative, (int)info.GetAddAd(aAttackBase, obj.GetLevel()));
                    obj.GetActualProperty().SetADefenceBase(Operate_Type.OT_Relative, (int)info.GetAddADp(aADefenceBase, obj.GetLevel()));
                    obj.GetActualProperty().SetMDefenceBase(Operate_Type.OT_Relative, (int)info.GetAddMDp(aMDefenceBase, obj.GetLevel()));
                    obj.GetActualProperty().SetCritical(Operate_Type.OT_Relative, info.GetAddCri(aCritical, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalPow(Operate_Type.OT_Relative, info.GetAddPow(aCriticalPow, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalBackHitPow(Operate_Type.OT_Relative, info.GetAddBackHitPow(aCriticalBackHitPow, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalCrackPow(Operate_Type.OT_Relative, info.GetAddCrackPow(aCriticalCrackPow, obj.GetLevel()));
                    obj.GetActualProperty().SetFireDamage(Operate_Type.OT_Relative, info.GetAddFireDam(aFireDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetFireERD(Operate_Type.OT_Relative, info.GetAddFireErd(aFireERD, obj.GetLevel()));
                    obj.GetActualProperty().SetIceDamage(Operate_Type.OT_Relative, info.GetAddIceDam(aIceDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetIceERD(Operate_Type.OT_Relative, info.GetAddIceErd(aIceERD, obj.GetLevel()));
                    obj.GetActualProperty().SetPoisonDamage(Operate_Type.OT_Relative, info.GetAddPoisonDam(aPoisonDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetPoisonERD(Operate_Type.OT_Relative, info.GetAddPoisonErd(aPoisonERD, obj.GetLevel()));
                    obj.GetActualProperty().SetWeight(Operate_Type.OT_Relative, info.GetAddWeight(aWeight, obj.GetLevel()));
                    obj.GetActualProperty().SetRps(Operate_Type.OT_Relative, info.GetAddRps(aRps, obj.GetLevel()));
                    obj.GetActualProperty().SetAttackRange(Operate_Type.OT_Relative, info.GetAddAttackRange(aAttackRange, obj.GetLevel()));
                }
            }
        }

        public static void RefixAttrByEquipment(CharacterInfo obj)
        {
            //计算装备影响
            for (int index = 0; index < EquipmentInfo.c_MaxEquipmentNum; ++index)
            {
                ItemDataInfo info = obj.GetEquipmentStateInfo().EquipmentInfo.Armor[index];
                if (null != info)
                {
                    float aMoveSpeed = obj.GetActualProperty().MoveSpeed;
                    int aHpMax = obj.GetActualProperty().HpMax;
                    int aEnergyMax = obj.GetActualProperty().EnergyMax;
                    float aHpRecover = obj.GetActualProperty().HpRecover;
                    float aEnergyRecover = obj.GetActualProperty().EnergyRecover;
                    int aAttackBase = obj.GetActualProperty().AttackBase;
                    int aADefenceBase = obj.GetActualProperty().ADefenceBase;
                    int aMDefenceBase = obj.GetActualProperty().MDefenceBase;
                    float aCritical = obj.GetActualProperty().Critical;
                    float aCriticalPow = obj.GetActualProperty().CriticalPow;
                    float aCriticalBackHitPow = obj.GetActualProperty().CriticalBackHitPow;
                    float aCriticalCrackPow = obj.GetActualProperty().CriticalCrackPow;
                    float aFireDamage = obj.GetActualProperty().FireDamage;
                    float aFireERD = obj.GetActualProperty().FireERD;
                    float aIceDamage = obj.GetActualProperty().IceDamage;
                    float aIceERD = obj.GetActualProperty().IceERD;
                    float aPoisonDamage = obj.GetActualProperty().PoisonDamage;
                    float aPoisonERD = obj.GetActualProperty().PoisonERD;
                    float aWeight = obj.GetActualProperty().Weight;
                    float aRps = obj.GetActualProperty().Rps;
                    float aAttackRange = obj.GetActualProperty().AttackRange;

                    obj.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Relative, info.GetAddSpd(aMoveSpeed, obj.GetLevel()));
                    obj.GetActualProperty().SetHpMax(Operate_Type.OT_Relative, (int)info.GetAddHpMax(aHpMax, obj.GetLevel()));
                    obj.GetActualProperty().SetEnergyMax(Operate_Type.OT_Relative, (int)info.GetAddEpMax(aEnergyMax, obj.GetLevel()));
                    obj.GetActualProperty().SetHpRecover(Operate_Type.OT_Relative, info.GetAddHpRecover(aHpRecover, obj.GetLevel()));
                    obj.GetActualProperty().SetEnergyRecover(Operate_Type.OT_Relative, info.GetAddEpRecover(aEnergyRecover, obj.GetLevel()));
                    obj.GetActualProperty().SetAttackBase(Operate_Type.OT_Relative, (int)info.GetAddAd(aAttackBase, obj.GetLevel()));
                    obj.GetActualProperty().SetADefenceBase(Operate_Type.OT_Relative, (int)info.GetAddADp(aADefenceBase, obj.GetLevel()));
                    obj.GetActualProperty().SetMDefenceBase(Operate_Type.OT_Relative, (int)info.GetAddMDp(aMDefenceBase, obj.GetLevel()));
                    obj.GetActualProperty().SetCritical(Operate_Type.OT_Relative, info.GetAddCri(aCritical, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalPow(Operate_Type.OT_Relative, info.GetAddPow(aCriticalPow, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalBackHitPow(Operate_Type.OT_Relative, info.GetAddBackHitPow(aCriticalBackHitPow, obj.GetLevel()));
                    obj.GetActualProperty().SetCriticalCrackPow(Operate_Type.OT_Relative, info.GetAddCrackPow(aCriticalCrackPow, obj.GetLevel()));
                    obj.GetActualProperty().SetFireDamage(Operate_Type.OT_Relative, info.GetAddFireDam(aFireDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetFireERD(Operate_Type.OT_Relative, info.GetAddFireErd(aFireERD, obj.GetLevel()));
                    obj.GetActualProperty().SetIceDamage(Operate_Type.OT_Relative, info.GetAddIceDam(aIceDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetIceERD(Operate_Type.OT_Relative, info.GetAddIceErd(aIceERD, obj.GetLevel()));
                    obj.GetActualProperty().SetPoisonDamage(Operate_Type.OT_Relative, info.GetAddPoisonDam(aPoisonDamage, obj.GetLevel()));
                    obj.GetActualProperty().SetPoisonERD(Operate_Type.OT_Relative, info.GetAddPoisonErd(aPoisonERD, obj.GetLevel()));
                    obj.GetActualProperty().SetWeight(Operate_Type.OT_Relative, info.GetAddWeight(aWeight, obj.GetLevel()));
                    obj.GetActualProperty().SetRps(Operate_Type.OT_Relative, info.GetAddRps(aRps, obj.GetLevel()));
                    obj.GetActualProperty().SetAttackRange(Operate_Type.OT_Relative, info.GetAddAttackRange(aAttackRange, obj.GetLevel()));
                }
            }
        }
    }
}
