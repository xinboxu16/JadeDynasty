using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashFireSpatial;

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

        public void RemoveObject(CharacterInfo obj)
        {
            RemoveObjectImpl(obj);
            cell_manager_.RemoveObject(obj);
        }
    }

    //六边形的
    public class HexagonalCellManager
    {
        private int max_row_;
        private int max_col_;

        private LinkedListDictionary<int, CharacterInfo>[,] cells_arr_;

        private bool IsValid(int row, int col)
        {
            return row >= 0 && row < max_row_ && col >= 0 && col < max_col_;
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
    }
}
