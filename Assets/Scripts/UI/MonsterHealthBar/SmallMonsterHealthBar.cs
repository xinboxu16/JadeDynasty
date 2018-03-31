using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;

public class SmallMonsterHealthBar : MonoBehaviour {

    private GameObject goHealthBar = null;
    private List<object> m_EventList = new List<object>();

    private bool m_CanCast = false;
    public float countdown = -1f;
    public float waitTime = 3f;
    private Vector3 positionVec3 = Vector3.zero;

    void Awake()
    {
        Transform trans = this.transform.FindChild("HealthBar");
        if (trans == null)
            return;
        goHealthBar = trans.gameObject;
        if (null == goHealthBar)
        {
            Debug.LogError("Can Not Find HealthBar");
        }
        object obj = LogicSystem.EventChannelForGfx.Subscribe<int, int, int>("ge_small_monster_healthbar", "ui", UpdateHealthBar);
        if (null != obj) m_EventList.Add(obj);
        obj = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null) m_EventList.Add(obj);
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
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (m_CanCast)
        {
            CastAnimation(goHealthBar);
        }
        if (countdown > 0)
        {
            countdown -= RealTime.deltaTime;
            if (countdown <= 0)
                NGUITools.SetActive(this.gameObject, false);
        }
	}

    private void UpdateHealthBar(int curValue, int maxValue, int hpDamage)
    {
        try
        {
            if (Vector3.zero == positionVec3)
            {
                positionVec3 = this.transform.localPosition;
            }
            NGUITools.SetActive(this.gameObject, true);
            ShakeHealthBar();
            SetProgressValue(curValue, maxValue, hpDamage);
            countdown = waitTime;
            if (maxValue <= 0)
                return;
            SetHealthValueText(curValue, maxValue);
            float value = curValue / (float)maxValue;
            Slider progressBar = goHealthBar.GetComponent<Slider>();
            if (null != progressBar)
            {
                progressBar.value = value;
                TweenSpriteAlpha(goHealthBar);
                if (value <= 0)
                {
                    SetLable("Dead");
                }
                else
                {
                    SetLable("x1");
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SetLable(string state)
    {
        string path = "HealthBar/state";
        Transform trans = this.transform.FindChild(path);
        if (trans == null)
            return;
        GameObject go = trans.gameObject;
        if (null != go)
        {
            Text label = go.GetComponent<Text>();
            if (null != label)
                label.text = state;
        }
    }

    private void TweenSpriteAlpha(GameObject father)
    {
        GameObject goBack = null;
        Slider progressBar = null;
        if(null != father)
        {
            progressBar = father.GetComponent<Slider>();
            Transform trans = father.transform.Find("white");
            if (trans != null)
                goBack = trans.gameObject;
        }
        if (goBack == null)
            return;
        Image spBack = null;
        if (null != goBack)
        {
            spBack = goBack.GetComponent<Image>();
        }
        if (null != spBack && null != progressBar)
        {
            if(spBack.fillAmount <= progressBar.value)
            {
                spBack.fillAmount = progressBar.value;
                SetCastFlag(true);
            }
            else
            {
                TweenAlpha tween = goBack.GetComponent<TweenAlpha>();
                if (null == tween)
                    return;
                tween.enabled = true;
                tween.ResetToBeginning();
                tween.PlayForward();
            }
        }
    }

    public void OnTweenAlphaFinished()
    {
        SetCastFlag(true);

        GameObject goBack = null;
        Slider progressBar = null;
        if (null != goHealthBar)
        {
            Transform trans = goHealthBar.transform.Find("white");
            if (trans != null)
                goBack = trans.gameObject;
            progressBar = goHealthBar.GetComponent<Slider>();
        }
        if (goBack == null)
            return;
        Image spBack = null;
        if (null != goBack)
        {
            spBack = goBack.GetComponent<Image>();
        }
        if (null != spBack && null != progressBar)
        {
            if (spBack.fillAmount >= progressBar.value)
            {
                spBack.fillAmount = progressBar.value;
                //Debug.Log("TweenAlpha is not null!!");
            }
        }

    }

    private void SetHealthValueText(int current, int max)
    {
        NGUITools.SetActive(this.gameObject, true);
        Transform trans = this.transform.Find("HealthBar/healthValue");
        if(null == trans)
        {
            return;
        }
        GameObject go = trans.gameObject;
        if(null != go)
        {
            Text label = go.GetComponent<Text>();
            if(null != label)
            {
                label.text = current.ToString() + "/" + max.ToString();
            }
        }
    }

    private void SetProgressValue(int curValue, int maxValue, int damage)
    {
        if(maxValue == 0)
        {
            return;
        }
        //因为damage是负值
        float percent = (curValue - damage) / (float)maxValue;
        if(goHealthBar != null)
        {
            Transform trans = goHealthBar.transform.Find("white");
            if (trans == null) return;
            GameObject go = trans.gameObject;
            Image sp = go.GetComponent<Image>();
            if (sp != null)
                sp.fillAmount = percent;
            trans = goHealthBar.transform.Find("forDel");
            if (trans == null) return;
            go = trans.gameObject;
            sp = go.GetComponent<Image>();
            if (sp != null)
                sp.fillAmount = percent;
        }
    }
     
    private void ShakeHealthBar()
    {
        TweenPosition.Begin(this.gameObject, new Vector3(positionVec3.x + 25, positionVec3.y + 25, positionVec3.z), positionVec3, 0.5f);
    }

    private void CastAnimation(GameObject father)
    {
        GameObject goBack = null;
        Slider progressBar = null;
        if (null != father)
        {
            Transform trans = father.transform.Find("forDel");
            if (trans != null)
                goBack = trans.gameObject;
            progressBar = father.GetComponent<Slider>();
        }
        Image spBack = null;
        if (null != goBack)
        {
            spBack = goBack.GetComponent<Image>();
        }
        if (null != spBack && null != progressBar)
        {
            if (spBack.fillAmount <= progressBar.value)
            {
                spBack.fillAmount = progressBar.value;
                SetCastFlag(false);
            }
            else
            {
                spBack.fillAmount -= RealTime.deltaTime * 0.2f;//血条动画
            }
        }
    }

    public void SetCastFlag(bool canCast)
    {
        m_CanCast = canCast;
    }
}
