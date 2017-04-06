using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
