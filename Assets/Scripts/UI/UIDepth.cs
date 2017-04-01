using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDepth : MonoBehaviour {

    public int order;
    public bool isUI = true;

	// Use this for initialization
	void Start () {
		if(isUI)
        {
            Canvas canvas = GetComponent<Canvas>();
            if(canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
        }
        else
        {
            Renderer[] render = GetComponentsInChildren<Renderer>();
            int size = render.Length;
            for(int i = 0; i < size; i++)
            {
                render[i].sortingOrder = order;
            }
        }
	}
}
