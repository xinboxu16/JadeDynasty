using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ControlSystem
    {
        private List<IController> m_Controllers = new List<IController>();

        public void Reset()
        {
            int ct = m_Controllers.Count;
            for (int ix = ct - 1; ix >= 0; --ix)
            {
                IController ctrl = m_Controllers[ix];
                ctrl.Recycle();
            }
            m_Controllers.Clear();
        }
    }
}
