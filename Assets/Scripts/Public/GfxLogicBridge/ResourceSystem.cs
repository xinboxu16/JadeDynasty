using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public static class ResourceSystem
    {
        public static UnityEngine.Object GetSharedResource(string res)
        {
            return ResourceManager.Instance.GetSharedResource(res);
        }

        public static bool RecycleObject(UnityEngine.Object obj)
        {
            return ResourceManager.Instance.RecycleObject(obj);
        }

        public static UnityEngine.Object NewObject(string res, float timeToRecycle)
        {
            return ResourceManager.Instance.NewObject(res, timeToRecycle);
        }

        public static UnityEngine.Object NewObject(UnityEngine.Object prefab)
        {
            return ResourceManager.Instance.NewObject(prefab);
        }

        public static UnityEngine.Object NewObject(UnityEngine.Object prefab, float timeToRecycle)
        {
            return ResourceManager.Instance.NewObject(prefab, timeToRecycle);
        }
    }
}
