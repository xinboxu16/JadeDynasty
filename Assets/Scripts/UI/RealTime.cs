using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime : MonoBehaviour {

    static RealTime mInst;
    float mRealTime = 0.0f;
    float mRealDelta = 0.0f;

    static public float time
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return Time.realtimeSinceStartup;
#endif
            if(mInst == null)
            {
                Spawn();
            }
            return mInst.mRealTime;
        }
    }

    static public float deltaTime
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return 0f;
#endif
            if(mInst == null)
            {
                Spawn();
            }
            return mInst.mRealDelta;
        }
    }

    static void Spawn()
    {
        GameObject go = new GameObject("_RealTime");
        DontDestroyOnLoad(go);
        mInst = go.AddComponent<RealTime>();
        mInst.mRealTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float rt = Time.realtimeSinceStartup;
        mRealDelta = Mathf.Clamp01(rt - mRealTime);
        mRealTime = rt;
    }
}
