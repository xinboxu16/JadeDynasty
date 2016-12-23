using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUITools 
{
    public static GameObject AddChild(GameObject parent, Object prefab)
    {
        GameObject go = prefab as GameObject;
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
        //GameObject parentGameObject = parentObject as GameObject;
        //Object obj = Object.Instantiate(childObject);
        //GameObject childGameObject = obj as GameObject;
        //childGameObject.transform.parent = parentGameObject.transform;
        //childGameObject.transform.localPosition = Vector3.zero;
        //childGameObject.transform.localScale = Vector3.zero;
        ////(childObject as GameObject).transform.parent = (parentObject as GameObject).transform;
        //return childGameObject;
    }

    public static void SetActive(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }

    public static void DestroyImmediate(Object nbm)
    {
        Object.DestroyImmediate(nbm);
        nbm = null;
    }
}
