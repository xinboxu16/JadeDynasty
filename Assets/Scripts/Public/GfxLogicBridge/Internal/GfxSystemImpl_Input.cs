using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed partial class GfxSystem
    {
        private bool m_IsLastHitUi;
        public bool IsLastHitUi
        {
            get { return m_IsLastHitUi; }
            internal set { m_IsLastHitUi = value; }
        }

        private MyDictionary<int, MyAction<int, int>> m_KeyboardHandlers = new MyDictionary<int, MyAction<int, int>>();
        private HashSet<int> m_KeysForListen = new HashSet<int>();

        private void ListenKeyboardEventImpl(Keyboard.Code c, MyAction<int, int> handler)
        {
            if (m_KeyboardHandlers.ContainsKey((int)c))
            {
                m_KeyboardHandlers[(int)c] = handler;
            }
            else
            {
                m_KeyboardHandlers.Add((int)c, handler);
            }
        }

        private void ListenKeyPressStateImpl(Keyboard.Code[] codes)
        {
            foreach (Keyboard.Code c in codes)
            {
                if (!m_KeysForListen.Contains((int)c))
                {
                    m_KeysForListen.Add((int)c);
                }
            }
        }
    }
}
