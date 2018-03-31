using SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Skill.Trigers
{
    public class GrabTargetTrigger : AbstractSkillTriger
    {
        private bool m_IsGrab = false;
        private string m_SourceNode;
        private string m_TargetNode;
        private long m_UpdateTime = 0;
        private bool m_IsNotified = false;

        private bool m_IsInited = false;

        private bool Init(GameObject obj)
        {
            m_IsInited = true;
            return !IsGameObjectSupperArmer(obj);
        }

        protected override void Load(ScriptableData.CallData callData)
        {
            if (callData.GetParamNum() >= 2)
            {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_IsGrab = bool.Parse(callData.GetParamId(1));
            }
            if (callData.GetParamNum() >= 5)
            {
                m_SourceNode = callData.GetParamId(2);
                m_TargetNode = callData.GetParamId(3);
                m_UpdateTime = long.Parse(callData.GetParamId(4));
            }
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            if (curSectionTime < m_StartTime)
            {
                return true;
            }
            GameObject obj = sender as GameObject;
            if (obj == null)
            {
                return false;
            }
            MoveTargetInfo target_info = instance.CustomDatas.GetData<MoveTargetInfo>();
            if (target_info == null || target_info.Target == null)
            {
                return false;
            }

            if (m_IsGrab)
            {
                if (!m_IsInited)
                {
                    if (!Init(target_info.Target))
                    {
                        return false;
                    }
                }
                if (TriggerUtil.AttachNodeToNode(obj, m_SourceNode, target_info.Target, m_TargetNode))
                {
                    if (!m_IsNotified)
                    {
                        DashFire.LogicSystem.NotifyGfxMoveControlStart(target_info.Target, instance.SkillId, true);
                        m_IsNotified = true;
                    }
                    TriggerUtil.UpdateObjPosition(target_info.Target);
                }
                if (curSectionTime < m_StartTime + m_UpdateTime)
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
                target_info.Target.transform.parent = null;
                DashFire.LogicSystem.NotifyGfxMoveControlFinish(target_info.Target, instance.SkillId, true);
                return false;
            }
        }

        public static bool IsGameObjectSupperArmer(GameObject target)
        {
            DashFire.SharedGameObjectInfo info = DashFire.LogicSystem.GetSharedGameObjectInfo(target);
            if (info == null)
            {
                return false;
            }
            return info.IsSuperArmor;
        }

        public override void Reset()
        {
            m_IsNotified = false;
            m_IsInited = false;
        }

        public override ISkillTriger Clone()
        {
            GrabTargetTrigger copy = new GrabTargetTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_IsGrab = m_IsGrab;
            copy.m_SourceNode = m_SourceNode;
            copy.m_TargetNode = m_TargetNode;
            copy.m_UpdateTime = m_UpdateTime;
            return copy;
        }
    }
}
