using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    public sealed class StoryConfigManager
    {
        private Dictionary<int, StoryInstance> m_StoryInstances = new Dictionary<int, StoryInstance>();
        private object m_Lock = new object();

        public void LoadStoryIfNotExist(int storyId, int sceneId, params string[] files)
        {
            if(!ExistStory(storyId, sceneId))
            {
                foreach (string file in files)
                {
                    LoadStory(file, sceneId);
                }
            }
        }

        public void LoadStory(string file, int sceneId)
        {
            if (!string.IsNullOrEmpty(file))
            {
                ScriptableData.ScriptableDataFile dataFile = new ScriptableData.ScriptableDataFile();
                if (dataFile.Load(file))
                {
                    Load(dataFile, sceneId);
                }
            }
        }

        private void Load(ScriptableData.ScriptableDataFile dataFile, int sceneId)
        {
            lock (m_Lock)
            {
                foreach (ScriptableData.ScriptableDataInfo info in dataFile.ScriptableDatas)
                {
                    if (info.GetId() == "story" || info.GetId() == "script")
                    {
                        ScriptableData.FunctionData funcData = info.First;
                        if (null != funcData)
                        {
                            ScriptableData.CallData callData = funcData.Call;
                            if (null != callData && callData.HaveParam())
                            {
                                int storyId = int.Parse(callData.GetParamId(0));
                                int id = GenId(storyId, sceneId);
                                if (!m_StoryInstances.ContainsKey(id))
                                {
                                    StoryInstance instance = new StoryInstance();
                                    instance.Init(info);
                                    m_StoryInstances.Add(id, instance);

                                    LogSystem.Debug("ParseStory {0}", id);
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool ExistStory(int storyId, int sceneId)
        {
            int id = GenId(storyId, sceneId);
            return null != GetStoryInstanceResource(id);
        }

        private StoryInstance GetStoryInstanceResource(int id)
        {
            StoryInstance instance = null;
            lock (m_Lock)
            {
                if (m_StoryInstances.ContainsKey(id))
                {
                    instance = m_StoryInstances[id];
                }
            }
            return instance;
        }

        public StoryInstance NewStoryInstance(int storyId, int sceneId)
        {
            StoryInstance instance = null;
            int id = GenId(storyId, sceneId);
            StoryInstance temp = GetStoryInstanceResource(id);
            if (null != temp)
            {
                instance = temp.Clone();
            }
            return instance;
        }

        private static int GenId(int storyId, int sceneId)
        {
            return sceneId * 100 + storyId;
        }

        public void Clear()
        {
            lock (m_Lock)
            {
                m_StoryInstances.Clear();
            }
        }

        public static StoryConfigManager Instance
        {
            get { return s_Instance; }
        }
        private static StoryConfigManager s_Instance = new StoryConfigManager();
    }
}
