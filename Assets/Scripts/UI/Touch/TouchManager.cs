using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : FingerGestures 
{
    private static bool gestureEnable = true;

    public static bool GestureEnable
    {
        get
        {
            return gestureEnable;
        }
        set
        {
            gestureEnable = value;
        }
    }

    public static bool TouchEnable { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void CheckInit()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            TouchEnable = true;
        }
        else if (instance != this)
        {
            // 保持只有一个TouchManager实例
            Debug.LogWarning("There is already an instance of FingerGestures created (" + instance.name + "). Destroying new one.");
            Destroy(this.gameObject);
            TouchEnable = false;
            return;
        }
    }
}
