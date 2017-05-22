using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public partial class Geometry
    {
        public const float c_FloatPrecision = 0.0001f;
        public static bool IsSameFloat(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < c_FloatPrecision;
        }

        public static float DistanceSquare(Vector3 p1, Vector3 p2)
        {
            return (p1.x - p2.x) * (p1.x - p2.x) + (p1.z - p2.z) * (p1.z - p2.z);
        }

        public static float Distance(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(DistanceSquare(p1, p2));
        }

        /// <summary>
        /// 求垂足
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector3 Perpendicular(Vector3 p, Vector3 p1, Vector3 p2)
        {
            float r = Relation(p, p1, p2);
            Vector3 tp = new Vector3();
            tp.x = p1.x + r * (p2.x - p1.x);
            tp.z = p1.z + r * (p2.z - p1.z);
            return tp;
        }

        /// <summary>
        /// 求点到线段的最小距离并返回距离最近的点。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="np"></param>
        /// <returns></returns>
        public static float PointToLineSegmentDistance(Vector3 p, Vector3 p1, Vector3 p2, out Vector3 np)
        {
            float r = Relation(p, p1, p2);
            if (r < 0)
            {
                np = p1;
                return Distance(p, p1);
            }
            if (r > 1)
            {
                np = p2;
                return Distance(p, p2);
            }
            np = Perpendicular(p, p1, p2);
            return Distance(p, np);
        }

        /// <summary>
        /// 判断p与线段p1p2的关系
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>
        /// r=0 p = p1
        /// r=1 p = p2
        /// r<0 p is on the backward extension of p1p2
        /// r>1 p is on the forward extension of p1p2
        /// 0<r<1 p is interior to p1p2
        /// </returns>
        public static float Relation(Vector3 p, Vector3 p1, Vector3 p2)
        {
            return DotMultiply(p, p2, p1) / DistanceSquare(p1, p2);
        }

        /// <summary>
        /// r=DotMultiply(p1,p2,p0),得到矢量(p1-p0)和(p2-p0)的点积。
        /// 注：两个矢量必须是非零矢量
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p0"></param>
        /// <returns>
        /// r>0:两矢量夹角为锐角；
        /// r=0：两矢量夹角为直角；
        /// r<0:两矢量夹角为钝角
        /// </returns>
        public static float DotMultiply(Vector3 p1, Vector3 p2, Vector3 p0)
        {
            return ((p1.x - p0.x) * (p2.x - p0.x) + (p1.z - p0.z) * (p2.z - p0.z));
        }
    }
}
