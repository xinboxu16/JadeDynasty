using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class Data_SceneConfig : IData
    {
        public int m_Id;
        public int m_Type;
        public int m_SubType;
        public int m_Chapter;
        public int m_Order;
        public string m_ChapterName;
        public string m_SceneName;
        public string m_SceneDescription;
        public string m_UnitFile;
        public string m_SceneLogicFile;
        public List<string> m_StoryDslFile;
        public string m_ClientSceneFile;
        public string m_ObstacleFile;
        public string m_BlockInfoFile;
        public float m_TiledDataScale;
        public int m_DropId;
        public List<int> m_CompletedRewards;
        public int m_CompletedTime;
        public int m_CompletedHitCount;
        public int m_CostStamina;
        public int m_RecommendFighting;
        public Vector3[] m_ReachableSet;
        public string m_AtlasPath;
        public string m_SceneIconName;
        public float m_RecoverHpCoefficient;
        public float m_RecoverMpCoefficient;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_SceneName = DBCUtil.ExtractString(node, "Name", "", false);
            m_ChapterName = DBCUtil.ExtractString(node, "ChapterName", "", false);
            m_SceneDescription = DBCUtil.ExtractString(node, "Description", "", false);
            m_Type = DBCUtil.ExtractNumeric<int>(node, "Type", 0, true);
            m_SubType = DBCUtil.ExtractNumeric<int>(node, "SubType", 0, true);
            m_Chapter = DBCUtil.ExtractNumeric<int>(node, "Chapter", 0, true);
            m_Order = DBCUtil.ExtractNumeric<int>(node, "Order", 0, true);
            m_UnitFile = DBCUtil.ExtractString(node, "UnitFile", "", false);
            m_SceneLogicFile = DBCUtil.ExtractString(node, "SceneLogicFile", "", false);
            m_StoryDslFile = DBCUtil.ExtractStringList(node, "StoryDslFile", "", false);
            m_ClientSceneFile = DBCUtil.ExtractString(node, "ClientSceneFile", "", true);
            m_ObstacleFile = DBCUtil.ExtractString(node, "ObstacleFile", "", false);
            m_BlockInfoFile = DBCUtil.ExtractString(node, "BlockInfoFile", "", false);
            m_TiledDataScale = DBCUtil.ExtractNumeric<float>(node, "TiledDataScale", 0, false);
            m_DropId = DBCUtil.ExtractNumeric<int>(node, "DropId", 0, false);
            m_CompletedRewards = DBCUtil.ExtractNumericList<int>(node, "CompletedReward", 0, false);
            m_CompletedTime = DBCUtil.ExtractNumeric<int>(node, "CompletedTime", 0, false);
            m_CompletedHitCount = DBCUtil.ExtractNumeric<int>(node, "CompletedHitCount", 0, false);
            m_CostStamina = DBCUtil.ExtractNumeric<int>(node, "CostStamina", 0, false);
            m_RecommendFighting = DBCUtil.ExtractNumeric<int>(node, "RecommendFighting", 0, false);
            m_AtlasPath = DBCUtil.ExtractString(node, "AtlasPath", "", false);
            m_SceneIconName = DBCUtil.ExtractString(node, "SceneIconName", "", false);
            m_RecoverHpCoefficient = DBCUtil.ExtractNumeric<float>(node, "RecoverHpCoefficient", 1, false);
            m_RecoverMpCoefficient = DBCUtil.ExtractNumeric<float>(node, "RecoverMpCoefficient", 1, false);

            List<float> coords = DBCUtil.ExtractNumericList<float>(node, "ReachableSet", 0, false);
            if (coords.Count > 0)
            {
                m_ReachableSet = new Vector3[coords.Count / 2];
                for (int i = 0; i < coords.Count - 1; i += 2)
                {
                    m_ReachableSet[i / 2] = new Vector3(coords[i], 0, coords[i + 1]);
                }
            }
            else
            {
                m_ReachableSet = null;
            }
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class Data_SceneDropOut : IData
    {

        public int m_Id;
        public int m_GoldSum;
        public int m_GoldMin;
        public int m_GoldMax;
        public int m_Exp;
        public int m_Diamond;
        public int m_HpCount;
        public int m_HpPercent;
        public int m_MpCount;
        public int m_MpPercent;
        public string m_GoldModel;
        public string m_GoldParticle;
        public string m_HpModel;
        public string m_HpParticle;
        public string m_MpModel;
        public string m_MpParticle;
        public int m_ItemCount;
        public List<int> m_ItemIdList;
        public List<int> m_ItemWeightList;
        public List<int> m_ItemCountList;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_GoldSum = DBCUtil.ExtractNumeric<int>(node, "GoldSum", 0, false);
            m_GoldMin = DBCUtil.ExtractNumeric<int>(node, "GoldMin", 0, false);
            m_GoldMax = DBCUtil.ExtractNumeric<int>(node, "GoldMax", 0, false);
            m_Exp = DBCUtil.ExtractNumeric<int>(node, "Exp", 0, false);
            m_Diamond = DBCUtil.ExtractNumeric<int>(node, "Diamond", 0, false);
            m_ItemCount = DBCUtil.ExtractNumeric<int>(node, "ItemCount", 0, false);
            m_HpCount = DBCUtil.ExtractNumeric<int>(node, "HpCount", 0, false);
            m_HpPercent = DBCUtil.ExtractNumeric<int>(node, "HpPercent", 0, false);
            m_MpCount = DBCUtil.ExtractNumeric<int>(node, "MpCount", 0, false);
            m_MpPercent = DBCUtil.ExtractNumeric<int>(node, "MpPercent", 0, false);
            m_GoldModel = DBCUtil.ExtractString(node, "GoldModel", "", false);
            m_GoldParticle = DBCUtil.ExtractString(node, "GoldParticle", "", false);
            m_HpModel = DBCUtil.ExtractString(node, "HpModel", "", false);
            m_HpParticle = DBCUtil.ExtractString(node, "HpParticle", "", false);
            m_MpModel = DBCUtil.ExtractString(node, "MpModel", "", false);
            m_MpParticle = DBCUtil.ExtractString(node, "MpParticle", "", false);
            m_ItemIdList = new List<int>();
            m_ItemWeightList = new List<int>();
            m_ItemCountList = new List<int>();
            for (int i = 0; i < m_ItemCount; i++)
            {
                m_ItemIdList.Add(DBCUtil.ExtractNumeric<int>(node, "Item" + i, 0, false));
                m_ItemWeightList.Add(DBCUtil.ExtractNumeric<int>(node, "Weight" + i, 0, false));
                m_ItemCountList.Add(DBCUtil.ExtractNumeric<int>(node, "Count" + i, 0, false));
            }
            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class SceneConfigProvider
    {
        private DataDictionaryMgr<Data_SceneConfig> m_SceneConfigMgr = new DataDictionaryMgr<Data_SceneConfig>();
        private DataDictionaryMgr<Data_SceneDropOut> m_SceneDropOutMgr = new DataDictionaryMgr<Data_SceneDropOut>();
        private MyDictionary<int, MapDataProvider> m_MapDataProviders = new MyDictionary<int, MapDataProvider>();

        public void Load(string file, string root)
        {
            m_SceneConfigMgr.CollectDataFromDBC(file, root);
        }

        public void LoadDropOutConfig(string file, string root)
        {
            m_SceneDropOutMgr.CollectDataFromDBC(file, root);
        }

        public void LoadAllSceneConfig(string rootPath)
        {
            m_MapDataProviders.Clear();
            foreach (int id in m_SceneConfigMgr.GetData().Keys)
            {
                MapDataProvider data = LoadSceneConfig(id, rootPath);
                if (null != data)
                {
                    m_MapDataProviders.Add(id, data);
                }
            }
        }

        public MapDataProvider LoadSceneConfig(int id, string rootPath)
        {
            MapDataProvider provider = null;
            Data_SceneConfig sceneCfg = m_SceneConfigMgr.GetDataById(id);
            if(null != sceneCfg)
            {
                provider = new MapDataProvider();
                provider.CollectData(DataMap_Type.DT_Unit, rootPath + sceneCfg.m_UnitFile, "UnitInfo");
                provider.CollectData(DataMap_Type.DT_SceneLogic, rootPath + sceneCfg.m_SceneLogicFile, "SceneLogic");
            }
            return provider;
        }

        public static SceneConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static SceneConfigProvider s_Instance = new SceneConfigProvider();
    }
}
