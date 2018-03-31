using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReturnToMaincity : MonoBehaviour {

    public int offset = 10;

	// Use this for initialization
	void Awake () {
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
	}

    private void OnClick()
    {
        LogicSystem.SendStoryMessage("returntomaincity");//调用story的message
    }
}
