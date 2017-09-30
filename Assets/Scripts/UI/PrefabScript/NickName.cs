using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickName : MonoBehaviour {

    private List<object> eventlist = new List<object>();

    private GameObject playergo = null;
    private float height = 3.0f;
    private Text nicklabel = null;

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
            NGUITools.DestroyImmediate(gameObject);
        }
        catch (Exception ex)
        {
            DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

	// Use this for initialization
	void Start () {
        if (eventlist != null) { eventlist.Clear(); }
        object eo = DashFire.LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
        if (eo != null) eventlist.Add(eo);
	}
	
	// Update is called once per frame
	void Update () {
        //activeSelf（read only只读）:物体本身的active状态，对应于其在inspector中的checkbox是否被勾选
        //activeInHierarchy（read only只读）:物体在层次中是否是active的。也就是说要使这个值为true，这个物体及其所有父物体(及祖先物体)的activeself状态都为true。
		if(nicklabel != null && playergo != null && nicklabel.enabled != playergo.activeInHierarchy)
        {
            nicklabel.enabled = playergo.activeInHierarchy;
        }
	}

    void LateUpdate()
    {
        if (playergo != null && Camera.main != null)
        {
            Vector3 pos = playergo.transform.position;
            pos = Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y + height, pos.z));
            pos.z = 0;
            pos = transform.root.GetComponent<Canvas>().worldCamera.ScreenToWorldPoint(pos);
            gameObject.transform.position = pos;
        }
    }

    public void SetPlayerGameObjectAndNickName(GameObject go, string nickname, Color col)
    {
        playergo = go;
        Text ul = gameObject.GetComponent<Text>();
        if(ul != null && nickname != null)
        {
            ul.text = nickname;
            ul.color = col;
            nicklabel = ul;
        }

        Update();
        LateUpdate();
    }
}
