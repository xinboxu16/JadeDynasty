using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    /// <summary>
    /// 这里放客户端与服务器存在差异的变量值，供各公共模块使用（如果是各模块所需的逻辑数据，则不要放在这里，独立写读表器）。
    /// </summary>
    public class GlobalVariables
    {
        private bool m_IsPublish = false;
        private bool m_IsClient = false;
        private bool m_IsDebug = false;

        public static GlobalVariables Instance
        {
            get { return s_Instance; }
        }
        private static GlobalVariables s_Instance = new GlobalVariables();

        public bool IsPublish
        {
            get { return m_IsPublish; }
            set { m_IsPublish = value; }
        }

        public bool IsClient
        {
            get
            {
                return m_IsClient;
            }
            set
            {
                m_IsClient = value;
            }
        }

        public bool IsDebug
        {
            get { return m_IsDebug; }
            set { m_IsDebug = value; }
        }
    }
}
