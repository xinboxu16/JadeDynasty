using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyGameObjectWindow : EditorWindow {

    static CopyGameObjectWindow myWindow = null;

    [MenuItem("Tools/CopyGameObjectWindow")]  
    static void Init()
    {
        myWindow = (CopyGameObjectWindow)EditorWindow.GetWindow(typeof(CopyGameObjectWindow), false, "复制对象窗口");//GetWindowWithRect
        myWindow.Show();
    }

    private string textNum = "0";

    void Start()
    {

    }

    void OnGUI()
    {
        //textNum = GUILayout.TextField(textNum);
        textNum = EditorGUILayout.TextField("输入复制数量", textNum);

        if (GUILayout.Button("粘贴"))
        {
            try
            {
                var num = int.Parse(textNum);
                if (0 == num)
                {
                    Debug.Log("num error");
                }

                var selectObj = Selection.activeObject;
                if (selectObj && selectObj.GetType() == typeof(GameObject))
                {
                    var parentObj = (selectObj as GameObject).transform.parent;
                    if (parentObj)
                    {
                        for(int i = 0; i < num; i++)
                        {
                            GameObject copyObj = (GameObject)GameObject.Instantiate(selectObj, parentObj, false);
                            copyObj.name = selectObj.name;
                        }
                        //Debug.Log(AssetDatabase.GetAssetPath(selectObj) + parentObj.name);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("Parse failed:{1}\n{2}", ex.Message, ex.StackTrace));
            }
        }

        if (GUILayout.Button("关闭窗口"))
        {
            myWindow.Close();
        }
    }
}
