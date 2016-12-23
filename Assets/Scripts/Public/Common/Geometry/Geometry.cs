using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public partial class Geometry
    {
        public const float c_FloatPrecision = 0.0001f;
        public static bool IsSameFloat(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < c_FloatPrecision;
        }
    }
}
