using DashFire;
using StoryDlg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewbieGuide : MonoBehaviour {

    private enum HeroIdEnum
    {
        WARRIOR = 1,
        ASSASSIN = 2,
    }

    private RoleInfo m_RoleInfo;
    private bool m_IsDetect = false;
    public Vector3 m_NpcPos;
    public float m_DetectRange = 2.0f;
    private bool m_EnableGUI = false;

    private GameObject m_RuntimeEffect;
    private ETCJoystick joyStick = null;

    //配置小手的位置
    private const int C_OffsetL = 70;
    private const int C_OffsetB = 180;
    private int m_TransFlag = 1;
    private int m_TransOffset = 0;

    private bool m_IsCommonSkillGuid = false;

    private bool m_HasSkillTeached = false;
    private bool m_HasLockFrame = false;
    public SkillCategory m_GuideCategory;
    private float m_SkillStartTime = 0;
    private const float m_Epsilon = 0.00001f;

    public int m_WarriorFirstSkill = 160201;
    public int m_WarriorSecondSkill = 160202;
    public float m_WarriorSkillLockFrameDelay = 1.8f;
    public int m_AssassinFirstSkill = 160201;
    public int m_AssassinSecondSkill = 160202;
    public float m_AssassinSkillLockFrameDelay = 1.8f;

	// Use this for initialization
	void Start () {
        m_RoleInfo = LobbyClient.Instance.CurrentRole;
        if (m_RoleInfo.SceneInfo.Count == 0)
        {
            //未实现
            JoyStickInputProvider.JoyStickEnable = false;
            LogicSystem.StartStory(2);
            SkillBar.OnButtonClickedHandler += HandleSkillBarClicked;
            SkillBar.OnCommomButtonClickHandler += HandlerOnSkillCommonButtonClick;
        }
        else
        {
            LogicSystem.StartStory(3);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsDetect)
        {
            GameObject player = LogicSystem.PlayerSelf;
            if(null != player)
            {
                if(Vector3.Distance(player.transform.position, m_NpcPos) < m_DetectRange)
                {
                    OnReachEnemy();
                    m_IsDetect = false;
                }
            }
        }
        UpdateSkillLockFrame();
	}

    void OnGUI()
    {
        if (m_EnableGUI)
        {
            GUI.depth = 0;
            Texture handTex = ResourceSystem.GetSharedResource("Sprite/shouzhi") as Texture;
            if(null != handTex)
            {
                Rect rect = VirtualScreen.GetRealRect(new Rect(C_OffsetL + m_TransOffset, C_OffsetB, 100, 100));
                
                UIBeginnerGuide.Instance.ShowGuideDlgInControl(rect.center, rect.height);

                rect.y = Screen.height - rect.y + 60;//GUI.DrawTexture 左上角是0,0点
                GUI.DrawTexture(rect, handTex);

                if (m_TransOffset == 0) m_TransFlag = 1;
                if (m_TransOffset == C_OffsetL) m_TransFlag = -1;
                m_TransOffset += m_TransFlag;
            }
            else
            {
                Debug.LogError("HandTex is null");
            }
        }
    }

    void OnDestroy()
    {
        //未实现
        StoryDlgManager.Instance.ClearListener();
        //UIBeginnerGuide.Instance.ClearHandler();
        SkillBar.OnButtonClickedHandler = null;
        SkillBar.OnCommomButtonClickHandler = null;
        GfxModule.Skill.GfxSkillSystem.OnGfxSkillStart = null;
    }

    private void OnReachEnemy()
    {
        SetGUIEnable(false);
        if (null != m_RuntimeEffect)
        {
            ResourceSystem.RecycleObject(m_RuntimeEffect);
        }
        LogicSystem.SendStoryMessage("reachenemy");
    }

    private void SetGUIEnable(bool enable)
    {
        m_EnableGUI = enable;
    }

    public void OnCloseController()
    {
        JoyStickInputProvider.JoyStickEnable = false;
    }

    public void OnStartGuide()
    {
        joyStick = ETCInput.GetControlJoystick(JoyStickInputProvider.JoyStickName);
        joyStick.onMoveStart.AddListener(onJoystickMoveStart);
        //新手关卡不允许直接退出
        UIManager.Instance.HideWindowByName("Tuichu");
        SetGUIEnable(true);
    }

    //在story.dsl中配置 触发攻击按钮 进入引导
    public void OnCommonAttack(int index)
    {
        m_IsCommonSkillGuid = true;//普通技能引导
        UIBeginnerGuide.Instance.TransHandInCommonAttact(index);
        UIBeginnerGuide.Instance.ShowGuideDlgAboveCommon(index);
    }

    public void OnSkillAAttack(int index)
    {
        UIBeginnerGuide.Instance.TransHandInFirstSkill();
        UIBeginnerGuide.Instance.ShowGuideDlgAboveSkillA(index);
    }

    //点击普通攻击按钮
    public void HandlerOnSkillCommonButtonClick()
    {
        if(m_IsCommonSkillGuid)
        {
            HideAllGuide();
            m_IsCommonSkillGuid = false;
        }
    }

    public void HideAllGuide()
    {
        UIManager.Instance.HideWindowByName("GuideHand");
        UIManager.Instance.HideWindowByName("GuideDlgRight");
    }

    private void HandleSkillBarClicked(SkillCategory category)
    {
        if (!m_HasSkillTeached)
        {
            if (m_GuideCategory == category)
            {
                if (m_SkillStartTime < m_Epsilon)
                {
                    m_SkillStartTime = Time.time;
                    EnableController(false);
                    LogicSystem.NotifyGfxForceStartSkill(LogicSystem.PlayerSelf, GetFirstSkill());
                    LogicSystem.NotifyGfxStartSkill(LogicSystem.PlayerSelf, category, Vector3.zero);
                    OnSkillAAttack(2);
                }
                else
                {
                    if (Time.time - m_SkillStartTime > GetLockFrameDelay() && m_HasLockFrame)
                    {
                        Time.timeScale = 1.0f;
                        LogicSystem.NotifyGfxForceStartSkill(LogicSystem.PlayerSelf, GetSecondSkill());
                        m_HasSkillTeached = true;
                        HideAllGuide();
                        EnableController(true);
                    }
                }
            }
        }
        else
        {
            LogicSystem.NotifyGfxStartSkill(LogicSystem.PlayerSelf, category, Vector3.zero);
        }
    }

    private float GetLockFrameDelay()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (null != role_info)
        {
            if (role_info.HeroId == (int)HeroIdEnum.WARRIOR)
            {
                return m_WarriorSkillLockFrameDelay;
            }
            else if (role_info.HeroId == (int)HeroIdEnum.ASSASSIN)
            {
                return m_AssassinSkillLockFrameDelay;
            }
        }
        return 0;
    }

    private void UpdateSkillLockFrame()
    {
        if (m_SkillStartTime > m_Epsilon && !m_HasLockFrame)
        {
            if (Time.time - m_SkillStartTime > GetLockFrameDelay())
            {
                m_HasLockFrame = true;
                Time.timeScale = 0.0f;//锁帧 暂停界面
            }
        }
    }

    private int GetFirstSkill()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (null != role_info)
        {
            if (role_info.HeroId == (int)HeroIdEnum.WARRIOR)
            {
                return m_WarriorFirstSkill;
            }
            else if (role_info.HeroId == (int)HeroIdEnum.ASSASSIN)
            {
                return m_AssassinFirstSkill;
            }
        }
        return 0;
    }

    private int GetSecondSkill()
    {
        RoleInfo role_info = LobbyClient.Instance.CurrentRole;
        if (null != role_info)
        {
            if (role_info.HeroId == (int)HeroIdEnum.WARRIOR)
            {
                return m_WarriorSecondSkill;
            }
            else if (role_info.HeroId == (int)HeroIdEnum.ASSASSIN)
            {
                return m_AssassinSecondSkill;
            }
        }
        return 0;
    }

    private void EnableController(bool active)
    {
        JoyStickInputProvider.SetActive(active);
        UIBeginnerGuide.Instance.SetCommonSkillBtnActive(active);
    }

    private void onJoystickMoveStart()
    {
        Debug.Log("onJoystickMoveStart");
        UIManager.Instance.HideWindowByName("GuideDlg");
        SetGUIEnable(false);
        joyStick.onMoveStart.RemoveListener(onJoystickMoveStart);
    }

    //返回场景
    private IEnumerator OnShowReturnCity()
    {
        yield return new WaitForSeconds(1.5f);
        UIBeginnerGuide.Instance.ShowReturnButton();
    }
}
