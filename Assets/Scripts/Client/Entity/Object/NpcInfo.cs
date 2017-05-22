using DashFireSpatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum NpcTypeEnum
    {
        Normal = 0,
        Skill,
        Mecha,
        Horse,
        InteractiveNpc,
        PvpTower,
        AutoPickItem,
        Task,
        BigBoss,
        LittleBoss,
        SceneObject,
        Friend,
    }

    public class NpcInfo : CharacterInfo
    {
        protected NpcInfo m_CastNpcInfo = null;

        public NpcInfo(int id):base(id)
        {
            m_SpaceObject = new SpaceObjectImpl(this, SpatialObjType.kNPC);
            m_CastNpcInfo = this;
        }

        private int m_SummonOwnerId = -1;
        private int m_NpcType = 0;

        public int SummonOwnerId
        {
            get { return m_SummonOwnerId; }
            set { m_SummonOwnerId = value; }
        }

        public int NpcType
        {
            get { return m_NpcType; }
        }
    }
}
