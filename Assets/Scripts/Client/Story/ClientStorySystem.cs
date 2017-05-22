using StorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ClientStorySystem
    {
        private class StoryInstanceInfo
        {
            public int m_StoryId;
            public StoryInstance m_StoryInstance;
            public bool m_IsUsed;
        }

        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();
        private List<StoryInstanceInfo> m_StoryLogicInfos = new List<StoryInstanceInfo>();
        private Dictionary<int, List<StoryInstanceInfo>> m_StoryInstancePool = new Dictionary<int, List<StoryInstanceInfo>>();

        public void Init()
        {
            //注册剧情命令
            //StoryCommandManager.Instance.RegisterCommandFactory("startstory", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
        }

        private void RecycleStorylInstance(StoryInstanceInfo info)
        {
            info.m_StoryInstance.Reset();
            info.m_IsUsed = false;
        }

        public void Reset()
        {
            m_GlobalVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index)
            {
                StoryInstanceInfo info = m_StoryLogicInfos[index];
                if (null != info)
                {
                    RecycleStorylInstance(info);
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }

        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
        }

        #region Sington
        public static ClientStorySystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static ClientStorySystem s_Instance = new ClientStorySystem();
        #endregion
    }
}
