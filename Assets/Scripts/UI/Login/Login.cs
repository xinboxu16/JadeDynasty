using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using uTools;

public class Login : MonoBehaviour {

    private int m_ServerId = 0;
    private int m_LogicServerId = 1;
    public bool CYMGAvailable = false;

    public Text lblServerName = null;
    public GameObject goServerBtn = null;

    public AnimationCurve CurveForUpwards = null;
    public AnimationCurve CurveForDown = null;
    public float DurationForUpwards = 0.3f;
    public float DurationForDown = 0.3f;
    public float TweenOffset = 150;
    private List<object> m_EventList = new List<object>();

    public enum AccountLoginResult
    {
        Success = 0,
        FirstLogin,
        Error
    }

    public void UnSubscribe()
    {
        try
        {
            foreach (object obj in m_EventList)
            {
                if (null != obj) LogicSystem.EventChannelForGfx.Unsubscribe(obj);
            }
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {
        goServerBtn.GetComponent<Button>().onClick.AddListener(OnChangeServer);
        this.transform.FindChild("StartBtn").GetComponent<Button>().onClick.AddListener(OnLoginButtonClick);

        object obj = LogicSystem.EventChannelForGfx.Subscribe<int>("ge_set_current_server", "ui", SetCurrentServer);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<int, string>("ge_login_result", "lobby", OnLoginResult);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_rolelist_result", "lobby", OnRoleListResult);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null) m_EventList.Add(obj);

        //获取上一次登录的服务器ID，若没有则默认值为0
        m_ServerId = PlayerPrefs.GetInt("LastLoginServerId");
        SetCurrentServer(m_ServerId);

        m_LogicServerId = PlayerPrefs.GetInt("LastLoginLogicServerId");
        SetCurrentLogicServer(m_LogicServerId);

#if UNITY_IPHONE
        CYMGWrapperIOS.GetUniqueIdentifier();
#else
        string deviceUniqueIdentifier = ((uint)Application.dataPath.GetHashCode()).ToString();
        LogicSystem.PublishLogicEvent("ge_device_info", "lobby", deviceUniqueIdentifier);
#endif
    }

    //设置当前服务器
    public void SetCurrentServer(int serverId)
    {
        try
        {
            m_ServerId = serverId;
            ServerConfig serverConfig = ServerConfigProvider.Instance.GetDataById(serverId);
            if (serverConfig != null)
            {
                StrDictionary strDic = StrDictionaryProvider.Instance.GetDataById(201);
                if (strDic != null)
                {
                    lblServerName.text = "" + serverId + strDic.m_String + serverConfig.ServerName;
                }
                else
                {
                    lblServerName.text = "" + serverId + serverConfig.ServerName;
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetCurrentLogicServer(int serverId)
    {
        try
        {
            m_LogicServerId = serverId;
        }
        catch(Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnLoginResult(int result, string accountId)
    {
        try
        {
            AccountLoginResult ret = (AccountLoginResult)result;
            if (ret == AccountLoginResult.Success)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
                //登录成功  
                UIManager.Instance.HideWindowByName("LoadingHint");
                if (CYMGAvailable)
                {
#if UNITY_IPHONE
                    CYMGWrapperIOS.OnLoginBillingSuccess(accountId);
#endif
                }
            }else if(ret == AccountLoginResult.FirstLogin)
            { 
                LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, false);
                //账号首次登录，提示输入激活码 
                UIManager.Instance.HideWindowByName("LoginPrefab");
                UIManager.Instance.ShowWindowByName("Verification");
                UIManager.Instance.HideWindowByName("LoadingHint");
                //UISkillGuide.Instance.SetSteps(1);//第一次登陆初始技能教学预设 未实现
                if (CYMGAvailable)
                {
#if UNITY_IPHONE
                    CYMGWrapperIOS.OnLoginBillingSuccess(accountId);
#endif
                }
                else
                {
                    //登录失败,重新登录
                    Debug.LogError("Account verify failed...Try again.");
                    if (CYMGAvailable)
                    {
#if UNITY_IPHONE
                         CYMGWrapperIOS.StartLogin(false);
#endif
                    }
                }

            }
        }
        catch(Exception e)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", e.Message, e.StackTrace);
        }
    }

    public void OnRoleListResult(bool isSuccess)
    {
        try
        {
            if(isSuccess == true)
            {
                //获取角色列表成功，跳转到角色选择页面
                UIManager.Instance.HideWindowByName("LoginPrefab");
                LogicSystem.EventChannelForGfx.Publish("ge_create_hero_scene", "ui", true);
            }
            else
            {
                //获取角色列表失败，给出错误提示
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnLoginButtonClick()
    {
        ServerConfig serverConfig = ServerConfigProvider.Instance.GetDataById(m_ServerId);
        if(serverConfig == null)
        {
            Debug.LogError("Can't read server info!!!");
            return;
        }

        PlayerPrefs.SetInt("LastLoginServerId", m_ServerId);
        m_LogicServerId = serverConfig.LogicServerId;
        PlayerPrefs.SetInt("LastLoginLogicServerId", m_LogicServerId);
        string nodeIp = serverConfig.NodeIp;
        int nodePort = serverConfig.NodePort;
        string serverAddress = "ws://" + nodeIp + ":" + nodePort;
        LogicSystem.PublishLogicEvent("ge_select_server", "lobby", serverAddress, m_LogicServerId);
        if (CYMGAvailable)
        {
            //连接畅游平台SDK，账号登录
#if UNITY_IPHONE
            CYMGWrapperIOS.StartLogin(true);
#else
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            LogicSystem.PublishLogicEvent("ge_direct_login", "lobby");
#endif
        }
        else
        {
            //无账号登录，快速开始
            LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
            LogicSystem.PublishLogicEvent("ge_direct_login", "lobby");
        }
    }

    public void OnChangeServer()
    {
        UIManager.Instance.ShowWindowByName("ServerSelect");
        LogicSystem.EventChannelForGfx.Publish("ge_recent_server", "ui", m_ServerId);
        if (goServerBtn != null)
        {
            Button uiBtn = goServerBtn.GetComponent<Button>();
            if (uiBtn != null) uiBtn.enabled = false;
            Vector3 serverBtnPos = (goServerBtn.transform as RectTransform).anchoredPosition;
            TweenPosition tweenPos = TweenPosition.Begin(goServerBtn, serverBtnPos, new Vector3(serverBtnPos.x, serverBtnPos.y + TweenOffset, 0.0f), DurationForUpwards);
            if (tweenPos != null)
            {
                tweenPos.method = EaseType.easeInOutBack;
                tweenPos.animationCurve = CurveForUpwards;
                UnityEvent unityEvent = new UnityEvent();
                unityEvent.AddListener(OnTweenUpwardsFinished);
                tweenPos.SetOnFinished(unityEvent);
            }
        }
    }

    private void OnTweenUpwardsFinished()
    {
        if (goServerBtn != null)
        {
            Button uibtn = goServerBtn.GetComponent<Button>();
            if (uibtn != null) uibtn.enabled = true;
            TweenPosition tween = goServerBtn.GetComponent<TweenPosition>();
            if (tween != null) Destroy(tween);
            NGUITools.SetActive(goServerBtn, false);
        }
    }

    public void TweenDownServerBtn()
    {
        if(null != goServerBtn)
        {
            NGUITools.SetActive(goServerBtn, true);
            Vector2 serverBtnPos = (goServerBtn.transform as RectTransform).anchoredPosition;
            TweenPosition posTween = TweenPosition.Begin(goServerBtn, serverBtnPos, new Vector3(serverBtnPos.x, serverBtnPos.y - TweenOffset, 0.0f), DurationForUpwards);
            if(posTween)
            {
                posTween.method = EaseType.easeInOutBack;
                posTween.animationCurve = CurveForDown;
            }
        }
    }
}
