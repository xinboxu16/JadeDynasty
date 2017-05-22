using DashFire.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class SceneResource
    {
        private Data_SceneConfig m_SceneConfig;
        private Data_SceneDropOut m_SceneDropOut;
        private MapDataProvider m_SceneStaticData;

        private bool m_IsSuccessEnter = false;
        private bool m_IsWaitSceneLoad = true;
        private bool m_IsWaitRoomServerConnect = true;
        private float m_CameraLookAtHeight = 0;

        public bool IsSuccessEnter
        {
            get { return m_IsSuccessEnter; }
        }

        public void Release()
        {
            GfxSystem.GfxLog("SceneResource.Release");
        }

        public bool IsMultiPve
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_MULTI_PVE;
            }
        }
        public bool IsPvp
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_PVP;
            }
        }

        private int m_SceneResId;

        public int ResId
        {
            get
            {
                return m_SceneResId;
            }
        }

        public bool IsServerSelectScene
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_Type == (int)SceneTypeEnum.TYPE_SERVER_SELECT;
            }
        }

        public bool IsExpedition
        {
            get
            {
                if (null == m_SceneConfig)
                    return false;
                else
                    return m_SceneConfig.m_SubType == (int)SceneSubTypeEnum.TYPE_EXPEDITION;
            }
        }

        private bool LoadSceneData(int sceneResId)
        {
            bool result = true;
            m_SceneResId = sceneResId;
            // 加载场景配置数据
            m_SceneConfig = SceneConfigProvider.Instance.GetSceneConfigById(sceneResId);
            if(null == m_SceneConfig)
            {
                LogSystem.Error("LoadSceneData {0} failed!", sceneResId);
            }
            //场景掉落
            m_SceneDropOut = SceneConfigProvider.Instance.GetSceneDropOutById(sceneResId);
            // 加载本场景xml数据
            m_SceneStaticData = SceneConfigProvider.Instance.GetMapDataBySceneResId(m_SceneResId);

            HashSet<int> monstList = null;
            if(IsExpedition)
            {
                monstList = new HashSet<int>();
                RoleInfo curRole = LobbyClient.Instance.CurrentRole;
                ExpeditionPlayerInfo expInfo = curRole.GetExpeditionInfo();
                ExpeditionPlayerInfo.TollgateData data = expInfo.Tollgates[expInfo.ActiveTollgate];
                monstList.UnionWith(data.EnemyList);
            }
            GfxSystem.LoadScene(m_SceneConfig.m_ClientSceneFile, m_SceneConfig.m_Chapter, m_SceneConfig.GetId(), monstList, OnLoadFinish);
            return result;
        }

        private void OnLoadFinish()
        {
            LogSystem.Info("SceneResource.OnLoadFinish");
            m_IsWaitSceneLoad = false;
            //if (null != m_SceneDropOut)
            //{
            //    //LogSystem.Debug("{0} {1} {2}", m_SceneDropOut.m_GoldSum, m_SceneDropOut.m_GoldMin, m_SceneDropOut.m_GoldMax);
            //}
            //foreach (int id in m_DropMoneyData.Keys)
            //{
            //    //LogSystem.Debug("id = {0}, monew = {1}", id, m_DropMoneyData[id]);
            //}

            Data_Unit unit = m_SceneStaticData.ExtractData(DataMap_Type.DT_Unit, GlobalVariables.GetUnitIdByCampId(NetworkSystem.Instance.CampId)) as Data_Unit;
            if (null != unit)
            {
                GfxSystem.SendMessage("GfxGameRoot", "CameraLookatImmediately", new float[] { unit.m_Pos.x, unit.m_Pos.y, unit.m_Pos.z });
            }
        }

        public void Init(int resId)
        {
            m_SceneResId = resId;
            LoadSceneData(resId);
            WorldSystem.Instance.SceneContext.SceneResId = resId;
            WorldSystem.Instance.SceneContext.IsRunWithRoomServer = IsPvp || IsMultiPve;
            m_IsWaitSceneLoad = true;
            m_IsWaitRoomServerConnect = true;
            m_IsSuccessEnter = false;

            Data_Unit unit = m_SceneStaticData.ExtractData(DataMap_Type.DT_Unit, GlobalVariables.GetUnitIdByCampId(NetworkSystem.Instance.CampId)) as Data_Unit;
            if (null != unit)
            {
                m_CameraLookAtHeight = unit.m_Pos.y;
            }

            GfxSystem.GfxLog("SceneResource.Init {0}", resId);
        }
    }
}
