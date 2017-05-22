using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DashFire
{
    public sealed partial class GfxSystem
    {
        private class GameObjectInfo
        {
            public GameObject ObjectInstance;
            public SharedGameObjectInfo ObjectInfo;

            public GameObjectInfo(GameObject o, SharedGameObjectInfo i)
            {
                ObjectInstance = o;
                ObjectInfo = i;
            }
        }

        private GfxSystem() { }

        private LinkedListDictionary<int, GameObjectInfo> m_GameObjects = new LinkedListDictionary<int, GameObjectInfo>();
        private MyDictionary<GameObject, int> m_GameObjectIds = new MyDictionary<GameObject, int>();
        private PublishSubscribeSystem m_EventChannelForLogic = new PublishSubscribeSystem();
        private PublishSubscribeSystem m_EventChannelForGfx = new PublishSubscribeSystem();
        private IActionQueue m_LogicInvoker;
        private MyAction<bool, string, object[]> m_LogicLogCallback;
        private string m_LoadingTip = "";
        private float m_LoadingProgress = 0;
        private string m_VersionInfo = "";
        private long m_LastLogTime = 0;

        //场景相关
        private int m_TargetSceneId = 0;
        private HashSet<int> m_TargetSceneLimitList = null;
        private string m_TargetScene = "";
        private int m_TargetChapter = 0;
        private UnityEngine.AsyncOperation m_LoadingBarAsyncOperation = null;
        private MyAction m_LevelLoadedCallback = null;

        private AsyncActionProcessor m_GfxInvoker = new AsyncActionProcessor();


        private string m_LoadingBarScene = "";

        internal void CallGfxLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            GfxLogImpl(msg);
        }

        //初始化阶段调用的函数
        private void InitImpl()
        {
            m_EventChannelForLogic.RunInLogicThread = true;
            m_EventChannelForGfx.RunInLogicThread = false;
        }

        private void TickImpl()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastLogTime + 10000 < curTime)
            {
                m_LastLogTime = curTime;

                if (m_GfxInvoker.CurActionNum > 10)
                {
                    CallGfxLog("GfxSystem.Tick actionNum:{0}", m_GfxInvoker.CurActionNum);
                }

                m_GfxInvoker.DebugPoolCount((string msg) => {
                    CallGfxLog("GfxActionQueue {0}", msg);
                });
            }
            //未实现
            //HandleSync();
            //HandleInput();
            //HandleLoadingProgress();
            //ResourceManager.Instance.Tick();
            m_GfxInvoker.HandleActions(4096);
        }

        private void ReleaseImpl()
        {

        }

        internal void BeginLoading()
        {
            m_LoadingProgress = 0;
            EventChannelForGfx.Publish("ge_loading_start", "ui");
        }

        internal void EndLoading()
        {
            m_LoadingProgress = 1;
            //延迟处理，在逻辑层逻辑处理之后通知loading条结束，同时也让loading条能走完（视觉效果）。
            if (null != m_LogicInvoker)
            {
                m_LogicInvoker.QueueAction(NotifyGfxEndloading);
            }
        }

        internal void SetLoadingBarScene(string name)
        {
            m_LoadingBarScene = name;
        }

        private void NotifyGfxEndloading()
        {
            GfxSystem.PublishGfxEvent("ge_loading_finish", "ui");
        }

        private void PublishGfxEventImpl(string evt, string group, object[] args)
        {
            m_EventChannelForGfx.Publish(evt, group, args);
        }

        private void SetLogicInvokerImpl(IActionQueue processor)
        {
            m_LogicInvoker = processor;
        }

        private void SetLogicLogCallbackImpl(MyAction<bool, string, object[]> callback)
        {
            m_LogicLogCallback = callback;
        }

        private void SendMessageImpl(string objname, string msg, object arg, bool needReceiver)
        {
            GameObject obj = GameObject.Find(objname);
            if (null != obj)
            {
                try
                {
                    obj.SendMessage(msg, arg, needReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver);
                }
                catch
                {

                }
            }
        }

        private void GfxLogImpl(string msg)
        {
            SendMessageImpl("GfxGameRoot", "LogToConsole", msg, false);
        }

        private void GfxErrorLogImpl(string error)
        {
            SendMessageImpl("GfxGameRoot", "LogToConsole", error, false);
            UnityEngine.Debug.LogError(error);
        }

        //游戏逻辑层执行的函数，供Gfx线程异步调用
        private void PublishLogicEventImpl(string evt, string group, object[] args)
        {
            m_EventChannelForLogic.Publish(evt, group, args);
        }

        internal void PublishLogicEvent(string evt, string group, object[] args)
        {
            if (null != m_LogicInvoker)
            {
                m_LogicInvoker.QueueActionWithDelegation((MyAction<string, string, object[]>)PublishLogicEventImpl, evt, group, args);
            }
        }

        internal void QueueLogicActionWithDelegation(Delegate action, params object[] args)
        {
            if (null != m_LogicInvoker)
            {
                m_LogicInvoker.QueueActionWithDelegation(action, args);
            }
        }

        internal void CallLogicLog(string format, params object[] args)
        {
            QueueLogicActionWithDelegation(m_LogicLogCallback, false, format, args);
        }

        internal void CallLogicErrorLog(string format, params object[] args)
        {
            QueueLogicActionWithDelegation(m_LogicLogCallback, true, format, args);
        }

        internal PublishSubscribeSystem EventChannelForGfx
        {
            get { return m_EventChannelForGfx; }
        }

        internal void UpdateLoadingTip(string tip)
        {
            m_LoadingTip = tip;
        }

        //进度
        internal void UpdateLoadingProgress(float progress)
        {
            m_LoadingProgress = progress;
        }

        internal float GetLoadingProgress()
        {
            return m_LoadingProgress;
        }

        internal string GetLoadingTip()
        {
            return m_LoadingTip;
        }

        internal void UpdateVersionInfo(string info)
        {
            m_VersionInfo = info;
        }

        internal float RadianToDegree(float dir)
        {
            return (float)(dir * 180 / Math.PI);
        }

        internal GameObject GetGameObject(int id)
        {
            GameObject ret = null;
            if (m_GameObjects.Contains(id))
                ret = m_GameObjects[id].ObjectInstance;
            return ret;
        }

        private GameObjectInfo GetGameObjectInfo(int id)
        {
            GameObjectInfo ret = null;
            if (m_GameObjects.Contains(id))
                ret = m_GameObjects[id];
            return ret;
        }

        private int GetGameObjectId(GameObject obj)
        {
            int ret = 0;
            if (m_GameObjectIds.ContainsKey(obj))
            {
                ret = m_GameObjectIds[obj];
            }
            return ret;
        }

        internal SharedGameObjectInfo GetSharedGameObjectInfo(int id)
        {
            SharedGameObjectInfo ret = null;
            if (m_GameObjects.Contains(id))
                ret = m_GameObjects[id].ObjectInfo;
            return ret;
        }

        internal SharedGameObjectInfo GetSharedGameObjectInfo(GameObject obj)
        {
            int id = GetGameObjectId(obj);
            return GetSharedGameObjectInfo(id);
        }

        //恢复材质
        private void RestoreMaterialImpl(int id)
        {
            GameObjectInfo objInfo = GetGameObjectInfo(id);
            if (null == objInfo)
            {
                return;
            }
            GameObject obj = objInfo.ObjectInstance;
            SharedGameObjectInfo info = objInfo.ObjectInfo;
            if (null != obj && null != info)
            {
                if (info.m_SkinedMaterialChanged)////皮肤材质改变
                {
                    SkinnedMeshRenderer[] renderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    int ix = 0;
                    int ct = info.m_SkinedOriginalMaterials.Count;
                    foreach (SkinnedMeshRenderer renderer in renderers)
                    {
                        if (ix < ct)
                        {
                            renderer.materials = info.m_SkinedOriginalMaterials[ix] as Material[];
                            ++ix;
                        }
                    }
                    info.m_SkinedMaterialChanged = false;
                }
                if (info.m_MeshMaterialChanged)//网格材质改变
                {
                    MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
                    int ix = 0;
                    int ct = info.m_MeshOriginalMaterials.Count;
                    foreach (MeshRenderer renderer in renderers)
                    {
                        if (ix < ct)
                        {
                            renderer.materials = info.m_MeshOriginalMaterials[ix] as Material[];
                            ++ix;
                        }
                    }
                    info.m_MeshMaterialChanged = false;
                }
            }
        }

        private void ForgetGameObject(int id, GameObject obj)
        {
            SharedGameObjectInfo info = GetSharedGameObjectInfo(id);
            if (null != info)
            {
                RestoreMaterialImpl(id);
                info.m_SkinedOriginalMaterials.Clear();
                info.m_MeshOriginalMaterials.Clear();
            }
            m_GameObjects.Remove(id);
            m_GameObjectIds.Remove(obj);
        }

        private void DestroyGameObjectImpl(int id)
        {
            try
            {
                GameObject obj = GetGameObject(id);
                if (null != obj)
                {
                    ForgetGameObject(id, obj);
                    obj.SetActive(false);
                    if (!ResourceManager.Instance.RecycleObject(obj))
                    {
                        GameObject.Destroy(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                GfxErrorLogImpl(string.Format("DestroyGameObject:{0} failed:{1}\n{2}", id, ex.Message, ex.StackTrace));
            }
        }

        //Gfx线程执行的函数，供游戏逻辑线程异步调用
        private void LoadSceneImpl(string name, int chapter, int sceneId, HashSet<int> limitList, MyAction onFinish)
        {
            CallLogicLog("Begin LoadScene:{0}", name);
            m_TargetScene = name;
            m_TargetChapter = chapter;
            m_TargetSceneId = sceneId;
            m_TargetSceneLimitList = limitList;
            BeginLoading();
            if (null == m_LoadingBarAsyncOperation)
            {
                m_LoadingBarAsyncOperation = SceneManager.LoadSceneAsync(m_LoadingBarScene);
                m_LevelLoadedCallback = onFinish;
            }
        }
    }
}
