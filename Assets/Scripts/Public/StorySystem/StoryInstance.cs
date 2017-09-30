using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    public sealed class StoryMessageHandler
    {
        private bool m_IsTriggered = false;
        private string m_MessageId = "";
        private object[] m_Arguments = null;

        private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();

        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();

        public string MessageId
        {
            get { return m_MessageId; }
        }

        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }

        public void Prepare()
        {
            Reset();
            foreach (IStoryCommand cmd in m_LoadedCommands)
            {
                m_CommandQueue.Enqueue(cmd);
            }
        }

        public void Trigger(StoryInstance instance, object[] args)
        {
            Prepare();
            m_IsTriggered = true;
            m_Arguments = args;
            foreach (IStoryCommand cmd in m_CommandQueue)
            {
                cmd.Prepare(instance, args.Length, args);
            }
        }

        public void Load(ScriptableData.FunctionData messageHandlerData)
        {
            ScriptableData.CallData callData = messageHandlerData.Call;
            if (null != callData && callData.HaveParam())
            {
                string[] args = new string[callData.GetParamNum()];
                for (int i = 0; i < callData.GetParamNum(); ++i)
                {
                    args[i] = callData.GetParamId(i);
                }
                m_MessageId = string.Join(":", args);
            }
            RefreshCommands(messageHandlerData);
        }

        private void RefreshCommands(ScriptableData.FunctionData handlerData)
        {
            m_LoadedCommands.Clear();
            foreach (ScriptableData.ISyntaxComponent data in handlerData.Statements)
            {
                IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(data);
                if (null != cmd)
                {
                    m_LoadedCommands.Add(cmd);

                    //LogSystem.Debug("AddCommand:{0}", cmd.GetType().Name);
                }
            }
        }

        public void Tick(StoryInstance instance, long delta)
        {
            while (m_CommandQueue.Count > 0)
            {
                IStoryCommand cmd = m_CommandQueue.Peek();
                if (cmd.Execute(instance, delta))
                {
                    break;
                }
                else
                {
                    cmd.Reset();
                    m_CommandQueue.Dequeue();
                }
            }
            if (m_CommandQueue.Count == 0)
            {
                m_IsTriggered = false;
            }
        }

        public StoryMessageHandler Clone()
        {
            StoryMessageHandler handler = new StoryMessageHandler();
            foreach (IStoryCommand cmd in m_LoadedCommands)
            {
                handler.m_LoadedCommands.Add(cmd.Clone());
            }
            handler.m_MessageId = m_MessageId;
            return handler;
        }

        public void Reset()
        {
            m_IsTriggered = false;
            foreach (IStoryCommand cmd in m_CommandQueue)
            {
                cmd.Reset();
            }
            m_CommandQueue.Clear();
        }
    }
    public sealed class StoryInstance
    {
        private class MessageInfo
        {
            public string m_MsgId = null;
            public object[] m_Args = null;
        }

        private bool m_IsTerminated = false;
        private int m_StoryId = 0;
        private object m_Context = null;
        private long m_CurTime = 0;
        private long m_LastTickTime = 0;
        private Dictionary<string, object> m_GlobalVariables = null;
        private Queue<MessageInfo> m_MessageQueue = new Queue<MessageInfo>();
        private List<StoryMessageHandler> m_MessageHandlers = new List<StoryMessageHandler>();

        private TypedDataCollection m_CustomDatas = new TypedDataCollection();

        private Dictionary<string, object> m_LocalVariables = new Dictionary<string, object>();

        public object Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        public Dictionary<string, object> GlobalVariables
        {
            get { return m_GlobalVariables; }
            set { m_GlobalVariables = value; }
        }

        public Dictionary<string, object> LocalVariables
        {
            get { return m_LocalVariables; }
        }

        public int StoryId
        {
            get { return m_StoryId; }
        }

        public bool IsTerminated
        {
            get { return m_IsTerminated; }
            set { m_IsTerminated = value; }
        }

        public void Start()
        {
            m_LastTickTime = 0;
            m_CurTime = 0;
            SendMessage("start");
        }

        public bool Init(ScriptableData.ScriptableDataInfo config)
        {
            bool ret = false;
            ScriptableData.FunctionData story = config.First;
            if (null != story && (story.GetId() == "story" || story.GetId() == "script"))
            {
                ret = true;
                ScriptableData.CallData callData = story.Call;
                if (null != callData && callData.HaveParam())
                {
                    m_StoryId = int.Parse(callData.GetParamId(0));
                }
                foreach (ScriptableData.ISyntaxComponent info in story.Statements)
                {
                    if (info.GetId() == "local")
                    {
                        ScriptableData.FunctionData sectionData = info as ScriptableData.FunctionData;
                        if (null != sectionData)
                        {
                            foreach (ScriptableData.ISyntaxComponent def in sectionData.Statements)
                            {
                                ScriptableData.CallData defData = def as ScriptableData.CallData;
                                if (null != defData && defData.HaveId() && defData.HaveParam())
                                {
                                    string id = defData.GetId();
                                    if (id.StartsWith("@") && !id.StartsWith("@@"))
                                    {
                                        StoryValue val = new StoryValue();
                                        val.InitFromDsl(defData.GetParam(0));
                                        if (!m_LocalVariables.ContainsKey(id))
                                        {
                                            m_LocalVariables.Add(id, val.Value);
                                        }
                                        else
                                        {
                                            m_LocalVariables[id] = val.Value;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            LogSystem.Error("Story {0} DSL, local must be a function !", m_StoryId);
                        }
                    }
                    else if (info.GetId() == "onmessage")
                    {
                        ScriptableData.FunctionData sectionData = info as ScriptableData.FunctionData;
                        if (null != sectionData)
                        {
                            StoryMessageHandler handler = new StoryMessageHandler();
                            handler.Load(sectionData);
                            m_MessageHandlers.Add(handler);
                        }
                        else
                        {
                            LogSystem.Error("Story {0} DSL, onmessage must be a function !", m_StoryId);
                        }
                    }
                    else
                    {
                        LogSystem.Error("StoryInstance::Init, unknown part {0}", info.GetId());
                    }
                }
            }
            else
            {
                LogSystem.Error("StoryInstance::Init, isn't story DSL");
            }
            //LogSystem.Debug("StoryInstance.Init message handler num:{0} {1}", m_MessageHandlers.Count, ret);
            return ret;
        }

        public void Tick(long curTime)
        {
            long delta = 0;
            if (m_LastTickTime == 0)
            {
                m_LastTickTime = curTime;
            }
            else
            {
                delta = curTime - m_LastTickTime;
                m_LastTickTime = curTime;
                m_CurTime += delta;
            }
            int ct = m_MessageHandlers.Count;
            if (m_MessageQueue.Count > 0)
            {
                int cantTriggerCount = 0;
                int triggerCount = 0;
                MessageInfo msgInfo = m_MessageQueue.Peek();
                for (int ix = ct - 1; ix >= 0; --ix)
                {
                    StoryMessageHandler handler = m_MessageHandlers[ix];
                    if (handler.MessageId == msgInfo.m_MsgId)
                    {
                        if (handler.IsTriggered)
                        {
                            ++cantTriggerCount;
                        }
                        else
                        {
                            handler.Trigger(this, msgInfo.m_Args);
                            ++triggerCount;
                        }
                    }
                }
                if (cantTriggerCount == 0 || triggerCount > 0)
                {
                    m_MessageQueue.Dequeue();
                }
            }
            for (int ix = ct - 1; ix >= 0; --ix)
            {
                StoryMessageHandler handler = m_MessageHandlers[ix];
                if (handler.IsTriggered)
                {
                    handler.Tick(this, delta);
                }
            }
        }

        public void SendMessage(string msgId, params object[] args)
        {
            MessageInfo msgInfo = new MessageInfo();
            msgInfo.m_MsgId = msgId;
            msgInfo.m_Args = args;
            m_MessageQueue.Enqueue(msgInfo);
        }

        public void Reset()
        {
            m_IsTerminated = false;
            m_MessageQueue.Clear();
            int ct = m_MessageHandlers.Count;
            for (int i = ct - 1; i >= 0; --i)
            {
                StoryMessageHandler handler = m_MessageHandlers[i];
                handler.Reset();
            }
            m_CustomDatas.Clear();
        }

        public StoryInstance Clone()
        {
            StoryInstance instance = new StoryInstance();
            foreach (string key in m_LocalVariables.Keys)
            {
                instance.m_LocalVariables.Add(key, m_LocalVariables[key]);
            }
            foreach (StoryMessageHandler handler in m_MessageHandlers)
            {
                instance.m_MessageHandlers.Add(handler.Clone());
            }
            instance.m_StoryId = m_StoryId;
            return instance;
        }
    }
}
