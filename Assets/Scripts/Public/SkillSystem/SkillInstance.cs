using DashFire;
using ScriptableData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSystem
{
    public sealed class SkillSection
    {
        private long m_Duration = 0;
        private long m_CurTime = 0;
        private bool m_IsFinished = true;

        private List<ISkillTriger> m_Trigers = new List<ISkillTriger>();
        private List<ISkillTriger> m_LoadedTrigers = new List<ISkillTriger>();

        public void Load(ScriptableData.FunctionData sectionData, int skillId)
        {
            ScriptableData.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam())
            {
                if (callData.GetParamNum() > 0)
                {
                    m_Duration = long.Parse(callData.GetParamId(0));
                }
                else
                {
                    m_Duration = 1000;
                    LogSystem.Error("Skill {0} DSL, section must have a time argument !", skillId);
                }
            }
            RefreshTrigers(sectionData, skillId);
        }

        public void Tick(object sender, SkillInstance instance, long delta)
        {
            if (m_IsFinished)
            {
                return;
            }
            m_CurTime += delta;
            int ct = m_Trigers.Count;
            for(int i = ct - 1; i >= 0; --i)
            {
                ISkillTriger triger = m_Trigers[i];
                if (!triger.Execute(sender, instance, delta, m_CurTime / 1000))
                {
                    triger.Reset();
                    m_Trigers.RemoveAt(i);
                }
            }
            if(m_CurTime / 1000 > m_Duration)
            {
                m_IsFinished = true;
            }
        }

        private void RefreshTrigers(ScriptableData.FunctionData sectionData, int skillId)
        {
            m_LoadedTrigers.Clear();
            foreach (ISyntaxComponent data in sectionData.Statements)
            {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(data);
                if (null != triger)
                {
                    m_LoadedTrigers.Add(triger);

                    //LogSystem.Debug("AddTriger:{0}", triger.GetType().Name);
                }
            }
        }

        public void Prepare()
        {
            foreach(ISkillTriger triger in m_Trigers)
            {
                triger.Reset();
            }

            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = false;
            foreach(ISkillTriger triger in m_LoadedTrigers)
            {
                m_Trigers.Add(triger);
            }
            m_Trigers.Sort((left, right) => { 
                if(left.GetStartTime() > right.GetStartTime())
                {
                    return -1;
                }
                else if(left.GetStartTime() == right.GetStartTime())
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
        }
        public void Reset()
        {
            foreach (ISkillTriger triger in m_Trigers)
            {
                triger.Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            m_IsFinished = true;
        }

        public SkillSection Clone()
        {
            SkillSection section = new SkillSection();
            foreach (ISkillTriger triger in m_LoadedTrigers)
            {
                section.m_LoadedTrigers.Add(triger.Clone());
            }
            section.m_Duration = m_Duration;
            return section;
        }

        public long Duration
        {
            get { return m_Duration; }
        }

        public long CurTime
        {
            get { return m_CurTime; }
        }

        public bool IsFinished
        {
            get { return m_IsFinished; }
        }
    }

    public class SkillMessageHandler
    {
        private string m_MsgId = "";
        private long m_CurTime = 0;
        private bool m_IsTriggered = false;

        private List<ISkillTriger> m_Trigers = new List<ISkillTriger>();
        private List<ISkillTriger> m_LoadedTrigers = new List<ISkillTriger>();

        public void Load(ScriptableData.FunctionData sectionData, int skillId)
        {
            ScriptableData.CallData callData = sectionData.Call;
            if (null != callData && callData.HaveParam())
            {
                string[] args = new string[callData.GetParamNum()];
                for (int i = 0; i < callData.GetParamNum(); ++i)
                {
                    args[i] = callData.GetParamId(i);
                }
                m_MsgId = string.Join(":", args);
            }
            RefreshTrigers(sectionData, skillId);
        }

        private void RefreshTrigers(FunctionData sectionData, int skillId)
        {
            m_LoadedTrigers.Clear();
            foreach (ISyntaxComponent data in sectionData.Statements)
            {
                ISkillTriger triger = SkillTrigerManager.Instance.CreateTriger(data);
                if (null != triger)
                {
                    m_LoadedTrigers.Add(triger);
                }
            }
        }

        public void Prepare()
        {
            foreach (ISkillTriger triger in m_Trigers)
            {
                triger.Reset();
            }
            m_Trigers.Clear();
            m_CurTime = 0;
            foreach (ISkillTriger triger in m_LoadedTrigers)
            {
                m_Trigers.Add(triger);
            }
            m_Trigers.Sort((left, right) =>
            {
                if (left.GetStartTime() > right.GetStartTime())
                {
                    return -1;
                }
                else if (left.GetStartTime() == right.GetStartTime())
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
        }

        public void Tick(object sender, SkillInstance instance, long delta)
        {
            m_CurTime += delta;
            int ct = m_Trigers.Count;
            for(int i = ct - 1; i >= 0; --i)
            {
                ISkillTriger triger = m_Trigers[i];
                if(!triger.Execute(sender, instance, delta, m_CurTime/1000))
                {
                    triger.Reset();
                    m_Trigers.RemoveAt(i);
                    if(m_Trigers.Count == 0)
                    {
                        m_IsTriggered = false;
                    }
                }
            }
        }

        public bool IsOver()
        {
            return m_Trigers.Count <= 0 ? true : false;
        }

        public void Reset()
        {
            m_IsTriggered = false;
            m_CurTime = 0;
            foreach (ISkillTriger triger in m_Trigers)
            {
                triger.Reset();
            }
            m_Trigers.Clear();
        }

        public SkillMessageHandler Clone()
        {
            SkillMessageHandler section = new SkillMessageHandler();
            foreach (ISkillTriger triger in m_LoadedTrigers)
            {
                section.m_LoadedTrigers.Add(triger.Clone());
            }
            section.m_MsgId = m_MsgId;
            return section;
        }

        public string MsgId
        {
            get { return m_MsgId; }
        }

        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }
    }

    public class SkillInstance
    {
        private int m_SkillId = 0;
        private List<SkillSection> m_Sections = new List<SkillSection>();
        private List<SkillMessageHandler> m_MessageHandlers = new List<SkillMessageHandler>();
        private Queue<string> m_MessageQueue = new Queue<string>();
        private SkillMessageHandler m_StopSection = null;
        private SkillMessageHandler m_InterruptSection = null;

        private bool m_IsControlMove = false;
        private bool m_IsInterrupted = false;
        private bool m_IsFinished = false;
        private bool m_IsCurveMoveEnable = true;
        private bool m_IsRotateEnable = true;
        private bool m_IsDamageEnable = false;

        private int m_CurSection = -1;
        private long m_CurSectionDuration = 0;
        private long m_CurSectionTime = 0;
        private long m_CurTime = 0;
        private int m_GoToSectionId = -1;

        private long m_OrigDelta = 0;
        private float m_MoveScale = 1;
        private float m_TimeScale = 1;
        private float m_EffectScale = 1;
        private bool m_IsStopCurSection = false;

        private TypedDataCollection m_CustomDatas = new TypedDataCollection();

        public bool Init(ScriptableData.ScriptableDataInfo config)
        {
            bool ret = false;
            FunctionData skill = config.First;
            if (null != skill && skill.GetId() == "skill")
            {
                ret = true;
                CallData callData = skill.Call;
                if (null != callData && callData.HaveParam())
                {
                    m_SkillId = int.Parse(callData.GetParamId(0));
                }
                foreach (ISyntaxComponent info in skill.Statements)
                {
                     if (info.GetId() == "section")
                     {
                         FunctionData sectionData = info as FunctionData;
                         if (null != sectionData)
                         {
                             SkillSection section = new SkillSection();
                             section.Load(sectionData, m_SkillId);
                             m_Sections.Add(section);
                         }
                         else
                         {
                             LogSystem.Error("Skill {0} DSL, section must be a function !", m_SkillId);
                         }
                     }
                     else if (info.GetId() == "onmessage")
                     {
                         FunctionData sectionData = info as FunctionData;
                         if (null != sectionData)
                         {
                             SkillMessageHandler handler = new SkillMessageHandler();
                             handler.Load(sectionData, m_SkillId);
                             m_MessageHandlers.Add(handler);
                         }
                         else
                         {
                             LogSystem.Error("Skill {0} DSL, onmessage must be a function !", m_SkillId);
                         }
                     }
                     else if (info.GetId() == "onstop")
                     {
                         FunctionData sectionData = info as FunctionData;
                         if (null != sectionData)
                         {
                             m_StopSection = new SkillMessageHandler();
                             m_StopSection.Load(sectionData, m_SkillId);
                         }
                         else
                         {
                             LogSystem.Error("Skill {0} DSL, onstop must be a function !", m_SkillId);
                         }
                     }
                     else if (info.GetId() == "oninterrupt")
                     {
                         ScriptableData.FunctionData sectionData = info as ScriptableData.FunctionData;
                         if (null != sectionData)
                         {
                             m_InterruptSection = new SkillMessageHandler();
                             m_InterruptSection.Load(sectionData, m_SkillId);
                         }
                         else
                         {
                             LogSystem.Error("Skill {0} DSL, oninterrupt must be a function !", m_SkillId);
                         }
                     }
                     else
                     {
                         LogSystem.Error("SkillInstance::Init, unknown part {0}", info.GetId());
                     }
                }
            }
            else
            {
                LogSystem.Error("SkillInstance::Init, isn't skill DSL");
            }

            return ret;
        }

        public void Tick(object sender, long deltaTime)
        {
            if (m_IsFinished)
            {
                return;
            }

            m_OrigDelta = deltaTime;
            long delta = (long)(deltaTime * m_TimeScale);
            m_CurSectionTime += delta;
            m_CurTime += delta;

            TickMessageHandlers(sender, delta);
            if (!IsSectionDone(m_CurSection))
            {
                m_Sections[m_CurSection].Tick(sender, this, delta);
            }

            // 改变section任务
            if(m_GoToSectionId > 0)
            {
                ResetCurSection();
                ChangeToSection(m_GoToSectionId);
                m_GoToSectionId = -1;
            }

            if (m_IsStopCurSection)
            {
                m_IsStopCurSection = false;
                ResetCurSection();
                ChangeToSection(m_CurSection + 1);
            }

            if (IsSectionDone(m_CurSection) && m_CurSection < m_Sections.Count - 1)
            {
                ResetCurSection();
                ChangeToSection(m_CurSection + 1);
            }

            if (IsMessageDone() && IsAllSectionDone())
            {
                OnSkillStop(sender, delta);
            }
        }

        private void TickMessageHandlers(object sender, long delta)
        {
            if(m_MessageQueue.Count > 0)
            {
                int cantTriggerCount = 0;
                int triggerCount = 0;
                string msgId = m_MessageQueue.Peek();
                foreach (SkillMessageHandler handler in m_MessageHandlers)
                {
                    if(handler.MsgId == msgId)
                    {
                        if (handler.IsTriggered)
                        {
                            ++cantTriggerCount;
                        }
                        else
                        {
                            handler.Prepare();
                            handler.IsTriggered = true;
                            ++triggerCount;
                        }
                    }
                }
                if (cantTriggerCount == 0 || triggerCount > 0)
                {
                    m_MessageQueue.Dequeue();
                }
            }

            foreach(SkillMessageHandler handler in m_MessageHandlers)
            {
                if(handler.IsTriggered)
                {
                    handler.Tick(sender, this, delta);
                    if(handler.IsOver())
                    {
                        handler.Reset();
                    }
                }
            }
        }

        public bool IsMessageDone()
        {
            foreach (SkillMessageHandler handler in m_MessageHandlers)
            {
                if (handler.IsTriggered)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSectionDone(int sectionNum)
        {
            if(sectionNum >= 0 && sectionNum < m_Sections.Count)
            {
                SkillSection section = m_Sections[sectionNum];
                if(section.IsFinished)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool IsAllSectionDone()
        {
            if (IsSectionDone(m_CurSection) && m_CurSection == m_Sections.Count - 1)
            {
                return true;
            }
            return false;
        }

        public void Start(object sender)
        {
            m_CurTime = 0;
            m_CurSection = -1;
            ChangeToSection(0);
        }

        public void OnInterrupt(object sender, long delta)
        {
            if(m_InterruptSection != null)
            {
                m_InterruptSection.Prepare();
                m_InterruptSection.Tick(sender, this, delta);
            }
            ResetCurSection();
            StopMessageHandlers();
            m_IsFinished = true;
        }

        public void OnSkillStop(object sender, long delta)
        {
            if(m_StopSection != null)
            {
                m_StopSection.Prepare();
                m_StopSection.Tick(sender, this, delta);
            }
            ResetCurSection();
            StopMessageHandlers();
            m_IsFinished = true;
        }

        public void ChangeToSection(int index)
        {
            if(index >= 0 && index < m_Sections.Count)
            {
                SkillSection section = m_Sections[index];
                m_CurSection = index;
                m_CurSectionDuration = section.Duration * 1000;
                m_CurSectionTime = 0;

                section.Prepare();
            }
        }

        private void ResetCurSection()
        {
            if(m_CurSection >= 0 && m_CurSection < m_Sections.Count)
            {
                SkillSection section = m_Sections[m_CurSection];
                section.Reset();
            }
        }

        private void StopMessageHandlers()
        {
            foreach(SkillMessageHandler handler in m_MessageHandlers)
            {
                if(handler.IsTriggered)
                {
                    handler.Reset();
                }
            }
        }

        public void SendMessage(string msgId)
        {
            m_MessageQueue.Enqueue(msgId);
        }

        public SkillInstance Clone()
        {
            SkillInstance instance = new SkillInstance();
            foreach (SkillSection section in m_Sections)
            {
                instance.m_Sections.Add(section.Clone());
            }
            instance.m_IsInterrupted = false;
            instance.m_IsFinished = false;
            instance.m_IsCurveMoveEnable = true;
            instance.m_IsRotateEnable = true;
            instance.m_IsDamageEnable = true;
            instance.m_CurSection = 0;
            instance.m_CurSectionDuration = 0;
            instance.m_CurSectionTime = 0;
            instance.m_CurTime = 0;
            instance.m_GoToSectionId = -1;

            instance.m_SkillId = m_SkillId;
            if (m_StopSection != null)
            {
                instance.m_StopSection = m_StopSection.Clone();
            }
            if (m_InterruptSection != null)
            {
                instance.m_InterruptSection = m_InterruptSection.Clone();
            }
            foreach (SkillMessageHandler section in m_MessageHandlers)
            {
                instance.m_MessageHandlers.Add(section.Clone());
            }
            return instance;
        }

        public void Reset()
        {
            m_IsInterrupted = false;
            m_IsFinished = false;
            m_IsCurveMoveEnable = true;
            m_IsRotateEnable = true;
            m_IsDamageEnable = true;
            m_IsStopCurSection = false;
            m_TimeScale = 1;
            m_CurSection = -1;
            m_GoToSectionId = -1;

            int ct = m_Sections.Count;
            for (int i = ct - 1; i >= 0; --i)
            {
                SkillSection section = m_Sections[i];
                section.Reset();
            }
            m_CustomDatas.Clear();
            m_MessageQueue.Clear();
        }

        public bool IsControlMove
        {
            get { return m_IsControlMove; }
            set { m_IsControlMove = value; }
        }

        public bool IsFinished
        {
            get { return m_IsFinished; }
            set { m_IsFinished = value; }
        }

        public int SkillId
        {
            get { return m_SkillId; }
        }

        public float TimeScale
        {
            get { return m_TimeScale; }
            set { m_TimeScale = value; }
        }

        public TypedDataCollection CustomDatas
        {
            get
            {
                return m_CustomDatas;
            }
        }

        public bool IsCurveMoveEnable
        {
            get { return m_IsCurveMoveEnable; }
            set { m_IsCurveMoveEnable = value; }
        }

        public bool IsRotateEnable
        {
            get { return m_IsRotateEnable; }
            set { m_IsRotateEnable = value; }
        }

        public long OrigDelta
        {
            get { return m_OrigDelta; }
        }

        public float MoveScale
        {
            get { return m_MoveScale; }
            set { m_MoveScale = value; }
        }

        public long CurTime
        {
            get { return m_CurTime / 1000; }
        }

        public float EffectScale
        {
            get { return m_EffectScale; }
            set { m_EffectScale = value; }
        }

        public bool IsDamageEnable
        {
            get { return m_IsDamageEnable; }
            set { m_IsDamageEnable = value; }
        }
    }
}
