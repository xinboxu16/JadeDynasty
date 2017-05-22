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

        private class UsedResourceInfo : IPoolAllocatedObject<UsedResourceInfo>
        {
            private ObjectPool<UsedResourceInfo> m_Pool = null;

            internal int m_ObjId;
            internal UnityEngine.Object m_Object;
            internal int m_ResId;
            internal float m_RecycleTime;

            public void InitPool(ObjectPool<UsedResourceInfo> pool)
            {
                m_Pool = pool;
            }

            internal void Recycle()
            {
                m_Object = null;
                m_Pool.Recycle(this);
            }

            public UsedResourceInfo Downcast()
            {
                return this;
            }
        }

        private Dictionary<string, UnityEngine.Object> m_LoadedPrefabs = new Dictionary<string, UnityEngine.Object>();
        private LinkedListDictionary<int, UsedResourceInfo> m_UsedResources = new LinkedListDictionary<int, UsedResourceInfo>();
        private Dictionary<int, Queue<UnityEngine.Object>> m_UnusedResources = new Dictionary<int, Queue<UnityEngine.Object>>();

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

        private void RemoveFromUsedResources(int objId)
        {
            m_UsedResources.Remove(objId);
        }

        //使对象结束
        private void FinalizeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj)
            {
                ParticleSystem ps0 = gameObj.GetComponent<ParticleSystem>();
                if (null != ps0 && ps0.main.playOnAwake)
                {
                    ps0.Stop();
                }
                ParticleSystem[] pss = gameObj.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in pss)
                {
                    if (null != ps)
                    {
                        ps.Clear();
                    }
                }
                if (null != gameObj.transform.parent)
                {
                    gameObj.transform.parent = null;
                }
                if (gameObj.activeSelf)
                {
                    gameObj.SetActive(false);
                }
            }
        }

        private void AddToUnusedResources(int res, UnityEngine.Object obj)
        {
            if (m_UnusedResources.ContainsKey(res))
            {
                Queue<UnityEngine.Object> queue = m_UnusedResources[res];
                queue.Enqueue(obj);
            }
            else
            {
                Queue<UnityEngine.Object> queue = new Queue<UnityEngine.Object>();
                queue.Enqueue(obj);
                m_UnusedResources.Add(res, queue);
            }
        }

        internal bool RecycleObject(UnityEngine.Object obj)
        {
            bool ret = false;
            if (null != obj)
            {
                UnityEngine.GameObject gameObject = obj as UnityEngine.GameObject;
                if (null != gameObject)
                {
                    LogicSystem.LogicLog("RecycleObject {0} {1}", gameObject.name, gameObject.tag);
                }

                int objId = obj.GetInstanceID();
                if (m_UsedResources.Contains(objId))
                {
                    UsedResourceInfo resInfo = m_UsedResources[objId];
                    if (null != resInfo)
                    {
                        FinalizeObject(resInfo.m_Object);
                        RemoveFromUsedResources(objId);
                        AddToUnusedResources(resInfo.m_ResId, obj);
                        resInfo.Recycle();
                        ret = true;
                    }
                }
            }
            return ret;
        }

        public static ResourceManager Instance
        {
            get { return s_Instance; }
        }
        private static ResourceManager s_Instance = new ResourceManager();
    }
}
