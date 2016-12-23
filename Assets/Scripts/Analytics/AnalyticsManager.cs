//
//  UmengManager.cs
//
//  Created by ZhuCong on 1/1/14.
//  Copyright 2014 Umeng.com . All rights reserved.
//  Version 1.1

using System;
using System.Collections.Generic;
using UnityEngine;
using Umeng;
namespace DashFire
{
    public class AnalyticsManager
    {
        private static string C_AppKey = "53df2593fd98c5bd5e00392e";
        private static string C_Channel = "App Store";
        private static bool C_EnableLog = false;
        private static List<System.Object> m_EventList = new List<System.Object>();

        public static void Init()
        {
#if UNITY_IPHONE || UNITY_ANDROID
      System.Object eventObj = null;
      // SetUserLevel
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string>(
        "ge_SetUserLevel", "analytics", GA.SetUserLevel);
      if (eventObj != null) m_EventList.Add(eventObj);

      // SetUserInfo
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string, GA.Gender, int, string>(
        "ge_SetUserInfo", "analytics", GA.SetUserInfo);
      if (eventObj != null) m_EventList.Add(eventObj);

      // StartLevel
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string>(
        "ge_StartLevel", "analytics", GA.StartLevel);
      if (eventObj != null) m_EventList.Add(eventObj);

      // FinishLevel
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string>(
        "ge_FinishLevel", "analytics", GA.FinishLevel);
      if (eventObj != null) m_EventList.Add(eventObj);

      // FailLevel
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string>(
        "ge_FailLevel", "analytics", GA.FailLevel);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Pay
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<double, GA.PaySource, double>(
        "ge_Pay", "analytics", GA.Pay);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Pay
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<double, GA.PaySource, string, int, double>(
        "ge_PayItem", "analytics", GA.Pay);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Buy
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string, int, double>(
        "ge_Buy", "analytics", GA.Buy);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Use
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string, int, double>(
        "ge_Use", "analytics", GA.Use);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Bonus
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<double, GA.BonusSource>(
        "ge_Bonus", "analytics", GA.Bonus);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Bonus
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string, int, double, GA.BonusSource>(
        "ge_BonusItem", "analytics", GA.Bonus);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Bonus
      eventObj = LogicSystem.EventChannelForGfx.Subscribe<string, int, double, GA.BonusSource>(
        "ge_BonusItem", "analytics", GA.Bonus);
      if (eventObj != null) m_EventList.Add(eventObj);

      // Start App
				//请到 http://www.umeng.com/analytics 获取app key
				GA.StartWithAppKeyAndChannelId (C_AppKey , C_Channel);
				//调试时开启日志
				GA.SetLogEnabled (C_EnableLog);
#endif
        }
        public static void Exit()
        {
#if UNITY_IPHONE || UNITY_ANDROID
      foreach (System.Object eventObj in m_EventList) {
        if (eventObj != null) {
          DashFire.LogicSystem.EventChannelForGfx.Unsubscribe(eventObj);
        }
      }
      m_EventList.Clear();
#endif
        }
    }
}
