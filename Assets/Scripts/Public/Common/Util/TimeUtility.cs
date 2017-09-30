using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class TimeUtility
    {
        private static TimeUtility s_Instance = new TimeUtility();
        private long m_ClientTickTimeUs = 0;
        private long m_ClientDeltaTime = 0;
        private long m_StartTimeUs = 0;

        //NetWork使用
        private static long s_AverageRoundtripTime = 0;
        private static long s_RemoteTimeOffset = 0;

        private TimeUtility()
        {
            m_StartTimeUs = GetElapsedTimeUs();
        }

        public static long GetElapsedTimeUs()
        {
            return DateTime.Now.Ticks / 10;//1 年 1 月 1 日午夜 12:00 以来所经过时间以 100 毫微秒为间隔表示时的数字 100 毫微秒=10^-7 秒
        }

        public static long AverageRoundtripTime
        {
            get { return s_AverageRoundtripTime; }
            set { s_AverageRoundtripTime = value; }
        }

        public static long RemoteTimeOffset
        {
            get { return s_RemoteTimeOffset; }
            set { s_RemoteTimeOffset = value; }
        }

        public static void SampleClientTick()
        {
            long curTime = GetElapsedTimeUs();
            s_Instance.m_ClientDeltaTime = curTime - s_Instance.m_ClientTickTimeUs;
            s_Instance.m_ClientTickTimeUs = curTime;
        }

        public static long GetLocalMilliseconds()
        {
            return (GetElapsedTimeUs() - s_Instance.m_StartTimeUs) / 1000;
        }

        public static long GetServerMilliseconds()
        {
            long val = GetLocalMilliseconds();
            if (GlobalVariables.Instance.IsClient)
            {
                return val + s_RemoteTimeOffset;
            }
            else
            {
                return val;
            }
        }
    }

    public sealed class TimeSnapshot
    {
        //具有ThreadStatic标记的静态变量，在每个线程中都有自己的副本。
        //用 ThreadStatic标记的 static 字段不在线程之间共享。每个执行线程都有单独的字段实例，并且独立地设置及获取该字段的值。如果在不同的线程中访问该字段，则该字段将包含不同的值。
        [ThreadStatic]
        private static TimeSnapshot s_Instance = null;

        private long m_StartTime = 0;
        private long m_LastSnapshotTime = 0;
        private long m_EndTime = 0;

        public static void Start()
        {
            Instance.Start_();
            //UnityEngine.Debug.Log("tick" + Instance.m_StartTime.ToString());
        }

        public static long End()
        {
            return Instance.End_();
        }

        public static long DoCheckPoint()
        {
            return Instance.DoCheckPoint_();
        }

        private void Start_()
        {
            m_LastSnapshotTime = TimeUtility.GetElapsedTimeUs();
            m_StartTime = m_LastSnapshotTime;
        }

        private long DoCheckPoint_()
        {
            long curTime = TimeUtility.GetElapsedTimeUs();
            long ret = curTime - m_LastSnapshotTime;
            m_LastSnapshotTime = curTime;
            return ret;
        }

        private long End_()
        {
            m_EndTime = TimeUtility.GetElapsedTimeUs();
            return m_EndTime - m_StartTime;
        }

        private static TimeSnapshot Instance
        {
            get
            {
                if(null == s_Instance)
                {
                    s_Instance = new TimeSnapshot();
                }
                return s_Instance;
            }
        }
    }
}
