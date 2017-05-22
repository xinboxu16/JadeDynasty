using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFireSpatial
{
    public sealed class CellManager
    {
        private MyDictionary<int, ICellMapView> cell_map_views_ = new MyDictionary<int, ICellMapView>();

        public void Reset()
        {
            cell_map_views_.Clear();
        }
    }
}
