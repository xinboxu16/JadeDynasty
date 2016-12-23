using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed partial class GfxSystem
    {
        private MyDictionary<int, MyAction<int, GestureArgs>> m_TouchHandlers = new MyDictionary<int, MyAction<int, GestureArgs>>();

        private void ListenTouchEventImpl(TouchEvent c, MyAction<int, GestureArgs> handler)
        {
            if (m_TouchHandlers.ContainsKey((int)c))
            {
                m_TouchHandlers[(int)c] = handler;
            }
            else
            {
                m_TouchHandlers.Add((int)c, handler);
            }
        }
    }
}
