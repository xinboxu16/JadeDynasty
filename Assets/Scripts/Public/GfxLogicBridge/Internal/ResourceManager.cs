using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    /// <summary>
    /// 资源管理器，提供资源缓存重用机制。
    /// 
    /// todo:分包策略确定后需要修改为从分包里加载资源
    /// </summary>
    internal class ResourceManager
    {
        private Dictionary<string, UnityEngine.Object> m_LoadedPrefabs = new Dictionary<string, UnityEngine.Object>();

        internal UnityEngine.Object GetSharedResource(string res)
        {
            UnityEngine.Object obj = null;
            if (string.IsNullOrEmpty(res))
            {
                return obj;
            }
            if (m_LoadedPrefabs.ContainsKey(res))
            {
                obj = m_LoadedPrefabs[res];
            }
            else
            {
                if (GlobalVariables.Instance.IsPublish)
                {
                    obj = ResUpdateHandler.LoadAssetFromABWithoutExtention(res);
                }

                if (obj == null)
                {
                    obj = Resources.Load(res);
                }

                if(obj != null)
                {
                    m_LoadedPrefabs.Add(res, obj);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("LoadAsset failed:" + res);
                }
            }
            return obj;
        }

        public static ResourceManager Instance
        {
            get { return s_Instance; }
        }
        private static ResourceManager s_Instance = new ResourceManager();
    }
}
