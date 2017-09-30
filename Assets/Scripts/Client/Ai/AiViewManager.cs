using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class AiViewManager
    {
        private AiViewManager() { }

        public void Init()
        {
            //TODO 未实现
            AbstractUserStateLogic.OnUserSendStoryMessage += this.OnUserSendStoryMessage;

            AbstractNpcStateLogic.OnNpcMeetEnemy += this.OnNpcMeetEnemy;
            AbstractNpcStateLogic.OnNpcFaceClient += this.OnNpcFace;
            AbstractNpcStateLogic.OnNpcSkill += this.NpcSkill;
        }

        private void OnUserSendStoryMessage(UserInfo user, string msgId, object[] args)
        {
            if (WorldSystem.Instance.IsPveScene() || WorldSystem.Instance.IsPureClientScene())
            {
                ClientStorySystem.Instance.SendMessage(msgId, args);
            }
        }

        private void OnNpcMeetEnemy(NpcInfo npc, Animation_Type animType)
        {
            CharacterView view = EntityManager.Instance.GetCharacterViewById(npc.GetId());
            if(null != view)
            {
                GfxSystem.SendMessage(view.Actor, "OnEventMeetEnemy", null);
            }
            //TODO 未实现
            ImpactSystem.Instance.SendImpactToCharacter(npc, npc.GetMeetEnemyImpact(), npc.GetId(), -1, -1, npc.GetMovementStateInfo().GetPosition3D(), npc.GetMovementStateInfo().GetFaceDir());
        }

        private void OnNpcFace(NpcInfo npc, float faceDirection)
        {
            npc.GetMovementStateInfo().SetWantFaceDir(faceDirection);
            ControlSystemOperation.AdjustCharacterFaceDir(npc.GetId(), faceDirection);
        }

        //释放技能
        private void NpcSkill(NpcInfo npc, int skillId)
        {
            if(null != npc)
            {
                if(npc.SkillController != null)
                {
                    SkillInfo skillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                    if(null != skillInfo)
                    {
                        long curTime = TimeUtility.GetServerMilliseconds();
                        if(!skillInfo.IsInCd(curTime / 1000.0f))
                        {
                            npc.SkillController.ForceStartSkill(skillId);
                            skillInfo.BeginCD();
                        }
                    }
                }
            }
        }

        public static AiViewManager Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static AiViewManager s_Instance = new AiViewManager();
    }
}
