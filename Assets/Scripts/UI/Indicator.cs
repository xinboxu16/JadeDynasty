using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    private List<string> m_TriggeredSign = new List<string>();
    private List<GameObject> m_RoadSignObject = new List<GameObject>();

    public string[] m_RoadSign = { "BDoor", "CDoor" };

    private GameObject m_Owner;
    private float m_Dir;
    private bool m_HideIndicator = false;
    private IndicatorType m_IndicatorTargetType = IndicatorType.NPC;

    public float m_InvisibleDis = 3.0f;

    private enum IndicatorType
    {
        ROAD_SING = 0,
        NPC = 1,
    }

	// Use this for initialization
	void Start () {
        m_TriggeredSign.Clear();
        for(int i = 0; i < m_RoadSign.Length; i++)
        {
            GameObject roadSign = GameObject.Find(m_RoadSign[i]);
            if(null != roadSign)
            {
                m_RoadSignObject.Add(roadSign);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (null != m_Owner && !m_HideIndicator)
        {
            this.transform.localPosition = m_Owner.transform.position;
        }
        if(IndicatorType.NPC == m_IndicatorTargetType)
        {
            this.transform.localRotation = Quaternion.Euler(0, LogicSystem.RadianToDegree(m_Dir), 0);
        }else if(IndicatorType.ROAD_SING == m_IndicatorTargetType)
        {
            GameObject roadSign = GetRoadSign();
            if(null != roadSign)
            {
                Vector3 scrPos = this.transform.position;
                Vector3 tarPos = roadSign.transform.position;

                if (Vector2.Distance(new Vector2(scrPos.x, scrPos.z), new Vector2(tarPos.x, tarPos.z)) < m_InvisibleDis)
                {
                    SetVisible(false);
                }
                else
                {
                    SetVisible(true);
                    Vector3 dir = roadSign.transform.position - this.transform.position;
                    dir.y = 0;
                    this.transform.localRotation = Quaternion.LookRotation(dir, Vector3.up);
                }
            }
            else
            {
                SetVisible(false);
            }
        }
        foreach (GameObject roadSign in m_RoadSignObject)
        {
            BoxCollider bc = roadSign.GetComponent<BoxCollider>();
            if (null != bc)
            {
                if (bc.bounds.Contains(this.transform.position + new Vector3(0, 1, 0)))
                {
                    m_TriggeredSign.Add(roadSign.name);
                }
            }
        }
	}

    private GameObject GetRoadSign()
    {
        for(int i = 0; i < m_RoadSign.Length; ++i)
        {
            GameObject roadSign = GameObject.Find(m_RoadSign[i]);
            if (null != roadSign)
            {
                BoxCollider bc = roadSign.GetComponent<BoxCollider>();
                if (null != bc && bc.isTrigger && !m_TriggeredSign.Contains(roadSign.name))
                {
                    return roadSign;
                }
            }
        }
        return null;
    }

    public void HideIndicator()
    {
        gameObject.transform.localPosition = Vector3.zero;
        m_HideIndicator = true;
    }

    public void ShowIndicator()
    {
        if (null != m_Owner)
        {
            this.transform.localPosition = m_Owner.transform.position;
        }
        m_HideIndicator = false;
    }

    private void  SetVisible(bool visible)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for(int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].enabled = visible;
        }
    }

    public void SetIndicatorDir(float dir)
    {
        m_Dir = dir;
    }

    public void SetOwner(int id)
    {
        m_Owner = LogicSystem.GetGameObject(id);
    }

    public void SetIndicatorTarget(int targetType)
    {
        m_IndicatorTargetType = (IndicatorType)targetType;
    }
}
