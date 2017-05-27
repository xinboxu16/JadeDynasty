using DashFire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFireSpatial
{
    public sealed class CellManager
    {
        private MyDictionary<int, ICellMapView> cell_map_views_ = new MyDictionary<int, ICellMapView>();

        private float map_width_;   // 地图的宽度
        private float map_height_;  // 地图的高度
        private float cell_width_; // 阻挡区域的一边边长
        private int max_row_;
        private int max_col_;

        internal byte[,] cells_arr_;

        // 初始化一张空的地图
        public void Init(float width, float height, float cellwidth)
        {
            map_width_ = width;
            map_height_ = height;
            cell_width_ = cellwidth;

            GetCell(new Vector3(width, 0, height), out max_row_, out max_col_);
            max_row_++;
            max_col_++;
            if (max_col_ % 2 == 0)
            {
                max_row_++;
            }
            cells_arr_ = new byte[max_row_, max_col_];
        }

        // 从文件读取
        public bool Init(string filename)
        {
            if (!FileReaderProxy.Exists(filename))
            {
                return false;
            }
            try
            {
                MemoryStream ms = FileReaderProxy.ReadFileAsMemoryStream(filename);
                BinaryReader br = new BinaryReader(ms);

                map_width_ = (float)br.ReadDouble();
                map_height_ = (float)br.ReadDouble();
                cell_width_ = (float)br.ReadDouble();

                GetCell(new Vector3(map_width_, 0, map_height_), out max_row_, out max_col_);

                max_row_++;
                max_col_++;
                if (max_col_ % 2 == 0)
                {
                    max_row_++;
                }

                cells_arr_ = new byte[max_row_, max_col_];
                int row = 0;
                int col = 0;
                while(ms.Position < ms.Length && row < max_row_)
                {
                    cells_arr_[row, col] = br.ReadByte();
                    if (++col >= max_col_)
                    {
                        col = 0;
                        ++row;
                    }
                }
                br.Close();
            }
            catch (Exception e)
            {
                LogSystem.Error("{0}\n{1}", e.Message, e.StackTrace);
                return false;
            }
            return true;
        }

        // 查询cell
        public bool GetCell(Vector3 pos, out int cell_row, out int cell_col)
        {
            cell_row = (int)(pos.z / cell_width_);
            float y = pos.z - cell_row * cell_width_;//不明白
            if (y >= cell_width_ + 0.001f)
            {
                ++cell_row;
            }

            cell_col = (int)(pos.x / cell_width_);
            float x = pos.x - cell_col * cell_width_;
            if (x >= cell_width_ + 0.001f)
                ++cell_col;

            return cell_row >= 0 && cell_row < max_row_ && cell_col >= 0 && cell_col < max_col_;
        }

        public void Reset()
        {
            cell_map_views_.Clear();
        }
    }
}
