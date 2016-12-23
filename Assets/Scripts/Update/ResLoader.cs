using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace DashFire
{
    /// <summary>
    /// 以ResUpdateHandler的方式对外提供注册接口，
    /// ResAsyncInfo提供异步的查询信息，用于与unity的异步接口一致，
    /// 如Application.LoadLevelAsync(...)
    /// 如需协同执行，执行yield return ResAsyncInfo.CurCoroutine;
    /// </summary>
    internal class ResLoader
    {
        internal static ResAsyncInfo LoadResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResLoaderCoroutineHandler.LoadResVersion(info));
            return info;
        }
        internal static ResAsyncInfo LoadResCache()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResLoaderCoroutineHandler.LoadResCache(info));
            return info;
        }
        internal static ResAsyncInfo LoadResSheet()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResLoaderCoroutineHandler.LoadResSheet(info));
            return info;
        }
        internal static ResAsyncInfo LoadClientResVersion()
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(ResLoaderCoroutineHandler.LoadClientResVersion(info));
            return info;
        }
        internal static void SaveClientResVersion()
        {
            if (ClientResVersionProvider.Instance != null)
            {
                string fileContent = ResUpdateControler.s_ResVersionClientHeader + "\n";
                foreach (ClientResVersionData data in ClientResVersionProvider.Instance.GetArray().Values)
                {
                    if (data != null)
                    {
                        string dataStr = string.Format(ResUpdateControler.s_ResVersionClientFormat + "\n",
                          data.m_Name, data.m_MD5, data.m_IsBuildIn);
                        fileContent += dataStr;
                    }
                }
                try
                {
                    string resVersionFilePersistPath = Path.Combine(Application.persistentDataPath,
                        ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ResVersionClientFile);
                    File.WriteAllText(resVersionFilePersistPath, fileContent);
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("SaveClientResVersion file failed!" + ex);
                }
            }
        }
    }

    /// <summary>
    /// Unity 4.2.2不支持在dll的类中混合IEnumerator和普通函数, 
    /// 因此，将包含yield的函数分离开来，by lixiaojiang
    /// Bug:http://lists.ximian.com/pipermail/mono-bugs/2011-July/112237.html
    /// </summary>
    internal class ResLoaderCoroutineHandler
    {
        internal static IEnumerator LoadResVersion(ResAsyncInfo info)
        {
            string url = ResLoadHelper.GetLocalResVersionFileURL();
            ResLoadHelper.Log("LoadResVersion url:" + url);
            AssetBundle assetBundle = null;
            if (ResLoadHelper.IsABBuiltIn(ResUpdateControler.s_ResVersionZip))
            {
                using (WWW tWWW = new WWW(url))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadResVersion www error url:" + url);
                            tWWW.Dispose();
                            yield break;
                        }
                        assetBundle = tWWW.assetBundle;
                        if (assetBundle == null)
                        {
                            ResLoadHelper.Log("LoadResVersion null url:" + url);
                            tWWW.Dispose();
                            info.IsError = true;
                            yield break;
                        }
                        TextAsset txt = assetBundle.LoadAsset(
                              ResUpdateControler.s_ResVersionFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                        if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                        {
                            ResLoadHelper.Log("LoadResVersion bytes null");
                            info.IsError = true;
                            yield break;
                        }
                        byte[] bytes = txt.bytes;
                        ResVersionProvider.Instance.Clear();
                        ResVersionProvider.Instance.CollectDataFromDBC(bytes);
                        AssetExManager.Instance.InitAllAssetEx();
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResVersion ab failed url:" + url + "ex:" + ex);
                        info.IsError = true;
                    }
                    finally
                    {
                        if (assetBundle != null)
                        {
                            assetBundle.Unload(true);
                        }
                        tWWW.Dispose();
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    if (buffer == null || buffer.Length == 0)
                    {
                        ResLoadHelper.Log("LoadResVersion bytes null url:" + url);
                        yield break;
                    }
                    AssetBundleCreateRequest abRequest = AssetBundle.LoadFromMemoryAsync(buffer);
                    yield return abRequest;
                    try
                    {
                        assetBundle = abRequest.assetBundle;
                        if (assetBundle == null)
                        {
                            ResLoadHelper.Log("LoadResVersion assetBundleObj null url:" + url);
                            fs.Close();
                            info.IsError = true;
                            yield break;
                        }
                        TextAsset txt = assetBundle.LoadAsset(
                              ResUpdateControler.s_ResVersionFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                        if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                        {
                            ResLoadHelper.Log("LoadResVersion bytes null");
                            info.IsError = true;
                            yield break;
                        }
                        byte[] bytes = txt.bytes;
                        ResVersionProvider.Instance.Clear();
                        ResVersionProvider.Instance.CollectDataFromDBC(bytes);
                        AssetExManager.Instance.InitAllAssetEx();
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResVersion res failed url:" + url + " ex:" + ex);
                    }
                    finally
                    {
                        if (assetBundle != null)
                        {
                            assetBundle.Unload(true);
                        }
                        fs.Close();
                    }
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator LoadClientResVersion(ResAsyncInfo info)
        {
            //从本地加载版本号
            string resVersionFilePersistPath = Path.Combine(Application.persistentDataPath,
              ResUpdateControler.s_ResCachePath + ResUpdateControler.s_ResVersionClientFile);
            string buildinVersionFilePath = Path.Combine(ResLoadHelper.GetStreamingAssetPath(),
                ResUpdateControler.s_ResBuildInPath + ResUpdateControler.s_ResVersionClientFile);

            if (File.Exists(resVersionFilePersistPath))
            {
                if (ResUpdateControler.IsNeedSyncPackage)
                {
                    ClientResVersionProvider.Instance.Clear();
                    ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);

                    using (WWW tWWW = new WWW(buildinVersionFilePath))
                    {
                        yield return tWWW;
                        try
                        {
                            if (tWWW.error != null)
                            {
                                ResLoadHelper.Log("LoadClientResVersion error");
                                tWWW.Dispose();
                                yield break;
                            }
                            byte[] bytes = tWWW.bytes;
                            if (bytes == null || bytes.Length == 0)
                            {
                                ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                                tWWW.Dispose();
                                yield break;
                            }
                            ClientResVersionProvider.Instance.CollectDataFromDBC(bytes);
                            ResLoader.SaveClientResVersion();
                        }
                        catch (System.Exception ex)
                        {
                            ResLoadHelper.Log("LoadClientResVersion ex:" + ex);
                            info.IsError = false;
                            yield break;
                        }
                        finally
                        {
                            tWWW.Dispose();
                        }
                    }
                }
                else
                {
                    ClientResVersionProvider.Instance.Clear();
                    ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);
                }
            }
            else
            {
                using (WWW tWWW = new WWW(buildinVersionFilePath))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadClientResVersion error");
                            tWWW.Dispose();
                            yield break;
                        }
                        byte[] bytes = tWWW.bytes;
                        if (bytes == null || bytes.Length == 0)
                        {
                            ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                            tWWW.Dispose();
                            yield break;
                        }
                        if (!string.IsNullOrEmpty(resVersionFilePersistPath))
                        {
                            string dir = Path.GetDirectoryName(resVersionFilePersistPath);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            if (null != bytes)
                            {
                                File.WriteAllBytes(resVersionFilePersistPath, bytes);
                            }
                            else
                            {
                                ResLoadHelper.Log("LoadClientResVersion bytes null or empty data;" + ResUpdateControler.s_ResVersionClientFile);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadClientResVersion ex:" + ex);
                        yield break;
                    }
                    finally
                    {
                        tWWW.Dispose();
                    }
                }

                ClientResVersionProvider.Instance.Clear();
                ClientResVersionProvider.Instance.CollectDataFromDBC(resVersionFilePersistPath);
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator LoadResCache(ResAsyncInfo info)
        {
            string url = ResLoadHelper.GetLocalResCacheFileURL();
            ResLoadHelper.Log("LoadResCache url:" + url);
            AssetBundle assetBundle = null;
            if (ResLoadHelper.IsABBuiltIn(ResUpdateControler.s_ResCacheZip))
            {
                using (WWW tWWW = new WWW(url))
                {
                    yield return tWWW;
                    try
                    {
                        if (tWWW.error != null)
                        {
                            ResLoadHelper.Log("LoadResCache www errro url:" + url);
                            tWWW.Dispose();
                            info.IsError = true;
                            yield break;
                        }
                        assetBundle = tWWW.assetBundle;
                        if (assetBundle == null)
                        {
                            ResLoadHelper.Log("LoadResCache assetBundleObj null url:" + url);
                            tWWW.Dispose();
                            info.IsError = true;
                            yield break;
                        }
                        TextAsset txt = assetBundle.LoadAsset(
                              ResUpdateControler.s_ResCacheFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                        if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                        {
                            ResLoadHelper.Log("LoadResCache bytes null");
                            info.IsError = true;
                            yield break;
                        }
                        byte[] bytes = txt.bytes;
                        ResCacheProvider.Instance.Clear();
                        ResCacheProvider.Instance.CollectDataFromDBC(bytes);
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResCache ab failed url:" + url + "ex:" + ex);
                        info.IsError = true;
                    }
                    finally
                    {
                        if (assetBundle != null)
                        {
                            assetBundle.Unload(true);
                        }
                        tWWW.Dispose();
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    if (buffer == null || buffer.Length == 0)
                    {
                        ResLoadHelper.Log("LoadResCache bytes null url:" + url);
                        yield break;
                    }
                    AssetBundleCreateRequest abRequest = AssetBundle.LoadFromMemoryAsync(buffer);
                    yield return abRequest;
                    try
                    {
                        assetBundle = abRequest.assetBundle;
                        if (assetBundle == null)
                        {
                            ResLoadHelper.Log("LoadResCache assetBundleObj null url:" + url);
                            fs.Close();
                            info.IsError = true;
                            yield break;
                        }
                        TextAsset txt = assetBundle.LoadAsset(
                              ResUpdateControler.s_ResCacheFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                        if (txt == null || txt.bytes == null || txt.bytes.Length <= 0)
                        {
                            ResLoadHelper.Log("LoadResCache bytes null");
                            info.IsError = true;
                            yield break;
                        }
                        byte[] bytes = txt.bytes;
                        ResCacheProvider.Instance.Clear();
                        ResCacheProvider.Instance.CollectDataFromDBC(bytes);
                    }
                    catch (System.Exception ex)
                    {
                        ResLoadHelper.Log("LoadResCache res failed url:" + url + " ex:" + ex);
                    }
                    finally
                    {
                        if (assetBundle != null)
                        {
                            assetBundle.Unload(true);
                        }
                        fs.Close();
                    }
                }
            }

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        internal static IEnumerator LoadResSheet(ResAsyncInfo info)
        {
            if (ResUpdateControler.IsResSheetConfigCached)
            {
                yield break;
            }
            string url = ResLoadHelper.GetLocalResSheetFileURL();
            ResLoadHelper.Log("LoadResSheet url:" + url);
            if (string.IsNullOrEmpty(url))
            {
                ResLoadHelper.Log("LoadResSheet invalid url:" + url);
                yield break;
            }
            using (WWW tWWW = new WWW(url))
            {
                yield return tWWW;
                AssetBundle assetBundle = null;
                try
                {
                    if (tWWW.error != null)
                    {
                        ResLoadHelper.Log("LoadResSheet BuildIn not cached url:" + url);
                        tWWW.Dispose();
                        yield break;
                    }
                    assetBundle = tWWW.assetBundle;
                    if (assetBundle != null)
                    {
                        ResUpdateTool.ExtractResSheet(assetBundle);
                        ResUpdateControler.IsResSheetConfigCached = true;
                        assetBundle.Unload(true);
                    }
                    else
                    {
                        ResLoadHelper.Log("LoadResSheet BuildIn AssetBundle failed:" + url);
                        tWWW.Dispose();
                        yield break;
                    }
                }
                catch (System.Exception ex)
                {
                    ResLoadHelper.Log("LoadResSheet ab failed url:" + url + "ex:" + ex);
                    info.IsError = true;
                }
                finally
                {
                    if (assetBundle != null)
                    {
                        assetBundle.Unload(false);
                    }
                    tWWW.Dispose();
                }
            }
            info.IsDone = true;
            info.Progress = 1.0f;
        }
    }
}
