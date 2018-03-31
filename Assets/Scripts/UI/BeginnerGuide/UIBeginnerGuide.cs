using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIBeginnerGuide : Singleton<UIBeginnerGuide>
{
    private bool m_IsShow = false;

    public List<SkillInfo> infos = new List<SkillInfo>();

    public void ShowGuideDlgInControl(Vector2 center, float y)
    {
        if(!m_IsShow)
        {
            m_IsShow = true;

            UIManager.Instance.HideWindowByName("SkillBar");

            GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlg");
            if(goGuideDlg == null)
            {
                goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlg");
            }

            if(goGuideDlg != null)
            {
                Canvas canvas = goGuideDlg.transform.root.GetComponent<Canvas>();
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.rectTransform(), center, canvas.worldCamera, out pos);
                (goGuideDlg.transform as RectTransform).localPosition = pos;
                UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
                if(null != guideDlg)
                {
                    guideDlg.SetDescription(501);
                }
            }
        }
    }

    public void ShowGuideDlgAboveCommon(int index)
    {
        Vector3 pos = new Vector3(20, 300, 0);
        GameObject goSkillBar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (goSkillBar != null)
        {
            SkillBar skillBar = goSkillBar.GetComponent<SkillBar>();
            if (skillBar != null && skillBar.CommonSkillGo!=null)
            {
                pos = skillBar.CommonSkillGo.transform.localPosition;
            }
        }

        GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlgRight");
        if (goGuideDlg == null)
        {
            goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlgRight");
            goGuideDlg.transform.localPosition = new Vector2(pos.x, pos.y + 80);
            UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
            if(guideDlg != null)
            {
                if (index == 1) guideDlg.SetDescription(502);
                if (index == 2) guideDlg.SetDescription(505);
            }
            UIManager.Instance.ShowWindowByName("GuideDlgRight");
        }
    }

    public void ShowGuideDlgAboveSkillA(int index)
    {
        Vector2 pos = new Vector2(20, 300);
        GameObject goSkillBar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (goSkillBar != null)
        {
            Transform tsSkillA = goSkillBar.transform.Find("Skill0/skill0");
            if (tsSkillA != null)
            {
                pos = NGUITools.TransformToLocalPosition(tsSkillA, goSkillBar.transform as RectTransform, DFMUiRoot.UiCanvas);
                //pos = tsSkillA.localPosition;
            }
        }

        GameObject goGuideDlg = UIManager.Instance.GetWindowGoByName("GuideDlgRight");
        if (goGuideDlg == null)
        {
            goGuideDlg = UIManager.Instance.LoadWindowByName("GuideDlgRight");
        }
        goGuideDlg.transform.localPosition = new Vector2(pos.x, pos.y + 80);
        UIGuideDlg guideDlg = goGuideDlg.GetComponent<UIGuideDlg>();
        if (guideDlg != null)
        {
            if (index == 1) guideDlg.SetDescription(503);
            if (index == 2) guideDlg.SetDescription(504);
        }
        UIManager.Instance.ShowWindowByName("GuideDlgRight");
    }

    //移动小手到普通攻击按钮、显示一个普通攻击按钮
    public void TransHandInCommonAttact(int index)
    {
        UIManager.Instance.ShowWindowByName("SkillBar");
        GameObject goHand = UIManager.Instance.GetWindowGoByName("GuideHand");
        if(goHand == null)
        {
            goHand = UIManager.Instance.LoadWindowByName("GuideHand");
        }
        GameObject goSkillbar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if(goHand != null && goSkillbar != null)
        {
            SkillBar sb = goSkillbar.GetComponent<SkillBar>();
            if(null != sb)
            {
                if (sb.spAshEx != null)
                {
                    NGUITools.SetActive(sb.spAshEx.gameObject, false);
                    if (index == 1) sb.InitSkillBar(null);//关掉所有技能按钮
                    if (sb.CommonSkillGo != null)
                    {
                        Vector3 pos = sb.CommonSkillGo.transform.localPosition;
                        RectTransform goHandRect = (goHand.GetComponentInChildren<Image>().transform as RectTransform);
                        goHand.transform.localPosition = new Vector2(pos.x + goHandRect.sizeDelta.x/2, pos.y - goHandRect.sizeDelta.y/2);
                    }
                }
            }
        }

        UIManager.Instance.ShowWindowByName("GuideHand");
    }

    //移动小手到第一个技能按钮
    public void TransHandInFirstSkill()
    {
        GameObject goHand = UIManager.Instance.GetWindowGoByName("GuideHand");
        GameObject goSkillbar = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (goHand == null)
        {
            goHand = UIManager.Instance.LoadWindowByName("GuideHand");
        }
        if (goHand != null && goSkillbar != null)
        {
            RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
            SkillBar sb = goSkillbar.GetComponent<SkillBar>();
            if(sb != null)
            {
                infos.Clear();
                foreach(SkillInfo info in roleInfo.SkillInfos)
                {
                    if(info.ConfigData.Category == SkillCategory.kSkillA)
                    {
                        infos.Add(info);
                        sb.InitSkillBar(infos);
                        break;
                    }
                }
            }
            Transform tsSkillA = goSkillbar.transform.Find("Skill0/skill0");
            if(tsSkillA != null)
            {
                Vector2 pos = NGUITools.TransformToLocalPosition(tsSkillA, goSkillbar.transform as RectTransform, DFMUiRoot.UiCanvas);
                //Vector3 pos = tsSkillA.transform.localPosition;
                RectTransform goHandRect = (goHand.GetComponentInChildren<Image>().transform as RectTransform);
                goHand.transform.localPosition = new Vector2(pos.x + goHandRect.sizeDelta.x / 2, pos.y - goHandRect.sizeDelta.y / 2);
            }
        }
        UIManager.Instance.ShowWindowByName("GuideHand");
    }

    //让普攻图标失效
    public void SetCommonSkillBtnActive(bool active)
    {
        GameObject go = UIManager.Instance.GetWindowGoByName("SkillBar");
        if (go == null) return;
        SkillBar skillBar = go.GetComponent<SkillBar>();
        if (skillBar != null && skillBar.CommonSkillGo != null)
        {
            Button btn = skillBar.CommonSkillGo.GetComponent<Button>();
            if (btn != null) btn.enabled = active;
        }
    }

    //显示回到主城按钮
    public void ShowReturnButton()
    {
        UIManager.Instance.HideWindowByName("GuideHand");
        UIManager.Instance.LoadWindowByName("ReturnToMaincity");
    }
}
