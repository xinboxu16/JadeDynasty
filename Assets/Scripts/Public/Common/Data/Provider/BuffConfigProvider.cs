using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class BuffConfig : IData
    {
        public int m_Id;
        public AttrDataConfig m_AttrData = new AttrDataConfig();

        /**
         * @brief 提取数据
         *
         * @param node
         *
         * @return 
         */
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_AttrData.CollectDataFromDBC(node);

            return true;
        }

        /**
         * @brief 获取数据ID
         *
         * @return 
         */
        public int GetId()
        {
            return m_Id;
        }

    }

    public class BuffConfigProvider
    {
        private DataDictionaryMgr<BuffConfig> m_BuffConfigMgr = new DataDictionaryMgr<BuffConfig>();

        public void Load(string file, string root)
        {
            m_BuffConfigMgr.CollectDataFromDBC(file, root);
        }

        public BuffConfig GetDataById(int id)
        {
            return m_BuffConfigMgr.GetDataById(id);
        }

        public static BuffConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static BuffConfigProvider s_Instance = new BuffConfigProvider();
    }
}
