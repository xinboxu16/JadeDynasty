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
        private HashSet<int> m_PreloadResources = new HashSet<int>();
        private ObjectPool<UsedResourceInfo> m_UsedResourceInfoPool = new ObjectPool<UsedResourceInfo>();

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
                    gameObj.transform.SetParent(null);
                    //gameObj.transform.parent = null;
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

        internal void CleanupResourcePool()
        {
            for(LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstValue; null != node;)
            {
                UsedResourceInfo resInfo = node.Value;
                if (!m_PreloadResources.Contains(resInfo.m_ResId))
                {
                    node = node.Next;
                    RemoveFromUsedResources(resInfo.m_ResId);
                    resInfo.Recycle();
                }else
                {
                    node = node.Next;
                }
            }
        }

        internal UnityEngine.Object NewObject(string res)
        {
            return NewObject(res, 0);
        }

        internal UnityEngine.Object NewObject(string res, float timeToRecycle)
        {
            UnityEngine.Object prefab = GetSharedResource(res);
            return NewObject(prefab, timeToRecycle);
        }

        internal UnityEngine.Object NewObject(UnityEngine.Object prefab)
        {
            return NewObject(prefab, 0);
        }

        internal UnityEngine.Object NewObject(UnityEngine.Object prefab, float timeToRecycle)
        {
            UnityEngine.Object obj = null;
            if(null != prefab)
            {
                float curTime = Time.time;
                float time = timeToRecycle;
                if (timeToRecycle > 0)
                {
                    time += curTime;
                }
                int resId = prefab.GetInstanceID();
                obj = NewFromUnusedResources(resId);
                if (null == obj)
                {
                    //这里克隆了一个物体，运行这一句发现，克隆的物体出现了在 Hieraychy 的最外层，而且查看 Transform 信息发现，和 源 GameObject上的数据一致。
                    //所以在克隆的时候，Unity 复制了 Transform 的信息，然后把 克隆出来的物体，扔到了世界坐标系中http://blog.csdn.net/huutu/article/details/50754754
                    obj = GameObject.Instantiate(prefab);
                }
                if (null != obj)
                {
                    AddToUsedResources(obj, resId, time);
                    InitializeObject(obj);
                }
            }
            return obj;
        }

        private UnityEngine.Object NewFromUnusedResources(int res)
        {
            UnityEngine.Object obj = null;
            if(m_UnusedResources.ContainsKey(res))
            {
                Queue<UnityEngine.Object> queue = m_UnusedResources[res];
                if(queue.Count > 0)
                {
                    obj = queue.Dequeue();
                }
            }
            return obj;
        }

        private void AddToUsedResources(UnityEngine.Object obj, int resId, float recycleTime)
        {
            int objId = obj.GetInstanceID();
            if(!m_UsedResources.Contains(objId))
            {
                UsedResourceInfo info = m_UsedResourceInfoPool.Alloc();
                info.m_ObjId = objId;
                info.m_Object = obj;
                info.m_ResId = resId;
                info.m_RecycleTime = recycleTime;

                m_UsedResources.AddLast(objId, info);
            }
        }

        private void InitializeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if(null != gameObj)
            {
                if(!gameObj.activeSelf)
                {
                    gameObj.SetActive(true);
                }
                ParticleSystem ps = gameObj.GetComponent<ParticleSystem>();
                if(null != ps && ps.main.playOnAwake)
                {
                    ps.Play();
                }
            }
        }

        internal void Tick()
        {
            float curTime = Time.time;

            for (LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstValue; null != node; )
            {
                UsedResourceInfo resInfo = node.Value;
                if (resInfo.m_RecycleTime > 0 && resInfo.m_RecycleTime < curTime)
                {
                    node = node.Next;

                    GameObject gameObject = resInfo.m_Object as GameObject;
                    if (null != gameObject)
                    {
                        //LogicSystem.LogicLog("RecycleObject {0} {1} by Tick", gameObject.name, gameObject.tag);
                    }

                    FinalizeObject(resInfo.m_Object);
                    AddToUnusedResources(resInfo.m_ResId, resInfo.m_Object);
                    RemoveFromUsedResources(resInfo.m_ObjId);
                    resInfo.Recycle();
                }
                else
                {
                    node = node.Next;
                }
            }
        }

        public static ResourceManager Instance
        {
            get { return s_Instance; }
        }
        private static ResourceManager s_Instance = new ResourceManager();
    }
}
