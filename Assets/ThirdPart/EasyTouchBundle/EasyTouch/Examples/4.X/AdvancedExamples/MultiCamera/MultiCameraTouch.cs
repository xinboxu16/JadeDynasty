using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HedgehogTeam.EasyTouch;

public class MultiCameraTouch : MonoBehaviour {

	public Text label;

	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
	}
	
	void OnDestroy(){
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}

    void On_TouchDown(HedgehogTeam.EasyTouch.Gesture gesture)
	{
		if (gesture.pickedObject != null){
			label.text = "You touch : " + gesture.pickedObject.name + " on camera : " + gesture.pickedCamera.name;
		}
	}

    void On_TouchUp(HedgehogTeam.EasyTouch.Gesture gesture)
	{
		label.text = "";
	}
}
