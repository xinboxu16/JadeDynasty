using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MakeAtlasPrefab : Editor
{
    [MenuItem("Tools/AtalMakerPrefab")]
    static private void MakeAtlas()
    {
        string spriteDir = Application.dataPath + "/Resources/Sprite";

        if(!Directory.Exists(spriteDir))
        {
            Directory.CreateDirectory(spriteDir);
        }

        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Textures");
        foreach(DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
        {
            foreach (FileInfo pngFile in dirInfo.GetFiles())
            {
                if(pngFile.Name.EndsWith(".png") || pngFile.Name.EndsWith(".jpg"))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Textures"));
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    GameObject go = new GameObject(sprite.name);
                    go.AddComponent<SpriteRenderer>().sprite = sprite;
                    allPath = spriteDir + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    GameObject.DestroyImmediate(go);

                    //加载时
                    //Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
    }

//    [MenuItem ("MyMenu/Build Assetbundle")]
//    static private void BuildAssetBundle()
//    {
//        string dir = Application.dataPath +"/StreamingAssets";
 
//        if(!Directory.Exists(dir)){
//            Directory.CreateDirectory(dir);
//        }
//        DirectoryInfo rootDirInfo = new DirectoryInfo (Application.dataPath +"/Atlas");
//        foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories()) {
//            List<Sprite> assets = new List<Sprite>();
//            string path = dir +"/"+dirInfo.Name+".assetbundle";
//            foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories)) 
//            {
//                string allPath = pngFile.FullName;
//                string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
//                assets.Add(Resources.LoadAssetAtPath<Sprite>(assetPath));
//            }
//            if(BuildPipeline.BuildAssetBundle(null, assets.ToArray(), path,BuildAssetBundleOptions.UncompressedAssetBundle| BuildAssetBundleOptions.CollectDependencies, GetBuildTarget())){
//            }
//        }	
//    }
//        static private BuildTarget GetBuildTarget()
//        {
//            BuildTarget target = BuildTarget.WebPlayer;
//#if UNITY_STANDALONE
//            target = BuildTarget.StandaloneWindows;
//#elif UNITY_IPHONE
//            target = BuildTarget.iPhone;
//#elif UNITY_ANDROID
//            target = BuildTarget.Android;
//#endif
//            return target;
//        }
}

//AssetBundle使用
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
 
//public class UIMain : MonoBehaviour {
 
//    AssetBundle assetbundle = null;
//    void Start () 
//    {
//        CreatImage(loadSprite("image0"));
//        CreatImage(loadSprite("image1"));
//    }
 
//    private void CreatImage(Sprite sprite ){
//        GameObject go = new GameObject(sprite.name);
//        go.layer = LayerMask.NameToLayer("UI");
//        go.transform.parent = transform;
//        go.transform.localScale= Vector3.one;
//        Image image = go.AddComponent<Image>();
//        image.sprite = sprite;
//        image.SetNativeSize();
//    }
 
//    private Sprite loadSprite(string spriteName){
//#if USE_ASSETBUNDLE
//        if(assetbundle == null)
//            assetbundle = AssetBundle.CreateFromFile(Application.streamingAssetsPath +"/Main.assetbundle");
//                return assetbundle.Load(spriteName) as Sprite;
//#else
//        return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
//#endif	
//    }
 
//}