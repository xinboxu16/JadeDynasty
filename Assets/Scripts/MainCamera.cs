using UnityEngine;
using System.Collections;
using DashFire;

public class MainCamera : MonoBehaviour
{
    public void SetFollowEnable(bool value)
    {
        m_IsFollowEnable = value;
    }

    public void SetFollowSpeedByStr(string str_param)
    {
        string[] str_params = str_param.Split('|');
        if (str_params.Length < 5)
        {
            return;
        }
        float maxdistance = float.Parse(str_params[0]);
        float mindistance = float.Parse(str_params[1]);
        float maxspeed = float.Parse(str_params[2]);
        float minspeed = float.Parse(str_params[3]);
        int power = int.Parse(str_params[4]);
        SetFollowSpeed(maxdistance, mindistance, maxspeed, minspeed, power);
    }

    public void SetFollowSpeed(float max_speed_distance, float min_speed_distance,
                               float max_speed, float min_speed, int power)
    {
        m_MaxSpeedDistance = max_speed_distance;
        m_MinSpeedDistance = min_speed_distance;
        m_MaxSpeed = max_speed;
        m_MinSpeed = min_speed;
        m_Power = power;
        ComputeSpeedFactor();
    }

    public void ResetFollowSpeed()
    {
        m_MaxSpeedDistance = m_OrigMaxSpeedDistance;
        m_MinSpeedDistance = m_OrigMinSpeedDistance;
        m_MaxSpeed = m_OrigMaxSpeed;
        m_MinSpeed = m_OrigMinSpeed;
        m_Power = m_OrigPower;
        ComputeSpeedFactor();
    }

    public int GetTargetId()
    {
        return m_CurTargetId;
    }

    public void CameraFollow(int id)
    {
        GameObject obj = LogicSystem.GetGameObject(id);
        if (null != obj)
        {
            m_CurTargetId = id;
            m_Target = obj.transform;
            Collider collider = m_Target.GetComponent<Collider>();
            if (null != collider)
            {
                m_CenterOffset = collider.bounds.center - m_Target.position;
                m_HeadOffset = m_CenterOffset;
                m_HeadOffset.y = collider.bounds.max.y - m_Target.position.y;
                m_IsFollow = true;
                Cut();
            }
            else
            {
                m_IsFollow = false;
            }
        }
    }
    public void EndShake()
    {
        m_IsShaking = false;
    }
    public void BeginShake()
    {
        m_IsShaking = true;
    }
    public void CameraFollowImmediately(int id)
    {
        GameObject obj = LogicSystem.GetGameObject(id);
        if (null != obj)
        {
            m_CurTargetId = id;
            m_Target = obj.transform;
            Collider collider = m_Target.GetComponent<Collider>();
            if (null != collider)
            {
                m_CurTargetPos = m_Target.position;
                m_CenterOffset = collider.bounds.center - m_Target.position;
                m_HeadOffset = m_CenterOffset;
                m_HeadOffset.y = collider.bounds.max.y - m_Target.position.y;
                m_IsFollow = true;
                Cut();
            }
            else
            {
                m_IsFollow = false;
            }
        }
    }
    public void CameraContinueFollowImmediately()
    {
        CameraFollowImmediately(GetTargetId());
    }
    public void CameraLookat(float[] coord)
    {
        m_Target = null;
        m_IsFollow = false;
        m_TargetPos = new Vector3(coord[0], coord[1], coord[2]);
        //Debug.Log("CameraLookat:" + m_TargetPos.ToString());
        Cut();
    }
    public void CameraLookatImmediately(float[] coord)
    {
        m_Target = null;
        m_IsFollow = false;
        m_TargetPos = new Vector3(coord[0], coord[1], coord[2]);
        m_CurTargetPos = m_TargetPos;
        //Debug.Log("CameraLookat:" + m_TargetPos.ToString());
        Cut();
    }
    public void LookAtTarget()
    {
        if (m_Target != null)
        {
            m_CameraTransform.LookAt(m_Target.position + m_CenterOffset);
        }
    }
    public void CameraFixedYaw(float dir)
    {
        m_FixedYaw = LogicSystem.RadianToDegree(dir);
        m_AngularSmoothLag = 0.3f;
        m_SnapSmoothLag = 0.2f;
    }
    public void CameraYaw(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float dir = args[0];
        float lag = args[1] / 1000.0f;
        m_FixedYaw = LogicSystem.RadianToDegree(dir);
        m_AngularSmoothLag = lag;
        m_SnapSmoothLag = lag;
    }
    public void CameraHeight(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float height = args[0];
        float lag = args[1] / 1000.0f;
        if (height >= 0)
            m_Height = height;
        else
            m_Height = m_OrigHeight;
        m_HeightSmoothLag = lag;
        m_NeedLookat = true;
    }
    public void CameraDistance(float[] args)
    {
        if (null == args || args.Length != 2)
            return;
        float dist = args[0];
        float lag = args[1] / 1000.0f;
        if (dist >= 0)
        {
            m_Distance = dist;
        }
        else
        {
            m_Distance = m_OrigDistance;
        }
        m_DistanceSmoothLag = lag;
        m_NeedLookat = true;
    }

