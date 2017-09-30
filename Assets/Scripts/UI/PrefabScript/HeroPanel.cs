using DashFire;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum PanelType
{
    MySelf,
    Enemy
}

public class HeroPanel : MonoBehaviour {

    public PanelType panelType = PanelType.MySelf;

    private GfxUserInfo m_GfxUserInfo = null;
    public Slider hpProgressBar = null;
    public Slider mpProgressBar = null;
    public Text lblHp = null;
    public Text lblMp = null;
    public Text lblLevel = null;
    public Image spHeroPortrait = null;

    public bool m_IsInitialized = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (panelType == PanelType.MySelf)
        {
            UpdateSelfPanel();
        }
        else
        {
            UpdateEnemyPanel();
        }
	}

    public void SetUserInfo(GfxUserInfo userinfo)
    {
        m_GfxUserInfo = userinfo;
    }

    //更新血条
    private void UpdateHealthBar(int curValue, int maxValue)
    {
        if (maxValue <= 0 || curValue < 0)
            return;
        float value = curValue / (float)maxValue;

        if(null != hpProgressBar)
        {
            hpProgressBar.value = value;
        }

        if(null != lblHp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)(value * 100) + "%");
            lblHp.text = sb.ToString();
        }
    }

    //更新魔法值
    private void UpdateMp(int curValue, int maxValue)
    {
        if (maxValue <= 0 || curValue < 0)
            return;
        float value = curValue / (float)maxValue;
        if (null != mpProgressBar)
        {
            mpProgressBar.value = value;
        }
        if (null != lblMp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)(value * 100) + "%");
            lblMp.text = sb.ToString();
        }
    }

    //设置玩家等级
    void SetHeroLevel(int level)
    {
        if (lblLevel != null)
            lblLevel.text = level.ToString();
    }

    //设置玩家头像
    void SetHeroPortrait(string portrait)
    {
        if (null != spHeroPortrait)
        {
            Button btn = spHeroPortrait.GetComponent<Button>();
            if(btn != null)
            {
                btn.image.sprite = NGUITools.GetResourceSpriteByName(portrait);
            }
        }
    }


    //自己的血条
    public void UpdateSelfPanel()
    {
        if(m_GfxUserInfo != null)
        {
            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(m_GfxUserInfo.m_ActorId);
            if(null != shareInfo)
            {
                UpdateHealthBar((int)shareInfo.Blood, (int)shareInfo.MaxBlood);
                UpdateMp((int)shareInfo.Energy, (int)shareInfo.MaxEnergy);
                SetHeroLevel(m_GfxUserInfo.m_Level);

                if(!m_IsInitialized)
                {
                    Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(m_GfxUserInfo.m_HeroId);
                    if(playerData != null)
                    {
                        m_IsInitialized = true;
                        SetHeroPortrait(playerData.m_Portrait);
                    }
                }
            }
        }
    }

    //敌人的血条
    public void UpdateEnemyPanel()
    {
        if (m_GfxUserInfo == null) return;
        SharedGameObjectInfo enemyInfo = LogicSystem.GetSharedGameObjectInfo(m_GfxUserInfo.m_ActorId);
        if (enemyInfo != null)
        {
            UpdateHealthBar((int)enemyInfo.Blood, (int)enemyInfo.MaxBlood);
            UpdateMp((int)enemyInfo.Energy, (int)enemyInfo.MaxEnergy);
            SetHeroLevel(m_GfxUserInfo.m_Level);
            if (!m_IsInitialized)
            {
                Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(m_GfxUserInfo.m_HeroId);
                if (playerData != null)
                {
                    m_IsInitialized = true;
                    SetHeroPortrait(playerData.m_Portrait);
                }
            }
        }
    }
}
