using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D), typeof(LineRenderer))]
public class PolygonImage : Image {

    PolygonCollider2D polygonCollider = null;
    LineRenderer lineRender = null;

    new void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        DrawLine();
        return ContainsPoint(polygonCollider.points, screenPoint);
    }

    //多边形顶点，屏幕点击坐标
    bool ContainsPoint(Vector2[] polyPoints, Vector2 screenPoint)
    {
        int j = polyPoints.Length - 1;
        bool inside = false;

        for (int i = 0; i < polyPoints.Length; i++)
        {
            polyPoints[i].x += transform.position.x;
            polyPoints[i].y += transform.position.y;
            if (((polyPoints[i].y < screenPoint.y && screenPoint.y <= polyPoints[j].y) || (polyPoints[j].y < screenPoint.y && screenPoint.y <= polyPoints[i].y)) &&
            (polyPoints[i].x + (screenPoint.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) * (polyPoints[j].x - polyPoints[i].x)) > screenPoint.x)
                inside = !inside;

            j = i;
        }
        return inside;
    }

    //画出多边形区域
    void DrawLine()
    {
        lineRender.numPositions = polygonCollider.points.Length;
        for (int i = 0; i < polygonCollider.points.Length; i++)
        {
            float x = polygonCollider.points[i].x + transform.position.x;
            float y = polygonCollider.points[i].y + transform.position.y;
            Vector3 a = new Vector3(x, y, 0);
            lineRender.SetPosition(i, a);
        }
    }
}
