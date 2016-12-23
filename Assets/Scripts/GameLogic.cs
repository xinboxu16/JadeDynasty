using UnityEngine;
using System;
using System.Collections;

using DashFire;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    private bool m_IsDataFileExtracted = false;
    private bool m_IsDataFileExtractedPaused = false;
    private bool m_IsInit = false;
    private bool m_IsSettingModified = false;
    //AsyncOperation类---异步操作
    private AsyncOperation m_LoadLevelAsync = null; 

    internal void Awake()
    {
        GlobalVariables.Instance.IsClient = true;
        DontDestroyOnLoad(this.gameObject);
    }

    internal void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;//是否垂直同步
        QualitySettings.SetQualityLevel(1);//设置质量
        Application.runInBackground = true;//程序是否后台运行

        try
        {
            if (!GameControler.IsInited)
            {
                //AnalyticsManager.Init();//友盟数据统计

                HardWareQuality.Clear();
                HardWareQuality.ComputeHardwarePerformance();
                //HardWareQuality.SetResolution();
                HardWareQuality.SetQualityAll();

                Debug.Log("GlobalVariables" + GlobalVariables.Instance.IsPublish);
                //发布时更新资源
                if (GlobalVariables.Instance.IsPublish)
                {
                    ResUpdateControler.InitContext();
                }

                //Application.dataPath： 只可读不可写，放置一些资源数据
                //Application.persistentDataPath 可读可写，可以放一些存档文件
                string dataPath = Application.dataPath;
                string persistentDataPath = Application.persistentDataPath + "/DataFile";
                string streamingAssetsPath = Application.streamingAssetsPath;
                string tempPath = Application.temporaryCachePath;
                LogicSystem.LogicLog("dataPath:{0} persistentDataPath:{1} streamingAssetsPath:{2} tempPath:{3}", dataPath, Application.persistentDataPath, streamingAssetsPath, tempPath);
                Debug.Log(string.Format("dataPath:{0} persistentDataPath:{1} streamingAssetsPath:{2} tempPath:{3}", dataPath, Application.persistentDataPath, streamingAssetsPath, tempPath));

                if (GlobalVariables.Instance.IsPublish)
                {
                    GameControler.Init(tempPath, persistentDataPath);
                }else{
#if UNITY_ANDROID
	      GameControler.Init(tempPath, persistentDataPath);
#elif UNITY_IPHONE
	      GameControler.Init(tempPath, persistentDataPath);
#else
        if (Application.isEditor)
            GameControler.Init(tempPath, streamingAssetsPath);
        else
            GameControler.Init(dataPath, persistentDataPath);
#endif

                }
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("GameLogic.Start throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            Debug.Log(string.Format("GameLogic.Start throw exception:{0}\n{1}", ex.Message, ex.StackTrace));
        }
    }

    // Update is called once per frame
    internal void Update()
    {
        try
        {
            if (!m_IsDataFileExtracted && !m_IsDataFileExtractedPaused)
            {
                StartCoroutine(ExtractDataFileAndStartGame());
                m_IsDataFileExtracted = true;
            }
            //异步加载完成
            if(!m_IsInit && m_LoadLevelAsync != null && m_LoadLevelAsync.isDone)
            {
                m_LoadLevelAsync = null;
                m_IsInit = true;
                //清除所有资源
                AssetExManager.Instance.ClearAllAssetBundle();
            }

            if(!m_IsSettingModified)
            {
                QualitySettings.vSyncCount = 1;
                if (QualitySettings.vSyncCount == 1)
                {
                    m_IsSettingModified = true;
                }
            }

            if(m_IsInit)
            {
//                bool isLastHitUi = false;
//                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
//                {
//#if IPHONE || ANDROID
//            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
//#else
//                    if (EventSystem.current.IsPointerOverGameObject())
//#endif
//                        isLastHitUi = true;
//                }

//                LogicSystem.IsLastHitUi = isLastHitUi;
                GameControler.TickGame();
            }
            AssetExManager.Instance.Update();
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("GameLogic.Update throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            Debug.Log(string.Format("GameLogic.Update throw exception:{0}\n{1}", ex.Message, ex.StackTrace));
        }
    }

    internal void OnApplicationPause(bool isPause)
    {
        Debug.LogWarning("OnApplicationPause:" + isPause);
        GameControler.PauseLogic(isPause);
    }

    internal void OnApplicationQuit()
    {
        Debug.LogWarning("OnApplicationQuit");
        GameControler.StopLogic();
        GameControler.Release();
        AssetExManager.Instance.ClearAllAssetEx();
        Resources.UnloadUnusedAssets();
    }

    private IEnumerator ExtractDataFileAndStartGame()
    {
        LogicSystem.BeginLoading();
        if (GlobalVariables.Instance.IsPublish)
        {
            //未实现
            AssetExManager.Instance.Cleanup();
            yield break;
        }
        else if (!Application.isEditor)
        {
            //未实现
            yield break;
        }

        LogicSystem.EndLoading();
        StartLogic();
    }

    private void StartLogic()
    {
        GameControler.InitLogic();
        GameControler.StartLogic();
        LogicSystem.SetLoadingBarScene("LoadingBar");
        if (GlobalVariables.Instance.IsPublish)
        {
            Resources.UnloadUnusedAssets();
            m_LoadLevelAsync = SceneManager.LoadSceneAsync("loading");
            m_IsInit = false;
        }
        else
        {
            SceneManager.LoadScene("Loading");
            m_IsInit = true;
        }

        DashFire.LogicSystem.EventChannelForGfx.Publish("ge_show_login", "ui");
    }
}
