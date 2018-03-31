using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule
{
    public static class ScriptableDataUtility
    {
        public static Vector3 CalcVector3(ScriptableData.CallData callData)
        {
            if (null == callData || callData.GetId() != "vector3")
                return Vector3.zero;
            int num = callData.GetParamNum();
            if (3 == num)
            {
                float x = float.Parse(callData.GetParamId(0));
                float y = float.Parse(callData.GetParamId(1));
                float z = float.Parse(callData.GetParamId(2));
                return new Vector3(x, y, z);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static Quaternion CalcEularRotation(ScriptableData.CallData callData)
        {
            if (null == callData || callData.GetId() != "eular")
                return Quaternion.identity;
            int num = callData.GetParamNum();
            if (3 == num)
            {
                float x = float.Parse(callData.GetParamId(0));
                float y = float.Parse(callData.GetParamId(1));
                float z = float.Parse(callData.GetParamId(2));
                return Quaternion.Euler(x, y, z);
            }
            else
            {
                return Quaternion.identity;
            }
        }
    }
}
