using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

    public class Circle : Shape
    {
        private Vector3 relate_center_pos_;    //相对的圆心位置
        private Vector3 world_center_pos_;     //世界圆心位置
        private float radius_ = 0;

        public Circle(Vector3 centerpos, float radius)
        {
            relate_center_pos_ = centerpos;
            world_center_pos_ = relate_center_pos_;
            radius_ = radius;
        }

        public override float GetRadius()
        {
            return radius_;
        }

        public override object Clone()
        {
            return new Circle(relate_center_pos_, radius_);
        }

        public float radius() { return radius_; }
    }

    public class Line : Shape
    {
        public virtual List<Vector3> world_vertex() { return world_vertex_; }
        protected List<Vector3> world_vertex_ = new List<Vector3>();
        private Vector3 start_;
        private Vector3 end_;
        private Vector3 center_;
        private float radius_ = 0;

        public Line(Vector3 start, Vector3 end)
        {
            start_ = start;
            end_ = end;
            center_ = (start_ + end_) / 2;
            radius_ = Geometry.Distance(start_, end_) / 2;
        }
    }

    public class Polygon : Shape
    {
        protected List<Vector3> relation_vertex_;
        protected List<Vector3> world_vertex_;
        private bool need_recalc_ = true;
        private Vector3 center_;
        private float radius_ = 0;

        public void AddVertex(Vector3 pos)
        {
            if (relation_vertex_ == null)
            {
                relation_vertex_ = new List<Vector3>();
            }
            relation_vertex_.Add(pos);
            MarkRecalc();
        }

        public void AddVertex(float x, float y)
        {
            if (relation_vertex_ == null)
            {
                relation_vertex_ = new List<Vector3>();
            }
            relation_vertex_.Add(new Vector3((float)x, 0, (float)y));
            MarkRecalc();
        }

        protected void MarkRecalc()
        {
            need_recalc_ = true;
        }
    }

    public class Rect : Polygon
    {
        public float width() { return width_; }
        public float height() { return height_; }

        private float width_;
        private float height_;

        public Rect(float width, float height)
        {
            this.width_ = width;
            this.height_ = height;
            relation_vertex_ = new List<Vector3>();
            world_vertex_ = new List<Vector3>();
            AddVertex(-width / 2, height / 2);
            AddVertex(-width / 2, -height / 2);
            AddVertex(width / 2, -height / 2);
            AddVertex(width / 2, height / 2);
        }

        /*
         * 创建以pos为中心，width为宽度， height为高度的矩形
         */
        public Rect(Vector3 pos, float width, float height)
        {
            this.width_ = width;
            this.height_ = height;
            relation_vertex_ = new List<Vector3>();
            world_vertex_ = new List<Vector3>();
            AddVertex(pos.x - width / 2, pos.z + height / 2);
            AddVertex(pos.x - width / 2, pos.z - height / 2);
            AddVertex(pos.x + width / 2, pos.z - height / 2);
            AddVertex(pos.x + width / 2, pos.z + height / 2);
        }
    }
}
