using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUITools 
{
    public static GameObject AddChild(GameObject parent, Object prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

//#if UNITY_EDITOR && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
//        //编辑器模式 撤销对他们的创建
//        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
//#endif

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform, false);//一定要加false 要不界面就乱了
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
        Transform widgets = parent.transform.Find("Widgets");
        if (isClearWidget)
        {
            for (int i = 0; i < widgets.childCount; i++)
            {
                GameObject gameObject = widgets.GetChild(i).gameObject;
                GameObject.DestroyImmediate(gameObject);
            }
            widgets.DetachChildren();
        }
        GameObject go = prefab as GameObject;
        GameObject obj = null;
        if (go != null && parent != null)
        {
            obj = GameObject.Instantiate(go);
            Transform t = obj.transform;
            t.SetParent(widgets, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            obj.layer = parent.layer;
            go.layer = parent.layer;
        }
        return obj;
    }

    public static void SetActive(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }

    public static Sprite GetResourceSpriteByName(string name)
    {
        return Resources.Load<GameObject>("Sprite/" + name).GetComponent<SpriteRenderer>().sprite;
    }

    static public bool GetActive(GameObject go)
    {
#if UNITY_3_5
		return go && go.active;
#else
        return go && go.activeInHierarchy;//activeInHierarchy表示控件自身和父控件的active都是true, activeSelf是指自身
#endif
    }

    static public void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;
                    go.transform.parent = null;
                }

                UnityEngine.Object.Destroy(obj);
            }
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    public static void DestroyImmediate(Object obj)
    {
        if(obj != null)
        {
            if (Application.isEditor)
            {
                Object.DestroyImmediate(obj);
            }
            else
            {
                Object.Destroy(obj);
            }
        }
    }
}
