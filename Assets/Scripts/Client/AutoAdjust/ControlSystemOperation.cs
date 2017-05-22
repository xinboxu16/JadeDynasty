using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ControlSystemOperation
    {
        private static ControlSystemHelper s_Helper = new ControlSystemHelper();

        public static void Reset()
        {
            s_Helper.System.Reset();
        }

        private sealed class ControlSystemHelper
        {
            private ControlSystem m_ControlSystem = new ControlSystem();

            internal ControlSystem System
            {
                get { return m_ControlSystem; }
            }
        }
    }
}
