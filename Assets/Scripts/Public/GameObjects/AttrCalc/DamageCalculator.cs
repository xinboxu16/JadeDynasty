using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum SkillDamageType : int
    {
        DC_Ad = 0,
        DC_Ap = 1,
        MaxNum
    }
    public sealed class DamageCalculator
    {
        public static int CalcImpactDamage(CharacterInfo sender,
                                           CharacterInfo receiver,
                                           SkillDamageType damageType,
                                           ElementDamageType elementType,
                                           float attackFactor,
                                           int damageFromConfig,
                                           out bool isCritical)
        {
            isCritical = false;
            if (null == sender || null == receiver) return damageFromConfig;

            int ad = sender.GetActualProperty().AttackBase;
            int adp = receiver.GetActualProperty().ADefenceBase;
            int mdp = receiver.GetActualProperty().MDefenceBase;
            int level = receiver.GetLevel();
            float fd = sender.GetActualProperty().FireDamage;
            float fe = receiver.GetActualProperty().FireERD;
            float id = sender.GetActualProperty().IceDamage;
            float ie = receiver.GetActualProperty().IceERD;
            float pd = sender.GetActualProperty().PoisonDamage;
            float pe = receiver.GetActualProperty().PoisonERD;
            float c = sender.GetActualProperty().Critical;
            float cp = sender.GetActualProperty().CriticalPow;
            float cbhp = sender.GetActualProperty().CriticalBackHitPow;
            float ccp = sender.GetActualProperty().CriticalCrackPow;
            float sd = sender.GetMovementStateInfo().GetFaceDir();
            float rd = receiver.GetMovementStateInfo().GetFaceDir();
            if (level <= 0) return damageFromConfig;

            float baseDamage = 0f;
            float elementDamage = 0f;
            float normalDamage = 0f;
            float skillDamage = 0f;
            float totalDamage = damageFromConfig;
            /// base
            if (SkillDamageType.DC_Ad == damageType)
            {
                if (IsInvalid(adp, level))
                {
                    baseDamage = 0;
                }
                else
                {
                    baseDamage = ad * (1 - (adp / (float)level) / (float)(10 + Math.Abs(adp) / (float)level));
                }
            }
            else if (SkillDamageType.DC_Ap == damageType)
            {
                if (IsInvalid(mdp, level))
                {
                    baseDamage = 0;
                }
                else
                {
                    baseDamage = ad * (1 - (mdp / (float)level) / (float)(10 + Math.Abs(mdp) / (float)level));
                }
            }
            /// element
            if (ElementDamageType.DC_Fire == elementType)
            {
                if (IsInvalid(fe, level))
                {
                    elementDamage = 0;
                }
                else
                {
                    elementDamage = fd * (1 - (fe / (float)level) / (float)(10 + Math.Abs(fe) / (float)level));
                }
            }
            else if (ElementDamageType.DC_Ice == elementType)
            {
                if (IsInvalid(ie, level))
                {
                    elementDamage = 0;
                }
                else
                {
                    elementDamage = id * (1 - (ie / (float)level) / (float)(10 + Math.Abs(ie) / (float)level));
                }
            }
            else if (ElementDamageType.DC_Poison == elementType)
            {
                if (IsInvalid(pe, level))
                {
                    elementDamage = 0;
                }
                else
                {
                    elementDamage = pd * (1 - (pe / (float)level) / (float)(10 + Math.Abs(pe) / (float)level));
                }
            }
            /// normal
            normalDamage = (baseDamage + elementDamage) * attackFactor;
            /// skill
            if (IsInvalid(adp, level))
            {
                skillDamage = 0;
            }
            else
            {
                skillDamage = damageFromConfig * (1 - (adp / (float)level) / (float)(10 + Math.Abs(adp) / (float)level));
            }
            /// total
            totalDamage = normalDamage + skillDamage;
            /// critical
            int num = Helper.Random.Next(0, 100);
            float random = (float)(num * 0.01);
            if (random < c)
            {
                isCritical = true;
                totalDamage *= cp;
            }
            /// check backhit
            if (IsBackHit(sd, rd))
            {
                totalDamage *= cbhp;
            }
            /// ckeck crack
            if (IsCrack(receiver))
            {
                totalDamage *= ccp;
            }

            return (int)totalDamage;
        }

        private static bool IsInvalid(float value, int level)
        {
            if (0 == (10 + Math.Abs(value) / (float)level))
            {
                return true;
            }
            return false;
        }

        private static bool IsBackHit(float firstDir, float secondDir)
        {
            if (Math.Abs(firstDir - secondDir) < (Math.PI / 6))
            {
                return true;
            }
            return false;
        }

        private static bool IsCrack(CharacterInfo receiver)
        {
            if (null != receiver && null != receiver.GetSkillStateInfo()
              && receiver.GetSkillStateInfo().IsSkillActivated())
            {
                return true;
            }
            return false;
        }
    }
}