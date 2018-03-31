using DashFireMessage;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DashFire.Network
{
    public enum LoginMode : int
    {
        AccountLogin = 0,
        DirectLogin = 1,
    }

    public sealed partial class LobbyNetworkSystem
    {
        private void LobbyMessageInit()
        {
            GfxSystem.EventChannelForLogic.Subscribe<string, int>("ge_select_server", "lobby", SelectServer);
            GfxSystem.EventChannelForLogic.Subscribe("ge_direct_login", "lobby", DirectLoginLobby);
            GfxSystem.EventChannelForLogic.Subscribe<string>("ge_device_info", "lobby", InitDeviceInfo);
            GfxSystem.EventChannelForLogic.Subscribe("ge_create_nickname", "lobby", CreateNickname);
            GfxSystem.EventChannelForLogic.Subscribe<int, string>("ge_create_role", "lobby", CreateRole);
            GfxSystem.EventChannelForLogic.Subscribe<int>("ge_role_enter", "lobby", RoleEnter);

            GfxSystem.EventChannelForLogic.Subscribe<bool>("ge_request_relive", "lobby", RequestRelive);
            GfxSystem.EventChannelForLogic.Subscribe<int>("ge_stage_clear", "lobby", StageClear);

            LobbyMessageHandler();
        }

        private void LobbyMessageHandler()
        {
            RegisterMsgHandler(JsonMessageID.VersionVerifyResult, HandleVersionVerifyResult);
            RegisterMsgHandler(JsonMessageID.AccountLoginResult, HandleAccountLoginResult);
            RegisterMsgHandler(JsonMessageID.RoleListResult, typeof(DashFireMessage.Msg_LC_RoleListResult), HandleRoleListResult);
            RegisterMsgHandler(JsonMessageID.CreateNicknameResult, HandleCreateNicknameResult);
            RegisterMsgHandler(JsonMessageID.CreateRoleResult, HandleCreateRoleResult);
            RegisterMsgHandler(JsonMessageID.RoleEnterResult, typeof(DashFireMessage.Msg_LC_RoleEnterResult), HandleRoleEnterResult);
            RegisterMsgHandler(JsonMessageID.StageClearResult, typeof(DashFireMessage.Msg_LC_StageClearResult), HandleStageClearResult);
        }

        private void SelectServer(string serverAddress, int logicServerId)
        {
            m_Url = serverAddress;
            m_LogicServerId = logicServerId;
        }

        private void InitDeviceInfo(string uniqueIdentifier)
        {
            try
            {
                m_UniqueIdentifier = uniqueIdentifier;
            }
            catch (Exception ex)
            {
                DashFire.LogicSystem.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void CreateNickname()
        {
            try
            {
                JsonData sendMsg = new JsonData();
                sendMsg["m_Account"] = LobbyClient.Instance.AccountInfo.Account;
                SendMessage(JsonMessageID.CreateNickname, sendMsg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void CreateRole(int heroId, string nickname)
        {
            if (heroId <= 0)
            {
                return;
            }
            try
            {
                JsonData sendMsg = new JsonData();
                sendMsg["m_Account"] = LobbyClient.Instance.AccountInfo.Account;
                sendMsg["m_HeroId"] = heroId;
                sendMsg["m_Nickname"] = nickname;
                SendMessage(JsonMessageID.CreateRole, sendMsg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void RoleEnter(int index)
        {
            if (index < 0 || index >= LobbyClient.Instance.AccountInfo.Players.Count)
            {
                return;
            }
            try
            {
                RoleInfo pi = LobbyClient.Instance.AccountInfo.Players[index];
                if (pi != null)
                {
                    JsonMessage msg = new JsonMessage(JsonMessageID.RoleEnter);
                    msg.m_JsonData.Set("m_Account", LobbyClient.Instance.AccountInfo.Account);
                    DashFireMessage.Msg_CL_RoleEnter protoData = new DashFireMessage.Msg_CL_RoleEnter();
                    protoData.m_Guid = pi.Guid;
                    msg.m_ProtoData = protoData;
                    SendMessage(msg);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        //场景结算
        private void StageClear(int mainCityId)
        {
            try
            {
                LobbyClient.Instance.CurrentRole.CitySceneId = mainCityId;
                if (WorldSystem.Instance.IsPveScene() || WorldSystem.Instance.IsPvpScene())
                {
                    UserInfo player = WorldSystem.Instance.GetPlayerSelf();
                    if (null != player)
                    {
                        CombatStatisticInfo combatInfo = player.GetCombatStatisticInfo();
                        JsonMessage msg = new JsonMessage(JsonMessageID.StageClear);
                        msg.m_JsonData.Set("m_Guid", m_Guid);
                        DashFireMessage.Msg_CL_StageClear protoData = new DashFireMessage.Msg_CL_StageClear();
                        protoData.m_SceneId = NetworkSystem.Instance.SceneId;
                        protoData.m_HitCount = combatInfo.HitCount;
                        protoData.m_MaxMultHitCount = combatInfo.MaxMultiHitCount;
                        protoData.m_Hp = player.Hp;
                        protoData.m_Mp = player.Energy;
                        protoData.m_Gold = player.Money;
                        msg.m_ProtoData = protoData;
                        SendMessage(msg);
                        LogSystem.Info("SendMessage StageClear to lobby");
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        //清理场景返回
        private void HandleStageClearResult(JsonMessage lobbyMsg)
        {
            LogSystem.Debug("Receive StageClear message from lobby");
            if (null == lobbyMsg) return;
            JsonData jsonData = lobbyMsg.m_JsonData;
            DashFireMessage.Msg_LC_StageClearResult protoData = lobbyMsg.m_ProtoData as DashFireMessage.Msg_LC_StageClearResult;
            if(null != protoData)
            {
                int sceneId = WorldSystem.Instance.GetCurSceneId();
                ulong userGuid = jsonData.GetUlong("m_Guid");
                RoleInfo player = LobbyClient.Instance.AccountInfo.FindRole(userGuid);
                int hitCount = protoData.m_HitCount;
                int maxMultHitCount = protoData.m_MaxMultHitCount;
                long duration = protoData.m_Duration;
                int itemId = protoData.m_ItemId;
                int itemCount = protoData.m_ItemCount;
                int expPoint = protoData.m_ExpPoint;
                int hp = protoData.m_Hp;
                int mp = protoData.m_Mp;
                int gold = protoData.m_Gold;
                int deadCount = protoData.m_DeadCount;
                int completedRewardId = protoData.m_CompletedRewardId;
                int curSceneStar = protoData.m_SceneStarNum;
                Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(sceneId);
                if (null != cfg && cfg.m_SubType == (int)SceneSubTypeEnum.TYPE_EXPEDITION)
                {
                    ExpeditionPlayerInfo expedition = player.GetExpeditionInfo();
                    if (null != expedition)
                    {
                        expedition.Hp = hp;
                        expedition.Mp = mp;
                    }
                }
                int sceneStars = player.GetSceneInfo(sceneId);
                // 第一次通关副本不显示结算
                if (player.SceneInfo.Count == 0)
                {
                    ReturnMainCity();
                }
                else
                {
                    //TODO 未实现
                    if (WorldSystem.Instance.IsPveScene())
                    {
                        // 胜利页面
                        GfxSystem.PublishGfxEvent("ge_victory_panel", "ui", sceneId, maxMultHitCount, hitCount, deadCount, (int)(duration / 1000), expPoint, gold, (curSceneStar > sceneStars));
                        // 翻牌页面
                        GfxSystem.PublishGfxEvent("ge_turnover_card", "ui", itemId, itemCount);
                    }
                }

                // 记录通关信息
                player.SetSceneInfo(sceneId, curSceneStar);
                player.AddCompletedSceneCount(sceneId);

                // 客户端结算
                player.Exp += expPoint;
                player.Money += gold;

                // 首次通关结算
                if (completedRewardId > 0)
                {
                    Data_SceneDropOut dropOutConfig = SceneConfigProvider.Instance.GetSceneDropOutById(completedRewardId);
                    if (null != dropOutConfig)
                    {
                        player.Exp += dropOutConfig.m_Exp;
                        player.Money += dropOutConfig.m_GoldSum;
                        player.Gold += dropOutConfig.m_Diamond;
                    }
                }

                //任务处理
                if (null != player)
                {
                    MissionStateInfo mission_info = player.GetMissionStateInfo();
                    if (null != protoData.m_Missions && protoData.m_Missions.Count > 0 && null != mission_info)
                    {
                        int ct = protoData.m_Missions.Count;
                        for (int i = 0; i < ct; ++i)
                        {
                            DashFireMessage.Msg_LC_StageClearResult.MissionInfoForSync assit_info = protoData.m_Missions[i];
                            if (assit_info.m_IsCompleted)
                            {
                                mission_info.CompletedMission(assit_info.m_MissionId);
                                mission_info.CompletedMissions[assit_info.m_MissionId].Progress = assit_info.m_Progress;
                            }
                            else
                            {
                                if (protoData.m_Missions[i].m_MissionId >= 0)
                                {
                                    mission_info.AddMission(assit_info.m_MissionId, MissionStateType.UNCOMPLETED);
                                    mission_info.UnCompletedMissions[assit_info.m_MissionId].Progress = assit_info.m_Progress;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ReturnMainCity()
        {
            try
            {
                //切换场景
                WorldSystem.Instance.ChangeScene(LobbyClient.Instance.CurrentRole.CitySceneId);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void RequestRelive(bool isRelive)//Relive重新过
        {
            try
            {
                if(isRelive)
                {
                    JsonMessage msg = new JsonMessage(JsonMessageID.BuyLife);
                    msg.m_JsonData.Set("m_Guid", LobbyNetworkSystem.Instance.Guid);
                    SendMessage(msg);
                }
                else
                {
                    // 放弃副本
                    if(WorldSystem.Instance.IsPveScene())
                    {
                        ClientStorySystem.Instance.SendMessage("missionfailed");
                    }
                    else
                    {
                        //通知roomserver 放弃副本
                        NetworkSystem.Instance.SyncGiveUpCombat();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        //直接登录
        private void DirectLoginLobby()
        {
            try
            {
                m_LoginMode = LoginMode.DirectLogin;
                m_Account = m_UniqueIdentifier;
                ConnectIfNotOpen();
                m_IsWaitStart = false;
                m_IsLogged = false;
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void QueryExpeditionInfo()
        {
            try
            {
                JsonData queryExpeditionInfoMsg = new JsonData();
                queryExpeditionInfoMsg.Set("m_Guid", m_Guid);
                SendMessage(JsonMessageID.QueryExpeditionInfo, queryExpeditionInfoMsg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void UpdateFightingScore(float score)
        {
            try
            {
                UserInfo playerself = WorldSystem.Instance.GetPlayerSelf();
                if (null == playerself)
                    return;
                JsonMessage msg = new JsonMessage(JsonMessageID.UpdateFightingScore);
                msg.m_JsonData.Set("m_Guid", LobbyNetworkSystem.Instance.Guid);
                DashFireMessage.Msg_CL_UpdateFightingScore protoData = new DashFireMessage.Msg_CL_UpdateFightingScore();
                protoData.score = (int)score;
                msg.m_ProtoData = protoData;
                SendMessage(msg);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        //获取随机名字
        private void HandleCreateNicknameResult(JsonMessage lobbyMsg)
        {
            List<string> nickNameList = new List<string>();
            JsonData jsonData = lobbyMsg.m_JsonData;
            JsonData nicknames = jsonData["m_Nicknames"];
            if (nicknames.IsArray && nicknames.Count > 0)
            {
                for (int i = 0; i < nicknames.Count; ++i)
                {
                    nickNameList.Add(nicknames[i].AsString());
                }
            }
            GfxSystem.PublishGfxEvent("ge_nickname_result", "lobby", nickNameList);
        }

        private void HandleRoleEnterResult(JsonMessage lobbyMsg)
        {
            JsonData jsonData = lobbyMsg.m_JsonData;
            DashFireMessage.Msg_LC_RoleEnterResult protoData = lobbyMsg.m_ProtoData as DashFireMessage.Msg_LC_RoleEnterResult;
            if (null != protoData)
            {
                int ret = protoData.m_Result;
                ulong userGuid = jsonData.GetUlong("m_Guid");
                RoleInfo role = LobbyClient.Instance.AccountInfo.FindRole(userGuid);
                if (ret == (int)RoleEnterResult.Wait)
                {
                    m_Guid = userGuid;
                    Thread.Sleep(2000);
                    JsonMessage msg = new JsonMessage(JsonMessageID.RoleEnter);
                    msg.m_JsonData.Set("m_Account", LobbyClient.Instance.AccountInfo.Account);
                    DashFireMessage.Msg_CL_RoleEnter data = new DashFireMessage.Msg_CL_RoleEnter();
                    data.m_Guid = m_Guid;
                    msg.m_ProtoData = data;
                    SendMessage(msg);
                    LogSystem.Debug("Retry RoleEnter {0} {1}", LobbyClient.Instance.AccountInfo.Account, m_Guid);
                    return;
                }
                else if (ret == (int)RoleEnterResult.Success)
                {
                    if (role != null)
                    {
                        if (m_IsLogged)
                        {
                            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
                            return;
                        }
                        #region "HandleRoleEnterResult"
                        //客户端接收服务器传来的数据，创建玩家对象
                        role.NewBieGuideScene = protoData.m_NewbieGuideScene;
                        role.Money = protoData.m_Money;
                        role.Gold = protoData.m_Gold;
                        role.CurStamina = protoData.m_Stamina;
                        role.Exp = protoData.m_Exp;
                        role.Level = protoData.m_Level;
                        role.CitySceneId = protoData.m_CitySceneId;
                        role.BuyStaminaCount = protoData.m_BuyStaminaCount;
                        role.BuyMoneyCount = protoData.m_BuyMoneyCount;
                        role.SellItemGoldIncome = protoData.m_CurSellItemGoldIncome;
                        role.Vip = protoData.m_Vip;
                        GfxSystem.PublishGfxEvent("ge_role_enter_log", "log", LobbyClient.Instance.AccountInfo.AccountId, m_LogicServerId, role.Nickname, m_Guid, role.Level, LobbyClient.Instance.AccountInfo.AccountId);
                        // 通关信息
                        if(null != protoData.m_SceneData && protoData.m_SceneData.Count > 0)
                        {
                            int ct = protoData.m_SceneData.Count;
                            for(int i = 0; i < ct; ++i)
                            {
                                role.SetSceneInfo(protoData.m_SceneData[i].m_SceneId, protoData.m_SceneData[i].m_Grade);
                            }
                        }
                        //通关次数
                        if(null != protoData.m_SceneCompletedCountData && protoData.m_SceneCompletedCountData.Count > 0)
                        {
                            int ct = protoData.m_SceneCompletedCountData.Count;
                            for(int i = 0; i < ct; ++i)
                            {
                                role.AddCompletedSceneCount(protoData.m_SceneCompletedCountData[i].m_SceneId, protoData.m_SceneCompletedCountData[i].m_Count);
                            }
                        }
                        // 新手教学信息
                        if(null != protoData.m_NewbieGuides && protoData.m_NewbieGuides.Count > 0)
                        {
                            int ct = protoData.m_NewbieGuides.Count;
                            for(int i = 0; i < ct; ++i)
                            {
                                if(!role.NewbieGuides.Contains(protoData.m_NewbieGuides[i]))
                                {
                                    role.NewbieGuides.Add(protoData.m_NewbieGuides[i]);
                                }
                            }
                        }
                        // items
                        if (null != protoData.m_BagItems && protoData.m_BagItems.Count > 0 && null != role.Items)
                        {
                            int ct = protoData.m_BagItems.Count;
                            for(int i = 0; i < ct; i++)
                            {
                                ItemDataInfo info = new ItemDataInfo();
                                info.ItemId = protoData.m_BagItems[i].ItemId;
                                info.Level = protoData.m_BagItems[i].Level;
                                info.ItemNum = protoData.m_BagItems[i].Num;
                                info.RandomProperty = protoData.m_BagItems[i].AppendProperty;
                                role.Items.Add(info);
                            }
                        }
                        // equipments
                        if (null != protoData.m_Equipments && protoData.m_Equipments.Count > 0 && null != role.Equips)
                        {
                            int ct = protoData.m_Equipments.Count;
                            for (int i = 0; i < ct; i++)
                            {
                                if (null != protoData.m_Equipments[i])
                                {
                                    ItemDataInfo info = new ItemDataInfo();
                                    info.ItemId = protoData.m_Equipments[i].ItemId;
                                    info.Level = protoData.m_Equipments[i].Level;
                                    info.ItemNum = protoData.m_Equipments[i].Num;
                                    info.RandomProperty = protoData.m_Equipments[i].AppendProperty;
                                    role.SetEquip(i, info);
                                }
                            }
                        }
                        // skills
                        if (null != protoData.m_SkillInfo && protoData.m_SkillInfo.Count > 0 && null != role.SkillInfos)
                        {
                            Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(role.HeroId);
                            if (null != playerData && null != playerData.m_PreSkillList)
                            {
                                foreach (int skill_id_ in playerData.m_PreSkillList)
                                {
                                    role.SkillInfos.Add(new SkillInfo(skill_id_));
                                }
                            }

                            int ct = protoData.m_SkillInfo.Count;
                            for (int i = 0; i < ct; i++)
                            {
                                int skill_id = protoData.m_SkillInfo[i].ID;
                                SkillInfo si = new SkillInfo(skill_id);
                                si.SkillLevel = protoData.m_SkillInfo[i].Level;
                                si.Postions.Presets[0] = (SlotPosition)protoData.m_SkillInfo[i].Postions;
                                for (int index = 0; index < role.SkillInfos.Count; index++)
                                {
                                    if (role.SkillInfos[index].SkillId == si.SkillId)
                                    {
                                        role.SkillInfos[index].SkillLevel = si.SkillLevel;
                                        role.SkillInfos[index].Postions = si.Postions;
                                        break;
                                    }
                                }
                            }
                        }
                        // missions
                        MissionStateInfo mission_info = role.GetMissionStateInfo();
                        if (null != protoData.m_Missions && protoData.m_Missions.Count > 0 && null != mission_info)
                        {
                            int ct = protoData.m_Missions.Count;
                            for (int i = 0; i < ct; ++i)
                            {
                                if (protoData.m_Missions[i].m_IsCompleted)
                                {
                                    mission_info.AddMission(protoData.m_Missions[i].m_MissionId, MissionStateType.COMPLETED);
                                    mission_info.CompletedMissions[protoData.m_Missions[i].m_MissionId].Progress = protoData.m_Missions[i].m_Progress;
                                }
                                else
                                {
                                    mission_info.AddMission(protoData.m_Missions[i].m_MissionId, MissionStateType.UNCOMPLETED);
                                    mission_info.UnCompletedMissions[protoData.m_Missions[i].m_MissionId].Progress = protoData.m_Missions[i].m_Progress;
                                    GfxSystem.PublishGfxEvent("ge_about_task", "task", protoData.m_Missions[i].m_MissionId, MissionOperationType.ADD, protoData.m_Missions[i].m_Progress);
                                }
                            }
                        }
                        // legacys
                        if (null != protoData.m_Legacys && protoData.m_Legacys.Count > 0)
                        {
                            int ct = protoData.m_Legacys.Count;
                            for (int i = 0; i < ct; ++i)
                            {
                                if (null != protoData.m_Legacys[i])
                                {
                                    role.Legacys[i] = new ItemDataInfo();
                                    role.Legacys[i].ItemId = protoData.m_Legacys[i].ItemId;
                                    role.Legacys[i].Level = protoData.m_Legacys[i].Level;
                                    role.Legacys[i].RandomProperty = protoData.m_Legacys[i].AppendProperty;
                                    role.Legacys[i].IsUnlock = protoData.m_Legacys[i].IsUnlock;
                                }
                            }
                        }
                        // gow
                        if (null != protoData.m_Gow && null != role.Gow)
                        {
                            role.Gow.GowElo = protoData.m_Gow.GowElo;
                            role.Gow.GowMatches = protoData.m_Gow.GowMatches;
                            role.Gow.GowWinMatches = protoData.m_Gow.GowWinMatches;
                            role.Gow.LeftMatchCount = protoData.m_Gow.LeftMatchCount;
                        }
                        // friends
                        if (null != protoData.m_Friends && null != role.Friends)
                        {
                            role.Friends.Clear();
                            int ct = protoData.m_Friends.Count;
                            for (int i = 0; i < ct; ++i)
                            {
                                DashFireMessage.FriendInfoForMsg friend_msg = protoData.m_Friends[i];
                                if (null != friend_msg)
                                {
                                    FriendInfo friend_data = new FriendInfo();
                                    friend_data.Guid = friend_msg.Guid;
                                    friend_data.Nickname = friend_msg.Nickname;
                                    friend_data.Level = friend_msg.Level;
                                    friend_data.FightingScore = friend_msg.FightingScore;
                                    friend_data.IsBlack = friend_msg.IsBlack;
                                    if (!role.Friends.ContainsKey(friend_data.Guid))
                                    {
                                        role.Friends.Add(friend_data.Guid, friend_data);
                                    }
                                }
                            }
                        }
                        #endregion
                        //设置当前玩家角色
                        LobbyClient.Instance.CurrentRole = role;
                        m_Guid = userGuid;
                        if (null != LobbyClient.Instance.CurrentRole)
                        {
                            if (role.Level >= ExpeditionPlayerInfo.c_UnlockLevel)
                            {
                                QueryExpeditionInfo();
                            }
                            CharacterInfo.AddPropertyInfoChangeCB(LobbyClient.Instance.CurrentRole.FightingScoreChangeCB);
                        }
                        
                    }
                }

                if (role.SceneInfo.Count == 0)
                {
                    NetworkSystem.Instance.SceneId = LobbyClient.Instance.CurrentRole.NewBieGuideScene;
                }
                else
                {
                    NetworkSystem.Instance.SceneId = LobbyClient.Instance.CurrentRole.CitySceneId;
                }
                //进主城场景
                NetworkSystem.Instance.HeroId = LobbyClient.Instance.CurrentRole.HeroId;
                NetworkSystem.Instance.CampId = (int)CampIdEnum.Blue;
                GfxSystem.PublishGfxEvent("ge_login_finish", "lobby");
                WorldSystem.Instance.ChangeScene(NetworkSystem.Instance.SceneId);
                m_IsLogged = true;
            }
        }

        //创建角色返回
        private void HandleCreateRoleResult(JsonMessage lobbyMsg)
        {
            JsonData jsonData = lobbyMsg.m_JsonData;
            int ret = jsonData.GetInt("m_Result");
            if (ret == (int)CreateRoleResult.Success)
            {
                JsonData userInfo = jsonData["m_UserInfo"];
                ulong userGuid = userInfo.GetUlong("m_UserGuid");
                RoleInfo player = new RoleInfo();
                player.Guid = userGuid;
                player.Nickname = userInfo.GetString("m_Nickname");
                player.HeroId = userInfo.GetInt("m_HeroId");
                player.Level = userInfo.GetInt("m_Level");
                LobbyClient.Instance.AccountInfo.Players.Add(player);

                GfxSystem.PublishGfxEvent("ge_close_nickname_dialog", "lobby");

                //服务器自动开始游戏，客户端不需要再主动发RoleEnter消息
                //JsonData sendMsg = new JsonData();
                //sendMsg["m_Account"] = m_Account;
                //sendMsg["m_Guid"] = userGuid;
                //SendMessage(JsonMessageID.RoleEnter, sendMsg);
            }
            else
            {
                //角色创建失败
                GfxSystem.PublishGfxEvent("ge_createhero_result", "lobby", false);
            }
        }

        private void HandleAccountLoginResult(JsonMessage lobbyMsg)
        {
            JsonData jsonData = lobbyMsg.m_JsonData;
            int ret = jsonData.GetInt("m_Result");
            string accountId = jsonData.GetString("m_AccountId");
            if (m_IsLogged)//重连处理
            {
                JsonMessage msg = new JsonMessage(JsonMessageID.RoleEnter);
                msg.m_JsonData.Set("m_Account", LobbyClient.Instance.AccountInfo.Account);
                DashFireMessage.Msg_CL_RoleEnter protoData = new DashFireMessage.Msg_CL_RoleEnter();
                protoData.m_Guid = m_Guid;
                msg.m_ProtoData = protoData;
                SendMessage(msg);
            }
            else//首次登录处理
            {
                if(ret == (int)AccountLoginResult.Success)
                {
                    LobbyClient.Instance.AccountInfo.Account = m_Account;
                    LobbyClient.Instance.AccountInfo.AccountId = accountId;
                    //登录成功，向服务器请求玩家角色
                    JsonMessage sendMsg = new JsonMessage(JsonMessageID.RoleList);
                    sendMsg.m_JsonData.Set("m_Account", m_Account);
                    SendMessage(sendMsg);
                }
                else if(ret == (int)AccountLoginResult.FirstLogin)
                {
                    //账号首次登录，需要验证激活码
                }
                else
                {
                    //账号登录失败
                }
                GfxSystem.PublishGfxEvent("ge_login_result", "lobby", ret, accountId);
            }
        }

        private void HandleRoleListResult(JsonMessage lobbyMsg)
        {
            bool isSuccess = false;
            JsonData jsonData = lobbyMsg.m_JsonData;
            Msg_LC_RoleListResult protoData = lobbyMsg.m_ProtoData as Msg_LC_RoleListResult;
            if(null != protoData)
            {
                int ret = protoData.m_Result;
                if(ret == (int)RoleListResult.Success)
                {
                    //清空客户端已有的玩家角色列表
                    LobbyClient.Instance.AccountInfo.Players.Clear();
                    //获取玩家角色数据列表
                    int userInfoCount = protoData.m_UserInfoCount;
                    if(null != protoData.m_UserInfos && protoData.m_UserInfos.Count > 0)
                    {
                        int ct = protoData.m_UserInfos.Count;
                        for(int i = 0; i < ct; i++)
                        {
                            RoleInfo player = new RoleInfo();
                            player.Guid = protoData.m_UserInfos[i].m_UserGuid;
                            player.Nickname = protoData.m_UserInfos[i].m_Nickname;
                            player.HeroId = protoData.m_UserInfos[i].m_HeroId;
                            player.Level = protoData.m_UserInfos[i].m_Level;
                            LobbyClient.Instance.AccountInfo.Players.Add(player);
                        }
                    }
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
                GfxSystem.PublishGfxEvent("ge_rolelist_result", "lobby", isSuccess);
            }
        }

        private void HandleVersionVerifyResult(JsonMessage lobbyMsg)
        {
            JsonData jsonData = lobbyMsg.m_JsonData;
            int ret = jsonData.GetInt("m_Result");
            if(0 == ret)
            {
                //版本校验失败，提示用户需要更新版本。
                m_IsWaitStart = true;
                m_IsLogged = false;
                if(m_WebSocket != null)
                {
                    m_WebSocket.Close();
                }
                GfxSystem.PublishGfxEvent("ge_show_dialog", "ui", Dict.Get(5), Dict.Get(4), null, null, null);
            }
            else
            {
                //向服务器发送登录消息
                if(m_LoginMode == LoginMode.DirectLogin)
                {
                    JsonData loginMsg = new JsonData();
                    loginMsg.Set("m_Account", m_Account);
                    loginMsg.Set("m_LoginServerId", m_LogicServerId);
                    SendMessage(JsonMessageID.DirectLogin, loginMsg);
                }
                else if (m_LoginMode == LoginMode.AccountLogin)
                {
                    JsonData loginMsg = new JsonData();
                    loginMsg["m_Account"] = m_Account;
                    loginMsg["m_OpCode"] = m_OpCode;
                    loginMsg["m_ChannelId"] = m_ChannelId;
                    loginMsg["m_Data"] = m_Data;
                    loginMsg["m_LoginServerId"] = m_LogicServerId;
                    string version_num = VersionConfigProvider.Instance.GetVersionNum();
                    loginMsg["m_ClientGameVersion"] = version_num;
                    loginMsg["m_ClientLoginIp"] = GetIp();
                    loginMsg["m_UniqueIdentifier"] = m_UniqueIdentifier;
                    SendMessage(JsonMessageID.AccountLogin, loginMsg);
                }
            }
        }
    }
}
