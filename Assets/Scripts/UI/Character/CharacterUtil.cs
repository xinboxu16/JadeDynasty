using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtil : MonoBehaviour {

    public GameObject m_MeetEnemyEffect;
    public string m_MeetEnemyEffectBone;

    public GameObject m_DeadEffect;
    public GameObject m_OnHitGroundEffect;

	public void OnEnable()
    {
        BoxCollider[] bcs = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach(BoxCollider bc in bcs)
        {
            bc.isTrigger = false;
        }
    }

    public void OnEventMeetEnemy()
    {
        if(null != m_MeetEnemyEffect && !string.IsNullOrEmpty(m_MeetEnemyEffectBone))
        {
            GameObject obj = ResourceSystem.NewObject(m_MeetEnemyEffect, 2.0f) as GameObject;
            Transform parent = LogicSystem.FindChildRecursive(this.transform, m_MeetEnemyEffectBone);
            if(null != parent)
            {
                obj.transform.parent = parent;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void OnEventDead()
    {
        if (null != m_DeadEffect)
        {
            GameObject obj = ResourceSystem.NewObject(m_DeadEffect, 2.0f) as GameObject;
            if (null != obj)
            {
                obj.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);
            }
        }
    }

    public void OnHitGround()
    {
        if (null != m_OnHitGroundEffect)
        {
            GameObject obj = ResourceSystem.NewObject(m_OnHitGroundEffect, 2.0f) as GameObject;
            if (null != obj)
            {
                obj.transform.position = this.transform.position;
            }
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log("OnControllerColliderHit" + hit.collider.gameObject);
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    Debug.Log("OnTriggerEnter" + collider.gameObject);
    //}

    //private void OnCollisionEnter(Collider collider)
    //{
    //    Debug.Log("OnCollisionEnter" + collider.gameObject);
    //}
}
