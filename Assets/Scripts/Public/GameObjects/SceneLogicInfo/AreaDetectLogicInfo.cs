using System;
using System.Collections.Generic;
using UnityEngine;

namespace DashFire
{
    public class AreaDetectLogicInfo
    {
        public enum TiggerTypeEnum : int
        {
            Npc = 0,
            User,
            All,
        }
        public Vector3[] m_Area = null;
        public TiggerTypeEnum m_TriggerType = TiggerTypeEnum.Npc;
        public bool m_IsTriggered = true;
        public long m_Timeout = 0;
        public long m_CurTime = 0;
    }
}
