using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayInvoke : MonoBehaviour {

    private Action _action = null;

    private void InvokeAction()
    {
        _action();
    }

	public void Delay(Action action, int time)
    {
        _action = action;

        Invoke("InvokeAction", time);
    }
}
