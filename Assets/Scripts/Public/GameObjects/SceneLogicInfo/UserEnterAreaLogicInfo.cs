using System;
using System.Collections.Generic;
using UnityEngine;

namespace DashFire
{
    public class UserEnterAreaLogicInfo
    {
        public enum TiggerTypeEnum : int
        {
            Any = 0,
            All,
        }
        public Vector3[] m_Area = null;
        public TiggerTypeEnum m_TriggerType = TiggerTypeEnum.Any;
        public bool m_IsTriggered = true;
    }
}
