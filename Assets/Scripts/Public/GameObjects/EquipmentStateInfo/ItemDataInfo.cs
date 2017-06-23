using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class ItemDataInfo
    {
        private int m_ItemId = 0;
        private int m_ItemNum = 0;
        private int m_Level = 1;
        private int m_RandomProperty = 0;
        private bool m_IsUnlock = true;
        private ItemConfig m_ItemConfig = null;

        public int ItemId
        {
            get
            {
                if (0 == m_ItemId)
                {
                    if (null != m_ItemConfig)
                        m_ItemId = m_ItemConfig.m_ItemId;
                }
                return m_ItemId;
            }
            set { m_ItemId = value; }
        }

        public ItemConfig ItemConfig
        {
            get { return m_ItemConfig; }
            set { m_ItemConfig = value; }
        }
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        public int RandomProperty
        {
            get { return m_RandomProperty; }
            set { m_RandomProperty = value; }
        }

        public bool IsUnlock
        {
            get { return m_IsUnlock; }
            set { m_IsUnlock = value; }
        }

        public int ItemNum
        {
            get { return m_ItemNum; }
            set { m_ItemNum = value; }
        }
    }
}
