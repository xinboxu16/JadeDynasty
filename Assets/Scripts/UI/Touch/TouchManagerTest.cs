using DashFire;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TouchManagerTest : MonoSinleton<TouchManagerTest>
{
    // 手指触摸状态
    public enum FingerPhase
    {
        None = 0,
        Begin,
        Moving,
        Stationary,
    }

    // 手指列表接口
    public interface IFingerList : IEnumerable<Finger>
    {
        /*
         *  用此类的实例名去访问内部的数据,如:
            A temp=new A();
            可以用temp[0]去访问
         */
        Finger this[int index]
        {
            get;
        }

        //类中成员变量和方法，默认是private；接口中默认是public
        int Count
        {
            get;
        }

        // 获得平均的开始点位置
        Vector2 GetAverageStartPosition();

        // 获得平均位置
        Vector2 GetAveragePosition();

        // 获得前一帧平均位置
        Vector2 GetAveragePreviousPosition();

        // 获取触摸开始的位置和现在位置的平均距离
        float GetAverageDistanceFromStart();

        // 最晚按的手指开始时间
        Finger GetOldest();

        // 是否所有的触摸手指都在移动中
        bool AllMoving();

        // 触摸是否向相同方向移动
        // <param name="tolerance">0->1 映射 0->90 度</param>
        bool MovingInSameDirection(float tolerance);
    }

    public class Finger
    {
        // privete
        int index = 0;
        FingerPhase phase = FingerPhase.None;
        FingerPhase prevPhase = FingerPhase.None;
        Vector2 pos = Vector2.zero;
        Vector2 startPos = Vector2.zero;
        Vector2 prevPos = Vector2.zero;
        Vector2 deltaPos = Vector2.zero;
        float startTime = 0;
        float lastMoveTime = 0;
        float distFromStart = 0;
        bool moved = false;

        // 手指是否被过滤
        bool filteredOut = true;
        Collider collider;
        Collider prevCollider;
        float elapsedTimeStationary = 0;

        public Finger(int index)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return "Finger" + index;
        }

        /*
         * 例如 将Person转换成string public static implicit operator string(Person p)
         * 调用 Person dig = new Person(7); string result = dig;
         */
        public static implicit operator bool (Finger finger)
        {
            return finger != null;
        }

        internal void Update(bool newDownState, Vector2 newPos)
        {
            if(filteredOut && !newDownState)
            {
                filteredOut = false;
            }

            // 过滤
            if (!IsDown && newDownState && !TouchManagerTest.Instance.ShouldProcessTouch(index, newPos))
            {
                filteredOut = true;
                newDownState = false;
            }

            prevPhase = phase;

            if(newDownState)
            {
                // 新的触摸，重置手指状态
                if(!WasDown)
                {
                    phase = FingerPhase.Begin;

                    pos = newPos;
                    startPos = pos;
                    prevPos = pos;
                    deltaPos = Vector2.zero;
                    moved = false;
                    lastMoveTime = 0;

                    startTime = Time.time;
                    elapsedTimeStationary = 0;
                    distFromStart = 0;
                }
                else
                {
                    prevPos = pos;
                    pos = newPos;
                    distFromStart = Vector3.Distance(startPos, pos);
                    deltaPos = pos - prevPos;

                    if (deltaPos.sqrMagnitude > 0)
                    {
                        lastMoveTime = Time.time;
                        phase = FingerPhase.Moving;
                    }
                    else if (!IsMoving || ((Time.time - lastMoveTime) > 0.05f))
                    {
                        // 停止移动之后有一点缓冲时间
                        phase = FingerPhase.Stationary;
                    }

                    if (IsMoving)
                    {
                        // 手指至少移动一次
                        moved = true;
                    }
                    else
                    {
                        if (!WasStationary)
                        {
                            // 开始以后新的静止状态
                            elapsedTimeStationary = 0;
                        }
                        else
                        {
                            elapsedTimeStationary += Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                phase = FingerPhase.None;
            }
        }

        public int Index
        { 
            get
            {
                return index;
            }
        }

        public bool IsDown
        {
            get
            {
                return phase != FingerPhase.None;
            }
        }

        public FingerPhase Phase
        {
            get
            {
                return phase;
            }
        }

        // 前一帧状态
        public FingerPhase PreviousPhase
        {
            get
            {
                return prevPhase;
            }
        }

        public bool WasDown
        {
            get
            {
                return prevPhase != FingerPhase.None;
            }
        }

        public bool IsMoving
        {
            get
            {
                return phase == FingerPhase.Moving;
            }
        }

        public bool WasMoving
        {
            get
            {
                return prevPhase == FingerPhase.Moving;
            }
        }

        public bool IsStationary
        {
            get
            {
                return phase == FingerPhase.Stationary;
            }
        }

        public bool WasStationary
        {
            get
            {
                return prevPhase == FingerPhase.Stationary;
            }
        }

        public bool Moved
        {
            get
            {
                return moved;
            }
        }

        public float StarTime
        {
            get
            {
                return startTime;
            }
        }

        public Vector2 StartPosition
        {
            get
            {
                return startPos;
            }
        }

        public Vector2 Position
        {
            get
            {
                return pos;
            }
        }

        public Vector2 PreviousPosition
        {
            get
            {
                return prevPos;
            }
        }

        public Vector2 DeltaPosition
        {
            get
            {
                return deltaPos;
            }
        }

        public float DistanceFromStart
        {
            get
            {
                return distFromStart;
            }
        }

        public bool IsFiltered
        {
            get
            {
                return filteredOut;
            }
        }

        public float TimeStationary
        {
            get
            {
                return elapsedTimeStationary;
            }
        }
    }

    // 手指列表
    public class FingerList:IFingerList
    {
        public delegate T FingerPropertyGetterDelegate<T>(Finger finger);

        static FingerPropertyGetterDelegate<Vector2> delGetFingerStartPosition = GetFingerStartPosition;
        static FingerPropertyGetterDelegate<Vector2> delGetFingerPosition = GetFingerPosition;
        static FingerPropertyGetterDelegate<Vector2> delGetFingerPreviousPosition = GetFingerPreviousPosition;
        static FingerPropertyGetterDelegate<float> delGetFingerDistanceFromStart = GetFingerDistanceFromStart;

        List<Finger> list = null;

        public FingerList()
        {
            list = new List<Finger>();
        }

        public FingerList(List<Finger> list)
        {
            this.list = list;
        }

        public Finger this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public IEnumerator<Finger> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Finger touch)
        {
            list.Add(touch);
        }

        public bool Remove(Finger touch)
        {
            return list.Remove(touch);
        }

        public bool Contains(Finger touch)
        {
            return list.Contains(touch);
        }

        public void AddRange(IEnumerable<Finger> touches)
        {
            list.AddRange(touches);
        }

        public void Clear()
        {
            list.Clear();
        }

        public Vector2 AverageVector(FingerPropertyGetterDelegate<Vector2> getProperty)
        {
            Vector2 avg = Vector2.zero;
            if(Count > 0)
            {
                for(int i = 0; i < list.Count; ++i)
                {
                    avg += getProperty(list[i]);
                }
                avg /= Count;
            }
            return avg;
        }

        public float AverageFloat(FingerPropertyGetterDelegate<float> getProperty)
        {
            float avg = 0;
            if (Count > 0)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    avg += getProperty(list[i]);
                }
                avg /= Count;
            }
            return avg;
        }

        static Vector2 GetFingerStartPosition(Finger finger)
        {
            return finger.StartPosition;
        }
        static Vector2 GetFingerPosition(Finger finger)
        {
            return finger.Position;
        }
        static Vector2 GetFingerPreviousPosition(Finger finger)
        {
            return finger.PreviousPosition;
        }
        static float GetFingerDistanceFromStart(Finger finger)
        {
            return finger.DistanceFromStart;
        }

        public Vector2 GetAverageStartPosition()
        {
            return AverageVector(delGetFingerStartPosition);
        }

        public Vector2 GetAveragePosition()
        {
            return AverageVector(delGetFingerPosition);
        }

        public Vector2 GetAveragePreviousPosition()
        {
            return AverageVector(delGetFingerPreviousPosition);
        }

        public float GetAverageDistanceFromStart()
        {
            return AverageFloat(delGetFingerDistanceFromStart);
        }

        public Finger GetOldest()
        {
            Finger oldest = null;
            foreach (Finger finger in list)
            {
                if (oldest == null || (finger.StarTime < oldest.StarTime))
                {
                    oldest = finger;
                }
            }

            return oldest;
        }

        public bool MovingInSameDirection(float tolerance)
        {
            if (Count < 2)
            {
                return true;
            }
            float minDOT = Mathf.Max(0.1f, 1.0f - tolerance);
            Vector2 refDir = this[0].Position - this[0].StartPosition;
            refDir.Normalize();
            for (int i = 1; i < Count; ++i)
            {
                Vector2 dir = this[i].Position - this[i].StartPosition;
                dir.Normalize();
                if (Vector2.Dot(refDir, dir) < minDOT)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllMoving()
        {
            if (Count == 0)
            {
                return false;
            }
            // 所有手指必须在移动
            for (int i = 0; i < list.Count; ++i)
            {
                if (!list[i].IsMoving)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class InputProviderEvent
    {
        public InputProvider inputPrefab;
    }

    //TouchManager

    public static readonly RuntimePlatform[] TouchScreenPlatforms = 
    { 
        RuntimePlatform.IPhonePlayer,
        RuntimePlatform.Android,
#if !UNITY_3_5
        RuntimePlatform.BlackBerryPlayer,
        RuntimePlatform.WP8Player,
#endif
    };

    // 手指管理
    Finger[] fingers;
    FingerList touches;
    // 输入
    InputProvider inputProvider;

    // 默认输入
    public InputProvider mouseInputPrefab;
    public InputProvider touchInputPrefab;

    /// Global Input Filter
    /// <returns>true 允许输入, false 打断</returns>
    public delegate bool GlobalTouchFilterDelegate(int fingerIndex, Vector2 position);
    GlobalTouchFilterDelegate globalTouchFilterFunc;

    public delegate void EventHandler();
    public static EventHandler OnInputProviderChanged;

    private static bool gestureEnable = true;

    // 是否跨场景
    public bool aross = false;
    // 是否允许远程
    public bool unityremote = true;

    void Awake()
    {
        CheckInit();
    }

    void Start()
    {
        if(aross)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
//#if UNITY_EDITOR
//        if(unityremote && Input.touchCount > 0 && inputProvider.GetType() != touchInputPrefab.GetType())
//        {
//            // 检查到unityremote，切换触摸输入方式
//            InstallInputProvider(touchInputPrefab);
//            unityremote = false;
//            return;
//        }
//#endif
        if(inputProvider)
        {
            UpdateFingers();
        }
    }

    void OnEnable()
    {
        CheckInit();
    }

    void OnDestroy()
    {
        _instance = null;
    }

    private void CheckInit()
    {
        // 保持只有一个TouchManager实例
        if(_instance != this)
        {
            Destroy(this.gameObject);
            TouchEnable = false;
            return;
        }
        else
        {
            Init();
            TouchEnable = true;
        }
    }

    private void Init()
    {
        InitInputProvider();
    }

    private void InitInputProvider()
    {
        InputProviderEvent e = new InputProviderEvent();
        if(IsTouchScreenPlatform(Application.platform))
        {
            e.inputPrefab = touchInputPrefab;
        }
        else
        {
            e.inputPrefab = mouseInputPrefab;
        }
        InstallInputProvider(e.inputPrefab);
    }

    public void InstallInputProvider(InputProvider inputPrefab)
    {
        if(!inputPrefab)
        {
            //Debug.LogError("Invalid InputProvider (null)");
            return;
        }

        if(inputPrefab)
        {
            Destroy(inputProvider.gameObject);
        }

        inputProvider = ResourceSystem.NewObject(inputPrefab) as InputProvider;
        inputProvider.name = inputProvider.name;
        inputProvider.transform.parent = this.transform;

        InitFingers(MaxFingers);

        if (OnInputProviderChanged != null)
        {
            OnInputProviderChanged();
        }
    }

    private void InitFingers(int count)
    {
        // 每个手指数据
        fingers = new Finger[count];
        for(int i = 0; i < count; ++i)
        {
            fingers[i] = new Finger(i);
        }
        touches = new FingerList();
    }

    private void UpdateFingers()
    {
        touches.Clear();

        // 更新所有手指
        for (int i = 0; i < fingers.Length; ++i)
        {
            Finger finger = fingers[i];
            Vector2 pos = Vector2.zero;
            bool down = false;
            // 刷新输入状态
            inputProvider.GetInputState(finger.Index, out down, out pos);
            finger.Update(down, pos);
            if(finger.IsDown)
            {
                touches.Add(finger);
            }
        }
    }

    public static bool IsTouchScreenPlatform(RuntimePlatform platform)
    {
        for (int i = 0; i < TouchScreenPlatforms.Length; ++i)
        {
            if (platform == TouchScreenPlatforms[i])
                return true;
        }
        return false;
    }

    public static bool TouchEnable { get; set; }

    public static bool GestureEnable
    {
        get
        {
            return gestureEnable;
        }
        set
        {
            gestureEnable = value;
        }
    }

    // 同时支持的最大手指数
    public int MaxFingers
    {
        get
        {
            return inputProvider.MaxSimultaneousFingers;
        }
    }

    public static Finger GetFinger(int index)
    {
        return Instance.fingers[index];
    }

    // 当前触摸屏幕的手指列表
    public static IFingerList Touches
    {
        get
        {
            return Instance.touches;
        }
    }

    protected bool ShouldProcessTouch(int fingerIndex, Vector2 position)
    {
        if(globalTouchFilterFunc != null)
        {
            return globalTouchFilterFunc(fingerIndex, position);
        }
        return true;
    }
}

public abstract class InputProvider : MonoBehaviour
{
    // 最大同时手指数量
    public abstract int MaxSimultaneousFingers
    {
        get;
    }

    // 获得某个手指的输入状态
    public abstract void GetInputState(int fingerIndex, out bool down, out Vector2 position);
}
