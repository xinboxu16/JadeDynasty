using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashFireSpatial;
using UnityEngine;

namespace DashFire
{
    //瞄准的
    public sealed class SightManager
    {
        private MyDictionary<int, List<UserInfo>> camp_users_ = new MyDictionary<int, List<UserInfo>>();
        private HexagonalCellManager cell_manager_ = new HexagonalCellManager();

        private void RemoveObjectImpl(CharacterInfo obj)
        {
            UserInfo user = obj.CastUserInfo();
            if (null != user)
            {
                int camp = user.GetCampId();
                if (camp_users_.ContainsKey(camp))
                {
                    List<UserInfo> users = camp_users_[camp];
                    users.Remove(user);
                }
            }
        }

        public void AddObject(CharacterInfo obj)
        {
            AddObjectImpl(obj);
            cell_manager_.AddObject(obj);
        }

        public void RemoveObject(CharacterInfo obj)
        {
            RemoveObjectImpl(obj);
            cell_manager_.RemoveObject(obj);
        }

        private void AddObjectImpl(CharacterInfo obj)
        {
            UserInfo user = obj.CastUserInfo();
            if (null != user)
            {
                int camp = user.GetCampId();
                List<UserInfo> users = null;
                if(!camp_users_.ContainsKey(camp))
                {
                    users = new List<UserInfo>();
                    camp_users_.Add(camp, users);
                }
                else
                {
                    users = camp_users_[camp];
                }
                if(!users.Contains(user))
                {
                    users.Add(user);
                }
            }
        }
    }

    //六边形的
    public class HexagonalCellManager
    {
        private int max_row_;
        private int max_col_;

        private LinkedListDictionary<int, CharacterInfo>[,] cells_arr_;

        private const float c_CellWidth = 10;//这里的值与pvp里的最大视野有关，大约为最大视野的2/5    
        private const float c_GridWidth = c_CellWidth * 1.5f;//对应矩形网格宽
        private const float c_GridHeight = (float)(c_CellWidth * 0.8660254f);//对应矩形网格高
        private const float c_SectionsA = (c_GridWidth * c_GridWidth + c_GridHeight * c_GridHeight) / 2.0f;
        private const float c_SectionsB = (c_GridWidth * c_GridWidth - c_GridHeight * c_GridHeight) / 2.0f;

        private bool IsValid(int row, int col)
        {
            return row >= 0 && row < max_row_ && col >= 0 && col < max_col_;
        }

        public void AddObject(CharacterInfo obj)
        {
            int row, col;
            GetCell(obj.GetMovementStateInfo().GetPosition3D(), out row, out col);
            AddObject(row, col, obj);
        }

        private void AddObject(int row, int col, CharacterInfo obj)
        {
            int id = obj.GetId();
            if (IsValid(row, col) && !cells_arr_[row, col].Contains(id))
            {
                cells_arr_[row, col].AddLast(id, obj);
                obj.SightCell = new CellPos(row, col);
            }
        }

        public void RemoveObject(CharacterInfo obj)
        {
            CellPos cell = obj.SightCell;
            int id = obj.GetId();
            if (IsValid(cell.row, cell.col))
            {
                cells_arr_[cell.row, cell.col].Remove(id);
            }
        }

        public void GetCell(Vector3 pos, out int cell_row, out int cell_col)
        {
            int grid_y = (int)(pos.z / c_GridHeight);
            float y = pos.z - grid_y * c_GridHeight;

            int grid_x = (int)(pos.x / c_GridWidth);
            float x = pos.x - grid_x * c_GridWidth;

            if (((grid_x + grid_y) & 1) == 0)
            {
                //   _______
                //  |___/___| a--sections 
                cell_row = grid_y / 2;
                if (x * c_GridWidth + y * c_GridHeight < c_SectionsA)
                {   // left
                    cell_col = grid_x;
                    if (grid_x % 2 != 0)
                    {
                        cell_row = (grid_y - 1) / 2;
                    }
                }
                else
                { // right
                    cell_col = grid_x + 1;
                    if (grid_x % 2 != 0)
                    {
                        cell_row = (grid_y + 1) / 2;
                    }
                }
            }
            else
            {
                //   _______
                //  |___\___| b--sections 
                cell_row = grid_y / 2;
                if (x * c_GridWidth - y * c_GridHeight < c_SectionsB)
                {   // left
                    cell_col = grid_x;
                    if (grid_x % 2 == 0)
                    {
                        cell_row = (grid_y + 1) / 2;
                    }
                }
                else
                { // right
                    cell_col = grid_x + 1;
                    if (grid_x % 2 == 0)
                    {
                        cell_row = (grid_y - 1) / 2;
                    }
                }
            }
        }
    }
}
