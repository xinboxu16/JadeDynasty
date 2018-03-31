using DashFire.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DashFire
{
    /**
   * @brief 游戏控制器
   */
    public sealed class GameControler
    {
        //初始化日志系统
        private static LogicLogger s_LogicLogger = new LogicLogger();
        private static GameLogicThread s_LogicThread = new GameLogicThread();
        //----------------------------------------------------------------------
        // 标准接口
        //----------------------------------------------------------------------
        private static bool s_IsInited = false;
        public static bool IsInited
        {
            get { return s_IsInited; }
        }

        private static bool s_IsPaused = false;
        internal static bool IsPaused
        {
            get
            {
                return s_IsPaused;
            }
        }

        internal static LogicLogger LogicLoggerInstance
        {
            get
            {
                return s_LogicLogger;
            }
        }

        //一系列初始化
        public static void Init(string logPath, string dataPath)
        {
            s_IsInited = true;
            s_LogicLogger.Init(logPath);
            HomePath.CurHomePath = dataPath;
            GlobalVariables.Instance.IsDebug = false;

            //用于简单加密
            string key = "防君子不防小人";
            byte[] xor = Encoding.UTF8.GetBytes(key);//xor异或

            FileReaderProxy.RegisterReadFileHandler((string filePath) =>
            {
                byte[] buffer = null;
                try
                {
                    buffer = File.ReadAllBytes(filePath);
//#if !DEBUG
//                    if(filePath.EndsWith(".txt"))
//                    {
//                        Helper.Xor(buffer, xor);
//                    }
//#endif
                }catch(Exception e)
                {
                    GfxSystem.GfxLog("Exception:{0}\n{1}", e.Message, e.StackTrace);
                    return null;
                }
                return buffer;
            });

            //初始化日志输出
            LogSystem.OnOutput = (Log_Type type, string msg) => {
                if (Log_Type.LT_Error == type)
                {
                    GfxSystem.GfxErrorLog("{0}", msg);
                }
                else
                {
                    GfxSystem.GfxLog("{0}", msg);
                }
                s_LogicLogger.Log("{0}", msg);
            };

            GfxSystem.GfxLog("GameControler.Init");

            // GfxSystem初始化
            GfxSystem.Init();
            GfxSystem.SetLogicInvoker(s_LogicThread);
            GfxSystem.SetLogicLogCallback((bool isError, string format, object[] args)=> {
                if (isError)
                    GfxSystem.GfxErrorLog(format, args);
                else
                    GfxSystem.GfxLog(format, args);
                s_LogicLogger.Log(format, args);
            });
            GfxSystem.SetGameLogicNotification(GameLogicNotification.Instance);
            GfxModule.Skill.GfxSkillSystem.Instance.Init();
        }

        public static void InitLogic()
        {
            GfxSystem.GfxLog("GameControler.InitLogic");
            EntityManager.Instance.Init();

            //用来读取配置文件
            WorldSystem.Instance.Init();
            WorldSystem.Instance.LoadData();

            //未实现
            ClientStorySystem.Instance.Init();
            //GmCommands.ClientGmStorySystem.Instance.Init();

            PlayerControl.Instance.Init();
            LobbyNetworkSystem.Instance.Init(s_LogicThread);
            NetworkSystem.Instance.Init();
            AiViewManager.Instance.Init();
            SceneLogicViewManager.Instance.Init();
            ImpactViewManager.Instance.Init();//碰撞系统

        }

        public static void StartLogic()
        {
            GfxSystem.GfxLog("GameControler.StartLogic");
            s_LogicThread.Start();
        }

        public static void TickGame()
        {
            //这里是在渲染线程执行的tick，逻辑线程的tick在GameLogicThread.cs文件里执行。
            GfxSystem.Tick();
            GfxModule.Skill.GfxSkillSystem.Instance.Tick();
            GfxModule.Impact.GfxImpactSystem.Instance.Tick();
        }

        public static void PauseLogic(bool isPause)
        {
            s_IsPaused = isPause;
        }

        public static void StopLogic()
        {
            GfxSystem.GfxLog("GameControler.StopLogic");
            s_LogicThread.Stop();
            //未实现
            LobbyNetworkSystem.Instance.QuitClient();
            NetworkSystem.Instance.QuitClient();
        }
        public static void Release()
        {
            GfxSystem.GfxLog("GameControler.Release");
            //未实现
            //WorldSystem.Instance.Release();
            //EntityManager.Instance.Release();
            //NetworkSystem.Instance.Release();
            GfxSystem.Release();
            s_LogicLogger.Dispose();
        }

        //将log写入文件
        public sealed class LogicLogger : IDisposable
        {
            private StreamWriter m_LogStream;

            public void Log(string format, params object[] args)
            {
                string msg = string.Format(format, args);

                m_LogStream.WriteLine(msg);
                m_LogStream.Flush();
            }

            //生成log
            public void Init(string logPath)
            {
                string logFile = string.Format("{0}/Game_{1}.log", logPath, DateTime.Now.ToString("yyyy-MM-dd"));
                m_LogStream = new StreamWriter(logFile, true);
                Log("======GameLog Start ({0}, {1})======", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            private void Release()
            {
                m_LogStream.Close();
                m_LogStream.Dispose();
            }

            public void Dispose()
            {
                Release();
            }

            private object m_LogQueueLock = new object();
            private MyThread m_Thread = new MyThread();
            private long m_LastFlushTime = 0;
            private int m_CurQueueIndex = 0;
            private Queue<string> m_LogQueue;
            private Queue<string>[] m_LogQueues = new Queue<string>[] { new Queue<string>(), new Queue<string>() };

            private void FlushToFile(Queue<string> logQueue)
            {
                lock (m_LogQueueLock)
                {
                    //GfxSystem.GfxLog("LogicLogger.FlushToFile, count {0}.", logQueue.Count);
                    while (logQueue.Count > 0)
                    {
                        string msg = logQueue.Dequeue();
                        m_LogStream.WriteLine(msg);
                    }
                    m_LogStream.Flush();
                }
            }

            private void RequestFlush()
            {
                lock (m_LogQueueLock)
                {
                    m_Thread.QueueActionWithDelegation((MyAction<Queue<string>>)FlushToFile, m_LogQueue);
                    m_CurQueueIndex = 1 - m_CurQueueIndex;
                    m_LogQueue = m_LogQueues[m_CurQueueIndex];
                }
            }

            //用于写日志的线程
            public void Tick()
            {
#if !USE_DISK_LOG
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastFlushTime + 10000 < curTime)
                {
                    m_LastFlushTime = curTime;

                    RequestFlush();
                    //GfxSystem.GfxLog("LogicLogger.Tick.");
                }
#endif
            }
        }
    }
}
