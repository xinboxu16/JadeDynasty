using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ImpactLogicManager
    {
        public enum ImpactLogicId
        {
            ImpactLogic_General = 1,
            ImpactLogic_SuperArmor = 2,
            ImpactLogic_Invincible = 3,
            ImpactLogic_ChangeSkill = 4,
            ImpactLogic_StopImpact = 5,
            ImpactLogic_RefixDamage = 6,
            ImpactLogic_BlockAndBeat = 7,
            ImpactLogic_SuperArmorShield = 8,
            ImpactLogic_DamageImmunityShield = 9,
        }

        private Dictionary<int, IImpactLogic> m_ImpactLogics = new Dictionary<int, IImpactLogic>();

        private ImpactLogicManager()
        {
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_General, new ImpactLogic_General());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_SuperArmor, new ImpactLogic_SuperArmor());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Invincible, new ImpactLogic_Invincible());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_ChangeSkill, new ImpactLogic_ChangeSkill());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_StopImpact, new ImpactLogic_StopImpact());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_RefixDamage, new ImpactLogic_RefixDamage());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_BlockAndBeat, new ImpactLogic_BlockAndBeat());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_SuperArmorShield, new ImpactLogic_SuperArmorShield());
            //m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_DamageImmunityShield, new ImpactLogic_DamageImmunityShield());
        }

        public IImpactLogic GetImpactLogic(int id)
        {
            IImpactLogic logic = null;
            if (m_ImpactLogics.ContainsKey(id))
                logic = m_ImpactLogics[id];
            return logic;
        }

        public static ImpactLogicManager Instance
        {
            get { return s_Instance; }
        }

        private static ImpactLogicManager s_Instance = new ImpactLogicManager();
    }
}
