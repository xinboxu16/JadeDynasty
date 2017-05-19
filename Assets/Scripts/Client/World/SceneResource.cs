using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SceneResource
    {
        private bool m_IsSuccessEnter = false;

        public bool IsSuccessEnter
        {
            get { return m_IsSuccessEnter; }
        }

        private Data_SceneConfig m_SceneConfig;

        public bool IsMultiPve
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_MULTI_PVE;
            }
        }
        public bool IsPvp
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_PVP;
            }
        }

        private int m_SceneResId;

        public int ResId
        {
            get
            {
                return m_SceneResId;
            }
        }

        public bool IsServerSelectScene
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_SERVER_SELECT;
            }
        }
    }
}
