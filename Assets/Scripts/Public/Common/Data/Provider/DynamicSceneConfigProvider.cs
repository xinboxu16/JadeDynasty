using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class Data_DynamicSceneConfig : IData
    {
        // 基础属性
        public int m_Id;
        public string m_Ground;
        public string m_BeginArea;
        public string m_EndArea;
        public float m_TriggerDir;
        public float m_PushObjDeltaTime;
        public float m_PushGroundDeltaTime;
        public float m_OffsetAngle;
        public float m_ObjOffsetDis;
        public float m_GroundOffsetDis;
        public float m_ObjLifeTime;
        public float m_GroundLifeTime;
        public float m_Speed;
        public float m_MaxSpeed;
        public float m_Acceleration;

        public int ObjNum = 0;
        public List<string> ObjList = new List<string>();
        public int ParamNum = 0;
        public List<string> ExtraParams = new List<string>();

        /**
         * @brief 提取数据
         *
         * @param node
         *
         * @return 
         */
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", -1, true);
            m_Ground = DBCUtil.ExtractString(node, "Ground", "", true);
            m_BeginArea = DBCUtil.ExtractString(node, "BeginArea", "", true);
            m_EndArea = DBCUtil.ExtractString(node, "EndArea", "", true);
            m_TriggerDir = DBCUtil.ExtractNumeric<float>(node, "TriggerDir", -1, true);
            m_PushObjDeltaTime = DBCUtil.ExtractNumeric<float>(node, "PushObjDeltaTime", -1, true);
            m_PushGroundDeltaTime = DBCUtil.ExtractNumeric<float>(node, "PushGroundDeltaTime", -1, true);
            m_OffsetAngle = DBCUtil.ExtractNumeric<float>(node, "OffsetAngle", -1, true);
            m_ObjOffsetDis = DBCUtil.ExtractNumeric<float>(node, "ObjOffsetDis", -1, true);
            m_GroundOffsetDis = DBCUtil.ExtractNumeric<float>(node, "GroundOffsetDis", -1, true);
            m_ObjLifeTime = DBCUtil.ExtractNumeric<float>(node, "ObjLifeTime", -1, true);
            m_GroundLifeTime = DBCUtil.ExtractNumeric<float>(node, "GroundLifeTime", -1, true);
            m_Speed = DBCUtil.ExtractNumeric<float>(node, "Speed", -1, true);
            m_MaxSpeed = DBCUtil.ExtractNumeric<float>(node, "MaxSpeed", -1, true);
            m_Acceleration = DBCUtil.ExtractNumeric<float>(node, "Acceleration", -1, true);

            ObjNum = DBCUtil.ExtractNumeric<int>(node, "ObjNum", 0, false);
            ObjList.Clear();
            if (ObjNum > 0)
            {
                for (int i = 0; i < ObjNum; ++i)
                {
                    string key = "Obj" + i.ToString();
                    ObjList.Insert(i, DBCUtil.ExtractString(node, key, "", false));
                }
            }

            ParamNum = DBCUtil.ExtractNumeric<int>(node, "ParamNum", 0, false);
            ExtraParams.Clear();
            if (ParamNum > 0)
            {
                for (int i = 0; i < ParamNum; ++i)
                {
                    string key = "Param" + i.ToString();
                    ExtraParams.Insert(i, DBCUtil.ExtractString(node, key, "", false));
                }
            }

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

    public class DynamicSceneConfigProvider
    {
        DataDictionaryMgr<Data_DynamicSceneConfig> m_DynamicSceneLogicDataMgr;

        private DynamicSceneConfigProvider()
        {
            m_DynamicSceneLogicDataMgr = new DataDictionaryMgr<Data_DynamicSceneConfig>();
        }

        /**
         * @brief 读取数据
         *
         * @param node
         *
         * @return 
         */
        public bool CollectData(string file, string rootLabel)
        {
            bool result = false;
            result = m_DynamicSceneLogicDataMgr.CollectDataFromDBC(file, rootLabel);
            return result;
        }

        private static DynamicSceneConfigProvider s_instance_ = new DynamicSceneConfigProvider();
        public static DynamicSceneConfigProvider Instance
        {
            get { return s_instance_; }
        }
    }
}
