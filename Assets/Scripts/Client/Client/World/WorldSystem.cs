using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        //初始化
        public void Init()
        {
            m_IsObserver = false;
            m_CurScene = null;

            //未实现
            //GfxSystem.EventChannelForLogic.Subscribe("ge_change_hero", "game", ChangeHeroFromGfx);
            //GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_player_movemode", "game", ChangePlayerMoveMode);
            //GfxSystem.EventChannelForLogic.Subscribe<int, int>("ge_change_npc_movemode", "game", ChangeNpcMoveMode);
            //GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_scene", "game", ChangeSceneFromGfx);
            //GfxSystem.EventChannelForLogic.Subscribe("ge_resetdsl", "game", ResetDsl);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execscript", "game", ExecScript);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execcommand", "game", ExecCommand);
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
    }
}
