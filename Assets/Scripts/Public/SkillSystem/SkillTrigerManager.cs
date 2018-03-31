using ScriptableData;
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

        public SkillTrigerManager() { }

        public void RegisterTrigerFactory(string type, ISkillTrigerFactory factory)
        {
            if (!m_TrigerFactories.ContainsKey(type))
            {
                m_TrigerFactories.Add(type, factory);
            }
        }

        private ISkillTrigerFactory GetFactory(string type)
        {
            ISkillTrigerFactory factory = null;
            if (m_TrigerFactories.ContainsKey(type))
            {
                factory = m_TrigerFactories[type];
            }
            return factory;
        }

        public ISkillTriger CreateTriger(ISyntaxComponent trigerConfig)
        {
            ISkillTriger triger = null;
            string type = trigerConfig.GetId();
            ISkillTrigerFactory factory = GetFactory(type);
            if(null != factory)
            {
                triger = factory.Create(trigerConfig);
            }
            else
            {
                DashFire.LogSystem.Error("CreateTriger failed, unkown type:{0}", type);
            }
            if (null != triger)
            {
                //DashFire.LogSystem.Debug("CreateTriger, type:{0} triger:{1}", type, triger.GetType().Name);
            }
            return triger;
        }
    }
}
