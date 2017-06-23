using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using System.Threading;
using System.Reflection;

using System.Runtime.InteropServices;
using DashFireMessage;

namespace DashFire.Network
{
    public class NetworkSystem
    {
        private string m_Ip;
        private int m_Port;
        private bool m_IsConnected = false;
        private bool m_IsWaitStart = true;

        private bool m_IsQuited = false;

        private NetClient m_NetClient;
        private bool m_CanSendMessage = false;

        private bool m_NetClientStarted = false;

        private int m_CampId = 0;
        private int m_HeroId = 0;
        private int m_SceneId = 0;

        #region
        private static NetworkSystem s_Instance = new NetworkSystem();
        public static NetworkSystem Instance
        {
            get { return s_Instance; }
        }
        #endregion

        public void QuitBattle()
        {
            m_IsWaitStart = true;
            if (m_IsConnected)
            {
                Msg_CR_Quit msg = new Msg_CR_Quit();
                SendMessage(msg);
            }
            WorldSystem.Instance.QueueAction(this.ShutdownNetClient);
        }

        public void SendMessage(object msg)
        {
            try
            {
                if (!m_IsConnected)
                {
                    return;
                }
                NetOutgoingMessage om = m_NetClient.CreateMessage();
                byte[] bt = Serialize.Encode(msg);
                om.Write(bt);
                NetSendResult result = m_NetClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
                if (result == NetSendResult.FailedNotConnected)
                {
                    m_IsConnected = false;
                    m_CanSendMessage = false;
                    LogSystem.Debug("SendMessage FailedNotConnected");
                }
                else if (result == NetSendResult.Dropped)
                {
                    LogSystem.Error("SendMessage {0} Dropped", msg.ToString());
                }
                m_NetClient.FlushSendQueue();
            }
            catch (Exception ex)
            {
                LogSystem.Error("NetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void ShutdownNetClient()
        {
            if (m_NetClient != null)
            {
                if (m_NetClientStarted)
                {
                    m_NetClient.Shutdown("bye");
                    m_NetClientStarted = false;
                }
            }
        }

        public int CampId
        {
            get { return m_CampId; }
            set { m_CampId = value; }
        }

        public int SceneId
        {
            get { return m_SceneId; }
            set { m_SceneId = value; }
        }

        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }

        public void QuitClient()
        {
            m_IsQuited = true;
        }

        /**
        public bool Init()
        {
            Serialize.Init();
            InitMessageHandler();

            m_NetClientStarted = false;
            m_IsWaitStart = true;
            m_IsQuited = false;
            m_IsConnected = false;
            m_CanSendMessage = false;

            m_Config = new NetPeerConfiguration("RoomServer");
            m_Config.AutoFlushSendQueue = false;
            m_Config.DisableMessageType(NetIncomingMessageType.DebugMessage);
            m_Config.DisableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            m_NetClient = new NetClient(m_Config);
            m_NetThread = new Thread(new ThreadStart(NetworkThread));
            m_NetThread.Start();
            return true;
        }

        public void Start(uint key, string ip, int port, int heroId, int campId, int sceneId)
        {
            StartNetClient();

            m_Key = key;
            m_Ip = ip;
            m_Port = port;
            m_HeroId = heroId;
            m_CampId = campId;
            m_SceneId = sceneId;

            m_IsWaitStart = false;
            m_IsConnected = false;
            m_CanSendMessage = false;

            LogSystem.Info("NetworkSystem.Start key {0} ip {1} port {2} hero {3} camp {4} scene {5}", key, ip, port, heroId, campId, sceneId);
        }

        public void Tick()
        {
            try
            {
                if (m_NetClient == null)
                    return;
                if (m_IsConnected && m_CanSendMessage)
                {
                    if (TimeUtility.GetLocalMilliseconds() - m_LastPingTime >= m_PingInterval)
                    {
                        InternalPing();
                    }
                }
                ProcessMsg();
            }
            catch (Exception e)
            {
                string err = "Exception:" + e.Message + "\n" + e.StackTrace + "\n";
                LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
            }
        }

        public bool IsWaitStart
        {
            get { return m_IsWaitStart; }
        }

        public bool IsQuited
        {
            get
            {
                return m_IsQuited;
            }
        }

        public void QuitBattle()
        {
            m_IsWaitStart = true;
            if (m_IsConnected)
            {
                Msg_CR_Quit msg = new Msg_CR_Quit();
                SendMessage(msg);
            }
            WorldSystem.Instance.QueueAction(this.ShutdownNetClient);
        }

        public void QuitClient()
        {
            m_IsQuited = true;
        }

        public void SendLoginMsg(object msg)
        {
            SendMessage(msg);
        }

        public void SendMessage(object msg)
        {
            try
            {
                if (!m_IsConnected)
                {
                    return;
                }
                NetOutgoingMessage om = m_NetClient.CreateMessage();
                byte[] bt = Serialize.Encode(msg);
                om.Write(bt);
                NetSendResult result = m_NetClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
                if (result == NetSendResult.FailedNotConnected)
                {
                    m_IsConnected = false;
                    m_CanSendMessage = false;
                    LogSystem.Debug("SendMessage FailedNotConnected");
                }
                else if (result == NetSendResult.Dropped)
                {
                    LogSystem.Error("SendMessage {0} Dropped", msg.ToString());
                }
                m_NetClient.FlushSendQueue();
            }
            catch (Exception ex)
            {
                LogSystem.Error("NetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void Release()
        {
            ShutdownNetClient();
        }

        public void OnPong(long time, long sendPingTime, long sendPongTime)
        {
            if (time < sendPingTime) return;
            ++m_PingPongNumber;

            long rtt = time - sendPingTime;
            if (TimeUtility.AverageRoundtripTime == 0)
                TimeUtility.AverageRoundtripTime = rtt;
            else
                TimeUtility.AverageRoundtripTime = (long)(TimeUtility.AverageRoundtripTime * 0.7f + rtt * 0.3f);

            //LogSystem.Debug("RoundtripTime:{0} AverageRoundtripTime:{1}", rtt, TimeUtility.AverageRoundtripTime);

            long diff = sendPongTime + rtt / 2 - time;
            TimeUtility.RemoteTimeOffset = (TimeUtility.RemoteTimeOffset * (m_PingPongNumber - 1) + diff) / m_PingPongNumber;
        }

        public void SyncFaceDirection(float face_direction)
        {
            if (Math.Abs(m_LastFaceDir - face_direction) <= 0.01)
            {
                return;
            }
            m_LastFaceDir = face_direction;
            Msg_CRC_Face bd = new Msg_CRC_Face();
            bd.face_direction = face_direction;
            SendMessage(bd);
        }

        public void SyncPlayerMoveStart(float dir)
        {
            WorldSystem.Instance.IsAlreadyNotifyMeetObstacle = false;

            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if (null != userInfo)
            {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();
                DashFireMessage.Msg_CRC_MoveStart builder = new DashFireMessage.Msg_CRC_MoveStart();
                builder.send_time = TimeUtility.GetServerMilliseconds();
                builder.dir = dir;
                Position pos = new Position();
                pos.x = msi.PositionX;
                pos.z = msi.PositionZ;
                builder.position = pos;
                builder.is_skill_moving = msi.IsSkillMoving;
                SendMessage(builder);
            }
        }

        public void SyncPlayerMoveStop()
        {
            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if (null != userInfo)
            {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();
                DashFireMessage.Msg_CRC_MoveStop builder = new DashFireMessage.Msg_CRC_MoveStop();
                builder.send_time = TimeUtility.GetServerMilliseconds();
                Position pos = new Position();
                pos.x = msi.PositionX;
                pos.z = msi.PositionZ;
                builder.position = pos;
                SendMessage(builder);
            }
        }

        public void SyncPlayerSkill(CharacterInfo entity,
            int skillId)
        {
            if (entity.IsHaveStateFlag(CharacterState_Type.CST_Sleep))
            {
                return;
            }
            ScriptRuntime.Vector3 standPos = entity.GetMovementStateInfo().GetPosition3D();
            Msg_CRC_Skill bd = new Msg_CRC_Skill();
            bd.skill_id = skillId;
            bd.stand_pos = new DashFireMessage.Position();
            bd.stand_pos.x = standPos.X;
            bd.stand_pos.z = standPos.Z;
            bd.face_direction = entity.GetMovementStateInfo().GetFaceDir();
            bd.send_time = TimeUtility.GetServerMilliseconds();
            SendMessage(bd);

            //LogSystem.Debug("SyncPlayerSkill skill {0}, entity {1}, pos:{2}", skillId, entity.GetId(), entity.GetMovementStateInfo().GetPosition2D().ToString());
        }

        public void SyncPlayerStopSkill(CharacterInfo entity, int skillId)
        {
            if (entity.IsHaveStateFlag(CharacterState_Type.CST_Sleep))
            {
                return;
            }
            //LogSystem.Debug("SyncStopSkill {0}, entity {1}", skillId, entity.GetId());
            int id = entity.GetId();
            MovementStateInfo msi = entity.GetMovementStateInfo();

            if (id == WorldSystem.Instance.PlayerSelfId)
            {
                DashFireMessage.Msg_CRC_StopSkill msg = new DashFireMessage.Msg_CRC_StopSkill();
                msg.skill_id = skillId;
                SendMessage(msg);
            }
            else if (entity.OwnerId == WorldSystem.Instance.PlayerSelfId)
            {
                DashFireMessage.Msg_CRC_NpcStopSkill msg = new DashFireMessage.Msg_CRC_NpcStopSkill();
                msg.npc_id = entity.GetId();
                msg.skill_id = skillId;
                SendMessage(msg);
            }
        }

        public void SyncSendImpact(CharacterInfo sender,
          int impactId,
          int targetId,
          int skillId,
          int duration,
          ScriptRuntime.Vector3 senderPos,
          float senderDir)
        {
            Msg_CRC_SendImpactToEntity bd = new Msg_CRC_SendImpactToEntity();
            bd.sender_id = sender.GetId();
            bd.impact_id = impactId;
            bd.target_id = targetId;
            bd.skill_id = skillId;
            bd.duration = duration;
            bd.sender_pos = new Position3D();
            bd.sender_pos.x = senderPos.X;
            bd.sender_pos.y = senderPos.Y;
            bd.sender_pos.z = senderPos.Z;
            bd.sender_dir = senderDir;
            SendMessage(bd);
        }

        public void SyncStopGfxImpact(CharacterInfo sender,
          int targetId,
          int impactId)
        {
            Msg_CRC_StopGfxImpact bd = new Msg_CRC_StopGfxImpact();
            bd.impact_Id = impactId;
            bd.target_Id = targetId;
            SendMessage(bd);
        }

        public void SyncGfxMoveControlStart(CharacterInfo obj, int id, bool isSkill)
        {
            MovementStateInfo msi = obj.GetMovementStateInfo();
            Msg_CRC_GfxControlMoveStart msg = new Msg_CRC_GfxControlMoveStart();
            msg.obj_id = obj.GetId();
            msg.skill_or_impact_id = id;
            msg.is_skill = isSkill;
            msg.cur_pos = new DashFireMessage.Position();
            msg.cur_pos.x = msi.PositionX;
            msg.cur_pos.z = msi.PositionZ;
            msg.send_time = TimeUtility.GetServerMilliseconds();

            SendMessage(msg);
        }

        public void SyncGfxMoveControlStop(CharacterInfo obj, int id, bool isSkill)
        {
            MovementStateInfo msi = obj.GetMovementStateInfo();

            Msg_CRC_GfxControlMoveStop msg = new Msg_CRC_GfxControlMoveStop();
            msg.obj_id = obj.GetId();
            msg.skill_or_impact_id = id;
            msg.is_skill = isSkill;
            msg.target_pos = new DashFireMessage.Position();
            msg.target_pos.x = msi.PositionX;
            msg.target_pos.z = msi.PositionZ;
            msg.face_dir = msi.GetFaceDir();
            msg.send_time = TimeUtility.GetServerMilliseconds();

            SendMessage(msg);
        }

        public void SyncPlayerMoveToPos(ScriptRuntime.Vector3 target_pos)
        {
            WorldSystem.Instance.IsAlreadyNotifyMeetObstacle = false;

            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if (null != userInfo)
            {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();
                Msg_CR_UserMoveToPos builder = new Msg_CR_UserMoveToPos();
                builder.target_pos_x = target_pos.X;
                builder.target_pos_z = target_pos.Z;
                builder.cur_pos_x = msi.PositionX;
                builder.cur_pos_z = msi.PositionZ;
                SendMessage(builder);
            }
        }

        public void SyncPlayerMoveToAttack(int targetId, float attackRange)
        {
            WorldSystem.Instance.IsAlreadyNotifyMeetObstacle = false;

            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if (null != userInfo)
            {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();
                Msg_CR_UserMoveToAttack builder = new Msg_CR_UserMoveToAttack();
                builder.target_id = targetId;
                builder.attack_range = attackRange;
                builder.cur_pos_x = msi.PositionX;
                builder.cur_pos_z = msi.PositionZ;
                SendMessage(builder);
            }
        }

        public void SyncGiveUpCombat()
        {
            Msg_CR_GiveUpBattle msg = new Msg_CR_GiveUpBattle();
            SendMessage(msg);
        }

        public void SyncDeleteDeadNpc(int npcId)
        {
            Msg_CR_DeleteDeadNpc msg = new Msg_CR_DeleteDeadNpc();
            msg.npc_id = npcId;
            SendMessage(msg);
        }

        private void RegisterMsgHandler(Type msgtype, MessageDispatch.MsgHandler handler)
        {
            m_Dispatch.RegisterHandler(msgtype, handler);
        }
        private void InitMessageHandler()
        {
            RegisterMsgHandler(typeof(Msg_Pong), new MessageDispatch.MsgHandler(MsgPongHandler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ShakeHands_Ret), new MessageDispatch.MsgHandler(MsgShakeHandsRetHandler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_Create), new MessageDispatch.MsgHandler(Msg_CRC_Create_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_Enter), new MessageDispatch.MsgHandler(Msg_RC_Enter_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_Disappear), new MessageDispatch.MsgHandler(Msg_RC_Disappear_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_Dead), new MessageDispatch.MsgHandler(Msg_RC_Dead_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_Revive), new MessageDispatch.MsgHandler(Msg_RC_Revive_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_Exit), new MessageDispatch.MsgHandler(Msg_CRC_Exit_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_MoveStart), new MessageDispatch.MsgHandler(Msg_CRC_Move_Handler.OnMoveStart));
            RegisterMsgHandler(typeof(Msg_CRC_MoveStop), new MessageDispatch.MsgHandler(Msg_CRC_Move_Handler.OnMoveStop));
            RegisterMsgHandler(typeof(Msg_CRC_MoveMeetObstacle), new MessageDispatch.MsgHandler(Msg_CRC_MoveMeetObstacle_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_Face), new MessageDispatch.MsgHandler(Msg_CRC_Face_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_Skill), new MessageDispatch.MsgHandler(Msg_CRC_Skill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_StopSkill), new MessageDispatch.MsgHandler(Msg_CRC_StopSkill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_UserMove), new MessageDispatch.MsgHandler(Msg_RC_UserMove_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_UserFace), new MessageDispatch.MsgHandler(Msg_RC_UserFace_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CreateNpc), new MessageDispatch.MsgHandler(Msg_RC_CreateNpc_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_DestroyNpc), new MessageDispatch.MsgHandler(Msg_RC_DestroyNpc_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcEnter), new MessageDispatch.MsgHandler(Msg_RC_NpcEnter_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcMove), new MessageDispatch.MsgHandler(Msg_RC_NpcMove_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcFace), new MessageDispatch.MsgHandler(Msg_RC_NpcFace_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcTarget), new MessageDispatch.MsgHandler(Msg_RC_NpcTarget_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcSkill), new MessageDispatch.MsgHandler(Msg_RC_NpcSkill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_NpcStopSkill), new MessageDispatch.MsgHandler(Msg_CRC_NpcStopSkill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcDead), new MessageDispatch.MsgHandler(Msg_RC_NpcDead_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NpcDisappear), new MessageDispatch.MsgHandler(Msg_RC_NpcDisappear_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_SyncProperty), new MessageDispatch.MsgHandler(Msg_RC_SyncProperty_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_DebugSpaceInfo), new MessageDispatch.MsgHandler(Msg_RC_DebugSpaceInfo_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_SyncCombatStatisticInfo), new MessageDispatch.MsgHandler(Msg_RC_SyncCombatStatisticInfo_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_PvpCombatInfo), new MessageDispatch.MsgHandler(Msg_RC_PvpCombatInfo_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_SendImpactToEntity), new MessageDispatch.MsgHandler(Msg_CRC_SendImpactToEntity_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_StopGfxImpact), new MessageDispatch.MsgHandler(Msg_CRC_StopGfxImpact_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ImpactDamage), new MessageDispatch.MsgHandler(Msg_RC_ImpactDamage_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ImpactRage), new MessageDispatch.MsgHandler(Msg_RC_ImpactRage_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_InteractObject), new MessageDispatch.MsgHandler(Msg_CRC_InteractObject_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ControlObject), new MessageDispatch.MsgHandler(Msg_RC_ControlObject_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_RefreshItemSkills), new MessageDispatch.MsgHandler(Msg_RC_RefreshItemSkills_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_HighlightPrompt), new MessageDispatch.MsgHandler(Msg_RC_HighlightPrompt_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_NotifyEarnMoney), new MessageDispatch.MsgHandler(Msg_RC_NotifyEarnMoney_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_UpdateUserBattleInfo), new MessageDispatch.MsgHandler(Msg_RC_UpdateUserBattleInfo_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_MissionCompleted), new MessageDispatch.MsgHandler(Msg_RC_MissionCompleted_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_MissionFailed), new MessageDispatch.MsgHandler(Msg_RC_MissionFailed_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CampChanged), new MessageDispatch.MsgHandler(Msg_RC_CampChanged_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_EnableInput), new MessageDispatch.MsgHandler(Msg_RC_EnableInput_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ShowUi), new MessageDispatch.MsgHandler(Msg_RC_ShowUi_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ShowWall), new MessageDispatch.MsgHandler(Msg_RC_ShowWall_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_ShowDlg), new MessageDispatch.MsgHandler(Msg_RC_ShowDlg_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraLookat), new MessageDispatch.MsgHandler(Msg_RC_CameraLookat_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraFollow), new MessageDispatch.MsgHandler(Msg_RC_CameraFollow_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_GfxControlMoveStart), new MessageDispatch.MsgHandler(Msg_CRC_GfxControlMoveStart_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_CRC_GfxControlMoveStop), new MessageDispatch.MsgHandler(Msg_CRC_GfxControlMoveStop_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_UpdateCoefficient), new MessageDispatch.MsgHandler(Msg_RC_UpdateCoefficient_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_AdjustPosition), new MessageDispatch.MsgHandler(Msg_RC_AdjustPosition_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_LockFrame), new MessageDispatch.MsgHandler(Msg_RC_LockFrame_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_PlayAnimation), new MessageDispatch.MsgHandler(Msg_RC_PlayAnimation_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraYaw), new MessageDispatch.MsgHandler(Msg_RC_CameraYaw_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraHeight), new MessageDispatch.MsgHandler(Msg_RC_CameraHeight_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraDistance), new MessageDispatch.MsgHandler(Msg_RC_CameraDistance_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_SetBlockedShader), new MessageDispatch.MsgHandler(Msg_RC_SetBlockedShader_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_StartCountDown), new MessageDispatch.MsgHandler(Msg_RC_StartCountDown_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_PublishEvent), new MessageDispatch.MsgHandler(Msg_RC_PublishEvent_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_CameraEnable), new MessageDispatch.MsgHandler(Msg_RC_CameraEnable_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_SendGfxMessage), new MessageDispatch.MsgHandler(Msg_RC_SendGfxMessage_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_SendGfxMessageById), new MessageDispatch.MsgHandler(Msg_RC_SendGfxMessageById_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_AddSkill), new MessageDispatch.MsgHandler(Msg_RC_AddSkill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_RemoveSkill), new MessageDispatch.MsgHandler(Msg_RC_RemoveSkill_Handler.Execute));
            RegisterMsgHandler(typeof(Msg_RC_StopImpact), new MessageDispatch.MsgHandler(Msg_RC_StopImpact_Handler.Execute));
        }
        private void NetworkThread()
        {
            while (!m_IsQuited)
            {
                if (m_IsWaitStart)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    while (!m_IsQuited && !m_IsConnected && !m_IsWaitStart)
                    {
                        LogSystem.Debug("Connect ip:{0} port:{1} key:{2}\nNetPeer Statistic:{3}", m_Ip, m_Port, m_Key, m_NetClient.Statistics.ToString());
                        try
                        {
                            m_NetClient.Connect(m_Ip, m_Port);
                        }
                        catch
                        {
                        }
                        for (int ct = 0; ct < 10 && !m_IsConnected; ++ct)
                        {
                            OnRecvMessage();
                            LogSystem.Debug("Wait NetConnectionStatus.Connected ...");
                            if (!m_IsConnected)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                        if (!m_IsQuited && !m_IsConnected && !m_IsWaitStart)
                            GameControler.NotifyRoomServerDisconnected();
                    }
                    OnRecvMessage();
                }
            }
        }
        private void OnConnected(NetConnection conn)
        {
            m_Connection = conn;
            m_IsConnected = true;
            Msg_CR_ShakeHands bd = new Msg_CR_ShakeHands();
            bd.auth_key = m_Key;
            SendMessage(bd);

            GameControler.NotifyRoomServerConnected();
        }
        private void OnRecvMessage()
        {
            m_NetClient.MessageReceivedEvent.WaitOne(1000);
            NetIncomingMessage im;
            while ((im = m_NetClient.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        //LogSystem.Debug("Debug Message: {0}", im.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        //LogSystem.Debug("Error Message: {0}", im.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        //LogSystem.Debug("Warning Message: {0}", im.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = im.SenderConnection.Status;

                        string reason = im.ReadString();
                        LogSystem.Debug("Network Status Changed:{0} Reason:{1}\nStatistic:{2}", status.ToString(), reason, im.SenderConnection.Statistics.ToString());
                        if (NetConnectionStatus.Disconnected == status)
                        {
                            m_IsConnected = false;
                            m_CanSendMessage = false;
                        }
                        else if (NetConnectionStatus.Connected == status)
                        {
                            OnConnected(im.SenderConnection);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        if (m_IsConnected == false)
                        {
                            break;
                        }
                        try
                        {
                            byte[] data = im.ReadBytes(im.LengthBytes);
                            object msg = Serialize.Decode(data);
                            if (msg != null)
                            {
                                PushMsg(msg, im.SenderConnection);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogSystem.Error("Decode Message exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        }
                        break;
                    default:
                        break;
                }
                m_NetClient.Recycle(im);
            }
        }
        private bool PushMsg(object msg, NetConnection conn)
        {
            lock (m_Lock)
            {
                m_QueuePair.Enqueue(new KeyValuePair<NetConnection, object>(conn, msg));
                return true;
            }
        }
        private int ProcessMsg()
        {
            lock (m_Lock)
            {
                if (m_QueuePair.Count <= 0)
                    return -1;
                foreach (KeyValuePair<NetConnection, object> kv in m_QueuePair)
                {
                    object msg = kv.Value;
                    try
                    {
                        m_Dispatch.Dispatch(msg, kv.Key);
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error("ProcessMsg Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
                m_QueuePair.Clear();
                return 0;
            }
        }

        private void StartNetClient()
        {
            if (m_NetClient != null)
            {
                if (!m_NetClientStarted)
                {
                    m_NetClient.Start();
                    m_NetClientStarted = true;
                }
            }
        }
        private void ShutdownNetClient()
        {
            if (m_NetClient != null)
            {
                if (m_NetClientStarted)
                {
                    m_NetClient.Shutdown("bye");
                    m_NetClientStarted = false;
                }
            }
        }
        private void InternalPing()
        {
            Msg_Ping builder = new Msg_Ping();
            m_LastPingTime = TimeUtility.GetLocalMilliseconds();
            builder.send_ping_time = (int)m_LastPingTime;
            SendMessage(builder);
        }

        public bool CanSendMessage
        {
            get { return m_CanSendMessage; }
            set { m_CanSendMessage = value; }
        }
        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }
        public int CampId
        {
            get { return m_CampId; }
            set { m_CampId = value; }
        }
        public int SceneId
        {
            get { return m_SceneId; }
            set { m_SceneId = value; }
        }

        private NetworkSystem() { }

        private long m_PingPongNumber = 0;
        private long m_LastPingTime = TimeUtility.GetLocalMilliseconds();        // ms
        private int m_PingInterval = 1000;         // ms

        private NetPeerConfiguration m_Config;
        private NetClient m_NetClient;
        private NetConnection m_Connection;
        private Thread m_NetThread;
        private bool m_NetClientStarted = false;

        private string m_Ip;
        private int m_Port;
        private bool m_IsConnected = false;
        private bool m_IsWaitStart = true;

        private bool m_IsQuited = false;
        private bool m_CanSendMessage = false;
        private MessageDispatch m_Dispatch = new MessageDispatch();
        private Queue<KeyValuePair<NetConnection, object>> m_QueuePair = new Queue<KeyValuePair<NetConnection, object>>();
        private object m_Lock = new object();
        private uint m_Key = 0;
        private float m_LastFaceDir = 0.0f;
        private int m_HeroId = 0;
        private int m_CampId = 0;
        private int m_SceneId = 0;

        */
    }

    public struct PingRecord
    {
        public long m_Ping;
        public long m_TimeDifferental;
    }
}
