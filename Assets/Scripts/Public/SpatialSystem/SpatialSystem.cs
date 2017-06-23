using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashFire;
using UnityEngine;

namespace DashFireSpatial
{
    public sealed class SpatialSystem : ISpatialSystem
    {
        private Dictionary<uint, ISpaceObject> space_obj_collection_dict_ = new Dictionary<uint, ISpaceObject>();
        private Dictionary<uint, ISpaceObject> delete_obj_buffer_ = new Dictionary<uint, ISpaceObject>();
        private Dictionary<uint, ISpaceObject> add_obj_buffer_ = new Dictionary<uint, ISpaceObject>();

        private CellManager cell_manager_ = new CellManager();

        private KdTree m_KdTree = new KdTree();
        private KdObstacleTree m_KdObstacleTree = new KdObstacleTree();
        private PointKdTree m_ReachableSet = new PointKdTree();

        private string map_file_ = "";

        //构造地图
        public void Init(string map_file, Vector3[] reachableSet)
        {
            map_file_ = map_file;
            if (!cell_manager_.Init(DashFire.HomePath.GetAbsolutePath(map_file_)))
            {
                cell_manager_.Init(1024, 1024, 0.5f);
                LogSystem.Error("Init SpatialSystem from map file failed: {0}", map_file_);
            }
            if (null != reachableSet)
            {
                m_ReachableSet.Clear();
                m_ReachableSet.Build(reachableSet);
            }
        }

        public void LoadPatch(string patch_file)
        {
            MapPatchParser patchParser = new MapPatchParser();
            patchParser.Load(DashFire.HomePath.GetAbsolutePath(patch_file));
            patchParser.VisitPatches((int row, int col, byte obstacle) =>
            {
                SetCellStatus(row, col, obstacle);
            });
        }

        public void LoadObstacle(string file,float scale)
        {
            m_KdObstacleTree.Build();
        }

        public void SetCellStatus(int row, int col, byte status)
        {
            cell_manager_.SetCellStatus(row, col, status);
        }

        public void Reset()
        {
            space_obj_collection_dict_.Clear();
            delete_obj_buffer_.Clear();
            add_obj_buffer_.Clear();
            cell_manager_.Reset();
            m_KdTree.Clear();
            m_KdObstacleTree.Clear();
        }

        /** 
         * 删除物体obj
         * @param objid 删除的物体
         * @return 成功返回true, 失败返回false
         */
        public bool RemoveObj(ISpaceObject obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(delete_obj_buffer_.ContainsKey(CalcSpaceObjectKey(obj)))
            {
                return false;
            }
            delete_obj_buffer_.Add(CalcSpaceObjectKey(obj), obj);
            return false;
        }

        private uint CalcSpaceObjectKey(ISpaceObject obj)
        {
            //注意：这里假设obj.GetID()<0x10000000，应该不会有超过这个的id
            return ((uint)obj.GetObjType() << 24) | obj.GetID();
        }
    }
}
