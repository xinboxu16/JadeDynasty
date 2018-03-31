using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uTools;

public class BloodAnimation : MonoBehaviour {

    public enum Direction
    {
        UP,
        DOWN
    }

    Vector3 textPos = Vector3.zero;
    Transform tf = null;


    private void Awake()
    {
        tf = transform.Find("Text");
        textPos = tf.localPosition;
    }

    public void AnimaionFinish()
    {
        DashFire.ResourceSystem.RecycleObject(gameObject);
    }
 
    public void PlayAnimation(Direction direction = Direction.DOWN)
    {
        if (tf != null)
        {
            tf.localPosition = textPos;
            tf.localScale = Vector3.zero;
            TweenScale tween = TweenScale.Begin(tf.gameObject, new Vector3(0, 0, 0), new Vector3(1, 1, 1), 0.5f, 0);
            if (tween != null)
            {
                tween.method = EaseType.easeInOutBack;
                //tween.animationCurve = CurveForUpwards;
                UnityEvent unityEvent = new UnityEvent();
                unityEvent.RemoveAllListeners();
                unityEvent.AddListener(() =>
                {
                    TweenPosition tween1 = TweenPosition.Begin(tf.gameObject, new Vector3(0, 0, 0), direction == Direction.DOWN ? new Vector3(0, -100, 0) : new Vector3(0, 100, 0), 1.0f, 0);
                    tween1.method = EaseType.linear;
                    //tween.animationCurve = CurveForUpwards;
                    UnityEvent unityEvent1 = new UnityEvent();
                    unityEvent1.RemoveAllListeners();
                    unityEvent1.AddListener(AnimaionFinish);
                    tween1.SetOnFinished(unityEvent1);
                });
                tween.SetOnFinished(unityEvent);
            }
        }
    }
}
