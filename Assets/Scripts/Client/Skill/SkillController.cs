using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class SkillController : ISkillController
    {
        public static List<int> m_UnlockedSkills = new List<int>();
        protected Dictionary<SkillCategory, SkillNode> m_SkillCategoryDict = new Dictionary<SkillCategory, SkillNode>();
        protected bool m_IsAttacking = false;
        protected List<SkillNode> m_WaiteSkillBuffer = new List<SkillNode>();
        protected SkillNode m_LastSkillNode = null;
        protected SkillNode m_CurSkillNode = null;
        protected SkillQECanInputHandler m_SkillQECanInputHandler = null;
        protected SkillStartHandler m_SkillStartHandler = null;
        protected bool m_IsHaveBreakSkillTask = false;

        public virtual void Init()
        {
        }

        public virtual void RegisterSkillStartHandler(SkillStartHandler handler)
        {
            m_SkillStartHandler = handler;
        }

        public virtual void RegisterSkillQECanInputHandler(SkillQECanInputHandler handler)
        {
            m_SkillQECanInputHandler = handler;
        }

        //递归调用初始化所有技能和子技能
        public SkillNode InitCategorySkillNode(List<SkillLogicData> skills, SkillLogicData ss)
        {
            SkillNode first = new SkillNode();
            first.SkillId = ss.SkillId;
            first.Category = ss.Category;
            SkillLogicData nextSkillScript = GetSkillById(skills, ss.NextSkillId);
            if(nextSkillScript != null)
            {
                first.NextSkillNode = InitCategorySkillNode(skills, nextSkillScript);
            }

            SkillLogicData qSkillScript = GetSkillById(skills, ss.QSkillId);
            if (qSkillScript != null)
            {
                first.SkillQ = InitCategorySkillNode(skills, qSkillScript);
            }

            SkillLogicData eSkillScript = GetSkillById(skills, ss.ESkillId);
            if (eSkillScript != null)
            {
                first.SkillE = InitCategorySkillNode(skills, eSkillScript);
            }
            return first;
        }

        public SkillLogicData GetSkillById(List<SkillLogicData> skills, int id)
        {
            foreach(SkillLogicData ss in skills)
            {
                if(ss.SkillId == id)
                {
                    return ss;
                }
            }
            return null;
        }

        protected SkillNode GetSkillNodeById(int skillId)
        {
            SkillNode result = null;
            foreach(SkillNode head in m_SkillCategoryDict.Values)
            {
                result = FindSkillNodeInChildren(head, skillId);
                if(null != result)
                {
                    return result;
                }
            }
            return result;
        }

        //递归查找是否有此技能
        protected SkillNode FindSkillNodeInChildren(SkillNode head, int targetId)
        {
            if (head.SkillId == targetId)
            {
                return head;
            }
            if (null != head.NextSkillNode)
            {
                SkillNode node = FindSkillNodeInChildren(head.NextSkillNode, targetId);
                if (null != node)
                {
                    return node;
                }
            }
            if (null != head.SkillQ)
            {
                SkillNode node = FindSkillNodeInChildren(head.SkillQ, targetId);
                if (null != node)
                {
                    return node;
                }
            }
            if (head.SkillE != null)
            {
                SkillNode node = FindSkillNodeInChildren(head.SkillE, targetId);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        public virtual void OnTick()
        {
            DealBreakSkillTask();
            UpdateAttacking();
            UpdateSkillNodeCD();
            if(m_WaiteSkillBuffer.Count <= 0)
            {
                return;
            }

            //等待技能释放
            SkillNode node = m_WaiteSkillBuffer[m_WaiteSkillBuffer.Count-1];
            if(node == null)
            {
                m_WaiteSkillBuffer.Remove(node);
                return;
            }
            SkillNode nextNode = null;
            if((node.Category == SkillCategory.kSkillQ || node.Category == SkillCategory.kSkillE) && m_WaiteSkillBuffer.Count >= 2)
            {
                SkillNode lastOne = m_WaiteSkillBuffer[m_WaiteSkillBuffer.Count - 2];
                if(node.Category == SkillCategory.kSkillQ && lastOne.SkillQ != null && lastOne.SkillQ.SkillId == node.SkillId)
                {
                    nextNode = node;
                    node = lastOne;
                }
                else if(node.Category == SkillCategory.kSkillE && lastOne.SkillE != null && lastOne.SkillE.SkillId == node.SkillId)
                {
                    nextNode = node;
                    node = lastOne;
                }
            }

            //IsSkillCanBreak 能找到m_CurSkillNode这个节点并且下一个node在BreakSections列表中
            if(m_CurSkillNode == null || IsSkillCanBreak(m_CurSkillNode, node))
            {
                SkillCannotCastType cannotType = SkillCannotCastType.kUnknow;
                if(!IsSkillCanStart(node, out cannotType))
                {
                    LogSystem.Debug("skill can't start");
                    if(IsPlayerSelf())
                    {
                        GfxSystem.PublishGfxEvent("ge_skill_cannot_cast", "ui", cannotType);
                    }
                    m_WaiteSkillBuffer.Clear();
                    return;
                }

                if(m_CurSkillNode != null)
                {
                    StopSkill(m_CurSkillNode, node);
                }

                //if(m_CurSkillNode != null)
                //{
                //    Debug.LogError("skillController_StartSkill_" + m_CurSkillNode.SkillId + "_" + node.SkillId);
                //}

                if(StartSkill(node))
                {
                    //Debug.LogError("skillController_StartSkill_" + node.SkillId);
                    OnSkillStart(node);
                    if (nextNode != null)
                    {
                        m_WaiteSkillBuffer.Add(nextNode);
                    }
                    PostSkillStart(node);
                }
            }
        }

        private void OnSkillStart(SkillNode node)
        {
            HideSkillTip(SkillCategory.kNone);
            m_LastSkillNode = m_CurSkillNode;
            m_CurSkillNode = node;
            m_CurSkillNode.StartTime = TimeUtility.GetServerMilliseconds() / 1000.0f;
            m_CurSkillNode.IsCDChecked = false;

            m_WaiteSkillBuffer.RemoveAt(m_WaiteSkillBuffer.Count - 1);

            List<SkillNode> newBufferElement = new List<SkillNode>();
            newBufferElement.AddRange(m_WaiteSkillBuffer);
            m_WaiteSkillBuffer.Clear();

            if (m_CurSkillNode.NextSkillNode != null)
            {
                if (newBufferElement.Count >= 1)
                {
                    SkillNode last = newBufferElement[newBufferElement.Count - 1];
                    if(m_CurSkillNode != null && last != null && last.Category == m_CurSkillNode.Category)
                    {
                        PushSkill(last.Category, Vector3.zero);
                        newBufferElement.Clear();
                    }
                }
            }

            if(m_LastSkillNode != null && m_LastSkillNode.Category != SkillCategory.kAttack && m_LastSkillNode.Category != m_CurSkillNode.Category)
            {
                if(!m_LastSkillNode.IsCDChecked)
                {
                    BeginSkillCategoryCD(m_LastSkillNode.Category);
                    m_LastSkillNode.IsCDChecked = true;
                }
            }

            if (null != m_SkillStartHandler)
            {
                m_SkillStartHandler();
            }
        }

        public virtual void HideSkillTip(SkillCategory category)
        {
        }

        public virtual void PushSkill(SkillCategory category, Vector3 targetpos)
        {
            CancelBreakSkillTask();

            if(CanInput(category))
            {
                if(IsCategorySkillInCD(category))
                {
                    HideSkillTip(category);
                    LogSystem.Warn("skill is in cd");
                    return;
                }

                SkillNode targetNode = AddCategorySkillNode(category);
                if(null != targetNode)
                {
                    targetNode.TargetPos = targetpos;
                    if(SkillCategory.kSkillQ != category && SkillCategory.kSkillE != category)
                    {
                        List<SkillNode> qeSkills = new List<SkillNode>();
                        if(null != targetNode.SkillQ)
                        {
                            qeSkills.Add(targetNode.SkillQ);
                        }
                        if(null != targetNode.SkillE)
                        {
                            qeSkills.Add(targetNode.SkillE);
                        }
                        if(null != m_SkillQECanInputHandler)
                        {
                            m_SkillQECanInputHandler(GetWaitInputTime(m_CurSkillNode), qeSkills);
                        }
                    }
                }
                else
                {
                    HideSkillTip(category);
                    LogSystem.Debug("----not find skill " + category);
                }
            }
            else
            {
                LogSystem.Debug("skill can't input");
            }
        }

        public virtual bool IsSkillCanStart(SkillNode node, out SkillCannotCastType reason)
        {
            reason = SkillCannotCastType.kUnknow;
            return true;
        }

        public virtual bool IsCategorySkillInCD(SkillCategory category)
        {
            return IsSkillInCD(GetHead(category));
        }

        public virtual bool IsSkillInCD(SkillNode node)
        {
            return false;
        }

        //Deal 分配
        protected virtual void DealBreakSkillTask()
        {
            if(m_IsHaveBreakSkillTask)
            {
                if(m_CurSkillNode == null || IsSkillCanBreak(m_CurSkillNode))
                {
                    if(m_IsAttacking)
                    {
                        return;
                    }
                    if (m_CurSkillNode != null)
                    {
                        StopSkill(m_CurSkillNode, null);
                    }
                    m_IsHaveBreakSkillTask = false;
                    if(m_WaiteSkillBuffer.Count > 0)
                    {
                        HideSkillTip(SkillCategory.kNone);
                    }
                }
            }
        }

        private void UpdateSkillNodeCD()
        {
            if(m_CurSkillNode != null && !m_CurSkillNode.IsCDChecked)
            {
                if(m_CurSkillNode.NextSkillNode == null)
                {
                    BeginSkillCategoryCD(m_CurSkillNode.Category);
                    m_CurSkillNode.IsCDChecked = true;
                }
                else if (GetWaitInputTime(m_CurSkillNode) < TimeUtility.GetServerMilliseconds() / 1000.0f)
                {
                    BeginSkillCategoryCD(m_CurSkillNode.Category);
                    m_CurSkillNode.IsCDChecked = true;
                }
            }
        }

        protected virtual void BeginSkillCategoryCD(SkillCategory category)
        {
            SkillNode head = null;
            if(m_SkillCategoryDict.TryGetValue(category, out head))
            {
                GfxSystem.PublishGfxEvent("ge_cast_skill_cd", "ui", GetCategoryName(head.Category), GetSkillCD(head));
            }
        }

        protected string GetCategoryName(SkillCategory category)
        {
            switch (category)
            {
                case SkillCategory.kSkillA:
                    return "SkillA";
                case SkillCategory.kSkillB:
                    return "SkillB";
                case SkillCategory.kSkillC:
                    return "SkillC";
                case SkillCategory.kSkillD:
                    return "SkillD";
                default:
                    return "";
            }
        }

        public virtual float GetSkillCD(SkillNode node)
        {
            return 3;
        }

        public virtual bool IsSkillActive(SkillNode node)
        {
            if (node == null)
            {
                return false;
            }
            return true;
        }

        private void UpdateAttacking()
        {
            if(m_IsAttacking)
            {
                SkillNode nextNode = null;
                if(m_CurSkillNode != null)
                {
                    nextNode = m_CurSkillNode.NextSkillNode;
                }
                if(nextNode == null)
                {
                    nextNode = GetHead(SkillCategory.kAttack);
                }
                if(m_WaiteSkillBuffer.Count <= 0 && CanInput(SkillCategory.kAttack) && IsSkillCanBreak(m_CurSkillNode, nextNode))
                {
                    SkillNode nextAttackNode = AddAttackNode();
                    if(nextAttackNode != null)
                    {
                        nextAttackNode.TargetPos = Vector3.zero;
                    }
                }
            }
        }

        protected SkillNode AddAttackNode()
        {
            SkillNode node = null;
            if(CanInput(SkillCategory.kAttack))
            {
                node = AddCategorySkillNode(SkillCategory.kAttack);
            }
            return node;
        }

        protected SkillNode AddCategorySkillNode(SkillCategory category)
        {
            switch(category)
            {
                case SkillCategory.kSkillQ:
                case SkillCategory.kSkillE:
                    return AddQESkillNode(category);
                default:
                    return AddNextBasicSkill(category);

            }
        }

        private SkillNode AddQESkillNode(SkillCategory category)
        {
            float now = TimeUtility.GetServerMilliseconds() / 1000.0f;
            if(m_CurSkillNode == null)
            {
                return null;
            }
            SkillNode parent = m_CurSkillNode;
            bool isHaveWaitNode = false;
            if(m_WaiteSkillBuffer.Count > 0)
            {
                parent = m_WaiteSkillBuffer[m_WaiteSkillBuffer.Count - 1];
                isHaveWaitNode = true;
            }
            if(parent == null)
            {
                return null;
            }
            if(isHaveWaitNode || now < GetWaitInputTime(m_CurSkillNode))
            {
                SkillNode target = null;
                if(category == SkillCategory.kSkillQ)
                {
                    target = parent.SkillQ;
                }
                if (category == SkillCategory.kSkillE)
                {
                    target = parent.SkillE;
                }
                if (target != null && !target.IsLocked)
                {
                    m_WaiteSkillBuffer.Add(target);
                    return target;
                }
            }
            return null;
        }

        private SkillNode AddNextBasicSkill(SkillCategory category)
        {
            float now = TimeUtility.GetServerMilliseconds() / 1000.0f;
            if(m_CurSkillNode !=  null && m_CurSkillNode.Category == category)
            {
                if(m_CurSkillNode.NextSkillNode != null && !m_CurSkillNode.NextSkillNode.IsLocked && now < GetWaitInputTime(m_CurSkillNode))
                {
                    m_WaiteSkillBuffer.Add(m_CurSkillNode.NextSkillNode);
                    return m_CurSkillNode.NextSkillNode;
                }
            }
            SkillNode firstNode = null;
            if(m_SkillCategoryDict.TryGetValue(category, out firstNode))
            {
                if(!firstNode.IsLocked)
                {
                    m_WaiteSkillBuffer.Add(firstNode);
                    return firstNode;
                }
            }
            return null;

        }

        public SkillNode GetHead(SkillCategory category)
        {
            SkillNode target = null;
            m_SkillCategoryDict.TryGetValue(category, out target);
            return target;
        }

        protected bool CanInput(SkillCategory nextCategory)
        {
            float now = TimeUtility.GetServerMilliseconds() / 1000.0f;
            if(now < GetLockInputTime(m_CurSkillNode, nextCategory))
            {
                return false;
            }
            return true;
        }

        public virtual void AddBreakSection(int skillid, int breaktpye, int starttime, int endtime, bool isinterrupt)
        {
        }

        public virtual void ForceInterruptCurSkill()
        {
        }

        public virtual bool ForceStartSkill(int skillid)
        {
            return true;
        }

        public virtual void PostSkillStart(SkillNode node)
        {
        }

        public virtual void AddBreakSkillTask()
        {
            m_IsHaveBreakSkillTask = true;
        }

        public void CancelBreakSkillTask()
        {
            m_IsHaveBreakSkillTask = false;
        }

        public virtual bool StartSkill(SkillNode node)
        {
            return true;
        }

        public virtual bool StopSkill(SkillNode node, SkillNode nextnode)
        {
            return true;
        }

        //开始攻击
        public virtual void StartAttack(Vector3 targetpos)
        {
            CancelBreakSkillTask();
            SkillNode node = AddAttackNode();
            if(node != null)
            {
                node.TargetPos = targetpos;
            }
            m_IsAttacking = true;
        }

        //停止攻击
        public virtual void StopAttack()
        {
            Debug.LogWarning("StopAttack");
            m_IsAttacking = false;
        }

        public virtual bool IsSkillCanBreak(SkillNode node, SkillNode nextNode = null)
        {
            if(m_CurSkillNode == null)
            {
                return true;
            }
            return false;
        }

        public virtual bool IsSkillCanBreak(SkillNode node, int breaktype, out bool isinterrupt)
        {
            isinterrupt = false;
            if (node == null)
            {
                return true;
            }
            return false;
        }

        public virtual float GetLockInputTime(SkillNode node, SkillCategory category)
        {
            return 0;
        }

        public virtual float GetWaitInputTime(SkillNode node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.StartTime + 5;
        }

        public virtual bool IsPlayerSelf()
        {
            return false;
        }
    }
}
