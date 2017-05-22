using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFireSpatial
{
    public enum SpatialObjType
    {
        kTypeBegin = 0,
        kUser,
        kNPC,
        kBullet,
        kTypeEnd
    }

    public struct CellPos
    {
        public CellPos(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public int row;
        public int col;
    }

    public interface ISpaceObject
    {
        uint GetID();
        SpatialObjType GetObjType();
        Vector3 GetPosition();
        float GetRadius();
        Vector3 GetVelocity();
        bool IsAvoidable();
        Shape GetCollideShape();                  // 取得碰撞形状
        List<ISpaceObject> GetCollideObjects();   // 取得与当前物体碰撞的物体
        void OnCollideObject(ISpaceObject obj);   // 增加与当前物体碰撞的物体
        void OnDepartObject(ISpaceObject obj);    // 删除与当前物体碰撞的物体
        object RealObject { get; }
    }
}
