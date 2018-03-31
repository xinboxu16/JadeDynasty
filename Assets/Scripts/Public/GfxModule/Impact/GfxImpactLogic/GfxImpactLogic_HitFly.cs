using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public class GfxImpactLogic_HitFly : AbstarctGfxImpactLogic
    {
        private enum HitFlyState
        {
            Rising,
            Falling,
            OnGround,
            StandUp,
        }

        private class HitFlyParams
        {
            public HitFlyState ImpactState = HitFlyState.Rising;
            public float HitGroundTime = 0.0f;
            public float OnGroundTime = 0.0f;
            public float FallDownTime = 0.0f;
            public float FallDownSpeed = 1.0f;
            public float GetUpTime = 0.0f;
            public float GetUpSpeed = 1.0f;
        }

        private const float m_Gravity = -30.0f;
        private const float m_MinCrossfadeTime = 0.5f;

        public override void Tick(ImpactLogicInfo logicInfo)
        {
            UpdateMovement(logicInfo, Time.deltaTime);
            UpdateEffect(logicInfo);
            GameObject target = logicInfo.Target;
            HitFlyParams param = GetHitFlyParams(logicInfo);
            switch (param.ImpactState)
            {
                case HitFlyState.Rising:
                    if (logicInfo.Velocity.y <= 0)
                    {
                        param.ImpactState = HitFlyState.Falling;
                        float crossFadeTime = Mathf.Abs((target.transform.position.y - GetTerrainHeight(target.transform.position)) / m_Gravity);
                        if (crossFadeTime < m_MinCrossfadeTime)
                        {
                            crossFadeTime = m_MinCrossfadeTime;
                        }
                        CrossFadeAnimation(logicInfo.Target, Animation_Type.AT_FlyDown, crossFadeTime);
                    }
                    break;
                case HitFlyState.Falling:
                    CharacterController cc = target.GetComponent<CharacterController>();
                    if (cc.isGrounded)
                    {
                        // 落地尘土
                        logicInfo.Target.SendMessage("OnHitGround", SendMessageOptions.DontRequireReceiver);
                        param.ImpactState = HitFlyState.OnGround;
                        param.HitGroundTime = Time.time;
                        PlayAnimation(target, Animation_Type.AT_FlyDownGround, param.FallDownSpeed);
                        SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.HitFly);
                        SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_AddBit, GfxCharacterState_Type.KnockDown);
                    }
                    break;
                case HitFlyState.OnGround:
                    if (IsLogicDead(target))
                    {
                        SetGfxDead(target, true);
                        StopImpact(logicInfo);
                        CrossFadeAnimation(target, Animation_Type.AT_OnGround);
                    }
                    if (Time.time > param.HitGroundTime + param.FallDownTime + logicInfo.ConfigData.OnGroundTime)
                    {
                        // 倒地时间
                        PlayAnimation(target, Animation_Type.AT_GetUp1, param.GetUpSpeed);
                        param.ImpactState = HitFlyState.StandUp;
                        SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.KnockDown);
                        SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_AddBit, GfxCharacterState_Type.GetUp);
                    }
                    break;
                case HitFlyState.StandUp:
                    if (Time.time > param.HitGroundTime + param.FallDownTime + logicInfo.ConfigData.OnGroundTime + param.GetUpTime)
                    {
                        SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_RemoveBit, GfxCharacterState_Type.GetUp);
                        StopImpact(logicInfo);
                    }
                    break;
            }
        }

        public override void StartImpact(ImpactLogicInfo logicInfo)
        {
            GeneralStartImpact(logicInfo);
            SetGfxStateFlag(logicInfo.Target, Operate_Type.OT_AddBit, GfxCharacterState_Type.HitFly);
            float crossFadeTime = Mathf.Abs(logicInfo.Velocity.y / m_Gravity);
            HitFlyParams param = GetHitFlyParams(logicInfo);
            param.ImpactState = HitFlyState.Rising;
            param.OnGroundTime = logicInfo.ConfigData.OnGroundTime;
            if (logicInfo.ConfigData.FallDownTime > 0)
            {
                param.FallDownTime = logicInfo.ConfigData.FallDownTime;
            }
            else
            {
                param.FallDownTime = GetAnimationLenthByType(logicInfo.Target, Animation_Type.AT_FlyDownGround);
            }
            param.FallDownSpeed = GetAnimationLenthByType(logicInfo.Target, Animation_Type.AT_FlyDownGround) / param.FallDownTime;
            if (logicInfo.ConfigData.GetUpTime > 0)
            {
                param.GetUpTime = logicInfo.ConfigData.GetUpTime;
            }
            else
            {
                param.GetUpTime = GetAnimationLenthByType(logicInfo.Target, Animation_Type.AT_GetUp1);
            }
            param.GetUpSpeed = GetAnimationLenthByType(logicInfo.Target, Animation_Type.AT_GetUp1) / param.GetUpTime;
            if (crossFadeTime < m_MinCrossfadeTime)
            {
                crossFadeTime = m_MinCrossfadeTime;
            }
            CrossFadeAnimation(logicInfo.Target, Animation_Type.AT_FlyUp, crossFadeTime);
        }

        public override void StopImpact(ImpactLogicInfo logicInfo)
        {
            if (IsLogicDead(logicInfo.Target))
            {
                //SetGfxDead(logicInfo.Target, true);
            }
            logicInfo.IsActive = false;
            GeneralStopImpact(logicInfo);
        }

        public override void OnInterrupted(ImpactLogicInfo logicInfo)
        {
            logicInfo.IsActive = false;
            GeneralStopImpact(logicInfo);
        }

        protected override void UpdateMovement(ImpactLogicInfo info, float deltaTime)
        {
            // update virtical
            if (null != info.ConfigData && null != info.Target)
            {
                HitFlyParams param = GetHitFlyParams(info);
                float virticalSpeed = info.Velocity.y;
                info.Velocity = info.MoveDir * info.MovementInfo.GetSpeedByTime(Time.time - info.StartTime, -30);
                Vector3 motion = Vector3.zero;
                if (HitFlyState.Rising == param.ImpactState || HitFlyState.Falling == param.ImpactState)
                {
                    info.Velocity = new Vector3(info.Velocity.x, info.Velocity.y, info.Velocity.z);
                }
                else
                {
                    info.Velocity = new Vector3(info.Velocity.x, 0.0f, info.Velocity.z);
                }
                motion = info.Velocity * deltaTime;
                info.NormalPos += motion;
                motion = GfxImpactSystem.Instance.GetAdjustPoint(info.NormalPos - info.OrignalPos, info) + info.OrignalPos - info.Target.transform.position;
                Move(info.Target, motion);
                LogicSystem.NotifyGfxUpdatePosition(info.Target, info.Target.transform.position.x, info.Target.transform.position.y, info.Target.transform.position.z, 0, info.Target.transform.rotation.eulerAngles.y * Mathf.PI / 180f, 0);
            }
        }

        //碰撞后产生的额外特效
        public override bool OnOtherImpact(int logicId, ImpactLogicInfo logicInfo, bool isSameImpact)
        {
            HitFlyParams param = GetHitFlyParams(logicInfo);
            bool result = false;
            switch(logicId)
            {
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Default:
                    break;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_HitFly:
                    if (HitFlyState.Falling == param.ImpactState || HitFlyState.Rising == param.ImpactState)
                    {
                        PlayAnimation(logicInfo.Target, Animation_Type.AT_Hurt0, 1.0f, AnimationBlendMode.Additive);
                    }
                    if (!isSameImpact)
                    {
                        StopImpact(logicInfo);
                    }
                    result = true;
                    break;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_KnockDown:
                    break;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Stiffness:
                    if (HitFlyState.StandUp == param.ImpactState)
                    {
                        StopImpact(logicInfo);
                        result = true;
                    }
                    else if (HitFlyState.Falling == param.ImpactState || HitFlyState.Rising == param.ImpactState)
                    {
                        PlayAnimation(logicInfo.Target, Animation_Type.AT_Hurt0, 1.0f, AnimationBlendMode.Additive);
                    }
                    break;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Grab:
                    StopImpact(logicInfo);
                    result = true;
                    break;
            }
            return result;
        }

        private HitFlyParams GetHitFlyParams(ImpactLogicInfo logicInfo)
        {
            HitFlyParams result = logicInfo.CustomDatas.GetData<HitFlyParams>();
            if (null == result)
            {
                result = new HitFlyParams();
                logicInfo.CustomDatas.AddData<HitFlyParams>(result);
            }
            return result;
        }
    }
}
