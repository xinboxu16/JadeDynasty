using UnityEngine;
using System;
using System.Collections;

using DashFire;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using StoryDlg;
using System.IO;

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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
#if UNITY_ANDROID || UNITY_IPHONE
        Debug.logger.logEnabled = false;
#endif

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
		if (Application.isEditor)
		{
			GameControler.Init(tempPath, streamingAssetsPath);
		}
		else
		{
			GameControler.Init(tempPath, persistentDataPath);
		}
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
        Debug.Log("BeginLoading");
        LogicSystem.BeginLoading();
        if (GlobalVariables.Instance.IsPublish)
        {
            //未实现
            AssetExManager.Instance.Cleanup();
            yield break;
        }
        else if (!Application.isEditor)
        {
            Debug.Log("ExtractDataFileAndStartGame");
			// 加载txt资源
			LogicSystem.UpdateLoadingTip ("加载配置数据");
			string srcPath = Application.streamingAssetsPath;
			string destPath = Application.persistentDataPath + "/DataFile";
			Debug.Log (srcPath);
			Debug.Log (destPath);
			if (!srcPath.Contains ("://"))
				srcPath = "file://" + srcPath;
			string listPath = srcPath + "/list.txt";
            if (!Directory.Exists(destPath))
            {
                WWW listData = new WWW(listPath);
                //Debug.Log("wait for www " + listPath + " done");
                yield return listData;
                //Debug.Log("www " + listPath + " is done");
                string listTxt = listData.text;
                if (null != listTxt)
                {
                    //Debug.Log(listTxt);
                    using (StringReader sr = new StringReader(listTxt))
                    {
                        string numStr = sr.ReadLine();
                        float totalNum = 50;
                        if (null != numStr)
                        {
                            numStr = numStr.Trim();
                            totalNum = (float)int.Parse(numStr);
                            if (totalNum <= 0)
                                totalNum = 50;
                        }
                        for (float num = 1; ; num += 1)
                        {
                            string path = sr.ReadLine();
                            if (null != path)
                            {
                                path = path.Trim();
                                string url = srcPath + "/" + path;
                                //Debug.Log("extract " + url);
                                string filePath = Path.Combine(destPath, path);
                                string dir = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);
                                WWW temp = new WWW(url);
                                yield return temp;
                                if (null != temp.bytes)
                                {
                                    File.WriteAllBytes(filePath, temp.bytes);
                                }
                                else
                                {
                                    Debug.Log(path + " can't load");
                                }
                                temp = null;
                            }
                            else
                            {
                                break;
                            }

                            LogicSystem.UpdateLoadingProgress(0.8f + 0.2f * num / totalNum);
                        }
                        sr.Close();
                    }
                    listData = null;
                }
                else
                {
                    Debug.Log("Can't load list.txt");
                }
            }
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

    internal void RestartLocgic()
    {
        LogicSystem.SetLoadingBarScene("LoadingBar");
        //Application.LoadLevel("Loading");
        LogicSystem.PublishLogicEvent("ge_change_scene", "game", 0);
        m_IsInit = true;
    }

    public void ShowUi(bool show)
    {
        UIManager.Instance.SetAllUiVisible(show);
    }

    public void TriggerStory(int storyId)
    {
        StoryDlgInfo storyInfo = StoryDlgManager.Instance.GetStoryInfoByID(storyId);
        if (null != storyInfo)
        {
            if(storyInfo.DlgType == StoryDlgType.Small)
            {
                GameObject obj = UIManager.Instance.GetWindowGoByName("StoryDlgSmall");
                if(null != obj)
                {
                    StoryDlgPanel dlg = obj.GetComponent<StoryDlgPanel>();
                    dlg.OnTriggerStory(storyInfo);
                }
            }
            else
            {
                GameObject obj = UIManager.Instance.GetWindowGoByName("StoryDlgBig");
                if(null != obj)
                {
                    StoryDlgPanel dlg = obj.GetComponent<StoryDlgPanel>();
                    dlg.OnTriggerStory(storyInfo);
                }
            }
        }
        else
        {
            Debug.LogError("Wrong Story id = " + storyId);
        }
    }
}
