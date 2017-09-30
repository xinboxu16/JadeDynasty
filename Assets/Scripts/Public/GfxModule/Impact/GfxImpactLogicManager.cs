using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GfxModule.Impact
{
    public sealed class GfxImpactLogicManager
    {
        public enum GfxImpactLogicId
        {
            GfxImpactLogic_Default = 0,
            GfxImpactLogic_Stiffness = 1,
            GfxImpactLogic_HitFly = 2,
            GfxImpactLogic_KnockDown = 3,
            GfxImpactLogic_Grab = 4,
        }

        private Dictionary<int, IGfxImpactLogic> m_GfxImpactLogics = new Dictionary<int, IGfxImpactLogic>();

        public IGfxImpactLogic GetGfxImpactLogic(int id)
        {
            IGfxImpactLogic logic = null;
            if(m_GfxImpactLogics.ContainsKey(id))
            {
                logic = m_GfxImpactLogics[id];
            }
            return logic;
        }

        public static GfxImpactLogicManager Instance
        {
            get { return s_Instance; }
        }

        private static GfxImpactLogicManager s_Instance = new GfxImpactLogicManager();
    }
}
