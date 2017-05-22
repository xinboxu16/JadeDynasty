using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class SceneLogicInfoManager
    {
        private int m_SceneLogicInfoPoolSize = 1024;
        private const int c_StartId = 40000;
        private const int c_StartId_Client = 50000;
        private const int c_MaxIdNum = 10000;
        private int m_NextInfoId = c_StartId;

        private LinkedListDictionary<int, SceneLogicInfo> m_SceneLogicInfos = new LinkedListDictionary<int, SceneLogicInfo>();
        private List<SceneLogicInfo> m_DelayAdd = new List<SceneLogicInfo>();
        private Queue<SceneLogicInfo> m_UnusedSceneLogicInfos = new Queue<SceneLogicInfo>();

        public SceneLogicInfoManager(int poolSize)
        {
            m_SceneLogicInfoPoolSize = poolSize;
        }

        public void Reset()
        {
            m_SceneLogicInfos.Clear();
            m_DelayAdd.Clear();
            m_UnusedSceneLogicInfos.Clear();
            m_NextInfoId = c_StartId;
        }
    }
}
