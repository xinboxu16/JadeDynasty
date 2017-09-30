using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PveFightInfo : MonoBehaviour {

    private Text moneylabel = null;
    private bool signtime = false;
    private Text timeorsomelabel = null;
    private Text defenselabel = null;
    private Image scrollus = null;
    private int countDownTime = 0;
    private long StartTime = 0;
    private int count = 0;

    private List<object> eventlist = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            if (eventlist != null)
            {
                foreach (object eo in eventlist)
                {
                    if (eo != null)
                    {
                        DashFire.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                    }
                }
                eventlist.Clear();
            }
            NGUITools.DestroyImmediate(gameObject);
            DFMUiRoot.pveFightInfo = null;
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

	// Use this for initialization
	void Start () {
        if (eventlist != null) { eventlist.Clear(); }
        object eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventlist.Add(eo);

        Transform tf = transform.Find("jinbi/Label");
        if(tf != null)
        {
            Text ul = tf.transform.GetComponent<Text>();
            if(ul != null)
            {
                moneylabel = ul;
                moneylabel.text = "0";
            }
        }

        tf = transform.Find("");//得到什么？

        //TODO 未实现
        //GfxSystem.EventChannelForLogic.Publish("ge_get_scenestart_time", "lobby");
	}
	
	// Update is called once per frame
	void Update() {
		if(signtime)
        {
            if(timeorsomelabel != null)
            {
                long residuetime = (long)countDownTime - (TimeUtility.GetServerMilliseconds() - StartTime) / 1000;//残余时间

                string str1 = (residuetime / 60).ToString();
                if(str1.Length == 1)
                {
                    str1 = "0" + str1;
                }

                string str2 = (residuetime % 60).ToString();
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }

                timeorsomelabel.text = str1 + ":" + str2;
                if(residuetime <= 0)
                {
                    //时间超过 挑战失败
                    signtime = false;
                    SetDefeat();
                }
            }
        }
	}

    public void SetDefeat()
    {
        Transform tf = transform.Find("TimeOrSome/Sprite");
        if(tf != null)
        {
            Image us = tf.gameObject.GetComponent<Image>();
            if(us != null)
            {
                us.sprite = NGUITools.GetResourceSpriteByName("tzshibai");
                us.SetNativeSize();
            }
        }
    }

    public void SetInitInfo(int type, int num0, int num1, int num2)
    {
        count++;
        Transform tf = transform.Find("TimeOrSome/Sprite");
        Image us = null;
        if(tf != null)
        {
            us = tf.gameObject.GetComponent<Image>();
        }

        switch(type)
        {
            case 0:
                if(us != null)
                {
                    us.sprite = NGUITools.GetResourceSpriteByName("zd_bjan");//被击
                }
                tf = transform.Find("TimeOrSome/Back");
                if(tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                tf = transform.Find("TimeOrSome/Label");
                if(tf != null)
                {
                    Text ul = tf.gameObject.GetComponent<Text>();
                    if(ul != null)
                    {
                        timeorsomelabel = ul;
                    }
                }
                break;
            case 1:
               if(us != null)
                {
                    us.sprite = NGUITools.GetResourceSpriteByName("zd_fsan");//防御
                }
                tf = transform.Find("TimeOrSome/Label");
                if(tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                tf = transform.Find("TimeOrSome/Back/Label");
                if(tf != null)
                {
                    Text ul = tf.gameObject.GetComponent<Text>();
                    if(ul != null)
                    {
                        defenselabel = ul;
                    }
                }
                tf = transform.Find("TimeOrSome/Back/Front");
                if (tf != null)
                {
                    Image uss = tf.gameObject.GetComponent<Image>();
                    if (uss != null)
                    {
                        scrollus = uss;
                    }
                }
                break;
            case 2:
                if (us != null)
                {
                    us.sprite = NGUITools.GetResourceSpriteByName("zd_tzan");//挑战
                }
                tf = transform.Find("TimeOrSome/Back");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                tf = transform.Find("TimeOrSome/Label");
                if (tf != null)
                {
                    Text ul = tf.gameObject.GetComponent<Text>();
                    if (ul != null)
                    {
                        timeorsomelabel = ul;
                    }
                }
                SetTimeCountDownTime(num0);
                break;
            case 3:
                if (count == 2)
                {
                    StartTime = DashFire.TimeUtility.GetServerMilliseconds();
                }
                if (us != null)
                {
                    us.sprite = NGUITools.GetResourceSpriteByName("zd_txan");//突袭
                }
                tf = transform.Find("TimeOrSome/Back");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                tf = transform.Find("TimeOrSome/Label");
                if (tf != null) {
                Text ul = tf.gameObject.GetComponent<Text>();
                  if (ul != null) {
                    timeorsomelabel = ul;
                  }
                }
                SetTimeCountDownTime(num0);
                break;
            case 4:
                tf = transform.Find("TimeOrSome");
                if (tf != null)
                {
                    NGUITools.SetActive(tf.gameObject, false);
                }
                break;
        }
        SetUpdateInfo(type, num0, num1, num2);
    }

    public void SetUpdateInfo(int type, int num0, int num1, int num2)
    {
        switch (type)
        {
            case 0:
                if (num0 > num1) 
                { 
                    num0 = num1; 
                }
                if (timeorsomelabel != null)
                {
                    timeorsomelabel.text = num0.ToString() + "/" + num1;
                }
                if (num0 == num1)
                {
                    SetDefeat();
                }
                break;
            case 1:
                if (num1 > num2) { num1 = num2; }
                if (defenselabel != null)
                {
                    defenselabel.text = DashFire.StrDictionaryProvider.Instance.Format(307, num0);
                }
                if (scrollus != null)
                {
                    scrollus.fillAmount = (float)(num1 * 1.0f / num2);
                }
                break;
            case 2:
                if (!signtime)
                {
                    SetTimeCountDownTime(num0);
                }
                break;
            case 3:
                if (!signtime)
                {
                    SetTimeCountDownTime(num0);
                }
                break;
        }
    }

    private void SetTimeCountDownTime(int time)
    {
        countDownTime = time;
        signtime = true;
    }
}
