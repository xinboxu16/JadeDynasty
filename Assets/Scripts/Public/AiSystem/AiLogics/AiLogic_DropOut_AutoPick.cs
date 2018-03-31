using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class DropOutInfo
    {
        public DropOutType DropType;
        public string Model;
        public string Particle;
        public int Value;
    }
    public enum DropOutType
    {
        GOLD = 0,
        HP = 1,
        MP = 2,
    }

    public delegate void DropOutPlayEffectDelegation(int targetId, string effect, string path);

    public class AiLogic_DropOut_AutoPick : AbstractNpcStateLogic
    {
        public static DropOutPlayEffectDelegation OnDropoutPlayEffect;

        private const long m_IntervalTime = 500; //检测间隔

        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
        }

        protected override void OnStateLogicInit(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            NpcAiStateInfo info = npc.GetAiStateInfo();
            info.Time = 0;
            npc.GetMovementStateInfo().IsMoving = false;
            info.HomePos = npc.GetMovementStateInfo().GetPosition3D();
            info.Target = 0;
        }

        private void IdleHandler(NpcInfo npc, AiCommandDispatcher aiCmdDispatcher, long deltaTime)
        {
            NpcAiStateInfo info = npc.GetAiStateInfo();
            info.Time += deltaTime;
            if (info.Time > m_IntervalTime)
            {
                info.Time = 0;
                UserInfo target = null;
                target = AiLogicUtility.GetNearstTargetHelper(npc, CharacterRelation.RELATION_FRIEND, AiTargetType.USER) as UserInfo;
                if (null != target && !npc.NeedDelete)
                {
                    DropOutInfo dropInfo = npc.GetAiStateInfo().AiDatas.GetData<DropOutInfo>();
                    string path = "";
                    if (null != dropInfo)
                    {
                        switch (dropInfo.DropType)
                        {
                            case DropOutType.GOLD:
                                target.Money += dropInfo.Value;
                                target.UserManager.FireGainMoneyEvent(target.GetId(), dropInfo.Value);
                                path = "ef_head";
                                break;
                            case DropOutType.HP:
                                int addHp = (int)(target.GetActualProperty().HpMax * dropInfo.Value / 100.0f);
                                if (target.GetActualProperty().HpMax > addHp + target.Hp)
                                {
                                    target.SetHp(Operate_Type.OT_Relative, addHp);
                                }
                                else
                                {
                                    target.SetHp(Operate_Type.OT_Absolute, target.GetActualProperty().HpMax);
                                }
                                target.UserManager.FireDamageEvent(target.GetId(), -1, false, false, addHp, 0);
                                path = "Bone_Root";
                                break;
                            case DropOutType.MP:
                                int addEnergy = (int)(target.GetActualProperty().EnergyMax * dropInfo.Value / 100.0f);
                                if (target.GetActualProperty().EnergyMax > addEnergy + target.Energy)
                                {
                                    target.SetEnergy(Operate_Type.OT_Relative, addEnergy);
                                }
                                else
                                {
                                    target.SetEnergy(Operate_Type.OT_Absolute, target.GetActualProperty().EnergyMax);
                                }
                                target.UserManager.FireDamageEvent(target.GetId(), -1, false, false, 0, addEnergy);
                                path = "Bone_Root";
                                break;
                        }
                        if (null != OnDropoutPlayEffect)
                        {
                            OnDropoutPlayEffect(target.GetId(), dropInfo.Particle, path);
                        }
                    }
                    npc.NeedDelete = true;
                }
            }
        }
    }
}
