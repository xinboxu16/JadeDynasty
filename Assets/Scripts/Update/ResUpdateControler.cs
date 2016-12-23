using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DashFire
{
    public class ResUpdateControler
    {
        public static bool s_EnableResServer = true;
        public static bool s_EnableResServerSkip = true;
        public static bool s_UseNewWWW = false;
        public static bool s_UseDynmaicUrl = true;

        public static long s_CachingSpaceMin = 3 * 1024 * 1024;
        public static float s_ProgressUpgrade = 0.2f;
        public static float s_ProgressCacheGlobal = 0.2f;
        public static float s_ProgressCacheMainMenu = 0.2f;
        public static string[] s_ConfigSplit = new string[] { ";", "|", };
        public static int s_AsyncCoroutineMax = 2; //HardWareQuality中配置
        //public static int s_AsyncExtractCoroutineMax = 10; //HardWareQuality中配置
        public static int s_LoadSceneId = 0; //配置在SceneConfig.txt中

#if UNITY_ANDROID
    public static string s_ForceDownloadUrl = "http://mrd.changyou.com/DF/dfm_{0}.apk";
#elif UNITY_IPHONE
    public static string s_ForceDownloadUrl = "itms-services://?action=download-manifest&amp;url=https://git.oschina.net/johnleencist/dashfire/raw/master/dfm_{0}.plist";
#else
        public static string s_ForceDownloadUrl = "http://10.1.8.69:456/latest/download.html";
#endif

        public static string s_VersionInfoFormat = "版本号:{0}.{1}.{2} ({3}.{4})";
        public static int s_ReconnectSkipNumMax = 3;

        #region  Define
        public static string s_ClientVersionFile = "ClientVersion.txt";
        public static string s_ServerVersionFile = "ServerVersion.txt";
        public static string s_ResVersionFile = "ResVersion.txt";
        public static string s_ResVersionZip = "ResVersion.ab";
        public static string s_AssetDBFile = "AssetDB.txt";
        public static string s_AssetDBZip = "AssetDB.ab";
        public static string s_ResCacheFile = "ResCache.txt";
        public static string s_ResCacheZip = "ResCache.ab";
        public static string s_DynamicLevel = "Empty";
        public static string s_ResBuildInPath = "ResFile/";
        public static string s_ResCachePath = "ResFile/";

        public static string s_ResVersionClientFile = "ClientResVersion.txt";
        public static string s_ResVersionClientFormat = "{0}	{1}	{2}";
        public static string s_ResVersionClientHeader = "Name	MD5	IsBuildIn";

        public static string s_ResSheetFile = "list.txt";
        public static string s_ResSheetZip = "ResSheet.ab";
        public static string s_ResSheetPattern = ".txt;.map;.dsl";
        public static string s_ResSheetCachePath = "DataFile/";

        #endregion

        #region Config
        public static string ClientVersion
        {
            get { return ClientVersionInfo.Version.GetVersionStr(); }
            set
            {
                ClientVersionInfo.Version = new VersionNum(value);
                ClientVersionInfo.Save();
            }
        }
        public static string Platform
        {
            get { return ClientVersionInfo.Platform; }
            set { ClientVersionInfo.Platform = value; ClientVersionInfo.Save(); }
        }
        public static string AppName
        {
            get { return ClientVersionInfo.AppName; }
            set { ClientVersionInfo.AppName = value; ClientVersionInfo.Save(); }
        }
        public static string Channel
        {
            get { return ClientVersionInfo.Channel; }
            set { ClientVersionInfo.Channel = value; ClientVersionInfo.Save(); }
        }
        public static string ResServerURL
        {
            get { return ClientVersionInfo.ResServerURL; }
            set { ClientVersionInfo.ResServerURL = value; ClientVersionInfo.Save(); }
        }
        public static int CurChapter
        {
            get { return ClientVersionInfo.CurChapter; }
            set { ClientVersionInfo.CurChapter = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResVersionConfigCached
        {
            get { return ClientVersionInfo.IsResVersionConfigCached; }
            set { ClientVersionInfo.IsResVersionConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsAssetDBConfigCached
        {
            get { return ClientVersionInfo.IsAssetDBConfigCached; }
            set { ClientVersionInfo.IsAssetDBConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResCacheConfigCached
        {
            get { return ClientVersionInfo.IsResCacheConfigCached; }
            set { ClientVersionInfo.IsResCacheConfigCached = value; ClientVersionInfo.Save(); }
        }
        public static bool IsResSheetConfigCached
        {
            get { return ClientVersionInfo.IsResSheetConfigCached; }
            set { ClientVersionInfo.IsResSheetConfigCached = value; ClientVersionInfo.Save(); }
        }
        #endregion

        public static VersionInfo BuildinClientVersionInfo;
        public static VersionInfo PersistClientVersionInfo;
        public static VersionInfo ClientVersionInfo;
        public static VersionInfo ServerVersionInfo;
        public static bool IsNeedUpdate;
        public static bool IsNeedPauseUpdate;
        public static bool IsNeedLoadConfig;
        public static int DownLoadNum;
        public static float ProgressStart;
        public static float ProgressMax;
        public static int ProgressBarType;
        public static int TargetChapter;
        public static bool IsNeedSyncPackage;
        public static int ReconnectNum;

        public delegate void EventUpdateFailed();
        public static EventUpdateFailed HandleUpdateFailed;

        public static bool InitContext()
        {
            ResUpdateHandler.HandleStartUpdate = StartUpdate;
            ResUpdateHandler.HandleStartUpdateChapter = StartUpdateChapter;
            ResUpdateHandler.HandleExitUpdate = ExitUpdate;
            ResUpdateHandler.HandleSetUpdateProgressRange = SetUpdateProgressRange;
            ResUpdateHandler.HandleCacheResByConfig = CacheResByConfig;

            ResUpdateHandler.HandleGetEnableResServerSkip = GetEnableResServerSkip;
            ResUpdateHandler.HandleGetReconnectSkipNumMax = GetReconnectSkipNumMax;
            ResUpdateHandler.HandleGetReconnectNum = GetReconnectNum;
            ResUpdateHandler.HandleIncReconnectNum = IncReconnectNum;
            ResUpdateHandler.HandleResetReconnectNum = ResetReconnectNum;
            ResUpdateHandler.HandleCacheResByConfig = CacheResByConfig;

            ResUpdateHandler.HandleLoadAssetFromABWithoutExtention = AssetExManager.Instance.GetAssetByNameWithoutExtention;
            ResUpdateHandler.HandleReleaseAllAssetBundle = AssetExManager.Instance.ClearAllAssetBundle;
            ResUpdateHandler.HandleCleanup = AssetExManager.Instance.Cleanup;

            IsNeedLoadConfig = true;

            ClientResVersionProvider.Instance.Clear();
            return true;
        }

        public static ResAsyncInfo StartUpdate(int targetChapter)
        {
            TargetChapter = targetChapter;
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.StartUpdate(info));
            return info;
        }

        public static ResAsyncInfo StartUpdateChapter(int targetChapter)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            if (targetChapter > ResUpdateControler.CurChapter)
            {
                TargetChapter = targetChapter;
                info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.StartUpdate(info));
            }
            else
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                info.CurCoroutine = null;
            }
            return info;
        }

        public static void InitUpdate()
        {
            IsNeedUpdate = false;
            IsNeedPauseUpdate = false;
            DownLoadNum = 0;
            ProgressStart = 0;
            ProgressMax = 0;
            ProgressBarType = 0;
            HandleUpdateFailed = null;
        }

        public static void ExitUpdate()
        {
            IsNeedUpdate = false;
            DownLoadNum = 0;
            ProgressStart = 0;
            ProgressMax = 0;
            ProgressBarType = 0;
            HandleUpdateFailed = null;
            TargetChapter = 0;
        }

        public static void SetUpdateProgressRange(float start, float max, int progressBarType = 0)
        {
            ProgressStart = start;
            ProgressMax = max;
            ProgressBarType = progressBarType;
        }

        public static bool GetEnableResServerSkip()
        {
            return s_EnableResServerSkip;
        }

        public static int GetReconnectSkipNumMax()
        {
            return s_ReconnectSkipNumMax;
        }

        public static int GetReconnectNum()
        {
            return ReconnectNum;
        }

        public static void IncReconnectNum()
        {
            ReconnectNum++;
        }
        public static void ResetReconnectNum()
        {
            ReconnectNum = 0;
        }
        internal static ResAsyncInfo DetectVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.DetectVersion(info));
            return info;
        }
        internal static ResAsyncInfo RequestConfig()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.RequestConfig(info));
            return info;
        }
        internal static ResAsyncInfo LoadConfigAsync()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.LoadConfigAsync(info));
            return info;
        }
        internal static ResAsyncInfo StartDownload()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResUpdateControlerHandler.StartDownload(info));
            return info;
        }
        internal static ResAsyncInfo CacheResByConfig(List<ResCacheConfig> cacheConfigList)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(
              ResUpdateControlerHandler.CacheResByConfig(info, cacheConfigList));
            return info;
        }
        internal static void OnUpdateFailed()
        {
            if (HandleUpdateFailed != null)
            {
                HandleUpdateFailed();
            }
        }
        internal static void OnUpdateProgress(float progress, string tip = null)
        {
            if (ProgressBarType == 1)
            {
                if (tip != null)
                {
                    LogicSystem.UpdateLoadingTip(tip);
                }
                LogicSystem.UpdateLoadingProgress(ProgressStart + ProgressMax * progress);
            }
            else
            {
                //TODO:用于在不同场合中Loading条的不同表现。
            }
        }
        internal static void OnUpdateVersionInfo()
        {
            if (ClientVersionInfo == null || ClientVersionInfo.Version == null)
            {
                return;
            }
            VersionNum cVersionNum = ClientVersionInfo.Version;
            string versionInfo = string.Format(s_VersionInfoFormat,
                  cVersionNum.AppChannel, cVersionNum.AppMaster, cVersionNum.ResBase, CurChapter, (double)(CodeVersion.Value));
            LogicSystem.UpdateVersinoInfo(versionInfo);
            ResLoadHelper.Log("OnUpdateVersionInfo:" + versionInfo);
        }
        internal static void OnUpdateCompleted(bool isdownload)
        {
            if (isdownload)
            {
                ResUpdateControler.ClientVersion = ResUpdateControler.ServerVersionInfo.Version.GetVersionStr();
                ResUpdateControler.CurChapter = ResUpdateControler.TargetChapter;
                if (ResUpdateControler.DownLoadNum > 0)
                {
                    ResLoader.SaveClientResVersion();
                    ResUpdateControler.DownLoadNum = 0;
                }
                ResUpdateControler.OnUpdateVersionInfo();
            }
            ReconnectNum = 0;
            IsNeedSyncPackage = false;
        }
        internal static void OnForceDownload(bool selected)
        {
            if (selected)
            {
#if UNITY_IPHONE
        string downloadUrl = string.Format(s_ForceDownloadUrl, Channel) + "&amp;time=" + DateTime.Now.ToString("yyyyMMddhhmmss");
#elif UNITY_ANDROID
        string downloadUrl = string.Format(s_ForceDownloadUrl, Channel)
#else
                string downloadUrl = s_ForceDownloadUrl;
#endif
                ResLoadHelper.Log("OnForceDownload:" + downloadUrl);
                Application.OpenURL(downloadUrl);
                Application.Quit();
            }
        }
    }

    /// <summary>
    /// Unity 4.2.2不支持在dll的类中混合IEnumerator和普通函数, 
    /// 因此，将包含yield的函数分离开来，by lixiaojiang
    /// Bug:http://lists.ximian.com/pipermail/mono-bugs/2011-July/112237.html
    /// </summary>
    internal class ResUpdateControlerHandler
    {
        internal static IEnumerator StartUpdate(ResAsyncInfo info)
        {
            if (ResUpdateControler.s_EnableResServerSkip
              && ResUpdateControler.GetReconnectNum() >= ResUpdateControler.GetReconnectSkipNumMax())
            {
                ResLoadHelper.Log("无法连接资源服务器，直接跳过资源更新！");
                ResUpdateControler.IsNeedUpdate = false;
            }
            else if (ResUpdateControler.s_EnableResServer)
            {
                ResAsyncInfo detectVersionInfo = ResUpdateControler.DetectVersion();
                yield return detectVersionInfo.CurCoroutine;
                if (detectVersionInfo.IsError)
                {
                    ResLoadHelper.Log("检测版本信息错误");
                    info.IsError = true;
                    yield break;
                }
            }
            else
            {
                ResLoadHelper.Log("关闭资源服务器，要求资源都置于本地！");
                ResUpdateControler.IsNeedUpdate = false;
            }

            if (ResUpdateControler.IsNeedPauseUpdate)
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                yield break;
            }

            if (ResUpdateControler.IsNeedUpdate)
            {
                ResUpdateControler.OnUpdateProgress(0, "开始下载资源");

                ResUpdateControler.IsNeedLoadConfig = false;
                ResAsyncInfo loadClientResVersionInfo = ResLoader.LoadClientResVersion();
                yield return loadClientResVersionInfo.CurCoroutine;
                if (loadClientResVersionInfo.IsError)
                {
                    ResLoadHelper.Log("加载本地资源列表错误");
                    info.IsError = true;
                    yield break;
                }

                ResAsyncInfo requestConfigInfo = ResUpdateControler.RequestConfig();
                yield return requestConfigInfo.CurCoroutine;
                if (requestConfigInfo.IsError)
                {
                    ResLoadHelper.Log("请求版本信息错误");
                    info.IsError = true;
                    yield break;
                }

                ResAsyncInfo startDownloadInfo = ResUpdateControler.StartDownload();
                yield return startDownloadInfo.CurCoroutine;
                if (startDownloadInfo.IsError)
                {
                    ResLoadHelper.Log("更新资源错误");
                    info.IsError = true;
                    yield break;
                }
                ResUpdateControler.OnUpdateCompleted(true);
            }
            else if (ResUpdateControler.IsNeedLoadConfig)
            {
                ResUpdateControler.IsNeedLoadConfig = false;
                ResAsyncInfo loadClientResVersionInfo = ResLoader.LoadClientResVersion();
                yield return loadClientResVersionInfo.CurCoroutine;
                if (loadClientResVersionInfo.IsError)
                {
                    ResLoadHelper.Log("加载本地资源列表错误");
                    info.IsError = true;
                    yield break;
                }

                ResAsyncInfo loadConfigInfo = ResUpdateControler.LoadConfigAsync();
                yield return loadConfigInfo.CurCoroutine;
                if (loadConfigInfo.IsError)
                {
                    ResLoadHelper.Log("加载版本信息错误");
                    info.IsError = true;
                    yield break;
                }
                ResUpdateControler.OnUpdateCompleted(false);
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator DetectVersion(ResAsyncInfo info)
        {
            //ResUpdateControler.OnUpdateProgress(0, "加载服务器版本...");
            ResAsyncInfo requestServerVersionInfo = ResUpgrader.RequestServerVersion();
            yield return requestServerVersionInfo.CurCoroutine;
            if (requestServerVersionInfo.IsError)
            {
                ResLoadHelper.Log("加载服务器版本错误");
                info.IsError = true;
                yield break;
            }

            VersionNum serverVersionNum = ResUpdateControler.ServerVersionInfo.Version;
            VersionNum clientVersionNum = ResUpdateControler.ClientVersionInfo.Version;
            ResUpdateControler.IsNeedPauseUpdate = false;

            ResLoadHelper.Log(string.Format("ClientVersion:{0} ServerVersion:{1}",
              clientVersionNum.GetVersionStr(), serverVersionNum.GetVersionStr()));

            ResUpdateControler.IsNeedUpdate = false;
            if (clientVersionNum.GetAppVersionForceValue() < serverVersionNum.GetAppVersionForceValue())
            {
                ResUpdateControler.IsNeedPauseUpdate = true;

                string tipInfo = string.Format("有新版本可更新，版本号：" + serverVersionNum.GetVersionStr());
                ResLoadHelper.Log(tipInfo);

                Action<bool> fun = ResUpdateControler.OnForceDownload;
                string dlgTip = "有新版本可更新";
                DashFire.LogicSystem.EventChannelForGfx.Publish("ge_show_yesornot", "ui", dlgTip, fun);
                //Application.Quit();
            }
            else if (ResUpdateControler.CurChapter < ResUpdateControler.TargetChapter)
            {
                ResUpdateControler.IsNeedUpdate = true;
            }
            else
            {
                if (clientVersionNum.GetResVersionValue() < serverVersionNum.GetResVersionValue())
                {
                    //ResUpdateControler.OnUpdateProgress(0, );
                    ResLoadHelper.Log("检测到有资源需要更新:" + serverVersionNum.GetVersionStr());
                    ResUpdateControler.IsNeedUpdate = true;
                }
                else
                {
                    ResLoadHelper.Log("已经是最新版本:" + serverVersionNum.GetVersionStr());
                    //ResUpdateControler.OnUpdateProgress(0, "已经是最新版本:" + serverVersionNum.GetVersionStr());
                }
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator RequestConfig(ResAsyncInfo info)
        {
            //ResUpdateControler.OnUpdateProgress(0, "请求版本资源...");
            ResAsyncInfo requestResVersionInfo = ResUpgrader.RequestResVersion();
            yield return requestResVersionInfo.CurCoroutine;
            if (requestResVersionInfo.IsError)
            {
                ResLoadHelper.Log("请求版本资源错误");
                info.IsError = true;
                yield break;
            }
            //ResUpdateControler.OnUpdateProgress(0, "请求资源缓存列表...");
            ResAsyncInfo requestResCacheInfo = ResUpgrader.RequestResCache();
            yield return requestResCacheInfo.CurCoroutine;
            if (requestResCacheInfo.IsError)
            {
                ResLoadHelper.Log("请求资源缓存列表错误");
                info.IsError = true;
                yield break;
            }

            //ResUpdateControler.OnUpdateProgress(0, "请求表格资源...");
            ResAsyncInfo requestResSheetInfo = ResUpgrader.RequestResSheet();
            yield return requestResSheetInfo.CurCoroutine;
            if (requestResSheetInfo.IsError)
            {
                ResLoadHelper.Log("请求表格资源错误");
                info.IsError = true;
                yield break;
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator LoadConfigAsync(ResAsyncInfo info)
        {
            //ResUpdateControler.OnUpdateProgress(0, "加载版本资源...");
            ResAsyncInfo loadResVersionInfo = ResLoader.LoadResVersion();
            yield return loadResVersionInfo.CurCoroutine;
            if (loadResVersionInfo.IsError)
            {
                ResLoadHelper.Log("加载版本资源错误");
                info.IsError = true;
                yield break;
            }
            //ResUpdateControler.OnUpdateProgress(0, "加载资源缓存列表...");
            ResAsyncInfo loadResCacheInfo = ResLoader.LoadResCache();
            yield return loadResCacheInfo.CurCoroutine;
            if (loadResCacheInfo.IsError)
            {
                ResLoadHelper.Log("加载缓存资源列表错误");
                info.IsError = true;
                yield break;
            }
            //ResUpdateControler.OnUpdateProgress(0, "加载表格资源...");
            ResAsyncInfo loadResSheetInfo = ResLoader.LoadResSheet();
            yield return loadResSheetInfo.CurCoroutine;
            if (loadResSheetInfo.IsError)
            {
                ResLoadHelper.Log("加载表格资源列表错误");
                info.IsError = true;
                yield break;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator StartDownload(ResAsyncInfo info)
        {
            List<int> toUpgradeRes = new List<int>();
            ResUpdateTool.SearchUpgradeRes(toUpgradeRes);
            if (toUpgradeRes != null && toUpgradeRes.Count > 0)
            {
                DateTime tStartDownload = DateTime.Now;
                long totalSize = ResLoadHelper.CaculateABSize(toUpgradeRes);
                long accSize = 0;
                List<ResAsyncInfo> asyncList = new List<ResAsyncInfo>();
                List<ResAsyncInfo> asyncDoneList = new List<ResAsyncInfo>();
                int index = 0;
                while (true)
                {
                    while (asyncList.Count < ResUpdateControler.s_AsyncCoroutineMax && index < toUpgradeRes.Count)
                    {
                        int abId = toUpgradeRes[index++];
                        ResAsyncInfo loadAssetInfo = ResUpgrader.DownLoadResById(abId);
                        loadAssetInfo.Target = (System.Object)(abId);
                        asyncList.Add(loadAssetInfo);
                    }
                    asyncDoneList.Clear();
                    foreach (ResAsyncInfo loadAssetInfo in asyncList)
                    {
                        if (loadAssetInfo.IsError)
                        {
                            ResLoadHelper.Log("更新资源错误 ab:" + loadAssetInfo.Tip);
                            info.IsError = true;
                            yield break;
                        }
                        else if (loadAssetInfo.IsDone)
                        {
                            int abId = (int)loadAssetInfo.Target;
                            ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
                            if (data != null)
                            {
                                accSize += data.m_Size;
                            }
                            double elapsed = (DateTime.Now - tStartDownload).TotalSeconds;
                            if (elapsed < double.Epsilon)
                            {
                                elapsed = double.Epsilon;
                            }
                            double speed = accSize / elapsed;
                            info.Progress = (float)(accSize / ((totalSize > 0) ? totalSize : double.Epsilon));
                            string tinfo = string.Format("下载资源：速度：{0:F1}KB/s {1:F1}M/{2:F1}M",
                              (float)speed / 1024, (float)accSize / (1024 * 1024),
                              (float)totalSize / (1024 * 1024));
                            info.Tip = tinfo;
                            ResUpdateControler.OnUpdateProgress(info.Progress, info.Tip);

                            asyncDoneList.Add(loadAssetInfo);
                        }
                    }
                    foreach (ResAsyncInfo loadAssetInfo in asyncDoneList)
                    {
                        asyncList.Remove(loadAssetInfo);
                    }
                    if (ResUpdateControler.DownLoadNum >= 20)
                    {
                        ResLoader.SaveClientResVersion();
                        ResUpdateControler.DownLoadNum = 0;
                    }
                    asyncDoneList.Clear();
                    if (asyncList.Count == 0 && index >= toUpgradeRes.Count)
                    {
                        break;
                    }
                    yield return 1;
                }
                ResUpdateControler.OnUpdateProgress(info.Progress, "资源下载完成");
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator CacheResByConfig(ResAsyncInfo info, List<ResCacheConfig> cacheConfigList)
        {
            //ResUpdateControler.OnUpdateProgress(0, "开始加载缓存资源");

            Dictionary<int, ResCacheData> cacheDataDict = new Dictionary<int, ResCacheData>();
            foreach (ResCacheConfig cacheConfig in cacheConfigList)
            {
                ResCacheProvider.Instance.SearchCacheDataByConfig(cacheConfig, ref cacheDataDict);
            }
            List<ResCacheData> cacheDataList = ResCacheProvider.Instance.SortCacheDataList(cacheDataDict);
            cacheDataDict.Clear();
            if (cacheDataList == null || cacheDataList.Count == 0)
            {
                info.IsDone = true;
                info.Progress = 1.0f;
                yield break;
            }
            //ResLoadHelper.Log(ResLoadHelper.DumpResCacheDataInfo(cacheDataList));
            //AssetExManager.Instance.OutputAssetExManagerInfo();

            List<AssetEx> assetList = new List<AssetEx>();
            List<AssetEx> assetCacheList = new List<AssetEx>();
            AssetExManager.Instance.SearchByCacheDataList(cacheDataList, ref assetList, ref assetCacheList);

            //ResLoadHelper.Log(AssetExManager.Instance.DumpSpecificAssetInfo(assetList));

            AssetLoadingProgressHandler loadingProgress = new AssetLoadingProgressHandler();
            loadingProgress.CurNum = 0;
            loadingProgress.TotalNum = assetList.Count;

            //List<CacheCmd> cacheCmdList = new List<CacheCmd>();
            //List<ExtractCmd> extractCmdList = new List<ExtractCmd>();

            foreach (AssetEx asset in assetList)
            {
                asset.IncAssetBundleRefCount(true);
                CacheCmd cmd = AssetExManager.Instance.AddCacheCmd(asset.Id, false);
                cmd.RegisterHandler(loadingProgress);
                //cacheCmdList.Add(cmd);
            }
            foreach (AssetEx asset in assetCacheList)
            {
                ExtractCmd cmd = AssetExManager.Instance.AddExtractCmd(asset.Id);
                //extractCmdList.Add(cmd);
            }
            //foreach (CacheCmd cmd in cacheCmdList) {
            //  if (cmd != null) {
            //    if (!cmd.IsDone()) {
            //      yield return 1;
            //    } else if (cmd.IsError()) {
            //      info.IsError = true;
            //      yield break;
            //    }
            //  }
            //}
            //foreach (ExtractCmd cmd in extractCmdList) {
            //  if (cmd != null && !cmd.IsDone()) {
            //    yield return 1;
            //  }
            //}
            while (!loadingProgress.IsDone() || AssetExManager.Instance.m_ExtractCmdDict.Count > 0)
            {
                yield return 1;
            }
            //cacheCmdList.Clear();
            //extractCmdList.Clear();

            ResUpdateControler.OnUpdateProgress(1.0f);
            Resources.UnloadUnusedAssets();

            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
