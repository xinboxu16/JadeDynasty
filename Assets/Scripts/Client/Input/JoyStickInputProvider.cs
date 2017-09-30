using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInputProvider : MonoBehaviour {

    private static bool m_JoyStickEnable = true;
    private static GameObject m_JoyStickObj = null;
    public GameObject m_JoyStick = null;

    void Awake()
    {
        //Awake调用顺序不同
        //GameObject parentObj = DFMUiRoot.RootTransform.FindChild("JoyStickWidget").gameObject;
        GameObject canvas = GameObject.Find("Canvas");
        GameObject parentObj = canvas.transform.FindChild("JoyStickWidget").gameObject;
        m_JoyStickObj = NGUITools.AddChild(parentObj, m_JoyStick);
        //Transform parent = GameObject.Find("Canvas/Widgets").transform;
        //m_JoyStickObj = GameObject.Instantiate(m_JoyStick, parent) as GameObject;
        //m_JoyStickObj.transform.position = Vector3.zero;
        //m_JoyStickObj.transform.localScale = Vector3.one;//导致添加在画布中心
    }

	// Use this for initialization
	void Start () {
		if(m_JoyStick != null)
        {
            if (m_JoyStickObj != null)
            {
                ETCJoystick joyStickScript = m_JoyStickObj.GetComponentInChildren<ETCJoystick>();
                if(joyStickScript != null)
                {
                    ETCInput.SetControlVisible(m_JoyStickObj.name, true);
                    ETCInput.SetControlVisible(m_JoyStickObj.name, false);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void OnDestroy()
    {
        if (null != m_JoyStickObj)
        {
            GameObject.Destroy(m_JoyStickObj);
        }
    }

    private static void ShowJoyStick()
    {
        if(m_JoyStickObj != null)
        {
            //m_JoyStickObj.SetActive(true);
            ETCInput.SetControlVisible(m_JoyStickObj.name, true);
            //ETCJoystick ej = ETCInput.GetControlJoystick("Joystick");
        }
    }

    private static void HideJoyStick()
    {
        if (m_JoyStickObj != null)
        {
            ETCInput.SetControlVisible(m_JoyStickObj.name, false);
        }
    }

    public static bool JoyStickEnable
    {
        get
        {
            return m_JoyStickEnable;
        }
        set
        {
            m_JoyStickEnable = value;
            if(m_JoyStickEnable)
            {
                ShowJoyStick();
            }
            else
            {
                HideJoyStick();
            }
        }
    }

    public static string JoyStickName
    {
        get
        {
            return m_JoyStickObj.name;
        }
    }

    public static ETCJoystick JoyStick
    {
        get
        {
            return ETCInput.GetControlJoystick(m_JoyStickObj.name);
        }
    }
}
