using ScriptableData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Skill.Trigers
{
    public enum ColliderType
    {
        kSceneCollider,
        kBoneCollider,
        kSceneBoxCollider,
        kBoneBoxCollider
    }

    public class AttachConfig
    {
        public AttachConfig()
        {
            IsAttachEnemy = false;
            AttachNodeName = "";
            AttachRotation = new Vector3(0, 0, 0);
            AttachImpact = 0;
            AttachImpactTime = -1;
            FallImpact = 0;
            FallImpactTime = -1;
            FinalImpact = 0;
            FinalImpactTime = -1;
        }
        public bool IsAttachEnemy;
        public string AttachNodeName;
        public Vector3 AttachRotation;
        public int AttachImpact;
        public int AttachImpactTime;
        public int FallImpact;
        public int FallImpactTime;
        public int FinalImpact;
        public int FinalImpactTime;
    };

    internal sealed class ColliderTriggerInfo
    {
        private Dictionary<BeHitState, StateImpact> m_StateImpacts = new Dictionary<BeHitState, StateImpact>();

        private ColliderType m_ColliderType = ColliderType.kSceneCollider;
        private AttachConfig m_AttachConfig = new AttachConfig();
        private Dictionary<int, string> m_CollideLayerHandler = new Dictionary<int, string>();

        private Vector3 m_Size;
        private Vector3 m_Position;
        private string m_Bone = "";
        private bool m_IsAttach = false;
        private Quaternion m_Eular;
        private bool m_IsShow = false;
        private string m_Prefab = "";
        private GameObject m_Collider = null;

        public void Load(List<ScriptableData.ISyntaxComponent> statements)
        {
            foreach (ISyntaxComponent statement in statements)
            {
                ScriptableData.CallData stCall = statement as ScriptableData.CallData;
                if (null != stCall)
                {
                    if (stCall.GetId() == "stateimpact")
                    {
                        LoadStateImpactConfig(stCall);
                    }
                    else if (stCall.GetId() == "scenecollider")
                    {
                        LoadSceneColliderConfig(stCall);
                    }
                    else if (stCall.GetId() == "bonecollider")
                    {
                        LoadBoneColliderConfig(stCall);
                    }
                    else if (stCall.GetId() == "sceneboxcollider")
                    {
                        LoadSceneBoxColliderConfig(stCall);
                    }
                    else if (stCall.GetId() == "boneboxcollider")
                    {
                        LoadBoneBoxColliderConfig(stCall);
                    }
                    else if (stCall.GetId() == "oncollidelayer")
                    {
                        LoadCollideLayerConfig(stCall);
                    }
                    else if (stCall.GetId() == "attachenemy")
                    {
                        LoadAttachEnemyConfig(stCall);
                    }
                }
            }
        }

        private void LoadAttachEnemyConfig(ScriptableData.CallData stCall)
        {
            if (stCall.GetParamNum() >= 8)
            {
                m_AttachConfig.IsAttachEnemy = true;
                m_AttachConfig.AttachNodeName = stCall.GetParamId(0);
                m_AttachConfig.AttachRotation = ScriptableDataUtility.CalcVector3(stCall.GetParam(1) as ScriptableData.CallData);
                m_AttachConfig.AttachImpact = int.Parse(stCall.GetParamId(2));
                m_AttachConfig.AttachImpactTime = int.Parse(stCall.GetParamId(3));
                m_AttachConfig.FallImpact = int.Parse(stCall.GetParamId(4));
                m_AttachConfig.FallImpactTime = int.Parse(stCall.GetParamId(5));
                m_AttachConfig.FinalImpact = int.Parse(stCall.GetParamId(6));
                m_AttachConfig.FinalImpactTime = int.Parse(stCall.GetParamId(7));
            }
        }

        private void LoadCollideLayerConfig(ScriptableData.CallData stCall)
        {
            if (stCall.GetParamNum() >= 2)
            {
                int layer = LayerMask.NameToLayer(stCall.GetParamId(0));
                string message = stCall.GetParamId(1);
                if (!string.IsNullOrEmpty(message))
                {
                    if (m_CollideLayerHandler.ContainsKey(layer))
                    {
                        m_CollideLayerHandler[layer] = message;
                    }
                    else
                    {
                        m_CollideLayerHandler.Add(layer, message);
                    }
                }
            }
        }

        private void LoadSceneBoxColliderConfig(ScriptableData.CallData stCall)
        {
            m_ColliderType = ColliderType.kSceneBoxCollider;
            if (stCall.GetParamNum() >= 5)
            {
                m_Size = ScriptableDataUtility.CalcVector3(stCall.GetParam(0) as ScriptableData.CallData);
                m_Position = ScriptableDataUtility.CalcVector3(stCall.GetParam(1) as ScriptableData.CallData);
                m_Eular = ScriptableDataUtility.CalcEularRotation(stCall.GetParam(2) as ScriptableData.CallData);
                m_IsAttach = bool.Parse(stCall.GetParamId(3));
                m_IsShow = bool.Parse(stCall.GetParamId(4));
            }
        }

        private void LoadSceneColliderConfig(ScriptableData.CallData stCall)
        {
            if (stCall.GetParamNum() >= 2)
            {
                m_ColliderType = ColliderType.kSceneCollider;
                m_Prefab = stCall.GetParamId(0);
                ScriptableData.CallData param1 = stCall.GetParam(1) as ScriptableData.CallData;
                if (null != param1)
                    m_Position = ScriptableDataUtility.CalcVector3(param1);
            }
        }

        private void LoadBoneColliderConfig(ScriptableData.CallData stCall)
        {
            if (stCall.GetParamNum() >= 3)
            {
                m_ColliderType = ColliderType.kBoneCollider;
                m_Prefab = stCall.GetParamId(0);
                m_Bone = stCall.GetParamId(1);
                m_IsAttach = bool.Parse(stCall.GetParamId(2));
            }
        }

        private void LoadBoneBoxColliderConfig(ScriptableData.CallData stCall)
        {
            m_ColliderType = ColliderType.kBoneBoxCollider;
            if(stCall.GetParamNum() >= 6)
            {
                m_Size = ScriptableDataUtility.CalcVector3(stCall.GetParam(0) as CallData);
                m_Bone = stCall.GetParamId(1);
                m_Position = ScriptableDataUtility.CalcVector3(stCall.GetParam(2) as CallData);
                m_Eular = ScriptableDataUtility.CalcEularRotation(stCall.GetParam(3) as CallData);
                m_IsAttach = bool.Parse(stCall.GetParamId(4));
                m_IsShow = bool.Parse(stCall.GetParamId(5));
            }
        }

        private void LoadStateImpactConfig(ScriptableData.CallData stCall)
        {
            StateImpact stateimpact = TriggerUtil.ParseStateImpact(stCall);
            m_StateImpacts[stateimpact.m_State] = stateimpact;
        }

        public void CreateTriger(GameObject obj, float liveTime, object onTriggerEnter, object onTriggerExit, object onDestroy)
        {
            switch(m_ColliderType)
            {
                case ColliderType.kSceneCollider:
                case ColliderType.kBoneCollider:
                    CreateSceneOrBoneCollider(obj, liveTime, onTriggerEnter, onTriggerExit); 
                    break;
                case ColliderType.kSceneBoxCollider:
                case ColliderType.kBoneBoxCollider:
                    CreateBoxCollider(obj, liveTime, onTriggerEnter, onTriggerExit, onDestroy);
                    break;
            }
        }

        private void CreateSceneOrBoneCollider(GameObject obj, float liveTime, object onTriggerEnter, object onTriggerExit)
        {
            GameObject collider_obj = DashFire.ResourceSystem.NewObject(m_Prefab, liveTime) as GameObject;
            if (null == collider_obj)
            {
                Debug.LogError("------create collider failed! " + m_Prefab);
                return;
            }
            m_Collider = collider_obj;
            Transform[] transes = collider_obj.GetComponentsInChildren<Transform>();
            foreach (Transform child in transes)
            {
                child.gameObject.SendMessage("SetOnTriggerEnter", onTriggerEnter, SendMessageOptions.DontRequireReceiver);
                child.gameObject.SendMessage("SetOnTriggerExit", onTriggerExit, SendMessageOptions.DontRequireReceiver);
            }

            if (m_ColliderType == ColliderType.kSceneCollider)
            {
                Vector3 pos = obj.transform.position + obj.transform.rotation * m_Position;
                collider_obj.transform.position = pos;
            }
            else
            {
                Transform node = TriggerUtil.GetChildNodeByName(obj, m_Bone);
                if (node != null)
                {
                    collider_obj.transform.parent = node;
                    collider_obj.transform.localPosition = Vector3.zero;
                    collider_obj.transform.localRotation = Quaternion.identity;
                    if (!m_IsAttach)
                    {
                        collider_obj.transform.parent = null;
                    }
                }
            }
        }

        private void CreateBoxCollider(GameObject obj, float liveTime, object onTriggerEnter, object onTriggerExit, object onDestroy)
        {
            GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            collider.transform.localScale = m_Size;
            BoxCollider boxcollider = collider.GetComponent<BoxCollider>();
            if (boxcollider != null)
            {
                boxcollider.isTrigger = true;
            }
            collider.layer = LayerMask.NameToLayer("SceneObj");
            Rigidbody rigidbody = collider.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            ColliderScript cs = collider.AddComponent<ColliderScript>();
            if (cs != null)
            {
                cs.SetOnTriggerEnter((DashFire.MyAction<Collider>)onTriggerEnter);
                cs.SetOnTriggerExit((DashFire.MyAction<Collider>)onTriggerExit);
                cs.SetOnDestroy((DashFire.MyAction)(onDestroy));
            }
            if (!m_IsShow)
            {
                MeshRenderer mesh = collider.GetComponent<MeshRenderer>();
                if (mesh != null)
                {
                    GameObject.Destroy(mesh);
                }
            }
            if (m_ColliderType == ColliderType.kBoneBoxCollider)
            {
                Transform child = TriggerUtil.GetChildNodeByName(obj, m_Bone);
                if (child != null)
                {
                    collider.transform.parent = child;
                }
                else
                {
                    Debug.LogError("not find bone " + m_Bone);
                }
            }
            else
            {
                collider.transform.parent = obj.transform;
            }
            collider.transform.localPosition = m_Position;
            collider.transform.localRotation = m_Eular;
            if (!m_IsAttach)
            {
                collider.transform.parent = null;
            }
            m_Collider = collider;
            GameObject.Destroy(collider, liveTime);
        }

        public Dictionary<BeHitState, StateImpact> GetStateImpacts()
        {
            return m_StateImpacts;
        }

        public string GetCollideLayerMessage(int layer)
        {
            string message = "";
            m_CollideLayerHandler.TryGetValue(layer, out message);
            return message;
        }

        public AttachConfig GetAttachConfig()
        {
            return m_AttachConfig;
        }

        public GameObject GetCollider()
        {
            return m_Collider;
        }

        public ColliderTriggerInfo Clone()
        {
            ColliderTriggerInfo copy = new ColliderTriggerInfo();
            copy.m_ColliderType = m_ColliderType;
            copy.m_Prefab = m_Prefab;
            copy.m_Position = m_Position;
            copy.m_Bone = m_Bone;
            copy.m_IsAttach = m_IsAttach;
            copy.m_AttachConfig = m_AttachConfig;

            copy.m_Size = m_Size;
            copy.m_Eular = m_Eular;
            copy.m_IsShow = m_IsShow;
            copy.m_StateImpacts = m_StateImpacts;
            copy.m_CollideLayerHandler = m_CollideLayerHandler;
            m_Collider = null;
            return copy;
        }
    }
}
