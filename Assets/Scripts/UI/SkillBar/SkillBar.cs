using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uTools;

public class SkillBar : MonoBehaviour 
{
    public delegate void OnCommonButtonDelegate();
    public static OnCommonButtonDelegate OnCommomButtonClickHandler;
    public delegate void OnButtonClickDelegate(SkillCategory skillType);
    public static OnButtonClickDelegate OnButtonClickedHandler;

    public Image spAshEx = null;
    public GameObject CommonSkillGo = null;
    public Image spAgeryValueEx = null;
    public Image spFullEx = null;
    public GameObject goEffect = null;
    public AnimationCurve tweenAnimation = null;


    private GameObject m_RuntimeEffect = null;
    private List<BoxCollider> m_GoList = new List<BoxCollider>();

    private bool m_IsPressed = false;
    public bool m_IsAttact = false;
    public float m_TipsCD = 0f;
    public float m_TipsDelta = 0.5f;

    private float time = 0.6f;
    private const int c_SkillNum = 4;
    private float[] remainCdTime = new float[c_SkillNum];
    private float[] skillsCDTime = new float[c_SkillNum];

    private SkillCategory[] m_CategoryArray = new SkillCategory[4]{
        SkillCategory.kSkillA,
        SkillCategory.kSkillB,
        SkillCategory.kSkillC,
        SkillCategory.kSkillD
      }; 

    private List<object> m_EventList = new List<object>();

    void Awake()
    {
        //object obj = LogicSystem.EventChannelForGfx.Subscribe<List<SkillInfo>>("ge_equiped_skills", "ui", InitSkillBar);
        //if (obj != null) m_EventList.Add(obj);
        //LogicSystem.PublishLogicEvent("ge_request_equiped_skills", "ui"); 
    }

	// Use this for initialization
	void Start() {
        if (null != CommonSkillGo)
        {
            EventTriggerListener.Get(CommonSkillGo).onDown = OnButtonDown;
            EventTriggerListener.Get(CommonSkillGo).onUp = OnButtonUp;
            EventTriggerListener.Get(transform.Find("Skill0/skill0").gameObject).onClick = OnButtonClick;
            EventTriggerListener.Get(transform.Find("Skill1/skill0").gameObject).onClick = OnButtonClick;
            EventTriggerListener.Get(transform.Find("Skill2/skill0").gameObject).onClick = OnButtonClick;
            EventTriggerListener.Get(transform.Find("Skill3/skill0").gameObject).onClick = OnButtonClick;
            EventTriggerListener.Get(transform.Find("Ex").gameObject).onClick = OnExButtonClick;
        }
            
        object obj = LogicSystem.EventChannelForGfx.Subscribe<string, float>("ge_cast_skill_cd", "ui", CastCDByGroup);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe<SkillCannotCastType>("ge_skill_cannot_cast", "ui", HandleCastSkillResult);
        if (obj != null) m_EventList.Add(obj);
        obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (obj != null) m_EventList.Add(obj);
	}

    void Update()
    {
        UpdateAngryValue();
        if (m_TipsCD >= 0f)
        {
            m_TipsCD -= Time.deltaTime;
        }

        //按钮不可点击时停止普通攻击
        if (CommonSkillGo != null)
        {
            Button btn = CommonSkillGo.GetComponent<Button>();
            if (!btn.enabled)
            {
                m_IsPressed = false;
            }
        }
        
        if(m_IsPressed)
        {
            //发送攻击
            m_IsAttact = true;
            GfxModule.Skill.GfxSkillSystem.Instance.StartAttack(LogicSystem.PlayerSelf, Vector3.zero);
        }
        else
        {
            if(m_IsAttact)
            {
                m_IsAttact = false;
                GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(DashFire.LogicSystem.PlayerSelf);
            }
        }
        if(time > 0)
        {
            //忽略缩放时间 使用每帧间隔的真实时间
            time -= RealTime.deltaTime;
            if(time <= 0)
            {
                foreach (BoxCollider collider in m_GoList)
                {
                    if(collider != null)
                    {
                        collider.enabled = true;
                    }
                }
                m_GoList.Clear();
            }
        }

        //刷新时间
        for (int index = 0; index < c_SkillNum; ++index)
        {
            if(remainCdTime[index] > 0)
            {
                string path = "Skill" + index.ToString() + "/skill0/CD";
                GameObject go = GetGoByPath(path);
                if(null != go)
                {
                    Image sp = go.GetComponent<Image>();
                    if (null != sp)
                    {
                        sp.fillAmount -= RealTime.deltaTime / GetCDTimeByIndex(index);
                        remainCdTime[index] = sp.fillAmount;
                        if (remainCdTime[index] <= 0)
                        {
                            IconFlashByIndex(index);
                        }
                    }
                }
            }
        }
    }

