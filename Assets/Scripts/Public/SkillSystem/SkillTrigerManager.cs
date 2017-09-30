using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSystem
{
    /// <summary>
    /// 这个类不加锁，约束条件：所有触发器的注册必须在程序启动时完成。
    /// </summary>
    public class SkillTrigerManager : Singleton<SkillTrigerManager>
    {
        private Dictionary<string, ISkillTrigerFactory> m_TrigerFactories = new Dictionary<string, ISkillTrigerFactory>();

        private SkillTrigerManager() { }

        public void RegisterTrigerFactory(string type, ISkillTrigerFactory factory)
        {
            if (!m_TrigerFactories.ContainsKey(type))
            {
                m_TrigerFactories.Add(type, factory);
            }
        }
    }
}
