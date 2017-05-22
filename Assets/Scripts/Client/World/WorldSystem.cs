using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashFireSpatial;
using DashFire.Network;

/**
 * @file GameSystem.cs
 * @brief 游戏系统
 *          负责：
 *                  切换场景
 *                  预加载资源
 * @version 1.0.0
 * @date 2012-12-16
 */

namespace DashFire
{
    /**
     * @brief 游戏系统
     */
    public class WorldSystem
    {
        private DelayActionProcessor m_DelayActionProcessor = new DelayActionProcessor();

        private bool m_IsObserver = false;
        private SceneResource m_CurScene;
        private List<int> m_DebugObstacleActors = new List<int>();
        private bool m_IsDebugObstacleCreated = false;


        private UserManager m_UserMgr = new UserManager(16);
        private NpcManager m_NpcMgr = new NpcManager(256);
        private SceneLogicInfoManager m_SceneLogicInfoMgr = new SceneLogicInfoManager(256);

        private SceneLogicSystem m_SceneLogicSystem = new SceneLogicSystem();
        //形状碰撞
        private SpatialSystem m_SpatialSystem = new SpatialSystem();

        private AiSystem m_AiSystem = new AiSystem();

        private BlackBoard m_BlackBoard = new BlackBoard();

        private SceneContextInfo m_SceneContext = new SceneContextInfo();

        private long m_LastTryChangeSceneTime = 0;

        //初始化
        public void Init()
        {
            m_IsObserver = false;
            m_CurScene = null;

            //未实现
            //GfxSystem.EventChannelForLogic.Subscribe("ge_change_hero", "game", ChangeHeroFromGfx);
            //GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_player_movemode", "game", ChangePlayerMoveMode);
            //GfxSystem.EventChannelForLogic.Subscribe<int, int>("ge_change_npc_movemode", "game", ChangeNpcMoveMode);
            GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_scene", "game", ChangeSceneFromGfx);//切换场景
            //GfxSystem.EventChannelForLogic.Subscribe("ge_resetdsl", "game", ResetDsl);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execscript", "game", ExecScript);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execcommand", "game", ExecCommand);
        }

