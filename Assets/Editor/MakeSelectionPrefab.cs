using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MakeSelectionPrefab : Editor
{
    [MenuItem("Assets/MakeSelection")]
    static private void MakeSelection()
    {
        string spriteDir = Application.dataPath + "/Resources/Sprite";
        if (!Directory.Exists(spriteDir))
        {
            Directory.CreateDirectory(spriteDir);
        }
        Object obj = Selection.activeObject;
        if (obj != null && obj.GetType() == typeof(Texture2D))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if(path.EndsWith(".png") || path.EndsWith(".jpg"))
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                path = spriteDir + "/" + sprite.name + ".prefab";
                string prefabPath = path.Substring(path.IndexOf("Assets"));
                PrefabUtility.CreatePrefab(prefabPath, go);
                GameObject.DestroyImmediate(go);
                Debug.Log("MakeSelection_Success");
            }
        }
    }
}
