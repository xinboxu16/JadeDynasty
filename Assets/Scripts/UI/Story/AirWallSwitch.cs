using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AirWallSwitch : MonoBehaviour {
    public GameObject m_DistroyEffect;
    private BoxCollider m_BoxCollider;

	// Use this for initialization
	void Start () {
        m_BoxCollider = gameObject.GetComponent<BoxCollider>();
        if(!m_BoxCollider.isTrigger)
        {
            EnableParticlas();
        }
        else
        {
            DisableParticals();
        }
	}

    public void OpenDoor()
    {
        m_BoxCollider.isTrigger = false;
        EnableParticlas();
    }

    public void CloseDoor()
    {
        m_BoxCollider.isTrigger = true;
        DisableParticals();
    }
	
	// Update is called once per frame
	public void EnableParticlas()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            ps.Play();
        }
    }

    private void DisableParticals()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            ps.Stop();
        }
        if (null != m_DistroyEffect)
        {
            ParticleSystem[] deadPss = m_DistroyEffect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in deadPss)
            {
                ps.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("ontriggerenter");
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("ontriggerexit");
    }
}
