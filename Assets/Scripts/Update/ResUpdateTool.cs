using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.IO;
using System.Security.Cryptography;

namespace DashFire
{
    internal class ResUpdateTool
    {
        public static bool SearchUpgradeRes(List<int> container)
        {
            Dictionary<int, ResVersionData> resVersinList = ResVersionProvider.Instance.GetData();
            if (resVersinList != null && resVersinList.Count > 0)
            {
                foreach (ResVersionData rv in resVersinList.Values)
                {
                    if (ResLoadHelper.IsABInChapter(rv.m_Chapter, ResUpdateControler.TargetChapter))
                    {
                        if (!ResLoadHelper.IsABCached(rv))
                        {
                            container.Add(rv.GetId());
                        }
                    }
                }
            }
            return true;
        }
        private static List<ResCacheData> FindCacheDataByCacheType(Dictionary<int, ResCacheData> cacheDataList, ResCacheType cacheType)
        {
            List<ResCacheData> retList = new List<ResCacheData>();
            foreach (ResCacheData cacheData in cacheDataList.Values)
            {
                if (((cacheData.m_CacheType & cacheType) != 0))
                {
                    retList.Add(cacheData);
                }
            }
            return retList;
        }
        public static void SaveCacheAB(byte[] bytes, string abName, string md5)
        {
            try
            {
                if (bytes == null)
                {
                    ResLoadHelper.Log("SaveCacheAB bytes null or empty data;" + abName);
                    return;
                }
                string persistPath = ResLoadHelper.GetLocalResABURLForSave(abName);
                if (!string.IsNullOrEmpty(persistPath))
                {
                    string dir = Path.GetDirectoryName(persistPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.WriteAllBytes(persistPath, bytes);
                    ClientResVersionData lrv = ClientResVersionProvider.Instance.GetDataByName(abName);
                    if (lrv != null)
                    {
                        lrv.m_MD5 = md5;
                        lrv.m_IsBuildIn = false;
                    }
                    else
                    {
                        lrv = new ClientResVersionData();
                        lrv.m_Name = abName;
                        lrv.m_MD5 = md5;
                        lrv.m_IsBuildIn = false;
                        ClientResVersionProvider.Instance.AddData(lrv);
                    }
                    ResUpdateControler.DownLoadNum++;
                }
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("SaveCacheAB abName:" + abName + "ex:" + ex);
            }
        }
        public static void ExtractResSheet(AssetBundle assetBundle)
        {
            if (assetBundle == null)
            {
                return;
            }

            string outPath = Path.Combine(Application.persistentDataPath, ResUpdateControler.s_ResSheetCachePath);
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            string[] tResSheetPattern =
                ResUpdateControler.s_ResSheetPattern.Split(ResUpdateControler.s_ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
            if (tResSheetPattern == null)
            {
                ResLoadHelper.Log("ExtractResSheet ResSheetPattern invalid:" + ResUpdateControler.s_ResSheetPattern);
                return;
            }

            StreamReader sr = null;
            MemoryStream ms = null;
            try
            {
                TextAsset txt = assetBundle.LoadAsset(ResUpdateControler.s_ResSheetFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                if (txt != null && txt.bytes != null && txt.bytes.Length > 0)
                {
                    string sheetListFilePath = Path.Combine(outPath, ResUpdateControler.s_ResSheetFile);
                    File.WriteAllBytes(sheetListFilePath, txt.bytes);

                    ms = new MemoryStream(txt.bytes);
                    ms.Seek(0, SeekOrigin.Begin);
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    sr = new StreamReader(ms, encoding);

                    int totalNum = int.Parse(sr.ReadLine());
                    for (int sheetIndex = 1; sheetIndex <= totalNum; sheetIndex++)
                    {
                        string sheetFile = sr.ReadLine();
                        if (!string.IsNullOrEmpty(sheetFile.Trim()) && ResLoadHelper.CheckFilePatternEndWith(sheetFile, tResSheetPattern))
                        {
                            TextAsset asset = assetBundle.LoadAsset(sheetFile, typeof(UnityEngine.TextAsset)) as TextAsset;
                            if (asset != null && asset.bytes != null && asset.bytes.Length > 0)
                            {
                                string sheetFilePath = Path.Combine(outPath, sheetFile);
                                string dir = Path.GetDirectoryName(sheetFilePath);
                                if (!Directory.Exists(dir))
                                {
                                    Directory.CreateDirectory(dir);
                                }
                                File.WriteAllBytes(sheetFilePath, asset.bytes);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("ExtractResSheet failed ex:" + ex);
            }
            finally
            {
                if (sr != null) { sr.Close(); sr = null; }
                if (ms != null) { ms.Close(); ms = null; }
            }
        }
    }
}