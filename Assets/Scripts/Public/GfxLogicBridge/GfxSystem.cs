using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        #region keyboard

        public static bool IsKeyPressed(Keyboard.Code c)
        {
            return s_Instance.IsKeyPressedImpl(c);
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

        #endregion keyboard

        #region joystick
        
        public static float GetJoystickDir()
        {
            return s_Instance.GetJoystickDirImpl();
        }

        public static float GetJoystickTargetPosX()
        {
            return s_Instance.GetJoystickTargetPosXImpl();
        }

        public static float GetJoystickTargetPosY()
        {
            return s_Instance.GetJoystickTargetPosYImpl();
        }

        public static float GetJoystickTargetPosZ()
        {
            return s_Instance.GetJoystickTargetPosZImpl();
        }

        #endregion

        #region mouse

        //输入状态，允许跨线程读取
        public static float GetMouseX()
        {
            return s_Instance.GetMouseXImpl();
        }
        public static float GetMouseY()
        {
            return s_Instance.GetMouseYImpl();
        }
        public static float GetMouseZ()
        {
            return s_Instance.GetMouseZImpl();
        }

        #endregion

        public static void LoadScene(string sceneName, int chapter, int sceneId, HashSet<int> limitList, MyAction onFinish)
        {
            QueueGfxAction(s_Instance.LoadSceneImpl, sceneName, chapter, sceneId, limitList, onFinish);
        }

        public static void MarkPlayerSelf(int id)
        {
            QueueGfxAction(s_Instance.MarkPlayerSelfImpl, id);
        }

        public static void SendMessageWithTag(string objtag, string msg, object arg)
        {
            SendMessageWithTag(objtag, msg, arg, false);
        }

        public static void SendMessageWithTag(string objtag, string msg, object arg, bool needReceiver)
        {
            QueueGfxAction(s_Instance.SendMessageWithTagImpl, objtag, msg, arg, needReceiver);
        }

        public static void SendMessage(string objname, string msg, object arg, bool needReceiver)
        {
            QueueGfxAction(s_Instance.SendMessageImpl, objname, msg, arg, needReceiver);
        }

        public static void SendMessage(int objid, string msg, object arg, bool needReceiver)
        {
            QueueGfxAction(s_Instance.SendMessageByIdImpl, objid, msg, arg, needReceiver);
        }

        public static void SendMessage(string objname, string msg, object arg)
        {
            SendMessage(objname, msg, arg, false);
        }

        public static void SendMessage(int objid, string msg, object arg)
        {
            SendMessage(objid, msg, arg, false);
        }

        public static void SetShader(int id, string shaderPath)
        {
            QueueGfxAction(s_Instance.SetShaderImpl, id, shaderPath);
        }

        public static void SetBlockedShader(int id, uint rimColor, float rimPower, float cutValue)
        {
            QueueGfxAction(s_Instance.SetBlockedShaderImpl, id, rimColor, rimPower, cutValue);
        }

        public static void PlayQueuedAnimation(int id, string animationName)
        {
            PlayQueuedAnimation(id, animationName, false, false);
        }

        public static void PlayQueuedAnimation(int id, string animationName, bool isPlayNow, bool isStopAll)
        {
            QueueGfxAction(s_Instance.PlayQueuedAnimationImpl, id, animationName, isPlayNow, isStopAll);
        }

        public static void StopAnimation(int id, string animationName)
        {
            QueueGfxAction(s_Instance.StopAnimationImpl, id, animationName);
        }

        public static void CrossFadeAnimation(int id, string animationName)
        {
            CrossFadeAnimation(id, animationName, 0.3f, false);
        }

        public static void SetAnimationSpeed(int id, string animationName, float speed)
        {
            QueueGfxAction(s_Instance.SetAnimationSpeedImpl, id, animationName, speed);
        }

        public static void CrossFadeAnimation(int id, string animationName, float fadeLength)
        {
            CrossFadeAnimation(id, animationName, fadeLength, false);
        }

        public static void CrossFadeAnimation(int id, string animationName, float fadeLength, bool isStopAll)
        {
            QueueGfxAction(s_Instance.CrossFadeAnimationImpl, id, animationName, fadeLength, isStopAll);
        }

        public static void ResetInputState()
        {
            QueueGfxAction(s_Instance.ResetInputStateImpl);
        }

        public static void SetGameObjectVisible(int id, bool visible)
        {
            QueueGfxAction(s_Instance.SetGameObjectVisibleImpl, id, visible);
        }

        public static void CreateGameObject(int id, string resource, SharedGameObjectInfo info)
        {
            QueueGfxAction(s_Instance.CreateGameObjectImpl, id, resource, info);
        }

        public static void CreateGameObject(int id, string resource, float x, float y, float z, float rx, float ry, float rz, bool attachTerrain)
        {
            QueueGfxAction(s_Instance.CreateGameObjectImpl, id, resource, x, y, z, rx, ry, rz, attachTerrain);
        }

        public static void CreateAndAttachGameObject(string resource, int parentId, string path, float recycleTime = -1)
        {
            QueueGfxAction(s_Instance.CreateAndAttachGameObjectImpl, resource, parentId, path, recycleTime);
        }

        public static void SetTimeScale(float scale)
        {
            QueueGfxAction(s_Instance.SetTimeScaleImpl, scale);
        }

        public static void DestroyGameObject(int id)
        {
            QueueGfxAction(s_Instance.DestroyGameObjectImpl, id);
        }

        public static object SyncLock
        {
            get
            {
                return s_Instance.m_SyncLock;
            }
        }

        public static void UpdateGameObjectLocalRotateY(int id, float ry)
        {
            QueueGfxAction(s_Instance.UpdateGameObjectLocalRotateYImpl, id, ry);
        }

        public static void UpdateGameObjectLocalPosition2D(int id, float x, float z)
        {
            UpdateGameObjectLocalPosition2D(id, x, z, true);
        }

        public static void UpdateGameObjectLocalPosition2D(int id, float x, float z, bool attachTerrain)
        {
            QueueGfxAction(s_Instance.UpdateGameObjectLocalPosition2DImpl, id, x, z, attachTerrain);
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

        public static void SetGameLogicNotification(IGameLogicNotification notification)
        {
            s_Instance.SetGameLogicNotificationImpl(notification);
        }

        public static void QueueGfxActionWithDelegation(Delegate action, params object[] args)
        {
            if (null != s_Instance.m_GfxInvoker)
            {
                s_Instance.m_GfxInvoker.QueueActionWithDelegation(action, args);
            }
        }
        //逻辑层与unity3d脚本交互函数
        public static void QueueGfxAction(MyAction action)
        {
            QueueGfxActionWithDelegation(action);
        }

        public static void QueueGfxAction<T1>(MyAction<T1> action, T1 t1)
        {
            QueueGfxActionWithDelegation(action, t1);
        }

        public static void QueueGfxAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
        {
            QueueGfxActionWithDelegation(action, t1, t2);
        }

        public static void QueueGfxAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3);
        }

        public static void QueueGfxAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4);
        }

        public static void QueueGfxAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4, t5);
        }

        public static void QueueGfxAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7);
        }

        public static void QueueGfxAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static void QueueGfxAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            QueueGfxActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
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
