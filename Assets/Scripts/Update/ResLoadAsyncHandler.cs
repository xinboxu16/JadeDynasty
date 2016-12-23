using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DashFire
{
    public interface AsynCmdDoneHandler
    {
        void Execute(AssetEx asset);
    }

    public class AssetLoadingProgressHandler : AsynCmdDoneHandler
    {
        public int TotalNum = 0;
        public int CurNum = 0;
        public void Execute(AssetEx asset)
        {
            CurNum++;
            //string tip = string.Format("加载缓存资源 {0}/{1}", CurNum, TotalNum);
            float progress = (float)CurNum / TotalNum;
            ResUpdateControler.OnUpdateProgress(progress);
        }
        public bool IsDone()
        {
            return CurNum >= TotalNum;
        }
    }
    public class AssetLoadItemHandler : AsynCmdDoneHandler
    {
        public Image UITextureObj;
        public void Execute(AssetEx asset)
        {
            if (asset.AssetRef == null)
            {
                asset.AssetRefType |= ResCacheType.async;
                asset.ExtractAsset();
            }
            if (UITextureObj != null && asset.AssetRef != null)
            {
                UITextureObj.material.mainTexture = asset.AssetRef as Texture;
            }
            asset.DesAssetBundleRefCount(true);
        }
    }

    public class ResLoadAsyncHandler
    {
        public static void LoadAsyncItem(string targetAsset, Image uiTextureObj)
        {
            if (!GlobalVariables.Instance.IsPublish)
            {
                return;
            }
            if (string.IsNullOrEmpty(targetAsset))
            {
                return;
            }
            AssetEx assetEx = AssetExManager.Instance.GetAssetByName(targetAsset);
            if (assetEx != null)
            {
                AssetLoadItemHandler handler = new AssetLoadItemHandler();
                handler.UITextureObj = uiTextureObj;
                ExtractCmd cmd = AssetExManager.Instance.AddExtractCmd(assetEx.Id);
                cmd.RegisterHandler(handler);
            }
        }
    }
}
