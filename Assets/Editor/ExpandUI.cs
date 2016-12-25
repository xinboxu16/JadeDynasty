using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ExpandUI : Editor
{
    [MenuItem("GameObject/UI/PolygonImage")]
    static void CreatePolygonImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(PolygonImage));
                go.GetComponent<PolygonImage>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
            }
        }
    }

    [MenuItem("GameObject/UI/UnRaycastImage")]
    static void CreateImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
            }
        }
    }

    [MenuItem("GameObject/UI/UnRaycastText")]
    static void CreateText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                go.GetComponent<Text>().raycastTarget = false;
                go.GetComponent<Text>().text = "New Text";
                go.GetComponent<Text>().color = Color.black;
                (go.GetComponent<Text>().transform as RectTransform).sizeDelta = new Vector2(160, 30);
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
            }
        }
    }
}
