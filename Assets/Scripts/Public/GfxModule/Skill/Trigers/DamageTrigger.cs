using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GfxModule.Skill.Trigers
{
  public enum BeHitState
  {
    kDefault = 0,
    kStand = 1,
    kStiffness = 2,
    kLauncher = 3,
    kKnockDown = 4,
  }

  public class StateImpact
  {
    public BeHitState m_State;
    public List<ImpactData> m_Impacts = new List<ImpactData>();
  }

  public class ImpactData
  {
    public int ImpactId;
    public int RemainTime;
  }

  public class SkillDamageManager
  {
    public SkillDamageManager(GameObject owner)
    {
      m_Owner = owner;
    }

    public GameObject GetOwner()
    {
      return m_Owner;
    }

    public bool IsContainObject(GameObject obj)
    {
      foreach (GameObject item_obj in m_DamagedObjects) {
        if (item_obj == obj) {
          return true;
        }
      }
      return false;
    }

    public bool AddDamagedObject(GameObject obj)
    {
      if (!IsEnemy(m_Owner, obj)) {
        return false;
      }
      if (!IsContainObject(obj)) {
        m_IsDamagedEnemy = true;
        m_DamagedObjects.Add(obj);
        return true;
      }
      return false;
    }

    public void ClearDamagePoool()
    {
      m_DamagedObjects.Clear();
    }

    public void RemoveGameObject(GameObject obj)
    {
      m_DamagedObjects.Remove(obj);
    }

    public bool IsDamagedEnemy
    {
      get { return m_IsDamagedEnemy; }
      set { m_IsDamagedEnemy = value; }
    }

    public static bool IsEnemy(GameObject obj, GameObject other)
    {
      DashFire.SharedGameObjectInfo obj_info = DashFire.LogicSystem.GetSharedGameObjectInfo(obj);
      DashFire.SharedGameObjectInfo other_info = DashFire.LogicSystem.GetSharedGameObjectInfo(other);
      if (obj_info == null || other_info == null) {
        return false;
      }
      DashFire.CharacterRelation relation = DashFire.CharacterInfo.GetRelation(obj_info.CampId, other_info.CampId);
      if (relation == DashFire.CharacterRelation.RELATION_ENEMY) {
        return true;
      }
      return false;
    }

    public static BeHitState GetBeHitState(GameObject obj)
    {
      DashFire.SharedGameObjectInfo objinfo = DashFire.LogicSystem.GetSharedGameObjectInfo(obj);
      if (objinfo == null) {
        return BeHitState.kDefault;
      }
      switch (objinfo.GfxStateFlag) {
        case (int)DashFire.GfxCharacterState_Type.HitFly:
          return BeHitState.kLauncher;
        case (int)DashFire.GfxCharacterState_Type.KnockDown:
          return BeHitState.kKnockDown;
        case (int)DashFire.GfxCharacterState_Type.Stiffness:
          return BeHitState.kStiffness;
        default:
          return BeHitState.kStand;
      }
    }

    public void SendImpactToObject(GameObject source, GameObject target, Dictionary<BeHitState, StateImpact> stateimpacts, int skillid)
    {
      if (stateimpacts == null) {
        return;
      }
      if (IsUserStandUp(target)) {
        return;
      }
      BeHitState state = GetBeHitState(target);
      StateImpact stateimpact = null;
      if (stateimpacts.ContainsKey(state)) {
        stateimpact = stateimpacts[state];
      } else if (stateimpacts.ContainsKey(BeHitState.kDefault)) {
        stateimpact = stateimpacts[BeHitState.kDefault];
      }
      if (stateimpact == null) {
        return;
      }
      int final_skill_id = -1;
      GameObject damageOwner = TriggerUtil.GetFinalOwner(source, skillid, out final_skill_id);
      //Debug.Log("------------send impact to object " + target.name);
      foreach (ImpactData im in stateimpact.m_Impacts)
      {
          DashFire.LogicSystem.NotifyGfxHitTarget(damageOwner, im.ImpactId, target, 1, final_skill_id, im.RemainTime, source.transform.position, TriggerUtil.GetObjFaceDir(source));
      }
    }

    public void SendImpactToObject(GameObject source, GameObject target, int impactid, int remaintime, int skillid)
    {
      if (IsUserStandUp(target)) {
        return;
      }
      int final_skill_id = skillid;
      GameObject damageOwner = TriggerUtil.GetFinalOwner(source, skillid, out final_skill_id);

      DashFire.LogicSystem.NotifyGfxHitTarget(damageOwner, impactid, target, 1, final_skill_id, remaintime, source.transform.position, TriggerUtil.GetObjFaceDir(source));

    }

    public static bool IsUserStandUp(GameObject obj)
    {
      DashFire.SharedGameObjectInfo objinfo = DashFire.LogicSystem.GetSharedGameObjectInfo(obj);
      if (objinfo == null) {
        return false;
      }
      if (objinfo.IsPlayer && objinfo.GfxStateFlag == (int)DashFire.GfxCharacterState_Type.GetUp) {
        return true;
      }
      return false;
    }

    private List<GameObject> m_DamagedObjects = new List<GameObject>();
    private bool m_IsDamagedEnemy = false;
    private GameObject m_Owner;
  }

  /// <summary>
  /// areadamage(start_time,center_x, center_y, center_z, range, is_clear_when_finish[,impact_id,...]) {
  ///   showtip(time, color_r, color_g, color_b);
  ///   stateimpact(statename, impact_id[,impact_id...]); 
  /// };
  /// </summary>
  internal class AreaDamageTriger : AbstractSkillTriger
  {
    public override ISkillTriger Clone()
    {
      AreaDamageTriger triger = new AreaDamageTriger();
      triger.m_StartTime = m_StartTime;
      triger.m_RelativeCenter = m_RelativeCenter;
      triger.m_Range = m_Range;
      triger.m_ImpactList = m_ImpactList;
      triger.m_IsClearWhenFinish = m_IsClearWhenFinish;
      triger.m_IsShowTip = m_IsShowTip;
      triger.m_ShowTime = m_ShowTime;
      triger.m_Color = m_Color;
      foreach (StateImpact impact in m_StateImpacts.Values) {
        triger.m_StateImpacts[impact.m_State] = impact;
      }
      return triger;
    }
    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
        if (curSectionTime >= m_StartTime)
        {
            GameObject obj = sender as GameObject;
            if (null == obj)
            {
                return false;
            }
            if (!instance.IsDamageEnable)
            {
                return false;
            }
            Vector3 center = obj.transform.TransformPoint(m_RelativeCenter);
            Collider[] hits = Physics.OverlapSphere(center, m_Range, 1 << LayerMask.NameToLayer("Character"));
            SkillDamageManager damage_manager = instance.CustomDatas.GetData<SkillDamageManager>();
            if (damage_manager == null)
            {
                damage_manager = new SkillDamageManager(obj);
                instance.CustomDatas.AddData<SkillDamageManager>(damage_manager);
            }
            if (m_IsShowTip)
            {
                GameObject circle = TriggerUtil.DrawCircle(center, m_Range, m_Color);
                GameObject.Destroy(circle, m_ShowTime / 1000.0f);
            }
            foreach (Collider hit in hits)
            {
                if (!SkillDamageManager.IsEnemy(obj, hit.gameObject))
                {
                    continue;
                }
                if (SkillDamageManager.IsUserStandUp(hit.gameObject))
                {
                    continue;
                }

                //对敌人造成伤害
                if (!damage_manager.IsContainObject(hit.gameObject))
                {
                    damage_manager.IsDamagedEnemy = true;
                    //通知目标
                    damage_manager.SendImpactToObject(obj, hit.gameObject, m_StateImpacts, instance.SkillId);

                    if (!m_IsClearWhenFinish)
                    {
                        damage_manager.AddDamagedObject(hit.gameObject);
                    }
                }
            }

            if (damage_manager.IsDamagedEnemy)
            {
                instance.SendMessage("oncollide");
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    protected override void Load(ScriptableData.CallData callData)
    {
      int num = callData.GetParamNum();
      if (num >= 6) {
        m_StartTime = long.Parse(callData.GetParamId(0));
        m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
        m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
        m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
        m_Range = float.Parse(callData.GetParamId(4));
        m_IsClearWhenFinish = bool.Parse(callData.GetParamId(5));
      }
    }

    protected override void Load(ScriptableData.FunctionData funcData)
    {
      ScriptableData.CallData callData = funcData.Call;
      if (null != callData) {
        Load(callData);

        foreach (ScriptableData.ISyntaxComponent statement in funcData.Statements) {
          ScriptableData.CallData stCall = statement as ScriptableData.CallData;
          if (null != stCall) {
            string id = stCall.GetId();
            if (id == "stateimpact") {
              StateImpact stateimpact = TriggerUtil.ParseStateImpact(stCall);
              m_StateImpacts[stateimpact.m_State] = stateimpact;
            } else if (id == "showtip") {
              m_IsShowTip = true;
              m_ShowTime = long.Parse(stCall.GetParamId(0));
              if (stCall.GetParamNum() >= 4) {
                float r = float.Parse(stCall.GetParamId(1));
                float g = float.Parse(stCall.GetParamId(2));
                float b = float.Parse(stCall.GetParamId(3));
                m_Color = new Color(r, g, b, 1);
              }
            }
          }
        }
      }
    }

    private Vector3 m_RelativeCenter = Vector3.zero;
    private float m_Range = 0;
    private bool m_IsClearWhenFinish = false;
    private List<int> m_ImpactList = new List<int>();
    private Dictionary<BeHitState, StateImpact> m_StateImpacts = new Dictionary<BeHitState, StateImpact>();
    private bool m_IsShowTip = false;
    private long m_ShowTime = 0;
    private Color m_Color = new Color(0.5f, 1.0f, 0.5f);
  }
}
