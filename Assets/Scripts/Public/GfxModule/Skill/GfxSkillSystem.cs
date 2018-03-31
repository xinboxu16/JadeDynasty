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

            public int SkillId
            {
                get
                {
                    return m_SkillInfo.m_SkillId;
                }
            }

            public SkillInstance SkillInst
            {
                get
                {
                    return m_SkillInfo.m_SkillInstance;
                }
            }

            public SkillInstanceInfo Info
            {
                get
                {
                    return m_SkillInfo;
                }
            }
        }



        public static GfxSkillStartHandler OnGfxSkillStart = null;

        private List<SkillLogicInfo> m_SkillLogicInfos = new List<SkillLogicInfo>();
        private Dictionary<int, List<SkillInstanceInfo>> m_SkillInstancePool = new Dictionary<int, List<SkillInstanceInfo>>();
        private List<ISkillProduct> m_SkillProducts = new List<ISkillProduct>();

        //技能系统
        public void Init()
        {
            //注册点击
            GfxSystem.OnFingerDown += TriggerUtil.OnFingerDown;
            GfxSystem.OnFingerUp += TriggerUtil.OnFingerUp;

            //注册技能触发器
            SkillTrigerManager.Instance.RegisterTrigerFactory("settransform", new SkillTrigerFactoryHelper<Trigers.SetTransformTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setenable", new SkillTrigerFactoryHelper<Trigers.SetEnableTrigger>());

            SkillTrigerManager.Instance.RegisterTrigerFactory("movecontrol", new SkillTrigerFactoryHelper<Trigers.MoveControlTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("animation", new SkillTrigerFactoryHelper<Trigers.AnimationTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("findmovetarget", new SkillTrigerFactoryHelper<Trigers.ChooseTargetTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("startcurvemove", new SkillTrigerFactoryHelper<Trigers.CurveMovementTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("playsound", new SkillTrigerFactoryHelper<Trigers.PlaySoundTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("charactereffect", new SkillTrigerFactoryHelper<Trigers.CharacterEffectTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("areadamage", new SkillTrigerFactoryHelper<Trigers.AreaDamageTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("shakecamera2", new SkillTrigerFactoryHelper<Trigers.ShakeCamera2Trigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("colliderdamage", new SkillTrigerFactoryHelper<Trigers.ColliderDamageTriger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("cleardamagestate", new SkillTrigerFactoryHelper<Trigers.ClearDamageStateTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("destroyself", new SkillTrigerFactoryHelper<Trigers.DestroySelfTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("addimpacttoself", new SkillTrigerFactoryHelper<Trigers.AddImpactToSelfTrigger>());//防御
            SkillTrigerManager.Instance.RegisterTrigerFactory("summonnpc", new SkillTrigerFactoryHelper<Trigers.SummonObjectTrigger>());//召唤


            //user
            SkillTrigerManager.Instance.RegisterTrigerFactory("addbreaksection", new SkillTrigerFactoryHelper<Trigers.BreakSectionTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("movechild", new SkillTrigerFactoryHelper<Trigers.MoveChildTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("setanimspeed", new SkillTrigerFactoryHelper<Trigers.AnimationSpeedTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("stopeffect", new SkillTrigerFactoryHelper<Trigers.StopEffectTrigger>());
            SkillTrigerManager.Instance.RegisterTrigerFactory("sceneeffect", new SkillTrigerFactoryHelper<Trigers.SceneEffectTriger>());

            //SkillTrigerManager.Instance.RegisterTrigerFactory("stopsound", new SkillTrigerFactoryHelper<Trigers.StopSoundTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("charge", new SkillTrigerFactoryHelper<Trigers.ChargeTriger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("jump", new SkillTrigerFactoryHelper<Trigers.JumpTriger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("timescale", new SkillTrigerFactoryHelper<Trigers.TimeScaleTriger>());

            //SkillTrigerManager.Instance.RegisterTrigerFactory("addimpacttoself", new SkillTrigerFactoryHelper<Trigers.AddImpactToSelfTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("lockframe", new SkillTrigerFactoryHelper<Trigers.LockFrameTriger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("setcross2othertime", new SkillTrigerFactoryHelper<Trigers.SetCrossFadeTimeTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("addlockinputtime", new SkillTrigerFactoryHelper<Trigers.AddLockInputTimeTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("destroysummonnpc", new SkillTrigerFactoryHelper<Trigers.DestroySummonObjectTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("setchildvisible", new SkillTrigerFactoryHelper<Trigers.SetChildVisibleTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("rotate", new SkillTrigerFactoryHelper<Trigers.RotateTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("gotosection", new SkillTrigerFactoryHelper<Trigers.GotoSectionTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("createshadow", new SkillTrigerFactoryHelper<Trigers.CreateShadowTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("cleardamagepool", new SkillTrigerFactoryHelper<Trigers.ClearDamagePoolTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("checkonground", new SkillTrigerFactoryHelper<Trigers.CheckOnGroundTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("stopcursection", new SkillTrigerFactoryHelper<Trigers.StopCurSectionTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("enablechangedir", new SkillTrigerFactoryHelper<Trigers.EnableChangeDirTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("setcamerafollowspeed", new SkillTrigerFactoryHelper<Trigers.SetCameraFollowSpeed>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("resetcamerafollowspeed", new SkillTrigerFactoryHelper<Trigers.ResetCameraFollowSpeed>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("movecamera", new SkillTrigerFactoryHelper<Trigers.MoveCameraTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("facetotarget", new SkillTrigerFactoryHelper<Trigers.FaceToTargetTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("rotatecamera", new SkillTrigerFactoryHelper<Trigers.RotateCameraTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("setlifetime", new SkillTrigerFactoryHelper<Trigers.SetlifeTimeTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("simulatemove", new SkillTrigerFactoryHelper<Trigers.SimulateMoveTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("exchangeposition", new SkillTrigerFactoryHelper<Trigers.ExchangePositionTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("fruitninjia", new SkillTrigerFactoryHelper<Trigers.FruitNinjiaTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("oninput", new SkillTrigerFactoryHelper<Trigers.OnInputTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("move2targetpos", new SkillTrigerFactoryHelper<Trigers.Move2TargetPosTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("crosssummonmove", new SkillTrigerFactoryHelper<Trigers.CrossSummonMoveTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("storepos", new SkillTrigerFactoryHelper<Trigers.StorePosTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("restorepos", new SkillTrigerFactoryHelper<Trigers.RestorePosTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("blackscene", new SkillTrigerFactoryHelper<Trigers.BlackSceneTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("setuivisible", new SkillTrigerFactoryHelper<Trigers.SetUIVisibleTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("addimpacttotarget", new SkillTrigerFactoryHelper<Trigers.AddImpactToTargetTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("grabtarget", new SkillTrigerFactoryHelper<Trigers.GrabTargetTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("oncross", new SkillTrigerFactoryHelper<Trigers.OnCrossTrigger>());
            //SkillTrigerManager.Instance.RegisterTrigerFactory("fieldofview", new SkillTrigerFactoryHelper<Trigers.FieldOfViewTrigger>());
        }

        public void Tick()
        {
            int ct = m_SkillLogicInfos.Count;
            long delta = (long)(Time.deltaTime * 1000 * 1000);
            for(int ix = ct - 1; ix >= 0; --ix)//回收不用对象
            {
                SkillLogicInfo info = m_SkillLogicInfos[ix];
                bool exist = LogicSystem.ExistGameObject(info.Sender);
                if(exist)
                {
                    info.SkillInst.Tick(info.Sender, delta);
                }
                if(!exist || info.SkillInst.IsFinished)
                {
                    if(!exist)
                    {
                        info.SkillInst.OnSkillStop(info.Sender, 0);
                    }
                    StopSkillInstance(info);
                    m_SkillLogicInfos.RemoveAt(ix);
                }
            }

            int productCount = m_SkillProducts.Count;
            for(int i = productCount - 1;  i >= 0; --i)
            {
                ISkillProduct product = m_SkillProducts[i];
                product.Tick(delta);
                if(product.IsStoped())
                {
                    m_SkillProducts.RemoveAt(i);
                }
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

        private SkillInstanceInfo GetUnusedSkillInstanceInfoFromPool(int skillId)
        {
            SkillInstanceInfo info = null;
            if(m_SkillInstancePool.ContainsKey(skillId))
            {
                List<SkillInstanceInfo> infos = m_SkillInstancePool[skillId];
                int ct = infos.Count;
                for(int ix = 0; ix < ct; ++ix)
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

        private SkillInstanceInfo NewSkillInstance(int skillId)
        {
            SkillInstanceInfo instInfo = GetUnusedSkillInstanceInfoFromPool(skillId);
            if(null == instInfo)
            {
                SkillLogicData skillData = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_SKILL, skillId) as SkillLogicData;
                if(null != skillData)
                {
                    //加载dsl数据
                    string filePath = HomePath.GetAbsolutePath(FilePathDefine_Client.C_SkillDslPath + skillData.SkillDataFile);
                    SkillConfigManager.Instance.LoadSkillIfNotExist(skillId, filePath);

                    //创建技能实例
                    SkillInstance inst = SkillConfigManager.Instance.NewSkillInstance(skillId);
                    if(null == inst)
                    {
                        LogSystem.Error("Can't load skill config, skill:{0} !", skillId);
                        return null;
                    }

                    SkillInstanceInfo res = new SkillInstanceInfo();
                    res.m_SkillId = skillId;
                    res.m_SkillInstance = inst;
                    res.m_IsUsed = true;

                    AddSkillInstanceInfoToPool(skillId, res);
                    return res;
                }
                else
                {
                    LogSystem.Error("Can't find skill config, skill:{0} !", skillId);
                    return null;
                }
            }
            else
            {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }

        //用于上层逻辑检查通过后调用
        public void StartSkill(int actorId, int skillId)
        {
            GameObject obj = LogicSystem.GetGameObject(actorId);
            if (null != obj)
            {
                SkillLogicInfo logicInfo = m_SkillLogicInfos.Find(info => info.Sender == obj && info.SkillId == skillId);
                if (logicInfo != null)
                {
                    return;
                }

                SharedGameObjectInfo objInfo = LogicSystem.GetSharedGameObjectInfo(actorId);
                if(null != objInfo)
                {
                    ChangeDir(obj, objInfo.WantFaceDir);
                }

                SkillInstanceInfo inst = NewSkillInstance(skillId);
                if(null != inst)
                {
                    m_SkillLogicInfos.Add(new SkillLogicInfo(obj, inst));
                }
                else
                {
                    LogicSystem.NotifyGfxStopSkill(obj, skillId);
                    return;
                }

                logicInfo = m_SkillLogicInfos.Find(info => info.Sender == obj && info.SkillId == skillId);
                if(null != logicInfo)
                {
                    if (OnGfxSkillStart != null)
                    {
                        OnGfxSkillStart(obj, skillId);
                    }
                    LogicSystem.NotifyGfxAnimationStart(obj, true);
                    logicInfo.SkillInst.Start(logicInfo.Sender);
                }
            }
        }

        //actorId自己的(user或者npc)id
        public void StopSkill(int actorId, bool isinterrupt)
        {
            GameObject obj = LogicSystem.GetGameObject(actorId);
            if(null == obj)
            {
                return;
            }
            int count = m_SkillLogicInfos.Count;
            for(int index = count - 1; index >= 0; --index)
            {
                SkillLogicInfo info = m_SkillLogicInfos[index];
                if(info.Sender == obj)
                {
                    if(isinterrupt)
                    {
                        info.SkillInst.OnInterrupt(obj, 0);
                    }
                    else
                    {
                        info.SkillInst.OnSkillStop(obj, 0);
                    }
                    StopSkillInstance(info, isinterrupt);
                    m_SkillLogicInfos.RemoveAt(index);
                }
            }
        }

        private void StopSkillInstance(SkillLogicInfo info)
        {
            StopSkillInstance(info, false);
        }

        private void StopSkillInstance(SkillLogicInfo info, bool isInterrupt)
        {
            if(!isInterrupt)
            {
                if(info.SkillInst.IsControlMove)
                {
                    LogicSystem.NotifyGfxMoveControlFinish(info.Sender, info.SkillId, true);
                    info.SkillInst.IsControlMove = false;
                }
                LogicSystem.NotifyGfxAnimationFinish(info.Sender, true);
                LogicSystem.NotifyGfxStopSkill(info.Sender, info.SkillId);
            }
            else
            {
                if (info.SkillInst.IsControlMove)
                {
                    info.SkillInst.IsControlMove = false;
                }
            }
            RecycleSkillInstance(info.Info);
        }

        private void AddSkillInstanceInfoToPool(int skillId, SkillInstanceInfo info)
        {
            if (m_SkillInstancePool.ContainsKey(skillId))
            {
                List<SkillInstanceInfo> infos = m_SkillInstancePool[skillId];
                infos.Add(info);
            }
            else
            {
                List<SkillInstanceInfo> infos = new List<SkillInstanceInfo>();
                infos.Add(info);
                m_SkillInstancePool.Add(skillId, infos);
            }
        }

        private void RecycleSkillInstance(SkillInstanceInfo info)
        {
            info.m_SkillInstance.Reset();
            info.m_IsUsed = false;
        }

        public static void ChangeDir(GameObject obj, float direction)
        {
            Vector3 rotate = new Vector3(0, direction * 180 / Mathf.PI, 0);
            obj.transform.eulerAngles = rotate;
            LogicSystem.NotifyGfxUpdatePosition(obj, obj.transform.position.x, obj.transform.position.y,
                                                obj.transform.position.z, 0, direction, 0);
        }

        public static void ChangeDir(GameObject obj, Vector3 dir)
        {
            dir.y = 0;
            obj.transform.forward = dir;
            Vector3 rotate = obj.transform.rotation.eulerAngles;
            DashFire.LogicSystem.NotifyGfxUpdatePosition(obj, obj.transform.position.x, obj.transform.position.y,
                                                obj.transform.position.z, 0, rotate.y * Mathf.PI / 180, 0);
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
