using System;
using System.IO;
using System.Collections.Generic;
using DashFireMessage;

namespace DashFire.Network
{
    public class Serialize
    {
        public static bool Init()
        {
            if (!s_Inited)
            {
                s_Inited = true;

                RegisterIDName((int)MessageDefine.Msg_Ping, typeof(Msg_Ping));
                RegisterIDName((int)MessageDefine.Msg_Pong, typeof(Msg_Pong));
                RegisterIDName((int)MessageDefine.Msg_CR_ShakeHands, typeof(Msg_CR_ShakeHands));
                RegisterIDName((int)MessageDefine.Msg_RC_ShakeHands_Ret, typeof(Msg_RC_ShakeHands_Ret));
                RegisterIDName((int)MessageDefine.Msg_CR_Observer, typeof(Msg_CR_Observer));
                RegisterIDName((int)MessageDefine.Msg_CRC_Create, typeof(Msg_CRC_Create));
                RegisterIDName((int)MessageDefine.Msg_RC_Enter, typeof(Msg_RC_Enter));
                RegisterIDName((int)MessageDefine.Msg_RC_Disappear, typeof(Msg_RC_Disappear));
                RegisterIDName((int)MessageDefine.Msg_RC_Dead, typeof(Msg_RC_Dead));
                RegisterIDName((int)MessageDefine.Msg_RC_Revive, typeof(Msg_RC_Revive));
                RegisterIDName((int)MessageDefine.Msg_CRC_Exit, typeof(Msg_CRC_Exit));
                RegisterIDName((int)MessageDefine.Msg_CRC_MoveStart, typeof(Msg_CRC_MoveStart));
                RegisterIDName((int)MessageDefine.Msg_CRC_MoveStop, typeof(Msg_CRC_MoveStop));
                RegisterIDName((int)MessageDefine.Msg_CRC_MoveMeetObstacle, typeof(Msg_CRC_MoveMeetObstacle));
                RegisterIDName((int)MessageDefine.Msg_CRC_Face, typeof(Msg_CRC_Face));
                RegisterIDName((int)MessageDefine.Msg_CRC_Skill, typeof(Msg_CRC_Skill));
                RegisterIDName((int)MessageDefine.Msg_CRC_StopSkill, typeof(Msg_CRC_StopSkill));
                RegisterIDName((int)MessageDefine.Msg_RC_CreateNpc, typeof(Msg_RC_CreateNpc));
                RegisterIDName((int)MessageDefine.Msg_RC_DestroyNpc, typeof(Msg_RC_DestroyNpc));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcEnter, typeof(Msg_RC_NpcEnter));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcMove, typeof(Msg_RC_NpcMove));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcFace, typeof(Msg_RC_NpcFace));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcTarget, typeof(Msg_RC_NpcTarget));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcSkill, typeof(Msg_RC_NpcSkill));
                RegisterIDName((int)MessageDefine.Msg_CRC_NpcStopSkill, typeof(Msg_CRC_NpcStopSkill));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcDead, typeof(Msg_RC_NpcDead));
                RegisterIDName((int)MessageDefine.Msg_RC_NpcDisappear, typeof(Msg_RC_NpcDisappear));
                RegisterIDName((int)MessageDefine.Msg_RC_SyncProperty, typeof(Msg_RC_SyncProperty));
                RegisterIDName((int)MessageDefine.Msg_RC_DebugSpaceInfo, typeof(Msg_RC_DebugSpaceInfo));
                RegisterIDName((int)MessageDefine.Msg_CR_SwitchDebug, typeof(Msg_CR_SwitchDebug));
                RegisterIDName((int)MessageDefine.Msg_RC_SyncCombatStatisticInfo, typeof(Msg_RC_SyncCombatStatisticInfo));
                RegisterIDName((int)MessageDefine.Msg_RC_PvpCombatInfo, typeof(Msg_RC_PvpCombatInfo));
                RegisterIDName((int)MessageDefine.Msg_CRC_SendImpactToEntity, typeof(Msg_CRC_SendImpactToEntity));
                RegisterIDName((int)MessageDefine.Msg_CRC_StopGfxImpact, typeof(Msg_CRC_StopGfxImpact));
                RegisterIDName((int)MessageDefine.Msg_RC_ImpactDamage, typeof(Msg_RC_ImpactDamage));
                RegisterIDName((int)MessageDefine.Msg_RC_ImpactRage, typeof(Msg_RC_ImpactRage));
                RegisterIDName((int)MessageDefine.Msg_CRC_InteractObject, typeof(Msg_CRC_InteractObject));
                RegisterIDName((int)MessageDefine.Msg_RC_ControlObject, typeof(Msg_RC_ControlObject));
                RegisterIDName((int)MessageDefine.Msg_RC_RefreshItemSkills, typeof(Msg_RC_RefreshItemSkills));
                RegisterIDName((int)MessageDefine.Msg_RC_HighlightPrompt, typeof(Msg_RC_HighlightPrompt));
                RegisterIDName((int)MessageDefine.Msg_RC_NotifyEarnMoney, typeof(Msg_RC_NotifyEarnMoney));
                RegisterIDName((int)MessageDefine.Msg_CR_Quit, typeof(Msg_CR_Quit));
                RegisterIDName((int)MessageDefine.Msg_RC_UserMove, typeof(Msg_RC_UserMove));
                RegisterIDName((int)MessageDefine.Msg_RC_UserFace, typeof(Msg_RC_UserFace));
                RegisterIDName((int)MessageDefine.Msg_CR_UserMoveToPos, typeof(Msg_CR_UserMoveToPos));
                RegisterIDName((int)MessageDefine.Msg_CR_UserMoveToAttack, typeof(Msg_CR_UserMoveToAttack));
                RegisterIDName((int)MessageDefine.Msg_RC_UpdateUserBattleInfo, typeof(Msg_RC_UpdateUserBattleInfo));
                RegisterIDName((int)MessageDefine.Msg_RC_MissionCompleted, typeof(Msg_RC_MissionCompleted));
                RegisterIDName((int)MessageDefine.Msg_RC_MissionFailed, typeof(Msg_RC_MissionFailed));
                RegisterIDName((int)MessageDefine.Msg_RC_CampChanged, typeof(Msg_RC_CampChanged));
                RegisterIDName((int)MessageDefine.Msg_RC_EnableInput, typeof(Msg_RC_EnableInput));
                RegisterIDName((int)MessageDefine.Msg_RC_ShowUi, typeof(Msg_RC_ShowUi));
                RegisterIDName((int)MessageDefine.Msg_RC_ShowWall, typeof(Msg_RC_ShowWall));
                RegisterIDName((int)MessageDefine.Msg_RC_ShowDlg, typeof(Msg_RC_ShowDlg));
                RegisterIDName((int)MessageDefine.Msg_CR_DlgClosed, typeof(Msg_CR_DlgClosed));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraLookat, typeof(Msg_RC_CameraLookat));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraFollow, typeof(Msg_RC_CameraFollow));
                RegisterIDName((int)MessageDefine.Msg_CRC_GfxControlMoveStart, typeof(Msg_CRC_GfxControlMoveStart));
                RegisterIDName((int)MessageDefine.Msg_CRC_GfxControlMoveStop, typeof(Msg_CRC_GfxControlMoveStop));
                RegisterIDName((int)MessageDefine.Msg_CR_GiveUpCombat, typeof(Msg_CR_GiveUpBattle));
                RegisterIDName((int)MessageDefine.Msg_CR_DeleteDeadNpc, typeof(Msg_CR_DeleteDeadNpc));
                RegisterIDName((int)MessageDefine.Msg_RC_UpdateCoefficient, typeof(Msg_RC_UpdateCoefficient));
                RegisterIDName((int)MessageDefine.Msg_RC_AdjustPosition, typeof(Msg_RC_AdjustPosition));
                RegisterIDName((int)MessageDefine.Msg_RC_LockFrame, typeof(Msg_RC_LockFrame));
                RegisterIDName((int)MessageDefine.Msg_RC_PlayAnimation, typeof(Msg_RC_PlayAnimation));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraYaw, typeof(Msg_RC_CameraYaw));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraHeight, typeof(Msg_RC_CameraHeight));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraDistance, typeof(Msg_RC_CameraDistance));
                RegisterIDName((int)MessageDefine.Msg_RC_SetBlockedShader, typeof(Msg_RC_SetBlockedShader));
                RegisterIDName((int)MessageDefine.Msg_RC_StartCountDown, typeof(Msg_RC_StartCountDown));
                RegisterIDName((int)MessageDefine.Msg_RC_PublishEvent, typeof(Msg_RC_PublishEvent));
                RegisterIDName((int)MessageDefine.Msg_RC_CameraEnable, typeof(Msg_RC_CameraEnable));
                RegisterIDName((int)MessageDefine.Msg_CR_HitCountChanged, typeof(Msg_CR_HitCountChanged));
                RegisterIDName((int)MessageDefine.Msg_RC_SendGfxMessage, typeof(Msg_RC_SendGfxMessage));
                RegisterIDName((int)MessageDefine.Msg_RC_SendGfxMessageById, typeof(Msg_RC_SendGfxMessageById));
                RegisterIDName((int)MessageDefine.Msg_RC_AddSkill, typeof(Msg_RC_AddSkill));
                RegisterIDName((int)MessageDefine.Msg_RC_RemoveSkill, typeof(Msg_RC_RemoveSkill));
                RegisterIDName((int)MessageDefine.Msg_RC_StopImpact, typeof(Msg_RC_StopImpact));
            }
            return true;
        }
        private static void RegisterIDName(int id, Type msgtype)
        {
            s_DicIDMsg[id] = msgtype;
            s_DicIDName[msgtype] = id;
        }
        public static byte[] Encode(object msg)
        {
            if (s_DicIDName.ContainsKey(msg.GetType()))
            {
                DataStream.SetLength(0);
                Serializer.Serialize(DataStream, msg);
                byte[] ret = new byte[2 + DataStream.Length];
                int id = s_DicIDName[msg.GetType()];
                ret[0] = (byte)(id >> 8);
                ret[1] = (byte)(id);
                DataStream.Position = 0;
                DataStream.Read(ret, 2, ret.Length - 2);

                //LogSystem.Info("encode message:id {0} len({1})[{2}]", id, ret.Length - 2, msg.GetType().Name);

                return ret;
            }
            else
            {
                return null;
            }
        }
        public static object Decode(byte[] msgbuf)
        {
            int id = (int)(((int)msgbuf[0] << 8) | ((int)msgbuf[1]));
            if (id < 0)
            {
                LogSystem.Debug("decode message error:id({0}) len({1}) error !!!", id, msgbuf.Length - 2);
                return null;
            }

            if (s_DicIDMsg.ContainsKey(id))
            {
                Type t = s_DicIDMsg[id];
                DataStream.SetLength(0);
                DataStream.Write(msgbuf, 2, msgbuf.Length - 2);
                DataStream.Position = 0;
                try
                {
                    object msg = Serializer.Deserialize(DataStream, null, t);
                    if (msg == null)
                    {
                        LogSystem.Debug("decode message error:can't find id {0} len({1}) !!!", id, msgbuf.Length - 2);
                        return null;
                    }
                    //LogSystem.Info("decode message:id {0} len({1})[{2}]", id, msgbuf.Length - 2, msg.GetType().Name);
                    return msg;
                }
                catch (Exception ex)
                {
                    LogSystem.Error("decode message error:id({0}) len({1}) {2}\n{3}\nData:\n{4}", id, msgbuf.Length - 2, ex.Message, ex.StackTrace, Helper.BinToHex(msgbuf, 2));
                    throw ex;
                }
            }
            return null;
        }

        private static MemoryStream DataStream
        {
            get
            {
                if (null == s_Stream)
                    s_Stream = new MemoryStream(4096);
                return s_Stream;
            }
        }
        private static ProtobufSerializer Serializer
        {
            get
            {
                if (null == s_Serializer)
                    s_Serializer = new ProtobufSerializer();
                return s_Serializer;
            }
        }

        [ThreadStatic]
        private static MemoryStream s_Stream = null;
        [ThreadStatic]
        private static ProtobufSerializer s_Serializer = null;
        private static MyDictionary<int, Type> s_DicIDMsg = new MyDictionary<int, Type>();
        private static MyDictionary<Type, int> s_DicIDName = new MyDictionary<Type, int>();
        private static bool s_Inited = false;
    }
}
