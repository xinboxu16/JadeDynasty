using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUITools 
{
    //isInstantiate是否需要实例化
    public static GameObject AddChild(GameObject parent, Object prefab, bool isInstantiate = true)
    {
        GameObject go = isInstantiate ? GameObject.Instantiate(prefab, parent.transform, false) as GameObject : prefab as GameObject;

//#if UNITY_EDITOR && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
//        //编辑器模式 撤销对他们的创建
//        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
//#endif

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            //t.SetParent(parent.transform, false);//一定要加false 要不界面就乱了
            /*
             * t.localPosition = new Vector3(0,0,0)是将A的中心重合到画布中心了。
             * 如果设置要添加的控件位置不在画布中心 导致直接放在画布中心 位置错误
             */
            if(!isInstantiate)
            {
                t.SetParent(parent.transform, false);
            }
            //t.localPosition = Vector3.zero; 
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

    public static GameObject AddWidget(GameObject parent, Object prefab, bool isClearWidget = false)
    {
        //GameObject parent = GameObject.FindGameObjectWithTag("UI_BASE");
        Transform widgets = parent.transform;
        if (null == widgets)
        {
            return null;
        }
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
        if (go != null && parent != null)
        {
            go = GameObject.Instantiate(go, widgets, false);
            Transform t = go.transform;
            //t.SetParent(widgets, false);
            //t.localPosition = Vector3.zero; //如果设置要添加的控件位置不在画布中心 导致直接放在画布中心 位置错误
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
            //go.layer = parent.layer;
        }
        return go;
    }

    public static void SetActive(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }

    public static Sprite GetResourceSpriteByName(string name)
    {
        GameObject obj = Resources.Load<GameObject>("Sprite/" + name);
        if(null != obj)
        {
            return obj.GetComponent<SpriteRenderer>().sprite;
        }
        return null;
    }

    //this修饰参数作用https://zhidao.baidu.com/question/263333215940019885.html
    //public static Vector2 TransformToCanvasLocalPosition(this Transform current, Canvas canvas)
    public static Vector2 TransformToLocalPosition(Transform node, RectTransform localRect, Canvas canvas)
    {
        Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(node.transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(localRect, screenPos, canvas.worldCamera, out localPos);
        return localPos;
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
