using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    class ImpactUtility
    {
        private static string[] s_ListSplitString = new string[] { ",", " ", ", ", "|" };

        public static float RadianToDegree(float dir)
        {
            return (float)(dir * 180 / Math.PI);
        }

        public static Vector3 ConvertVector3D(string vec)
        {
            string strPos = vec;
            string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.None);
            Vector3 vector = new Vector3(Convert.ToSingle(resut[0]), Convert.ToSingle(resut[1]), Convert.ToSingle(resut[2]));
            return vector;
        }

        public static void MoveObject(GameObject obj, Vector3 motion)
        {
            CharacterController ctrl = obj.GetComponent<CharacterController>();
            if(null != ctrl)
            {
                ctrl.Move(motion);
            }
            else
            {
                ctrl.transform.position += motion;
            }
        }
    }
}
