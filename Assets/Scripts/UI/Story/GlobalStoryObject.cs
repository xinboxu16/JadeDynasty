using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStoryObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerselfScale(object[] args)
    {
        if(null != args && 3 == args.Length)
        {
            float x = (float)args[0];
            float y = (float)args[1];
            float z = (float)args[2];

            GameObject playerSelf = LogicSystem.PlayerSelf;
            if(null != playerSelf)
            {
                playerSelf.transform.localScale = new Vector3(x, y, z);
            }
        }
    }

    public void SetPlayerselfPosition(object[] args)
    {
        if (null != args && 3 == args.Length)
        {
            float x = (float)args[0];
            float y = (float)args[1];
            float z = (float)args[2];
            GameObject playerself = LogicSystem.PlayerSelf;
            if (null != playerself)
            {
                LogicSystem.NotifyGfxMoveControlStart(playerself, 0, false);
                LogicSystem.NotifyGfxUpdatePosition(playerself, x, y, z);
                playerself.transform.position = new Vector3(x, y, z);
                LogicSystem.NotifyGfxMoveControlFinish(playerself, 0, false);
            }
        }
    }
}
