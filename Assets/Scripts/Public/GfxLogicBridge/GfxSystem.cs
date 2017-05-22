using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed partial class GfxSystem
    {
        //引擎线程调用的方法，不要在逻辑线程调用
        public static void Init()
        {
            s_Instance.InitImpl();
        }

        //渲染线程执行
        public static void Tick()
        {
            s_Instance.TickImpl();
        }

        public static void Release()
        {
            s_Instance.ReleaseImpl();
        }

        //事件注册,事件处理会通过IActionQueue在游戏逻辑线程处理（仅在初始化时调用【第一次tick前】)
        public static void ListenKeyboardEvent(Keyboard.Code c, MyAction<int, int> handler)
        {
            s_Instance.ListenKeyboardEventImpl(c, handler);
        }

        public static void ListenTouchEvent(TouchEvent c, MyAction<int, GestureArgs> handler)
        {
            s_Instance.ListenTouchEventImpl(c, handler);
        }

        //指出需要查询状态与处理事件的键列表（仅在初始化时调用【第一次tick前】，一般是初始化时，可多次设置不同的键）
        public static void ListenKeyPressState(params Keyboard.Code[] keys)
        {
            s_Instance.ListenKeyPressStateImpl(keys);
        }

        public static void DestroyGameObject(int id)
        {
            QueueGfxAction(s_Instance.DestroyGameObjectImpl, id);
        }

        public static void PublishGfxEvent(string evt, string group, params object[] args)
        {
            QueueGfxAction(s_Instance.PublishGfxEventImpl, evt, group, args);
        }

        //注册异步处理接口（这个是渲染线程向逻辑线程返回信息的底层机制：逻辑线程向渲染线程注册处理，渲染线程完成请求后将此处理发回逻辑线程执行）
        public static void SetLogicInvoker(IActionQueue processor)
        {
            s_Instance.SetLogicInvokerImpl(processor);
        }

        public static void SetLogicLogCallback(MyAction<bool, string, object[]> callback)
        {
            s_Instance.SetLogicLogCallbackImpl(callback);
        }

        public static void QueueGfxActionWithDelegation(Delegate action, params object[] args)
        {
            //if (null != s_Instance.m_GfxInvoker)
            //{
            //    s_Instance.m_GfxInvoker.QueueActionWithDelegation(action, args);
            //}
        }
        //逻辑层与unity3d脚本交互函数
        public static void QueueGfxAction<T1>(MyAction<T1> action, T1 t1)
        {
            QueueGfxActionWithDelegation(action, t1);
        }

        public static void QueueGfxAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3);
        }

        public static void QueueGfxAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4, t5);
        }

        public static void QueueGfxAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4);
        }

        public static void LoadScene(string sceneName, int chapter, int sceneId, HashSet<int> limitList, MyAction onFinish)
        {
            QueueGfxAction(s_Instance.LoadSceneImpl, sceneName, chapter, sceneId, limitList, onFinish);
        }

        public static void SendMessage(string objname, string msg, object arg, bool needReceiver)
        {
            QueueGfxAction(s_Instance.SendMessageImpl, objname, msg, arg, needReceiver);
        }

        public static void SendMessage(string objname, string msg, object arg)
        {
            SendMessage(objname, msg, arg, false);
        }

        public static void GfxLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            QueueGfxAction(s_Instance.GfxLogImpl, msg);
            UnityEngine.Debug.Log(msg);//我自己加的
        }

        public static void GfxErrorLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            QueueGfxAction(s_Instance.GfxErrorLogImpl, msg);
            UnityEngine.Debug.LogError(msg);//我自己加的
        }

        public static PublishSubscribeSystem EventChannelForLogic
        {
            get { return s_Instance.m_EventChannelForLogic; }
        }

        internal static GfxSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static GfxSystem s_Instance = new GfxSystem();
    }
}
