using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyTalk : MonoBehaviour {

    private AndroidJavaClass ajc;
    private AndroidJavaObject ajo;

    public Button StartButton;
    public Text ResultText;

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        ResultText.text = "start";
        ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity"); 
        if (StartButton)
        {
            StartButton.onClick.AddListener(() => { StartListening(); });
        }
        Debug.Log("call end");
        ResultText.text = "end";
	}

    public void StartListening()
    {
        ResultText.text = "StartListening start";
		ajo.Call("StartSpeechListener");  
        ResultText.text = "StartListening end";
    }

    public void OnStartListening(string ret)
    {
        int result = int.Parse(ret);
        StartButton.interactable = result == 0;
    }

    public void Result(string ret)
    {
        ResultText.text = ret;
    }

    public void OnResult(string ret)
    {
        ResultText.text = ret;
    }

    public void OnError(string errorMessage)
    {
        ResultText.text = errorMessage;
    }

    public void OnEvent(string eventMsg)
    {
        ResultText.text = eventMsg;
    }

    public void OnEndOfSpeech()
    {
        StartButton.GetComponentInChildren<Text>().text = "已结束，点击聆听";
        StartButton.interactable = true;
    }

    public void OnBeginOfSpeech()
    {
        StartButton.GetComponentInChildren<Text>().text = "聆听ing";
        StartButton.interactable = false;
    }


    public void OnStartSpeaking(string ret)
    {
        ResultText.text = "OnStartSpeaking" + ret;
    }

    public void OnCompleted(string ret)
    {
        ResultText.text = ret;
    }
}
