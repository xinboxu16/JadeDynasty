using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class GameLogicNotification : IGameLogicNotification
    {
        public void OnGfxStartStory(int id)
        {
            if(WorldSystem.Instance.IsPveScene())
            {
                ClientStorySystem.Instance.StartStory(id);
            }
            else
            {
                // OnGfxStartStory 只会在单人pve场景中被调用
            }
        }

        public void OnGfxSendStoryMessage(string msgId, object[] args)
        {
            if (WorldSystem.Instance.IsPureClientScene() || WorldSystem.Instance.IsPveScene())
            {
                ClientStorySystem.Instance.SendMessage(msgId, args);
            }
            else
            {
                //通知服务器
                string msgIdPrefix = "dialogover:";
                if (msgId.StartsWith(msgIdPrefix))
                {
                    DashFireMessage.Msg_CR_DlgClosed msg = new DashFireMessage.Msg_CR_DlgClosed();
                    msg.dialog_id = int.Parse(msgId.Substring(msgIdPrefix.Length));
                    Network.NetworkSystem.Instance.SendMessage(msg);
                }
            }
        }

        public void OnGfxControlMoveStart(int objId, int id, bool isSkill)
        {
            //TODO未实现
        }

        public void OnGfxControlMoveStop(int objId, int id, bool isSkill)
        {
            //TODO未实现
        }

        public void OnGfxMoveMeetObstacle(int id)
        {
            CharacterInfo charObj = WorldSystem.Instance.GetCharacterById(id);
            if(null != charObj)
            {
                charObj.GetMovementStateInfo().IsMoveMeetObstacle = true;
                WorldSystem.Instance.NotifyMoveMeetObstacle(false);
            }
        }

        public void OnGfxStartSkill(int id, SkillCategory category, float x, float y, float z)
        {
            //TODO未实现
        }

        public void OnGfxStartAttack(int id, float x, float y, float z)
        {
            //TODO未实现
        }

        public void OnGfxStopAttack(int id)
        {
            //TODO未实现
        }

        public void OnGfxHitTarget(int id, int impactId, int targetId, int hitCount, int skillId, int duration, float x, float y, float z, float dir)
        {
            //TODO未实现
        }

        public void OnGfxForceStartSkill(int id, int skillId)
        {
            //TODO未实现
        }

        public void OnGfxStopSkill(int id, int skillId)
        {
            //TODO未实现
        }

        public void OnGfxSkillBreakSection(int id, int skillid, int breaktype, int startime, int endtime, bool isinterrupt)
        {
            //TODO未实现
        }

        public void OnGfxStopImpact(int id, int impactId)
        {
            //TODO未实现
        }

        public void OnGfxSetCrossFadeTime(int id, string fadeTargetAnim, float crossTime)
        {
            //TODO未实现
        }

        public void OnGfxAddLockInputTime(int id, SkillCategory category, float lockinputtime)
        {
            //TODO未实现
        }

        public void OnGfxSummonNpc(int owner_id, int owner_skill_id, int npc_type_id, string modelPrefab, int skillid, float x, float y, float z)
        {
            //TODO未实现
        }

        public void OnGfxDestroyObj(int id)
        {
            //TODO未实现
        }

        public void OnGfxDestroySummonObject(int id)
        {
            //TODO未实现
        }

        public void OnGfxSetObjLifeTime(int id, long life_remaint_time)
        {
            //TODO未实现
        }

        public void OnGfxSimulateMove(int id)
        {
            //TODO未实现
        }

        public static GameLogicNotification Instance
        {
            get { return s_Instance; }
        }
        private static GameLogicNotification s_Instance = new GameLogicNotification();
    }
}
