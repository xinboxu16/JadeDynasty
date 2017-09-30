using DashFire;
using GfxModule.Skill.Trigers;
using SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Skill
{
    public delegate void GfxSkillStartHandler(GameObject obj, int skillid);

    public sealed class GfxSkillSystem
    {
        private class SkillInstanceInfo
        {
            public int m_SkillId;
            public SkillInstance m_SkillInstance;
            public bool m_IsUsed;
        }

        private class SkillLogicInfo
        {
            private GameObject m_Sender;
            private SkillInstanceInfo m_SkillInfo;

            public SkillLogicInfo(GameObject obj, SkillInstanceInfo info)
            {
                m_Sender = obj;
                m_SkillInfo = info;
            }

            public GameObject Sender
            {
                get
                {
                    return m_Sender;
                }
            }
        }

        private List<SkillLogicInfo> m_SkillLogicInfos = new List<SkillLogicInfo>();

        //技能系统
        public void Init()
        {
            //注册点击
            GfxSystem.OnFingerDown += TriggerUtil.OnFingerDown;
            GfxSystem.OnFingerUp += TriggerUtil.OnFingerUp;

            //注册技能触发器
            SkillTrigerManager.Instance.RegisterTrigerFactory("movecontrol", new SkillTrigerFactoryHelper<Trigers.MoveControlTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("animation", new SkillTrigerFactoryHelper<Trigers.AnimationTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("areadamage", new SkillTrigerFactoryHelper<Trigers.AreaDamageTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("colliderdamage", new SkillTrigerFactoryHelper<Trigers.ColliderDamageTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("playsound", new SkillTrigerFactoryHelper<Trigers.PlaySoundTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopsound", new SkillTrigerFactoryHelper<Trigers.StopSoundTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("charactereffect", new SkillTrigerFactoryHelper<Trigers.CharacterEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sceneeffect", new SkillTrigerFactoryHelper<Trigers.SceneEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("charge", new SkillTrigerFactoryHelper<Trigers.ChargeTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("jump", new SkillTrigerFactoryHelper<Trigers.JumpTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("timescale", new SkillTrigerFactoryHelper<Trigers.TimeScaleTriger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("addimpacttoself", new SkillTrigerFactoryHelper<Trigers.AddImpactToSelfTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("lockframe", new SkillTrigerFactoryHelper<Trigers.LockFrameTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("movechild", new SkillTrigerFactoryHelper<Trigers.MoveChildTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addbreaksection", new SkillTrigerFactoryHelper<Trigers.BreakSectionTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("startcurvemove", new SkillTrigerFactoryHelper<Trigers.CurveMovementTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("shakecamera2", new SkillTrigerFactoryHelper<Trigers.ShakeCamera2Trigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setanimspeed", new SkillTrigerFactoryHelper<Trigers.AnimationSpeedTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setcross2othertime", new SkillTrigerFactoryHelper<Trigers.SetCrossFadeTimeTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("findmovetarget", new SkillTrigerFactoryHelper<Trigers.ChooseTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addlockinputtime", new SkillTrigerFactoryHelper<Trigers.AddLockInputTimeTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("summonnpc", new SkillTrigerFactoryHelper<Trigers.SummonObjectTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("settransform", new SkillTrigerFactoryHelper<Trigers.SetTransformTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("destroyself", new SkillTrigerFactoryHelper<Trigers.DestroySelfTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("destroysummonnpc", new SkillTrigerFactoryHelper<Trigers.DestroySummonObjectTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setchildvisible", new SkillTrigerFactoryHelper<Trigers.SetChildVisibleTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("rotate", new SkillTrigerFactoryHelper<Trigers.RotateTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setenable", new SkillTrigerFactoryHelper<Trigers.SetEnableTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("gotosection", new SkillTrigerFactoryHelper<Trigers.GotoSectionTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("createshadow", new SkillTrigerFactoryHelper<Trigers.CreateShadowTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("cleardamagepool", new SkillTrigerFactoryHelper<Trigers.ClearDamagePoolTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("cleardamagestate", new SkillTrigerFactoryHelper<Trigers.ClearDamageStateTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("checkonground", new SkillTrigerFactoryHelper<Trigers.CheckOnGroundTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopcursection", new SkillTrigerFactoryHelper<Trigers.StopCurSectionTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("enablechangedir", new SkillTrigerFactoryHelper<Trigers.EnableChangeDirTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopeffect", new SkillTrigerFactoryHelper<Trigers.StopEffectTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setcamerafollowspeed", new SkillTrigerFactoryHelper<Trigers.SetCameraFollowSpeed>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("resetcamerafollowspeed", new SkillTrigerFactoryHelper<Trigers.ResetCameraFollowSpeed>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("movecamera", new SkillTrigerFactoryHelper<Trigers.MoveCameraTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("facetotarget", new SkillTrigerFactoryHelper<Trigers.FaceToTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("rotatecamera", new SkillTrigerFactoryHelper<Trigers.RotateCameraTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setlifetime", new SkillTrigerFactoryHelper<Trigers.SetlifeTimeTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("simulatemove", new SkillTrigerFactoryHelper<Trigers.SimulateMoveTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("exchangeposition", new SkillTrigerFactoryHelper<Trigers.ExchangePositionTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("fruitninjia", new SkillTrigerFactoryHelper<Trigers.FruitNinjiaTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("oninput", new SkillTrigerFactoryHelper<Trigers.OnInputTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("move2targetpos", new SkillTrigerFactoryHelper<Trigers.Move2TargetPosTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("crosssummonmove", new SkillTrigerFactoryHelper<Trigers.CrossSummonMoveTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("storepos", new SkillTrigerFactoryHelper<Trigers.StorePosTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("restorepos", new SkillTrigerFactoryHelper<Trigers.RestorePosTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("blackscene", new SkillTrigerFactoryHelper<Trigers.BlackSceneTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setuivisible", new SkillTrigerFactoryHelper<Trigers.SetUIVisibleTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addimpacttotarget", new SkillTrigerFactoryHelper<Trigers.AddImpactToTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("grabtarget", new SkillTrigerFactoryHelper<Trigers.GrabTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("oncross", new SkillTrigerFactoryHelper<Trigers.OnCrossTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("fieldofview", new SkillTrigerFactoryHelper<Trigers.FieldOfViewTrigger>());
        }

        public void Tick()
        {
            int ct = m_SkillLogicInfos.Count;
            long delta = (long)(Time.deltaTime * 1000 * 1000);
            for(int ix = ct - 1; ix > 0; --ix)
            {
                SkillLogicInfo info = m_SkillLogicInfos[ix];
                //未实现
            }
        }

        public void StartAttack(GameObject obj, Vector3 targetpos)
        {
            LogicSystem.NotifyGfxStartAttack(obj, targetpos.x, targetpos.y, targetpos.z);
        }

        public void StopAttack(GameObject obj)
        {
            LogicSystem.NotifyGfxStopAttack(obj);
        }

        public void PushSkill(GameObject obj, DashFire.SkillCategory category, Vector3 targetpos)
        {
            LogicSystem.NotifyGfxStartSkill(obj, category, targetpos);
        }

        public static GfxSkillSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxSkillSystem s_Instance = new GfxSkillSystem();
    }

}
