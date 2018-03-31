using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DashFire
{
    public delegate void BeforeLoadSceneDelegation(string curName, string targetName, int targetSceneId);
    public delegate void AfterLoadSceneDelegation(string targetName, int targetSceneId);

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

        private object m_SyncLock = new object();

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
        private UnityEngine.AsyncOperation m_LoadingLevelAsyncOperation = null;
        private MyAction m_LevelLoadedCallback = null;

        private IGameLogicNotification m_GameLogicNotification = null;
        private GameObjectInfo m_PlayerSelf = null;

        private ResAsyncInfo m_UpdateChapterInfo = null;
        private ResAsyncInfo m_LoadCacheResInfo = null;
        private bool m_LoadScenePaused = false;
        private BeforeLoadSceneDelegation m_OnBeforeLoadScene;
        private AfterLoadSceneDelegation m_OnAfterLoadScene;

        private AsyncActionProcessor m_GfxInvoker = new AsyncActionProcessor();


        private string m_LoadingBarScene = "";

        private float c_MinTerrainHeight = 120.0f;

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
            
            HandleSync();
            HandleInput();
            HandleLoadingProgress();
            ResourceManager.Instance.Tick();//每帧调用一次TODO:清理资源
            m_GfxInvoker.HandleActions(4096);
        }

        private void HandleSync()
        {
            if(Monitor.TryEnter(m_SyncLock))
            {
                try
                {
                    for (LinkedListNode<GameObjectInfo> node = m_GameObjects.FirstValue; null != node; node = node.Next)
                    {
                        GameObjectInfo info = node.Value;
                        if(null != info && null != info.ObjectInstance && null != info.ObjectInfo)
                        {
                            if(info.ObjectInfo.DataChangedByLogic)
                            {
                                Debug.Log("info.ObjectInfo.DataChangedByLogic");
                                Vector3 pos = new Vector3(info.ObjectInfo.X, info.ObjectInfo.Y, info.ObjectInfo.Z);
                                GameObject obj = info.ObjectInstance;
                                Vector3 old = obj.transform.position;
                                CharacterController ctrl = obj.GetComponent<CharacterController>();
                                if(null != ctrl)
                                {
                                    ctrl.Move(pos - old);
                                }
                                else
                                {
                                    info.ObjectInstance.transform.position = pos;
                                }
                                info.ObjectInstance.transform.rotation = Quaternion.Euler(0, RadianToDegree(info.ObjectInfo.FaceDir), 0);

                                info.ObjectInfo.DataChangedByLogic = false;
                            }
                            else
                            {
                                if(!info.ObjectInfo.IsGfxMoveControl)
                                {
                                    if(info.ObjectInfo.IsLogicMoving)
                                    {
                                        GameObject obj = info.ObjectInstance;
                                        Vector3 old = obj.transform.position;
                                        Vector3 pos;
                                        float distance = info.ObjectInfo.MoveSpeed * Time.deltaTime;
                                        //Debug.Log("info.ObjectInfo.IsLogicMoving" + distance);
                                        if(distance * distance < info.ObjectInfo.MoveTargetDistanceSqr)
                                        {
                                            float dz = distance * info.ObjectInfo.MoveCos;
                                            float dx = distance * info.ObjectInfo.MoveSin;

                                            if(info.ObjectInfo.CurTime + Time.deltaTime < info.ObjectInfo.TotalTime)
                                            {
                                                info.ObjectInfo.CurTime += Time.deltaTime;
                                                float scale = Time.deltaTime / info.ObjectInfo.TotalTime;
                                                dx += info.ObjectInfo.AdjustDx * scale;
                                                dz += info.ObjectInfo.AdjustDz * scale;
                                            }
                                            else
                                            {
                                                info.ObjectInfo.TotalTime = 0;
                                            }
                                            
                                            CharacterController ctrl = obj.GetComponent<CharacterController>();
                                            if (null != ctrl)
                                            {
                                                ctrl.Move(new Vector3(dx, 0, dz));//一个复杂的运动增量
                                                pos = obj.transform.position;
                                                Debug.Log("CharacterController.collisionFlags" + ctrl.collisionFlags);
                                                //collisionFlags 在最后的CharacterController.Move调用期间，胶囊体的哪个部分与周围环境相碰撞。
                                                //CollisionFlags.CollidedBelow  监测底部发生碰撞  flags & CollisionFlags.CollidedBelow 返回1  
                                                //CollisionFlags.CollidedSides  监测顶部发生碰撞  flags & CollisionFlags.CollidedSides 返回1  
                                                //CollisionFlags.CollidedAbove    监测四周发生碰撞  flags & CollisionFlags.CollidedAbove 返回1 
                                                if (info == m_PlayerSelf && ctrl.collisionFlags == CollisionFlags.Sides)
                                                {
                                                    if (null != m_GameLogicNotification && null != m_LogicInvoker)
                                                    {
                                                        m_LogicInvoker.QueueActionWithDelegation((MyAction<int>)m_GameLogicNotification.OnGfxMoveMeetObstacle, info.ObjectInfo.m_LogicObjectId);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pos = old + new Vector3(dx, 0, dz);
                                            }

                                            info.ObjectInfo.X = pos.x;
                                            info.ObjectInfo.Y = pos.y;
                                            info.ObjectInfo.Z = pos.z;

                                            info.ObjectInfo.DataChangedByGfx = true;
                                        }
                                    }

                                    Vector3 nowPos = info.ObjectInstance.transform.position;
                                    float terrainHeight = SampleTerrainHeight(nowPos.x, nowPos.z);
                                    if (!info.ObjectInfo.IsFloat && nowPos.y > terrainHeight)
                                    {
                                        float cur_height = nowPos.y + info.ObjectInfo.VerticlaSpeed * Time.deltaTime - 9.8f * Time.deltaTime * Time.deltaTime / 2;
                                        if (cur_height < terrainHeight)
                                        {
                                            cur_height = terrainHeight;
                                        }
                                        info.ObjectInfo.VerticlaSpeed += -9.8f * Time.deltaTime;
                                        CharacterController cc = info.ObjectInstance.GetComponent<CharacterController>();
                                        if (null != cc)
                                        {
                                            cc.Move(new Vector3(nowPos.x, cur_height, nowPos.z) - nowPos);
                                        }
                                        else
                                        {
                                            info.ObjectInstance.transform.position = new Vector3(nowPos.x, cur_height, nowPos.z);
                                        }
                                        info.ObjectInfo.Y = cur_height;
                                        info.ObjectInfo.DataChangedByGfx = true;
                                    }
                                    else
                                    {
                                        info.ObjectInfo.VerticlaSpeed = 0;
                                    }
                                    info.ObjectInstance.transform.rotation = Quaternion.Euler(RadianToDegree(0), RadianToDegree(info.ObjectInfo.FaceDir), RadianToDegree(0));
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(m_SyncLock);
                }
            }
        }

        private void HandleLoadingProgress()
        {
            if (GlobalVariables.Instance.IsPublish && m_LoadScenePaused)
            {
                return;
            }
            //先等待loading bar加载完成,发起对目标场景的加载
            if(null != m_LoadingBarAsyncOperation)
            {
                if(m_LoadingBarAsyncOperation.isDone)
                {
                    m_LoadingBarAsyncOperation = null;

                    CallLogicLog("HandleLoadingProgress m_LoadingBarAsyncOperation.isDone");

                    if(null != m_OnBeforeLoadScene)
                    {
                        m_OnBeforeLoadScene(SceneManager.GetActiveScene().name, m_TargetScene, m_TargetSceneId);
                    }

                    if (GlobalVariables.Instance.IsPublish)
                    {
                        ResUpdateHandler.Cleanup();
                    }

                    ResourceManager.Instance.CleanupResourcePool();
                    if (GlobalVariables.Instance.IsPublish)
                    {
                        ResUpdateHandler.InitUpdate();
                        ResUpdateHandler.SetUpdateProgressRange(0, 1.0f, 1);
                        m_UpdateChapterInfo = ResUpdateHandler.StartUpdateChapter(m_TargetChapter);
                    }
                    else
                    {
                        m_LoadingLevelAsyncOperation = SceneManager.LoadSceneAsync(m_TargetScene);
                        UpdateLoadingTip("加载场景不费流量");
                    }
                }
            }
            else if(m_UpdateChapterInfo != null)
            {
                if (m_UpdateChapterInfo.IsDone)
                {
                    m_UpdateChapterInfo = null;
                    CallLogicLog("HandleLoadingProgress m_UpdateChapterInfo.IsDone");

                    UpdateLoadingProgress(0.0f);
                    UpdateLoadingTip("加载场景不费流量");
                    ResUpdateHandler.SetUpdateProgressRange(0.0f, 0.5f, 1);
                    List<ResCacheConfig> cacheConfigList = new List<ResCacheConfig>();
                    ResCacheConfig levelConfig = new ResCacheConfig(ResCacheType.level, m_TargetSceneId);
                    cacheConfigList.Add(levelConfig);
                    if (m_TargetSceneLimitList != null)
                    {
                        levelConfig.LinkLimitList = m_TargetSceneLimitList;
                    }
                    m_LoadCacheResInfo = ResUpdateHandler.CacheResByConfig(cacheConfigList);
                }else if (m_UpdateChapterInfo.IsError)
                {
                    CallLogicLog("HandleLoadingProgress m_UpdateChapterInfo.IsError");

                    ReStartLoad();
                }
            }
            else if (m_LoadCacheResInfo != null)
            {
                if (m_LoadCacheResInfo.IsDone)
                {
                    m_LoadCacheResInfo = null;
                    ResUpdateHandler.ExitUpdate();

                    CallLogicLog("HandleLoadingProgress m_LoadCacheResInfo.IsDone");

                    string levelName = m_TargetScene;
                    if (GlobalVariables.Instance.IsPublish)
                    {
                        levelName = levelName.ToLower();
                    }
                    m_LoadingLevelAsyncOperation = SceneManager.LoadSceneAsync(levelName);
                    //UpdateLoadingTip("加载场景数据...");
                }
                else if (m_LoadCacheResInfo.IsError)
                {

                    CallLogicLog("HandleLoadingProgress m_LoadCacheResInfo.IsError");

                    ReStartLoad();
                }
            }
            else if (null != m_LoadingLevelAsyncOperation)
            {
                //再等待目标场景加载
                if(m_LoadingLevelAsyncOperation.isDone)
                {
                    if(GlobalVariables.Instance.IsPublish)
                    {
                        ResUpdateHandler.ReleaseAllAssetBundle();
                    }
                    m_LoadingLevelAsyncOperation = null;

                    //加载模型
                    Resources.Load("Monster/Campaign_Desert/01_TLPMArmy/5_Mon_TLPMSpear_01");

                    CallLogicLog("HandleLoadingProgress m_LoadingLevelAsyncOperation.IsDone");

                    UpdateLoadingTip("场景加载完成...");
                    Resources.UnloadUnusedAssets();

                    EndLoading();
                    CallLogicLog("End LoadScene:{0}", m_TargetScene);

                    //new DelayInvoke().Delay(() =>
                    //    {
                    //    }, 5);

                    if (null != m_LogicInvoker && null != m_LevelLoadedCallback)
                    {
                        QueueLogicActionWithDelegation(m_LevelLoadedCallback);
                        m_LevelLoadedCallback = null;
                    }

                    if(null != m_OnAfterLoadScene)
                    {
                        m_OnAfterLoadScene(m_TargetScene, m_TargetSceneId);
                    }
                }
                else
                {
                    UpdateLoadingProgress(0.5f + m_LoadingLevelAsyncOperation.progress * 0.5f);
                }
            }

        }

        private void ReStartLoad()
        {
            ResUpdateHandler.IncReconnectNum();
            m_LoadScenePaused = true;
            string info = "网络连接错误,请重试连接";
            Action<bool> fun = new Action<bool>(delegate(bool selected)
            {
                if (selected)
                {
                    m_LoadScenePaused = false;
                    m_LoadCacheResInfo = null;
                    m_UpdateChapterInfo = null;
                    m_LoadingBarAsyncOperation = null;
                    m_LoadingLevelAsyncOperation = null;
                    ResUpdateHandler.ExitUpdate();
                    LoadSceneImpl(m_TargetScene, m_TargetChapter, m_TargetSceneId, m_TargetSceneLimitList, m_LevelLoadedCallback);
                }
            });
            DashFire.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", info, fun);
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

        private void UpdateGameObjectLocalRotateYImpl(int id, float ry)
        {
            GameObject obj = GetGameObject(id);
            if (null != obj)
            {
                float rx = obj.transform.localRotation.eulerAngles.x;
                float rz = obj.transform.localRotation.eulerAngles.z;
                obj.transform.localRotation = Quaternion.Euler(rx, RadianToDegree(ry), rz);
            }
        }

        private void UpdateGameObjectLocalPosition2DImpl(int id, float x, float z, bool attachTerrain)
        {
            GameObject obj = GetGameObject(id);
            if (null != obj)
            {
                float y = 0;
                if(attachTerrain)
                {
                    y = SampleTerrainHeight(x, z);
                }
                else
                {
                    y = obj.transform.localPosition.y;
                }
                obj.transform.localPosition = new Vector3(x, y, z);    
            }
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

        private void SetGameLogicNotificationImpl(IGameLogicNotification notification)
        {
            m_GameLogicNotification = notification;
        }

        private void SendMessageWithTagImpl(string objtag, string msg, object arg, bool needReceiver)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(objtag);
            if (null != objs)
            {
                foreach (GameObject obj in objs)
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

        private void SendMessageByIdImpl(int objid, string msg, object arg, bool needReceiver)
        {
            GameObject obj = GetGameObject(objid);
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

        private void ResetInputStateImpl()
        {
            for (int i = 0; i < (int)Keyboard.Code.MaxNum; ++i)
            {
                m_KeyPressed[i] = false;
            }
            for (int i = 0; i < (int)Mouse.Code.MaxNum; ++i)
            {
                m_ButtonPressed[i] = false;
            }
        }

        private void StopAnimationImpl(int id, string animationName)
        {
            GameObject obj = GetGameObject(id);
            Animation animation = obj.GetComponent<Animation>();
            if(null != obj && null != animation)
            {
                if(null != animation[animationName])
                {
                    animation.Stop(animationName);
                }
                else
                {
                    CallLogicErrorLog("Obj {0} StopAnimation {1} AnimationState is null, clipcount {2}", id, animationName, animation.GetClipCount());
                }
            }
        }

        private void SetAnimationSpeedImpl(int id, string animationName, float speed)
        {
            GameObject obj = GetGameObject(id);
            Animation animation = obj.GetComponent<Animation>();
            if(null != obj && null != animation)
            {
                AnimationState state = animation[animationName];
                if(null != state)
                {
                    state.speed = speed;
                }
                else
                {
                    CallLogicErrorLog("Obj {0} SetAnimationSpeed {1} AnimationState is null, clipcount {2}", id, animationName, animation.GetClipCount());
                }
            }
        }

        private void PlayQueuedAnimationImpl(int id, string animationName, bool isPlayNow, bool isStopAll)
        {
            GameObject obj = GetGameObject(id);
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                if (null != animation[animationName])
                {
                    animation.PlayQueued(animationName, isPlayNow ? QueueMode.PlayNow : QueueMode.CompleteOthers, isStopAll ? PlayMode.StopAll : PlayMode.StopSameLayer);
                }
                else
                {
                    CallLogicErrorLog("Obj {0} PlayQueuedAnimation {1} AnimationState is null, clipcount {2}", id, animationName, animation.GetClipCount());
                }
            }
        }

        private void SetShaderImpl(int id, string shaderPath)
        {
            GameObject obj = GetGameObject(id);
            if(null == obj)
            {
                return;
            }
            Shader shader = Shader.Find(shaderPath);
            if (null == shader)
            {
                CallLogicErrorLog("id={0} obj can't find shader {1}!", id, shaderPath);
                return;
            }
            SkinnedMeshRenderer[] renderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < renderers.Length; ++i)
            {
                if (renderers[i].material.shader != shader)
                {
                    renderers[i].material.shader = shader;
                }
            }
        }

        private void SetBlockedShaderImpl(int id, uint rimColor, float rimPower, float cutValue)
        {
            GameObjectInfo objInfo = GetGameObjectInfo(id);
            if (null == objInfo || null == objInfo.ObjectInstance || null == objInfo.ObjectInfo)
            {
                return;
            }
            bool needChange = false;
            SkinnedMeshRenderer[] skinnedRenderers = objInfo.ObjectInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    string name = mat.shader.name;
                    if (0 != name.CompareTo("DFM/Blocked") && 0 != name.CompareTo("DFM/NotBlocked"))
                    {
                        needChange = true;
                    }
                }
            }
            MeshRenderer[] meshRenderers = objInfo.ObjectInstance.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    string name = mat.shader.name;
                    if (0 != name.CompareTo("DFM/Blocked") && 0 != name.CompareTo("DFM/NotBlocked"))
                    {
                        needChange = true;
                    }
                }
            }

            if (needChange)
            {
                byte rb = (byte)((rimColor & 0xFF000000) >> 24);
                byte gb = (byte)((rimColor & 0x00FF0000) >> 16);
                byte bb = (byte)((rimColor & 0x0000FF00) >> 8);
                byte ab = (byte)(rimColor & 0x000000FF);
                float r = (float)rb / 255.0f;
                float g = (float)gb / 255.0f;
                float b = (float)bb / 255.0f;
                float a = (float)ab / 255.0f;
                Color c = new Color(r, g, b, a);

                Shader blocked = Shader.Find("DFM/Blocked");
                Shader notBlocked = Shader.Find("DFM/NotBlocked");
                if (null == blocked)
                {
                    CallLogicLog("id={0} obj can't find shader DFM/Blocked !", id);
                    return;
                }
                if (null == notBlocked)
                {
                    CallLogicLog("id={0} obj can't find shader DFM/NotBlocked !", id);
                    return;
                }

                foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
                {
                    objInfo.ObjectInfo.m_SkinedMaterialChanged = true;
                    Texture texture = renderer.material.mainTexture;
                    Material blockedMat = new Material(blocked);
                    Material notBlockedMat = new Material(notBlocked);
                    Material[] mats = new Material[]{notBlockedMat,blockedMat};
                    blockedMat.SetColor("_RimColor", c);
                    blockedMat.SetFloat("_RimPower", rimPower);
                    blockedMat.SetFloat("_CutValue", cutValue);
                    notBlockedMat.SetTexture("_MainTex", texture);

                    renderer.materials = mats;
                }
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    objInfo.ObjectInfo.m_MeshMaterialChanged = true;
                    Texture texture = renderer.material.mainTexture;

                    Material blockedMat = new Material(blocked);
                    Material notBlockedMat = new Material(notBlocked);
                    Material[] mats = new Material[]{notBlockedMat,blockedMat};
                    blockedMat.SetColor("_RimColor", c);
                    blockedMat.SetFloat("_RimPower", rimPower);
                    blockedMat.SetFloat("_CutValue", cutValue);
                    notBlockedMat.SetTexture("_MainTex", texture);

                    renderer.materials = mats;
                }
            }
        }

        //播放动画
        private void CrossFadeAnimationImpl(int id, string animationName, float fadeLength, bool isStopAll)
        {
            GameObject obj = GetGameObject(id);
            SharedGameObjectInfo objInfo = GetSharedGameObjectInfo(id);
            Animation animation = obj.GetComponent<Animation>();
            if (null != obj && null != animation)
            {
                if (null != animation[animationName] && null != objInfo && !objInfo.IsGfxAnimation)
                {
                    /**动画层 解释StopAll和StopSameLayer
                       是非常有用的概念，它允许群组动画的存在和设置动画的优先级别。例如，有一个射击动画，一个空闲和行走循环动画，想要使行走和空闲动画基于玩家的速度连续过渡，但当玩家射击时，仅显示射击动画，因此，射击动画需要有更高的优先级。
                       要做到这点最简单的方法是在射击时简单地保持行走和空闲动画，然后我们需要确保射击动画比空闲和行走动画在更高的层。设置层使用"animation["动画名"].layer"属性，值越大层越高，优先级就越高。下面的代码片段说明了层的使用，做到了前面例子所需要的效果：
                      */
                    if (objInfo.IsPlayer)
                    {
                        Debug.Log("CrossFadeAnimationImpl_" + animationName);
                    }
                    animation.CrossFade(animationName, fadeLength, isStopAll ? PlayMode.StopAll : PlayMode.StopSameLayer);
                    //bool isPlaying = animation.IsPlaying(animationName);
                    //if (!isPlaying || animation[animationName].wrapMode != WrapMode.Loop)
                    //{
                    //    //PlayMode.StopSameLayer所有在同一个层的动画将停止播放。如果模式是PlayMode.StopAll，那么所有当前在播放的动画将停止播放
                    //    animation.CrossFade(animationName, fadeLength, isStopAll ? PlayMode.StopAll : PlayMode.StopSameLayer);
                    //    UnityEngine.Debug.LogFormat("CrossFadeAnimationImpl_{0}_{1}_{2}", animationName, animation.clip.name, isPlaying);
                    //}
                }else
                {
                    if (null == animation[animationName])
                    {
                        CallLogicErrorLog("Obj {0} CrossFadeAnimation {1} AnimationState is null, clipcount {2}", id, animationName, animation.GetClipCount());
                    }
                    if (null == objInfo)
                    {
                        CallLogicErrorLog("Obj {0} CrossFadeAnimation {1} obj_info is null, obj name {2}", id, animationName, obj.name);
                    }
                }
            }
        }

        //设置UserView
        private void SetGameObjectVisibleImpl(int id, bool visible)
        {
            GameObject obj = GetGameObject(id);
            if (null != obj)
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; ++i)
                {
                    renderers[i].enabled = visible;
                }
            }
        }

        private void CreateGameObjectImpl(int id, string resource, SharedGameObjectInfo info)
        {
            if(null != info)
            {
                try
                {
                    Vector3 pos = new Vector3(info.X, info.Y, info.Z);
                    if (!info.IsFloat)
                        pos.y = SampleTerrainHeight(pos.x, pos.z);
                    Quaternion q = Quaternion.Euler(0, RadianToDegree(info.FaceDir), 0);
                    GameObject obj = ResourceManager.Instance.NewObject(resource) as GameObject;
                    if(null != obj)
                    {
                        if (null != obj.transform)
                        {
                            obj.transform.position = pos;
                            obj.transform.localRotation = q;
                            obj.transform.localScale = new Vector3(info.Sx, info.Sy, info.Sz);
                        }
                        RememberGameObject(id, obj, info);
                        obj.SetActive(true);
                    }
                    else
                    {
                        CallLogicErrorLog("CreateGameObject {0} can't load resource", resource);
                    }
                }
                catch (Exception ex)
                {
                    CallGfxErrorLog("CreateGameObject {0} throw exception:{1}\n{2}", resource, ex.Message, ex.StackTrace);
                }
            }
        }

        private void CreateGameObjectImpl(int id, string resource, float x, float y, float z, float rx, float ry, float rz, bool attachTerrain)
        {
            try
            {
                if (attachTerrain)
                {
                    y = SampleTerrainHeight(x, z);
                }
                Vector3 pos = new Vector3(x, z);
                Quaternion q = Quaternion.Euler(RadianToDegree(rx), RadianToDegree(ry), RadianToDegree(rz));
                GameObject obj = ResourceManager.Instance.NewObject(resource) as GameObject;
                if (null != obj)
                {
                    obj.transform.position = pos;
                    obj.transform.localRotation = q;
                    RememberGameObject(id, obj);
                    obj.SetActive(true);
                }
                else
                {
                    CallLogicErrorLog("CreateGameObject {0} can't load resource", resource);
                }
            }
            catch (Exception ex)
            {
                CallGfxErrorLog("CreateGameObject {0} throw exception:{1}\n{2}", resource, ex.Message, ex.StackTrace);
            }
        }

        private void CreateAndAttachGameObjectImpl(string resource, int parentId, string path, float recycleTime)
        {
            try
            {
                GameObject obj = ResourceManager.Instance.NewObject(resource, recycleTime) as GameObject;
                GameObject parent = GetGameObject(parentId);
                if(null != obj)
                {
                    obj.SetActive(true);
                    if(null != obj.transform && null != parent && null != parent.transform)
                    {
                        Transform t = parent.transform;
                        if(!String.IsNullOrEmpty(path))
                        {
                            t = FindChildRecursive(parent.transform, path);
                        }
                        if (null != t)
                        {
                            obj.transform.parent = t;
                            obj.transform.localPosition = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            CallLogicErrorLog("Obj {0} CreateAndAttachGameObject {1} can't find bone {2}", resource, parentId, path);
                        }
                    }
                }
                else
                {
                    CallLogicErrorLog("CreateAndAttachGameObject {0} can't load resource", resource);
                }
            }
            catch (Exception ex)
            {
                CallGfxErrorLog("CreateAndAttachGameObject {0} throw exception:{1}\n{2}", resource, ex.Message, ex.StackTrace);
            }
        }

        //递归查找子节点
        internal Transform FindChildRecursive(Transform parent, string bonePath)
        {
            Transform t = parent.Find(bonePath);
            if(null != t)
            {
                return t;
            }
            else
            {
                int ct = parent.childCount;
                for(int i = 0; i < ct; ++i)
                {
                    t = FindChildRecursive(parent.GetChild(i), bonePath);
                    if(null != t)
                    {
                        return t;
                    }
                }
            }
            return null;
        }

        private void RememberGameObject(int id, GameObject obj)
        {
            RememberGameObject(id, obj, null);
        }

        private void RememberGameObject(int id, GameObject obj, SharedGameObjectInfo info)
        {
            if (m_GameObjects.Contains(id))
            {
                GameObject oldObj = m_GameObjects[id].ObjectInstance;
                oldObj.SetActive(false);
                m_GameObjectIds.Remove(oldObj);
                GameObject.Destroy(oldObj);
                m_GameObjects[id] = new GameObjectInfo(obj, info);
            }
            else
            {
                m_GameObjects.AddLast(id, new GameObjectInfo(obj, info));
            }
            if (null != info)
            {
                if (!info.m_SkinedMaterialChanged)
                {
                    SkinnedMeshRenderer[] renderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer renderer in renderers)
                    {
                        info.m_SkinedOriginalMaterials.Add(renderer.materials);
                    }
                }
                if (!info.m_MeshMaterialChanged)
                {
                    MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer renderer in renderers)
                    {
                        info.m_MeshOriginalMaterials.Add(renderer.materials);
                    }
                }
            }
            m_GameObjectIds.Add(obj, id);
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

        private void MarkPlayerSelfImpl(int id)
        {
            GameObjectInfo info = GetGameObjectInfo(id);
            if(null != info)
            {
                m_PlayerSelf = info;
                if(null != info.ObjectInstance)
                {
                    int layer = LayerMask.NameToLayer("Character");
                    if(layer >= 0)
                    {
                        info.ObjectInstance.layer = layer;
                    }
                }
            }
        }

        private void SetTimeScaleImpl(float scale)
        {
            Time.timeScale = scale;
        }

        //Gfx线程执行的函数，对游戏逻辑线程的异步调用由这里发起
        internal float SampleTerrainHeight(float x, float z)
        {
            float y = c_MinTerrainHeight;
            if(null != Terrain.activeTerrain)
            {
                y = Terrain.activeTerrain.SampleHeight(new Vector3(x, c_MinTerrainHeight, z));
            }
            else
            {
                //origin 在世界坐标，射线的起始点 direction 射线的方向 distance 射线的长度 layerMask只选定Layermask层内的碰撞器
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, c_MinTerrainHeight * 2, z), Vector3.down, out hit, c_MinTerrainHeight * 2, 1 << LayerMask.NameToLayer("Terrains")))
                {
                    y = hit.point.y;//point 在世界空间中，射线碰到碰撞器的碰撞点
                }
            }
            return y;
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

        internal void CallGfxErrorLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            GfxErrorLogImpl(msg);
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

        internal GameObject PlayerSelf
        {
            get
            {
                if (null != m_PlayerSelf)
                    return m_PlayerSelf.ObjectInstance;
                else
                    return null;
            }
        }

        internal SharedGameObjectInfo PlayerSelfInfo
        {
            get
            {
                if (null != m_PlayerSelf)
                    return m_PlayerSelf.ObjectInfo;
                else
                    return null;
            }
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

        internal bool ExistGameObject(GameObject obj)
        {
            int id = GetGameObjectId(obj);
            return id > 0;
        }

        internal IGameLogicNotification GameLogicNotification
        {
            get { return m_GameLogicNotification; }
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


        //GfxSystemImpl_Touch
        #region GfxSystemImpl_Touch

        public delegate void FingerStatus(GestureArgs e);
        public static FingerStatus OnFingerDown;
        public static FingerStatus OnFingerUp;

        private MyDictionary<int, MyAction<int, GestureArgs>> m_TouchHandlers = new MyDictionary<int, MyAction<int, GestureArgs>>();

        // Joystick
        private float m_CurJoyDir;
        private Vector3 m_CurJoyTargetPos;

        private void ListenTouchEventImpl(TouchEvent c, MyAction<int, GestureArgs> handler)
        {
            if (m_TouchHandlers.ContainsKey((int)c))
            {
                m_TouchHandlers[(int)c] = handler;
            }
            else
            {
                m_TouchHandlers.Add((int)c, handler);
            }
        }

        internal void SetJoystickInfoImpl(GestureArgs e)
        {
            if (null != e)
            {
                m_CurJoyDir = e.towards;
                m_CurJoyTargetPos.x = e.airWelGamePosX;
                m_CurJoyTargetPos.y = e.airWelGamePosY;
                m_CurJoyTargetPos.z = e.airWelGamePosZ;
            }
        }

        private float GetJoystickDirImpl()
        {
            return m_CurJoyDir;
        }

        private float GetJoystickTargetPosXImpl()
        {
            return m_CurJoyTargetPos.x;
        }

        private float GetJoystickTargetPosYImpl()
        {
            return m_CurJoyTargetPos.y;
        }
        private float GetJoystickTargetPosZImpl()
        {
            return m_CurJoyTargetPos.z;
        }

        #endregion
    }
}
