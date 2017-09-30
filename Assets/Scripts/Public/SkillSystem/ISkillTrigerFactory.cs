using ScriptableData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSystem
{
    public interface ISkillTrigerFactory
    {
        ISkillTriger Create(ISyntaxComponent trigerConfig);
    }

    public class SkillTrigerFactoryHelper<T> : ISkillTrigerFactory where T :ISkillTriger, new()
    {
        public ISkillTriger Create(ISyntaxComponent trigerConfig)
        {
            T t = new T();
            t.Init(trigerConfig);
            return t;
        }
    }
}
