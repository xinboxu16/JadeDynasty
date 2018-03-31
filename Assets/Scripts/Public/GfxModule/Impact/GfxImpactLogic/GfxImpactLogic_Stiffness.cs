using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public class GfxImpactLogic_Stiffness : AbstarctGfxImpactLogic
    {
        private enum StiffnessAction
        {
            HURT_FRONT = 0,
            HURT_RIGHT = 1,
            HURT_LEFT = 2,
        }

        public override void Tick(ImpactLogicInfo logicInfo)
        {
            float animSpeed = GetAnimationSpeedByTime(logicInfo, Time.time - logicInfo.StartTime);
            UpdateMovement(logicInfo, Time.deltaTime);
            SetAnimationSpeed(logicInfo.Target, (Animation_Type)logicInfo.ActionType, animSpeed);
            UpdateEffect(logicInfo);
            logicInfo.ElapsedTime += Time.deltaTime * animSpeed;
            logicInfo.ElapsedTimeForEffect += Time.deltaTime * GetLockFrameRate(logicInfo, Time.time - logicInfo.StartTime);
            if (logicInfo.ElapsedTime > GetAnimationLenthByType(logicInfo.Target, (Animation_Type)logicInfo.ActionType))
            {
                StopImpact(logicInfo);
            }
        }

        protected override void UpdateMovement(ImpactLogicInfo info, float deltaTime)
        {
            if(null != info.ConfigData && null != info.Target)
            {
                float speedRate = GetLockFrameRate(info, Time.time - info.StartTime);
                Vector3 motion = info.MoveDir * info.MovementInfo.GetSpeedByTime(info.ElapsedTimeForEffect) * deltaTime * speedRate;
                info.NormalPos += motion;//info.NormalPos是目标位置
                motion = GfxImpactSystem.Instance.GetAdjustPoint(info.NormalPos - info.OrignalPos, info) + info.OrignalPos - info.Target.transform.position;//OrignalPos 原始目标位置
                Vector3 pos = info.Target.transform.position + motion;
                pos = new Vector3(pos.x, GetTerrainHeight(pos), pos.z);
                MoveTo(info.Target, pos);
                LogicSystem.NotifyGfxUpdatePosition(info.Target, info.Target.transform.position.x, info.Target.transform.position.y, info.Target.transform.position.z, 0, info.Target.transform.rotation.eulerAngles.y * Mathf.PI / 180.0f, 0);
            }
        }

        //来自于SendImpactToCharacterImpl方法
        public override void StartImpact(ImpactLogicInfo logicInfo)
        {
            GeneralStartImpact(logicInfo);
            logicInfo.ActionType = GetStiffnessAction(logicInfo);
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(logicInfo.Target);
            if (null != shareInfo)
            {
                shareInfo.GfxStateFlag = shareInfo.GfxStateFlag | (int)GfxCharacterState_Type.Stiffness;//GetBeHitState方法会使用
            }
            PlayAnimation(logicInfo.Target, (Animation_Type)logicInfo.ActionType);
        }

        public override void StopImpact(ImpactLogicInfo logicInfo)
        {
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(logicInfo.Target);
            if (null != shareInfo)
            {
                shareInfo.GfxStateFlag = shareInfo.GfxStateFlag & ~((int)GfxCharacterState_Type.Stiffness);
            }
            logicInfo.IsActive = false;
            GeneralStopImpact(logicInfo);
        }

        public override void OnInterrupted(ImpactLogicInfo logicInfo)
        {
            StopAnimation(logicInfo.Target, (Animation_Type)logicInfo.ActionType);
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(logicInfo.Target);
            if (null != shareInfo)
            {
                shareInfo.GfxStateFlag = shareInfo.GfxStateFlag & ~((int)GfxCharacterState_Type.Stiffness);
            }
            logicInfo.IsActive = false;
            GeneralStopImpact(logicInfo);
        }

        public override bool OnOtherImpact(int logicId, ImpactLogicInfo logicInfo, bool isSameImpact)
        {
            switch (logicId)
            {
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Default:
                    return false;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_HitFly:
                    StopImpact(logicInfo);
                    return true;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_KnockDown:
                    StopImpact(logicInfo);
                    return true;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Stiffness:
                    if (!isSameImpact)
                    {
                        StopImpact(logicInfo);
                    }
                    return true;
                case (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Grab:
                    StopImpact(logicInfo);
                    return true;
                default:
                    return false;
            }
        }

        private int GetStiffnessAction(ImpactLogicInfo logicInfo)
        {
            int type = 0;
            int actionCount = logicInfo.ConfigData.ActionList.Count;
            if (null != logicInfo && actionCount > 0)
            {
                type = logicInfo.ConfigData.ActionList[UnityEngine.Random.Range(0, actionCount)];
            }
            switch (type)
            {
                case (int)StiffnessAction.HURT_FRONT:
                    return (int)Animation_Type.AT_Hurt0;
                case (int)StiffnessAction.HURT_RIGHT:
                    return (int)Animation_Type.AT_Hurt1;
                case (int)StiffnessAction.HURT_LEFT:
                    return (int)Animation_Type.AT_Hurt2;
                default:
                    return (int)Animation_Type.AT_Hurt0;
            }
        }
    }
}
