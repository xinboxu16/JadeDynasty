using DashFire;
using StoryDlg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryDlgPanel : MonoBehaviour {
    private GameObject m_StoryDlgGO = null;
    private StoryDlgInfo m_StoryInfo = null;
    private int m_Count = 0;
    private int m_StepNumber = 0;
    private bool m_IsStoryDlgActive = false;
    private int m_StoryId = 0;
    private StoryDlgType m_StoryDlgType = StoryDlgType.Big;
    private float m_IntervalTime = 5.0f;

    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnNextBtnClicked);
    }

	public void OnTriggerStory(StoryDlgInfo storyInfo)
    {
        StoryDlgManager.Instance.FireStoryStartMsg();
        if (m_IsStoryDlgActive == false)
        {
            m_StoryInfo = storyInfo;
            if(null != m_StoryInfo)
            {
                m_StoryId = m_StoryInfo.ID;
                m_StoryDlgType = m_StoryInfo.DlgType;
                m_IntervalTime = m_StoryInfo.IntervalTime;
                m_IsStoryDlgActive = true;
                m_StoryDlgGO = this.gameObject;
                m_Count = 0;//剧情对话计数器，触发一个新的剧情时重置为0
                m_StepNumber = m_StoryInfo.StoryItems.Count;
                StoryDlgItem item = m_StoryInfo.StoryItems[m_Count];
                UpdateStoryDlg(m_StoryDlgGO.transform, item);
                NGUITools.SetActive(m_StoryDlgGO, true);

                m_Count++;
                if(item.IntervalTime > 0.0f)
                {
                    Invoke("NextStoryItem", item.IntervalTime);
                }
            }
        }
    }

    //下一句
    private void NextStoryItem()
    {
        //剧情对话框处于活跃状态时，处理单击操作
        if(m_IsStoryDlgActive)
        {
            CancelInvoke("NextStoryItem");//取消延时调用
            if(null != m_StoryDlgGO)
            {
                bool isActive = NGUITools.GetActive(m_StoryDlgGO);
                if(isActive == true)
                {
                    if(m_Count < m_StepNumber)
                    {
                        StoryDlgItem item = m_StoryInfo.StoryItems[m_Count];
                        UpdateStoryDlg(m_StoryDlgGO.transform, item);
                        NGUITools.SetActive(m_StoryDlgGO, true);
                        m_Count++;
                        if(item.IntervalTime > 0.0f)
                        {
                            Invoke("NextStoryItem", item.IntervalTime);
                        }
                    }
                    else
                    {
                        FinishStoryDlg();
                    }
                }
            }
        }
    }

    //直接结束剧情对话
    private void StopStoryDlg()
    {
        //剧情对话框处于活跃状态时，处理单击操作
        CancelInvoke("NextStoryItem");
        if (m_IsStoryDlgActive == true)
        {
            if (null != m_StoryDlgGO)
            {
                FinishStoryDlg();
            }
        }
    }

    private void FinishStoryDlg()
    {
        m_IsStoryDlgActive = false;
        NGUITools.SetActive(m_StoryDlgGO, false);
        m_StoryDlgGO = null;
        m_StoryInfo = null;
        RaiseStoryDlgOverEvent();
    }

    //剧情对话结束引发事件
    private void RaiseStoryDlgOverEvent()
    {
        LogicSystem.SendStoryMessage("dialogover:" + m_StoryId);
        StoryDlgManager.Instance.FireStoryEndMsg(m_StoryId);
        Debug.Log("dialogover" + m_StoryId.ToString());
    }

    private void UpdateStoryDlg(Transform storyTrans, StoryDlgItem item)
    {
        Text lblName = storyTrans.Find("SpeakerName").GetComponent<Text>();
        Text lblWords = storyTrans.Find("SpeakerWords").GetComponent<Text>();
        Image spriteLeft = storyTrans.Find("SpeakerImageLeft").GetComponent<Image>();
        Image spriteRight = storyTrans.Find("SpeakerImageRight").GetComponent<Image>();

        if (m_StoryDlgType == StoryDlgType.Big)
        {
            lblName.text = string.Format("<color='#c9b2ae'>{0}</color>", item.SpeakerName);
            lblWords.text = item.Words;
            spriteLeft.sprite = NGUITools.GetResourceSpriteByName(item.ImageLeftSmall);
            spriteRight.sprite = NGUITools.GetResourceSpriteByName(item.ImageRightSmall);
        }
        else
        {
            lblName.text = string.Format("<color='#c9b2ae'>{0}</color>", item.SpeakerName);
            item.Words = item.Words.Replace("[\\n]", "\n");
            lblWords.text = item.Words;
            Sprite sp = NGUITools.GetResourceSpriteByName(item.ImageLeft);
            if (null != sp)
            {
                NGUITools.SetActive(spriteLeft.gameObject, true);
                spriteLeft.sprite = NGUITools.GetResourceSpriteByName(item.ImageLeft);
            }
            else
            {
                NGUITools.SetActive(spriteLeft.gameObject, false);
                Debug.Log("!!!ImageLeftAtlas is null.");
            }

            sp = NGUITools.GetResourceSpriteByName(item.ImageRight);
            if (null != sp)
            {
                NGUITools.SetActive(spriteRight.gameObject, true);
                spriteRight.sprite = NGUITools.GetResourceSpriteByName(item.ImageRight);
            }
            else
            {
                NGUITools.SetActive(spriteRight.gameObject, false);
                Debug.Log("!!!ImageLeftAtlas is null.");
            }
        }
    }

    public void OnNextBtnClicked()
    {
        this.NextStoryItem();
    }
}
