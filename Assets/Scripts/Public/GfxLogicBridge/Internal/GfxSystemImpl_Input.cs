using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public sealed partial class GfxSystem
    {
        private bool m_IsLastHitUi;
        private Vector3 m_LastMousePos;
        private Vector3 m_CurMousePos;
        private Vector3 m_MouseRayPoint;

        public bool IsLastHitUi
        {
            get { return m_IsLastHitUi; }
            internal set { m_IsLastHitUi = value; }
        }

        private MyDictionary<int, MyAction<int, int>> m_KeyboardHandlers = new MyDictionary<int, MyAction<int, int>>();
        private HashSet<int> m_KeysForListen = new HashSet<int>();
        private bool[] m_KeyPressed = new bool[(int)Keyboard.Code.MaxNum];
        private bool[] m_ButtonPressed = new bool[(int)Mouse.Code.MaxNum];

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

        private void HandleInput()
        {
            m_LastMousePos = m_CurMousePos;
            m_CurMousePos = Input.mousePosition;

            //sqrMagnitude 向量的长度是用勾股定理计算出来，计算机计算两次方和开根的运算量比加减法要费时的多。所以如果是想比较两个向量的长度，用sqrMagnitude可以快出很多。
            if ((m_CurMousePos - m_LastMousePos).sqrMagnitude >= 1 && null != Camera.main)
            {
                Ray ray = Camera.main.ScreenPointToRay(m_CurMousePos);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    m_MouseRayPoint = hitInfo.point;
                }
            }

            foreach (int key in m_KeysForListen)
            {
                if (Input.GetKeyDown((KeyCode)key))
                {
                    m_KeyPressed[key] = true;
                    FireKeyboard(key, (int)Keyboard.Event.Down);
                }
                else if (Input.GetKeyUp((KeyCode)key))
                {
                    m_KeyPressed[key] = false;
                    FireKeyboard(key, (int)Keyboard.Event.Up);
                }
            }
        }

        private void FireKeyboard(int c, int e)
        {
            if (null != m_LogicInvoker && m_KeyboardHandlers.ContainsKey(c))
            {
                MyAction<int, int> handler = m_KeyboardHandlers[c];
                QueueLogicActionWithDelegation(handler, c, e);
            }
        }

        public bool IsKeyPressedImpl(Keyboard.Code c)
        {
            return m_KeyPressed[(int)c];
        }


        private float GetMouseXImpl()
        {
            return m_CurMousePos.x;
        }
        private float GetMouseYImpl()
        {
            return m_CurMousePos.y;
        }
        private float GetMouseZImpl()
        {
            return m_CurMousePos.z;
        }
    }
}
