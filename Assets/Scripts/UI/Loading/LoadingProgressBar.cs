using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour {

    private bool sign1 = true;
    private bool sign2 = true;
    private bool sign3 = true;
    private float time = 0f;

    private Slider slider = null;
    // Use this for initialization
    void Awake () {
        slider = transform.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate(float dt) { (slider.transform.Find("Label").GetComponent<Text>()).text = (dt * 100).ToString() + "%"; });
	}

    private void LateUpdate()
    {
        if (JoyStickInputProvider.JoyStickEnable)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        float progressValue = DashFire.LogicSystem.GetLoadingProgress();
        if (slider != null)
        {
            slider.value = progressValue;
        }
        Transform tipObj = gameObject.transform.Find("Tip");
        if (tipObj != null)
        {
            Text tipLabel = tipObj.gameObject.GetComponent<Text>();
            if (tipLabel != null)
            {
                tipLabel.text = DashFire.LogicSystem.GetLoadingTip();
            }
        }
        if (progressValue >= 0.9999f)
        {
            Transform tf = gameObject.transform.Find("Handle Slide Area/Panel/Icon");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (JoyStickInputProvider.JoyStickEnable)
        {
            JoyStickInputProvider.JoyStickEnable = false;
        }
        if (sign1 && DashFire.LogicSystem.GetLoadingProgress() > 0)
        {
            sign1 = false;
            Transform tf = gameObject.transform.Find("Handle Slide Area/Icon");
            if (tf != null)
            {
                NGUITools.SetActive(tf.gameObject, true);
            }
        }

        if (sign3)
        {
            if (!sign2)
            {
                time += RealTime.deltaTime;//每帧的变化
                if (time > 2.0f)
                {
                    DestoryLoading();
                }
                else
                {
                    if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
                    {
                        //DestoryLoading();
                        sign3 = false;
                        time = 0f;
                    }
                }
            }
        }
        else
        {
            time += RealTime.deltaTime;
            if (time >= 2.0f)
            {
                DestoryLoading();
            }
        }
    }

    public void EndLoading()
    {
        Debug.Log("EndLoading");
        sign2 = false;
        time = 0.0f;
    }

    void DestoryLoading()
    {
        if (InputType.Joystick == DFMUiRoot.InputMode)
        {
            JoyStickInputProvider.JoyStickEnable = UIManager.Instance.IsUIVisible;
        }

        sign1 = true;
        sign2 = true;
        sign3 = true;
        time = 0f;
        slider.onValueChanged.RemoveAllListeners();
        NGUITools.DestroyImmediate(transform.parent.gameObject);
    }
}
