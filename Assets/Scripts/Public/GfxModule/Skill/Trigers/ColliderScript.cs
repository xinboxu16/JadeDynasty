using UnityEngine;

public class ColliderScript : MonoBehaviour
{
  public void SetOnTriggerEnter(DashFire.MyAction<Collider> onEnter)
  {
    m_OnTrigerEnter += onEnter;
  }
  public void SetOnTriggerExit(DashFire.MyAction<Collider> onExit)
  {
    m_OnTrigerExit += onExit;
  }

  public void SetOnDestroy(DashFire.MyAction onDestroy)
  {
    m_OnDestroy += onDestroy;
  }

  void OnDestroy()
  {
    if (m_OnDestroy != null) {
      m_OnDestroy();
    }
  }

  internal void OnTriggerEnter(Collider collider)
  {
    if (null != m_OnTrigerEnter) {
      m_OnTrigerEnter(collider);
    }
  }
  internal void OnTriggerExit(Collider collider)
  {
    if (null != m_OnTrigerExit) {
      m_OnTrigerExit(collider);
    }
  }

  private DashFire.MyAction<Collider> m_OnTrigerEnter;
  private DashFire.MyAction<Collider> m_OnTrigerExit;
  private DashFire.MyAction m_OnDestroy;
}
