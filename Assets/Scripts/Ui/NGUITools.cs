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

    public static GameObject AddWidget(GameObject parent, Object prefab, bool isClearWidget)
    {
        if (isClearWidget)
        {
            Transform widgets = parent.transform.Find("Widgets");
            for (int i = 0; i < widgets.childCount; i++)
            {
                GameObject gameObject = widgets.GetChild(i).gameObject;
                GameObject.DestroyImmediate(gameObject);
            }
            parent.transform.Find("Widgets").DetachChildren();
        }
        GameObject go = prefab as GameObject;
        if (go != null && parent != null)
        {
            Transform t = MonoBehaviour.Instantiate(go).transform;
            t.SetParent(parent.transform.Find("Widgets"), false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
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
