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
    }
}
