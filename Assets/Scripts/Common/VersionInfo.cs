using UnityEngine;
using System.Collections;
using System;

namespace DashFire
{
    public class VersionNum
    {
        public static string[] C_SplitInterval = new string[] { "." };
        public static int C_VersionSlot = 3;

        public bool IsValid;
        public int AppChannel;
        public int AppMaster;
        public int ResBase;

        public VersionNum(string version)
        {
            try
            {
                version = version.Trim();
                string[] splitRet = version.Split(C_SplitInterval, StringSplitOptions.RemoveEmptyEntries);
                if (splitRet != null && splitRet.Length == C_VersionSlot)
                {
                    AppChannel = Convert.ToInt32(splitRet[0]);
                    AppMaster = Convert.ToInt32(splitRet[1]);
                    ResBase = Convert.ToInt32(splitRet[2]);
                    IsValid = true;
                }
                else
                {
                    AppChannel = 0;
                    AppMaster = 0;
                    ResBase = 0;
                    IsValid = false;
                    ResLoadHelper.Log("VersionInstance parse error");
                }
            }
            catch (System.Exception ex)
            {
                ResLoadHelper.Log("VersionInstance parse error ex:" + ex);
                IsValid = false;
            }
        }

        public int GetAppVersionForceValue()
        {
            return ((AppMaster & 0xFF) << (16));
        }
        public int GetResVersionValue()
        {
            return ((ResBase & 0xFFFF) << 0);
        }
        public int GetVersionValue()
        {
            return ((AppMaster & 0xFF) << (16))
              + ((ResBase & 0xFFFF) << (0));
        }
        public string GetAppVersionStr()
        {
            return string.Format("{0}.{1}",
              AppChannel,
              AppMaster);
        }
        public string GetAppVersionForceStr()
        {
            return string.Format("{0}",
              AppMaster);
        }
        public string GeResVersionStr()
        {
            return string.Format("{0}",
              ResBase);
        }
        public string GetVersionStr()
        {
            return string.Format("{0}.{1}.{2}",
              AppChannel,
              AppMaster,
              ResBase);
        }
    }

    public class VersionInfo
    {
        public string IniFile = string.Empty;
        public VersionNum Version;
        public string Platform;
        public string AppName;
        public string Channel;
        public string ResServerURL;

        public bool IsResVersionConfigCached;
        public bool IsAssetDBConfigCached;
        public bool IsResCacheConfigCached;
        public bool IsResSheetConfigCached;

        public int CurChapter;

        public void Load(string iniFile)
        {
            IniFile = iniFile;
            ConfigFile m_IniFile = new ConfigFile(IniFile);
            Version = new VersionNum(m_IniFile.GetSetting("AppSetting", "Version"));
            Platform = m_IniFile.GetSetting("AppSetting", "Platform");
            AppName = m_IniFile.GetSetting("AppSetting", "AppName");
            Channel = m_IniFile.GetSetting("AppSetting", "Channel");
            ResServerURL = m_IniFile.GetSetting("AppSetting", "ResServerURL");

            CurChapter = Convert.ToInt32(m_IniFile.GetSetting("Runtime", "CurChapter"));
            IsResVersionConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResVersionConfigCached"));
            IsAssetDBConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsAssetDBConfigCached"));
            IsResCacheConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResCacheConfigCached"));
            IsResSheetConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResSheetConfigCached"));
        }

        public void Load(string iniFile, byte[] buffer)
        {
            IniFile = iniFile;
            ConfigFile m_IniFile = new ConfigFile(IniFile, buffer);
            Version = new VersionNum(m_IniFile.GetSetting("AppSetting", "Version"));
            Platform = m_IniFile.GetSetting("AppSetting", "Platform");
            AppName = m_IniFile.GetSetting("AppSetting", "AppName");
            Channel = m_IniFile.GetSetting("AppSetting", "Channel");
            ResServerURL = m_IniFile.GetSetting("AppSetting", "ResServerURL");

            CurChapter = Convert.ToInt32(m_IniFile.GetSetting("Runtime", "CurChapter"));
            IsResVersionConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResVersionConfigCached"));
            IsAssetDBConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsAssetDBConfigCached"));
            IsResCacheConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResCacheConfigCached"));
            IsResSheetConfigCached = Convert.ToBoolean(m_IniFile.GetSetting("Runtime", "IsResSheetConfigCached"));
        }

        public void Save()
        {
            ConfigFile m_IniFile = new ConfigFile(IniFile);
            m_IniFile.AddSetting("AppSetting", "Version", Version.GetVersionStr());
            m_IniFile.AddSetting("AppSetting", "Platform", Platform);
            m_IniFile.AddSetting("AppSetting", "AppName", AppName);
            m_IniFile.AddSetting("AppSetting", "Channel", Channel);
            m_IniFile.AddSetting("AppSetting", "ResServerURL", ResServerURL);

            m_IniFile.AddSetting("Runtime", "CurChapter", CurChapter.ToString());
            m_IniFile.AddSetting("Runtime", "IsResVersionConfigCached", IsResVersionConfigCached.ToString());
            m_IniFile.AddSetting("Runtime", "IsAssetDBConfigCached", IsAssetDBConfigCached.ToString());
            m_IniFile.AddSetting("Runtime", "IsResCacheConfigCached", IsResCacheConfigCached.ToString());
            m_IniFile.AddSetting("Runtime", "IsResSheetConfigCached", IsResSheetConfigCached.ToString());
            m_IniFile.SaveSettings(IniFile);
        }

    }
}
