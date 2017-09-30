using DashFire;
using StoryDlg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DFMUiRoot : MonoBehaviour {
    
    private static GameObject _rootWidget = null;
    private static Transform _rootTransform = null;
    private GameObject DynamicWidgetPanel = null;
    private GameObject ScreenTipPanel = null;
    private GameObject loading = null;

    private int m_EnemyNum = 0;

    public SceneTypeEnum m_SceneType = SceneTypeEnum.TYPE_UNKNOWN;
    public SceneSubTypeEnum m_SubSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;

    static public Dictionary<GameObject, GameObject> NpcGameObjectS = new Dictionary<GameObject, GameObject>();
    static public List<GfxUserInfo> UserInfoForUI = new List<GfxUserInfo>();

    static public int NowSceneID = 0;

    //pve战斗信息单独处理,type = 0,1,2,3(被击，防御，挑战，突袭)
    static public GameObject pveFightInfo = null;

    private static InputType inputMode = InputType.Joystick;

    public static InputType InputMode
    {
        set
        {
            inputMode = value;
            if(InputType.Joystick == inputMode)
            {
                JoyStickInputProvider.JoyStickEnable = true;
                TouchManager.GestureEnable = false;
            }
            else
            {
                TouchManager.GestureEnable = true;
                JoyStickInputProvider.JoyStickEnable = false;
            }
        }

        get
        {
            return inputMode;
        }
    }

    private void AddSubscribeForGfx()
    {
        LogicSystem.EventChannelForGfx.Subscribe("ge_loading_start", "ui", StartLoading);
        LogicSystem.EventChannelForGfx.Subscribe("ge_loading_finish", "ui", EndLoading);
        LogicSystem.EventChannelForGfx.Subscribe("ge_show_login", "ui", ShowLogin);
        LogicSystem.EventChannelForGfx.Subscribe<int>("ge_enter_scene", "ui", EnterInScene);
        LogicSystem.EventChannelForGfx.Subscribe<GfxUserInfo>("ge_show_name_plate", "ui", CreateHeroNickName);
        LogicSystem.EventChannelForGfx.Subscribe<List<GfxUserInfo>>("ge_show_name_plates", "ui", CreateHeroNickName);
        LogicSystem.EventChannelForGfx.Subscribe<GfxUserInfo>("ge_show_npc_name_plate", "ui", CreateNpcNickName);
        LogicSystem.EventChannelForGfx.Subscribe<int, int, int, int>("ge_pve_fightinfo", "ui", SetPveFightInfo);
    }

    void Awake()
    {
        _rootWidget = this.gameObject;
        _rootTransform = this.transform;
        VirtualScreen.Instance.ComputeScene(_rootWidget.GetComponent<CanvasScaler>());
    }

	// Use this for initialization
	void Start () 
    {
        //Input.multiTouchEnabled = false;

        DontDestroyOnLoad(this.gameObject.transform);
        UIManager.Instance.Init(this.transform.Find("Widgets").gameObject);
        UIManager.Instance.OnAllUiLoadedDelegate += AfterAllUiLoaded;

        AddSubscribeForGfx();

        DynamicWidgetPanel = transform.Find("DynamicWidget").gameObject;
        ScreenTipPanel = transform.Find("ScreenTipPanel").gameObject;
	}


    void StartLoading()
    {
        try
        {
            NpcGameObjectS.Clear();

            //开始加载Loading条时，卸载所有UI
            UIManager.Instance.UnLoadAllWindow();

            GameObject goConnect = UIManager.Instance.GetWindowGoByName("Connect");
            if (goConnect != null)
            {
                GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
            }

            //if (InputType.Joystick == DFMUiRoot.InputMode)
            //{
            //    JoyStickInputProvider.JoyStickEnable = false;
            //}

            GameObject mgo = UIManager.Instance.GetWindowGoByName("Mars");
            if (mgo != null)
            {
                UIManager.Instance.HideWindowByName("Mars");
            }

            if (loading != null) return;

            //加载进度页面
            GameObject go = ResourceSystem.GetSharedResource("Loading/Loading2") as GameObject;
            if (go != null)
            {
                GameObject widgetGameObject = _rootTransform.FindChild("Widgets").gameObject;
                loading = NGUITools.AddWidget(widgetGameObject, go, true);
                if (loading != null)
                {
                    //Debug.Log("loading"+loading.transform.GetSiblingIndex().ToString());
                    loading.transform.localPosition = new Vector3(0, 0, 0);
                    NGUITools.SetActive(loading, true);
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

    }

    void EndLoading()
    {
        try
        {
            if(loading != null)
            {
                //SendMessage("EndLoading") EndLoading这个是ProgressBar中的方法
                //SendMessage ("函数名",参数，SendMessageOptions) //GameObject自身的Script
                //BroadcastMessage ("函数名",参数，SendMessageOptions)  //自身和子Object的Script
                //SendMessageUpwards ("函数名",参数，SendMessageOptions)  //自身和父Object的Script
                loading.transform.Find("ProgressBar").SendMessage("EndLoading", SendMessageOptions.RequireReceiver);
                loading = null;
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void ShowLogin()
    {
        try
        {
            UIManager.Instance.LoadAllWindows((int)UISceneType.Login, transform.root.GetComponent<Canvas>().worldCamera);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void SetPveFightInfo(int type, int num0, int num1, int num2)
    {
        if (pveFightInfo == null)
        {
            string path = UIManager.Instance.GetPathByName("PveFightInfo");
            GameObject go = ResourceSystem.GetSharedResource(path) as GameObject;
            if (go != null)
            {
                GameObject parent = transform.FindChild("Widgets").gameObject;
                go = NGUITools.AddChild(parent, go);
                pveFightInfo = go;
                PveFightInfo pfi = go.GetComponent<PveFightInfo>();
                if (pfi != null)
                {
                    pfi.SetInitInfo(type, num0, num1, num2);
                }
            }
        }
        else
        {
            PveFightInfo pfi = pveFightInfo.GetComponent<PveFightInfo>();
            if (pfi != null)
            {
                pfi.SetInitInfo(type, num0, num1, num2);
            }

            Transform tf = pveFightInfo.transform.Find("TimeOrSome");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
        }
    }

    //进入场景
    public void EnterInScene(int sceneId)
    {
        try
        {
            NowSceneID = sceneId;
            SceneSubTypeEnum prevSceneType = m_SubSceneType;//上一场景类型
            m_EnemyNum = 0;

            Data_SceneConfig dsc = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
            if(dsc != null)
            {
                if(dsc.m_SubType == (int)SceneSubTypeEnum.TYPE_EXPEDITION)
                {
                    m_SubSceneType = SceneSubTypeEnum.TYPE_EXPEDITION;
                }
                else
                {
                    m_SubSceneType = SceneSubTypeEnum.TYPE_UNKNOWN;
                }

                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_SERVER_SELECT)
                {
                    m_SceneType = SceneTypeEnum.TYPE_SERVER_SELECT;
                    UIManager.Instance.LoadAllWindows(0, null);
                }

                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PURE_CLIENT_SCENE;
                    UIManager.Instance.LoadAllWindows(1, null);
                    StartCoroutine(DelayForNewbieGuide());
                }

                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PVP)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVP;
                    LoadUiInGame(sceneId);
                    if (UIManager.Instance.OnAllUiLoadedDelegate != null)
                    {
                        UIManager.Instance.OnAllUiLoadedDelegate();
                    }
                }

                if (dsc.m_Type == (int)SceneTypeEnum.TYPE_PVE)
                {
                    m_SceneType = SceneTypeEnum.TYPE_PVE;
                    LoadUiInGame(sceneId);
                    if (UIManager.Instance.OnAllUiLoadedDelegate != null)
                    {
                        UIManager.Instance.OnAllUiLoadedDelegate();
                    }
                    SetPveFightInfo(4, 0, 0, 0);
                }

                //如果刚打完远征，则打开远征界面
                if (prevSceneType == SceneSubTypeEnum.TYPE_EXPEDITION && m_SceneType == SceneTypeEnum.TYPE_PURE_CLIENT_SCENE)
                {
                    UIManager.Instance.ShowWindowByName("cangbaotu");
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //加载ui sceneType == -1
    public void LoadUiInGame(int sceneId)
    {
        UIManager.Instance.LoadAllWindows(-1, null);
        //初始化剧情配置数据
        InitStoryDlg();
    }

    private void InitStoryDlg()
    {
        StoryDlgManager.Instance.Init();
    }

    public IEnumerator DelayForNewbieGuide()
    {
        yield return new WaitForSeconds(0.5f);

        NewbieGuideManager nbm = gameObject.GetComponent<NewbieGuideManager>();
        if (nbm != null)
        {
            NGUITools.DestroyImmediate(nbm);
        }
        RoleInfo role = LobbyClient.Instance.CurrentRole;
        if (null != role && role.NewbieGuides.Count > 0)//NewbieGuides LobbyMessageHandler.cs(218)赋值
        {
            SystemGuideConfig config = SystemGuideConfigProvider.Instance.GetDataById(role.NewbieGuides[0]);
            if (null != config)
            {
                ShowNewbieGuide(config.Operations);
            }
        }
    }

    private void ShowNewbieGuide(List<int> idList)
    {
        if(idList != null)
        {
            NewbieGuideManager ngm = gameObject.AddComponent<NewbieGuideManager>();
            if (ngm != null)
            {
                ngm.SetMySelf(ngm, transform);
                //TODO:未实现
                //ngm.DoInitGuid(idlist);
            }
        }
    }

    public bool IsCombatWithGhost(List<GfxUserInfo> gfxUsers)
    {
        if (gfxUsers == null) return false;
        RoleInfo roleInfo = LobbyClient.Instance.CurrentRole;
        if (roleInfo == null) return false;
        UserInfo userInfo = roleInfo.GetPlayerSelfInfo();
        if (userInfo == null) return false;
        for (int i = 0; i < gfxUsers.Count; ++i)
        {
            if (gfxUsers[i] != null)
            {
                SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(gfxUsers[i].m_ActorId);
                if(shareInfo != null && userInfo.GetCampId() != shareInfo.CampId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //pve创建
    public void CreatePvPHeroPanel(List<GfxUserInfo> gfxUsers)
    {
        m_EnemyNum = 0;
        if(gfxUsers != null)
        {
            if(m_SubSceneType == SceneSubTypeEnum.TYPE_EXPEDITION && IsCombatWithGhost(gfxUsers))
            {
                //TODO 未实现
            }
            else
            {
                //目前普通PVE只有玩家自己
                if (gfxUsers.Count > 0 && gfxUsers[0] != null)
                {
                    GameObject go = UIManager.Instance.GetWindowGoByName("HeroPanel");
                    if(go == null)
                    {
                        Debug.Log("!!!HeroPanel is null.");
                        return;
                    }
                    HeroPanel scriptHp = go.GetComponent<HeroPanel>();
                    if (scriptHp != null) 
                        scriptHp.SetUserInfo(gfxUsers[0]);
                }
            }
        }
    }

    private void AboutHeroNickName(GfxUserInfo gui, int cmpid)
    {
        if(gui != null)
        {
            UserInfoForUI.Add(gui);
            GameObject pargo = LogicSystem.GetGameObject(gui.m_ActorId);//ObjectInstance
            SharedGameObjectInfo sgoi = LogicSystem.GetSharedGameObjectInfo(gui.m_ActorId);//ObjectInfo
            if(pargo != null && sgoi != null)
            {
                string path = UIManager.Instance.GetPathByName("NickName");
                GameObject go = ResourceSystem.GetSharedResource(path) as GameObject;
                if(go != null && DynamicWidgetPanel != null)
                {
                    go = NGUITools.AddChild(DynamicWidgetPanel, go);
                    if (go != null)
                    {
                        NickName nn = go.GetComponent<NickName>();
                        if(nn != null)
                        {
                            nn.SetPlayerGameObjectAndNickName(pargo, gui.m_Nick, sgoi.CampId == cmpid ? Color.white : Color.red);
                        }
                    }
                }
            }
        }
    }

    public void CreateHeroNickName(GfxUserInfo gfxUser)
    {
        try
        {
            //TODO 未实现
            //CreatePvPHeroPanel(gfxUser);
            RoleInfo ri = LobbyClient.Instance.CurrentRole;
            if(null != ri)
            {
                UserInfo ui = ri.GetPlayerSelfInfo();
                if(ui != null)
                {
                    AboutHeroNickName(gfxUser, ui.GetCampId());
                }
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void CreateHeroNickName(List<GfxUserInfo> gfxUsers)
    {
        try
        {
            UserInfoForUI.Clear();
            CreatePvPHeroPanel(gfxUsers);
            RoleInfo ri = DashFire.LobbyClient.Instance.CurrentRole;
            if (ri != null)
            {
                UserInfo ui = ri.GetPlayerSelfInfo();
                if (ui != null)
                {
                    foreach (GfxUserInfo gui in gfxUsers)
                    {
                        AboutHeroNickName(gui, ui.GetCampId());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void CreateNpcNickName(GfxUserInfo gfxNpc)
    {
        try
        {
            AboutNpcNickName(gfxNpc);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void AboutNpcNickName(GfxUserInfo gui)
    {
        //TODO未实现
    }

    public static GameObject RootWidget
    {
        get
        {
            return _rootWidget;
        }
    }

    public static Transform RootTransform
    {
        get
        {
            return _rootTransform;
        }
    }

    public void AfterAllUiLoaded()
    {
        //TODO:未实现

    }
}
