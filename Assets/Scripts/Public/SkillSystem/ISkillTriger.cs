using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSystem
{
    public interface ISkillTriger
    {
        //从DSL语言初始化触发器实例
        void Init(ScriptableData.ISyntaxComponent config);
    }
}
