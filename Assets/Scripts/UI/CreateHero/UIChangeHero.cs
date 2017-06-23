using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChangeHero : MonoBehaviour {

    private List<object> eventlist = new List<object>();

    private List<ulong> playguidlist = new List<ulong>();//存储英雄的GUID
    private List<int> heroidlist = new List<int>();//存储英雄的id

    private int m_NicknameCount = 0;
    private List<string> m_NicknameList = new List<string>();

    private int num = 4;
    private int signforbuttonpress = 0;//现在被按下的按钮
    private static int HeroCount = 4;
    private int lastselectbut = 0;//创建英雄前选择的按钮
    private bool signforcreate = false;//选择英雄或创建英雄

    void Awake()
    {
        if (eventlist != null) { eventlist.Clear(); }
        object eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_show_hero_scene", "lobby", OnShowHeroScene);
        if (eo != null) { eventlist.Add(eo); }
        eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe<List<string>>("ge_nickname_result", "lobby", OnReceiveNicknames);
        if (eo != null) { eventlist.Add(eo); }
        eo = LogicSystem.EventChannelForGfx.Subscribe<string, int, string, ulong, int, string>("ge_role_enter_log", "log", OnRoleEnterLog);
        if (eo != null) { eventlist.Add(eo); }
        eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_login_finish", "lobby", LoginInFinish);
        if (eo != null) { eventlist.Add(eo); }
        eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe<bool>("ge_createhero_result", "lobby", CreateHeroResult);
        if (eo != null) { eventlist.Add(eo); }
        eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_close_nickname_dialog", "lobby", No);
        if (eo != null) { eventlist.Add(eo); }
    }

	// Use this for initialization
	void Start () {
        transform.Find("ButtonPanel/LeftButton").GetComponent<Button>().onClick.AddListener(LeftButtonClick);
        transform.Find("ButtonPanel/RightButton").GetComponent<Button>().onClick.AddListener(RightButtonClick);
        for (int i = 0; i < HeroCount; i++)
        {
            int index = i;//给delegate传参数必须在方法里面定义
            Transform tf = transform.Find("ButtonCreateHero/ButtonHero/" + i.ToString());
            tf.GetComponent<Button>().onClick.AddListener(delegate() { ButtonCreateHero(index); });
        }
        transform.Find("YesOrNot/Sprite/ChatInput/ButtonNickName").GetComponent<Button>().onClick.AddListener(ButtonForNickName);
        transform.Find("YesOrNot/Sprite/YES").GetComponent<Button>().onClick.AddListener(Yes);
        transform.Find("YesOrNot/Sprite/NO").GetComponent<Button>().onClick.AddListener(No);
        transform.Find("ScenePanel/ReturnLogin").GetComponent<Button>().onClick.AddListener(ReturnLogin);
        transform.Find("ScenePanel/EnterGame").GetComponent<Button>().onClick.AddListener(EnterGame);
        transform.Find("SelectHeroPanel/SelectHero0").GetComponent<Button>().onClick.AddListener(SelectHero0);
        transform.Find("SelectHeroPanel/SelectHero1").GetComponent<Button>().onClick.AddListener(SelectHero1);
        transform.Find("SelectHeroPanel/SelectHero2").GetComponent<Button>().onClick.AddListener(SelectHero2);

        //ChangeHeroIntroduce(0);

        //SetSceneVisible(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LeftButtonClick()
    {
        SetHeroVisible(num-- % HeroCount, false);
        if (num <= 0)
        {
            num = 1;
        }
        //SetHeroVisible(num % HeroCount, true);
        //ChangeHeroIntroduce(num % HeroCount);
        ButtonCreateHeroColourScale(num % (HeroCount+1));
    }

    public void RightButtonClick()
    {
        SetHeroVisible(num++ % HeroCount, false);
        //SetHeroVisible(num % HeroCount, true);
        //ChangeHeroIntroduce(num % HeroCount);
        if (num >= HeroCount)
        {
            num = HeroCount;
        }
        ButtonCreateHeroColourScale(num % (HeroCount+1));
    }

    public void OnReceiveNicknames(List<string> nickNameList)
    {
        try
        {
            m_NicknameList.Clear();
            m_NicknameList = nickNameList;
            m_NicknameCount = 0;
            if (m_NicknameList.Count >= 1)
            {
                SetHeroNickName(m_NicknameList[m_NicknameCount]);
                m_NicknameCount++;
            }
        }catch(Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OnRoleEnterLog(string account, int logicServerId, string nickname, ulong userGuid, int userLevel, string accountId)
    {
        try
        {
#if UNITY_IPHONE
      CYMGWrapperIOS.MBIOnLogin(account, logicServerId.ToString(), nickname, userGuid.ToString(), userLevel, accountId);
#endif
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void LoginInFinish()
    {
        try
        {
            UnSubscribe();
            if(gameObject)
            {
                DestroyImmediate(gameObject);
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    //角色创建失败
    private void CreateHeroResult(bool result)
    {
        try
        {
            if (!result)
            {
                ChooseYesOrNoVisible(DashFire.StrDictionaryProvider.Instance.GetDictString(127), true);
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    void OnShowHeroScene(bool vis)
    {
        try
        {
            SetSceneVisible(vis);
            OkToLoadHero(LobbyClient.Instance.AccountInfo.Players);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void OkToLoadHero(List<RoleInfo> playlist)
    {
        if(playguidlist != null && heroidlist != null)
        {
            heroidlist.Clear();
            playguidlist.Clear();
        }
        if (playlist != null && playlist.Count > 0)
        {
            //SortList(playlist);//对英雄按等级由高到低显示
            int i = playlist.Count;
            i = (i <= 3 ? i : 3);
            for(int j = 0; j < i; j++)
            {
                RoleInfo pi = playlist[j];
                if(pi != null)
                {
                    SetHeroInfo(j, pi);
                    if (playguidlist != null)
                    {
                        playguidlist.Add(pi.Guid);
                    }
                    if (heroidlist != null)
                    {
                        heroidlist.Add(pi.HeroId);
                    }
                }
            }

            if (i < 3)
            {
                Transform tf = transform.Find("SelectHeroPanel");
                Transform tfc = tf.Find("SelectHero" + i);
                NGUITools.SetActive(tfc.gameObject, true);
            }

            RoleInfo playerzero = playlist[0];
            if (playerzero != null)
            {
                ChangeHeroIntroduce(playerzero.HeroId);
                SetHeroVisible(1, false);
                num = playerzero.HeroId;
                SetHeroVisible(num, true);
            }

            WitchButtonPress(0);
        }
        else
        {
            ChangeHeroIntroduce(0);
            num = 0;
            SetHeroVisible(num, true);
            SelectHero0();
        }
    }

    private void SelectHero0()
    {
        if (playguidlist.Count < 1)
        {
            //创建英雄
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 0)
            {
                SetHeroVisible(num % HeroCount, false);
                num = heroidlist[0];
                SetHeroVisible(num % HeroCount, true);
                ChangeHeroIntroduce(num % HeroCount);
            }
        }
        WitchButtonPress(0);
    }

    private void SelectHero1()
    {
        if (playguidlist.Count < 2)
        {
            //创建英雄
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 1)
            {
                SetHeroVisible(num % HeroCount, false);
                num = heroidlist[1];
                SetHeroVisible(num % HeroCount, true);
                ChangeHeroIntroduce(num % HeroCount);
            }
        }
        WitchButtonPress(1);
    }

    public void SelectHero2()
    {
        if (playguidlist.Count < 3)
        {
            //创建英雄
            CreateHero();
        }
        else
        {
            if (heroidlist != null && heroidlist.Count > 2)
            {
                SetHeroVisible(num % HeroCount, false);
                num = heroidlist[2];
                SetHeroVisible(num % HeroCount, true);
                ChangeHeroIntroduce(num % HeroCount);
            }
        }
        WitchButtonPress(2);
    }
    

    private void CreateHero()
    {
        try
        {
            SetHeroVisible(num % HeroCount, false); 
            ButtonCreateHero(0);
            lastselectbut = signforbuttonpress;

            ButtonCreateHero(0);//设为第一个英雄
            ChangeActionShowAbout(true);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void ChangeActionShowAbout(bool actionsign)
    {
        if (transform == null) return;

        Transform tf = transform.Find("ButtonPanel");
             if (tf != null) {
               NGUITools.SetActive(tf.gameObject, actionsign);
             }
        tf = transform.Find("SelectHeroPanel");
        NGUITools.SetActive(tf.gameObject, !actionsign);
        tf = transform.Find("ButtonCreateHero");
        NGUITools.SetActive(tf.gameObject, actionsign);
        tf = transform.Find("Title/Sprite");
        Image us = tf.gameObject.GetComponent<Image>();
        if(us != null)
        {
            if(actionsign)
            {
                us.sprite = NGUITools.GetResourceSpriteByName("chuang-jian-jue-se");
            }
            else
            {
                us.sprite = NGUITools.GetResourceSpriteByName("xuan-ze-jue-se");
            }
        }
        tf = transform.Find("IntroducePanelCopy");
        NGUITools.SetActive(tf.gameObject, actionsign);
        tf = transform.Find("ScenePanel/ReturnLogin");
        NGUITools.SetActive(tf.gameObject, actionsign);
        tf = transform.Find("ScenePanel");
        tf = tf.Find("EnterGame/Text");
        Text ul = tf.gameObject.GetComponent<Text>();
        if (actionsign)
        {
            ul.text = DashFire.StrDictionaryProvider.Instance.GetDictString(133);
        }
        else
        {
            ul.text = DashFire.StrDictionaryProvider.Instance.GetDictString(132);
        }

        signforcreate = actionsign;
    }

    public void ButtonCreateHero(int index)
    {
        //transform.Find("ButtonCreateHero/ButtonHero/" + index.ToString());
        if(index < 2)
        {
            ButtonCreateHeroColourScale(index + 1);
        }
    }

    public void ButtonForNickName()
    {
        if (m_NicknameCount < m_NicknameList.Count)
        {
            SetHeroNickName(m_NicknameList[m_NicknameCount]);
            m_NicknameCount++;
        }
        else
        {
            LogicSystem.PublishLogicEvent("ge_create_nickname", "lobby");
        }
    }

    private string GetHeroNickName()
    {
        Transform tf = transform.Find("YesOrNot/Sprite/ChatInput/Back/Text");
        if (tf != null && tf.transform != null)
        {
            Text ul = tf.gameObject.GetComponent<Text>();
            if (ul != null)
            {
                return ul.text;
            }
        }
        return null;
    }

    public void Yes()
    {
        string nickname = GetHeroNickName().Trim();
        if(StringHelper.CalculateStringByte(nickname) > 14)
        {
            ChooseYesOrNoVisible(DashFire.StrDictionaryProvider.Instance.GetDictString(362), true);
        }else
        {
            if(nickname.Equals(String.Empty))
            {
                ChooseYesOrNoVisible(DashFire.StrDictionaryProvider.Instance.GetDictString(129), true);
                return;
            }
            bool ret = WordFilter.Instance.Check(nickname);
            if(ret)
            {
                ChooseYesOrNoVisible(DashFire.StrDictionaryProvider.Instance.GetDictString(128), true);
                return;
            }
            LogicSystem.PublishLogicEvent("ge_create_role", "lobby", num % (HeroCount+1), nickname);
        }
    }

    public void No()
    {
        ChooseYesOrNoVisible(null, false);
    }

    private void ChooseYesOrNoVisible(string str, bool vis)
    {
        Transform tf = transform.Find("YesOrNot");
        if (vis && tf != null)
        {
            if (tf.gameObject != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
            tf = tf.Find("Sprite/Label");
            Text ul = tf.gameObject.GetComponent<Text>();
            if (ul != null && str != null)
            {
                ul.text = str;
            }
        }
        else
        {
            if (tf != null && tf.gameObject != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
        }
    }

    private void ReturnLogin()
    {
        if (LobbyClient.Instance.AccountInfo.Players == null || LobbyClient.Instance.AccountInfo.Players.Count == 0)
        {
            return;
        }
        if (signforcreate)
        {
            if(heroidlist != null && heroidlist.Count > lastselectbut)
            {
                ButtonCreateHeroColourScale(0);
                SetHeroVisible(num % HeroCount, false);
                num = heroidlist[lastselectbut];
                SetHeroVisible(num % HeroCount, true);
                ChangeHeroIntroduce(num % HeroCount);
            }
            WitchButtonPress(lastselectbut);
            ChangeActionShowAbout(false);
        }
        else
        {
            //       SetSceneVisible(false);
            //       //返回登陆界面
            //       UIManager.Instance.ShowWindowByName("LoginPrefab");
        }
    }

    private void SetHeroNickName(string nickname)
    {
        try
        {
            if (nickname != null)
            {
                Transform tf = transform.Find("YesOrNot/Sprite/ChatInput/Back");
                if (tf != null)
                {
                    InputField ui = tf.gameObject.GetComponent<InputField>();
                    if (ui != null)
                    {
                        ui.text = nickname;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void ButtonCreateHeroColourScale(int newnum)
    {
        if(transform != null)
        {
            for(int i = 0; i < HeroCount; i++)
            {
                Transform tf = transform.Find("ButtonCreateHero/ButtonHero/" + (i) + "/Back");
                if (tf != null)
                {
                    if (i == (newnum-1))
                    {
                        NGUITools.SetActive(tf.gameObject, true);
                    }
                    else
                    {
                        NGUITools.SetActive(tf.gameObject, false);
                    }
                }
            }
        }
        SetHeroVisible(num, false);
        num = newnum;
        SetHeroVisible(num, true);
        ChangeHeroIntroduce(num);
    }

    private void WitchButtonPress(int sign)
    {
        if (sign < 0) return;
        if(transform != null)
        {
            Transform tf = transform.Find("SelectHeroPanel/SelectHero" + signforbuttonpress);
            tf = tf.Find("Back/Frame");
            NGUITools.SetActive(tf.gameObject, false);

            tf = transform.Find("SelectHeroPanel/SelectHero" + sign);
            tf = tf.Find("Back/Frame");
            NGUITools.SetActive(tf.gameObject, true);
        }

        signforbuttonpress = sign;
    }

    private void SetHeroVisible(int id, bool vis)
    {
        Transform tf = transform.Find("Hero" + id);
        if (tf != null && tf.gameObject != null)
        {
            if (NGUITools.GetActive(tf.gameObject) != vis)
            {
                NGUITools.SetActive(tf.gameObject, vis);
            }
        }
    }

    private void SetHeroInfo(int num, RoleInfo pi)
    {
        if (num < 0 || num > 3) return;
        Transform tf = transform.Find("SelectHeroPanel/SelectHero" + num);
        if(tf != null)
        {
            Image us = tf.gameObject.GetComponent<Image>();
            us.color = Color.white;
            NGUITools.SetActive(tf.gameObject, true);
            tf = tf.transform.Find("Back");
            NGUITools.SetActive(tf.gameObject, true);
            tf = tf.Find("Label");
            Text ul = tf.gameObject.GetComponent<Text>();
            ul.text = "<i>Lv." + pi.Level + "\n" + pi.Nickname + "</i>";
        }
        tf = transform.Find("SelectHeroPanel/SelectHero" + num + "/Back/Head");
        if (tf != null)
        {
            Image us = tf.gameObject.GetComponent<Image>();
            if (us != null)
            {
                us.sprite = pi.HeroId == 1 ? NGUITools.GetResourceSpriteByName("kuang-zhan-shi-tou-xiang2") : pi.HeroId == 2 ? NGUITools.GetResourceSpriteByName("ci-ke-tou-xiang2") : null;
                us.SetNativeSize();
            }
        }
        tf = transform.Find("SelectHeroPanel/SelectHero" + num + "/Sprite");
        if (tf != null)
        {
            NGUITools.SetActive(tf.gameObject, false);
        }

    }

    public void EnterGame()
    {
        if (signforcreate)
        {
            //LogicSystem.PublishLogicEvent("ge_create_role", "lobby", num % HeroCount, GetHeroNickName());
            Transform tf = transform.Find("YesOrNot/Sprite/ChatInput/Back");
            if (tf != null)
            {
                InputField ui = tf.gameObject.GetComponent<InputField>();
                if (ui != null)
                {
                    ui.text = "";
                }
            }
            ChooseYesOrNoVisible(DashFire.StrDictionaryProvider.Instance.GetDictString(135), true);
            LogicSystem.PublishLogicEvent("ge_create_nickname", "lobby");
        }
        else
        {
            LogicSystem.PublishLogicEvent("ge_role_enter", "lobby", signforbuttonpress);
        }
    }

    private void SetSceneVisible(bool vis)
    {
        NGUITools.SetActive(gameObject, vis);
    }

    private void ChangeHeroIntroduce(int heroId)
    {
        Data_PlayerConfig dpc = PlayerConfigProvider.Instance.GetPlayerConfigById(heroId);
        if (heroId >= 0 && dpc != null)
        {
            Transform containerTf = transform.Find("IntroducePanelCopy/Container");
            Transform tf = null;
            if (containerTf != null)
            {
                tf = containerTf.Find("Sprite/Name");
                if (tf != null)
                {
                    Text ul = tf.gameObject.GetComponent<Text>();
                    if (ul != null)
                    {
                        ul.text = "<i>" + dpc.m_Name + "</i>";
                    }
                }
            }

            tf = containerTf.Find("Bula");
            if (tf != null)
            {
                Text ul = tf.GetComponent<Text>();
                if (ul != null)
                {
                    ul.text = dpc.m_HeroIntroduce2;
                }
            }

            tf = containerTf.Find("Introduce");
            if (tf != null)
            {
                Text ul = tf.gameObject.GetComponent<Text>();
                if (ul != null)
                {
                    ul.text = dpc.m_HeroIntroduce1;
                }
            }

            tf = containerTf.Find("Sprite/Back/Head");
            if (tf != null)
            {
                Image us = tf.GetComponent<Image>();
                if(us != null)
                {
                    us.sprite = heroId == 1 ? NGUITools.GetResourceSpriteByName("kuang-zhan-shi-tou-xiang2") : heroId == 2 ? NGUITools.GetResourceSpriteByName("ci-ke-tou-xiang2") : null;
                    us.SetNativeSize();
                }
            }
        }

    }

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
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
