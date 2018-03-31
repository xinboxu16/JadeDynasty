using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Impact
{
    public class CurveInfo
    {
        private class Section
        {
            public float StartTime = 0.0f;
            public float EndTime = 0.0f;
            public float Speed = 1.0f;
        }

        private List<Section> m_Sections = new List<Section>();

        public CurveInfo(string data)
        {
            List<float> floatData = Converter.ConvertNumericList<float>(data);
            if(floatData.Count > 0)
            {
                float curStartTime = floatData[0];
                for (int i = 1; i <= (floatData.Count - 1) / 2; ++i)
                {
                    Section sec = new Section();
                    sec.StartTime = curStartTime;
                    sec.EndTime = curStartTime + floatData[2 * i - 1];
                    sec.Speed = floatData[2 * i];
                    curStartTime += floatData[2 * i - 1];
                    m_Sections.Add(sec);
                }
            }
        }

        public virtual float GetSpeedByTime(float time)
        {
            foreach(Section sec in m_Sections)
            {
                if(time >= sec.StartTime && time < sec.EndTime)
                {
                    return sec.Speed;
                }
            }
            return 1.0f;
        }
    }

    public class CurveMoveInfo
    {
        private class Section
        {
            public float StartTime = 0.0f;
            public float EndTime = 0.0f;
            public float XSpeed = 0.0f;
            public float YSpeed = 0.0f;
            public float ZSpeed = 0.0f;
            public float XAcce = 0.0f;
            public float YAcce = 0.0f;
            public float ZAcce = 0.0f;
        }

        private List<Section> m_Sections = new List<Section>();

        public CurveMoveInfo(string data)
        {
            List<float> floatData = Converter.ConvertNumericList<float>(data);
            if (floatData.Count > 0)
            {
                float curStartTime = floatData[0];
                for (int i = 1; i <= (floatData.Count - 1) / 7; ++i)
                {
                    Section sec = new Section();
                    sec.StartTime = curStartTime;
                    sec.EndTime = curStartTime + floatData[7 * i - 6];
                    sec.XSpeed = floatData[7 * i - 5];
                    sec.YSpeed = floatData[7 * i - 4];
                    sec.ZSpeed = floatData[7 * i - 3];
                    sec.XAcce = floatData[7 * i - 2];
                    sec.YAcce = floatData[7 * i - 1];
                    sec.ZAcce = floatData[7 * i];
                    curStartTime += floatData[7 * i - 6];
                    m_Sections.Add(sec);
                }
            }
        }

        public Vector3 GetSpeedByTime(float time, float gravity = 0.0f)
        {
            foreach (Section sec in m_Sections)
            {
                if (time >= sec.StartTime && time < sec.EndTime)
                {
                    return new Vector3(sec.XSpeed + sec.XAcce * (time - sec.StartTime),
                                       sec.YSpeed + (sec.YAcce + gravity) * (time - sec.StartTime),
                                       sec.ZSpeed + sec.ZAcce * (time - sec.StartTime));
                }
            }
            if (time < GetStartTime())
            {
                return new Vector3(0, gravity * time, 0);
            }
            if (time > GetEndTime())
            {
                return new Vector3(0, (time - GetEndTime()) * gravity, 0);
            }

            return Vector3.zero;
        }

        private float GetStartTime()
        {
            if (m_Sections.Count > 0)
            {
                return m_Sections[0].StartTime;
            }
            return 0.0f;
        }

        private float GetEndTime()
        {
            if (m_Sections.Count > 0)
            {
                return m_Sections[m_Sections.Count - 1].EndTime;
            }
            return 0.0f;
        }
    }

    public class ImpactLogicInfo
    {
        protected GameObject m_Sender;
        protected GameObject m_Target;
        protected int m_LogicId = 0;
        protected int m_ImpactId = 0;
        protected ImpactLogicData m_ConfigData;
        protected int m_ActionType = (int)Animation_Type.AT_Hurt0;
        protected float m_StartTime = 0.0f;
        protected float m_ElapsedTime = 0.0f;
        protected float m_ElapsedTimeForEffect = 0.0f;   // 不收动作缩放的影响。
        protected float m_Duration = 0.0f;
        protected bool m_IsActive = false;
        // Adjust info
        protected Vector3 m_AdjustPoint = new Vector3();
        protected float m_AdjustAppend;
        protected float m_AdjustDegreeXZ = 0;
        protected float m_AdJustDegreeY = 0;
        protected Vector3 m_NormalEndPoint;
        protected Vector3 m_OrignalPos;
        protected Vector3 m_NormalPos;
        protected Vector3 m_Velocity;
        protected Vector3 m_Accelerate;
        protected float m_MovingDelay = 0.0f;
        protected float m_MovingTime = 0.0f;
        protected Vector3 m_ImpactSrcPos;
        protected float m_ImpactSrcDir;
        protected TypedDataCollection m_CustomDatas = new TypedDataCollection();
        // 分段动作，定帧，位移信息
        protected CurveInfo m_AnimationInfo;
        protected CurveInfo m_LockFrameInfo;
        protected CurveMoveInfo m_MovementInfo;
        protected Quaternion m_MoveDir = Quaternion.identity;
        // 特效信息
        protected List<EffectInfo> m_EffectList = new List<EffectInfo>();
        protected List<GameObject> m_EffectsDelWithImpact = new List<GameObject>();

        public int LogicId
        {
            get { return m_LogicId; }
            set { m_LogicId = value; }
        }

        public UnityEngine.GameObject Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        public UnityEngine.GameObject Sender
        {
            get { return m_Sender; }
            set { m_Sender = value; }
        }

        public int ImpactId
        {
            get { return m_ImpactId; }
            set { m_ImpactId = value; }
        }

        public float StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        public Quaternion MoveDir
        {
            get { return m_MoveDir; }
            set { m_MoveDir = value; }
        }

        public float ElapsedTime
        {
            get { return m_ElapsedTime; }
            set { m_ElapsedTime = value; }
        }

        public float ElapsedTimeForEffect
        {
            get { return m_ElapsedTimeForEffect; }
            set { m_ElapsedTimeForEffect = value; }
        }

        public int ActionType
        {
            get { return m_ActionType; }
            set { m_ActionType = value; }
        }

        public TypedDataCollection CustomDatas
        {
            get { return m_CustomDatas; }
            set { m_CustomDatas = value; }
        }

        public UnityEngine.Vector3 ImpactSrcPos
        {
            get { return m_ImpactSrcPos; }
            set { m_ImpactSrcPos = value; }
        }

        public float ImpactSrcDir
        {
            get { return m_ImpactSrcDir; }
            set { m_ImpactSrcDir = value; }
        }

        public UnityEngine.Vector3 OrignalPos
        {
            get { return m_OrignalPos; }
            set { m_OrignalPos = value; }
        }

        public UnityEngine.Vector3 NormalPos
        {
            get { return m_NormalPos; }
            set { m_NormalPos = value; }
        }

        public UnityEngine.Vector3 NormalEndPoint
        {
            get { return m_NormalEndPoint; }
            set { m_NormalEndPoint = value; }
        }

        public float Duration
        {
            get { return m_Duration; }
            set { m_Duration = value; }
        }

        public DashFire.ImpactLogicData ConfigData
        {
            get { return m_ConfigData; }
            set { m_ConfigData = value; }
        }

        public Vector3 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public float AdjustDegreeY
        {
            get { return m_AdJustDegreeY; }
            set { m_AdJustDegreeY = value; }
        }

        public float AdjustDegreeXZ
        {
            get { return m_AdjustDegreeXZ; }
            set { m_AdjustDegreeXZ = value; }
        }

        public float AdjustAppend
        {
            get { return m_AdjustAppend; }
            set { m_AdjustAppend = value; }
        }

        public UnityEngine.Vector3 AdjustPoint
        {
            get { return m_AdjustPoint; }
            set { m_AdjustPoint = value; }
        }

        public CurveMoveInfo MovementInfo
        {
            get { return m_MovementInfo; }
            set { m_MovementInfo = value; }
        }

        public CurveInfo AnimationInfo
        {
            get { return m_AnimationInfo; }
            set { m_AnimationInfo = value; }
        }

        public CurveInfo LockFrameInfo
        {
            get { return m_LockFrameInfo; }
            set { m_LockFrameInfo = value; }
        }

        public List<EffectInfo> EffectList
        {
            get { return m_EffectList; }
            set { m_EffectList = value; }
        }

        public List<GameObject> EffectsDelWithImpact
        {
            get { return m_EffectsDelWithImpact; }
        }

        public void AddEffectData(int id)
        {
            EffectLogicData effectData = (EffectLogicData)SkillConfigProvider.Instance.ExtractData(SkillConfigType.SCT_EFFECT, id);
            if (null != effectData)
            {
                EffectInfo effectInfo = new EffectInfo();
                effectInfo.IsActive = false;
                effectInfo.Path = effectData.EffectPath;
                effectInfo.PlayTime = effectData.PlayTime;
                effectInfo.RelativePoint = ImpactUtility.ConvertVector3D(effectData.RelativePos);
                effectInfo.RotateWithTarget = effectData.RotateWithTarget;
                effectInfo.RelativeRotation = ImpactUtility.ConvertVector3D(effectData.RelativeRotation);
                effectInfo.MountPoint = effectData.MountPoint;
                effectInfo.DelayTime = effectData.EffectDelay;
                effectInfo.DelWithImpact = effectData.DelWithImpact;
                m_EffectList.Add(effectInfo);
            }
        }
    }
}
