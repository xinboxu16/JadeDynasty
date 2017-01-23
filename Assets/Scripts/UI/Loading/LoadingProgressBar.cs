using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour {

    private bool sign1 = true;
    private bool sign2 = true;
    private bool sign3 = true;
    private float time = 0f;

    // Use this for initialization
    void Start () {
		
	}

    private void LateUpdate()
    {
        //if (JoyStickInputProvider.JoyStickEnable)
        //{
        //    JoyStickInputProvider.JoyStickEnable = false;
        //}
        Slider slider = transform.GetComponent<Slider>();
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
        //if (JoyStickInputProvider.JoyStickEnable)
        //{
        //    JoyStickInputProvider.JoyStickEnable = false;
        //}
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

            }
        }
    }

    void EndLoading()
    {
        sign2 = false;
        time = 0.0f;
    }
}
