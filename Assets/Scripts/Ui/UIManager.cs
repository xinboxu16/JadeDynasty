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

        if (DFMUiRoot.InputMode == InputType.Joystick)
        {
            JoyStickInputProvider.JoyStickEnable = true;
        }
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

        if (DFMUiRoot.InputMode == InputType.Joystick)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
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
            if(isCloseExclusion)
            {
                UiConfig uiCfg = GetUiConfigByName(windowName);
                if (uiCfg != null && uiCfg.m_IsExclusion == true)
                {
                    CloseExclusionWindow(windowName);
                }
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
            if(isOpenExclusion)
            {
                UiConfig uiCfg = GetUiConfigByName(windowName);
                if (uiCfg != null && uiCfg.m_IsExclusion == true)
                {
                    OpenExclusionWindow(windowName);//打开之前关闭的窗口
                }
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

    public GameObject LoadWindowByName(string windowName)
    {
        GameObject window = null;
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if(null != uiCfg)
        {
            window = ResourceSystem.GetSharedResource(uiCfg.m_WindowPath) as GameObject;
            if (null != window)
            {
                window = NGUITools.AddChild(m_RootWindow, window);
                string name = uiCfg.m_WindowName;
                while(m_IsLoadedWindow.ContainsKey(name))
                {
                    name += "ex";
                }
                m_IsLoadedWindow.Add(name, window);
                m_VisibleWindow.Add(name, window);
                return window;
            }else
            {
                Debug.Log("!!!load " + uiCfg.m_WindowPath + " failed");
            }
        }
        else
        {
            Debug.Log("!!!load " + windowName + " failed");
        }
        return null;
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

    public void SetAllUiVisible(bool isVisible)
    {
        if(isVisible)
        {
            TouchManager.TouchEnable = true;
            OpenExclusionWindow("");
        }
        else
        {
            TouchManager.TouchEnable = false;
            CloseExclusionWindow("");
        }
        IsUIVisible = isVisible;
        NicknameAndMoney(isVisible);
    }

    void NicknameAndMoney(bool vis)
    {
        if (m_RootWindow != null)
        {
            Transform tf = m_RootWindow.transform.parent.Find("DynamicWidget");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, vis);
            }
            tf = m_RootWindow.transform.Find("PveFightInfo(Clone)");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, vis);
            }
            tf = m_RootWindow.transform.Find("ScreenScrollTip");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, vis);
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

    //获取UI的路径
    public string GetPathByName(string windowName)
    {
        UiConfig uiCfg = GetUiConfigByName(windowName);
        if (uiCfg != null)
        {
            return uiCfg.m_WindowPath;
        }
        return null;
    }

    static private UIManager m_Instance = new UIManager();
    static public UIManager Instance
    {
        get { return m_Instance; }
    }
}
