using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashFireSpatial;
using DashFire.Network;
using DashFireMessage;
using UnityEngine;

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
    public class WorldSystemProfiler
    {
        public long sceneTickTime;
        public long entityMgrTickTime;
        public long controlSystemTickTime;
        public long movementSystemTickTime;
        public long spatialSystemTickTime;
        public long aiSystemTickTime;
        public long sceneLogicSystemTickTime;
        public long storySystemTickTime;
        public long usersTickTime;
        public long npcsTickTime;
        public long combatSystemTickTime;
    }

    /**
     * @brief 游戏系统
     */
    public class WorldSystem
    {
        private DelayActionProcessor m_DelayActionProcessor = new DelayActionProcessor();

        private bool m_IsObserver = false;//观战
        private bool m_IsFollowObserver = false;
        private int m_FollowTargetId = 0;
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

        private long m_LastLogicTickTime = 0;

        private long m_LastTryChangeSceneTime = 0;
        private long m_LastNotifyMoveMeetObstacleTime = 0;
        private bool m_IsAlreadyNotifyMeetObstacle = false;
        private bool m_IsWaitMatch = false;

        private const long c_ChangeSceneTimeout = 60000;
        private const long c_IntervalPerSecond = 5000;
        private long m_LastTickTimeForTickPerSecond = 0;

        private UserInfo m_PlayerSelf = null;
        private int m_PlayerSelfId = -1;

        private long m_SceneStartTime = 0;

        private WorldSystemProfiler m_Profiler = new WorldSystemProfiler();

        private WorldSystem()
        {
            m_SceneContext.SpatialSystem = m_SpatialSystem;
            m_SceneContext.SceneLogicInfoManager = m_SceneLogicInfoMgr;
            m_SceneContext.NpcManager = m_NpcMgr;
            m_SceneContext.UserManager = m_UserMgr;
            m_SceneContext.BlackBoard = m_BlackBoard;

            m_NpcMgr.SetSceneContext(m_SceneContext);
            m_SceneLogicInfoMgr.SetSceneContext(m_SceneContext);

            m_AiSystem.SetNpcManager(m_NpcMgr);
            m_AiSystem.SetUserManager(m_UserMgr);
            m_SceneLogicSystem.SetSceneLogicInfoManager(m_SceneLogicInfoMgr);
        }

        //初始化
        public void Init()
        {
            m_IsObserver = false;
            m_CurScene = null;

            //未实现
            //GfxSystem.EventChannelForLogic.Subscribe("ge_change_hero", "game", ChangeHeroFromGfx);
            GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_player_movemode", "game", ChangePlayerMoveMode);//改变角色移动模式
            //GfxSystem.EventChannelForLogic.Subscribe<int, int>("ge_change_npc_movemode", "game", ChangeNpcMoveMode);
            GfxSystem.EventChannelForLogic.Subscribe<int>("ge_change_scene", "game", ChangeSceneFromGfx);//切换场景
            //GfxSystem.EventChannelForLogic.Subscribe("ge_resetdsl", "game", ResetDsl);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execscript", "game", ExecScript);
            //GfxSystem.EventChannelForLogic.Subscribe<string>("ge_execcommand", "game", ExecCommand);
        }

        public void OnRoomServerConnected()
        {
            GfxSystem.PublishGfxEvent("ge_ui_connect_hint", "ui", false, false);
        }

        public void QuitBattle()
        {
            OnRoomServerConnected();
            NetworkSystem.Instance.QuitBattle();
        }

        private void ChangePlayerMoveMode(int mode)
        {
            UserInfo player = WorldSystem.Instance.GetPlayerSelf();
            if(null != player)
            {
                if((int)MovementMode.HighSpeed == mode)
                {
                    player.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, player.GetBaseProperty().RunSpeed);
                    player.GetMovementStateInfo().MovementMode = MovementMode.Normal;
                }
                else if ((int)MovementMode.LowSpeed == mode)
                {
                    player.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, player.GetBaseProperty().WalkSpeed);
                    player.GetMovementStateInfo().MovementMode = MovementMode.LowSpeed;
                }
            }
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

        public bool ChangeScene(int sceneId)
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

                if (null != m_CurScene.SceneConfig)
                {
                    //如果是服务器选择场景
                    if (IsServerSelectScene())
                    {
                        LobbyClient.Instance.CurrentRole = null;
                    }
                    Data_SceneConfig sceneConfig = SceneConfigProvider.Instance.GetSceneConfigById(m_CurScene.ResId);
                    m_SpatialSystem.Init(FilePathDefine_Client.C_RootPath + sceneConfig.m_BlockInfoFile, sceneConfig.m_ReachableSet);
                    m_SpatialSystem.LoadPatch(FilePathDefine_Client.C_RootPath + sceneConfig.m_BlockInfoFile + ".patch");
                    m_SpatialSystem.LoadObstacle(FilePathDefine_Client.C_RootPath + sceneConfig.m_ObstacleFile, 1 / sceneConfig.m_TiledDataScale);

                    LogSystem.Debug("init SpatialSystem:{0}", FilePathDefine_Client.C_RootPath + sceneConfig.m_BlockInfoFile);
                    LogSystem.Debug("GameSystem.ChangeNextScene:{0}", m_CurScene.ResId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public int GetCurSceneId()
        {
            if (m_CurScene != null)
            {
                return m_CurScene.ResId;
            }
            return 0;
        }
        public SceneResource GetCurScene()
        {
            return m_CurScene;
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

        public bool IsPureClientScene()
        {
            if (null == m_CurScene)
                return true;
            else
                return m_CurScene.IsPureClientScene;
        }

        /**
         * @brief 逻辑循环
         */
        public void Tick()
        {
            //逻辑限帧率10帧
            long curTime = TimeUtility.GetLocalMilliseconds();
            if(m_LastLogicTickTime + 40 <= curTime)
            {
                m_LastLogicTickTime = curTime;
            }
            else
            {
                return;
            }
            TimeSnapshot.Start();//时间快照
            TimeSnapshot.DoCheckPoint();
            if (m_CurScene == null)
            {
                return;
            }
            //处理延迟调用
            m_DelayActionProcessor.HandleActions(100);//每帧最大抛出100个消息
            //角色进场景逻辑
            //IsWaitRoomServerConnect是在切换场景时设置为true
            if(!m_CurScene.IsWaitSceneLoad && m_CurScene.IsWaitRoomServerConnect)
            {
                if (this.IsPureClientScene() || this.IsPveScene() || this.IsServerSelectScene() || NetworkSystem.Instance.CanSendMessage)
                {
                    GfxSystem.PublishGfxEvent("ge_enter_scene", "ui", m_CurScene.ResId);//加载当前场景UI

                    StorySystem.StoryConfigManager.Instance.Clear();
                    ClientStorySystem.Instance.ClearStoryInstancePool();//清除缓存剧情
                    //初始化10条剧情实例用于缓存剧情实例
                    for (int i = 1; i < 10; ++i)
                    {
                        ClientStorySystem.Instance.PreloadStoryInstance(1);
                    }
                    PlayerControl.Instance.Reset();

                    if (IsObserver)//观战
                    {
                        //TODO:未实现
                    }
                    else if (this.IsPureClientScene() || IsPveScene())//pve
                    {
                        //单机游戏逻辑启动
                        CreateSceneLogics();
                        if (IsExpeditionScene())
                        {
                            //TODO:未实现
                            //ExpeditionStartGame();
                        }
                        else
                        {
                            StartGame();
                        }
                        m_CurScene.NotifyUserEnter();
                        ClientStorySystem.Instance.StartStory(1);
                    }
                    else
                    {
                        //TODO:未实现
                        //下副本时玩家的角色ID与本地客户端的角色不一致，所以下副本前先删掉本地角色
                        //DestroyHero();
                        //CreateSceneLogics();

                        //if (IsPvpScene() || IsMultiPveScene())
                        //{
                        //    DashFireMessage.Msg_CRC_Create build = new DashFireMessage.Msg_CRC_Create();
                        //    NetworkSystem.Instance.SendMessage(build);
                        //    LogSystem.Debug("send Msg_CRC_Create to roomserver");
                        //}
                    }

                    if(IsPveScene() || IsPureClientScene())
                    {
                        SyncGfxUsersInfo();
                    }

                    m_CurScene.IsWaitRoomServerConnect = false;
                }
            }
            //超时没有进入场景
            if(!m_CurScene.IsSuccessEnter)
            {
                if(curTime > m_LastTryChangeSceneTime + c_ChangeSceneTimeout)
                {
                    m_LastTryChangeSceneTime = curTime;
                    PromptExceptionAndGotoMainCity();
                }
                return;
            }

            m_Profiler.sceneTickTime = TimeSnapshot.DoCheckPoint();

            EntityManager.Instance.Tick();
            m_Profiler.entityMgrTickTime = TimeSnapshot.DoCheckPoint();

            ControlSystemOperation.Tick();
            m_Profiler.controlSystemTickTime = TimeSnapshot.DoCheckPoint();

            m_Profiler.movementSystemTickTime = TimeSnapshot.DoCheckPoint();

            m_SpatialSystem.Tick();//碰撞类处理
            m_Profiler.spatialSystemTickTime = TimeSnapshot.DoCheckPoint();

            if (m_Profiler.spatialSystemTickTime > 50000)
            {
                LogSystem.Warn("*** SpatialSystem tick time is {0}", m_Profiler.spatialSystemTickTime);
                for (LinkedListNode<UserInfo> node = UserManager.Users.FirstValue; null != node; node = node.Next)
                {
                    UserInfo userInfo = node.Value;
                    if(null != userInfo)
                    {
                        LogSystem.Warn("===>User:{0} Pos:{1}", userInfo.GetId(), userInfo.GetMovementStateInfo().GetPosition3D().ToString());
                    }
                }
                for(LinkedListNode<NpcInfo> node = NpcManager.Npcs.FirstValue; null != node; node = node.Next)
                {
                    NpcInfo npcInfo = node.Value;
                    if(null != npcInfo)
                    {
                        LogSystem.Warn("===>Npc:{0} Pos:{1}", npcInfo.GetId(), npcInfo.GetMovementStateInfo().GetPosition3D().ToString());
                    }
                }
            }

            TickMoveMeetObstacle();

            //obj特殊逻辑处理
            TickUsers();
            m_Profiler.usersTickTime = TimeSnapshot.DoCheckPoint();

            TickNpcs();
            m_Profiler.npcsTickTime = TimeSnapshot.DoCheckPoint();

            try
            {
                TickSystemByCharacters();
            }
            catch (Exception e)
            {
                LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
            }

            m_Profiler.combatSystemTickTime = TimeSnapshot.DoCheckPoint();

            if(IsPureClientScene() || IsPveScene())
            {
                TickPve();
            }

            if (IsPveScene())
            {
                TickRecover();//hp mp ep 回复
            }

            //GmCommands.ClientGmStorySystem.Instance.Tick();//TODO:未实现

            m_SceneLogicSystem.Tick();
            m_Profiler.sceneLogicSystemTickTime = TimeSnapshot.DoCheckPoint();

            long tickTime = TimeSnapshot.End();
            if (tickTime > 30000)
            {
                //LogSystem.Debug("*** PerformanceWarning: {0}", m_Profiler.GenerateLogString(tickTime));
            }
        }

        private void TickPve()
        {
            m_AiSystem.Tick();
            m_Profiler.aiSystemTickTime = TimeSnapshot.DoCheckPoint();

            ClientStorySystem.Instance.Tick();
            m_Profiler.storySystemTickTime = TimeSnapshot.DoCheckPoint();
        }

        private void TickRecover()
        {
            float hp_coefficient = 1.0f;
            float mp_coefficient = 1.0f;

            if (null != m_CurScene && null != m_CurScene.SceneConfig)
            {
                Data_SceneConfig scene_data = m_CurScene.SceneConfig;
                hp_coefficient = scene_data.m_RecoverHpCoefficient;
                mp_coefficient = scene_data.m_RecoverMpCoefficient;
            }
            long curTime = TimeUtility.GetServerMilliseconds();
            if (curTime > m_LastTickTimeForTickPerSecond + c_IntervalPerSecond)
            {
                m_LastTickTimeForTickPerSecond = curTime;

                for (LinkedListNode<UserInfo> linkNode = m_UserMgr.Users.FirstValue; null != linkNode; linkNode = linkNode.Next)
                {
                    UserInfo info = linkNode.Value;
                    if (!info.IsDead())
                    {
                        float hpRecover = info.GetActualProperty().HpRecover * hp_coefficient;
                        float epRecover = info.GetActualProperty().EnergyRecover * mp_coefficient;

                        if (hpRecover > 0.0001)
                        {
                            if (info.Hp + (int)hpRecover >= info.GetActualProperty().HpMax)
                            {
                                info.SetHp(Operate_Type.OT_Absolute, (int)info.GetActualProperty().HpMax);
                            }
                            else
                            {
                                info.SetHp(Operate_Type.OT_Relative, (int)hpRecover);
                            }
                        }

                        if (epRecover > 0.0001)
                        {
                            if (info.Energy + (int)epRecover >= info.GetActualProperty().EnergyMax)
                            {
                                info.SetEnergy(Operate_Type.OT_Absolute, (int)info.GetActualProperty().EnergyMax);
                            }
                            else
                            {
                                info.SetEnergy(Operate_Type.OT_Relative, (int)epRecover);
                            } 
                        }
                    }
                }

                for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
                {
                    NpcInfo info = linkNode.Value;
                    if (!info.IsDead())
                    {
                        float hpRecover = info.GetActualProperty().HpRecover;
                        float npRecover = info.GetActualProperty().EnergyRecover;
                        if (hpRecover > 0.0001)
                        {
                            if (info.Hp + (int)hpRecover >= info.GetActualProperty().HpMax)
                                info.SetHp(Operate_Type.OT_Absolute, (int)info.GetActualProperty().HpMax);
                            else
                                info.SetHp(Operate_Type.OT_Relative, (int)hpRecover);
                        }
                        if (npRecover > 0.0001)
                        {
                            if (info.Energy + (int)npRecover >= info.GetActualProperty().EnergyMax)
                                info.SetEnergy(Operate_Type.OT_Absolute, (int)info.GetActualProperty().EnergyMax);
                            else
                                info.SetEnergy(Operate_Type.OT_Relative, (int)npRecover);
                        }
                    }
                }
            }
        }

        private void TickSystemByCharacters()
        {
            for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                CharacterInfo character = linkNode.Value;
                ImpactSystem.Instance.Tick(character);
            }

            for (LinkedListNode<UserInfo> linkNode = m_UserMgr.Users.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                CharacterInfo character = linkNode.Value;
                ImpactSystem.Instance.Tick(character);
            }
        }

        private void TickNpcs()
        {
            List<NpcInfo> deletes = new List<NpcInfo>();
            for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo info = linkNode.Value;
                if(info.SkillController != null)
                {
                    info.SkillController.OnTick();
                }
                if(info.LevelChanged || info.GetSkillStateInfo().BuffChanged || info.GetEquipmentStateInfo().EquipmentChanged || info.GetLegacyStateInfo().LegacyChanged)
                {
                    NpcAttrCalculator.Calc(info);
                    info.LevelChanged = false;
                    info.GetSkillStateInfo().BuffChanged = false;
                    info.GetEquipmentStateInfo().EquipmentChanged = false;
                    info.GetLegacyStateInfo().LegacyChanged = false;
                }
                if(info.Hp <= 0 && info.EmptyBloodTime <= 0)
                {
                    CharacterView view = EntityManager.Instance.GetCharacterViewById(info.GetId());
                    if(null != view)
                    {
                        //TODO:未实现
                        GfxSystem.SendMessage(view.Actor, "OnEventEmptyBlood", null);
                    }
                    info.EmptyBloodTime = TimeUtility.GetServerMilliseconds();
                    OnNpcKilled(info);
                }
                if (info.Hp <= 0 && info.DeadTime <= 0)
                {
                    CharacterView view = EntityManager.Instance.GetCharacterViewById(info.GetId());
                    if (null != view)
                    {
                        if(view.ObjectInfo.IsDead || !info.GetSkillStateInfo().IsImpactControl())
                        {
                            info.DeadTime = TimeUtility.GetServerMilliseconds();
                            if (info.GetSkillStateInfo().IsSkillActivated())
                            {
                                info.SkillController.ForceInterruptCurSkill();
                            }
                            info.GetSkillStateInfo().RemoveAllImpact();

                            if(info.OwnerId == WorldSystem.Instance.PlayerSelfId)
                            {
                                NetworkSystem.Instance.SyncDeleteDeadNpc(info.GetId());
                            }
                        }
                        else
                        {
                            if(info.IsHaveGfxStateFlag(GfxCharacterState_Type.Stiffness))
                            {
                                SendDeadImpact(info);
                            }
                        }
                    }
                }
                if(info.IsBorning && IsNpcBornOver(info))
                {
                    info.IsBorning = false;
                    info.SetAIEnable(true);
                    info.SetStateFlag(Operate_Type.OT_RemoveBit, CharacterState_Type.CST_Invincible);
                }

                if(info.LifeEndTime > 0 && TimeUtility.GetServerMilliseconds() > info.LifeEndTime)
                {
                    OnNpcKilled(info);
                    DestroyNpc(info);
                }

                if (IsPureClientScene() || IsPveScene())
                {
                    // 约定npc的高度低于130时，直接判定npc死亡。
                    if(130.0f > info.GetMovementStateInfo().GetPosition3D().y)
                    {
                        info.NeedDelete = true;
                        OnNpcKilled(info);
                    }
                    if (info.NeedDelete)
                    {
                        deletes.Add(info);
                    }
                    else if (info.Hp <= 0 && info.DeadTime > 0 && TimeUtility.GetServerMilliseconds() - info.DeadAnimTime * 1000 - info.DeadTime > info.ReleaseTime)
                    {
                        deletes.Add(info);
                    }
                }
            }
            if (IsPureClientScene() || IsPveScene())
            {
                if (deletes.Count > 0)
                {
                    foreach (NpcInfo ni in deletes)
                    {
                        DestroyNpc(ni);
                        return;
                    }
                }
            }
            ExpeditionNpcDeadHandle();
        }

        private void DestroyNpc(NpcInfo ni)
        {
            CalcKillIncome(ni);
            ni.DeadTime = 0;
            if(ni.SkillController != null)
            {
                ni.SkillController.ForceInterruptCurSkill();
            }
            CharacterView view = EntityManager.Instance.GetCharacterViewById(ni.GetId());
            if (null != view)
            {
                //TODO:未实现
                GfxSystem.SendMessage(view.Actor, "OnEventDead", null);
            }
            EntityManager.Instance.DestroyNpcView(ni.GetId());
            WorldSystem.Instance.DestroyCharacterById(ni.GetId());
        }

        private void CalcKillIncome(NpcInfo npc)
        {
            if(this.IsPureClientScene() || IsPveScene())
            {
                if (npc.KillerId == WorldSystem.Instance.PlayerSelfId)
                {
                    UserInfo userKiller = WorldSystem.Instance.GetPlayerSelf();
                    Data_SceneDropOut dropOutInfo = m_CurScene.SceneDropOut;
                    if (null != dropOutInfo)
                    {
                        if (m_CurScene.GetDropMoney(npc.GetUnitId()) > 0)
                        {
                            DropNpc(DropOutType.GOLD, dropOutInfo.m_GoldModel, dropOutInfo.m_GoldParticle, m_CurScene.GetDropMoney(npc.GetUnitId()), npc);
                        }
                        if (m_CurScene.GetDropHp(npc.GetUnitId()) > 0)
                        {
                            DropNpc(DropOutType.HP, dropOutInfo.m_HpModel, dropOutInfo.m_HpParticle, m_CurScene.GetDropHp(npc.GetUnitId()), npc);
                        }
                        if (m_CurScene.GetDropMp(npc.GetUnitId()) > 0)
                        {
                            DropNpc(DropOutType.MP, dropOutInfo.m_MpModel, dropOutInfo.m_MpParticle, m_CurScene.GetDropMp(npc.GetUnitId()), npc);
                        }
                    }
                }
            }
        }

        private void DropNpc(DropOutType dropType, string model, string particle, int value, NpcInfo npc)
        {
            Data_Unit unit = new Data_Unit();
            unit.m_Id = -1;
            unit.m_LinkId = 100001;
            unit.m_AiLogic = (int)AiStateLogicId.DropOut_AutoPick;
            unit.m_RotAngle = 0;
            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if(null != userInfo)
            {
                Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();

                NpcInfo npcInfo = NpcManager.AddNpc(unit);
                npcInfo.GetMovementStateInfo().SetPosition(pos);
                npcInfo.GetMovementStateInfo().SetFaceDir(0);
                npcInfo.GetMovementStateInfo().IsMoving = false;
                npcInfo.SetAIEnable(true);
                npcInfo.SetCampId(userInfo.GetCampId());
                npcInfo.OwnerId = userInfo.GetId();

                DropOutInfo dropInfo = new DropOutInfo();
                dropInfo.DropType = dropType;
                dropInfo.Model = model;
                dropInfo.Particle = particle;
                dropInfo.Value = value;
                npcInfo.GetAiStateInfo().AiDatas.AddData<DropOutInfo>(dropInfo);
                npcInfo.SetModel(model);
                EntityManager.Instance.CreateNpcView(npcInfo.GetId());
            }
        }

        private void ExpeditionNpcDeadHandle()
        {
            if (IsExpeditionScene())
            {
                //TODO:未实现
            }
        }

        private bool IsNpcBornOver(NpcInfo npc)
        {
            if (npc == null)
            {
                return false;
            }
            long cur_time = TimeUtility.GetServerMilliseconds();
            long born_anim_time = npc.BornAnimTimeMs;
            if((npc.BornTime + born_anim_time) > cur_time)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SendDeadImpact(NpcInfo npc)
        {
            if(npc.IsCombatNpc())
            {
                CharacterView view = EntityManager.Instance.GetCharacterViewById(npc.GetId());
                if(null != view)
                {
                    //TODO:未实现
                    GfxSystem.QueueGfxAction(GfxModule.Impact.GfxImpactSystem.Instance.SendDeadImpact, view.Actor);
                }
            }
        }

        private void OnNpcKilled(NpcInfo info)
        {
            if(IsPveScene() || IsPureClientScene())
            {
                ClientStorySystem.Instance.SendMessage(string.Format("npckilled:{0}", info.GetUnitId()));
                TryFireAllNpcKilled(info.GetId());
            }
        }

        private void TryFireAllNpcKilled(int deadNpcId)
        {
            int ct = GetBattleNpcCount();
            LogSystem.Debug("npc {0} killed, left {1}", deadNpcId, ct);
            if(0 == ct)
            {
                //TODO:未实现
                ClientStorySystem.Instance.SendMessage("allnpckilled");
            }
        }

        private void TickUsers()
        {
            for (LinkedListNode<UserInfo> linkNode = m_UserMgr.Users.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                UserInfo info = linkNode.Value;
                if(info.SkillController != null)
                {
                    info.SkillController.OnTick();
                }
                if(info.LevelChanged || info.GetSkillStateInfo().BuffChanged || info.GetEquipmentStateInfo().EquipmentChanged || info.GetLegacyStateInfo().LegacyChanged)
                {
                    UserAttrCalculator.Calc(info);
                    info.LevelChanged = false;
                    info.GetSkillStateInfo().BuffChanged = false;
                    info.GetEquipmentStateInfo().EquipmentChanged = false;
                    info.GetLegacyStateInfo().LegacyChanged = false;
                }
                UserView view = EntityManager.Instance.GetUserViewById(info.GetId());
                if(null != view)
                {
                    if (IsPveScene() && !IsExpeditionScene() || IsMultiPveScene())
                    {
                        int battleNpcCount = GetBattleNpcCount();
                        if (battleNpcCount <= 0)
                        {
                            view.SetIndicatorInfo(true, 0, 0);
                        }
                        else if (battleNpcCount <= 3)
                        {
                            float minPowDist = 99999;
                            NpcInfo npc = m_NpcMgr.GetNearest(info.GetMovementStateInfo().GetPosition3D(), ref minPowDist);
                            if (null != npc && minPowDist > info.IndicatorDis * info.IndicatorDis)
                            {
                                float dir = Geometry.GetDirFromVector(npc.GetMovementStateInfo().GetPosition3D() - info.GetMovementStateInfo().GetPosition3D());
                                view.SetIndicatorInfo(true, dir, 1);
                            }
                            else
                            {
                                view.SetIndicatorInfo(false, 0.0f, 1);
                            }
                        }
                        else
                        {
                            view.SetIndicatorInfo(false, 0.0f, 1);
                        }
                    }
                }
            }
            // 连击
            if(null != m_PlayerSelf)
            {
                long curTime = TimeUtility.GetLocalMilliseconds();
                CombatStatisticInfo combatInfo = m_PlayerSelf.GetCombatStatisticInfo();
                if(combatInfo.LastHitTime + 1500 < curTime && combatInfo.MultiHitCount > 1)
                {
                    combatInfo.MultiHitCount = 1;
                    GfxSystem.PublishGfxEvent("ge_hitcount", "ui", 0);
                }
            }
            UserInfo player = WorldSystem.Instance.GetPlayerSelf();
            if (null != player && player.Hp <= 0)
            {
                if(player.DeadTime <= 0)
                {
                    player.GetCombatStatisticInfo().AddDeadCount(1);  //死亡计数+1
                    if(player.SkillController != null)
                    {
                        player.SkillController.ForceInterruptCurSkill();//强制中断当前技能
                    }

                    if (IsPveScene() || IsPureClientScene())
                    {
                        //TODO:未实现 参数
                        ClientStorySystem.Instance.SendMessage("userkilled", player.GetId());
                        ClientStorySystem.Instance.SendMessage("playerselfkilled", player.GetId());
                    }
                    player.DeadTime = TimeUtility.GetServerMilliseconds();
                    if (WorldSystem.Instance.IsPveScene() && !IsExpeditionScene() || WorldSystem.Instance.IsMultiPveScene())
                    {
                        //TODO:未实现 参数
                        GfxSystem.PublishGfxEvent("ge_role_dead", "ui");
                    }

                    // 禁止输入
                    PlayerControl.Instance.EnableMoveInput = false;
                    PlayerControl.Instance.EnableRotateInput = false;
                    PlayerControl.Instance.EnableSkillInput = false;
                }
                ExpeditionUserDeadHandle();
            }
            ExpeditionImageDeadHandle();
        }

        private void ExpeditionUserDeadHandle()
        {
            RoleInfo role_info = LobbyClient.Instance.CurrentRole;
            if (IsExpeditionScene() && null != role_info)
            {
                ExpeditionPlayerInfo expedition = role_info.GetExpeditionInfo();
                if (null != expedition && expedition.ActiveTollgate >= 0 && null != expedition.Tollgates && expedition.ActiveTollgate < expedition.Tollgates.Length)
                {
                    //TODO:未实现
                }
            }
        }

        private void ExpeditionImageDeadHandle()
        {
            if (IsExpeditionScene() && null != LobbyClient.Instance.CurrentRole)
            {
                ExpeditionPlayerInfo expedition = LobbyClient.Instance.CurrentRole.GetExpeditionInfo();
                if (null != expedition.Tollgates && expedition.ActiveTollgate >= 0 && expedition.ActiveTollgate < expedition.Tollgates.Length)
                {
                    //TODO:未实现
                }
            }
        }

        private int GetBattleNpcCount()
        {
            int ct = 0;
            for (LinkedListNode<NpcInfo> linkNode = m_NpcMgr.Npcs.FirstValue; null != linkNode; linkNode = linkNode.Next)
            {
                NpcInfo info = linkNode.Value;
                if(null != info && info.EmptyBloodTime <= 0 && (info.NpcType == (int)NpcTypeEnum.Normal || info.NpcType == (int)NpcTypeEnum.BigBoss || info.NpcType == (int)NpcTypeEnum.LittleBoss))
                {
                    ++ct;
                }
            }
            return ct;
        }

        private void TickMoveMeetObstacle()
        {
            UserInfo mySelf = GetPlayerSelf();
            if (null != mySelf && mySelf.GetMovementStateInfo().IsMoving && mySelf.GetMovementStateInfo().IsMoveMeetObstacle)
            {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastNotifyMoveMeetObstacleTime + 100 <= curTime)
                {
                    NotifyMoveMeetObstacle(true);
                    m_LastNotifyMoveMeetObstacleTime = TimeUtility.GetLocalMilliseconds();
                }
            }
        }

        //通知移动
        public void NotifyMoveMeetObstacle(bool force)
        {
            if(!IsAlreadyNotifyMeetObstacle || force)
            {
                if(null != m_PlayerSelf)
                {
                    MovementStateInfo msi = m_PlayerSelf.GetMovementStateInfo();

                    Msg_CRC_MoveMeetObstacle msg = new Msg_CRC_MoveMeetObstacle();
                    msg.cur_pos_x = msi.PositionX;
                    msg.cur_pos_z = msi.PositionZ;
                    msg.send_time = TimeUtility.GetServerMilliseconds();
                    NetworkSystem.Instance.SendMessage(msg);

                    IsAlreadyNotifyMeetObstacle = true;
                }
            }
        }

        public void PromptExceptionAndGotoMainCity()
        {
            if(WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
            {
                //TODO:未实现
            }
            if(WorldSystem.Instance.IsPveScene() || WorldSystem.Instance.IsMultiPveScene())
            {
                GfxSystem.PublishGfxEvent("ge_show_dialog", "ui", Dict.Get(11), Dict.Get(4), null, null, (MyAction<int>)((int btn) =>
                {
                    LobbyNetworkSystem.Instance.QuitRoom();
                    NetworkSystem.Instance.QuitBattle();
                    QueueAction((MyFunc<int, bool>)this.ChangeScene, LobbyClient.Instance.CurrentRole.CitySceneId);
                }), true);
            }
            else
            {
                if (!NetworkSystem.Instance.IsWaitStart)
                {
                    GfxSystem.PublishGfxEvent("ge_show_dialog", "ui", Dict.Get(10), Dict.Get(4), null, null, (MyAction<int>)((int btn) =>
                    {
                        LobbyNetworkSystem.Instance.QuitRoom();
                        NetworkSystem.Instance.QuitBattle();
                        QueueAction((MyFunc<int, bool>)this.ChangeScene, LobbyClient.Instance.CurrentRole.CitySceneId);
                    }), true);
                }
            }
        }

        public void StartGame()
        {
            m_SceneStartTime = TimeUtility.GetServerMilliseconds();
            UserInfo user = GetPlayerSelf();
            if(null != user)
            {
                //清除用户
                EntityManager.Instance.DestroyUserView(user.GetId());
                DestroyCharacterById(user.GetId());
            }
            user = CreatePlayerSelf(1, NetworkSystem.Instance.HeroId);//创建用户
            user.SetAIEnable(true);
            user.SetCampId(NetworkSystem.Instance.CampId);

            Data_Unit unit = m_CurScene.StaticData.ExtractData(DataMap_Type.DT_Unit, GlobalVariables.GetUnitIdByCampId(NetworkSystem.Instance.CampId)) as Data_Unit;
            if(null != unit)
            {
                user.GetMovementStateInfo().SetPosition(unit.m_Pos);
                user.GetMovementStateInfo().SetFaceDir(unit.m_RotAngle);
                user.SetHp(Operate_Type.OT_Absolute, user.GetActualProperty().HpMax);
                user.SetEnergy(Operate_Type.OT_Absolute, user.GetActualProperty().EnergyMax);
            }
            EntityManager.Instance.CreatePlayerSelfView(1);
            UserView view = EntityManager.Instance.GetUserViewById(1);
            if (null != view)
            {
                view.Visible = true;
            }

            if (null != LobbyClient.Instance.CurrentRole)
            {
                RoleInfo role_info = LobbyClient.Instance.CurrentRole;
                if(role_info.Nickname.Length > 0)
                {
                    user.SetNickName(role_info.Nickname);
                }

                //level
                if(role_info.Level > 0)
                {
                    user.SetLevel(role_info.Level);
                }

                // equips
                if(null != role_info.Equips)
                {
                    int equipLen = role_info.Equips.Length;
                    for (int i = 0; i < equipLen; i++)
                    {
                        if(null != role_info.Equips[i])
                        {
                            int item_id = role_info.Equips[i].ItemId;
                            if (item_id > 0)
                            {
                                ItemDataInfo info = new ItemDataInfo();
                                info.Level = role_info.Equips[i].Level;
                                info.ItemNum = role_info.Equips[i].ItemNum;
                                info.RandomProperty = role_info.Equips[i].RandomProperty;
                                info.ItemConfig = ItemConfigProvider.Instance.GetDataById(item_id);
                                if (null != info.ItemConfig)
                                {
                                    user.GetEquipmentStateInfo().SetEquipmentData(i, info);
                                }
                            }
                        }
                    }
                }

                // skills
                RefixSkills(user);

                // legacys
                if(null != role_info.Legacys)
                {
                    for(int i = 0; i < role_info.Legacys.Length; i++)
                    {
                        if(null != role_info.Legacys[i] && role_info.Legacys[i].IsUnlock)
                        {
                            user.GetLegacyStateInfo().ResetLegacyData(i);
                            int itemId = role_info.Legacys[i].ItemId;
                            if(itemId > 0)
                            {
                                ItemDataInfo info = new ItemDataInfo();
                                info.Level = role_info.Legacys[i].Level;
                                info.ItemNum = role_info.Legacys[i].ItemNum;
                                info.RandomProperty = role_info.Legacys[i].RandomProperty;
                                info.ItemConfig = ItemConfigProvider.Instance.GetDataById(itemId);
                                if(null != info.ItemConfig)
                                {
                                    user.GetLegacyStateInfo().SetLegacyData(i, info);
                                }
                            }
                        }
                    }
                }

                UserAttrCalculator.Calc(user);
                user.SetHp(Operate_Type.OT_Absolute, user.GetActualProperty().HpMax);
                user.SetEnergy(Operate_Type.OT_Absolute, user.GetActualProperty().EnergyMax);
            }

            // create npc
            foreach(Data_Unit npcUnit in m_CurScene.StaticData.m_UnitMgr.GetData().Values)
            {
                if(npcUnit.m_IsEnable)
                {
                    NpcInfo npc = m_NpcMgr.GetNpcInfoByUnitId(npcUnit.GetId());
                    if(null != npc)
                    {
                        npc = m_NpcMgr.AddNpc(npcUnit);
                        if (null != npc)
                        {
                            npc.SetAIEnable(true);
                            npc.SkillController = new SwordManSkillController(npc, GfxModule.Skill.GfxSkillSystem.Instance);
                            EntityManager.Instance.CreateNpcView(npc.GetId());
                        }
                    }
                }
            }

            // scene tips
            if (IsELiteScene())//ELite精英
            {
                RoleInfo curRole = LobbyClient.Instance.CurrentRole;
                int stars = curRole.GetSceneInfo(m_CurScene.ResId);
                if (stars == 1)
                {
                    GfxSystem.PublishGfxEvent("ge_pve_fightinfo", "ui", 2, m_CurScene.SceneConfig.m_CompletedTime, 0, 0);
                }
                else if (stars == 2)
                {
                    GfxSystem.PublishGfxEvent("ge_pve_fightinfo", "ui", 0, 0, m_CurScene.SceneConfig.m_CompletedHitCount, 0);
                }
            }
        }

        public void RefixSkills(UserInfo user)
        {
            if (null != user)
            {
                if(null != LobbyClient.Instance.CurrentRole && null != LobbyClient.Instance.CurrentRole.SkillInfos)
                {
                    List<SkillInfo> skill_info_list = LobbyClient.Instance.CurrentRole.SkillInfos;
                    SkillInfo[] skill_assit = new SkillInfo[]{new SkillInfo(0), new SkillInfo(0), new SkillInfo(0), new SkillInfo(0)};//初始化方式
                    int cur_preset_index = 0;//预先布置
                    if(cur_preset_index >= 0)
                    {
                        for(int i = 0; i < skill_assit.Length; i++)
                        {
                            for(int j = 0; j < skill_info_list.Count; j++)
                            {
                                if(skill_info_list[j].Postions.Presets[cur_preset_index] == (SlotPosition)(i+1))
                                {
                                    skill_assit[i].SkillId = skill_info_list[j].SkillId;
                                    skill_assit[i].SkillLevel = skill_info_list[j].SkillLevel;
                                    break;
                                }
                            }
                        }
                    }

                    user.GetSkillStateInfo().RemoveAllSkill();
                    for(int i = 0; i < skill_assit.Length; i++)
                    {
                        if(skill_assit[i].SkillId > 0)
                        {
                            SkillInfo info = new SkillInfo(skill_assit[i].SkillId);
                            info.SkillLevel = skill_assit[i].SkillLevel;
                            info.Postions.SetCurSkillSlotPos(0, (SlotPosition)(i + 1));//当前技能挂点位置
                            SkillCategory cur_skill_pos = SkillCategory.kNone;
                            if ((i + 1) == (int)SlotPosition.SP_A)
                            {
                                cur_skill_pos = SkillCategory.kSkillA;
                            }
                            else if ((i + 1) == (int)SlotPosition.SP_B)
                            {
                                cur_skill_pos = SkillCategory.kSkillB;
                            }
                            else if ((i + 1) == (int)SlotPosition.SP_C)
                            {
                                cur_skill_pos = SkillCategory.kSkillC;
                            }
                            else if ((i + 1) == (int)SlotPosition.SP_D)
                            {
                                cur_skill_pos = SkillCategory.kSkillD;
                            }
                            info.ConfigData.Category = cur_skill_pos;
                            user.GetSkillStateInfo().AddSkill(info);

                            AddSubSkill(user, info.SkillId, cur_skill_pos, info.SkillLevel);
                        }
                    }
                    Data_PlayerConfig playerData = PlayerConfigProvider.Instance.GetPlayerConfigById(user.GetLinkId());
                    if(null != playerData && null != playerData.m_FixedSkillList && playerData.m_FixedSkillList.Count > 0)
                    {
                        foreach(int skill_id in playerData.m_FixedSkillList)
                        {
                            if(null == user.GetSkillStateInfo().GetSkillInfoById(skill_id))
                            {
                                SkillInfo info = new SkillInfo(skill_id, 1);
                                user.GetSkillStateInfo().AddSkill(info);
                            }
                        }
                    }
                    user.ResetSkill();
                }
            }
        }

        public void SyncGfxUsersInfo()
        {
            List<GfxUserInfo> gfxUsers = new List<GfxUserInfo>();
            LinkedListDictionary<int, UserInfo> users = UserManager.Users;
            for(LinkedListNode<UserInfo> node = users.FirstValue; null != node; node = node.Next)
            {
                UserInfo user = node.Value;
                if (null != user)
                {
                    UserView view = EntityManager.Instance.GetUserViewById(user.GetId());
                    if (null != view)
                    {
                        GfxUserInfo gfxUser = new GfxUserInfo();
                        gfxUser.m_ActorId = view.Actor;
                        gfxUser.m_HeroId = user.GetLinkId();
                        gfxUser.m_Level = user.GetLevel();
                        gfxUser.m_Nick = user.GetNickName();
                        gfxUsers.Add(gfxUser);
                    }
                }
            }
            GfxSystem.PublishGfxEvent("ge_show_name_plates", "ui", gfxUsers);
        }

        public void SyncGfxUserInfo(int objId)
        {
            UserInfo user = UserManager.GetUserInfo(objId);
            if (null != user)
            {
                UserView view = EntityManager.Instance.GetUserViewById(user.GetId());
                if (null != view)
                {
                    GfxUserInfo gfxUser = new GfxUserInfo();
                    gfxUser.m_ActorId = view.Actor;
                    gfxUser.m_HeroId = user.GetLinkId();
                    gfxUser.m_Level = user.GetLevel();
                    gfxUser.m_Nick = user.GetNickName();

                    GfxSystem.PublishGfxEvent("ge_show_name_plate", "ui", gfxUser);
                }
            }
        }

        public void SyncGfxNpcInfo(int objId)
        {
            NpcInfo npc = NpcManager.GetNpcInfo(objId);
            if (null != npc)
            {
                NpcView view = EntityManager.Instance.GetNpcViewById(npc.GetId());
                if (null != view)
                {
                    GfxUserInfo gfxUser = new GfxUserInfo();
                    gfxUser.m_ActorId = view.Actor;
                    gfxUser.m_HeroId = npc.GetLinkId();
                    gfxUser.m_Level = npc.GetLevel();
                    gfxUser.m_Nick = "";

                    GfxSystem.PublishGfxEvent("ge_show_npc_name_plate", "ui", gfxUser);
                }
            }
        }

        public bool AddSubSkill(UserInfo user, int skill_id, SkillCategory pos, int level)
        {
            if(null == user)
            {
                return false;
            }
            SkillLogicData skill_data = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skill_id) as SkillLogicData;
            if(null != skill_data && skill_data.NextSkillId > 0)
            {
                SkillInfo info = new SkillInfo(skill_data.NextSkillId);
                info.SkillLevel = level;
                info.ConfigData.Category = pos;
                user.GetSkillStateInfo().AddSkill(info);
                AddSubSkill(user, info.SkillId, pos, level);
            }
            return true;
        }

        public UserInfo CreatePlayerSelf(int id, int resId)
        {
            m_PlayerSelf = CreateUser(id, resId);
            m_PlayerSelfId = id;
            return m_PlayerSelf;
        }

        public UserInfo CreateUser(int id, int resId)
        {
            UserInfo info = m_UserMgr.AddUser(id, resId);
            if (null != info)
            {
                info.SceneContext = m_SceneContext;
                info.SkillController = new SwordManSkillController(info, GfxModule.Skill.GfxSkillSystem.Instance);//技能
                info.SkillController.Init();
                if (null != m_SpatialSystem)
                {
                    m_SpatialSystem.AddObj(info.SpaceObject);
                }
            }
            return info;
        }

        public void DestroyCharacterById(int id)
        {
            if(m_NpcMgr.Npcs.Contains(id))
            {
                m_NpcMgr.RemoveNpc(id);
            }

            if(m_PlayerSelfId == id)
            {
                m_PlayerSelf = null;
            }
            if(m_UserMgr.Users.Contains(id))
            {
                if(null != m_SpatialSystem)
                {
                    CharacterInfo info = m_UserMgr.Users[id];
                    if(null != info)
                    {
                        info.SceneContext = null;
                        m_SpatialSystem.RemoveObj(info.SpaceObject);
                    }
                }
                m_UserMgr.RemoveUser(id);
            }
        }

        private void CreateSceneLogics()
        {
            MyDictionary<int, object> slogics = m_CurScene.StaticData.m_SceneLogicMgr.GetData();//当前场景的地图逻辑数据
            foreach(SceneLogicConfig sc in slogics.Values)
            {
                if(null != sc)
                {
                    if(IsPureClientScene() || IsPveScene())
                    {
                        m_SceneLogicInfoMgr.AddSceneLogicInfo(sc.GetId(), sc);
                    }
                }
            }
        }

        public NpcInfo CreateNpc(int id, int unitId)
        {
            NpcInfo ret = null;
            Data_Unit mapUnit = GetCurScene().StaticData.ExtractData(DataMap_Type.DT_Unit, unitId) as Data_Unit;
            if (null != mapUnit)
            {
                ret = m_NpcMgr.AddNpc(id, mapUnit);
                if (null != ret)
                {
                    ret.SkillController = new SwordManSkillController(ret, GfxModule.Skill.GfxSkillSystem.Instance);
                    ret.SkillController.Init();
                }
            }
            return ret;
        }

        public NpcInfo CreateNpcByLinkId(int id, int linkId)
        {
            Data_Unit mapUnit = new Data_Unit();
            mapUnit.m_Id = -1;
            mapUnit.m_LinkId = linkId;
            NpcInfo npc = m_NpcMgr.AddNpc(id, mapUnit);
            if (null != npc)
            {
                npc.SkillController = new SwordManSkillController(npc, GfxModule.Skill.GfxSkillSystem.Instance);
                npc.SkillController.Init();
            }
            return npc;
        }

        public void CreateNpcEntity(int unitId)
        {
            Data_Unit mapUnit = GetCurScene().StaticData.ExtractData(DataMap_Type.DT_Unit, unitId) as Data_Unit;
            if(null != mapUnit)
            {
                NpcInfo npc = m_NpcMgr.AddNpc(mapUnit);
                if(null != npc)
                {
                    //未实现
                    npc.SkillController = new SwordManSkillController(npc, GfxModule.Skill.GfxSkillSystem.Instance);
                    npc.SkillController.Init();
                    EntityManager.Instance.CreateNpcView(npc.GetId());
                    CustomNpcByUnitId(npc, unitId);
                }
            }
        }

        public void CreateNpcEntity(int unitId, float rnd)
        {
            Data_Unit mapUnit = GetCurScene().StaticData.ExtractData(DataMap_Type.DT_Unit, unitId) as Data_Unit;
            if (null != mapUnit)
            {
                Vector3 pos;
                if (Geometry.IsSamePoint(mapUnit.m_Pos2, Vector3.zero))
                {
                    pos = mapUnit.m_Pos;
                }
                else
                {
                    pos = mapUnit.m_Pos * (1.0f - rnd) + mapUnit.m_Pos2 * rnd;
                }
                NpcInfo npc = m_NpcMgr.AddNpc(mapUnit);
                if (null != npc)
                {
                    npc.GetMovementStateInfo().SetPosition(pos);
                    npc.SkillController = new SwordManSkillController(npc, GfxModule.Skill.GfxSkillSystem.Instance);
                    npc.SkillController.Init();
                    EntityManager.Instance.CreateNpcView(npc.GetId());
                    CustomNpcByUnitId(npc, unitId);
                }
            }
        }

        public void CreateNpcEntityWithPos(int unitId, float x, float y, float z, float rotateAngle)
        {
            Data_Unit mapUnit = GetCurScene().StaticData.ExtractData(DataMap_Type.DT_Unit, unitId) as Data_Unit;
            if (null != mapUnit)
            {
                NpcInfo npc = m_NpcMgr.AddNpc(mapUnit);
                if (null != npc)
                {
                    npc.GetMovementStateInfo().SetPosition(x, y, z);
                    npc.GetMovementStateInfo().SetFaceDir(rotateAngle);
                    npc.SkillController = new SwordManSkillController(npc, GfxModule.Skill.GfxSkillSystem.Instance);
                    npc.SkillController.Init();
                    EntityManager.Instance.CreateNpcView(npc.GetId());
                }
            }
        }

        public CharacterInfo GetCharacterByUnitId(int unitId)
        {
            CharacterInfo obj = null;
            if (null != m_NpcMgr)
                obj = m_NpcMgr.GetNpcInfoByUnitId(unitId);
            return obj;
        }

        private void CustomNpcByUnitId(NpcInfo npc, int unitId)
        {
            Data_Unit mapUnit = GetCurScene().StaticData.ExtractData(DataMap_Type.DT_Unit, unitId) as Data_Unit;
            if(null != mapUnit)
            {
                NpcView view = EntityManager.Instance.GetCharacterViewById(npc.GetId()) as NpcView;
                if (null != view)
                {
                    view.SetIdleAnim(mapUnit.m_IdleAnims);
                }
            }
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

        public void SetBlockedShader(uint rimColor1, float rimPower1, float rimCutValue1, uint rimColor2, float rimPower2, float rimCutValue2)
        {
            UserInfo myself = GetPlayerSelf();
            if(null != myself)
            {
                UserView myselfView = EntityManager.Instance.GetUserViewById(myself.GetId());
                if (null != myselfView)
                {
                    GfxSystem.SetBlockedShader(myselfView.Actor, rimColor1, rimPower1, rimCutValue1);
                }

                LinkedListDictionary<int, UserInfo> users = UserManager.Users;
                for (LinkedListNode<UserInfo> node = users.FirstValue; null != node; node = node.Next)
                {
                    UserInfo user = node.Value;
                    if (null != user && user != myself)
                    {
                        UserView view = EntityManager.Instance.GetUserViewById(user.GetId());
                        if (null != view)
                        {
                            if (CharacterInfo.GetRelation(myself, user) == CharacterRelation.RELATION_FRIEND)
                            {
                                GfxSystem.SetBlockedShader(view.Actor, rimColor1, rimPower1, rimCutValue1);

                            }
                            else
                            {
                                GfxSystem.SetBlockedShader(view.Actor, rimColor2, rimPower2, rimCutValue2);
                            }
                        }
                    }
                }

                LinkedListDictionary<int, NpcInfo> npcs = NpcManager.Npcs;
                for (LinkedListNode<NpcInfo> node = npcs.FirstValue; null != node; node = node.Next)
                {
                    NpcInfo npc = node.Value;
                    if (null != npc)
                    {
                        NpcView view = EntityManager.Instance.GetNpcViewById(npc.GetId());
                        if (null != view)
                        {
                            if (CharacterInfo.GetRelation(myself, npc) == CharacterRelation.RELATION_FRIEND)
                            {
                                GfxSystem.SetBlockedShader(view.Actor, rimColor1, rimPower1, rimCutValue1);

                            }
                            else
                            {
                                GfxSystem.SetBlockedShader(view.Actor, rimColor2, rimPower2, rimCutValue2);
                            }
                        }
                    }
                }
            }
        }

        public void QueueAction(MyAction action)
        {
            m_DelayActionProcessor.QueueAction(action);
        }

        public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            QueueActionWithDelegation(action, t1, t2, t3);
        }

        public void QueueAction<T1, R>(MyFunc<T1, R> action, T1 t1)
        {
            QueueActionWithDelegation(action, t1);
        }

        public void QueueActionWithDelegation(Delegate action, params object[] args)
        {
            if (null != m_DelayActionProcessor)
            {
                m_DelayActionProcessor.QueueActionWithDelegation(action, args);
            }
        }

        //单例
        #region Singleton
        private static WorldSystem s_Instance = new WorldSystem();
        public static WorldSystem Instance
        {
            get { return s_Instance; }
        }

        #endregion

        public UserInfo GetPlayerSelf()
        {
            return m_PlayerSelf;
        }

        public int PlayerSelfId
        {
            get { return m_PlayerSelfId; }
            set { m_PlayerSelfId = value; }
        }

        public int CampId
        {
            get { return NetworkSystem.Instance.CampId; }
        }

        public bool IsMultiPveScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsMultiPve;
        }

        public bool IsPveScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsPve;
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

        public bool IsELiteScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsELite;
        }

        public bool IsAlreadyNotifyMeetObstacle
        {
            get { return m_IsAlreadyNotifyMeetObstacle; }
            set { m_IsAlreadyNotifyMeetObstacle = value; }
        }

        public bool IsWaitMatch
        {
            get { return m_IsWaitMatch; }
            set { m_IsWaitMatch = value; }
        }

        //观战
        public bool IsObserver
        {
            get { return m_IsObserver; }
        }

        public bool IsFollowObserver
        {
            get { return m_IsFollowObserver; }
            set { m_IsFollowObserver = value; }
        }

        public int FollowTargetId
        {
            get { return m_FollowTargetId; }
            set { m_FollowTargetId = value; }
        }

        public bool IsExpeditionScene()
        {
            if (null == m_CurScene)
                return false;
            else
                return m_CurScene.IsExpedition;
        }

        public ISpatialSystem SpatialSystem
        {
            get
            {
                return m_SpatialSystem;
            }
        }

        public SceneLogicInfoManager SceneLogicInfoManager
        {
            get
            {
                return m_SceneLogicInfoMgr;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return m_UserMgr;
            }
        }

        public NpcManager NpcManager
        {
            get
            {
                return m_NpcMgr;
            }
        }

        public long SceneStartTime
        {
            get { return m_SceneStartTime; }
            set
            {
                m_SceneStartTime = value;
                m_SceneContext.StartTime = m_SceneStartTime;
            }
        }
    }
}
