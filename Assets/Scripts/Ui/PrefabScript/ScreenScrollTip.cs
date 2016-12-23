using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;

public class ScreenScrollTip : MonoBehaviour
{
    public class TipCount
    {
        public TipCount(int num)
        {
            count = num;
        }
        public int count = 0;
    }

    private float DelayTime = 5.0f;
    private int strcount = 0;
    public GameObject TweenLabel = null;
    private Dictionary<string, TipCount> tipStrDic = new Dictionary<string, TipCount>();
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
            Destroy(gameObject);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {
        if (eventlist != null) { eventlist.Clear(); }
        if (tipStrDic != null) { tipStrDic.Clear(); }
        object eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe<string, int>("ge_notice", "notice", GetScreenScrollTip);
        if (eo != null) eventlist.Add(eo);
        if (TweenLabel != null)
        {
            TweenLabel.SetActive(true);
        }
    }

    void OnDestroy()
    {
        UnSubscribe();
    }

    float GetOffsetWidth()
    {
        float width = 0;
        RectTransform up = gameObject.GetComponent<RectTransform>();
        if (up != null)
        {
            width -= up.sizeDelta.x;
            if (TweenLabel != null)
            {
                Text ul = TweenLabel.GetComponent<Text>();
                if (ul != null)
                {
                    width -= (ul.transform as RectTransform).sizeDelta.x;
                    return width;
                }
            }
        }
        return 0f;
    }

    void CheckAndDeleteStr()
    {
        if (TweenLabel != null)
        {
            Text ul = TweenLabel.GetComponent<Text>();
            if (ul != null && ul.text.Length > 0)
            {
                string strtip = "";
                List<string> strlist = new List<string>();
                foreach (string str in tipStrDic.Keys)
                {
                    if (str != null)
                    {
                        if (tipStrDic[str].count == 0)
                        {
                            strlist.Add(str);
                        }
                        else
                        {
                            tipStrDic[str].count -= 1;
                            strtip += str;
                        }
                    }
                }
                for (int i = 0; i < strlist.Count; ++i)
                {
                    string str2 = strlist[i];
                    if (str2 != null && tipStrDic.ContainsKey(str2))
                    {
                        tipStrDic.Remove(str2);
                    }
                }
                ul.text = strtip;
                strcount = strtip.Length;
            }
        }
    }

    void GetScreenScrollTip(string info, int num)
    {
        try
        {
            if (info == null || num == 0) { return; }
            if (tipStrDic.ContainsKey(info))
            {
                tipStrDic[info].count += num;
            }
            else
            {
                if (tipStrDic.Count == 0)
                {
                    tipStrDic.Add(info, new TipCount(--num));
                    if (TweenLabel != null)
                    {
                        Text ul = TweenLabel.GetComponent<Text>();
                        if (ul != null)
                        {
                            ul.text = info;
                            strcount = info.Length;
                        }
                    }
                }
                else
                {
                    tipStrDic.Add(info, new TipCount(num));
                }
                if (!TweenLabel.gameObject.activeSelf)
                {
                    TweenLabel.gameObject.SetActive(true);
                    TweenPosition tp = TweenLabel.GetComponent<TweenPosition>();
                    if (tp != null)
                    {
                        tp.ResetToBeginning();
                        tp.to = new Vector3(GetOffsetWidth(), tp.to.y, tp.to.z);
                        tp.duration = DelayTime + strcount * 0.3f;
                        tp.enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void OnceEnd()
    {
        try
        {
            if (TweenLabel != null)
            {
                CheckAndDeleteStr();
                TweenPosition tp = TweenLabel.GetComponent<TweenPosition>();
                if (tp != null)
                {
                    tp.ResetToBeginning();
                    tp.to = new Vector3(GetOffsetWidth(), tp.to.y, tp.to.z);
                    tp.duration = DelayTime + strcount * 0.3f;
                    tp.enabled = true;
                }
            }
            if (tipStrDic.Count == 0)
            {
                TweenLabel.gameObject.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
