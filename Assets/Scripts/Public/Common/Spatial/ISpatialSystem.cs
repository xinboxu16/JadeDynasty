using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFireSpatial
{
    public interface ISpatialSystem
    {
        /// <summary>
        /// 将物体obj加入到空间系统新增列表，物体将在下一帧加入到管理队列中，
        /// 通过区域等查询接口也只能在下一帧后才能取到该物体
        /// </summary>
        /// <param name="obj">待新增的物体</param>
        /// <returns></returns>
        RetCode AddObj(ISpaceObject obj);
        /// <summary>
        /// 将物体obj加入到空间系统的删除缓冲列表, 物体将在当前帧之后被删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool RemoveObj(ISpaceObject obj);

        void VisitObjectInPolygon(IList<Vector3> polygon, MyAction<float, ISpaceObject> visitor);
        void VisitObjectInPolygon(IList<Vector3> polygon, MyFunc<float, ISpaceObject, bool> visitor);
        void VisitObjectInCircle(Vector3 center, float radius, MyAction<float, ISpaceObject> visitor);
        //void VisitObjectInCircle(Vector3 center, float radius, MyFunc<float, ISpaceObject, bool> visitor);

        /// <summary>
        /// 取得圆心在center位置，半径为radius的圆内的物体
        /// </summary>
        /// <param name="center">圆心位置</param>
        /// <param name="radius">半径</param>
        /// <returns>圆形区域内的物体</returns>
        List<ISpaceObject> GetObjectInCircle(Vector3 center, float radius);

        /// <summary>
        /// 查找从from到to的路径
        /// </summary>
        /// <param name="from">出发位置</param>
        /// <param name="to">结果位置</param>
        /// <param name="bodysize">物体所在网格的深度，如1时为1格，2时为二层9格</param>
        /// <returns>从from到to的路点，没有找到路径时为空</returns>
        List<Vector3> FindPath(Vector3 from, Vector3 to, int bodysize);

        /// <summary>
        /// 计算对象避让其它对象后的速度矢量
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prefDir">优先的移动方向，需要是单位向量</param>
        /// <param name="stepTime"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="neighborDist"></param>
        /// <param name="isUsingAvoidanceVelocity"></param>
        /// <returns></returns>
        Vector3 ComputeVelocity(ISpaceObject obj, Vector3 prefDir, float stepTime, float maxSpeed, float neighborDist, bool isUsingAvoidanceVelocity);



        /// <summary>
        /// 判断物体obj是否可以走到to的位置，检测物体是否被阻挡挡住
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="to">所要去的位置</param>
        /// <returns>过以走到to时返回true, 反之返回false</returns>
        bool CanPass(ISpaceObject obj, Vector3 to);
        bool CanPass(Vector3 from, Vector3 to);


        ICellMapView GetCellMapView(int radius);
    }
}
