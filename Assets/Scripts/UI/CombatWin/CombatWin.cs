using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatWin : MonoBehaviour {

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
        catch (System.Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

	// Use this for initialization
	void Start () {
        //未实现

        UIManager.Instance.HideWindowByName("CombatWin");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
