using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * @brief 转换工具类
 */

namespace DashFire
{
    /**
     * @brief 资源类解析工具
     */
    public class Converter
    {
        private static string[] s_ListSplitString = new string[] { ",", " ", ", ", "|" };
        /**
         * @brief 将字符串解析为int数组
         *
         * @param vec 字符串，类似于"1,2,3,4"
         *
         * @return 
         */
        public static List<T> ConvertNumericList<T>(string vec)
        {
            List<T> list = new List<T>();
            string strPos = vec;
            string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.None);

            try
            {
                if (resut != null && resut.Length > 0 && resut[0] != "")
                {
                    for (int index = 0; index < resut.Length; index++)
                    {
                        list.Add((T)Convert.ChangeType(resut[index], typeof(T)));
                    }
                }
            }
            catch (System.Exception ex)
            {
                string info = string.Format("ConvertNumericList vec:{0} ex:{1} stacktrace:{2}",
                  vec, ex.Message, ex.StackTrace);
                LogSystem.Debug(info);

                list.Clear();
            }

            return list;
        }

        /**
         * @brief 将字符串解析为字符串数组
         *
         * @param vec 字符串,类似于"100,200,200"
         *
         * @return 
         */
        public static List<string> ConvertStringList(string vec)
        {
            string[] resut = vec.Split(s_ListSplitString, StringSplitOptions.None);
            List<string> list = new List<string>();
            foreach (string str in resut)
            {
                list.Add(str);
            }

            return list;
        }

        /**
         * @brief 将字符串解析为Vector3D
         *
         * @param vec 字符串,类似于"100,200,200"
         *
         * @return 
         */
        public static Vector3 ConvertVector3D(string vec)
        {
            string strPos = vec;
            string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.None);
            Vector3 vector = new Vector3(Convert.ToSingle(resut[0]), Convert.ToSingle(resut[1]), Convert.ToSingle(resut[2]));
            return vector;
        }
    }
}
