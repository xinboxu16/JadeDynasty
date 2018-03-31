using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Skill
{
    public class ISkillProduct
    {
        public virtual void Tick(long deltaNs)
        {
        }

        public virtual bool IsStoped()
        {
            return true;
        }
    }

    public class ShadowProduct : ISkillProduct
    {
        private bool m_IsOver = false;
        private GameObject m_OrigObject;
        private GameObject m_ShadowObject;
        private string m_ShadowMaterial;
        private string m_ShaderName;
        private long m_StartTime;
        private long m_HoldTime;
        private long m_FadeOutTime;
        private float m_StartAlpha;
        private long m_CurTimeNs = 0;
        private List<String> m_IgnoreList = new List<String>();

        public ShadowProduct(GameObject origObj, string shadowMaterial, string shaderName, long holdTime, long fadeOutTime, float startAlpha, List<string> ignoreList)
        {
            m_IsOver = false;
            m_OrigObject = origObj;
            m_ShadowMaterial = shadowMaterial;
            m_ShaderName = shaderName;
            m_CurTimeNs = 0;
            m_HoldTime = holdTime;
            m_FadeOutTime = fadeOutTime;
            m_StartAlpha = startAlpha;
            m_IgnoreList.AddRange(ignoreList);

            Start(origObj);
        }

        public void Start(GameObject obj)
        {
            m_ShadowObject = ResourceSystem.NewObject(obj) as GameObject;
            if (m_ShadowObject == null)
            {
                m_IsOver = true;
                return;
            }
            m_ShadowObject.transform.position = obj.transform.position;
            m_ShadowObject.transform.rotation = obj.transform.rotation;

            Animation shadowAnimation = m_ShadowObject.GetComponent<Animation>();
            foreach(AnimationState state in obj.GetComponent<Animation>())
            {
                if(state.enabled && state.weight > 0)
                {
                    AnimationState shadowState = shadowAnimation[state.name];
                    shadowState.normalizedTime = state.normalizedTime;
                    shadowState.weight = state.weight;
                    shadowState.speed = 0;
                    shadowAnimation.Play(state.name);
                }
            }

            Renderer[] renders = m_ShadowObject.GetComponentsInChildren<Renderer>();
            Texture shadowTexture = Resources.Load(m_ShadowMaterial) as Texture;
            Shader shader = Shader.Find(m_ShaderName);
            foreach(Renderer r in renders)
            {
                if(r.gameObject != null && IsInIgnoreList(r.gameObject.name))
                {
                    r.enabled = false;
                    continue;
                }
                if(!r.enabled)
                {
                    continue;
                }
                if(shader != null)
                {
                    r.material.shader = shader;
                }
                r.material.mainTexture = shadowTexture;
                Color co = r.material.color;
                co.a = m_StartAlpha;
                r.material.color = co;
            }
        }

        public override void Tick(long deltaNs)
        {
            if (m_IsOver)
            {
                return;
            }
            m_CurTimeNs += deltaNs;
            if(m_HoldTime < GetCurTime())
            {
                long passTime = GetCurTime() - m_HoldTime;
                float t = passTime / (m_FadeOutTime * 1.0f);
                t = t > 1 ? 1 : t;
                float newAlpha = Mathf.Lerp(m_StartAlpha, 0, t);
                SetGameObjectAlpha(m_ShadowObject, newAlpha);
            }

            if (m_HoldTime + m_FadeOutTime < GetCurTime())
            {
                ResourceSystem.RecycleObject(m_ShadowObject);
                m_IsOver = true;
            }
        }

        public override bool IsStoped()
        {
            return m_IsOver;
        }

        private void SetGameObjectAlpha(GameObject obj, float alpha)
        {
            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renders)
            {
                if (r.gameObject != null && IsInIgnoreList(r.gameObject.name))
                {
                    continue;
                }
                if (r.enabled)
                {
                    Color oldColor = r.material.color;
                    oldColor.a = alpha;
                    r.material.color = oldColor;
                }
            }
        }

        private long GetCurTime()
        {
            return m_CurTimeNs / 1000;
        }

        private bool IsInIgnoreList(string partName)
        {
            foreach(string ignore in m_IgnoreList)
            {
                if(ignore.StartsWith(partName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
