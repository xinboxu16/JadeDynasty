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
        private MyDictionary<int, UserView> m_UserViews = new MyDictionary<int, UserView>();
        private MyDictionary<int, NpcView> m_NpcViews = new MyDictionary<int, NpcView>();

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

        public void DestroyUserView(int objId)
        {
            if (m_UserViews.ContainsKey(objId))
            {
                UserView view = m_UserViews[objId];
                if (view != null)
                {
                    view.Destroy();
                }
                m_UserViews.Remove(objId);
            }
        }

        public void DestroyNpcView(int objId)
        {
            if (m_NpcViews.ContainsKey(objId))
            {
                NpcView view = m_NpcViews[objId];
                if (view != null)
                {
                    view.Destroy();
                }
                m_NpcViews.Remove(objId);
            }
        }

        public UserView GetUserViewById(int objId)
        {
            UserView view = null;
            if (m_UserViews.ContainsKey(objId))
                view = m_UserViews[objId];
            return view;
        }

        public NpcView GetNpcViewById(int objId)
        {
            NpcView view = null;
            if (m_NpcViews.ContainsKey(objId))
                view = m_NpcViews[objId];
            return view;
        }

        public CharacterView GetCharacterViewById(int objId)
        {
            CharacterView view = GetUserViewById(objId);
            if (null == view)
                view = GetNpcViewById(objId);
            return view;
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
