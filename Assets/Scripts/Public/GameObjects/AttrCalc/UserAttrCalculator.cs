using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class UserAttrCalculator
    {
        public static void Calc(UserInfo user)
        {
            AttrCalculateUtility.ResetBaseProperty(user);
            AttrCalculateUtility.RefixAttrByEquipment(user);
            AttrCalculateUtility.RefixAttrByLegacy(user);
            AttrCalculateUtility.RefixAttrByImpact(user);
            AttrCalculateUtility.RefixFightingScoreByProperty(user);

            int hpMax = user.GetActualProperty().HpMax;
            user.GetActualProperty().SetHpMax(Operate_Type.OT_Absolute, (int)(user.HpMaxCoefficient * hpMax));//Coefficient 系数
            int mpMax = user.GetActualProperty().EnergyMax;
            user.GetActualProperty().SetEnergyMax(Operate_Type.OT_Absolute, (int)(user.EnergyMaxCoefficient * mpMax));
        }
    }
}
