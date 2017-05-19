using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * @file EntityManager.cs
 * @brief 角色管理器
 *
 * @author lixiaojiang
 * @version 0
 * @date 2012-11-14
 */

namespace DashFire
{
    /**
     * @brief 角色管理器
     * @remarks 这个类在GameObjects采取数据驱动的方式后，它的职责变为GameObjects的View层，它在每个Tick负责更新各个GameObject的显示
     */
    public sealed class EntityManager
    {
        /**
         * @brief 构筑函数
         *
         * @return 
         */
        private EntityManager()
        {
        }

        /**
         * @brief 初始化
         *
         * @return 
         */
        public void Init()
        {
        }

        #region Singleton
        private static EntityManager s_instance_ = new EntityManager();
        public static EntityManager Instance
        {
            get { return s_instance_; }
        }
#endregion
    }
}
