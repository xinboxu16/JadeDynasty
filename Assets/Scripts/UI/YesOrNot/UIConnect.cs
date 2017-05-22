using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum UIConnectEnumType
{
    None,
    Reconnect,
    ChangeScene
}

public class UIConnect : MonoBehaviour {
    public Button btnConfirm = null;
    public Image spConnect = null;
    public Text lblHintMsg = null;

    public float ConnectDelta = 10f;//切换场景超过10S 提示玩家重新点击挑战按钮
    public float ReconnectCountDown = 10f;//重连10s之后提示玩家 按确定键退出游戏 
    private const int c_ConnectCountMax = 3;
    private int m_ConnectCount = 0;//重连计数
    private float m_ReconnectJudgetime = 60;//s 大于60秒不发重连消息，代表链接上，重置m_ConnectCount;

    private UIConnectEnumType m_ConnectShowType = UIConnectEnumType.None;
    private float m_ConnectCD = 0f;
    private float m_ReconnectCD = 0f;
    private List<object> m_EventList = new List<object>();

    void Awake()
    {
        object obj = LogicSystem.EventChannelForGfx.Subscribe<bool, bool>("ge_ui_connect_hint", "ui", OnShowConnectHint);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null) m_EventList.Add(obj);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateInfo();
	}

    void OnEnable()
    {
        if (btnConfirm != null)
        {
            NGUITools.SetActive(btnConfirm.gameObject, false);
        }
    }

    void OnDestroy()
    {
        //JoyStickInputProvider.SetActive(true);//未实现
        m_ConnectShowType = UIConnectEnumType.None;
    }

    public void OnConfirmClick()
    {
        switch(m_ConnectShowType)
        {
            case UIConnectEnumType.ChangeScene:
                UIManager.Instance.HideWindowByName("Connect");
                break;
            case UIConnectEnumType.Reconnect:
                GameObject go = GameObject.Find("GfxGameRoot");
                if(go != null)
                {
                    GameLogic gameLogic = go.GetComponent<GameLogic>();
                    if(gameLogic != null)
                    {
                        gameLogic.RestartLocgic();
                    }
                }
        }
    }

    public void UnSubscribe()
    {
        try
        {
            foreach(object obj in m_EventList)
            {
                if(null != obj)
                {
                    LogicSystem.EventChannelForGfx.Unsubscribe(obj);
                }
            }
            m_EventList.Clear();
        }catch(Exception e)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", e.Message, e.StackTrace);
        }
    }
}