    public void CameraEnable(object[] args)
    {
        if (null != args[0] && null != args[1])
        {
            string cameraName = args[0] as string;
            bool isEnable = (bool)args[1];

            if (null != cameraName)
            {
                GameObject cameraObj = GameObject.Find(cameraName);
                if (cameraObj != null)
                {
                    Camera camera = cameraObj.GetComponent<Camera>();
                    if (null != camera)
                    {
                        camera.enabled = isEnable;
                    }
                }
            }
        }
    }

    public void DisableBloom()
    {
        if (null != Camera.main)
        {
            ScreenBloom bloom = Camera.main.GetComponent<ScreenBloom>();
            if (null != bloom)
            {
                bloom.enabled = false;
            }
        }
    }
    public void EnableBloom()
    {
        if (null != Camera.main)
        {
            ScreenBloom bloom = Camera.main.GetComponent<ScreenBloom>();
            if (null != bloom)
            {
                bloom.enabled = true;
            }
        }
    }

    internal void Awake()
    {
        m_CameraTransform = Camera.main.transform;
        m_CameraSpeed = m_CameraFollowSpeed;
        if (!m_CameraTransform)
        {
            Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
            enabled = false;
        }
        ComputeSpeedFactor();
        m_OrigMaxSpeedDistance = m_MaxSpeedDistance;
        m_OrigMinSpeedDistance = m_MinSpeedDistance;
        m_OrigMaxSpeed = m_MaxSpeed;
        m_OrigMinSpeed = m_MinSpeed;
        m_OrigPower = m_Power;

        m_OrigHeight = m_Height;
        m_OrigDistance = m_Distance;
        m_CurDistance = m_Distance;
    }

    internal void LateUpdate()
    {
        if (null == Camera.main || !Camera.main.enabled)
        {
            m_CameraTransform = null;
            return;
        }
        if (!m_IsShaking && DashFire.LobbyClient.Instance.CurrentRole != null)
        {
            Apply();
        }
    }

    private void DebugDrawStuff()
    {
        Debug.DrawLine(m_Target.position, m_Target.position + m_HeadOffset);
    }

