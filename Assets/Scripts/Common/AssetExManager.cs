using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DashFire
{
    public class AsyncCmd
    {
        public AssetEx m_TargetAsset;
        public List<AsynCmdDoneHandler> m_HandlerList = new List<AsynCmdDoneHandler>();
        public void RegisterHandler(AsynCmdDoneHandler handler)
        {
            //ResLoadHelper.Log("RegisterHandler:");
            m_HandlerList.Add(handler);
        }
        public void Clear()
        {
            m_HandlerList.Clear();
            //m_TargetAsset = null;
        }
        public void OnCmdDone()
        {
            //ResLoadHelper.Log("OnCmdDone:" + m_TargetAsset.ToString());
            foreach (AsynCmdDoneHandler handler in m_HandlerList)
            {
                handler.Execute(m_TargetAsset);
            }
            Clear();
        }
        public virtual void Execute() { }
        public virtual bool IsDone() { return true; }
    }

    public class CacheCmd : AsyncCmd
    {
        public bool m_Depend = false;
        public ResAsyncInfo m_AsynInfo = null;
        public override void Execute()
        {
            if (!IsDone())
            {
                m_AsynInfo = m_TargetAsset.CacheAssetBundle(m_Depend);
            }
        }
        public override bool IsDone()
        {
            if (m_Depend)
            {
                return m_TargetAsset.IsAllAssetBundleCached;
            }
            else
            {
                return m_TargetAsset.IsAssetBundleCached;
            }
        }
        public bool IsError()
        {
            return (m_AsynInfo != null) && m_AsynInfo.IsError;
        }
    }

    public class ExtractCmd : AsyncCmd
    {
        public override void Execute()
        {
            if (!IsDone())
            {
                m_TargetAsset.ExtractAsset();
                m_TargetAsset.DesAssetBundleRefCount(true);
            }
        }
        public override bool IsDone()
        {
            if (m_TargetAsset.IsAssetCached)
            {
                return true;
            }
            return false;
        }
    }

    //partial局部的类
    public partial class AssetExManager
    {
        #region Singleton
        private static AssetExManager s_Instance = new AssetExManager();
        public static AssetExManager Instance
        {
            get { return s_Instance; }
        }
        #endregion

        public Dictionary<int, AssetEx> m_AssetExDict { get; private set; }
        public Dictionary<int, CacheCmd> m_CacheCmdDict;
        private HashSet<int> m_CurCacheCmdSet;

        public Dictionary<int, ExtractCmd> m_ExtractCmdDict;
        private HashSet<int> m_CurExtractCmdSet;

        private List<int> m_DoneCmd;

        private AssetExManager()
        {
            m_AssetExDict = new Dictionary<int, AssetEx>();
            m_CacheCmdDict = new Dictionary<int, CacheCmd>();
            m_CurCacheCmdSet = new HashSet<int>();

            m_ExtractCmdDict = new Dictionary<int, ExtractCmd>();
            m_CurExtractCmdSet = new HashSet<int>();

            m_DoneCmd = new List<int>();
        }
        public bool InitAllAssetEx()
        {
            Cleanup();
            m_AssetExDict.Clear();
            Dictionary<int, ResVersionData> resInfoDict = ResVersionProvider.Instance.GetData();
            foreach (ResVersionData resInfo in resInfoDict.Values)
            {
                if (!m_AssetExDict.ContainsKey(resInfo.GetId()))
                {
                    AssetEx assetEx = new AssetEx();
                    assetEx.Init(resInfo);
                    m_AssetExDict.Add(assetEx.Id, assetEx);
                }
            }
            //CoroutineInsManager.Instance.StartCoroutine(Update());
            return true;
        }
        public void Cleanup()
        {
            if (m_CacheCmdDict.Count > 0)
            {
                foreach (CacheCmd cmd in m_CacheCmdDict.Values)
                {
                    cmd.Clear();
                }
                m_CacheCmdDict.Clear();
            }
            if (m_ExtractCmdDict.Count > 0)
            {
                foreach (ExtractCmd cmd in m_ExtractCmdDict.Values)
                {
                    cmd.Clear();
                }
                m_ExtractCmdDict.Clear();
            }
            m_CurCacheCmdSet.Clear();
            m_CurExtractCmdSet.Clear();

            ClearAllAssetEx();
        }
        public void Update()
        {
            if (!GlobalVariables.Instance.IsPublish)
            {
                return;
            }

            //while (true) {
            // Cache
            if (m_CacheCmdDict != null && m_CacheCmdDict.Count > 0)
            {
                foreach (CacheCmd cmd in m_CacheCmdDict.Values)
                {
                    if (m_CurCacheCmdSet.Count < ResUpdateControler.s_AsyncCoroutineMax)
                    {
                        if (!m_CurCacheCmdSet.Contains(cmd.m_TargetAsset.Id))
                        {
                            m_CurCacheCmdSet.Add(cmd.m_TargetAsset.Id);
                            cmd.Execute();
                            //ResLoadHelper.Log("AssetExManager.m_CurCacheCmdSet.Count:" + m_CurCacheCmdSet.Count);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (m_CurCacheCmdSet != null && m_CurCacheCmdSet.Count > 0)
            {
                m_DoneCmd.Clear();
                foreach (int cmdId in m_CurCacheCmdSet)
                {
                    CacheCmd cmd = m_CacheCmdDict[cmdId];
                    if (cmd != null && cmd.IsDone())
                    {
                        cmd.OnCmdDone();
                        m_DoneCmd.Add(cmdId);
                        //ResLoadHelper.Log("AssetExManager.CacheCmd.Done:" + cmd.m_TargetAsset.ToString());
                    }
                }
                if (m_DoneCmd.Count > 0)
                {
                    foreach (int cmdId in m_DoneCmd)
                    {
                        m_CurCacheCmdSet.Remove(cmdId);
                        m_CacheCmdDict.Remove(cmdId);
                    }
                    m_DoneCmd.Clear();
                }
            }

            // Extract
            if (m_ExtractCmdDict != null && m_ExtractCmdDict.Count > 0)
            {
                m_CurExtractCmdSet.Clear();
                foreach (ExtractCmd cmd in m_ExtractCmdDict.Values)
                {
                    if (cmd.m_TargetAsset.IsAllAssetBundleCached)
                    {
                        m_CurExtractCmdSet.Add(cmd.m_TargetAsset.Id);
                        cmd.Execute();
                        cmd.OnCmdDone();
                    }
                }
                if (m_CurExtractCmdSet.Count > 0)
                {
                    foreach (int cmdId in m_CurExtractCmdSet)
                    {
                        m_ExtractCmdDict.Remove(cmdId);
                    }
                    m_CurExtractCmdSet.Clear();
                }
            }

            //yield return 1;
            //}
        }

        public CacheCmd AddCacheCmd(int assetId, bool depend = true)
        {
            CacheCmd cmd = null;
            AssetEx asset = GetAsset(assetId);
            if (asset == null)
            {
                return null;
            }
            if (!m_CacheCmdDict.ContainsKey(assetId))
            {
                cmd = new CacheCmd();
                cmd.m_TargetAsset = asset;
                cmd.m_Depend = depend;
                m_CacheCmdDict.Add(assetId, cmd);
                if (depend && asset.DependList.Count > 0)
                {
                    foreach (int dependId in asset.DependList)
                    {
                        AddCacheCmd(dependId, false);
                    }
                }
            }
            else
            {
                cmd = m_CacheCmdDict[assetId];
            }
            return cmd;
        }

        public ExtractCmd AddExtractCmd(int assetId)
        {
            ExtractCmd cmd = null;
            AssetEx asset = GetAsset(assetId);
            if (asset == null)
            {
                return null;
            }
            if (!m_ExtractCmdDict.ContainsKey(assetId))
            {
                if (!asset.IsAllAssetBundleCached)
                {
                    AddCacheCmd(assetId, true);
                }
                cmd = new ExtractCmd();
                cmd.m_TargetAsset = asset;
                cmd.m_TargetAsset.IncAssetBundleRefCount(true);
                m_ExtractCmdDict.Add(assetId, cmd);
            }
            else
            {
                cmd = m_ExtractCmdDict[assetId];
            }
            return cmd;
        }

        public bool ClearAllAssetEx()
        {
            foreach (AssetEx assetEx in m_AssetExDict.Values)
            {
                assetEx.Clear();
            }
            return true;
        }
        public void ClearAllAssetBundle()
        {
            foreach (AssetEx assetEx in m_AssetExDict.Values)
            {
                assetEx.ReleaseAssetBundle(false, true);
            }
        }
        public void ReleaseAssetExs(ResCacheType cacheType)
        {
            foreach (AssetEx assetEx in m_AssetExDict.Values)
            {
                if (((assetEx.AssetRefType & cacheType) != 0))
                {
                    assetEx.ReleaseAsset(cacheType);
                }
            }
        }

        public AssetEx GetAsset(int id)
        {
            if (m_AssetExDict.ContainsKey(id))
            {
                return m_AssetExDict[id];
            }
            return null;
        }
        public AssetEx GetAssetByName(string assetNameWithoutExtention)
        {
            if (string.IsNullOrEmpty(assetNameWithoutExtention))
            {
                return null;
            }

            assetNameWithoutExtention = assetNameWithoutExtention.ToLower();
            assetNameWithoutExtention = ResLoadHelper.ConvertResourceAssetPathToAbs(assetNameWithoutExtention);
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                if (asset.AssetName.StartsWith(assetNameWithoutExtention))
                {
                    return asset;
                }
            }
            return null;
        }
        public List<AssetEx> GetAsset(IEnumerable<int> idList)
        {
            List<AssetEx> assetList = new List<AssetEx>();
            foreach (int assetId in idList)
            {
                AssetEx asset = GetAsset(assetId);
                if (asset != null)
                {
                    assetList.Add(asset);
                }
            }
            return assetList;
        }
        public List<int> GetAsset(IEnumerable<AssetEx> assetList)
        {
            List<int> assetIdList = new List<int>();
            foreach (AssetEx asset in assetList)
            {
                if (asset != null)
                {
                    assetIdList.Add(asset.Id);
                }
            }
            return assetIdList;
        }
        public List<int> GetAssetDependency(int id)
        {
            List<int> retAssetList = new List<int>();
            retAssetList.Add(id);
            AssetEx asset = GetAsset(id);
            if (asset != null)
            {
                retAssetList.AddRange(asset.DependList);
            }
            return retAssetList;
        }
        public List<int> CombileAsset(List<int> unionWith, List<int> exceptWith, bool depend)
        {
            List<int> dependList = null;
            HashSet<int> toCombileAssetEx = new HashSet<int>();
            if (unionWith != null && unionWith.Count > 0)
            {
                toCombileAssetEx.UnionWith(unionWith);
                if (depend)
                {
                    foreach (int assetId in unionWith)
                    {
                        dependList = GetAssetDependency(assetId);
                        toCombileAssetEx.UnionWith(dependList);
                    }
                }
            }
            HashSet<int> toKeepAssetEx = new HashSet<int>();
            if (exceptWith != null && exceptWith.Count > 0)
            {
                toKeepAssetEx.UnionWith(exceptWith);
                if (depend)
                {
                    foreach (int assetId in exceptWith)
                    {
                        dependList = GetAssetDependency(assetId);
                        toKeepAssetEx.UnionWith(dependList);
                    }
                }
            }
            toCombileAssetEx.ExceptWith(toKeepAssetEx);
            return new List<int>(toCombileAssetEx);
        }
        public UnityEngine.Object GetAssetByNameWithoutExtention(string assetNameWithoutExtention)
        {
            if (string.IsNullOrEmpty(assetNameWithoutExtention))
            {
                return null;
            }

            assetNameWithoutExtention = assetNameWithoutExtention.ToLower();
            assetNameWithoutExtention = ResLoadHelper.ConvertResourceAssetPathToAbs(assetNameWithoutExtention);
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                if (asset.AssetName.StartsWith(assetNameWithoutExtention))
                {
                    if (asset.AssetRef == null)
                    {
                        ResLoadHelper.Log("GetAssetByNameWithoutExtention failed:" + assetNameWithoutExtention);
                    }
                    return asset.AssetRef;
                }
            }
            return null;
        }
        public bool SearchByCacheData(ResCacheData cacheData,
          ref List<AssetEx> assetList, ref List<AssetEx> assetCacheList)
        {
            HashSet<int> assetContainer = new HashSet<int>();
            HashSet<int> assetCacheSet = new HashSet<int>();
            bool isLevelCache = ((cacheData.m_CacheType & ResCacheType.level) != 0);
            assetContainer.UnionWith(cacheData.m_Assets);
            IncAssetBundleRefCount(cacheData.m_Assets);
            if (!isLevelCache)
            {
                assetCacheSet.UnionWith(cacheData.m_Assets);
            }
            foreach (int assetId in cacheData.m_Assets)
            {
                AssetEx assetEx = GetAsset(assetId);
                if (assetEx != null)
                {
                    assetContainer.UnionWith(assetEx.DependList);
                }
            }
            assetList.AddRange(AssetExManager.Instance.GetAsset(assetContainer));
            assetCacheList.AddRange(AssetExManager.Instance.GetAsset(assetCacheSet));
            return true;
        }
        public bool SearchByCacheDataList(List<ResCacheData> cacheDataList,
         ref List<AssetEx> assetList, ref List<AssetEx> assetCacheList)
        {
            HashSet<int> assetContainer = new HashSet<int>();
            HashSet<int> assetCacheSet = new HashSet<int>();
            foreach (ResCacheData cacheData in cacheDataList)
            {
                bool isLevelCache = ((cacheData.m_CacheType & ResCacheType.level) != 0);
                assetContainer.UnionWith(cacheData.m_Assets);
                if (!isLevelCache)
                {
                    assetCacheSet.UnionWith(cacheData.m_Assets);
                    AppendAssetExAssetRefType(cacheData.m_Assets, cacheData.m_CacheType);
                }
                foreach (int assetId in cacheData.m_Assets)
                {
                    AssetEx assetEx = GetAsset(assetId);
                    if (assetEx != null)
                    {
                        assetContainer.UnionWith(assetEx.DependList);
                    }
                }
            }
            assetList.AddRange(AssetExManager.Instance.GetAsset(assetContainer));
            assetCacheList.AddRange(AssetExManager.Instance.GetAsset(assetCacheSet));
            return true;
        }
        public void IncAssetBundleRefCount(IEnumerable<int> idList)
        {
            foreach (int assetId in idList)
            {
                AssetEx asset = GetAsset(assetId);
                if (asset != null)
                {
                    asset.IncAssetBundleRefCount(true);
                }
            }
        }
        public void AppendAssetExAssetRefType(IEnumerable<int> idList, ResCacheType cacheType)
        {
            foreach (int assetId in idList)
            {
                AssetEx asset = GetAsset(assetId);
                if (asset != null)
                {
                    asset.AssetRefType |= cacheType;
                }
            }
        }
        public string DumpAssetInfoWithAssetRef()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DumpAssetInfoWithAssetRef:");
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                if (asset.AssetRef != null)
                {
                    sb.AppendFormat("AssetEx:{0}\n", asset.ToString());
                }
            }
            return sb.ToString();
        }
        public string DumpAssetInfoWithAssetBundle()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DumpAssetInfoWithAssetBundle:");
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                if (asset.AssetBundleRef != null)
                {
                    sb.AppendFormat("AssetEx:{0}\n", asset.ToString());
                }
            }
            return sb.ToString();
        }
        public string DumpAssetInfoWithAssetBundleRefCount()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DumpAssetInfoWithAssetBundleRefCount:");
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                if (asset.AssetBundleRefCount > 0)
                {
                    sb.AppendFormat("AssetEx:{0}\n", asset.ToString());
                }
            }
            return sb.ToString();
        }
        public string DumpSpecificAssetInfo(List<AssetEx> assetList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DumpSpecificAssetInfo:" + assetList.Count);
            foreach (AssetEx asset in assetList)
            {
                sb.AppendFormat("AssetEx:{0}\n", asset.ToString());
            }
            return sb.ToString();
        }
        public string DumpAllAssetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DumpAllAssetInfo:" + m_AssetExDict.Count);
            foreach (AssetEx asset in m_AssetExDict.Values)
            {
                //sb.AppendFormat("AssetEx:{0}\n", asset.ToString());
            }
            return sb.ToString();
        }
        public void OutputAssetExManagerInfo()
        {
            ResLoadHelper.Log(DumpAssetInfoWithAssetRef());
            ResLoadHelper.Log(DumpAssetInfoWithAssetBundle());
            ResLoadHelper.Log(DumpAssetInfoWithAssetBundleRefCount());
            ResLoadHelper.Log(DumpAllAssetInfo());
        }
    }
}
