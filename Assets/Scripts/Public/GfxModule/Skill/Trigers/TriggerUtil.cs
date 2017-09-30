using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GfxModule.Skill.Trigers
{
    public class TriggerUtil
    {
        public static void OnFingerDown(GestureArgs e)
        {
            if(LogicSystem.PlayerSelfInfo != null)
            {
                LogicSystem.PlayerSelfInfo.IsTouchDown = true;
            }
        }

        public static void OnFingerUp(DashFire.GestureArgs e)
        {
            if (LogicSystem.PlayerSelfInfo != null)
            {
                LogicSystem.PlayerSelfInfo.IsTouchDown = false;
            }
        }
    }
}