    private float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);

        return Mathf.Abs(b - a);
    }

    public void Apply()
    {
        if (!m_IsFollowEnable || null == Camera.main || !Camera.main.enabled)
        {
            return;
        }
        if (null == m_CameraTransform)
        {
            m_CameraTransform = Camera.main.transform;
        }
        SetUpPosition();

        Vector3 targetCenter = m_CurTargetPos + m_CenterOffset;
        Vector3 targetHead = m_CurTargetPos + m_HeadOffset;

        // Always look at the target	
        SetUpRotation(targetCenter, targetHead);
    }

    public void SetUpPosition()
    {
        AdjustSpeedAndMoveTarget();

        Vector3 targetCenter = m_CurTargetPos + m_CenterOffset;
        Vector3 targetHead = m_CurTargetPos + m_HeadOffset;
        //DebugDrawStuff();

        // Calculate the current & target rotation angles
        float originalTargetAngle = m_FixedYaw;//m_Target.eulerAngles.y;
        float currentAngle = m_CameraTransform.eulerAngles.y;

        // Adjust real target angle when camera is locked
        float targetAngle = originalTargetAngle;

        // When pressing Fire2 (alt) the camera will snap to the target direction real quick.
        // It will stop snapping when it reaches the target
        //m_Snap = true;

        if (m_Snap)
        {
            // We are close to the target, so we can stop snapping now!
            if (AngleDistance(currentAngle, originalTargetAngle) < 3.0)
                m_Snap = false;

            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref m_AngleVelocity, m_SnapSmoothLag, m_SnapMaxSpeed);
        }
        // Normal camera motion
        else
        {
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref m_AngleVelocity, m_AngularSmoothLag, m_AngularMaxSpeed);
        }

        /*
          // When jumping don't move camera upwards but only down!
          if (false)
          {
              // We'd be moving the camera upwards, do that only if it's really high
              float newTargetHeight = targetCenter.y + m_Height;
              if (newTargetHeight < m_TargetHeight || newTargetHeight - m_TargetHeight > 5)
                  m_TargetHeight = targetCenter.y + m_Height;
          }
          // When walking always update the target height
          else*/
        {
            m_TargetHeight = targetCenter.y + m_Height;
        }

        // Damp the height
        float currentHeight = m_CameraTransform.position.y;
        currentHeight = Mathf.SmoothDamp(currentHeight, m_TargetHeight, ref m_HeightVelocity, m_HeightSmoothLag);
        m_CurDistance = Mathf.SmoothDamp(m_CurDistance, m_Distance, ref m_DistanceVelocity, m_DistanceSmoothLag);

        // Convert the angle into a rotation, by which we then reposition the camera
        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 pos = targetCenter;
        pos += currentRotation * Vector3.back * m_CurDistance;

        // Set the height of the camera
        pos.y = currentHeight;

        m_CameraTransform.position = pos;
    }

    private void Cut()
    {
        float oldHeightSmooth = m_HeightSmoothLag;
        float oldDistanceSmooth = m_DistanceSmoothLag;
        float oldSnapMaxSpeed = m_SnapMaxSpeed;
        float oldSnapSmooth = m_SnapSmoothLag;

        m_SnapMaxSpeed = 10000;
        m_SnapSmoothLag = 0.001f;
        m_HeightSmoothLag = 0.001f;
        m_DistanceSmoothLag = 0.001f;

        m_Snap = true;
        Apply();

        m_HeightSmoothLag = oldHeightSmooth;
        m_DistanceSmoothLag = oldDistanceSmooth;
        m_SnapMaxSpeed = oldSnapMaxSpeed;
        m_SnapSmoothLag = oldSnapSmooth;
    }

    private void AdjustSpeedAndMoveTarget()
    {
        if (null != m_Target)
        {
            m_TargetPos = m_Target.position;
        }
        float delta = Time.deltaTime;
        Vector3 distDir = m_TargetPos - m_CurTargetPos;
        float dist = distDir.magnitude;
        distDir.Normalize();
        m_CameraSpeed = GetCurSpeed(dist);
        Vector3 motion = distDir * m_CameraSpeed * delta;
        if (motion.magnitude >= dist)
        {
            m_CurTargetPos = m_TargetPos;
            m_CameraSpeed = dist / Time.deltaTime;
        }
        else
        {
            m_CurTargetPos += motion;
        }
    }

    private void SetUpRotation(Vector3 centerPos, Vector3 headPos)
    {
        //height与distance变化时，需要保持lookat目标
        float currentHeight = m_CameraTransform.position.y;
        if (m_NeedLookat)
        {
            if (!Geometry.IsSameFloat(currentHeight, m_TargetHeight) || !Geometry.IsSameFloat(m_CurDistance, m_Distance))
            {
                m_CameraTransform.LookAt(m_CurTargetPos);
            }
            else
            {
                m_NeedLookat = false;
            }
        }
        else
        {
            // Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
            // * When jumping up and down we don't want to center the guy in screen space.
            //  This is important to give a feel for how high you jump and avoiding large camera movements.
            //   
            // * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
            //
            // So here is what we will do:
            //
            // 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
            // 2. When grounded we make him be centered
            // 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
            // 4. When landing we smoothly interpolate towards centering him on screen
            Vector3 cameraPos = m_CameraTransform.position;
            Vector3 offsetToCenter = centerPos - cameraPos;

            // Generate base rotation only around y-axis
            Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));

            Vector3 relativeOffset = Vector3.forward * m_CurDistance + Vector3.down * m_Height;
            m_CameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);

            // Calculate the projected center position and top position in world space
            Ray centerRay = m_CameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 1));
            Ray topRay = m_CameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, m_ClampHeadPositionScreenSpace, 1));

            Vector3 centerRayPos = centerRay.GetPoint(m_CurDistance);
            Vector3 topRayPos = topRay.GetPoint(m_CurDistance);

            float centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);

            float heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);

            float extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
            if (extraLookAngle < centerToTopAngle)
            {
                extraLookAngle = 0;
            }
            else
            {
                extraLookAngle = extraLookAngle - centerToTopAngle;
                m_CameraTransform.rotation *= Quaternion.Euler(-extraLookAngle, 0, 0);
            }
        }
    }

    private Vector3 GetCenterOffset()
    {
        return m_CenterOffset;
    }

    private float GetCurSpeed(float distance)
    {
        float result = m_MinSpeed;
        if (distance > m_MaxSpeedDistance)
        {
            return m_MaxSpeed;
        }
        if (distance < m_MinSpeedDistance)
        {
            return m_MinSpeed;
        }
        result = m_FactorA * Mathf.Pow(distance, m_Power) + m_FactorB;
        return result;
    }

    private void ComputeSpeedFactor()
    {
        //a*min_distance^n + b = min_speed
        //a*max_distance^n + b = max_speed
        float denominator = Mathf.Pow(m_MaxSpeedDistance, m_Power) - Mathf.Pow(m_MinSpeedDistance, m_Power);
        if (denominator != 0)
        {
            m_FactorA = (m_MaxSpeed - m_MinSpeed) / denominator;
        }
        else
        {
            m_FactorA = 0;
        }
        m_FactorB = m_MinSpeed - m_FactorA * Mathf.Pow(m_MinSpeedDistance, m_Power);
    }

    // The distance in the x-z plane to the target
    public float m_Distance = 7.0f;
    // the height we want the camera to be above the target
    public float m_Height = 6.0f;

    public float m_MaxSpeedDistance = 50;
    public float m_MinSpeedDistance = 0.5f;
    public float m_MaxSpeed = 50;
    public float m_MinSpeed = 20;
    public int m_Power = 1;
    public float m_CameraFollowSpeed = 10.0f;
    public float m_CameraSpeed;

    private bool m_IsFollow = false;
    private bool m_IsShaking = false;
    private bool m_NeedLookat = false;

    private float m_FixedYaw = 0;
    private Transform m_CameraTransform;
    private Transform m_Target;
    private Vector3 m_CurTargetPos;
    private Vector3 m_TargetPos;
    private float m_CurDistance;

    private float m_FactorA;
    private float m_FactorB;
    private bool m_IsFollowEnable = true;
    private float m_OrigMaxSpeedDistance;
    private float m_OrigMinSpeedDistance;
    private float m_OrigMaxSpeed;
    private float m_OrigMinSpeed;
    private int m_OrigPower;
    private float m_OrigHeight;
    private float m_OrigDistance;

    private float m_HeightSmoothLag = 0.3f;
    private float m_DistanceSmoothLag = 3.0f;
    private float m_AngularSmoothLag = 0.3f;
    private float m_AngularMaxSpeed = 15.0f;
    private float m_SnapSmoothLag = 0.2f;
    private float m_SnapMaxSpeed = 720.0f;
    private float m_ClampHeadPositionScreenSpace = 0.75f;
    private Vector3 m_HeadOffset = Vector3.zero;
    private Vector3 m_CenterOffset = Vector3.zero;
    private float m_HeightVelocity = 0.0f;
    private float m_AngleVelocity = 0.0f;
    private float m_DistanceVelocity = 0.0f;
    private bool m_Snap = false;
    private float m_TargetHeight = 100000.0f;
    private int m_CurTargetId;
}