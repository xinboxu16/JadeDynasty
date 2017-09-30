using StorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ClientStorySystem
    {
        private class StoryInstanceInfo
        {
            public int m_StoryId;
            public StoryInstance m_StoryInstance;
            public bool m_IsUsed;
        }

        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();
        private List<StoryInstanceInfo> m_StoryLogicInfos = new List<StoryInstanceInfo>();
        private Dictionary<int, List<StoryInstanceInfo>> m_StoryInstancePool = new Dictionary<int, List<StoryInstanceInfo>>();

        public void Init()
        {
            //注册剧情命令
            StoryCommandManager.Instance.RegisterCommandFactory("startstory", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("stopstory", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("firemessage", new StoryCommandFactoryHelper<Story.Commands.FireMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("missioncompleted", new StoryCommandFactoryHelper<Story.Commands.MissionCompletedCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("missionfailed", new StoryCommandFactoryHelper<Story.Commands.MissionFailedCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("updatecoefficient", new StoryCommandFactoryHelper<Story.Commands.UpdateCoefficientCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("restartareamonitor", new StoryCommandFactoryHelper<Story.Commands.RestartAreaMonitorCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("restarttimeout", new StoryCommandFactoryHelper<Story.Commands.RestartTimeoutCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("restartareadetect", new StoryCommandFactoryHelper<Story.Commands.RestartAreaDetectCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("createnpc", new StoryCommandFactoryHelper<Story.Commands.CreateNpcCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("destroynpc", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcface", new StoryCommandFactoryHelper<Story.Commands.NpcFaceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcmove", new StoryCommandFactoryHelper<Story.Commands.NpcMoveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcmovewithwaypoints", new StoryCommandFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcpatrol", new StoryCommandFactoryHelper<Story.Commands.NpcPatrolCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcstop", new StoryCommandFactoryHelper<Story.Commands.NpcStopCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcattack", new StoryCommandFactoryHelper<Story.Commands.NpcAttackCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcpursuit", new StoryCommandFactoryHelper<Story.Commands.NpcPursuitCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("enableai", new StoryCommandFactoryHelper<Story.Commands.EnableAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("setai", new StoryCommandFactoryHelper<Story.Commands.SetAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcaddimpact", new StoryCommandFactoryHelper<Story.Commands.NpcAddImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcremoveimpact", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npccastskill", new StoryCommandFactoryHelper<Story.Commands.NpcCastSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcstopskill", new StoryCommandFactoryHelper<Story.Commands.NpcStopSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcaddskill", new StoryCommandFactoryHelper<Story.Commands.NpcAddSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("npcremoveskill", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("setcamp", new StoryCommandFactoryHelper<Story.Commands.SetCampCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objface", new StoryCommandFactoryHelper<Story.Commands.ObjFaceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objmove", new StoryCommandFactoryHelper<Story.Commands.ObjMoveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objmovewithwaypoints", new StoryCommandFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objstop", new StoryCommandFactoryHelper<Story.Commands.ObjStopCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objanimation", new StoryCommandFactoryHelper<Story.Commands.ObjAnimationCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objpursuit", new StoryCommandFactoryHelper<Story.Commands.ObjPursuitCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objenableai", new StoryCommandFactoryHelper<Story.Commands.ObjEnableAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objsetai", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objaddimpact", new StoryCommandFactoryHelper<Story.Commands.ObjAddImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objremoveimpact", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objcastskill", new StoryCommandFactoryHelper<Story.Commands.ObjCastSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objstopskill", new StoryCommandFactoryHelper<Story.Commands.ObjStopSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objaddskill", new StoryCommandFactoryHelper<Story.Commands.ObjAddSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("objremoveskill", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveSkillCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("setblockedshader", new StoryCommandFactoryHelper<Story.Commands.SetBlockedShaderCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("playerselfface", new StoryCommandFactoryHelper<Story.Commands.PlayerselfFaceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("playerselfmove", new StoryCommandFactoryHelper<Story.Commands.PlayerselfMoveCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("playerselfmovewithwaypoints", new StoryCommandFactoryHelper<Story.Commands.PlayerselfMoveWithWaypointsCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("playerselfpursuit", new StoryCommandFactoryHelper<Story.Commands.PlayerselfPursuitCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("playerselfstop", new StoryCommandFactoryHelper<Story.Commands.PlayerselfStopCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("cameralookat", new StoryCommandFactoryHelper<Story.Commands.CameraLookatCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("camerafollow", new StoryCommandFactoryHelper<Story.Commands.CameraFollowCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("cameralookatimmediately", new StoryCommandFactoryHelper<Story.Commands.CameraLookatImmediatelyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("camerafollowimmediately", new StoryCommandFactoryHelper<Story.Commands.CameraFollowImmediatelyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("lockframe", new StoryCommandFactoryHelper<Story.Commands.LockFrameCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("camerayaw", new StoryCommandFactoryHelper<Story.Commands.CameraYawCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("cameraheight", new StoryCommandFactoryHelper<Story.Commands.CameraHeightCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("cameradistance", new StoryCommandFactoryHelper<Story.Commands.CameraDistanceCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("cameraenable", new StoryCommandFactoryHelper<Story.Commands.CameraEnableCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("enableinput", new StoryCommandFactoryHelper<Story.Commands.EnableInputCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("showui", new StoryCommandFactoryHelper<Story.Commands.ShowUiCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("showwall", new StoryCommandFactoryHelper<Story.Commands.ShowWallCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("showdlg", new StoryCommandFactoryHelper<Story.Commands.ShowDlgCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("startcountdown", new StoryCommandFactoryHelper<Story.Commands.StartCountDownCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("reconnectlobby", new StoryCommandFactoryHelper<Story.Commands.ReconnectLobbyCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("publishfilterevent", new StoryCommandFactoryHelper<Story.Commands.PublishFilterEventCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("publishlogicevent", new StoryCommandFactoryHelper<Story.Commands.PublishLogicEventCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("publishgfxevent", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("sendgfxmessage", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("sendgfxmessagewithtag", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
            StoryCommandManager.Instance.RegisterCommandFactory("sendgfxmessagebyid", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageByIdCommand>());

            //注册值与函数处理
            StoryValueManager.Instance.RegisterValueHandler("useridlist", new StoryValueFactoryHelper<Story.Values.UserIdListValue>());
            StoryValueManager.Instance.RegisterValueHandler("playerselfid", new StoryValueFactoryHelper<Story.Values.PlayerselfIdValue>());
            StoryValueManager.Instance.RegisterValueHandler("winuserid", new StoryValueFactoryHelper<Story.Values.WinUserIdValue>());
            StoryValueManager.Instance.RegisterValueHandler("lostuserid", new StoryValueFactoryHelper<Story.Values.LostUserIdValue>());
            StoryValueManager.Instance.RegisterValueHandler("npcidlist", new StoryValueFactoryHelper<Story.Values.NpcIdListValue>());
            StoryValueManager.Instance.RegisterValueHandler("unitid2objid", new StoryValueFactoryHelper<Story.Values.UnitId2ObjIdValue>());
            StoryValueManager.Instance.RegisterValueHandler("objid2unitid", new StoryValueFactoryHelper<Story.Values.ObjId2UnitIdValue>());
            StoryValueManager.Instance.RegisterValueHandler("getposition", new StoryValueFactoryHelper<Story.Values.GetPositionValue>());
            StoryValueManager.Instance.RegisterValueHandler("getpositionx", new StoryValueFactoryHelper<Story.Values.GetPositionXValue>());
            StoryValueManager.Instance.RegisterValueHandler("getpositiony", new StoryValueFactoryHelper<Story.Values.GetPositionYValue>());
            StoryValueManager.Instance.RegisterValueHandler("getpositionz", new StoryValueFactoryHelper<Story.Values.GetPositionZValue>());
            StoryValueManager.Instance.RegisterValueHandler("getcamp", new StoryValueFactoryHelper<Story.Values.GetCampValue>());
            StoryValueManager.Instance.RegisterValueHandler("isenemy", new StoryValueFactoryHelper<Story.Values.IsEnemyValue>());
            StoryValueManager.Instance.RegisterValueHandler("isfriend", new StoryValueFactoryHelper<Story.Values.IsFriendValue>());
            StoryValueManager.Instance.RegisterValueHandler("gethp", new StoryValueFactoryHelper<Story.Values.GetHpValue>());
            StoryValueManager.Instance.RegisterValueHandler("getenergy", new StoryValueFactoryHelper<Story.Values.GetEnergyValue>());
            StoryValueManager.Instance.RegisterValueHandler("getrage", new StoryValueFactoryHelper<Story.Values.GetRageValue>());
            StoryValueManager.Instance.RegisterValueHandler("getmaxhp", new StoryValueFactoryHelper<Story.Values.GetMaxHpValue>());
            StoryValueManager.Instance.RegisterValueHandler("getmaxenergy", new StoryValueFactoryHelper<Story.Values.GetMaxEnergyValue>());
            StoryValueManager.Instance.RegisterValueHandler("getmaxrage", new StoryValueFactoryHelper<Story.Values.GetMaxRageValue>());
            StoryValueManager.Instance.RegisterValueHandler("islobbyconnected", new StoryValueFactoryHelper<Story.Values.IsLobbyConnectedValue>());
            StoryValueManager.Instance.RegisterValueHandler("calcdir", new StoryValueFactoryHelper<Story.Values.CalcDirValue>());
        }

        private StoryInstanceInfo NewStoryInstance(int storyId)
        {
            StoryInstanceInfo instInfo = GetUnusedStoryInstanceInfoFromPool(storyId);//从pool获取未使用的实例if
            if(instInfo == null)
            {
                Data_SceneConfig cfg = SceneConfigProvider.Instance.GetSceneConfigById(WorldSystem.Instance.GetCurSceneId());
                if(null != cfg)
                {
                    int ct = cfg.m_StoryDslFile.Count;
                    string[] filePath = new string[ct];
                    for(int i = 0; i < ct; i++)
                    {
                        filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Client.C_RootPath+ cfg.m_StoryDslFile[i]);
                    }
                    StoryConfigManager.Instance.LoadStoryIfNotExist(storyId, 0, filePath);
                    StoryInstance inst = StoryConfigManager.Instance.NewStoryInstance(storyId, 0);

                    if(inst == null)
                    {
                        DashFire.LogSystem.Error("Can't load story config, story:{0} !", storyId);
                        return null;
                    }
                    StoryInstanceInfo res = new StoryInstanceInfo();
                    res.m_StoryId = storyId;
                    res.m_StoryInstance = inst;
                    res.m_IsUsed = true;

                    AddStoryInstanceInfoToPool(storyId, res);
                    return res;
                }
                else
                {
                    DashFire.LogSystem.Error("Can't find story config, story:{0} !", storyId);
                    return null;
                }
            }else
            {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }

        private void AddStoryInstanceInfoToPool(int storyId, StoryInstanceInfo info)
        {
            if (m_StoryInstancePool.ContainsKey(storyId))
            {
                List<StoryInstanceInfo> infos = m_StoryInstancePool[storyId];
                infos.Add(info);
            }
            else
            {
                List<StoryInstanceInfo> infos = new List<StoryInstanceInfo>();
                infos.Add(info);
                m_StoryInstancePool.Add(storyId, infos);
            }
        }

        private void RecycleStorylInstance(StoryInstanceInfo info)
        {
            info.m_StoryInstance.Reset();
            info.m_IsUsed = false;
        }

        public void Reset()
        {
            m_GlobalVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index)
            {
                StoryInstanceInfo info = m_StoryLogicInfos[index];
                if (null != info)
                {
                    RecycleStorylInstance(info);
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }

        public void PreloadStoryInstance(int storyId)
        {
            StoryInstanceInfo info = NewStoryInstance(storyId);
            if(null != info)
            {
                RecycleStorylInstance(info);
            }
        }

        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
        }

        private StoryInstanceInfo GetUnusedStoryInstanceInfoFromPool(int storyId)
        {
            StoryInstanceInfo info = null;
            if(m_StoryInstancePool.ContainsKey(storyId))
            {
                List<StoryInstanceInfo> infos = m_StoryInstancePool[storyId];
                int ct = infos.Count;
                for (int ix = 0; ix < ct; ++ix)
                {
                    if(!infos[ix].m_IsUsed)
                    {
                        info = infos[ix];
                        break;
                    }
                }
            }
            return info;
        }

        public void StartStory(int storyId)
        {
            StoryInstanceInfo inst = NewStoryInstance(storyId);
            if(null != inst)
            {
                m_StoryLogicInfos.Add(inst);
                inst.m_StoryInstance.Context = WorldSystem.Instance;
                inst.m_StoryInstance.GlobalVariables = m_GlobalVariables;
                inst.m_StoryInstance.Start();

                LogSystem.Info("StartStory {0}", storyId);
            }
        }

        public void StopStory(int storyId)
        {
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index)
            {
                StoryInstanceInfo info = m_StoryLogicInfos[index];
                if(info.m_StoryId == storyId)
                {
                    RecycleStorylInstance(info);
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }

        public void SendMessage(string msgId, params object[] args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix)
            {
                StoryInstanceInfo info = m_StoryLogicInfos[ix];
                info.m_StoryInstance.SendMessage(msgId, args);
            }
        }

        public void Tick()
        {
            long time = TimeUtility.GetLocalMilliseconds();
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix)
            {
                StoryInstanceInfo info = m_StoryLogicInfos[ix];
                info.m_StoryInstance.Tick(time);
                if (info.m_StoryInstance.IsTerminated)
                {
                    RecycleStorylInstance(info);
                    m_StoryLogicInfos.RemoveAt(ix);
                }
            }
        }

        private ClientStorySystem() { }

        #region Sington
        public static ClientStorySystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static ClientStorySystem s_Instance = new ClientStorySystem();
        #endregion
    }
}
