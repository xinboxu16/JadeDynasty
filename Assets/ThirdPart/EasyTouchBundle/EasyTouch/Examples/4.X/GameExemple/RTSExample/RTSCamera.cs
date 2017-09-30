using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;

public class RTSCamera : MonoBehaviour {

	private Vector3 delta;

	void OnEnable(){
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_Twist += On_Twist;
		EasyTouch.On_Pinch += On_Pinch;
	}


    void On_Twist(HedgehogTeam.EasyTouch.Gesture gesture)
    {

		transform.Rotate( Vector3.up * gesture.twistAngle);
	}

	void OnDestroy(){
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_Twist -= On_Twist;
	}


    void On_Drag(HedgehogTeam.EasyTouch.Gesture gesture)
    {
		On_Swipe( gesture);
	}

    void On_Swipe(HedgehogTeam.EasyTouch.Gesture gesture)
    {

		transform.Translate( Vector3.left * gesture.deltaPosition.x / Screen.width);
		transform.Translate( Vector3.back * gesture.deltaPosition.y / Screen.height);
	}

    void On_Pinch(HedgehogTeam.EasyTouch.Gesture gesture)
    {	
		Camera.main.fieldOfView += gesture.deltaPinch * Time.deltaTime;
	}

}
