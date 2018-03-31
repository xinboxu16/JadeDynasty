using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GfxModule.Skill.Trigers
{
    public class TriggerUtil
    {
        private static float m_RayCastMaxDistance = 50;
        private static int m_TerrainLayer = 1 << 16;

        public static void OnFingerDown(GestureArgs e)
        {
            if(LogicSystem.PlayerSelfInfo != null)
            {
                LogicSystem.PlayerSelfInfo.IsTouchDown = true;
            }
        }

        public static void OnFingerUp(DashFire.GestureArgs e)
        {
            if (LogicSystem.PlayerSelfInfo != null)
            {
                LogicSystem.PlayerSelfInfo.IsTouchDown = false;
            }
        }

        public static bool AttachNodeToNode(GameObject source, string sourcenode, GameObject target, string targetnode)
        {
            Transform source_child = GetChildNodeByName(source, sourcenode);
            Transform target_child = GetChildNodeByName(target, targetnode);
            if (source_child == null || target_child == null)
            {
                return false;
            }
            target.transform.parent = source_child;
            target.transform.localRotation = Quaternion.identity;
            target.transform.localPosition = Vector3.zero;
            Vector3 ss = source_child.localScale;
            Vector3 scale = new Vector3(1 / ss.x, 1 / ss.y, 1 / ss.z);
            Vector3 relative_motion = (target_child.position - target.transform.position);
            target.transform.position -= relative_motion;
            //target.transform.localPosition = Vector3.Scale(target.transform.localPosition, scale);
            return true;
        }

        public static void MoveChildToNode(int actorId, string childName, string nodeName)
        {
            GameObject obj = LogicSystem.GetGameObject(actorId);
            MoveChildToNode(obj, childName, nodeName);
        }

        public static void MoveChildToNode(GameObject obj, string childname, string nodename)
        {
            Transform child = GetChildNodeByName(obj, childname);
            if (child == null)
            {
                LogSystem.Debug("----not find child! {0} on {1}", childname, obj.name);
                return;
            }

            Transform toggleNode = TriggerUtil.GetChildNodeByName(obj, nodename);
            if (toggleNode == null)
            {
                DashFire.LogSystem.Debug("----not find node! {0} on {1}", nodename, obj.name);
                return;
            }

            child.parent = toggleNode;
            child.localRotation = Quaternion.identity;
            child.localPosition = Vector3.zero;
        }

        public static Transform GetChildNodeByName(GameObject gameobj, string name)
        {
            if (gameobj == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Transform[] ts = gameobj.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts)
            {
                if (t.name == name)
                {
                    return t;
                }
            }
            return null;
        }

        public static List<GameObject> FindTargetInSector(Vector3 center, float radius, Vector3 direction, Vector3 degreeCenter, float degree)
        {
            List<GameObject> result = new List<GameObject>();
            //将会返回以参数1为原点和参数2为半径的球体内“满足一定条件”的碰撞体集合，此时我们把这个球体称为 3D相交球
            Collider[] colliders = Physics.OverlapSphere(center, radius, 1 << LayerMask.NameToLayer("Character"));
            direction.y = 0;
            foreach(Collider co in colliders)
            {
                GameObject obj = co.gameObject;
                Vector3 targetDir = obj.transform.position - degreeCenter;
                targetDir.y = 0;
                if(Mathf.Abs(Vector3.Angle(targetDir, direction)) <= degree)
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        //发现敌人
        public static List<GameObject> FiltEnimy(GameObject source, List<GameObject> list)
        {
            List<GameObject> result = new List<GameObject>();
            foreach(GameObject obj in list)
            {
                if(SkillDamageManager.IsEnemy(source, obj) && !IsObjectDead(obj))
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        public static bool IsObjectDead(GameObject obj)
        {
            SharedGameObjectInfo si = DashFire.LogicSystem.GetSharedGameObjectInfo(obj);
            if(si.Blood <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //max_distance指半径
        public static GameObject GetObjectByPriority(GameObject source, List<GameObject> list, float distance_priority, float degree_priority, float max_distance, float max_degree)
        {
            GameObject target = null;
            float maxScore = -1;
            foreach(GameObject obj in list)
            {
                Vector3 targetDir = obj.transform.position - source.transform.position;

                float distance = targetDir.magnitude;
                float diatanceScore = 1 - distance / max_distance;

                float angle = Vector3.Angle(targetDir, source.transform.position);
                float degreeScore = 1 - angle / max_degree;

                float finalScore = diatanceScore * distance_priority + degreeScore * degree_priority;
                if(finalScore > maxScore)
                {
                    target = obj;
                    maxScore = finalScore;
                }
            }
            return target;
        }

        public static float ConvertToSecond(long delta)
        {
            return delta / 1000000.0f;
        }

        public static void MoveObjTo(GameObject obj, Vector3 position)
        {
            CharacterController ctrl = obj.GetComponent<CharacterController>();
            if (null != ctrl)
            {
                ctrl.Move(position - obj.transform.position);
            }
            else
            {
                obj.transform.position = position;
            }
        }

        public static void UpdateObjPosition(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            DashFire.LogicSystem.NotifyGfxUpdatePosition(obj, obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
        }

        public static void SetObjVisible(GameObject obj, bool isShow)
        {
            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renders)
            {
                r.enabled = isShow;
            }
        }

        public static float GetObjFaceDir(GameObject obj)
        {
            return obj.transform.rotation.eulerAngles.y * UnityEngine.Mathf.PI / 180.0f;
        }

        public static GameObject DrawCircle(Vector3 center, float radius, Color color, float circle_step = 0.05f)
        {
            GameObject obj = new GameObject();
            LineRenderer lineRender = obj.AddComponent<LineRenderer>();
            lineRender.startWidth = 0.05f;
            lineRender.endWidth = 0.05f;

            Shader shader = Shader.Find("Particles/Additive");
            if(shader != null)
            {
                lineRender.material = new Material(shader);
            }
            lineRender.startColor = color;
            lineRender.endColor = color;

            //每步度数
            float stepDegree = Mathf.Atan(circle_step / 2) * 2;
            int count = (int)(2 * Mathf.PI / stepDegree);

            //设置线段长度，这个数值须要和绘制线3D点的数量相等 
            //这里是设置圆的点数，加1是因为加了一个终点（起点）
            lineRender.numPositions = count + 1;

            for (int i = 0; i < count + 1; i++)
            {
                float angle = 2 * Mathf.PI / count * i;
                Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                lineRender.SetPosition(i, pos);
            }
            return obj;
        }

        //召唤 TODO 不懂
        public static GameObject GetFinalOwner(GameObject source, int skillid, out int final_skillid)
        {
            DashFire.SharedGameObjectInfo result = null;
            DashFire.SharedGameObjectInfo si = DashFire.LogicSystem.GetSharedGameObjectInfo(source);
            final_skillid = skillid;
            int break_protector = 10000;
            while (si != null)
            {
                result = si;
                if (si.SummonOwnerActorId >= 0)
                {
                    final_skillid = si.SummonOwnerSkillId;
                    si = DashFire.LogicSystem.GetSharedGameObjectInfo(si.SummonOwnerActorId);
                }
                else
                {
                    break;
                }
                if (break_protector-- <= 0)
                {
                    break;
                }
            }
            if (result != null)
            {
                return DashFire.LogicSystem.GetGameObject(result.m_ActorId);
            }
            else
            {
                return source;
            }
        }

        public static StateImpact ParseStateImpact(ScriptableData.CallData stCall)
        {
            StateImpact stateImpact = new StateImpact();
            stateImpact.m_State = GetBeHitStateFromStr(stCall.GetParamId(0));
            for(int i = 1; i < stCall.GetParamNum(); i = i+2)
            {
                ImpactData im = new ImpactData();
                im.ImpactId = int.Parse(stCall.GetParamId(i));
                if(stCall.GetParamNum() > i+1)
                {
                    im.RemainTime = int.Parse(stCall.GetParamId(i));
                }
                else
                {
                    im.RemainTime = -1;
                }
                stateImpact.m_Impacts.Add(im);
            }
            return stateImpact;
        }

        public static BeHitState GetBeHitStateFromStr(string str)
        {
            BeHitState result = BeHitState.kDefault;
            if (str.Equals("kDefault"))
            {
                result = BeHitState.kDefault;
            }
            else if (str.Equals("kStand"))
            {
                result = BeHitState.kStand;
            }
            else if (str.Equals("kStiffness"))
            {
                result = BeHitState.kStiffness;
            }
            else if (str.Equals("kKnockDown"))
            {
                result = BeHitState.kKnockDown;
            }
            else if (str.Equals("kLauncher"))
            {
                result = BeHitState.kLauncher;
            }
            return result;
        }

        public static bool IsPlayerSelf(GameObject obj)
        {
            return DashFire.LogicSystem.PlayerSelf == obj;
        }


        /**ShakeCamera2Trigger 逻辑 TODO:看不懂*/
        #region ShakeCamera2Trigger

        public static int CAMERA_CONTROL_FAILED = -1;
        public static int CAMERA_NO_ONE_CONTROL = 0;
        public static int CAMERA_CONTROL_START_ID = 1;

        private static int m_CurCameraControlId = 0;
        private static bool m_IsMoveCameraTriggerContol = false;

        public static bool IsControledCamera(int control_id)
        {
            if (m_CurCameraControlId == CAMERA_NO_ONE_CONTROL)
            {
                return false;
            }
            if (control_id <= CAMERA_CONTROL_FAILED)
            {
                return false;
            }
            if (control_id == m_CurCameraControlId)
            {
                return true;
            }
            return false;
        }

        public static int ControlCamera(bool is_control, bool is_move_trigger = false)
        {
            if (m_IsMoveCameraTriggerContol && !is_move_trigger)
            {
                return CAMERA_CONTROL_FAILED;
            }
            GameObject gfx_root = GameObject.Find("GfxGameRoot");
            if (gfx_root != null)
            {
                if (is_control)
                {
                    if (++m_CurCameraControlId < 0)
                    {
                        m_CurCameraControlId = CAMERA_CONTROL_START_ID;
                    }
                    if (is_move_trigger)
                    {
                        m_IsMoveCameraTriggerContol = true;
                    }
                    gfx_root.SendMessage("BeginShake");
                }
                else
                {
                    m_CurCameraControlId = CAMERA_NO_ONE_CONTROL;
                    m_IsMoveCameraTriggerContol = false;
                    gfx_root.SendMessage("EndShake");
                }
                return m_CurCameraControlId;
            }
            return CAMERA_CONTROL_FAILED;
        }

        public static void UpdateObjTransform(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            DashFire.LogicSystem.NotifyGfxUpdatePosition(obj, obj.transform.position.x, obj.transform.position.y, obj.transform.position.z,
                                                         0, (float)(obj.transform.rotation.eulerAngles.y * Math.PI / 180.0f), 0);
        }

        public static void UpdateObjWantDir(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            DashFire.LogicSystem.NotifyGfxChangedWantDir(obj, (float)(obj.transform.rotation.eulerAngles.y * Math.PI / 180.0f));
        }

        public static Vector3 GetGroundPos(Vector3 pos)
        {
            Vector3 sourcePos = pos;
            RaycastHit hit;
            pos.y += 2;
            if (Physics.Raycast(pos, -Vector3.up, out hit, m_RayCastMaxDistance, m_TerrainLayer))
            {
                sourcePos.y = hit.point.y;
            }
            return sourcePos;
        }

        public static void SetFollowEnable(bool is_enable)
        {
            GameObject gfx_root = GameObject.Find("GfxGameRoot");
            if (gfx_root != null)
            {
                gfx_root.SendMessage("SetFollowEnable", is_enable);
            }
        }

        public static GameObject GetCameraObj()
        {
            GameObject gfx_root = GameObject.Find("GfxGameRoot");
            return gfx_root;
        }

        #endregion
    }
}
