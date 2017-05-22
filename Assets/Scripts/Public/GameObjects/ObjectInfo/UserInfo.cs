using DashFireSpatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class UserInfo : CharacterInfo
    {
        public UserInfo(int id):base(id)
        {
            m_SpaceObject = new SpaceObjectImpl(this, SpatialObjType.kUser);
            m_CastUserInfo = this;
        }
    }
}
