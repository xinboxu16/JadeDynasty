using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
        JoyStickInputProvider.JoyStick.onMoveStart.AddListener(OnJoystickMoveStart);
        JoyStickInputProvider.JoyStick.onMove.AddListener(OnJoystickMove);
        JoyStickInputProvider.JoyStick.onMoveEnd.AddListener(OnJoystickMoveEnd);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnJoystickMoveStart()
    {
        Debug.Log("OnJoystickMoveStart");
    }

    //pos相对于thumb轮盘的位置 x -1到1 y -1到1
    private void OnJoystickMove(Vector2 pos)
    {
        //Debug.Log("OnJoystickMove" + pos);
        if(TouchManager.Touches.Count > 0)
        {
            TriggerMove(pos, false);
        }
    }

    private void OnJoystickMoveEnd()
    {
        Debug.Log("OnJoystickMoveEnd");
        TriggerMove(Vector2.zero, true);
    }

    private void TriggerMove(Vector2 deltaPos, bool isLift)
    {
        if (isLift)
        {
            GestureArgs e = new GestureArgs();
            e.name = "OnSingleTap";
            e.airWelGamePosX = 0f;
            e.airWelGamePosY = 0f;
            e.airWelGamePosZ = 0f;
            e.selectedObjID = -1;
            e.towards = -1;
            e.inputType = InputType.Joystick;
            LogicSystem.SetJoystickInfo(e);
            return;
        }

        GameObject playerSelf = LogicSystem.PlayerSelf;

        ETCJoystick move = JoyStickInputProvider.JoyStick;
        if (playerSelf != null && deltaPos != Vector2.zero)
        {
            Vector2 joyStickDir = deltaPos * 10.0f;
            Vector3 targetRot = new Vector3(joyStickDir.x, 0, joyStickDir.y);
            Vector3 targetPos = playerSelf.transform.position + targetRot;

            GestureArgs e = new GestureArgs();
            e.name = "OnSingleTap";
            e.selectedObjID = -1;
            float towards = Mathf.Atan2(targetPos.x - playerSelf.transform.position.x, targetPos.z - playerSelf.transform.position.z);
            e.towards = towards;
            e.airWelGamePosX = targetPos.x;
            e.airWelGamePosY = targetPos.y;
            e.airWelGamePosZ = targetPos.z;
            e.inputType = InputType.Joystick;
            LogicSystem.SetJoystickInfo(e);
        }
    }
}
