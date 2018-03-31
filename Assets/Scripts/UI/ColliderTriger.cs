using System;
using System.Collections.Generic;
using UnityEngine;

namespace GfxModule.Skill.Script
{
  public class ColliderTriger : MonoBehaviour
  {
    public void SetOnTriggerEnter(DashFire.MyAction<Collider> onEnter)
    {
      m_OnTrigerEnter += onEnter;
    }
    public void SetOnTriggerExit(DashFire.MyAction<Collider> onExit)
    {
      m_OnTrigerExit += onExit;
    }

    internal void OnTriggerEnter(Collider collider)
    {
      if (null != m_OnTrigerEnter)
        m_OnTrigerEnter(collider);
    }
    internal void OnTriggerExit(Collider collider)
    {
      if (null != m_OnTrigerExit)
        m_OnTrigerExit(collider);      
    }

    private DashFire.MyAction<Collider> m_OnTrigerEnter;
    private DashFire.MyAction<Collider> m_OnTrigerExit;
  }
}
