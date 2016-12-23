using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DashFire
{
    /**
     * @brief 获取游戏工作目录
     */
    public class HomePath
    {
        private static string m_HomePath = "";
        public static string CurHomePath
        {
            get { return m_HomePath; }
            set { m_HomePath = value; }
        }

        public static string GetAbsolutePath(string path)
        {
            return Path.Combine(m_HomePath, path);
        }
    }
}
