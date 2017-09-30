using DashFire;
using GfxModule.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkillInput : MonoBehaviour {
    public KeyCode m_AttackKey = KeyCode.J;
    public KeyCode m_SkillAKey = KeyCode.I;
    public KeyCode m_SkillBKey = KeyCode.O;
    public KeyCode m_SkillCKey = KeyCode.K;
    public KeyCode m_SkillDKey = KeyCode.L;
    public KeyCode m_SkillQKey = KeyCode.Q;
    public KeyCode m_SkillEKey = KeyCode.E;
    public KeyCode m_SkillEX = KeyCode.Y;

    void Update()
    {
        if (gameObject != LogicSystem.PlayerSelf)
        {
            return;
        }

        if (Input.GetKeyDown(m_AttackKey))
        {
            GfxSkillSystem.Instance.StartAttack(gameObject, Vector3.zero);
        }
        if (Input.GetKeyUp(m_AttackKey))
        {
            GfxSkillSystem.Instance.StopAttack(gameObject);
        }
        if (Input.GetKeyUp(m_SkillAKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillA, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillBKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillB, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillCKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillC, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillDKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillD, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillQKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillQ, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillEKey))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kSkillE, Vector3.zero);
        }
        if (Input.GetKeyUp(m_SkillEX))
        {
            GfxSkillSystem.Instance.PushSkill(gameObject, SkillCategory.kEx, Vector3.zero);
        }
    }
}
