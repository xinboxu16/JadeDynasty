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
