using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIBeginnerGuide : Singleton<UIBeginnerGuide>
{
    private bool m_IsShow = false;

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
}
