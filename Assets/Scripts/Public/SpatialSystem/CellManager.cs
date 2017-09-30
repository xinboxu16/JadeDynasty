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
                //FileStream fs = new FileStream(filename.Substring(0, filename.LastIndexOf("/")) + "/Scene.txt", FileMode.Create);
                //StreamWriter sw = new StreamWriter(fs);

                MemoryStream ms = FileReaderProxy.ReadFileAsMemoryStream(filename);
                BinaryReader br = new BinaryReader(ms);

                map_width_ = (float)br.ReadDouble();
                map_height_ = (float)br.ReadDouble();
                cell_width_ = (float)br.ReadDouble();

                //sw.Write(map_width_);
                //sw.Write("\r\n");
                //sw.Write(map_width_);
                //sw.Write("\r\n");
                //sw.Write(cell_width_);
                //sw.Write("\r\n");

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

                    //int tem = (int)cells_arr_[row, col];
                    //sw.Write(tem);
                    //sw.Write(" ");

                    if (++col >= max_col_)
                    {
                        //sw.Write("\r\n");
                        col = 0;
                        ++row;
                    }
                }
                br.Close();

                //sw.Flush();
                //sw.Close();
                //fs.Close();
            }
            catch (Exception e)
            {
                LogSystem.Error("{0}\n{1}", e.Message, e.StackTrace);
                return false;
            }
            return true;
        }

        public ICellMapView GetCellMapView(int radius)
        {
            ICellMapView view = null;
            if (cell_map_views_.ContainsKey(radius))
            {
                view = cell_map_views_[radius];
            }
            else
            {
                view = new CellMapView(this, radius);
                cell_map_views_.Add(radius, view);
            }
            return view;
        }

        // 查询cell
        public bool GetCell(Vector3 pos, out int cell_row, out int cell_col)
        {
            //计算是否有多余的行列
            cell_row = (int)(pos.z / cell_width_);
            float y = pos.z - cell_row * cell_width_;
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

        public byte GetCellStatus(int row, int col)
        {
            if (row >= max_row_ || col >= max_col_ || row < 0 || col < 0)
            {
                return BlockType.OUT_OF_BLOCK;
            }
            return cells_arr_[row, col];
        }

        public byte GetCellStatus(Vector3 pos)
        {
            int row, col;
            GetCell(pos, out row, out col);
            if (row >= max_row_ || col >= max_col_ || row < 0 || col < 0)
            {
                return BlockType.OUT_OF_BLOCK;
            }
            return cells_arr_[row, col];
        }

        public void SetCellStatus(int row, int col, byte status)
        {
            if (row >= max_row_ || col >= max_col_ || row < 0 || col < 0)
            {
                return;
            }
            cells_arr_[row, col] = status;
        }

        public Vector3 GetCellCenter(int row, int col)
        {
            Vector3 center = new Vector3();
            center.x = col * cell_width_ + cell_width_ / 2;
            center.z = row * cell_width_ + cell_width_ / 2;
            return center;
        }

        public void Reset()
        {
            cell_map_views_.Clear();
        }

        public int GetMaxRow() { return max_row_; }
        public int GetMaxCol() { return max_col_; }
        public float GetMapWidth() { return map_width_; }
        public float GetMapHeight() { return map_height_; }
        public float GetCellWidth() { return cell_width_; }


        //TODO 这都是写了些 什么啊 看不懂
        public void VisitCellsCrossByLine(Vector3 start, Vector3 end, MyFunc<int, int, bool> visitor)
        {
            int startRow, startCol, endRow, endCol;
            bool ltValid = GetCell(start, out startRow, out startCol);
            bool rbValid = GetCell(end, out endRow, out endCol);
            if (ltValid && rbValid)
            {
                const float c_Scale = 1000.0f;//防止tx/deltaTx/tz/deltaTz过小的倍率（防止小于浮点精度）
                int c = (int)(start.x / cell_width_);
                int r = (int)(start.z / cell_width_);
                int cend = (int)(end.x / cell_width_);
                int rend = (int)(end.z / cell_width_);

                int dc = ((c < cend) ? 1 : ((c > cend) ? -1 : 0));
                int dr = ((r < rend) ? 1 : ((r > rend) ? -1 : 0));

                float minX = cell_width_ * (float)Math.Floor(start.x / cell_width_);
                float maxX = minX + cell_width_;
                float tx = (dc == 0 ? float.MaxValue : (dc > 0 ? (maxX - start.x) : (start.x - minX)) * c_Scale / Math.Abs(end.x - start.x));
                float minZ = cell_width_ * (float)Math.Floor(start.z / cell_width_);
                float maxZ = minZ + cell_width_;
                float tz = (dr == 0 ? float.MaxValue : (dr > 0 ? (maxZ - start.z) : (start.z - minZ)) * c_Scale / Math.Abs(end.z - start.z));

                float deltaTx = (dc == 0 ? float.MaxValue : cell_width_ * c_Scale / Math.Abs(end.x - start.x));
                float deltaTz = (dr == 0 ? float.MaxValue : cell_width_ * c_Scale / Math.Abs(end.z - start.z));

                if (dr == 0 && dc == 0)
                {
                    visitor(r, c);
                }
                else
                {
                    const int c_ErrorCount = 500;
                    int ct = 0;
                    for (; ct < c_ErrorCount; ++ct)
                    {
                        if (!visitor(r, c))
                            break;
                        if (tx <= tz)
                        {
                            if (c == cend)
                                break;
                            tx += deltaTx;
                            c += dc;
                        }
                        else
                        {
                            if (r == rend)
                                break;
                            tz += deltaTz;
                            r += dr;
                        }
                    }
                    if (ct >= c_ErrorCount)
                    {
                        LogSystem.Error("VisitCellsCrossByLine({0} -> {1}) c:{2} cend:{3} dc:{4} r:{5} rend:{6} dr:{7} deltaTx:{8} deltaTz:{9} minX:{10} maxX:{11} minZ:{12} maxZ:{13} tx:{14} tz:{15}", start.ToString(), end.ToString(),
                          c, cend, dc, r, rend, dr, deltaTx, deltaTz, minX, maxX, minZ, maxZ, tx, tz);
                    }
                }
            }
        }
    }
}
