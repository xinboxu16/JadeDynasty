using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType : int
{
    DontLoad = -1,
    NoneActive = 0,
    Active = 1
}

public enum UISceneType
{
    MainCopy = -1,
    MainCyty = 1,
    Login = 0,
    Common = 2,
    //Other = "Other" 枚举不允许定义string
}

public class UIManager
{
    public delegate void VoidDelegate();
    public VoidDelegate OnAllUiLoadedDelegate;

    private GameObject m_RootWindow = null;
    public bool IsUIVisible = true;
    MyDictionary<int, object> uiConfigDataDic = new MyDictionary<int, object>();
    private List<string> m_ExclusionWindow = new List<string>();
    private Dictionary<string, GameObject> m_VisibleWindow = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_UnVisibleWindow = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_IsLoadedWindow = new Dictionary<string, GameObject>();

    public void Init(GameObject rootWindow)
    {
        LogicSystem.EventChannelForGfx.Subscribe<string>("show_ui", "ui", ShowWindowByName);
        LogicSystem.EventChannelForGfx.Subscribe<string>("hide_ui", "ui", HideWindowByName);
        m_RootWindow = rootWindow;
        uiConfigDataDic = UiConfigProvider.Instance.GetData();
    }

    public UiConfig GetUiConfigByName(string name)
    {
        foreach (UiConfig uiCfg in uiConfigDataDic.Values)
        {
            if (uiCfg != null && uiCfg.m_WindowName == name)
            {
                return uiCfg;
            }
        }
        return null;
    }

    //打开之前关闭的窗口
    public void OpenExclusionWindow(string windowName)
    {
        foreach(string name in m_ExclusionWindow)
        {
            ShowWindow(name, false);
        }
        m_ExclusionWindow.Clear();

        //未实现
        //if (DFMUiRoot.InputMode == InputType.Joystick)
        //{
        //    JoyStickInputProvider.JoyStickEnable = true;
        //}
    }

    //关闭除windowName之外的所有窗口
    public void CloseExclusionWindow(string windowName)
    {
        foreach (string name in m_VisibleWindow.Keys)
        {
            if (name != windowName)
            {
                m_ExclusionWindow.Add(name);
            }
        }

        foreach (string name in m_ExclusionWindow)
        {
            HideWindow(name, false);
        }

        //未实现
        //if (DFMUiRoot.InputMode == InputType.Joystick)
        //{
        //    JoyStickInputProvider.JoyStickEnable = false;
        //}
    }

    private void ShowWindow(string windowName, bool isCloseExclusion = true)
    {
        try
        {
            if (windowName == null) return;
            if (m_VisibleWindow.ContainsKey(windowName))
                return;
            if (m_UnVisibleWindow.ContainsKey(windowName))
            {
                GameObject window = m_UnVisibleWindow[windowName];
                if (null != window)
                {
                    NGUITools.SetActive(window, true);
                    m_VisibleWindow.Add(windowName, window);
                    m_UnVisibleWindow.Remove(windowName);
                }
            }
            UiConfig uiCfg = GetUiConfigByName(windowName);
            if (uiCfg != null && uiCfg.m_IsExclusion == true && isCloseExclusion)
            {
                CloseExclusionWindow(windowName);
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowWindowByName(string windowName)
    {
        ShowWindow(windowName);
    }

    private void HideWindow(string windowName, bool isOpenExclusion = true)
    {
        try
        {
            if (windowName == null) return;
            if (m_UnVisibleWindow.ContainsKey(windowName))
                return;

            if (m_VisibleWindow.ContainsKey(windowName))
            {
                GameObject window = m_VisibleWindow[windowName];
                if (null != window)
                {
                    NGUITools.SetActive(window, false);
                    m_UnVisibleWindow.Add(windowName, window);
                    m_VisibleWindow.Remove(windowName);
                }
            }
            UiConfig uiCfg = GetUiConfigByName(windowName);
            if (uiCfg != null && uiCfg.m_IsExclusion == true && isOpenExclusion)
            {
                OpenExclusionWindow(windowName);//打开之前关闭的窗口
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void HideWindowByName(string windowName)
    {
        HideWindow(windowName);
    }

    public void LoadAllWindows(int sceneType, Camera cam)
    {
        if (null == m_RootWindow)
            return;
        foreach (UiConfig info in uiConfigDataDic.Values)
        {
            if (info.m_ShowType != (int)(UIType.DontLoad) && sceneType == info.m_OwnToSceneId)
            {
                //Debug.Log(info.m_WindowName);
                GameObject go = ResourceSystem.GetSharedResource(info.m_WindowPath) as GameObject;
                if (go == null)
                {
                    Debug.LogWarning("!!!Load ui " + info.m_WindowPath + " failed.");
                    continue;
                }
                GameObject child = NGUITools.AddChild(m_RootWindow, go);
                if (info.m_ShowType == (int)(UIType.Active))
                {
                    NGUITools.SetActive(child, true);
                    if (!m_VisibleWindow.ContainsKey(info.m_WindowName))
                    {
                        m_VisibleWindow.Add(info.m_WindowName, child);
                    }
                }
                else
                {
                    NGUITools.SetActive(child, false);
                    if (!m_UnVisibleWindow.ContainsKey(info.m_WindowName))
                    {
                        m_UnVisibleWindow.Add(info.m_WindowName, child);
                    }
                }
                //Vector3 screenPos = CalculateUiPos(info.m_OffsetLeft, info.m_OffsetRight, info.m_OffsetTop, info.m_OffsetBottom);
                if (!m_IsLoadedWindow.ContainsKey(info.m_WindowName))
                {
                    m_IsLoadedWindow.Add(info.m_WindowName, child);
                }
                //if (null != child && cam != null)
                //    child.transform.position = cam.ScreenToWorldPoint(screenPos);
            }
        }
        IsUIVisible = true;
    }

    public void UnLoadAllWindow()
    {
        //每一个订阅事件的窗口UI都需要一个UnSubscribe函数用于消除事件
        LogicSystem.EventChannelForGfx.Publish("ge_ui_unsubscribe", "ui");
        foreach(GameObject window in m_IsLoadedWindow.Values)
        {
            if(null != window)
            {
                NGUITools.DestroyImmediate(window);
            }
        }
    }

    //获取已经加载的窗口GameObject
    public GameObject GetWindowGoByName(string windowName)
    {
        if (windowName == null) return null;
        if (m_IsLoadedWindow.ContainsKey(windowName.Trim()))
            return m_IsLoadedWindow[windowName];
        return null;
    }

    static private UIManager m_Instance = new UIManager();
    static public UIManager Instance
    {
        get { return m_Instance; }
    }
}
