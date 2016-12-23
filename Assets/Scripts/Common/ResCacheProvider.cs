using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DashFire;
using UnityEngine;

namespace DashFire
{
    public class ResCacheData : IData
    {
        public int m_Id;
        public int m_Chapter;
        public ResCacheType m_CacheType;
        public int m_ResId;
        //public List<string> m_AssetNames;
        public List<int> m_Assets;
        public List<int> m_LinkList;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", -1, true);
            m_Chapter = DBCUtil.ExtractNumeric<int>(node, "Chapter", -1, true);
            m_CacheType = (ResCacheType)Enum.Parse(typeof(ResCacheType), DBCUtil.ExtractString(node, "CacheType", "", true).Trim().ToLower());
            m_ResId = DBCUtil.ExtractNumeric<int>(node, "ResId", -1, true);

            //m_AssetNames = new List<string>();
            //string tAssets = DBCUtil.ExtractString(node, "AssetNames", "", true).Trim().ToLower();
            //if (!string.IsNullOrEmpty(tAssets)) {
            //  string[] tAssetsList = tAssets.Split(ResUpdateControler.s_ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
            //  if (tAssetsList != null && tAssetsList.Length > 0) {
            //    m_AssetNames.AddRange(tAssetsList);
            //  }
            //}

            m_Assets = new List<int>();
            string tAssets = DBCUtil.ExtractString(node, "Assets", "", true).Trim().ToLower();
            if (!string.IsNullOrEmpty(tAssets))
            {
                string[] tAssetsList = tAssets.Split(ResUpdateControler.s_ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
                if (tAssetsList != null && tAssetsList.Length > 0)
                {
                    foreach (string assetId in tAssetsList)
                    {
                        m_Assets.Add(Convert.ToInt32(assetId));
                    }
                }
            }
            m_LinkList = new List<int>();
            string tLinks = DBCUtil.ExtractString(node, "Links", "", false).Trim().ToLower();
            if (!string.IsNullOrEmpty(tAssets))
            {
                string[] tLinksList = tLinks.Split(ResUpdateControler.s_ConfigSplit, StringSplitOptions.RemoveEmptyEntries);
                if (tLinksList != null && tLinksList.Length > 0)
                {
                    foreach (string linkId in tLinksList)
                    {
                        m_LinkList.Add(Convert.ToInt32(linkId));
                    }
                }
            }
            return true;
        }
        public int GetId()
        {
            return m_Id;
        }
    }

    public class ResCacheProvider
    {
        #region Singleton
        private static ResCacheProvider s_instance_ = new ResCacheProvider();
        public static ResCacheProvider Instance
        {
            get { return s_instance_; }
        }
        #endregion

        Dictionary<int, ResCacheData> m_ConfigDict = new Dictionary<int, ResCacheData>();
        public void Clear()
        {
            m_ConfigDict.Clear();
        }
        public bool CollectDataFromDBC(string file)
        {
            bool result = true;
            DBC document = new DBC();
            document.Load(file);
            for (int index = 0; index < document.RowNum; index++)
            {
                DBC_Row node = document.GetRowByIndex(index);
                if (node != null)
                {
                    ResCacheData data = new ResCacheData();
                    bool ret = data.CollectDataFromDBC(node);
                    if (ret)
                    {
                        m_ConfigDict.Add(data.GetId(), data);
                    }
                    else
                    {
                        string info = string.Format("DataTempalteMgr.CollectDataFromXml collectData Row:{0} failed!", index);
                        LogSystem.Assert(ret, info);
                        result = false;
                    }
                }
            }
            return result;
        }
        public bool CollectDataFromDBC(byte[] bytes)
        {
            bool result = true;
            MemoryStream ms = null;
            StreamReader sr = null;
            try
            {
                DBC document = new DBC();
                ms = new MemoryStream(bytes);
                ms.Seek(0, SeekOrigin.Begin);
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                sr = new StreamReader(ms, encoding);
                document.LoadFromStream(sr);
                for (int index = 0; index < document.RowNum; index++)
                {
                    DBC_Row node = document.GetRowByIndex(index);
                    if (node != null)
                    {
                        ResCacheData data = new ResCacheData();
                        bool ret = data.CollectDataFromDBC(node);
                        if (ret)
                        {
                            m_ConfigDict.Add(data.GetId(), data);
                        }
                        else
                        {
                            string info = string.Format("DataTempalteMgr.CollectDataFromXml collectData Row:{0} failed!", index);
                            LogSystem.Assert(ret, info);
                            result = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                string info = string.Format("DataTempalteMgr.CollectDataFromXml exception ex:", ex);
                LogSystem.Assert(false, info);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return result;
        }
        public ResCacheData GetDataById(int id)
        {
            if (m_ConfigDict.ContainsKey(id))
            {
                return m_ConfigDict[id];
            }
            return null;
        }
        public List<ResCacheData> GetArray()
        {
            List<ResCacheData> list = new List<ResCacheData>();
            foreach (ResCacheData config in m_ConfigDict.Values)
            {
                list.Add(config);
            }
            return list;
        }
        public void SearchCacheDataByConfig(ResCacheConfig config, ref Dictionary<int, ResCacheData> container)
        {
            foreach (ResCacheData cacheData in m_ConfigDict.Values)
            {
                if (!container.ContainsKey(cacheData.m_Id)
                  && ((cacheData.m_CacheType & config.CacheType) != 0)
                  && (config.ResId == ResCacheConfig.S_ALL_RES || config.ResId == cacheData.m_ResId))
                {
                    container.Add(cacheData.m_Id, cacheData);
                    if (cacheData.m_LinkList != null && cacheData.m_LinkList.Count > 0)
                    {
                        SearchCacheDataRecursively(cacheData, config.LinkLimitList, ref container);
                    }
                }
            }
        }
        public List<ResCacheData> SortCacheDataList(Dictionary<int, ResCacheData> cacheDataList)
        {
            List<ResCacheData> list = new List<ResCacheData>();
            foreach (ResCacheData cacheData in cacheDataList.Values)
            {
                if ((cacheData.m_CacheType & ResCacheType.level) != 0)
                {
                    list.Add(cacheData);
                }
                else
                {
                    list.Insert(0, cacheData);
                }
            }
            return list;
        }
        private bool SearchCacheDataRecursively(ResCacheData cacheData, HashSet<int> linkLimitList,
          ref Dictionary<int, ResCacheData> container)
        {
            if (cacheData.m_LinkList != null && cacheData.m_LinkList.Count > 0)
            {
                foreach (int link in cacheData.m_LinkList)
                {
                    try
                    {
                        int resId = link;
                        if (m_ConfigDict.ContainsKey(resId) && !container.ContainsKey(resId))
                        {
                            ResCacheData childCacheData = m_ConfigDict[resId];
                            if (linkLimitList != null && !linkLimitList.Contains(childCacheData.m_ResId))
                            {
                                continue;
                            }
                            container.Add(resId, childCacheData);
                            if (childCacheData.m_LinkList != null && childCacheData.m_LinkList.Count > 0)
                            {
                                SearchCacheDataRecursively(childCacheData, null, ref container);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        string info = string.Format("ResCacheProvider.SearchCacheDataRecursively res parse failed cacheData:{0} link:{1} ex:{2}",
                          cacheData.m_Id, link, ex);
                        ResLoadHelper.Log(info);
                    }
                }
            }
            return true;
        }
    }
}

