using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatFalse : MonoBehaviour {

    private List<object> m_EventList = new List<object>();
    public void UnSubscribe()
    {
        try
        {
            foreach (object obj in m_EventList)
            {
                if (null != obj)
                {
                    DashFire.LogicSystem.EventChannelForGfx.Unsubscribe(obj);
                }
            }
            m_EventList.Clear();
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

	// Use this for initialization
	void Start () {
        //未实现

        UIManager.Instance.HideWindowByName("CombatFalse");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
