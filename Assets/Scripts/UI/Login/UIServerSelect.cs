using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using uTools;

public class UIServerSelect : MonoBehaviour {

    public GameObject goItemContainer = null;
    public GameObject goTweenContainer = null;
    public UIServerItem recentLogin = null;
    public AnimationCurve curveForDown = null;
    public AnimationCurve curveForUpwards = null;
    public float durationForDown = 0.3f;
    public float durationForUpwards = 0.2f;
    public float tweenOffset = 500.0f;
    private Transform trans = null;
    private Canvas canvas = null;
    private int m_SelectServerId = -1;
    private List<object> m_EventList = new List<object>();

    void Awake()
    {
        object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_recent_server", "ui", SetRecentServer);
        if (null != obj) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (null != obj) m_EventList.Add(obj);
        trans = this.transform;
    }

    void Start()
    {
        Init();
        if(goTweenContainer != null)
        {
            goTweenContainer.transform.localPosition = new Vector3(0, 1000f, 0);
        }
    }

    void Update()
    {

    }

    public void Init()
    {
        if (goItemContainer == null) return;
        GridLayoutGroup grid = this.GetComponentInChildren<GridLayoutGroup>();
        if (grid == null) return;
        MyDictionary<int, object> serverConfigDic = ServerConfigProvider.Instance.GetData();
        int serverIndex = 0;
        GameObject go = null;
        foreach(ServerConfig cfg in serverConfigDic.Values)
        {
            if(serverIndex%2 == 0)
            {
                go = NGUITools.AddChild(grid.gameObject, goItemContainer, true);
            }
            if (go != null)
            {
                UIServerItemContainer itemContainer = go.GetComponent<UIServerItemContainer>();
                if (itemContainer != null)
                {
                    itemContainer.SetServerItem(serverIndex % 2, cfg.ServerId, cfg.ServerName, "baoman");
                    serverIndex++;
                }
            }
            
            //if (serverIndex % 2 == 1 && go != null)
            //{
            //    UIServerItemContainer con = go.GetComponent<UIServerItemContainer>();
            //    if (con != null) con.HideServerItem();
            //}
        }
    }

    public void SetRecentServer(int serverId)
    {
        try
        {
            if (recentLogin != null)
            {
                ServerConfig cfg = ServerConfigProvider.Instance.GetDataById(serverId);
                if (cfg != null)
                {
                    recentLogin.SetServerInfo(cfg.ServerId, cfg.ServerName, "baoman");
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnEnable()
    {
        tweenOffset = Screen.height;
        if (canvas == null)
        {
            canvas = transform.root.GetComponent<Canvas>();
        }
        if (canvas != null)
        {
            tweenOffset = 640;
        }

        if (goTweenContainer != null)
        {
            TweenPosition tweenPos = TweenPosition.Begin(this.goTweenContainer, new Vector3(0, tweenOffset, 0.0f), new Vector3(0, 0, 0.0f), durationForDown);
            if(tweenPos != null)
            {
                tweenPos.method = EaseType.easeInOutBack;
                tweenPos.animationCurve = curveForDown;
                tweenPos.from = new Vector3(0, tweenOffset*1.5f, 0);
            }
        }
    }

    //向上移动至消失
    public void TweenUpwards(int serverId)
    {
        if (goTweenContainer != null)
        {
            m_SelectServerId = serverId;
            TweenPosition tweenPos = TweenPosition.Begin(this.goTweenContainer, new Vector3(0, 0, 0.0f), new Vector3(0, tweenOffset*1.5f, 0f), durationForUpwards);
            if (tweenPos != null)
            {
                tweenPos.method = EaseType.easeInOutBack;
                tweenPos.animationCurve = curveForUpwards;
                UnityEvent unityEvent = new UnityEvent();
                unityEvent.AddListener(OnTweenUpwardsFinished);
                tweenPos.SetOnFinished(unityEvent);
            }
        }
    }

    private void OnTweenUpwardsFinished()
    {
        UIManager.Instance.HideWindowByName("ServerSelect");
        GameObject goLogin = UIManager.Instance.GetWindowGoByName("LoginPrefab");
        if (goLogin != null)
        {
            Login uiLogin = goLogin.GetComponent<Login>();
            if (uiLogin != null) uiLogin.TweenDownServerBtn();
        } 
        LogicSystem.EventChannelForGfx.Publish("ge_set_current_server", "ui", m_SelectServerId);
        //DashFire.GfxSystem.PublishGfxEvent("ge_set_current_server", "ui", m_SelectServerId);
        TweenPosition tween = goTweenContainer.GetComponent<TweenPosition>();
        if (null != tween) Destroy(tween);
    }

    public void UnSubscribe()
    {
        try
        {
            foreach(object obj in m_EventList)
            {
                if (null != obj)
                    LogicSystem.EventChannelForGfx.Unsubscribe(obj);
            }
            m_EventList.Clear();
        }catch(Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
