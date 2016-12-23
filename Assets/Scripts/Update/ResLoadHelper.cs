using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;

namespace DashFire
{
    public class ResLoadHelper
    {
        private static string s_StreamingAssetsPath = string.Empty;
        private static string s_ResServerURLAbs = string.Empty;

        #region Resources Server Url
        public static string GetResServerURLAbs()
        {
            if (!string.IsNullOrEmpty(s_ResServerURLAbs))
            {
                return s_ResServerURLAbs;
            }
            else
            {
                s_ResServerURLAbs = string.Format("{0}/{1}/{2}_{3}/",
                  ResUpdateControler.ResServerURL, ResUpdateControler.AppName,
                  ResUpdateControler.Platform, ResUpdateControler.Channel);
                return s_ResServerURLAbs;
            }
        }

        //获取版本文件的下载路径
        public static string GetResVersionFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResVersionZip;
        }

        //获取Asset数据文件的下载路径
        public static string GetAssetDBFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_AssetDBZip;
        }

        //获取素材缓存文件的下载路径
        public static string GetResCacheFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResCacheZip;
        }

        public static string GetResSheetFileURL()
        {
            return GetResServerURLAbs() + ResUpdateControler.s_ResSheetZip;
        }

        //获取素材AssetBundle文件的下载路径
        public static string GetResABURL(ResVersionData resInfo)
        {
            if (resInfo == null)
            {
                ResLoadHelper.Log("GetResABURL ResVersionData null");
                return string.Empty;
            }
            return GetResServerURLAbs() + resInfo.m_AssetBundleName;
        }
        #endregion

        #region Resources Local Url
        public static string GetLocalResPath(string abPath, bool isBuildIn)
        {
            StringBuilder sb = new StringBuilder();
            if (isBuildIn)
            {
                sb.Append(GetStreamingAssetPath());
                sb.Append(ResUpdateControler.s_ResBuildInPath);
                sb.Append(abPath);
            }
            else
            {
                sb.Append(Application.persistentDataPath);
                sb.Append("/");
                sb.Append(ResUpdateControler.s_ResCachePath);
                sb.Append(abPath);
            }
            return sb.ToString();
        }

        public static string GetLocalResVersionFileURL()
        {
            return GetLocalResPath(ResUpdateControler.s_ResVersionZip, IsABBuiltIn(ResUpdateControler.s_ResVersionZip));
        }

        public static string GetLocalAssetDBFileURL()
        {
            return GetLocalResPath(ResUpdateControler.s_AssetDBZip, IsABBuiltIn(ResUpdateControler.s_AssetDBZip));
        }

        public static string GetLocalResCacheFileURL()
        {
            return GetLocalResPath(ResUpdateControler.s_ResCacheZip, IsABBuiltIn(ResUpdateControler.s_ResCacheZip));
        }

        public static string GetLocalResSheetFileURL()
        {
            return GetLocalResPath(ResUpdateControler.s_ResSheetZip, IsABBuiltIn(ResUpdateControler.s_ResSheetZip));
        }

        public static string GetLocalResABURL(string abName)
        {
            return GetLocalResPath(abName, IsABBuiltIn(abName));
        }

        public static string GetLocalResABURL(ResVersionData resInfo)
        {
            if (resInfo == null)
            {
                ResLoadHelper.Log("GetLocalResABURL ResVersionData null");
                return string.Empty;
            }
            return GetLocalResABURL(resInfo.m_AssetBundleName);
        }

        public static string GetLocalResPathForSave(string abPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Application.persistentDataPath);
            sb.Append("/");
            sb.Append(ResUpdateControler.s_ResCachePath);
            sb.Append(abPath);
            return sb.ToString();
        }

        public static string GetLocalResVersionFileURLForSave()
        {
            return GetLocalResPathForSave(ResUpdateControler.s_ResVersionZip);
        }

        public static string GetLocalAssetDBFileURLForSave()
        {
            return GetLocalResPathForSave(ResUpdateControler.s_AssetDBZip);
        }

        public static string GetLocalResCacheFileURLForSave()
        {
            return GetLocalResPathForSave(ResUpdateControler.s_ResCacheZip);
        }

        public static string GetLocalResSheetFileURLForSave()
        {
            return GetLocalResPathForSave(ResUpdateControler.s_ResSheetZip);
        }

        public static string GetLocalResABURLForSave(string abName)
        {
            return GetLocalResPathForSave(abName);
        }

        public static string GetLocalResABURLForSave(ResVersionData resInfo)
        {
            if (resInfo == null)
            {
                ResLoadHelper.Log("GetLocalResABURL ResVersionData null");
                return string.Empty;
            }
            return GetLocalResABURLForSave(resInfo.m_AssetBundleName);
        }
        #endregion

        #region AssetBundle Getter
        public static bool IsABInChapter(int abChapter, int curChapter)
        {
            return abChapter >= 1 && abChapter <= curChapter;
        }

        public static bool IsABBuiltIn(string abName)
        {
            if (abName.Equals(ResUpdateControler.s_ResVersionZip))
            {
                return !ResUpdateControler.IsResVersionConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_AssetDBZip))
            {
                return !ResUpdateControler.IsAssetDBConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_ResCacheZip))
            {
                return !ResUpdateControler.IsResCacheConfigCached;
            }
            else if (abName.Equals(ResUpdateControler.s_ResSheetZip))
            {
                return !ResUpdateControler.IsResSheetConfigCached;
            }

            ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
            return (lrv != null) && lrv.m_IsBuildIn;
        }

        public static bool IsABCached(string abName, string md5)
        {
            ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
            if (lrv == null)
            {
                return false;
            }
            if (string.Compare(md5, lrv.m_MD5, true) != 0)
            {
                return false;
            }
            if (!IsABBuiltIn(abName) && !File.Exists(GetLocalResABURL(abName)))
            {
                return false;
            }
            return true;
        }

        public static bool IsABCached(ResVersionData resInfo)
        {
            if (resInfo == null)
            {
                ResLoadHelper.Log("IsABCached ResVersionData null");
                return false;
            }
            return IsABCached(resInfo.m_AssetBundleName, resInfo.m_MD5);
        }
        #endregion

        #region ResVersionData Helper
        public static ResVersionData GetResVersionDataByAssetId(int resId)
        {
            ResVersionData resVersionData = ResVersionProvider.Instance.GetDataById(resId);
            if (resVersionData == null)
            {
                ResLoadHelper.Log("GetResVersionDataByAssetId GetResVersionData failed resId:" + resId);
                return null;
            }
            return resVersionData;
        }

        public static ResVersionData GetResVersionDataByAssetName(string resName)
        {
            ResVersionData resVersionData = ResVersionProvider.Instance.GetDataByAssetName(resName);
            if (resVersionData == null)
            {
                ResLoadHelper.Log("GetResVersionDataByAssetId GetResVersionData failed resName:" + resName);
                return null;
            }
            return resVersionData;
        }
        #endregion

        #region Path Converter
        public static string GetStreamingAssetPath()
        {
            if (!string.IsNullOrEmpty(s_StreamingAssetsPath))
            {
                return s_StreamingAssetsPath;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
#if UNITY_ANDROID
      sb.Append("jar:file://");
      sb.Append(Application.dataPath);
      sb.Append("!/assets/");
#else
                sb.Append("file://");
                sb.Append(Application.streamingAssetsPath);
                sb.Append("/");
#endif
                s_StreamingAssetsPath = sb.ToString();
                return s_StreamingAssetsPath;
            }
        }

        public static string ConvertResourceAssetPathToAbs(string resPathInResDir)
        {
            if (resPathInResDir.StartsWith("assets/"))
            {
                return resPathInResDir;
            }
            else
            {
                //NOTE:Unity 4.2.2 支持多个Resources目录遍历查找，但DF项目强制只能创建一个Resources目录。
                //NOTE:且不能带有后缀
                StringBuilder sb = new StringBuilder();
                sb.Append("assets/resources/");
                sb.Append(resPathInResDir);
                //sb.Append(".prefab");
                return sb.ToString();
            }
        }
        #endregion

        #region Tools
        public static long CaculateABSize(List<int> toUpgradeRes)
        {
            long size = 0;
            foreach (int abId in toUpgradeRes)
            {
                ResVersionData data = ResVersionProvider.Instance.GetDataById(abId);
                if (data != null)
                {
                    size += data.m_Size;
                }
            }
            return size;
        }

        public static bool CheckFilePatternEndWith(string filePath, string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
            {
                return false;
            }
            foreach (string tPattern in pattern)
            {
                if (filePath.EndsWith(tPattern))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckFilePatternStartsWith(string filePath, string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
            {
                return false;
            }
            foreach (string tPattern in pattern)
            {
                if (filePath.StartsWith(tPattern))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetFullNameWithoutExtention(string asset)
        {
            if (asset.Contains("."))
            {
                int endIndex = asset.LastIndexOf(".");
                return asset.Substring(0, endIndex);
            }
            else
            {
                return asset;
            }
        }
        #endregion

        public static string DumpResCacheDataInfo(List<ResCacheData> cacheDataList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ResCacheData Count:" + cacheDataList.Count);
            foreach (ResCacheData data in cacheDataList)
            {
                sb.AppendFormat("ResCacheData:CacheId:{0} CacheType:{1} Assets:{2}\n", data.GetId(), data.m_CacheType, FormatListContent(data.m_Assets));
                foreach (int assetId in data.m_Assets)
                {
                    sb.AppendFormat("ResCacheData assetId:{0} count:{1} depend:{2}\n",
                      assetId,
                      AssetExManager.Instance.GetAssetDependency(assetId).Count,
                      FormatListContent(AssetExManager.Instance.GetAssetDependency(assetId)));
                }
            }
            return sb.ToString();
        }

        public static string FormatListContent<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    sb.Append(list[index].ToString().Trim() + ";");
                }
            }
            return sb.ToString();
        }

        public static void Log(string msg)
        {
            LogicSystem.LogicLog(msg);
            UnityEngine.Debug.Log(msg);
        }

        public static void ErrLog(string msg)
        {
            LogicSystem.LogicErrorLog(msg);
            UnityEngine.Debug.LogError(msg);
        }

        public static string GetDynamicUrl(string url)
        {
            if (ResUpdateControler.s_UseDynmaicUrl)
            {
                url += "?time=" + DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            return url;
        }
    }
}