using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class NewbieGuide : IData
    {
        public int Id = 0;
        public string m_ParentPath = "";
        public string m_MyPath = "";
        public float[] m_RotateThree = { 0.0f, 0.0f, 0.0f };
        public bool m_Visible = false;
        public int m_Type = 0;
        public int m_PreviousGuideId = 0;
        public float[] m_LocalPosition = { 0.0f, 0.0f, 0.0f };
        public int m_ChildNumber = 0;
        public string m_ChildName = "";
        public float[] m_Scale = { 1.0f, 1.0f, 1.0f };

        public bool CollectDataFromDBC(DBC_Row node)
        {
            Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_ParentPath = DBCUtil.ExtractString(node, "ParentPath", "", true);
            m_MyPath = DBCUtil.ExtractString(node, "MyPath", "", true);
            List<float> list = DBCUtil.ExtractNumericList<float>(node, "Rotate", 0, false);
            int num = list.Count;
            if (num > 0) m_RotateThree[0] = list[0];
            if (num > 1) m_RotateThree[1] = list[1];
            if (num > 2) m_RotateThree[2] = list[2];

            m_Visible = DBCUtil.ExtractNumeric<bool>(node, "Visible", false, false);
            m_Type = DBCUtil.ExtractNumeric<int>(node, "Type", 0, true);
            m_PreviousGuideId = DBCUtil.ExtractNumeric<int>(node, "PreviousGuideId", 0, true);

            list = DBCUtil.ExtractNumericList<float>(node, "LocalPosition", 0, false);
            num = list.Count;
            if (num > 0) m_LocalPosition[0] = list[0];
            if (num > 1) m_LocalPosition[1] = list[1];
            if (num > 2) m_LocalPosition[2] = list[2];

            m_ChildNumber = DBCUtil.ExtractNumeric<int>(node, "ChildNumber", 0, true);
            m_ChildName = DBCUtil.ExtractString(node, "ChildName", "", true);

            list = DBCUtil.ExtractNumericList<float>(node, "Scale", 0, false);
            num = list.Count;
            if (num > 0) m_Scale[0] = list[0];
            if (num > 1) m_Scale[1] = list[1];
            if (num > 2) m_Scale[2] = list[2];
            return true;
        }
        public int GetId()
        {
            return Id;
        }
    }

    public class NewbieGuideProvider
    {
        private DataDictionaryMgr<NewbieGuide> m_NewbieGuideMgr = new DataDictionaryMgr<NewbieGuide>();

        public void Load(string file, string root)
        {
            m_NewbieGuideMgr.CollectDataFromDBC(file, root);
        }

        public static NewbieGuideProvider Instance
        {
            get { return s_Instance; }
        }
        private static NewbieGuideProvider s_Instance = new NewbieGuideProvider();
    }
}
