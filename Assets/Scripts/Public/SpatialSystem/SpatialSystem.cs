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
        private RvoAlgorithm m_RvoAlgorithm = new RvoAlgorithm();

        private string map_file_ = "";

        private const long c_CountInterval = 10000;
        private long m_LastCountTime = 0;

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

        /** 
         * collide module 心跳tick, 处理相关计算
         */
        public void Tick()
        {
            // remove objects in delete buffer
            foreach (KeyValuePair<uint, ISpaceObject> pair in delete_obj_buffer_)
            {
                // 删除与当前物体碰撞的物体信息
                ISpaceObject obj2 = pair.Value;
                foreach(ISpaceObject spaceObj in obj2.GetCollideObjects())
                {
                    spaceObj.OnDepartObject(obj2);
                }
                obj2.GetCollideObjects().Clear();
                space_obj_collection_dict_.Remove(pair.Key);
            }
            delete_obj_buffer_.Clear();

            // add new obj
            foreach (KeyValuePair<uint, ISpaceObject> pair in add_obj_buffer_)
            {
                if(!space_obj_collection_dict_.ContainsKey(pair.Key))
                {
                    space_obj_collection_dict_.Add(pair.Key, pair.Value);
                }
            }
            add_obj_buffer_.Clear();

            IList<ISpaceObject> obj_list = null;
            if(space_obj_collection_dict_.Count > 0)
            {
                ISpaceObject[] temp = new ISpaceObject[space_obj_collection_dict_.Count];
                space_obj_collection_dict_.Values.CopyTo(temp, 0);
                obj_list = temp;
            }
            else
            {
                obj_list = new List<ISpaceObject>();
            }

            //构造空间索引
            if(obj_list.Count > 0)
            {
                m_KdTree.Build(obj_list);
            }
            else
            {
                m_KdTree.Clear();
            }

            bool isCountTick = false;
            long curTime = TimeUtility.GetServerMilliseconds();
            if(m_LastCountTime + c_CountInterval < curTime)
            {
                m_LastCountTime = curTime;
                isCountTick = true;
            }

            int userCt = 0;
            int npcCt = 0;
            int bulletCt = 0;
            foreach(ISpaceObject hiter in obj_list)
            {
                if(isCountTick)
                {
                    switch(hiter.GetObjType())
                    {
                        case SpatialObjType.kUser:
                            ++userCt;
                            break;
                        case SpatialObjType.kNPC:
                            ++npcCt;
                            break;
                        case SpatialObjType.kBullet:
                            ++bulletCt;
                            break;
                    }
                }
            }
            //if (isCountTick && !GlobalVariables.Instance.IsClient)
            //{
            //    LogSystem.Debug("SpatialSystem object count:{0} user:{1} npc:{2} bullet:{3}", obj_list.Count, userCt, npcCt, bulletCt);
            //}
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
         * 增加物体
         * @param obj 增加的物体
         * @return 成功返回SUCCESS，失败返回其它错误码
         */
        public RetCode AddObj(ISpaceObject obj)
        {
            if (obj == null)
            {
                return RetCode.NULL_POINTER;
            }
            if (add_obj_buffer_.ContainsKey(CalcSpaceObjectKey(obj)))
            {
                return RetCode.OBJECT_EXIST;
            }
            add_obj_buffer_.Add(CalcSpaceObjectKey(obj), obj);
            return RetCode.SUCCESS;
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

        public List<Vector3> FindPath(Vector3 from, Vector3 to, int bodysize)
        {
            List<Vector3> result = new List<Vector3>();
            CellPos start, end;
            ICellMapView view = cell_manager_.GetCellMapView(bodysize);
            CellPos start0;
            view.GetCell(from, out start0.row, out start0.col);
            view.GetCell(to, out end.row, out end.col);
            start = view.GetFirstWalkableCell(from, to);
            if (start.row >= 0 && start.col >= 0)
            {
                List<CellPos> path = PathFinder.FindPath(cell_manager_.GetCellMapView(bodysize), start, end);
                if (start0.row != start.row || start0.col != start.col)
                    result.Add(view.GetCellCenter(start0.row, start0.col));
                for (int ix = path.Count - 1; ix >= 0; --ix)
                {
                    CellPos pos = path[ix];
                    result.Add(view.GetCellCenter(pos.row, pos.col));
                }
            }
            else
            {
                result.Add(view.GetCellCenter(start0.row, start0.col));
                result.Add(view.GetCellCenter(end.row, end.col));
            }
            return result;
        }

        public bool CanPass(Vector3 curPos, Vector3 to)
        {
            CellPos cur_cell;
            cell_manager_.GetCell(curPos, out cur_cell.row, out cur_cell.col);

            CellPos to_cell;
            cell_manager_.GetCell(to, out to_cell.row, out to_cell.col);

            if(cur_cell.row == to_cell.row && cur_cell.col == to_cell.col)
            {
                return true;
            }

            byte curStatus = cell_manager_.GetCellStatus(cur_cell.row, cur_cell.col);
            byte block = BlockType.GetBlockType(curStatus);
            byte subType = BlockType.GetBlockSubType(curStatus);
            if(block != BlockType.NOT_BLOCK && subType != BlockType.SUBTYPE_ENERGYWALL)
            {
                return true;
            }

            byte status = cell_manager_.GetCellStatus(to_cell.row, to_cell.col);
            block = BlockType.GetBlockType(status);
            subType = BlockType.GetBlockSubType(status);
            if (block != BlockType.NOT_BLOCK && subType != BlockType.SUBTYPE_ENERGYWALL)
            {
                //LogSystem.Debug("CanPass ({0},{1})->({2},{3}), target is blocked {4}", cur_cell.row, cur_cell.col, to_cell.row, to_cell.col, block);
                return false;
            }

            if (Math.Abs(cur_cell.row - to_cell.row) >= 1 || Math.Abs(cur_cell.col - to_cell.col) >= 1)
            {
                Vector3 from = cell_manager_.GetCellCenter(cur_cell.row, cur_cell.col);
                to = cell_manager_.GetCellCenter(to_cell.row, to_cell.col);

                bool ret = true;
                cell_manager_.VisitCellsCrossByLine(from, to, (row, col) =>
                {
                    status = cell_manager_.cells_arr_[row, col];
                    block = BlockType.GetBlockType(status);
                    subType = BlockType.GetBlockSubType(status);
                    if (block != BlockType.NOT_BLOCK && subType != BlockType.SUBTYPE_ENERGYWALL)
                    {
                        ret = false;
                        //LogSystem.Debug("CanPass ({0},{1})->({2},{3}), ({4},{5}) is blocked {6}", cur_cell.row, cur_cell.col, to_cell.row, to_cell.col, row, col, block);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });
                return ret;
            }
            return true;
        }

        public bool CanPass(ISpaceObject obj, Vector3 to)
        {
            return CanPass(obj.GetPosition(), to);
        }

        //使用RVO避让算法 计算避让障碍后的速度指向
        public Vector3 ComputeVelocity(ISpaceObject obj, Vector3 prefDir, float stepTime, float maxSpeed, float neighborDist, bool isUsingAvoidanceVelocity)
        {
            Vector3 newVelocity = m_RvoAlgorithm.ComputeNewVelocity(obj, prefDir, stepTime, m_KdTree, m_KdObstacleTree, maxSpeed, neighborDist, isUsingAvoidanceVelocity);
            return newVelocity;
        }

        public ICellMapView GetCellMapView(int radius)
        {
            return cell_manager_.GetCellMapView(radius);
        }

        public void VisitObjectInPolygon(IList<Vector3> polygon, MyAction<float,ISpaceObject> visitor)
        {
            Vector3 centroid;
            float radius = Geometry.CalcPolygonCentroidAndRadius(polygon, 0, polygon.Count, out centroid);
            if (radius > Geometry.c_FloatPrecision)
            {
                m_KdTree.Query(centroid, radius, (float distSqr, KdTreeObject kdObj) =>
                {
                    ISpaceObject obj = kdObj.SpaceObject;
                    if (null != obj)
                    {
                        if (Geometry.PointInPolygon(kdObj.Position, polygon, 0, polygon.Count) >= 0)
                        {
                            visitor(distSqr, obj);
                        }
                    }
                });
            }
        }

        public void VisitObjectInPolygon(IList<Vector3> polygon, MyFunc<float, ISpaceObject, bool> visitor)
        {
            Vector3 centroid;
            float radius = Geometry.CalcPolygonCentroidAndRadius(polygon, 0, polygon.Count, out centroid);

            if (radius > Geometry.c_FloatPrecision)
            {
                m_KdTree.Query(centroid, radius, (float distSqr, KdTreeObject kdObj) =>
                {
                    ISpaceObject obj = kdObj.SpaceObject;
                    if (null != obj)
                    {
                        if (Geometry.PointInPolygon(kdObj.Position, polygon, 0, polygon.Count) >= 0)
                        {
                            return visitor(distSqr, obj);
                        }
                    }
                    return true;
                });
            }
        }

        public List<ISpaceObject> GetObjectInCircle(Vector3 center, float radius)
        {
            return GetObjectInCircle(center, radius, (distSqr, obj) => true);
        }
        public List<ISpaceObject> GetObjectInCircle(Vector3 center, float radius, MyFunc<float, ISpaceObject, bool> pred)
        {
            List<ISpaceObject> objects_in_circle = new List<ISpaceObject>();
            VisitObjectInCircle(center, radius, (float distSqr, ISpaceObject obj) =>
            {
                if (pred(distSqr, obj))
                {
                    objects_in_circle.Add(obj);
                }
            });
            return objects_in_circle;
        }

        public void VisitObjectInCircle(Vector3 center, float radius, MyAction<float, ISpaceObject> visitor)
        {
            m_KdTree.Query(center, (float)radius, (float distSqr, KdTreeObject kdObj) =>
            {
                ISpaceObject obj = kdObj.SpaceObject;
                if (null != obj)
                {
                    visitor(distSqr, obj);
                }
            });
        }
    }
}
