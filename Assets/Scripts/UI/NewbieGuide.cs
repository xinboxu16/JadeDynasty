using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewbieGuide : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
        m_RoleInfo = LobbyClient.Instance.CurrentRole;
        if (m_RoleInfo.SceneInfo.Count == 0)
        {
            //未实现
            JoyStickInputProvider.JoyStickEnable = false;
            LogicSystem.StartStory(2);
            //SkillBar.OnButtonClickedHandler += HandleSkillBarClicked;
            //SkillBar.OnCommomButtonClickHandler += HandlerOnSkillCommonButtonClick;
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
        //未实现
        //UpdateSkillLockFrame();
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
        //StoryDlgManager.Instance.ClearListener();
        //UIBeginnerGuide.Instance.ClearHandler();
        //SkillBar.OnButtonClickedHandler = null;
        //SkillBar.OnCommomButtonClickHandler = null;
        //GfxModule.Skill.GfxSkillSystem.OnGfxShillStart = null;
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

    private void onJoystickMoveStart()
    {
        Debug.Log("onJoystickMoveStart");
        UIManager.Instance.HideWindowByName("GuideDlg");
        SetGUIEnable(false);
        joyStick.onMoveStart.RemoveListener(onJoystickMoveStart);
    }
}
