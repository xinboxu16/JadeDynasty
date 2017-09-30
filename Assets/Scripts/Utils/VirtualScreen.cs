using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualScreen : Singleton<VirtualScreen>
{
    private float virtualWidth = 960;
    private float virtualHeight = 540;

    public static float width = 960;
    public static float height = 540;

    public static float xRatio = 1.0f;
    public static float yRatio = 1.0f;

    public void ComputeScene(CanvasScaler canvasScaler)
    {
        width = virtualWidth;
        height = virtualHeight;

        float realRatio = (float)((float)Screen.width / (float)Screen.height);
        float standardRatio = width / height;//计算宽高比例
        if (realRatio < standardRatio)//计算矫正比例
        {
            height = Screen.height; 
            xRatio = (float)Screen.width / width;
            yRatio = (float)Screen.height / height;

            canvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            width = Screen.width;
            xRatio = (float)Screen.width / width;
            yRatio = (float)Screen.height / height;

            canvasScaler.matchWidthOrHeight = 1;
        }
        //if(Screen.width > Screen.height)
        //{
        //    realRatio = (float)((float)Screen.width / (float)Screen.height);

        //    height = width/realRatio;
        //    xRatio = (float)Screen.width / width;
        //    yRatio = (float)Screen.height / height;

        //    canvasScaler.matchWidthOrHeight = 1;
        //}
        //else
        //{
        //    realRatio = (float)((float)Screen.height / (float)Screen.width);
        //    width = height/realRatio;
        //    xRatio = (float)Screen.width / width;
        //    yRatio = (float)Screen.height / height;

        //    canvasScaler.matchWidthOrHeight = 0;
        //}

        canvasScaler.referenceResolution = new Vector2(virtualWidth, virtualHeight);
    }

	public static Rect GetRealRect(Rect rect)
    {
        return new Rect(rect.x * xRatio, rect.y * yRatio, rect.width * xRatio, rect.height * yRatio);
    }
}
