using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    /// <summary>
    /// 剧情命令接口，剧情脚本的基本单位（有的命令是复合命令，由基本命令构成）。
    /// 命令中使用的值由IStoryValue<T>接口描述，用以支持参数、局部变量与内建函数（返回一个剧情命令用到的值）。
    /// </summary>
    public interface IStoryCommand
    {

    }
}
