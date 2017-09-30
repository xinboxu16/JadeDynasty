using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class EquipmentInfo
    {
        public const int c_MaxEquipmentNum = 8;
        private ItemDataInfo[] m_BodyArmor = new ItemDataInfo[c_MaxEquipmentNum];//装备数据 Armor盔甲

        public ItemDataInfo[] Armor
        {
            get
            {
                return m_BodyArmor;
            }
        }

        public void Reset()
        {
            for (int ix = 0; ix < c_MaxEquipmentNum; ++ix)
            {
                m_BodyArmor[ix] = null;
            }
        }
    }

    public class EquipmentStateInfo
    {
        public const int c_EquipmentCapacity = 8;

        private ElementDamageType m_DamageType = ElementDamageType.DC_None;
        private EquipmentInfo m_EquipmentInfo = new EquipmentInfo();
        private bool m_EquipmentChanged = false;

        public void SetEquipmentData(int index, ItemDataInfo info)
        {
            if (index >= 0 && index < c_EquipmentCapacity && info != null)
            {
                m_EquipmentInfo.Armor[index] = info;
                m_EquipmentChanged = true;
                if ((int)EquipmentType.E_Weapon == index && null != info.ItemConfig)
                {
                    WeaponDamageType = info.ItemConfig.m_DamageType;
                }
            }
        }

        public EquipmentInfo EquipmentInfo
        {
            get { return m_EquipmentInfo; }
        }

        public bool EquipmentChanged
        {
            get { return m_EquipmentChanged; }
            set { m_EquipmentChanged = value; }
        }

        public ElementDamageType WeaponDamageType
        {
            get { return m_DamageType; }
            set { m_DamageType = value; }
        }
    }
}
