using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    private List<object> eventList = new List<object>();

    private MyAction<int> doSomething = null;
    private bool isLogic = false;

    void Awake()
    {
        if (eventList != null)
        {
            eventList.Clear();
        }
        object eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventList.Add(eo);
        eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe<string, string, string, string, MyAction<int>, bool>("ge_show_dialog", "ui", ManageDialog);
        if (eo != null) eventList.Add(eo);
    }

	// Use this for initialization
	void Start () {
        transform.Find("Sprite/Button0").GetComponent<Button>().onClick.AddListener(onButton0);
        transform.Find("Sprite/Button1").GetComponent<Button>().onClick.AddListener(onButton1);
        transform.Find("Sprite/Button2").GetComponent<Button>().onClick.AddListener(onButton2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ManageDialog(string message, string button0, string button1, string button2, MyAction<int> dofunction, bool islogic)
    {
        try
        {
            doSomething = dofunction;
            isLogic = islogic;
            UIManager.Instance.ShowWindowByName("Dialog");

            Transform tf = transform.Find("Sprite/Button0");
            if(tf != null)
            {
                if(button0 == null)
                {
                    if(NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Text");
                    if(tf != null)
                    {
                        Text ul = tf.gameObject.GetComponent<Text>();
                        if(ul != null)
                        {
                            ul.text = button0;
                        }
                    }
                }
            }

            tf = transform.Find("Sprite/Button1");
            if (tf != null)
            {
                if (button1 == null)
                {
                    if (NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Label");
                    if (tf != null)
                    {
                        Text ul = tf.gameObject.GetComponent<Text>();
                        if (ul != null)
                        {
                            ul.text = button1;
                        }
                    }
                }
            }

            tf = transform.Find("Sprite/Button2");
            if (tf != null)
            {
                if (button2 == null)
                {
                    if (NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
                else
                {
                    if (!NGUITools.GetActive(tf.gameObject))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    tf = tf.Find("Label");
                    if (tf != null)
                    {
                        Text ul = tf.gameObject.GetComponent<Text>();
                        if (ul != null)
                        {
                            ul.text = button2;
                        }
                    }
                }
            }

            tf = transform.Find("Sprite/Label");
            if (tf != null)
            {
                Text ul = tf.gameObject.GetComponent<Text>();
                if (ul != null)
                {
                    ul.text = message;
                }
            }

        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void onButton0()
    {
        UIManager.Instance.HideWindowByName("Dialog");
        if(doSomething != null)
        {
            if(isLogic)
            {
                DashFire.LogicSystem.QueueLogicAction<int>(doSomething, 0);
            }else
            {
                doSomething(0);
            }
        }
    }

    public void onButton1()
    {
        UIManager.Instance.HideWindowByName("Dialog");
        if (doSomething != null)
        {
            if (isLogic)
            {
                DashFire.LogicSystem.QueueLogicAction<int>(doSomething, 1);
            }
            else
            {
                doSomething(1);
            }
        }
        doSomething = null;
    }

    public void onButton2()
    {
        UIManager.Instance.HideWindowByName("Dialog");
        if (doSomething != null)
        {
            if (isLogic)
            {
                DashFire.LogicSystem.QueueLogicAction(doSomething, 2);
            }
            else
            {
                doSomething(2);
            }
        }
        doSomething = null;
    }

    public void UnSubscribe()
    {
        try
        {
            if (eventList != null)
            {
                foreach (object eo in eventList)
                {
                    if (eo != null)
                    {
                        DashFire.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
                    }
                }
                eventList.Clear();
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
