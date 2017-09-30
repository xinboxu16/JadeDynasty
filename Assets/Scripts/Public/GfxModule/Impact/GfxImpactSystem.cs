using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public sealed class GfxImpactSystem
    {
        private const int m_DeadImpactId = 88888;
        private List<ImpactLogicInfo> m_ImpactLogicInfos = new List<ImpactLogicInfo>();

        private bool m_ShowDebug = false;

        public void SendDeadImpact(int targetId)
        {
            GameObject targetObj = LogicSystem.GetGameObject(targetId);
            if(null != targetObj)
            {
                bool needSendImpact = true;
                Vector3 srcPos = Vector3.zero;
                float srcDir = 0.0f;
                for(int i = m_ImpactLogicInfos.Count - 1; i >= 0; --i)
                {
                    ImpactLogicInfo info = m_ImpactLogicInfos[i];
                    if(null != info)
                    {
                        if(info.IsActive)
                        {
                            if(info.Target.GetInstanceID() == targetObj.GetInstanceID())
                            {
                                if(info.LogicId != (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Default)
                                {
                                    IGfxImpactLogic logic = GfxImpactLogicManager.Instance.GetGfxImpactLogic(info.LogicId);
                                    if (null != logic)
                                    {
                                        logic.OnInterrupted(info);
                                    }
                                    needSendImpact = true;
                                    srcDir = info.ImpactSrcDir;
                                    srcPos = info.ImpactSrcPos;
                                    m_ImpactLogicInfos.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                if(needSendImpact)
                {
                    SendImpactToCharacterImpl(targetObj, targetObj, m_DeadImpactId, srcPos.x, srcPos.y, srcPos.z, srcDir, -1);
                }
            }
        }

        private void SendImpactToCharacterImpl(GameObject senderObj, GameObject targetObj, int impactId, float x, float y, float z, float dir, int forceLogicId)
        {
            ImpactLogicData config = SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_IMPACT, impactId) as ImpactLogicData;
            if(null != config)
            {
                ImpactLogicInfo info = new ImpactLogicInfo();
                info.Sender = senderObj;
                info.Target = targetObj;
                info.ImpactId = impactId;
                info.StartTime = Time.time;
                info.LogicId = config.ImpactGfxLogicId;
                if(-1 != forceLogicId)
                {
                    info.LogicId = forceLogicId;
                }
                info.IsActive = true;
                info.AnimationInfo = new CurveInfo(config.AnimationInfo);
                info.LockFrameInfo = new CurveInfo(config.LockFrameInfo);
                info.MovementInfo = new CurveMoveInfo(config.CurveMoveInfo);
                info.AdjustPoint = Quaternion.Euler(0, ImpactUtility.RadianToDegree(dir), 0) * ImpactUtility.ConvertVector3D(config.AdjustPoint) + new Vector3(x, y, z);
                info.AdjustAppend = config.AdjustAppend;
                info.AdjustDegreeXZ = config.AdjustDegreeXZ;
                info.AdjustDegreeY = config.AdjustDegreeY;
                info.ConfigData = config;
                info.Duration = config.ImpactTime / 1000;
                info.ImpactSrcPos = new Vector3(x, y, z);
                info.ImpactSrcDir = dir;
                info.NormalEndPoint = GetImpactEndPos(info.Target.transform.position, Quaternion.Euler(0, ImpactUtility.RadianToDegree(info.ImpactSrcDir), 0), info);
                info.NormalPos = info.Target.transform.position;
                info.OrignalPos = info.Target.transform.position;

                ShowDebugObject(info.AdjustPoint, 0);
                ShowDebugObject(info.NormalEndPoint, 1);

                foreach (List<int> effect in config.EffectList)
                {
                    if (effect.Count > 0)
                    {
                        info.AddEffectData(effect[UnityEngine.Random.Range(0, effect.Count)]);
                    }
                }
                AddImpactInfo(info);
                IGfxImpactLogic logic = GfxImpactLogicManager.Instance.GetGfxImpactLogic(info.LogicId);
                if (logic != null)
                {
                    logic.StartImpact(info);
                }
                else
                {
                    DashFire.LogSystem.Debug("SendImpactToCharacter: Can't find impact logic {0}", info.LogicId);
                }
            }
        }

        private void AddImpactInfo(ImpactLogicInfo addInfo)
        {
            for(int i = m_ImpactLogicInfos.Count - 1; i >= 0; --i)
            {
                ImpactLogicInfo info = m_ImpactLogicInfos[i];
                if (null != info && null != info.Target && null != addInfo && null != addInfo.Target)
                {
                    if (info.Target.GetInstanceID() == addInfo.Target.GetInstanceID() && addInfo.ImpactId == info.ImpactId)
                    {
                        if(info.LogicId == (int) GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Default || addInfo.LogicId == (int)GfxImpactLogicManager.GfxImpactLogicId.GfxImpactLogic_Default)
                        {
                            m_ImpactLogicInfos.RemoveAt(i);
                        }
                    }
                }
            }
            m_ImpactLogicInfos.Add(addInfo);
        }

        private void ShowDebugObject(Vector3 pos, int type)
        {
            if (true == m_ShowDebug)
            {
                string res = "";
                if (type == 0)
                {
                    res = "BlueCylinder";
                }
                else
                {
                    res = "RedCylinder";
                }
                GameObject obj = ResourceSystem.NewObject(res, 1.0f) as GameObject;
                if (null != obj)
                {
                    obj.transform.position = pos;
                }
            }
        }

        private Vector3 GetImpactEndPos(Vector3 startPos, Quaternion q, ImpactLogicInfo info)
        {
            Vector3 result = Vector3.zero;
            List<float> list = Converter.ConvertNumericList<float>(info.ConfigData.CurveMoveInfo);
            for (int i = 1; i < list.Count - 6; i += 7)
            {
                float time = list[i];
                float sx = list[i + 1];
                float sy = list[i + 2];
                float sz = list[i + 3];
                float ax = list[i + 4];
                float ay = list[i + 5];
                float az = list[i + 6];
                float timeSqr_div_2 = time * time / 2;
                result += q * new Vector3(sx * time + timeSqr_div_2 * ax, sy * time + timeSqr_div_2 * ay, sz * time + timeSqr_div_2 * az);
            }

            return result + startPos;
        }

        public void StopGfxImpact(int objId, int impactId)
        {
            GameObject obj = LogicSystem.GetGameObject(objId);
            if (null != obj)
            {
                ImpactLogicInfo info = GetImpactInfoById(objId, impactId);
                if (null != info && info.IsActive)
                {
                    IGfxImpactLogic logic = GfxImpactLogicManager.Instance.GetGfxImpactLogic(info.LogicId);
                    logic.OnInterrupted(info);
                    info.IsActive = false;
                }
            }
        }

        public ImpactLogicInfo GetImpactInfoById(int objId, int impactId)
        {
            for (int i = m_ImpactLogicInfos.Count - 1; i >= 0; --i)
            {
                ImpactLogicInfo info = m_ImpactLogicInfos[i];
                if (null != info)
                {
                    if (info.IsActive && info.ImpactId == impactId)
                    {
                        GameObject target = info.Target;
                        if (target != null)
                        {
                            SharedGameObjectInfo shareInfo = LogicSystem.GetSharedGameObjectInfo(target);
                            if (shareInfo.m_ActorId == objId)
                            {
                                return info;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static GfxImpactSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxImpactSystem s_Instance = new GfxImpactSystem();
    }
}
