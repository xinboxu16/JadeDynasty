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

        private StoryCommandManager()
        {
            //注册通用命令
            RegisterCommandFactory("=", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("assign", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("inc", new StoryCommandFactoryHelper<CommonCommands.IncCommand>());
            RegisterCommandFactory("dec", new StoryCommandFactoryHelper<CommonCommands.DecCommand>());
            RegisterCommandFactory("propset", new StoryCommandFactoryHelper<CommonCommands.PropSetCommand>());
            RegisterCommandFactory("foreach", new StoryCommandFactoryHelper<CommonCommands.ForeachCommand>());
            RegisterCommandFactory("looplist", new StoryCommandFactoryHelper<CommonCommands.LoopListCommand>());
            RegisterCommandFactory("loop", new StoryCommandFactoryHelper<CommonCommands.LoopCommand>());
            RegisterCommandFactory("wait", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("sleep", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("terminate", new StoryCommandFactoryHelper<CommonCommands.TerminateCommand>());
            RegisterCommandFactory("localmessage", new StoryCommandFactoryHelper<CommonCommands.LocalMessageCommand>());
            RegisterCommandFactory("while", new StoryCommandFactoryHelper<CommonCommands.WhileCommand>());
            RegisterCommandFactory("if", new StoryCommandFactoryHelper<CommonCommands.IfElseCommand>());
            RegisterCommandFactory("log", new StoryCommandFactoryHelper<CommonCommands.LogCommand>());

            //注册通用值与内部函数
            //object
            StoryValueManager.Instance.RegisterValueHandler("propget", new StoryValueFactoryHelper<CommonValues.PropGetValue>());
            StoryValueManager.Instance.RegisterValueHandler("rndint", new StoryValueFactoryHelper<CommonValues.RandomIntValue>());
            StoryValueManager.Instance.RegisterValueHandler("rndfloat", new StoryValueFactoryHelper<CommonValues.RandomFloatValue>());
            StoryValueManager.Instance.RegisterValueHandler("vector2", new StoryValueFactoryHelper<CommonValues.Vector2Value>());
            StoryValueManager.Instance.RegisterValueHandler("vector3", new StoryValueFactoryHelper<CommonValues.Vector3Value>());
            StoryValueManager.Instance.RegisterValueHandler("vector4", new StoryValueFactoryHelper<CommonValues.Vector4Value>());
            StoryValueManager.Instance.RegisterValueHandler("quaternion", new StoryValueFactoryHelper<CommonValues.QuaternionValue>());
            StoryValueManager.Instance.RegisterValueHandler("eular", new StoryValueFactoryHelper<CommonValues.EularValue>());
            StoryValueManager.Instance.RegisterValueHandler("stringlist", new StoryValueFactoryHelper<CommonValues.StringListValue>());
            StoryValueManager.Instance.RegisterValueHandler("intlist", new StoryValueFactoryHelper<CommonValues.IntListValue>());
            StoryValueManager.Instance.RegisterValueHandler("floatlist", new StoryValueFactoryHelper<CommonValues.FloatListValue>());
            StoryValueManager.Instance.RegisterValueHandler("vector2list", new StoryValueFactoryHelper<CommonValues.Vector2ListValue>());
            StoryValueManager.Instance.RegisterValueHandler("vector3list", new StoryValueFactoryHelper<CommonValues.Vector3ListValue>());
            StoryValueManager.Instance.RegisterValueHandler("list", new StoryValueFactoryHelper<CommonValues.ListValue>());
            StoryValueManager.Instance.RegisterValueHandler("+", new StoryValueFactoryHelper<CommonValues.AddOperator>());
            StoryValueManager.Instance.RegisterValueHandler("-", new StoryValueFactoryHelper<CommonValues.SubOperator>());
            StoryValueManager.Instance.RegisterValueHandler("*", new StoryValueFactoryHelper<CommonValues.MulOperator>());
            StoryValueManager.Instance.RegisterValueHandler(">", new StoryValueFactoryHelper<CommonValues.GreaterThanOperator>());
            StoryValueManager.Instance.RegisterValueHandler(">=", new StoryValueFactoryHelper<CommonValues.GreaterEqualThanOperator>());
            StoryValueManager.Instance.RegisterValueHandler("==", new StoryValueFactoryHelper<CommonValues.EqualOperator>());
            StoryValueManager.Instance.RegisterValueHandler("!=", new StoryValueFactoryHelper<CommonValues.NotEqualOperator>());
            StoryValueManager.Instance.RegisterValueHandler("<", new StoryValueFactoryHelper<CommonValues.LessThanOperator>());
            StoryValueManager.Instance.RegisterValueHandler("<=", new StoryValueFactoryHelper<CommonValues.LessEqualThanOperator>());
            StoryValueManager.Instance.RegisterValueHandler("&&", new StoryValueFactoryHelper<CommonValues.AndOperator>());
            StoryValueManager.Instance.RegisterValueHandler("||", new StoryValueFactoryHelper<CommonValues.OrOperator>());
            StoryValueManager.Instance.RegisterValueHandler("!", new StoryValueFactoryHelper<CommonValues.NotOperator>());
            StoryValueManager.Instance.RegisterValueHandler("format", new StoryValueFactoryHelper<CommonValues.FormatValue>());
            StoryValueManager.Instance.RegisterValueHandler("substring", new StoryValueFactoryHelper<CommonValues.SubstringValue>());
            StoryValueManager.Instance.RegisterValueHandler("time", new StoryValueFactoryHelper<CommonValues.TimeValue>());
        }

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

        public IStoryCommand CreateCommand(ScriptableData.ISyntaxComponent commandConfig)
        {
            IStoryCommand command = null;
            string type = commandConfig.GetId();
            IStoryCommandFactory factory = GetFactory(type);
            if (null != factory)
            {
                command = factory.Create(commandConfig);
            }
            else
            {
                DashFire.LogSystem.Debug("CreateCommand failed, unkown type:{0}", type);
            }
            if (null != command)
            {
                //DashFire.LogSystem.Debug("CreateCommand, type:{0} command:{1}", type, command.GetType().Name);
            }
            return command;
        }

        private IStoryCommandFactory GetFactory(string type)
        {
            IStoryCommandFactory factory = null;
            if (m_StoryCommandFactories.ContainsKey(type))
            {
                factory = m_StoryCommandFactories[type];
            }
            return factory;
        }

        public static StoryCommandManager Instance
        {
            get { return s_Instance; }
        }
        private static StoryCommandManager s_Instance = new StoryCommandManager();
    }
}