    void OnDisable()
    {
        if (m_RuntimeEffect != null)
        {
            Destroy(m_RuntimeEffect);
            m_RuntimeEffect = null;
        }
    }
    void OnDestroy()
    {
        if (m_RuntimeEffect != null)
        {
            Destroy(m_RuntimeEffect);
            m_RuntimeEffect = null;
        }
    }

    private void IconFlashByIndex(int index)
    {
        string path = "Skill" + index.ToString() + "/skill0/bright";
        GameObject go = GetGoByPath(path);
        if (go == null)
            return;
        NGUITools.SetActive(go, true);

        TweenAlpha alpha = go.GetComponent<TweenAlpha>();
        if (null != alpha)
            Destroy(alpha);

        alpha = go.AddComponent<TweenAlpha>();
        if (null == alpha)
            return;
        alpha.from = 0;
        alpha.to = 1;
        alpha.duration = 0.4f;
        //alpha.method = EaseType.none;
        alpha.animationCurve = tweenAnimation;

        //TweenAlpha tweenAlpha = TweenAlpha.Begin(go, 0, 1, 0.4f);
        //tweenAlpha.animationCurve = tweenAnimation;

        GameObject goSkill = GetGoByPath("Skill" + index.ToString() + "/skill0");
        if (null != goSkill)
        {
            Button button = goSkill.GetComponent<Button>();
            if (button != null) button.enabled = true;
        }
    }

    public float GetCDTimeByIndex(int index)
    {
        float ret = 0;
        ret = skillsCDTime[index];
        return ret;
    }

