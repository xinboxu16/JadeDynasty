using System;
using System.Collections.Generic;
using StorySystem;
using DashFire;
using UnityEngine;

namespace DashFire.Story.Values
{
  internal sealed class UnitId2ObjIdValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "unitid2objid" && callData.GetParamNum() == 1) {
        m_UnitId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      UnitId2ObjIdValue val = new UnitId2ObjIdValue();
      val.m_UnitId = m_UnitId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_UnitId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_UnitId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_UnitId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_UnitId.HaveValue) {
        int unitId = m_UnitId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterByUnitId(unitId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetId();
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_UnitId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class ObjId2UnitIdValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "objid2unitid" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      ObjId2UnitIdValue val = new ObjId2UnitIdValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetUnitId();
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetPositionValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getposition" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetPositionValue val = new GetPositionValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetMovementStateInfo().GetPosition3D();
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetPositionXValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getpositionx" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetPositionXValue val = new GetPositionXValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetMovementStateInfo().PositionX;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetPositionYValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getpositionx" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetPositionYValue val = new GetPositionYValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetMovementStateInfo().PositionY;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetPositionZValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getpositionz" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetPositionZValue val = new GetPositionZValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetMovementStateInfo().PositionZ;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetCampValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getcamp" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetCampValue val = new GetCampValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetCampId();
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class IsEnemyValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "isenemy" && callData.GetParamNum() == 2) {
        m_Camp1.InitFromDsl(callData.GetParam(0));
        m_Camp2.InitFromDsl(callData.GetParam(1));
      }
    }
    public IStoryValue<object> Clone()
    {
      IsEnemyValue val = new IsEnemyValue();
      val.m_Camp1 = m_Camp1.Clone();
      val.m_Camp2 = m_Camp2.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_Camp1.Evaluate(iterator, args);
      m_Camp2.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_Camp1.Evaluate(instance);
      m_Camp2.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_Camp1.Analyze(instance);
      m_Camp2.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_Camp1.HaveValue && m_Camp2.HaveValue) {
        int camp1 = m_Camp1.Value;
        int camp2 = m_Camp2.Value;
        m_HaveValue = true;
        m_Value = (CharacterInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_ENEMY ? 1 : 0);
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_Camp1 = new StoryValue<int>();
    private IStoryValue<int> m_Camp2 = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class IsFriendValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "isfriend" && callData.GetParamNum() == 2) {
        m_Camp1.InitFromDsl(callData.GetParam(0));
        m_Camp2.InitFromDsl(callData.GetParam(1));
      }
    }
    public IStoryValue<object> Clone()
    {
      IsFriendValue val = new IsFriendValue();
      val.m_Camp1 = m_Camp1.Clone();
      val.m_Camp2 = m_Camp2.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_Camp1.Evaluate(iterator, args);
      m_Camp2.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_Camp1.Evaluate(instance);
      m_Camp2.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_Camp1.Analyze(instance);
      m_Camp2.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_Camp1.HaveValue && m_Camp2.HaveValue) {
        int camp1 = m_Camp1.Value;
        int camp2 = m_Camp2.Value;
        m_HaveValue = true;
        m_Value = (CharacterInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_FRIEND ? 1 : 0);
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_Camp1 = new StoryValue<int>();
    private IStoryValue<int> m_Camp2 = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetHpValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "gethp" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetHpValue val = new GetHpValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.Hp;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetEnergyValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getenergy" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetEnergyValue val = new GetEnergyValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.Energy;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetRageValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getrage" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetRageValue val = new GetRageValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.Rage;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetMaxHpValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getmaxhp" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetMaxHpValue val = new GetMaxHpValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetActualProperty().HpMax;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetMaxEnergyValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getmaxenergy" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetMaxEnergyValue val = new GetMaxEnergyValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetActualProperty().EnergyMax;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class GetMaxRageValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "getmaxrage" && callData.GetParamNum() == 1) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
      }
    }
    public IStoryValue<object> Clone()
    {
      GetMaxRageValue val = new GetMaxRageValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue) {
        int objId = m_ObjId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        if (null != obj) {
          m_HaveValue = true;
          m_Value = obj.GetActualProperty().RageMax;
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
  internal sealed class CalcDirValue : IStoryValue<object>
  {
    public void InitFromDsl(ScriptableData.ISyntaxComponent param)
    {
      ScriptableData.CallData callData = param as ScriptableData.CallData;
      if (null != callData && callData.GetId() == "calcdir" && callData.GetParamNum() == 2) {
        m_ObjId.InitFromDsl(callData.GetParam(0));
        m_TargetId.InitFromDsl(callData.GetParam(1));
      }
    }
    public IStoryValue<object> Clone()
    {
      CalcDirValue val = new CalcDirValue();
      val.m_ObjId = m_ObjId.Clone();
      val.m_TargetId = m_TargetId.Clone();
      val.m_HaveValue = m_HaveValue;
      val.m_Value = m_Value;
      return val;
    }
    public void Evaluate(object iterator, object[] args)
    {
      m_Iterator = iterator;
      m_Args = args;
      m_ObjId.Evaluate(iterator, args);
      m_TargetId.Evaluate(iterator, args);
    }
    public void Evaluate(StoryInstance instance)
    {
      m_ObjId.Evaluate(instance);
      m_TargetId.Evaluate(instance);
      TryUpdateValue(instance);
    }
    public void Analyze(StoryInstance instance)
    {
      m_ObjId.Analyze(instance);
      m_TargetId.Analyze(instance);
    }
    public bool HaveValue
    {
      get
      {
        return m_HaveValue;
      }
    }
    public object Value
    {
      get
      {
        return m_Value;
      }
    }

    private void TryUpdateValue(StoryInstance instance)
    {
      if (m_ObjId.HaveValue && m_TargetId.HaveValue) {
        int objId = m_ObjId.Value;
        int targetId = m_TargetId.Value;
        CharacterInfo obj = WorldSystem.Instance.GetCharacterById(objId);
        CharacterInfo target = WorldSystem.Instance.GetCharacterById(targetId);
        if (null != obj && null != target) {
          m_HaveValue = true;
          Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
          Vector2 targetPos = obj.GetMovementStateInfo().GetPosition2D();
          m_Value = Geometry.GetYAngle(srcPos, targetPos);
        }
      }
    }

    private object m_Iterator = null;
    private object[] m_Args = null;

    private IStoryValue<int> m_ObjId = new StoryValue<int>();
    private IStoryValue<int> m_TargetId = new StoryValue<int>();
    private bool m_HaveValue;
    private object m_Value;
  }
}