        private void ChangeSceneFromGfx(int sceneId)
        {
            try
            {
                if(null == m_CurScene || m_CurScene.IsSuccessEnter)
                {
                    if(0 == sceneId)
                    {
                        if (IsPvpScene() || IsMultiPveScene())
                        {
                            NetworkSystem.Instance.QuitBattle();
                        }
                        LobbyNetworkSystem.Instance.QuitClient();    
                    }

                    ChangeScene(sceneId);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("ExecCommand exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private bool ChangeScene(int sceneId)
        {
            try
            {
                if (null != m_CurScene)
                {
                    if (m_CurScene.ResId == sceneId)
                    {
                        return true;
                    }
                    else if (!m_CurScene.IsSuccessEnter && !this.IsServerSelectScene())
                    {
                        return false;
                    }

                    Reset();
                    m_CurScene.Release();
                    m_CurScene = null;
                }
                else
                {
                    Reset();
                }

                m_LastTryChangeSceneTime = TimeUtility.GetLocalMilliseconds();//运行到现在时间
                m_CurScene = new SceneResource();//场景管理
                m_CurScene.Init(sceneId);//初始化
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }

            return false;
        }

        //Obstacle 障碍物
        private void DestroyObstacleObjects()
        {
            foreach (int actor in m_DebugObstacleActors)
            {
                GfxSystem.DestroyGameObject(actor);
            }
            m_IsDebugObstacleCreated = false;
        }

        public void Reset()
        {
            LogSystem.Debug("WorldSystem.Reset Destory Objects...");
            DestroyObstacleObjects();

            for (LinkedListNode<UserInfo> linkNode = m_UserMgr.Users.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                UserInfo info = linkNode.Value;
                if (null != info)
                {
                    EntityManager.Instance.DestroyUserView(info.GetId());
                }
            }
            for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo info = linkNode.Value;
                if (null != info)
                {
                    EntityManager.Instance.DestroyNpcView(info.GetId());
                }
            }
            LogSystem.Debug("WorldSystem.Reset Destory Objects Finish.");

            m_UserMgr.Reset();
            m_NpcMgr.Reset();
            m_SceneLogicInfoMgr.Reset();

            m_SceneLogicSystem.Reset();
            m_SpatialSystem.Reset();
            m_AiSystem.Reset();
            m_BlackBoard.Reset();

            ControlSystemOperation.Reset();

            ClientStorySystem.Instance.Reset();
            ClientStorySystem.Instance.ClearStoryInstancePool();
            StorySystem.StoryConfigManager.Instance.Clear();
        }

        public CharacterInfo GetCharacterById(int id)
        {
            CharacterInfo obj = null;
            if (null != m_NpcMgr)
                obj = m_NpcMgr.GetNpcInfo(id);
            if (null != m_UserMgr && null == obj)
                obj = m_UserMgr.GetUserInfo(id);
            return obj;
        }

        public SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
        }

        public void LoadData()
        {
            SceneConfigProvider.Instance.Load(FilePathDefine_Client.C_SceneConfig, "ScenesConfigs");
            SceneConfigProvider.Instance.LoadDropOutConfig(FilePathDefine_Client.C_SceneDropOut, "SceneDropOut");
            SceneConfigProvider.Instance.LoadAllSceneConfig(FilePathDefine_Client.C_RootPath);

            ActionConfigProvider.Instance.Load(FilePathDefine_Client.C_ActionConfig, "ActionConfig");
            NpcConfigProvider.Instance.LoadNpcConfig(FilePathDefine_Client.C_NpcConfig, "NpcConfig");
            NpcConfigProvider.Instance.LoadNpcLevelupConfig(FilePathDefine_Client.C_NpcLevelupConfig, "NpcLevelupConfig");
            PlayerConfigProvider.Instance.LoadPlayerConfig(FilePathDefine_Client.C_PlayerConfig, "PlayerConfig");
            PlayerConfigProvider.Instance.LoadPlayerLevelupConfig(FilePathDefine_Client.C_PlayerLevelupConfig, "PlayerLevelupConfig");
            PlayerConfigProvider.Instance.LoadPlayerLevelupExpConfig(FilePathDefine_Client.C_PlayerLevelupExpConfig, "PlayerLevelupExpConfig");
            CriticalConfigProvider.Instance.Load(FilePathDefine_Client.C_CriticalConfig, "CriticalConfig");
            AttributeScoreConfigProvider.Instance.Load(FilePathDefine_Client.C_AttributeScoreConfig, "AttributeScoreConfig");

            AiActionConfigProvider.Instance.Load(FilePathDefine_Client.C_AiActionConfig, "AiActionConfig");

            ItemConfigProvider.Instance.Load(FilePathDefine_Client.C_ItemConfig, "ItemConfig");
            ItemLevelupConfigProvider.Instance.Load(FilePathDefine_Client.C_ItemLevelupConfig, "ItemLevelupConfig");
            EquipmentConfigProvider.Instance.LoadEquipmentConfig(FilePathDefine_Client.C_EquipmentConfig, "EquipmentConfig");
            BuyStaminaConfigProvider.Instance.Load(FilePathDefine_Client.C_BuyStaminaConfig, "BuyStaminaConfig");
            AppendAttributeConfigProvider.Instance.Load(FilePathDefine_Client.C_AppendAttributeConfig, "AppendAttributeConfig");
            LegacyLevelupConfigProvider.Instance.Load(FilePathDefine_Client.C_LegacyLevelupConfig, "LegacyLevelupConfig");

            SkillConfigProvider.Instance.CollectData(SkillConfigType.SCT_SOUND, FilePathDefine_Client.C_SoundConfig, "SoundConfig");
            SkillConfigProvider.Instance.CollectData(SkillConfigType.SCT_SKILL, FilePathDefine_Client.C_SkillSystemConfig, "SkillConfig");
            SkillConfigProvider.Instance.CollectData(SkillConfigType.SCT_IMPACT, FilePathDefine_Client.C_ImpactSystemConfig, "ImpactConfig");
            SkillConfigProvider.Instance.CollectData(SkillConfigType.SCT_EFFECT, FilePathDefine_Client.C_EffectConfig, "EffectConfig");
            SkillLevelupConfigProvider.Instance.Load(FilePathDefine_Client.C_SkillLevelupConfig, "SkillLevelupConfig");

            BuffConfigProvider.Instance.Load(FilePathDefine_Client.C_BuffConfig, "BuffConfig");

            SoundConfigProvider.Instance.Load(FilePathDefine_Client.C_GlobalSoundConfig, "C_GlobalSoundConfig");
            StrDictionaryProvider.Instance.Load(FilePathDefine_Client.C_StrDictionary, "StrDictionary");

            NewbieGuideProvider.Instance.Load(FilePathDefine_Client.C_NewbieGuide, "NewbieGuide");
            SystemGuideConfigProvider.Instance.Load(FilePathDefine_Client.C_SystemGuideConfig, "SystemGuideConfig");

            UiConfigProvider.Instance.Load(FilePathDefine_Client.C_UiConfig, "UiConfig");
            ServerConfigProvider.Instance.Load(FilePathDefine_Client.C_ServerConfig, "ServerConfig");
            MissionConfigProvider.Instance.Load(FilePathDefine_Client.C_MissionConfig, "MissionConfig");
            WordFilter.Instance.Load(FilePathDefine_Client.C_SensitiveDictionary);
            DynamicSceneConfigProvider.Instance.CollectData(FilePathDefine_Client.C_DynamicSceneConfig, "DynamicSceneConfig");

            ExpeditionMonsterAttrConfigProvider.Instance.Load(FilePathDefine_Client.C_ExpeditionMonsterAttrConfig, "ExpeditionMonsterAttrConfig");
            ExpeditionTollgateConfigProvider.Instance.Load(FilePathDefine_Client.C_ExpeditionTollgateConfig, "ExpeditionTollgateConfig");
            ExpeditionMonsterConfigProvider.Instance.Load(FilePathDefine_Client.C_ExpeditionMonsterConfig, "ExpeditionMonsterConfig");

            BuyMoneyConfigProvider.Instance.Load(FilePathDefine_Client.C_BuyMoneyConfig, "BuyMoneyConfig");
            GowConfigProvider.Instance.LoadForClient();

            VipConfigProvider.Instance.Load(FilePathDefine_Client.C_VipConfig, "VipConfig");
            VersionConfigProvider.Instance.Load(FilePathDefine_Client.C_VersionConfig, "VersionConfig");
        }

        public void QueueAction(MyAction action)
        {
            m_DelayActionProcessor.QueueAction(action);
        }

        //单例
        #region Singleton
        private static WorldSystem s_Instance = new WorldSystem();
        public static WorldSystem Instance
        {
            get { return s_Instance; }
        }

        #endregion



        public bool IsMultiPveScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsMultiPve;
        }

        public bool IsPvpScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsPvp;
        }

        public bool IsServerSelectScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsServerSelectScene;
        }
    }
}
