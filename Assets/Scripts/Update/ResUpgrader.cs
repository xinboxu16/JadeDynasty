using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using UnityEngine;

namespace DashFire
{
    /// <summary>
    /// 以ResUpdateHandler的方式对外提供注册接口，
    /// ResAsyncInfo提供异步的查询信息，用于与unity的异步接口一致，
    /// 如Application.LoadLevelAsync(...)
    /// 如需协同执行，执行yield return ResAsyncInfo.CurCoroutine;
    /// </summary>
    internal class ResUpgrader
    {
        internal static ResAsyncInfo RequestClientVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.RequestClientVersion(info));
            return info;
        }
        internal static ResAsyncInfo RequestServerVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.RequestServerVersion(info));
            return info;
        }
        internal static ResAsyncInfo RequestResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.RequestResVersion(info));
            return info;
        }
        internal static ResAsyncInfo RequestResCache()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.RequestResCache(info));
            return info;
        }
        internal static ResAsyncInfo RequestResSheet()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.RequestResSheet(info));
            return info;
        }
        internal static ResAsyncInfo DownLoadResById(int abId)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.DownLoadRes(data, info));
            return info;
        }
        internal static ResAsyncInfo DownLoadRes(ResVersionData data)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResCoroutineUpgradeHandler.DownLoadRes(data, info));
            return info;
        }
    }
    /// <summary>
    /// Unity 4.2.2不支持在dll的类中混合IEnumerator和普通函数, 
    /// 因此，将包含yield的函数分离开来，by lixiaojiang
    /// Bug:http://lists.ximian.com/pipermail/mono-bugs/2011-July/112237.html
    /// </summary>
    internal class ResCoroutineUpgradeHandler
    {
        public static IEnumerator RequestClientVersion(ResAsyncInfo info)
        {
            string versionFileStreamingPath = Path.Combine(ResLoadHelper.GetStreamingAssetPath(),
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ClientVersionFile);
            string versionFilePersistPath = Path.Combine(Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ClientVersionFile);
            bool isPersistFileAlreadyExit = File.Exists(versionFilePersistPath);

            // Read Streaming version file
            ResLoadHelper.Log("RequestClientVersion URL;" + versionFileStreamingPath);
            using (WWW tWWW = new WWW(versionFileStreamingPath))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestClientVersion error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] bytes = tWWW.bytes;
                    if (bytes == null || bytes.Length == 0)
                    {
                        ResLoadHelper.Log("RequestClientVersion bytes null or empty data;" + versionFileStreamingPath);
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }

                    VersionInfo clientVersionInfo = new VersionInfo();
                    clientVersionInfo.Load(versionFileStreamingPath, bytes);
                    ResUpdateControler.BuildinClientVersionInfo = clientVersionInfo;

                    ResUpdateControler.IsNeedSyncPackage = false;
                    if (isPersistFileAlreadyExit)
                    {
                        clientVersionInfo = new VersionInfo();
                        clientVersionInfo.Load(versionFilePersistPath);
                        ResUpdateControler.PersistClientVersionInfo = clientVersionInfo;

                        int buildInVersionValue = ResUpdateControler.BuildinClientVersionInfo.Version.GetVersionValue();
                        int persistVersionValue = ResUpdateControler.PersistClientVersionInfo.Version.GetVersionValue();
                        if (buildInVersionValue > persistVersionValue)
                        {
                            ResUpdateControler.IsNeedSyncPackage = true;
                        }
                    }
                    else
                    {
                        ResUpdateControler.IsNeedSyncPackage = true;
                    }

                    if (ResUpdateControler.IsNeedSyncPackage)
                    {
                        string dir = Path.GetDirectoryName(versionFilePersistPath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        if (null != bytes)
                        {
                            File.WriteAllBytes(versionFilePersistPath, bytes);
                        }
                        else
                        {
                            ResLoadHelper.Log("RequestClientVersion bytes null or empty data;" + versionFileStreamingPath);
                            info.IsError = true;
                            yield break;
                        }
                    }

                    clientVersionInfo = new VersionInfo();
                    clientVersionInfo.Load(versionFilePersistPath);
                    ResUpdateControler.ClientVersionInfo = clientVersionInfo;
                    ResUpdateControler.OnUpdateVersionInfo();
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestClientVersion ex:" + ex);
                    info.IsError = true;
                    yield break;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static IEnumerator RequestServerVersion(ResAsyncInfo info)
        {
            string versionFilePersistPath = ResLoadHelper.GetResServerURLAbs() + ResUpdateControler.s_ServerVersionFile;
            string persistVersionFilePath = Path.Combine(Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ServerVersionFile);
            versionFilePersistPath = ResLoadHelper.GetDynamicUrl(versionFilePersistPath);
            ResLoadHelper.Log("RequestServerVersion URL;" + versionFilePersistPath);
            using (WWW tWWW = new WWW(versionFilePersistPath))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestServerVersion error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }

                    byte[] bytes = tWWW.bytes;
                    if (bytes == null || bytes.Length == 0)
                    {
                        ResLoadHelper.Log("RequestServerVersion bytes null or empty data;" + ResUpdateControler.s_ServerVersionFile);
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                    if (!string.IsNullOrEmpty(persistVersionFilePath))
                    {
                        string dir = Path.GetDirectoryName(persistVersionFilePath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        File.WriteAllBytes(persistVersionFilePath, bytes);
                    }
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResVersion ex:" + ex);
                    info.IsError = true;
                    yield break;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            try
            {
                VersionInfo serverVersionInfo = new VersionInfo();
                serverVersionInfo.Load(persistVersionFilePath);
                ResUpdateControler.ServerVersionInfo = serverVersionInfo;
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("LoadVersionInfo parse ini ex:" + ex);
                info.IsError = true;
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static IEnumerator RequestResVersion(ResAsyncInfo info)
        {
            string requestResVersionUrl = ResLoadHelper.GetResVersionFileURL();
            requestResVersionUrl = ResLoadHelper.GetDynamicUrl(requestResVersionUrl);
            ResLoadHelper.Log("RequestResVersion URL;" + requestResVersionUrl);
            using (WWW tWWW = new WWW(requestResVersionUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResVersionList error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }

                    ResUpdateControler.IsResVersionConfigCached = true;
                    ResUpdateTool.SaveCacheAB(tWWW.bytes, ResUpdateControler.s_ResVersionZip, "");

                    TextAsset txt = tWWW.assetBundle.LoadAsset(
                      ResUpdateControler.s_ResVersionFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                    if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                    {
                        ResLoadHelper.Log("RequestResVersion bytes null");
                        info.IsError = true;
                        yield break;
                    }
                    byte[] bytes = txt.bytes;
                    ResVersionProvider.Instance.Clear();
                    ResVersionProvider.Instance.CollectDataFromDBC(bytes);
                    AssetExManager.Instance.InitAllAssetEx();
                    tWWW.assetBundle.Unload(true);
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResVersion ex:" + ex);
                    info.IsError = true;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static IEnumerator RequestResCache(ResAsyncInfo info)
        {
            string requestResCacheUrl = ResLoadHelper.GetResCacheFileURL();
            requestResCacheUrl = ResLoadHelper.GetDynamicUrl(requestResCacheUrl);
            ResLoadHelper.Log("RequestResCache URL;" + requestResCacheUrl);
            using (WWW tWWW = new WWW(requestResCacheUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResCache error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }

                    ResUpdateControler.IsResCacheConfigCached = true;
                    ResUpdateTool.SaveCacheAB(tWWW.bytes, ResUpdateControler.s_ResCacheZip, "");

                    TextAsset txt = tWWW.assetBundle.LoadAsset(ResUpdateControler.s_ResCacheFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                    if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                    {
                        ResLoadHelper.Log("RequestResCache bytes null");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                    byte[] bytes = txt.bytes;
                    ResCacheProvider.Instance.Clear();
                    ResCacheProvider.Instance.CollectDataFromDBC(bytes);
                    tWWW.assetBundle.Unload(true);
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResCache ex:" + ex);
                    info.IsError = true;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static IEnumerator RequestResSheet(ResAsyncInfo info)
        {
            string requestResSheetUrl = ResLoadHelper.GetResSheetFileURL();
            requestResSheetUrl = ResLoadHelper.GetDynamicUrl(requestResSheetUrl);
            ResLoadHelper.Log("RequestResCache URL;" + requestResSheetUrl);
            using (WWW tWWW = new WWW(requestResSheetUrl))
            {
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("RequestResSheet error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                    AssetBundle assetBundle = tWWW.assetBundle;
                    if (assetBundle != null)
                    {
                        ResUpdateTool.ExtractResSheet(assetBundle);
                        ResUpdateControler.IsResSheetConfigCached = true;
                        assetBundle.Unload(true);
                    }
                    else
                    {
                        ResLoadHelper.Log("RequestResSheet error");
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("RequestResSheet ex:" + ex);
                    info.IsError = true;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        public static IEnumerator DownLoadRes(ResVersionData data, ResAsyncInfo info)
        {
            string url = ResLoadHelper.GetResABURL(data);
            if (ResLoadHelper.IsABCached(data))
            {
                ResLoadHelper.Log("LoadRes res already cached data:" + data.m_AssetBundleName);
                info.IsDone = true;
                info.Progress = 1.0f;
                yield break;
            }
            info.Tip = data.m_AssetBundleName;
            url = ResLoadHelper.GetDynamicUrl(url);
            using (WWW tWWW = new WWW(url))
            {
                //tWWW.threadPriority = ThreadPriority.High;
                yield return tWWW;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("DownLoadRes error data:" + data.m_AssetBundleName + " Url:" + url);
                        info.IsError = true;
                        tWWW.Dispose();
                        yield break;
                    }
                    //NOTE:累加下载数，用于保存本地资源列表,by lixiaojiang
                    ResUpdateTool.SaveCacheAB(tWWW.bytes, data.m_AssetBundleName, data.m_MD5);
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("DownLoadRes ex:" + ex);
                    info.IsError = true;
                }
                finally
                {
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}