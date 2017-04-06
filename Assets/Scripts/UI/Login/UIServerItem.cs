using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIServerItem : MonoBehaviour {

    public Text lblId = null;
    public Text lblName = null;
    public Image lblState = null;

    private int m_ServerId = 0;

    public int ServerId
    {
        get { return m_ServerId; }
        set { m_ServerId = value; }
    }

    void Start()
    {
 
    }

    void Update()
    {

    }

    public void OnClick()
    {
        UIServerSelect serverSelect = transform.GetComponentInParent<UIServerSelect>();
        if(serverSelect != null)
        {
            serverSelect.TweenUpwards(ServerId);
        }
    }

    public void SetServerInfo(int id, string serverName, string state)
    {
        ServerId = id;
        if (lblId != null)
        {
            StrDictionary strDic = StrDictionaryProvider.Instance.GetDataById(201);
            if(strDic != null)
            {
                lblId.text = "" + id + strDic.m_String;
            }
            else
            {
                lblId.text = "" + id;
            }
        }

        if(lblName != null)
        {
            lblName.text = "" + serverName;
        }

        if(lblState != null)
        {
            lblState.sprite = Resources.Load<GameObject>("Sprite/" + state).GetComponent<SpriteRenderer>().sprite;
        }
    }
}
