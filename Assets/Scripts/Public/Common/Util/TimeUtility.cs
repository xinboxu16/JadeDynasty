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
            return DateTime.Now.Ticks / 10;
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
    }
}
