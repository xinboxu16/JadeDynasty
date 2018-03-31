using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public class EffectObjectInfo
  {
    public GameObject TargetObj;
    public float StartEffectTime;
    public float NextEffectTime;
  }

  public class AttachTargetInfo
  {
    public GameObject ParentObj;
    public GameObject TargetObj;
    public Transform AttachNode;
    public Vector3 ParentPos;
    public Vector3 Rotate;
    public CharacterController MoveControler;

    public string ToString()
    {
      string str = "Parent:" + ParentObj.name + " Target:" + TargetObj.name +
        "AttachNodePos:" + AttachNode.position + " ParentPos:" + ParentPos;
      return str;
    }
  }

  /// <summary>
  /// colliderdamage(start_time, remain_time, is_clear_when_finish, is_always_enter_damage, damage_interval, max_damage_times)
  /// {
  ///   stateimpact("kDefault", 100101);
  ///   scenecollider("prefab",vector3(0,0,0));
  ///   bonecollider("prefab","bone", is_attach);
  /// };
  /// </summary>
  internal class ColliderDamageTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      ColliderDamageTriger copy = new ColliderDamageTriger();
      copy.m_StartTime = m_StartTime;
      copy.m_RemainTime = m_RemainTime;
      copy.m_IsClearWhenFinish = m_IsClearWhenFinish;
      copy.m_IsAlwaysEnterDamage = m_IsAlwaysEnterDamage;
      copy.m_DamageInterval = m_DamageInterval;
      copy.m_MaxDamageTimes = m_MaxDamageTimes;
      copy.m_ColliderInfo = m_ColliderInfo.Clone(); 
      copy.m_ColliderCreated = m_ColliderCreated;
      return copy;
    }
    public override void Reset()
    {
      SendFinalImpact();
      foreach (AttachTargetInfo ati in m_AttachedObjects) {
        DashFire.LogicSystem.NotifyGfxMoveControlFinish(ati.TargetObj, m_OwnSkill.SkillId, true);
      }
      ClearDamagedObjIfNeed();
      m_LeaveDelObjects.Clear();
      m_MoreTimesEffectObjects.Clear();
      m_AttachedObjects.Clear();
      m_ColliderCreated = false;
      m_Owner = null;
      m_OwnSkill = null;
      m_DamageManager = null;
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
      if (curSectionTime < m_StartTime) {
        return true;
      }

      GameObject obj = sender as GameObject;
      if (null == obj) {
        return false;
      }
      if (curSectionTime > (m_StartTime + m_RemainTime)) {
        ClearDamagedObjIfNeed();
        return false;
      }

      if (!m_ColliderCreated) {
        m_ColliderCreated = true;
        m_Owner = obj;
        m_OwnSkill = instance;
        m_DamageManager = instance.CustomDatas.GetData<SkillDamageManager>();
        if (m_DamageManager == null) {
          m_DamageManager = new SkillDamageManager(obj);
          instance.CustomDatas.AddData<SkillDamageManager>(m_DamageManager);
        }
        m_ColliderInfo.CreateTriger(obj, m_RemainTime / 1000.0f,
                                    (object)(DashFire.MyAction<Collider>)this.OnTriggerEnter,
                                    (object)(DashFire.MyAction<Collider>)this.OnTriggerExit,
                                    (object)(DashFire.MyAction)this.OnDestroy);
      }

      if (!instance.IsDamageEnable) {
        return true;
      }
      UpdateMoreTimeEffectObjects(instance);
      UpdateAttachObjects();
      return true;
    }

    private void UpdateMoreTimeEffectObjects(SkillInstance instance)
    {
      float now = instance.CurTime / 1000.0f;
      foreach (EffectObjectInfo obj_info in m_MoreTimesEffectObjects) {
        if (obj_info.NextEffectTime <= now) {
          //Debug.Log("-----------more time damage obj " + obj_info.TargetObj.name + " time=" + obj_info.NextEffectTime);
          obj_info.NextEffectTime += m_DamageInterval / 1000.0f;
          m_DamageManager.SendImpactToObject(m_DamageManager.GetOwner(), obj_info.TargetObj, m_ColliderInfo.GetStateImpacts(), instance.SkillId);
        }
      }
    }

    private void UpdateAttachObjects()
    {
      AttachConfig attach_config = m_ColliderInfo.GetAttachConfig();
      for (int i = m_AttachedObjects.Count - 1; i >= 0; --i) {
        AttachTargetInfo ati = m_AttachedObjects[i];
        if (ati.ParentObj != null) {
          if (!UpdateAttachTargetPos(ati)) {
            m_AttachedObjects.RemoveAt(i);
            //Debug.Log("---FallImpact: send to " + ati.TargetObj.name);
            m_DamageManager.SendImpactToObject(m_DamageManager.GetOwner(),
                                               ati.TargetObj, attach_config.FallImpact, attach_config.FallImpactTime,
                                               m_OwnSkill.SkillId);
          }
        }
      }
    }

    protected override void Load(ScriptableData.FunctionData funcData)
    {
      ScriptableData.CallData callData = funcData.Call;
      if (null == callData) {
        return;
      }
      int num = callData.GetParamNum();
      if (num >= 6) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_RemainTime = long.Parse(callData.GetParamId(1));
        m_IsClearWhenFinish = bool.Parse(callData.GetParamId(2));
        m_IsAlwaysEnterDamage = bool.Parse(callData.GetParamId(3));
        m_DamageInterval = long.Parse(callData.GetParamId(4));
        m_MaxDamageTimes = int.Parse(callData.GetParamId(5));
      }
      //碰撞体数据
      m_ColliderInfo = new ColliderTriggerInfo();
      m_ColliderInfo.Load(funcData.Statements);
    }

    public void OnTriggerEnter(Collider collider)
    {
      if (m_DamageManager == null) {
        return;
      }
      string message = m_ColliderInfo.GetCollideLayerMessage(collider.gameObject.layer);
      if (!string.IsNullOrEmpty(message)) {
        m_OwnSkill.SendMessage(message);
      }
      if (!m_OwnSkill.IsDamageEnable) {
        return;
      }

      if (SkillDamageManager.IsUserStandUp(collider.gameObject)) {
        return;
      }

      if (m_DamageManager.AddDamagedObject(collider.gameObject)) {
        if (m_ColliderInfo.GetAttachConfig().IsAttachEnemy) {
          AddAttachObject(collider);
        } else {
          m_DamageManager.SendImpactToObject(m_DamageManager.GetOwner(), collider.gameObject, m_ColliderInfo.GetStateImpacts(), m_OwnSkill.SkillId);
          if (m_DamageInterval > 0) {
            //Debug.Log("----------add more times obj " + collider.gameObject.name + " time=" + m_OwnSkill.CurTime);
            AddMoreTimeEffectObject(collider.gameObject);
          }
          if (m_IsAlwaysEnterDamage) {
            m_LeaveDelObjects.Add(collider.gameObject);
          }
          m_DamagedObjects.Add(collider.gameObject);
        }
      }
      if (m_DamageManager.IsDamagedEnemy) {
        m_OwnSkill.SendMessage("oncollide");
      }
    }

    public void OnTriggerExit(Collider collider)
    {
      if (m_IsAlwaysEnterDamage) {
        m_LeaveDelObjects.Remove(collider.gameObject);
        if (m_DamageManager != null) {
          m_DamageManager.RemoveGameObject(collider.gameObject);
        }
      }
      if (m_DamageInterval > 0) {
        RemoveMoreTimeEffectObject(collider.gameObject);
      }
    }

    public void OnDestroy()
    {
    }

    public void AddMoreTimeEffectObject(GameObject obj)
    {
      if (FindEffectInfoByObj(obj) != null) {
        return;
      }
      EffectObjectInfo effectinfo = new EffectObjectInfo();
      effectinfo.TargetObj = obj;
      effectinfo.StartEffectTime = m_OwnSkill.CurTime / 1000.0f;
      effectinfo.NextEffectTime = m_OwnSkill.CurTime / 1000.0f + m_DamageInterval / 1000.0f;
      m_MoreTimesEffectObjects.Add(effectinfo);
    }

    public void RemoveMoreTimeEffectObject(GameObject obj)
    {
      EffectObjectInfo ei = FindEffectInfoByObj(obj);
      if (ei != null) {
        m_MoreTimesEffectObjects.Remove(ei);
      }
    }

    public EffectObjectInfo FindEffectInfoByObj(GameObject obj)
    {
      foreach (EffectObjectInfo ei in m_MoreTimesEffectObjects) {
        if (ei.TargetObj == obj) {
          return ei;
        }
      }
      return null;
    }

    private void AddAttachObject(Collider collider)
    {
      GameObject parent = m_ColliderInfo.GetCollider();
      if (parent == null) {
        return;
      }
      AttachConfig attach_config = m_ColliderInfo.GetAttachConfig();
      AttachTargetInfo attach_info = new AttachTargetInfo();
      attach_info.ParentObj = parent;
      attach_info.TargetObj = collider.gameObject;
      attach_info.AttachNode = TriggerUtil.GetChildNodeByName(collider.gameObject,
                                                              attach_config.AttachNodeName);

      Vector3 hit_pos = parent.GetComponent<Collider>().ClosestPointOnBounds(attach_info.AttachNode.position);
      attach_info.ParentPos = attach_info.ParentObj.transform.InverseTransformPoint(hit_pos);
      attach_info.Rotate = attach_config.AttachRotation;
      attach_info.MoveControler = attach_info.TargetObj.GetComponent<CharacterController>();
      m_AttachedObjects.Add(attach_info);
      UpdateAttachTargetPos(attach_info);
      DashFire.LogicSystem.NotifyGfxMoveControlStart(attach_info.TargetObj, m_OwnSkill.SkillId, true);
      //Debug.Log("---AttachImpact: send " + attach_config.AttachImpact + " to " + attach_info.TargetObj.name);
      m_DamageManager.SendImpactToObject(m_DamageManager.GetOwner(),
                                         collider.gameObject, attach_config.AttachImpact, 
                                         attach_config.AttachImpactTime,
                                         m_OwnSkill.SkillId);
    }

    private bool UpdateAttachTargetPos(AttachTargetInfo ati)
    {
      if (ati.AttachNode == null) {
        return false;
      }
      ati.TargetObj.transform.rotation = ati.ParentObj.transform.rotation;
      ati.TargetObj.transform.Rotate(ati.Rotate);
      Vector3 relative_motion = (ati.TargetObj.transform.position - ati.AttachNode.position);
      Vector3 target_pos = ati.ParentObj.transform.TransformPoint(ati.ParentPos) + relative_motion;
      CollisionFlags flag;
      if (ati.MoveControler != null) {
        flag = ati.MoveControler.Move(target_pos - ati.TargetObj.transform.position);
      } else {
        return false;
      }
      Vector3 cur_pos = ati.TargetObj.transform.position;
      //if (/*Math.Abs(cur_pos.x - target_pos.x) <= 0.5 && Math.Abs(cur_pos.z - target_pos.z) <= 0.5*/) {
      if ((flag & CollisionFlags.CollidedSides) <= 0) {
        ati.TargetObj.transform.position = target_pos;
        return true;
      } else {
        //Debug.Log("----can't move to " + target_pos + "  just move to " + cur_pos);
        return false;
      }
    }

    private void ClearDamagedObjIfNeed()
    {
      if (m_IsClearWhenFinish) {
        foreach (GameObject damaged_obj in m_DamagedObjects) {
          m_DamageManager.RemoveGameObject(damaged_obj);
        }
      }
      m_DamagedObjects.Clear();
    }

    private void SendFinalImpact()
    {
      AttachConfig attach_config = m_ColliderInfo.GetAttachConfig();
      for (int i = m_AttachedObjects.Count - 1; i >= 0; --i) {
        AttachTargetInfo ati = m_AttachedObjects[i];
        if (ati.ParentObj != null) {
          //Debug.Log("---FinalImpact: send " + attach_config.FinalImpact + " to " + ati.TargetObj.name);
          m_DamageManager.SendImpactToObject(m_DamageManager.GetOwner(),
                                              ati.TargetObj, attach_config.FinalImpact, 
                                              attach_config.FinalImpactTime,
                                              m_OwnSkill.SkillId);
        }
      }
    }

    private long m_RemainTime = 0;
    private bool m_IsClearWhenFinish = false;
    private bool m_IsAlwaysEnterDamage = false;
    private long m_DamageInterval = 0;
    private int m_MaxDamageTimes = 0;
    private ColliderTriggerInfo m_ColliderInfo;

    private bool m_ColliderCreated = false;
    private SkillInstance m_OwnSkill = null;
    private GameObject m_Owner = null;
    private SkillDamageManager m_DamageManager = null;
    private List<EffectObjectInfo> m_MoreTimesEffectObjects = new List<EffectObjectInfo>();
    private List<AttachTargetInfo> m_AttachedObjects = new List<AttachTargetInfo>();
    private List<GameObject> m_LeaveDelObjects = new List<GameObject>();
    private List<GameObject> m_DamagedObjects = new List<GameObject>();
  }
}