    public void CastCDByGroup(string group, float cdTime)
    {
        try
        {
            if (cdTime <= 0) return;
            int index = GetIndexByGroup(group);
            if (index == -1 || index >= c_SkillNum)
                return;
            skillsCDTime[index] = cdTime;
            string path = "Skill" + index.ToString() + "/skill0/CD";
            GameObject go = GetGoByPath(path);
            if (null != go)
            {
                Image sp = go.GetComponent<Image>();
                if (null != sp)
                {
                    sp.fillAmount = 1;
                    remainCdTime[index] = sp.fillAmount;
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public int GetIndexByGroup(string group)
    {
        int ret = -1;
        switch (group)
        {
            case "SkillA": ret = 0; break;
            case "SkillB": ret = 1; break;
            case "SkillC": ret = 2; break;
            case "SkillD": ret = 3; break;
            default: ret = -1; break;
        }
        return ret;
    }

    private GameObject GetGoByPath(string path)
    {
        GameObject go = null;
        Transform trans = this.transform.Find(path);
        if (null != trans)
        {
            go = trans.gameObject;
        }
        else
        {
            Debug.Log("Can not find " + path);
        }
        return go;
    }

    //更新怒气值
    public void UpdateAngryValue()
    {
        SharedGameObjectInfo share_info = LogicSystem.PlayerSelfInfo;
        if (share_info != null)
        {
            float value = share_info.Rage / share_info.MaxRage;
            if(spAgeryValueEx != null)
            {
                spAgeryValueEx.fillAmount = value;
            }
            if(value >= 1 && spFullEx != null)
            {
                NGUITools.SetActive(spFullEx.gameObject, true);
                //播放特效
                if (goEffect != null && m_RuntimeEffect==null && spAshEx!=null && NGUITools.GetActive(spAshEx.gameObject))
                {
                    m_RuntimeEffect = ResourceSystem.NewObject(goEffect) as GameObject;
                    if (m_RuntimeEffect != null)
                    {
                        m_RuntimeEffect.transform.position = spFullEx.transform.position;
                    }
                }
            }
            else
            {
                if(spFullEx != null)
                {
                    NGUITools.SetActive(spFullEx.gameObject, false);
                    if (m_RuntimeEffect != null)
                    {
                        Destroy(m_RuntimeEffect);
                        m_RuntimeEffect = null;
                    }
                }
            }
        }
    }

    //发送普通攻击消息
    private void OnButtonDown(GameObject obj, PointerEventData eventData)
    {
        //在update中触发技能
        LogicSystem.EventChannelForGfx.Publish("ge_attack", "game", true);
        m_IsPressed = true;
        if (OnCommomButtonClickHandler != null)
        {
            OnCommomButtonClickHandler();
        }
    }

    private void OnButtonUp(GameObject obj, PointerEventData eventData)
    {
        LogicSystem.EventChannelForGfx.Publish("ge_attack", "game", false);
        m_IsPressed = false;
        if (OnCommomButtonClickHandler != null)
        {
            OnCommomButtonClickHandler();
        }
    }

    public void OnExButtonClick(GameObject obj, PointerEventData eventData)
    {
        GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(DashFire.LogicSystem.PlayerSelf, SkillCategory.kEx, Vector3.zero);
    }

    private void OnButtonClick(GameObject obj, PointerEventData eventData)
    {
        if (EventSystem.current != null)
        {
            GameObject go = EventSystem.current.currentSelectedGameObject;
            if (go == null)
                return;
            Transform trans = go.transform.parent;
            if (trans == null)
                return;
            GameObject parentGo = trans.gameObject;
            string name = parentGo.name;
            SkillCategory skillType;
            switch (name)
            {
                case "Skill0": skillType = SkillCategory.kSkillA; break;
                case "Skill1": skillType = SkillCategory.kSkillB; break;
                case "Skill2": skillType = SkillCategory.kSkillC; break;
                case "Skill3": skillType = SkillCategory.kSkillD; break;
                default: skillType = SkillCategory.kNone; break;
            }
            //向上层逻辑发送释放技能的消息
            if (null != OnButtonClickedHandler)
            {
                OnButtonClickedHandler(skillType);
            }
            else
            {
                GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(DashFire.LogicSystem.PlayerSelf, skillType, Vector3.zero);
            }
        }
            
    }

    //关闭所有技能按钮
    public void InitSkillBar(List<SkillInfo> skillInfos)
    {
        try
        {
            foreach (SkillCategory category in m_CategoryArray)
            {
                UnlockSkill(category, false);
            }
            if (skillInfos != null)
            {
                foreach (SkillInfo skill_info in skillInfos)
                {
                    SkillCategory category = skill_info.ConfigData.Category;
                    UnlockSkill(category, true, skill_info.ConfigData);
                }
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void UnlockSkill(SkillCategory category, bool isActive, SkillLogicData skillData =null)
    {
        try
        {
            int index = GetIndexByGroup(category);
            string goPath = "Skill" + index.ToString();
            Transform ts = this.transform.Find(goPath);
            if(null != ts)
            {
                GameObject go = ts.gameObject;
                if(skillData != null)
                {
                    ts = go.transform.Find("skill0");
                    if(ts != null)
                    {
                        Image sp = ts.gameObject.GetComponent<Image>();
                        Button btn = ts.GetComponent<Button>();
                        if (btn != null && sp != null)
                        {
                            sp.sprite = NGUITools.GetResourceSpriteByName(skillData.ShowIconName);
                        }
                    }
                }

                NGUITools.SetActive(go, isActive);
            }
            else
            {
                Debug.Log("!!can not find " + goPath);
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public int GetIndexByGroup(SkillCategory category)
    {
        int ret = -1;
        switch (category)
        {
            case SkillCategory.kSkillA: ret = 0; break;
            case SkillCategory.kSkillB: ret = 1; break;
            case SkillCategory.kSkillC: ret = 2; break;
            case SkillCategory.kSkillD: ret = 3; break;
            default: ret = -1; break;
        }
        return ret;
    }

    public void HandleCastSkillResult(SkillCannotCastType result)
    {
        try
        {
            float x = Screen.width / 2f;
            float y = Screen.height * 0.5f;
            Vector3 screenPos = new Vector3(x, y, 0);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(DFMUiRoot.RootTransform as RectTransform, screenPos, DFMUiRoot.UiCanvas.worldCamera, out localPos);
            string CHN = StrDictionaryProvider.Instance.GetDictString(12);
            switch (result)
            {
                case SkillCannotCastType.kInCD:
                    break;
                case SkillCannotCastType.kCostNotEnough:
                    CHN = StrDictionaryProvider.Instance.GetDictString(13);
                    break;
                case SkillCannotCastType.kUnknow:
                    CHN = StrDictionaryProvider.Instance.GetDictString(14);
                    break;
                default:
                    CHN = StrDictionaryProvider.Instance.GetDictString(14);
                    break;
            }
            if (m_TipsCD <= 0f)
            {
                LogicSystem.EventChannelForGfx.Publish("ge_screen_tip", "ui", localPos.x, localPos.y, 0, false, CHN);
                m_TipsCD = m_TipsDelta;
            }
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
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
}
