using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public static class LogicSystem
    {
        public static bool IsLastHitUi
        {
            get { return GfxSystem.Instance.IsLastHitUi; }
            set { GfxSystem.Instance.IsLastHitUi = value; }
        }

        //开始加载
        public static void BeginLoading()
        {
            GfxSystem.Instance.BeginLoading();
        }

        public static void EndLoading()
        {
            GfxSystem.Instance.EndLoading();
        }

        public static void SetLoadingBarScene(string name)
        {
            GfxSystem.Instance.SetLoadingBarScene(name);
        }

        public static SharedGameObjectInfo GetSharedGameObjectInfo(GameObject obj)
        {
            return GfxSystem.Instance.GetSharedGameObjectInfo(obj);
        }

        public static SharedGameObjectInfo GetSharedGameObjectInfo(int id)
        {
            return GfxSystem.Instance.GetSharedGameObjectInfo(id);
        }

        public static bool ExistGameObject(GameObject obj)
        {
            return GfxSystem.Instance.ExistGameObject(obj);
        }

        //发布事件
        public static void PublishLogicEvent(string evt, string group, params object[] args)
        {
            GfxSystem.Instance.PublishLogicEvent(evt, group, args);
        }

        public static void LogicLog(string format, params object[] args)
        {
            GfxSystem.Instance.CallLogicLog(format, args);
        }

        public static void LogicErrorLog(string format, params object[] args)
        {
            GfxSystem.Instance.CallLogicErrorLog(format, args);
        }

        public static void UpdateLoadingTip(string tip)
        {
            GfxSystem.Instance.UpdateLoadingTip(tip);
        }

        public static void UpdateLoadingProgress(float progress)
        {
            GfxSystem.Instance.UpdateLoadingProgress(progress);
        }

        public static void UpdateVersinoInfo(string info)
        {
            GfxSystem.Instance.UpdateVersionInfo(info);
        }

        public static float GetLoadingProgress()
        {
            return GfxSystem.Instance.GetLoadingProgress();
        }

        public static string GetLoadingTip()
        {
            return GfxSystem.Instance.GetLoadingTip();
        }

        public static Transform FindChildRecursive(Transform parent, string bonePath)
        {
            return GfxSystem.Instance.FindChildRecursive(parent, bonePath);
        }

        public static PublishSubscribeSystem EventChannelForGfx
        {
            get
            {
                return GfxSystem.Instance.EventChannelForGfx;
            }
        }

        public static GameObject GetGameObject(int id)
        {
            return GfxSystem.Instance.GetGameObject(id);
        }

        public static GameObject PlayerSelf
        {
            get { return GfxSystem.Instance.PlayerSelf; }
        }

        public static SharedGameObjectInfo PlayerSelfInfo
        {
            get { return GfxSystem.Instance.PlayerSelfInfo; }
        }

        public static void SetJoystickInfo(GestureArgs args)
        {
            GfxSystem.Instance.SetJoystickInfoImpl(args);
        }


        public static void NotifyGfxHitTarget(GameObject src, int impactId, GameObject target, int hitCount, int skillId, int duration, Vector3 srcPos, float srcDir)
        {
            SharedGameObjectInfo srcInfo = GfxSystem.Instance.GetSharedGameObjectInfo(src);
            SharedGameObjectInfo targetInfo = GfxSystem.Instance.GetSharedGameObjectInfo(target);
            if (null != srcInfo && null != targetInfo)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxHitTarget, srcInfo.m_LogicObjectId, impactId, targetInfo.m_LogicObjectId, hitCount, skillId, duration, srcPos.x, srcPos.y, srcPos.z, srcDir);
                }
            }
            else
            {
                GfxSystem.Instance.CallGfxLog("NotifyGfxHitTarget:{0} {1} {2} {3}, can't find object !", src.name, impactId, target.name, hitCount);
            }
        }

        public static void NotifyGfxStartAttack(GameObject obj, float x, float y, float z)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    //TODO 未实现
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStartAttack, info.m_LogicObjectId, x, y, z);
                }
            }
        }

        public static void NotifyGfxStopAttack(GameObject obj)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    //TODO 未实现
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStopAttack, info.m_LogicObjectId);
                }
            }
        }

        public static void NotifyGfxStopImpact(GameObject src, int impactId, GameObject target)
        {
            if (null != src && null != target)
            {
                SharedGameObjectInfo srcInfo = GfxSystem.Instance.GetSharedGameObjectInfo(src);
                SharedGameObjectInfo targetInfo = GfxSystem.Instance.GetSharedGameObjectInfo(target);
                if (null != srcInfo && null != targetInfo)
                {
                    if (null != GfxSystem.Instance.GameLogicNotification)
                    {
                        QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStopImpact, targetInfo.m_LogicObjectId, impactId);
                    }
                }
            }
        }

        public static void NotifyGfxSetCrossFadeTime(GameObject obj, string fadeTargetAnim, float fadeTime)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxSetCrossFadeTime, info.m_LogicObjectId, fadeTargetAnim, fadeTime);
                }
            }
        }

        #region skill

        public static void NotifyGfxForceStartSkill(GameObject obj, int skillId)
        {
            if (null != obj)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if(null != info)
                {
                    if(null != GfxSystem.Instance.GameLogicNotification)
                    {
                        QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxForceStartSkill, info.m_LogicObjectId, skillId);
                    }
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxForceStartSkill:{0} {1}, can't find object !", obj.name, skillId);
                }
            }
        }

        public static void NotifyGfxStartSkill(GameObject obj, SkillCategory category, Vector3 targetpos)
        {
            if (null != obj)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if (null != info)
                {
                    if (null != GfxSystem.Instance.GameLogicNotification)
                    {
                        //TODO 未实现
                        QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStartSkill, info.m_LogicObjectId, category, targetpos.x, targetpos.y, targetpos.z);
                        //GfxLog("NotifyGfxStartSkill:{0} {1}", obj.name, category);
                    }
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxStartSkill:{0} {1}, can't find object !", obj.name, category);
                }
            }
        }

        public static void NotifyGfxStopSkill(GameObject obj, int skillId)
        {
            if(null != obj)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if(null != info)
                {
                    if(null != GfxSystem.Instance.GameLogicNotification)
                    {
                        //TODO 未实现
                        QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStopSkill, info.m_LogicObjectId, skillId);
                    }
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxStopSkill:{0} {1}, can't find object !", obj.name, skillId);
                }
            }
        }

        public static void NotifyGfxAnimationStart(GameObject obj, bool isSkill)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if(null != info)
            {
                if(isSkill)
                {
                    info.IsSkillGfxAnimation = true;
                }
                else
                {
                    info.IsImpactGfxAnimation = true;
                }
            }
            else
            {
                GfxSystem.Instance.CallGfxLog("NotifyGfxAnimationStart:{0}, can't find object !", obj.name);
            }
        }

        public static void NotifyGfxAnimationFinish(GameObject obj, bool isSkill)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (isSkill)
                {
                    info.IsSkillGfxAnimation = false;
                }
                else
                {
                    info.IsImpactGfxAnimation = false;
                }
            }
            else
            {
                GfxSystem.Instance.CallGfxLog("NotifyGfxAnimationFinish:{0}, can't find object !", obj.name);
            }
        }

        public static void NotifyGfxSkillBreakSection(GameObject obj, int skillid, int breaktype, int starttime, int endtime, bool isinterrupt)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if(null != info)
            {
                if(null != GfxSystem.Instance.GameLogicNotification)
                {
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxSkillBreakSection, info.m_LogicObjectId, skillid, breaktype, starttime, endtime, isinterrupt);
                }
            }
        }

        //召唤
        public static void NotifyGfxSummonNpc(GameObject obj, int owner_skillid, int npc_type_id, string model, int skillid, float pos_x, float pos_y, float pos_z)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    //TODO 未实现
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxSummonNpc, info.m_LogicObjectId, owner_skillid, npc_type_id, model, skillid,
                                                                                                     pos_x, pos_y, pos_z);
                }
            }
        }

        public static void NotifyGfxDestroyObj(GameObject obj)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    //TODO 未实现
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxDestroyObj, info.m_LogicObjectId);
                }
            }
        }

        #endregion

        public static void StartStory(int storyId)
        {
            if(null != GfxSystem.Instance.GameLogicNotification)
            {
                QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxStartStory, storyId);
            }
        }

        public static void SendStoryMessage(string msgId, params object[] args)
        {
            if (null != GfxSystem.Instance.GameLogicNotification)
            {
                QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxSendStoryMessage, msgId, args);
            }
        }

        public static void NotifyGfxMoveControlStart(GameObject obj, int id, bool isSkill)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if(null != info)
            {
                if(isSkill)
                {
                    info.IsSkillGfxMoveControl = true;//为true时表示不可移动 属于技能控制
                }
                else
                {
                    info.IsImpactGfxMoveControl = true;//SharedGameObjectInfo中的IsGfxMoveControl使用
                    info.IsImpactGfxRotateControl = true;//SharedGameObjectInfo中的IsGfxRotateControl使用
                }
                if(null != GfxSystem.Instance.GameLogicNotification)
                {
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxControlMoveStart, info.m_LogicObjectId, id, isSkill);
                }
            }
            else
            {
                GfxSystem.Instance.CallGfxLog(string.Format("NotifyGfxMoveControlStart:{0}, can't find object !", obj.name));
            }
        }

        public static void NotifyGfxUpdatePosition(GameObject obj, float x, float y, float z)
        {
            lock (GfxSystem.SyncLock)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if (null != info)
                {
                    info.X = x;
                    info.Y = y;
                    info.Z = z;
                    info.DataChangedByGfx = true;
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxUpdatePosition:{0} {1} {2} {3}, can't find object !", obj.name, x, y, z);
                }
            }
        }

        public static void NotifyGfxUpdatePosition(GameObject obj, float x, float y, float z, float rx, float ry, float rz)
        {
            lock(GfxSystem.SyncLock)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if (null != info)
                {
                    info.X = x;
                    info.Y = y;
                    info.Z = z;
                    info.FaceDir = ry;
                    info.DataChangedByGfx = true;
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxUpdatePosition:{0} {1} {2} {3} {4} {5} {6}, can't find object !", obj.name, x, y, z, rx, ry, rz);
                }
            }
        }

        public static void NotifyGfxChangedWantDir(GameObject obj, float ry)
        {
            lock (GfxSystem.SyncLock)
            {
                SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
                if (null != info)
                {
                    info.WantFaceDir = ry;
                    info.WantDirChangedByGfx = true;
                }
                else
                {
                    GfxSystem.Instance.CallGfxLog("NotifyGfxUpdatePosition:{0} {1}, can't find object !", obj.name, ry);
                }
            }
        }

        public static void NotifyGfxMoveControlFinish(GameObject obj, int id, bool isSkill)
        {
            SharedGameObjectInfo info = GfxSystem.Instance.GetSharedGameObjectInfo(obj);
            if (null != info)
            {
                if (isSkill)
                {
                    info.IsSkillGfxMoveControl = false;
                    info.IsSkillGfxRotateControl = false;
                }
                else
                {
                    info.IsImpactGfxMoveControl = false;
                    info.IsImpactGfxRotateControl = false;
                }

                if (null != GfxSystem.Instance.GameLogicNotification)
                {
                    QueueLogicAction(GfxSystem.Instance.GameLogicNotification.OnGfxControlMoveStop, info.m_LogicObjectId, id, isSkill);
                }

                //GfxLog("NotifyGfxMoveControlFinish:{0}", info.m_LogicObjectId);
            }
            else
            {
                GfxSystem.Instance.CallGfxLog("NotifyGfxMoveControlFinish:{0}, can't find object !", obj.name);
            }
        }

        public static float RadianToDegree(float dir)
        {
            return GfxSystem.Instance.RadianToDegree(dir);
        }

        public static void QueueLogicAction<T1>(MyAction<T1> action, T1 t1)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1);
        }

        public static void QueueLogicAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2);
        }

        public static void QueueLogicAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3);
        }

        public static void QueueLogicAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3, t4);
        }

        public static void QueueLogicAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3, t4, t5);
        }

        public static void QueueLogicAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3, t4, t5, t6);
        }

        public static void QueueLogicAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static void QueueLogicAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            GfxSystem.Instance.QueueLogicActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }
    }
}
