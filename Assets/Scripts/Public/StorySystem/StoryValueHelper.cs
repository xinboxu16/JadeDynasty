using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    public static class StoryValueHelper
    {
        public static T CastTo<T>(object obj)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }
    }
}
