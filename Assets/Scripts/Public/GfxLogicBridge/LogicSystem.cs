using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public static class LogicSystem
    {
        public static bool IsLastHitUi
        {
            get { return GfxSystem.Instance.IsLastHitUi; }
            set { GfxSystem.Instance.IsLastHitUi = value; }
        }

        //开始加载
        public static void BeginLoading()
        {
            GfxSystem.Instance.BeginLoading();
        }

        public static void EndLoading()
        {
            GfxSystem.Instance.EndLoading();
        }

        public static void SetLoadingBarScene(string name)
        {
            GfxSystem.Instance.SetLoadingBarScene(name);
        }

        //发布事件
        public static void PublishLogicEvent(string evt, string group, params object[] args)
        {
            GfxSystem.Instance.PublishLogicEvent(evt, group, args);
        }

        public static void LogicLog(string format, params object[] args)
        {
            GfxSystem.Instance.CallLogicLog(format, args);
        }

        public static void LogicErrorLog(string format, params object[] args)
        {
            GfxSystem.Instance.CallLogicErrorLog(format, args);
        }

        public static void UpdateLoadingTip(string tip)
        {
            GfxSystem.Instance.UpdateLoadingTip(tip);
        }

        public static void UpdateLoadingProgress(float progress)
        {
            GfxSystem.Instance.UpdateLoadingProgress(progress);
        }

        public static void UpdateVersinoInfo(string info)
        {
            GfxSystem.Instance.UpdateVersionInfo(info);
        }

        public static PublishSubscribeSystem EventChannelForGfx
        {
            get
            {
                return GfxSystem.Instance.EventChannelForGfx;
            }
        }

        public static GameObject GetGameObject(int id)
        {
            return GfxSystem.Instance.GetGameObject(id);
        }

        public static float RadianToDegree(float dir)
        {
            return GfxSystem.Instance.RadianToDegree(dir);
        }
    }
}
