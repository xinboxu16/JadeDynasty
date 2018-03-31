using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public class GfxImpactLogic_Default : AbstarctGfxImpactLogic
    {
        public override void StartImpact(ImpactLogicInfo logicInfo)
        {
        }

        public override void Tick(ImpactLogicInfo logicInfo)
        {
            UpdateEffect(logicInfo);
            if (Time.time > logicInfo.StartTime + logicInfo.Duration)
            {
                StopImpact(logicInfo);
            }
        }

        public override void StopImpact(ImpactLogicInfo logicInfo)
        {
            foreach (GameObject obj in logicInfo.EffectsDelWithImpact)
            {
                ResourceSystem.RecycleObject(obj);
            }
            logicInfo.IsActive = false;
        }

        public override void OnInterrupted(ImpactLogicInfo logicInfo)
        {
            StopImpact(logicInfo);
        }

        public override bool OnOtherImpact(int logicId, ImpactLogicInfo logicInfo, bool isSameImpact)
        {
            OnInterrupted(logicInfo);
            return true;
        }
    }
}
