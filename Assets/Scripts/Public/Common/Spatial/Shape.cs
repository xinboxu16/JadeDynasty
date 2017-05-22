using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFireSpatial
{
    public class Shape : ICloneable
    {
        public virtual object Clone()
        {
            return new Shape();
        }

        public virtual float GetRadius()
        {
            return 1.0f;
        }
    }
}
