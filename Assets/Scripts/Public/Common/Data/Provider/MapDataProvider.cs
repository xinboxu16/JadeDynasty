using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    /**
     * @file SceneElements.cs
     * @brief 单场景元素数据
     *              负责：
     *                  场景上的数据格式定义,及其解析方法
     *                  服务器与客户端公用
     *
     * @version 1.0.0
     * @date 2012-12-16
     */
     /**
      * @brief 单元数据
      */
    public class Data_Unit : IData
    {
        public const int c_MaxAiParamNum = 8;
        public const int c_MaxInteractionParamNum = 8;
        // 基础
        public int m_Id;
        public int m_LinkId;
        public int m_CampId;
        public Vector3 m_Pos;
        public Vector3 m_Pos2;
        public float m_RotAngle;
        public bool m_IsEnable;
        public List<int> m_IdleAnims;
        public List<int> m_AiActions;

        public int m_AiLogic;
        public string[] m_AiParam = new string[c_MaxAiParamNum];

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
            m_LinkId = DBCUtil.ExtractNumeric<int>(node, "LinkId", 0, true);
            m_CampId = DBCUtil.ExtractNumeric<int>(node, "CampId", 0, true);
            m_Pos = Converter.ConvertVector3D(DBCUtil.ExtractString(node, "Pos", "0.0,0.0,0.0", true));
            m_Pos2 = Converter.ConvertVector3D(DBCUtil.ExtractString(node, "Pos2", "0.0,0.0,0.0", false));
            m_RotAngle = DBCUtil.ExtractNumeric<float>(node, "RotAngle", 0.0f, true) * (float)Math.PI / 180;
            m_IsEnable = DBCUtil.ExtractBool(node, "IsEnable", false, true);
            m_AiLogic = DBCUtil.ExtractNumeric<int>(node, "AiLogic", 0, false);
            m_IdleAnims = DBCUtil.ExtractNumericList<int>(node, "IdleAnimSet", 0, false);
            m_AiActions = DBCUtil.ExtractNumericList<int>(node, "Actions", 0, false);
            for (int i = 0; i < c_MaxAiParamNum; ++i)
            {
                m_AiParam[i] = DBCUtil.ExtractString(node, "AiParam" + i, "", false);
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

        /**
         * @brief 克隆函数
         *
         * @return 
         */
        public object Clone()
        {
            Data_Unit data = new Data_Unit();
            data.m_Id = m_Id;
            data.m_LinkId = m_LinkId;
            data.m_CampId = m_CampId;
            data.m_Pos = new Vector3(m_Pos.x, m_Pos.y, m_Pos.z);
            data.m_RotAngle = m_RotAngle;

            data.m_IsEnable = m_IsEnable;
            data.m_AiLogic = m_AiLogic;

            for (int i = 0; i < c_MaxAiParamNum; ++i)
            {
                data.m_AiParam[i] = m_AiParam[i];
            }

            return data;
        }
    }

    public class SceneLogicConfig : IData
    {
        public int m_Id = 0;
        public int m_LogicId = 0;
        public bool m_IsClient = false;
        public bool m_IsServer = false;
        public int m_ParamNum = 0;
        public string[] m_Params = null;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_LogicId = DBCUtil.ExtractNumeric<int>(node, "LogicId", 0, true);
            m_IsClient = DBCUtil.ExtractBool(node, "IsClient", false, true);
            m_IsServer = DBCUtil.ExtractBool(node, "IsServer", false, true);
            m_ParamNum = DBCUtil.ExtractNumeric<int>(node, "ParamNum", 0, true);
            if (m_ParamNum > 0)
            {
                m_Params = new string[m_ParamNum];
                for (int i = 0; i < m_ParamNum; ++i)
                {
                    m_Params[i] = DBCUtil.ExtractString(node, "Param" + i, "", false);
                }
            }
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    /**
     * @brief 地图数据
     */
    public class MapDataProvider
    {
        public DataDictionaryMgr<Data_Unit> m_UnitMgr;
        public DataDictionaryMgr<SceneLogicConfig> m_SceneLogicMgr;

        /**
         * @brief 构造函数
         */
        public MapDataProvider()
        {
            m_UnitMgr = new DataDictionaryMgr<Data_Unit>();
            m_SceneLogicMgr = new DataDictionaryMgr<SceneLogicConfig>();
        }

        /**
         * @brief 读取数据
         *
         * @param node
         *
         * @return 
         */
        public bool CollectData(DataMap_Type type, string file, string rootLabel)
        {
            bool result = false;
            switch (type)
            {
                case DataMap_Type.DT_Unit:
                    {
                        result = m_UnitMgr.CollectDataFromDBC(file, rootLabel);
                    } break;
                case DataMap_Type.DT_SceneLogic:
                    {
                        result = m_SceneLogicMgr.CollectDataFromDBC(file, rootLabel);
                    } break;
                case DataMap_Type.DT_All:
                case DataMap_Type.DT_None:
                default:
                    {

                    } break;
            }

            return result;
        }

        /**
         * @brief 提取数据
         *
         * @param node
         *
         * @return 
         */
        public IData ExtractData(DataMap_Type type, int id)
        {
            IData result = null;
            switch(type)
            {
                case DataMap_Type.DT_Unit:
                    result = m_UnitMgr.GetDataById(id);
                    break;
                case DataMap_Type.DT_SceneLogic:
                    result = m_SceneLogicMgr.GetDataById(id);
                    break;
                case DataMap_Type.DT_All:
                case DataMap_Type.DT_None:
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
