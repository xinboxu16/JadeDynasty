using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace DashFire
{
    public class AssetEx
    {
        public AssetBundle AssetBundleRef = null;
        public int AssetBundleRefCount = 0;

        public UnityEngine.Object AssetRef = null;
        public ResCacheType AssetRefType = ResCacheType.none;

        public ResVersionData ResInfoRef = null;

        private bool IsStartCache = false;

        #region Properties
        public int Id
        {
            get
            {
                if (ResInfoRef != null)
                {
                    return ResInfoRef.GetId();
                }
                return -1;
            }
        }
        public string AssetName
        {
            get
            {
                if (ResInfoRef != null)
                {
                    return ResInfoRef.m_AssetName;
                }
                return string.Empty;
            }
        }
        public string AssetBundleName
        {
            get
            {
                if (ResInfoRef != null)
                {
                    return ResInfoRef.m_AssetBundleName;
                }
                return string.Empty;
            }
        }
        public List<int> DependList
        {
            get
            {
                if (ResInfoRef != null)
                {
                    return ResInfoRef.m_Depends;
                }
                return new List<int>();
            }
        }
        public bool IsCached
        {
            get
            {
                return AssetBundleRef != null || AssetRef != null;
            }
        }
        public bool IsAssetBundleCached
        {
            get
            {
                return AssetBundleRef != null;
            }
        }
        public bool IsAssetCached
        {
            get
            {
                return AssetRef != null;
            }
        }
        public bool IsAllAssetBundleCached
        {
            get
            {
                if (DependList != null && DependList.Count > 0)
                {
                    foreach (int dependId in DependList)
                    {
                        AssetEx dependAsset = AssetExManager.Instance.GetAsset(dependId);
                        if (dependAsset.AssetBundleRef == null)
                        {
                            return false;
                        }
                    }
                }
                return AssetBundleRef != null;
            }
        }
        public string WWWUrl
        {
            get
            {
                return ResLoadHelper.GetLocalResABURL(AssetBundleName);
            }
        }
        public bool IsBuildIn
        {
            get
            {
                return ResLoadHelper.IsABBuiltIn(AssetBundleName);
            }
        }
        public bool IsLevelAsset
        {
            get
            {
                return (AssetRefType & ResCacheType.level) != 0;
            }
        }
        #endregion
        #region API
        public void Init(ResVersionData resInfo)
        {
            ResInfoRef = resInfo;
        }
        public void Clear()
        {
            AssetRefType = ResCacheType.none;
            AssetBundleRefCount = 0;
            if (AssetBundleRef != null)
            {
                AssetBundleRef.Unload(false);
                AssetBundleRef = null;
            }
            if (AssetRef != null)
            {
                AssetRef = null;
            }
        }
        public void IncAssetBundleRefCount(bool depend = true)
        {
            AssetBundleRefCount++;
            if (depend && this.DependList != null && this.DependList.Count > 0)
            {
                foreach (int dependId in this.DependList)
                {
                    AssetEx assetDepend = AssetExManager.Instance.GetAsset(dependId);
                    if (assetDepend != null)
                    {
                        assetDepend.IncAssetBundleRefCount(false);
                    }
                }
            }
        }
        public void DesAssetBundleRefCount(bool depend = true)
        {
            AssetBundleRefCount--;
            //ResLoadHelper.Log("DesAssetBundleRefCount:" + this.ToString());

            if (AssetBundleRefCount <= 0)
            {
                AssetBundleRefCount = 0;
                ReleaseAssetBundle(false, false);
            }
            if (depend && this.DependList != null && this.DependList.Count > 0)
            {
                foreach (int dependId in this.DependList)
                {
                    AssetEx assetDepend = AssetExManager.Instance.GetAsset(dependId);
                    if (assetDepend != null)
                    {
                        assetDepend.DesAssetBundleRefCount(false);
                    }
                }
            }
        }
        public ResAsyncInfo CacheAssetBundle(bool depend = true)
        {
            ResAsyncInfo info = new ResAsyncInfo();
            info.CurCoroutine = CoroutineInsManager.Instance.StartCoroutine(CacheAssetBundleExAsync(info, depend));
            info.Tip = AssetBundleName;
            return info;
        }
        public void ReleaseAssetBundle(bool depend, bool force = false)
        {
            AssetBundleRefCount = 0;
            if (AssetBundleRef != null)
            {
                //ResLoadHelper.Log("ReleaseAssetBundle:" + this.ToString());
                AssetBundleRef.Unload(false);
                AssetBundleRef = null;
            }
            if (depend)
            {
                foreach (int dependId in DependList)
                {
                    AssetEx dependAssetEx = AssetExManager.Instance.GetAsset(dependId);
                    if (dependAssetEx != null)
                    {
                        dependAssetEx.ReleaseAssetBundle(false, force);
                    }
                }
            }
        }
        public void ExtractAsset()
        {
            try
            {
                if (AssetBundleRef == null)
                {
                    return;
                }
                AssetRef = AssetBundleRef.LoadAsset(AssetName);
            }
            catch (OutOfMemoryException exception)
            {
                ResLoadHelper.ErrLog(exception.ToString());
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            catch (Exception exception2)
            {
                ResLoadHelper.ErrLog("AssetEx Load Exception: " + exception2.ToString());
            }
        }
        public void ReleaseAsset(ResCacheType cacheType)
        {
            AssetRefType &= ~cacheType;
            if (AssetRefType == ResCacheType.none && AssetRef != null)
            {
                AssetRef = null;
            }
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Id:" + Id);
            sb.Append(" AssetName:" + AssetName);
            sb.Append(" AssetBundleName:" + AssetBundleName);
            sb.Append(" RefType:" + AssetRefType);
            sb.Append(" AssetBundleRef:" + AssetBundleRef);
            sb.Append(" AssetBundleRefCount:" + AssetBundleRefCount);
            return sb.ToString();
        }
        #endregion
        #region Enumator
        internal IEnumerator CacheAssetBundleExAsync(ResAsyncInfo info, bool depend)
        {
            if (!IsAssetBundleCached)
            {
                string url = WWWUrl;
                //ResLoadHelper.Log("LoadAssetBundle start abCacheInfo:" + this.ToString());
                if (string.IsNullOrEmpty(url))
                {
                    ResLoadHelper.Log("AssetBundleManager.LoadAssetBundle invalid url:" + url);
                    yield break;
                }
                if (IsBuildIn)
                {
                    using (WWW tWWW = new WWW(url))
                    {
                        yield return tWWW;
                        try
                        {
                            if (tWWW.error != null)
                            {
                                ResLoadHelper.Log("AssetBundleManager.LoadAssetBundle BuildIn not cached url:" + url);
                                tWWW.Dispose();
                                info.IsError = true;
                                yield break;
                            }
                            AssetBundle assetBundleObj = tWWW.assetBundle;
                            if (assetBundleObj == null)
                            {
                                ResLoadHelper.Log("AssetBundleManager.LoadAssetBundle assetBundleObj null url:" + url);
                                tWWW.Dispose();
                                info.IsError = true;
                                yield break;
                            }
                            AssetBundleRef = assetBundleObj;
                        }
                        catch (System.Exception ex)
                        {
                            ResLoadHelper.Log("AssetBundleManager.LoadAssetBundle res failed url:" + url + " ex:" + ex);
                            info.IsError = true;
                        }
                        finally
                        {
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
                            ResLoadHelper.Log("AssetBundleManager.LoadAssetBundle bytes null url:" + url);
                            info.IsError = true;
                            yield break;
                        }
                        AssetBundleCreateRequest abRequest = AssetBundle.LoadFromMemoryAsync(buffer);
                        yield return abRequest;
                        try
                        {
                            AssetBundleRef = abRequest.assetBundle;
                        }
                        catch (System.Exception ex)
                        {
                            ResLoadHelper.Log("LoadAssetBundle res failed url:" + url + " ex:" + ex);
                            info.IsError = true;
                        }
                        finally
                        {
                            if (abRequest != null)
                            {
                                abRequest = null;
                            }
                        }
                    }
                }
            }
            //ResLoadHelper.Log("LoadAssetBundle done abCacheInfo:" + this.ToString());

            info.IsDone = true;
            info.Progress = 1.0f;
        }
        #endregion
    };
}