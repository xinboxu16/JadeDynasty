using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class SceneContextInfo
    {
        private DashFireSpatial.ISpatialSystem m_SpatialSystem = null;
        private SightManager m_SightManager = null;

        private int m_SceneResId = 0;
        private bool m_IsRunWithRoomServer = true;

        public DashFireSpatial.ISpatialSystem SpatialSystem
        {
            get { return m_SpatialSystem; }
            set { m_SpatialSystem = value; }
        }

        public SightManager SightManager
        {
            get { return m_SightManager; }
            set { m_SightManager = value; }
        }

        public int SceneResId
        {
            get { return m_SceneResId; }
            set { m_SceneResId = value; }
        }

        public bool IsRunWithRoomServer
        {
            get { return m_IsRunWithRoomServer; }
            set { m_IsRunWithRoomServer = value; }
        }
    }
}
