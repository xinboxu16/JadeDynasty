using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    //数据接口
    public interface IData
    {
        /**
         * @brief 提取数据
         *
         * @param node
         *
         * @return 
         */
        bool CollectDataFromDBC(DBC_Row node);

        /**
         * @brief 获取数据ID
         *
         * @return 
         */
        int GetId();
    }
}
