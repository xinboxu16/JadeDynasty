using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    /**
     * @brief 角色属性类
     */
    public sealed class CharacterProperty
    {
        /**
         * @brief 奔跑速度
         */
        private float m_MoveSpeed;

        /**
         * @brief 构造函数
         *
         * @param owner
         *
         * @return 
         */
        public CharacterProperty()
        { }

        /**
         * @brief 移动速度
         */
        public float MoveSpeed
        {
            get { return m_MoveSpeed; }
        }
    }
}
