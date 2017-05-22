using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFireSpatial
{
    public interface ISpatialSystem
    {
        /// <summary>
        /// 将物体obj加入到空间系统的删除缓冲列表, 物体将在当前帧之后被删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool RemoveObj(ISpaceObject obj);
    }
}
