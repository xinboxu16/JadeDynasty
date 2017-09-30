using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsVictory : MonoBehaviour {

    private List<object> eventlist = new List<object>();
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
            }
            eventlist.Clear();
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

	// Use this for initialization
	void Start () {
        //未实现

        UIManager.Instance.HideWindowByName("MarsVictory");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
