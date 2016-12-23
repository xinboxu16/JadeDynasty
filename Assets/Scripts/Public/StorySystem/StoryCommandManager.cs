using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    /// <summary>
    /// 这个类不加锁，约束条件：所有命令注册必须在程序启动时完成。
    /// </summary>
    public sealed class StoryCommandManager
    {
        private Dictionary<string, IStoryCommandFactory> m_StoryCommandFactories = new Dictionary<string, IStoryCommandFactory>();

        public void RegisterCommandFactory(string type, IStoryCommandFactory factory)
        {
            if(!m_StoryCommandFactories.ContainsKey(type))
            {
                m_StoryCommandFactories.Add(type, factory);
            }else
            {
                //error
            }
        }

        public static StoryCommandManager Instance
        {
            get { return s_Instance; }
        }
        private static StoryCommandManager s_Instance = new StoryCommandManager();
    }
}
