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

        private NetPeerConfiguration m_Config;
        private NetClient m_NetClient;
        private NetConnection m_Connection;
        private Thread m_NetThread;

        private MessageDispatch m_Dispatch = new MessageDispatch();

        private bool m_CanSendMessage = false;

        private bool m_NetClientStarted = false;

        private int m_CampId = 0;
        private int m_HeroId = 0;
        private int m_SceneId = 0;

        private float m_LastFaceDir = 0.0f;

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

        public bool CanSendMessage
        {
            get { return m_CanSendMessage; }
            set { m_CanSendMessage = value; }
        }

        public void QuitClient()
        {
            m_IsQuited = true;
        }

        public bool IsWaitStart
        {
            get { return m_IsWaitStart; }
        }

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
            //未实现 不理解用处
            //m_NetThread = new Thread(new ThreadStart(NetworkThread));
            //m_NetThread.Start();

            return true;
        }

        public void SyncDeleteDeadNpc(int npcId)
        {
            Msg_CR_DeleteDeadNpc msg = new Msg_CR_DeleteDeadNpc();
            msg.npc_id = npcId;
            SendMessage(msg);
        }

        public void SyncPlayerMoveStart(float dir)
        {
            WorldSystem.Instance.IsAlreadyNotifyMeetObstacle = false;//是否通知移动

            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if(null != userInfo)
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

        public void SyncFaceDirection(float face_direction)
        {
            if(Math.Abs(m_LastFaceDir - face_direction) <= 0.01)
            {
                return;
            }
            m_LastFaceDir = face_direction;
            Msg_CRC_Face bd = new Msg_CRC_Face();
            bd.face_direction = face_direction;
            SendMessage(bd);
        }

        public void SyncPlayerMoveStop()
        {
            UserInfo userInfo = WorldSystem.Instance.GetPlayerSelf();
            if(null != userInfo)
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
    }

    public struct PingRecord
    {
        public long m_Ping;
        public long m_TimeDifferental;
    }
}
