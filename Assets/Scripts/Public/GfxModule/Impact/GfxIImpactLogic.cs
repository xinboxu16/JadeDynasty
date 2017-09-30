using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
