using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class UiConfig : IData
    {
        public int m_Id = 0;
        public string m_WindowName = "";
        public bool m_IsExclusion = false;//IsExclusion拒绝，杜绝; 排除
        public int m_ShowType = -1;
        public int m_OwnToSceneId = -1;
        public string m_WindowPath = "";

        //public int m_OffsetLeft = -1;
        //public int m_OffsetRight = -1;
        //public int m_OffsetBottom = -1;
        //public int m_OffsetTop = -1;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "ID", 0, true);
            m_WindowName = DBCUtil.ExtractString(node, "WindowName", "", true);
            m_WindowPath = DBCUtil.ExtractString(node, "WindowPath", "", true);
            m_IsExclusion = DBCUtil.ExtractBool(node, "IsExclusion", false, false);
            m_ShowType = DBCUtil.ExtractNumeric<int>(node, "ShowType", 0, true);
            m_OwnToSceneId = DBCUtil.ExtractNumeric<int>(node, "SceneType", 0, true);

            //m_OffsetLeft = DBCUtil.ExtractNumeric<int>(node, "OffsetLeft", -1, false);
            //m_OffsetRight = DBCUtil.ExtractNumeric<int>(node, "OffsetRight", -1, false);
            //m_OffsetTop = DBCUtil.ExtractNumeric<int>(node, "OffsetTop", -1, false);
            //m_OffsetBottom = DBCUtil.ExtractNumeric<int>(node, "OffsetBottom", -1, false);
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class UiConfigProvider
    {
        private DataDictionaryMgr<UiConfig> m_UiConfigMgr = new DataDictionaryMgr<UiConfig>();

        public UiConfig GetDataById(int id)
        {
            return m_UiConfigMgr.GetDataById(id);
        }

        public UiConfig GetDataByName(string windowName)
        {
            MyDictionary<int, object> datas = GetData();
            foreach(UiConfig uiData in datas.Values)
            {
                if (uiData.m_WindowName == windowName)
                {
                    return uiData;
                }
            }
            return null;
        }

        public MyDictionary<int, object> GetData()
        {
            return m_UiConfigMgr.GetData();
        }

        public void Load(string file, string root)
        {
            m_UiConfigMgr.CollectDataFromDBC(file, root);
        }

        public static UiConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static UiConfigProvider s_Instance = new UiConfigProvider();
    }
}
