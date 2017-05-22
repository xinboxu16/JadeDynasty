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
