using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideDlg : MonoBehaviour {

    public Text lblDesc = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDescription(int descId)
    {
        string chn_des = DashFire.StrDictionaryProvider.Instance.GetDictString(descId);
        if (lblDesc != null)
            lblDesc.text = chn_des;
    }
}
