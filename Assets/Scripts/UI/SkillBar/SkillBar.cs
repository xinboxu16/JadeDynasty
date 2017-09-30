using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBar : MonoBehaviour {

    private List<object> m_EventList = new List<object>();

    void Awake()
    {
        //object obj = LogicSystem.EventChannelForGfx.Subscribe<List<SkillInfo>>("ge_equiped_skills", "ui", InitSkillBar);
        //if (obj != null) m_EventList.Add(obj);
        //LogicSystem.PublishLogicEvent("ge_request_equiped_skills", "ui"); 
    }

	// Use this for initialization
	void Start () {
        //if (null != CommonSkillGo)
        //    UIEventListener.Get(CommonSkillGo).onPress = OnButtonPress;
        //object obj = LogicSystem.EventChannelForGfx.Subscribe<string, float>("ge_cast_skill_cd", "ui", CastCDByGroup);
        //if (obj != null) m_EventList.Add(obj);
        //obj = LogicSystem.EventChannelForGfx.Subscribe<SkillCannotCastType>("ge_skill_cannot_cast", "ui", HandleCastSkillResult);
        //if (obj != null) m_EventList.Add(obj);
        //obj = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        //if (obj != null) m_EventList.Add(obj);
	}
	
	// Update is called once per frame
	void Update () {
		
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
