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
        void Init(ScriptableData.ISyntaxComponent config);//从DSL语言初始化命令实例

        IStoryCommand Clone();//克隆一个新实例，每个命令只从DSL语言初始化一次，之后的实例由克隆产生，提升性能

        void Reset();//复位实例，保证实例状态为初始状态。

        void Prepare(StoryInstance instance, object iterator, object[] args);//准备执行，处理参数与一些上下文相关且在执行过程中不再更新的依赖

        bool Execute(StoryInstance instance, long delta);//执行命令，包括处理变量及命令逻辑
    }

    public abstract class AbstractStoryCommand : IStoryCommand
    {
        private bool m_IsCompositeCommand = false;//混合命令
        private bool m_LastExecResult = false;

        protected virtual void ResetState() { }
        public abstract IStoryCommand Clone();
        protected virtual void Load(ScriptableData.CallData callData) { }
        protected virtual void Load(ScriptableData.FunctionData funcData) { }
        protected virtual void Load(ScriptableData.StatementData statementData) { }
        protected virtual void UpdateArguments(object iterator, object[] args) { }
        protected virtual void UpdateVariables(StoryInstance instance) { }
        protected virtual bool ExecCommand(StoryInstance instance, long delta)
        {
            return false;
        }

        public void Init(ScriptableData.ISyntaxComponent config)
        {
            ScriptableData.CallData callData = config as ScriptableData.CallData;
            if (null != callData)
            {
                Load(callData);
            }
            else
            {
                ScriptableData.FunctionData funcData = config as ScriptableData.FunctionData;
                if (null != funcData)
                {
                    Load(funcData);
                }
                else
                {
                    ScriptableData.StatementData statementData = config as ScriptableData.StatementData;
                    if (null != statementData)
                    {
                        //是否支持语句类型的命令语法？
                        Load(statementData);
                    }
                    else
                    {
                        //error
                    }
                }
            }
        }

        public bool Execute(StoryInstance instance, long delta)
        {
            if (!m_LastExecResult || IsCompositeCommand)
            {
                //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
                UpdateVariables(instance);
            }
            m_LastExecResult = ExecCommand(instance, delta);
            return m_LastExecResult;
        }

        public void Prepare(StoryInstance instance, object iterator, object[] args)
        {
            UpdateArguments(iterator, args);
        }

        public void Reset()
        {
            m_LastExecResult = false;
            ResetState();
        }

        public bool IsCompositeCommand
        {
            get { return m_IsCompositeCommand; }
            protected set { m_IsCompositeCommand = value; }
        }
    }
}
