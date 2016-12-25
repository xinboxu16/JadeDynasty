using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFMUiRoot : MonoBehaviour {
    
    private GameObject DynamicWidgetPanel = null;
    private GameObject ScreenTipPanel = null;
    private GameObject loading = null;

    public SceneTypeEnum m_SceneType = SceneTypeEnum.TYPE_UNKNOWN;
    public SceneSubTypeEnum m_SubSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;

    private void AddSubscribeForGfx()
    {
        LogicSystem.EventChannelForGfx.Subscribe("ge_loading_start", "ui", StartLoading);
        LogicSystem.EventChannelForGfx.Subscribe("ge_loading_finish", "ui", EndLoading);
        LogicSystem.EventChannelForGfx.Subscribe("ge_show_login", "ui", ShowLogin);

    }

	// Use this for initialization
	void Start () 
    {
        //Input.multiTouchEnabled = false;

        DontDestroyOnLoad(this.gameObject.transform);
        UIManager.Instance.Init(this.gameObject);
        UIManager.Instance.OnAllUiLoadedDelegate += AfterAllUiLoaded;

        AddSubscribeForGfx();

        DynamicWidgetPanel = transform.Find("DynamicWidget").gameObject;
        ScreenTipPanel = transform.Find("ScreenTipPanel").gameObject;
	}

    void StartLoading()
    {
        try
        {
            //未实现（部分）
            //NpcGameObjectS.Clear();

            //开始加载Loading条时，卸载所有UI
            UIManager.Instance.UnLoadAllWindow();

            GameObject goConnect = UIManager.Instance.GetWindowGoByName("Connect");
            if (goConnect != null)
            {
                GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
            }

            //if (InputType.Joystick == DFMUiRoot.InputMode)
            //{
            //    JoyStickInputProvider.JoyStickEnable = false;
            //}

            GameObject mgo = UIManager.Instance.GetWindowGoByName("Mars");
            if (mgo != null)
            {
                UIManager.Instance.HideWindowByName("Mars");
            }

            if (loading != null) return;

            //加载进度页面
            GameObject go = ResourceSystem.GetSharedResource("Loading/Loading2") as GameObject;
            if (go != null)
            {
                loading = NGUITools.AddWidget(gameObject, go, true);
                if (loading != null)
                {
                    loading.transform.localPosition = new Vector3(0, 0, 0);
                    NGUITools.SetActive(loading, true);
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }

    void EndLoading()
    {
        try
        {
            if(loading != null)
            {
                //SendMessage("EndLoading")这个是ProgressBar中的方法
                loading.transform.Find("ProgressBar").SendMessage("EndLoading");
                loading = null;
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowLogin()
    {
        try
        {
            UIManager.Instance.LoadAllWindows(0, transform.root.GetComponent<Canvas>().worldCamera);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AfterAllUiLoaded()
    {
        //未实现

    }
}
