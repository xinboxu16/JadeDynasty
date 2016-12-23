using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    internal class FilePathDefine
    {
        public const string C_FirstName = @"Public/FirstName.txt";
        public const string C_LastName = @"Public/LastName.txt";
        public const string C_CuteFirstName = @"Public/CuteFirstName.txt";
        public const string C_CuteLastName = @"Public/CuteLastName.txt";
        public const string C_ChineseFirstName = @"Public/ChineseFirstName.txt";
        public const string C_ChineseLastName = @"Public/ChineseLastName.txt";
        public const string C_ForeignFirstName = @"Public/ForeignFirstName.txt";
        public const string C_ForeignLastName = @"Public/ForeignLastName.txt";
        public const string C_SceneConfig = @"Public/Scenes/SceneConfig.txt";
        public const string C_SceneDropOut = @"Public/Scenes/SceneDropOut.txt";

        public const string C_ActionConfig = @"Public/ActionConfig.txt";
        public const string C_NpcConfig = @"Public/NpcConfig.txt";
        public const string C_NpcLevelupConfig = @"Public/NpcLevelupConfig.txt";
        public const string C_PlayerConfig = @"Public/PlayerConfig.txt";
        public const string C_PlayerLevelupConfig = @"Public/PlayerLevelupConfig.txt";
        public const string C_PlayerLevelupExpConfig = @"Public/PlayerLevelupExpConfig.txt";
        public const string C_CriticalConfig = @"Public/CriticalConfig.txt";
        public const string C_DynamicSceneConfig = @"Public/DynamicSceneConfig.txt";
        public const string C_AttributeScoreConfig = @"Public/AttributeScoreConfig.txt";

        //物品装备
        public const string C_ItemConfig = @"Public/ItemConfig.txt";
        public const string C_ItemLevelupConfig = @"Public/ItemLevelupConfig.txt";
        public const string C_EquipmentConfig = @"Public/EquipmentConfig.txt";

        // AI
        public const string C_AiActionConfig = @"Public/AiActionConfig.txt";
        // 技能系统配置
        public const string C_SkillSystemConfig = @"Public/Skill/SkillData.txt";
        public const string C_ImpactSystemConfig = @"Public/Skill/ImpactData.txt";
        public const string C_SoundConfig = @"Public/Skill/SoundData.txt";
        public const string C_EffectConfig = @"Public/Skill/EffectData.txt";
        public const string C_BuffConfig = @"Public/Skill/BuffData.txt";
        public const string C_SkillSoundConfig = @"Public/Skill/SoundData.txt";
        public const string C_BuyStaminaConfig = @"Public/BuyStaminaConfig.txt";
        public const string C_BuyMoneyConfig = @"Public/BuyMoneyConfig.txt";
        public const string C_AppendAttributeConfig = @"Public/AppendAttributeConfig.txt";
        public const string C_LegacyLevelupConfig = @"Public/LegacyLevelupConfig.txt";
        public const string C_SkillLevelupConfig = @"Public/Skill/SkillLevelupConfig.txt";

        public const string C_ExpeditionMonsterAttrConfig = @"Public/ExpeditionMonsterAttrConfig.txt";
        public const string C_ExpeditionTollgateConfig = @"Public/ExpeditionTollgateConfig.txt";
        public const string C_ExpeditionMonsterConfig = @"Public/ExpeditionMonsterConfig.txt";

        public const string C_GowPrizeConfig = @"Public/Gow/GowPrize.txt";
        public const string C_GowTimeConfig = @"Public/Gow/GowTime.txt";

        //技能Dsl文件根
        public const string C_SkillDslPath = @"Public/SkillDsl/";

        //任务配置文件
        public const string C_MissionConfig = @"Public/MissionConfig.txt";

        public const string C_VipConfig = @"Public/VipConfig.txt";
        public const string C_VersionConfig = @"Public/VersionConfig.txt";

        //客户端专用
        public const string C_ResPoolConfig = @"Client/ResPoolConfig.txt";
        public const string C_StrDictionary = @"Client/StrDictionary.txt";
        public const string C_NewbieGuide = @"Client/NewbieGuide.txt";
        public const string C_SystemGuideConfig = @"Client/SystemGuideConfig.txt";
        public const string C_UiConfig = @"Client/UiConfig.txt";
        public const string C_DialogPath = @"Client/Dialog/";
        public const string C_DialogConfig = @"Client/Dialog/StoryDlgConfig.txt";
        public const string C_GlobalSoundConfig = @"Client/SoundConfig.txt";
        public const string C_ServerConfig = @"Client/ServerConfig.txt";
        public const string C_SensitiveDictionary = @"Client/SensitiveDictionary.txt";
    }

    public class FilePathDefine_Client
    {
        public const string C_RootPath = "";

        public const string C_SceneConfig = C_RootPath + FilePathDefine.C_SceneConfig;
        public const string C_SceneDropOut = C_RootPath + FilePathDefine.C_SceneDropOut;

        public const string C_ActionConfig = C_RootPath + FilePathDefine.C_ActionConfig;
        public const string C_NpcConfig = C_RootPath + FilePathDefine.C_NpcConfig;
        public const string C_NpcLevelupConfig = C_RootPath + FilePathDefine.C_NpcLevelupConfig;
        public const string C_PlayerConfig = C_RootPath + FilePathDefine.C_PlayerConfig;
        public const string C_PlayerLevelupConfig = C_RootPath + FilePathDefine.C_PlayerLevelupConfig;
        public const string C_PlayerLevelupExpConfig = C_RootPath + FilePathDefine.C_PlayerLevelupExpConfig;
        public const string C_CriticalConfig = C_RootPath + FilePathDefine.C_CriticalConfig;

        public const string C_DynamicSceneConfig = C_RootPath + FilePathDefine.C_DynamicSceneConfig;
        public const string C_AttributeScoreConfig = C_RootPath + FilePathDefine.C_AttributeScoreConfig;

        // AI
        public const string C_AiActionConfig = C_RootPath + FilePathDefine.C_AiActionConfig;
        //物品装备
        public const string C_ItemConfig = C_RootPath + FilePathDefine.C_ItemConfig;
        public const string C_ItemLevelupConfig = C_RootPath + FilePathDefine.C_ItemLevelupConfig;
        public const string C_EquipmentConfig = C_RootPath + FilePathDefine.C_EquipmentConfig;
        public const string C_GlobalSoundConfig = C_RootPath + FilePathDefine.C_GlobalSoundConfig;
        public const string C_BuyStaminaConfig = C_RootPath + FilePathDefine.C_BuyStaminaConfig;
        public const string C_AppendAttributeConfig = C_RootPath + FilePathDefine.C_AppendAttributeConfig;
        public const string C_LegacyLevelupConfig = C_RootPath + FilePathDefine.C_LegacyLevelupConfig;

        // 技能系统配置
        public const string C_SkillSystemConfig = C_RootPath + FilePathDefine.C_SkillSystemConfig;
        public const string C_ImpactSystemConfig = C_RootPath + FilePathDefine.C_ImpactSystemConfig;
        public const string C_BuffConfig = C_RootPath + FilePathDefine.C_BuffConfig;
        public const string C_SoundConfig = C_RootPath + FilePathDefine.C_SoundConfig;
        public const string C_EffectConfig = C_RootPath + FilePathDefine.C_EffectConfig;
        public const string C_SkillLevelupConfig = C_RootPath + FilePathDefine.C_SkillLevelupConfig;
        // 远征
        public const string C_ExpeditionMonsterAttrConfig = C_RootPath + FilePathDefine.C_ExpeditionMonsterAttrConfig;
        public const string C_ExpeditionTollgateConfig = C_RootPath + FilePathDefine.C_ExpeditionTollgateConfig;
        public const string C_ExpeditionMonsterConfig = C_RootPath + FilePathDefine.C_ExpeditionMonsterConfig;
        // 战神赛
        public const string C_GowPrizeConfig = C_RootPath + FilePathDefine.C_GowPrizeConfig;
        public const string C_GowTimeConfig = C_RootPath + FilePathDefine.C_GowTimeConfig;

        public const string C_SkillDslPath = C_RootPath + FilePathDefine.C_SkillDslPath;

        public const string C_ResPoolConfig = C_RootPath + FilePathDefine.C_ResPoolConfig;

        public const string C_StrDictionary = C_RootPath + FilePathDefine.C_StrDictionary;
        public const string C_NewbieGuide = C_RootPath + FilePathDefine.C_NewbieGuide;
        public const string C_SystemGuideConfig = C_RootPath + FilePathDefine.C_SystemGuideConfig;
        public const string C_UiConfig = C_RootPath + FilePathDefine.C_UiConfig;
        public const string C_DialogPath = C_RootPath + FilePathDefine.C_DialogPath;
        public const string C_DialogConfig = C_RootPath + FilePathDefine.C_DialogConfig;
        //服务器列表配置
        public const string C_ServerConfig = C_RootPath + FilePathDefine.C_ServerConfig;
        //任务配置
        public const string C_MissionConfig = C_RootPath + FilePathDefine.C_MissionConfig;
        //敏感词库
        public const string C_SensitiveDictionary = C_RootPath + FilePathDefine.C_SensitiveDictionary;

        public const string C_BuyMoneyConfig = C_RootPath + FilePathDefine.C_BuyMoneyConfig;

        public const string C_VipConfig = C_RootPath + FilePathDefine.C_VipConfig;
        public const string C_VersionConfig = C_RootPath + FilePathDefine.C_VersionConfig;
    }
}
