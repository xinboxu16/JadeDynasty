using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public interface IGfxImpactLogic
    {
        void StartImpact(ImpactLogicInfo logicInfo);
        void Tick(ImpactLogicInfo logicInfo);
        void StopImpact(ImpactLogicInfo logicInfo);
        void OnInterrupted(ImpactLogicInfo logicInfo);
        bool OnOtherImpact(int logicId, ImpactLogicInfo logicInfo, bool isSameImpact);
    }

    public abstract class AbstarctGfxImpactLogic : IGfxImpactLogic
    {
        private const float m_MaxMoveStep = 2.0f;

        private enum ImpactMovementType
        {
            SenderDir = 0,
            SenderToTarget = 1,
        }

        public virtual void StartImpact(ImpactLogicInfo logicInfo)
        {
        }

        //技能效果
        protected void GeneralStartImpact(ImpactLogicInfo logicInfo)
        {
            LogicSystem.NotifyGfxAnimationStart(logicInfo.Target, false);
            LogicSystem.NotifyGfxMoveControlStart(logicInfo.Target, logicInfo.ImpactId, false);
            InitMovement(logicInfo);
        }

        public virtual void Tick(ImpactLogicInfo logicInfo)
        {
        }

        public virtual void StopImpact(ImpactLogicInfo logicInfo)
        {
        }

        protected void GeneralStopImpact(ImpactLogicInfo logicInfo)
        {
            if(IsLogicDead(logicInfo.Target))
            {
                SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.Stiffness);
                SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.HitFly);
                SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.GetUp);
            }
            else
            {
                ClearGfxStateFlag(logicInfo.Target);
            }
            logicInfo.CustomDatas.Clear();
            LogicSystem.NotifyGfxAnimationFinish(logicInfo.Target, false);
            LogicSystem.NotifyGfxMoveControlFinish(logicInfo.Target, logicInfo.ImpactId, false);
            LogicSystem.NotifyGfxStopImpact(logicInfo.Sender, logicInfo.ImpactId, logicInfo.Target);
        }

        public virtual bool OnOtherImpact(int logicId, ImpactLogicInfo logicInfo, bool isSameImpact)
        {
            return true;
        }

        public virtual void OnInterrupted(ImpactLogicInfo logicInfo)
        {
            if (IsLogicDead(logicInfo.Target))
            {
                SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.Stiffness);
                SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.HitFly);
            }
            else
            {
                ClearGfxStateFlag(logicInfo.Target);
            }
            LogicSystem.NotifyGfxAnimationFinish(logicInfo.Target, false);
            LogicSystem.NotifyGfxMoveControlFinish(logicInfo.Target, logicInfo.ImpactId, false);
        }

        protected virtual void UpdateMovement(ImpactLogicInfo info, float deltaTime)
        {

        }

        protected void SetAnimationSpeed(GameObject obj, Animation_Type anim, float speed)
        {
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                string animName = GetAnimationNameByType(obj, anim);
                if (!string.IsNullOrEmpty(animName) && null != animation[animName])
                {
                    animation[animName].speed = speed;
                }
            }
            else
            {
                if (null == obj)
                {
                    Debug.LogError("null obj");
                }
                else
                {
                    Debug.LogError("null anim");
                }
            }
        }

        protected float GetAnimationLenthByType(GameObject obj, Animation_Type type)
        {
            string animName = GetAnimationNameByType(obj, type);
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                AnimationState state = animation[animName];
                if(null != state)
                {
                    return state.length;
                }
            }
            return 0.0f;
        }

        protected void CrossFadeAnimation(GameObject obj, Animation_Type anim, float time = 0.3f, float speed = 1.0f)
        {
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                string animName = GetAnimationNameByType(obj, anim);
                if (!string.IsNullOrEmpty(animName) && null != animation[animName])
                {
                    animation[animName].speed = speed;
                    animation.CrossFade(animName, time);
                }
            }
            else
            {
                if (null == obj)
                {
                    Debug.LogError("null obj");
                }
                else
                {
                    Debug.LogError("null anim");
                }
            }
        }

        protected void PlayAnimation(GameObject obj, Animation_Type anim, float speed = 1.0f, AnimationBlendMode blendMode = AnimationBlendMode.Blend)
        {
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                string animName = GetAnimationNameByType(obj, anim);
                if (!string.IsNullOrEmpty(animName) && null != animation[animName])
                {
                    animation[animName].speed = speed;
                    animation.Play(animName);
                    animation[animName].blendMode = blendMode;
                }
            }
            else
            {
                if (null == obj)
                {
                    Debug.LogError("null obj");
                }
                else
                {
                    Debug.LogError("null anim");
                }
            }
        }

        protected void StopAnimation(GameObject obj, Animation_Type anim)
        {
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                string animName = GetAnimationNameByType(obj, anim);
                if (!string.IsNullOrEmpty(animName) && null != animation[animName])
                {
                    animation.Stop(animName);
                }
            }
            else
            {
                if (null == obj)
                {
                    Debug.LogError("null obj");
                }
                else
                {
                    Debug.LogError("null anim");
                }
            }
        }

        protected float GetAnimationSpeedByTime(ImpactLogicInfo info, float time)
        {
            return info.AnimationInfo.GetSpeedByTime(time) * info.LockFrameInfo.GetSpeedByTime(time);
        }

        protected float GetLockFrameRate(ImpactLogicInfo info, float time)
        {
            return info.LockFrameInfo.GetSpeedByTime(time);
        }

        protected string GetAnimationNameByType(GameObject obj, Animation_Type type)
        {
            if (null == obj) return null;
            SharedGameObjectInfo info = LogicSystem.GetSharedGameObjectInfo(obj);
            if(null != info)
            {
                Data_ActionConfig curActionConfig = ActionConfigProvider.Instance.ActionConfigMgr.GetDataById(info.AnimConfigId);
                Data_ActionConfig.Data_ActionInfo action = null;
                if(null != curActionConfig && curActionConfig.m_ActionContainer.ContainsKey(type))
                {
                    if(curActionConfig.m_ActionContainer[type].Count > 0)
                    {
                        action = curActionConfig.m_ActionContainer[type][0];
                    }
                }
                if(null != action)
                {
                    return action.m_AnimName;
                }
            }
            return null;
        }

        protected float GetTerrainHeight(Vector3 pos)
        {
            if (Terrain.activeTerrain != null)
            {
                return Terrain.activeTerrain.SampleHeight(pos);
            }
            else
            {
                RaycastHit hit;
                pos.y += 2;
                if (Physics.Raycast(pos, -Vector3.up, out hit, 30 /*max_ray_cast_dis */, 1 << LayerMask.NameToLayer("Terrains")))
                {
                    return hit.point.y + 0.1f;
                }
                return 0;
            }
        }

        protected void SetGfxStateFlag(GameObject obj, Operate_Type opType, GfxCharacterState_Type mask)
        {
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(obj);
            if(null != shareInfo)
            {
                if(opType == Operate_Type.OT_AddBit)
                {
                    shareInfo.GfxStateFlag |= (int)mask;
                }
                else if(opType == Operate_Type.OT_RemoveBit)
                {
                    shareInfo.GfxStateFlag &= ~((int)mask);
                }
            }
        }

        protected void ClearGfxStateFlag(GameObject obj)
        {
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(obj);
            if (null != shareInfo)
            {
                shareInfo.GfxStateFlag = 0;
            }
        }

        protected void InitMovement(ImpactLogicInfo info)
        {
            ImpactLogicData config = info.ConfigData;
            if(null != config)
            {
                switch ((ImpactMovementType)config.MoveMode)
                {
                    case ImpactMovementType.SenderDir:
                        if(null != info.Sender)
                        {
                            info.MoveDir = Quaternion.Euler(0, ImpactUtility.RadianToDegree(info.ImpactSrcDir), 0);//ImpactSrcDir来自于发起者的方向
                        }
                        break;
                    case ImpactMovementType.SenderToTarget:
                        if (null != info.Target)
                        {
                            Vector3 direction = info.Target.transform.position - info.ImpactSrcPos;//ImpactSrcPos 发起者的位置
                            direction.y = 0.0f;
                            direction.Normalize();
                            info.MoveDir = Quaternion.LookRotation(direction);
                        }
                        break;
                }
            }
        }

        //obj目标对象
        protected void MoveTo(GameObject obj, Vector3 pos)
        {
            if(null != obj)
            {
                RaycastHit hit;
                if(Physics.Raycast(obj.transform.position, pos.normalized, out hit, pos.magnitude, 1 << LayerMask.NameToLayer("Terrains")))
                {
                    pos = hit.point;
                }
                Vector3 motion = pos - obj.transform.position;
                Move(obj, motion);
            }
        }

        protected void Move(GameObject obj, Vector3 motion)
        {
            if(null != obj)
            {
                SharedGameObjectInfo info = LogicSystem.GetSharedGameObjectInfo(obj);
                if(null != info && info.CanHitMove)
                {
                    //向量的长度
                    while(motion.magnitude > m_MaxMoveStep)
                    {
                        Vector3 childMotion = Vector3.ClampMagnitude(motion, m_MaxMoveStep);//ClampMagnitude 返回向量的固定长度，最大不超过maxLength所指示的长度。
                        motion = motion - childMotion;
                        ImpactUtility.MoveObject(obj, childMotion);
                    }
                    ImpactUtility.MoveObject(obj, motion);
                }
            }
        }

        protected bool IsLogicDead(GameObject obj)
        {
            if (null != obj)
            {
                SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(obj);
                {
                    if (null != shareInfo && shareInfo.Blood > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected void SetGfxDead(GameObject obj, bool isDead)
        {
            if (null != obj)
            {
                SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(obj);
                {
                    if (null != shareInfo)
                    {
                        shareInfo.IsDead = isDead;
                    }
                }
            }
        }

        public virtual void UpdateEffect(ImpactLogicInfo logicInfo)
        {
            if (null == logicInfo.Target) return;
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(logicInfo.Target);
            if (null != shareInfo && !shareInfo.AcceptStiffEffect) return;
            for(int i = 0; i < logicInfo.EffectList.Count; ++i)
            {
                EffectInfo effectInfo = logicInfo.EffectList[i];
                if(null != effectInfo)
                {
                    if (effectInfo.StartTime < 0 && Time.time > logicInfo.StartTime + effectInfo.DelayTime / 1000)
                    {
                        effectInfo.IsActive = true;
                        effectInfo.StartTime = Time.time;
                        GameObject obj = ResourceSystem.NewObject(effectInfo.Path, effectInfo.PlayTime / 1000) as GameObject;
                        if(null != obj)
                        {
                            if(effectInfo.DelWithImpact)
                            {
                                logicInfo.EffectsDelWithImpact.Add(obj);
                            }
                            if (String.IsNullOrEmpty(effectInfo.MountPoint))//MountPoint挂点
                            {
                                obj.transform.position = logicInfo.Target.transform.position + effectInfo.RelativePoint;
                                Quaternion q = Quaternion.Euler(effectInfo.RelativeRotation.x, effectInfo.RelativeRotation.y, effectInfo.RelativeRotation.z);
                                if (effectInfo.RotateWithTarget && null != logicInfo.Sender)
                                {
                                    obj.transform.rotation = Quaternion.LookRotation(logicInfo.Target.transform.position - logicInfo.Sender.transform.position, Vector3.up);
                                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles + effectInfo.RelativeRotation);
                                }
                                else
                                {
                                    obj.transform.rotation = q;
                                }
                            }
                            else
                            {
                                Transform parent = LogicSystem.FindChildRecursive(logicInfo.Target.transform, effectInfo.MountPoint);//MountPoint挂点
                                if (null != parent)
                                {
                                    obj.transform.parent = parent;
                                    obj.transform.localPosition = Vector3.zero;
                                    Quaternion q = Quaternion.Euler(ImpactUtility.RadianToDegree(effectInfo.RelativeRotation.x), ImpactUtility.RadianToDegree(effectInfo.RelativeRotation.y), ImpactUtility.RadianToDegree(effectInfo.RelativeRotation.z));
                                    obj.transform.localRotation = q;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("UpdateEffect" + effectInfo.Path);
                        }
                    }
                }
            }
        }
    }
}
