using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum SceneLogicId
    {
        USER_ENTER_AREA = 10001,
        REVIVE_POINT = 10002,
        TIME_OUT = 10003,
        AREA_DETECT = 10004,
    }

    public class SceneLogicInfo
    {
        private SceneContextInfo m_SceneContext = null;
        private SceneLogicConfig m_SceneLogicConfig = null;
        private int m_Id = 0;
        private long m_Time = 0;//由于场景逻辑主要在Tick里工作，通常需要限制工作的频率，这一数据用于此目的（由于LogicDatas的读取比较费，所以抽出来放公共里）
        private bool m_IsLogicFinished = false;
        private TypedDataCollection m_LogicDatas = new TypedDataCollection();

        public SceneLogicInfo(int id)
        {
            m_Id = id;
            m_IsLogicFinished = false;
        }

        public int GetId()
        {
            return m_Id;
        }

        public void InitId(int id)
        {
            m_Id = id;
        }

        public void Reset()
        {
            m_Time = 0;
            m_IsLogicFinished = false;
            m_LogicDatas.Clear();
        }

        public SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
            set { m_SceneContext = value; }
        }

        public SceneLogicConfig SceneLogicConfig
        {
            get { return m_SceneLogicConfig; }
            set { m_SceneLogicConfig = value; }
        }

        public bool IsLogicFinished
        {
            get { return m_IsLogicFinished; }
            set { m_IsLogicFinished = value; }
        }

        public long Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        public int LogicId
        {
            get
            {
                int id = 0;
                if (null != m_SceneLogicConfig)
                {
                    id = m_SceneLogicConfig.m_LogicId;
                }
                return id;
            }
        }

        public TypedDataCollection LogicDatas
        {
            get { return m_LogicDatas; }
        }

        public DashFireSpatial.ISpatialSystem SpatialSystem
        {
            get
            {
                DashFireSpatial.ISpatialSystem sys = null;
                if (null != m_SceneContext)
                {
                    sys = m_SceneContext.SpatialSystem;
                }
                return sys;
            }
        }

        public UserManager UserManager
        {
            get
            {
                UserManager mgr = null;
                if (null != m_SceneContext)
                {
                    mgr = m_SceneContext.UserManager;
                }
                return mgr;
            }
        }

        public int ConfigId
        {
            get
            {
                int id = 0;
                if (null != m_SceneLogicConfig)
                {
                    id = m_SceneLogicConfig.GetId();
                }
                return id;
            }
        }
    }
}
