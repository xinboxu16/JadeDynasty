using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class GowPrizeConfig : IData
    {
        public int m_Ranking;
        public int m_Money;
        public int m_Gold;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Ranking = DBCUtil.ExtractNumeric<int>(node, "Ranking", 0, true);
            m_Money = DBCUtil.ExtractNumeric<int>(node, "Money", 0, false);
            m_Gold = DBCUtil.ExtractNumeric<int>(node, "Gold", 0, false);
            return true;
        }

        public int GetId()
        {
            return m_Ranking;
        }
    }

    public sealed class GowTimeConfig : IData
    {
        public enum TimeTypeEnum : int
        {
            PrizeTime = 1,
            MatchTime,
        }

        public int m_Id;
        public int m_Type;
        public int m_StartHour;
        public int m_StartMinute;
        public int m_StartSecond;
        public int m_EndHour;
        public int m_EndMinute;
        public int m_EndSecond;

        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "ID", 0, true);
            m_Type = DBCUtil.ExtractNumeric<int>(node, "Type", 1, true);
            m_StartHour = DBCUtil.ExtractNumeric<int>(node, "StartHour", 0, true);
            m_StartMinute = DBCUtil.ExtractNumeric<int>(node, "StartMinute", 0, true);
            m_StartSecond = DBCUtil.ExtractNumeric<int>(node, "StartSecond", 0, true);
            m_EndHour = DBCUtil.ExtractNumeric<int>(node, "EndHour", 0, true);
            m_EndMinute = DBCUtil.ExtractNumeric<int>(node, "EndMinute", 0, true);
            m_EndSecond = DBCUtil.ExtractNumeric<int>(node, "EndSecond", 0, true);

            return true;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public sealed class GowConfigProvider
    {
        private DataListMgr<GowPrizeConfig> m_GowPrizeConfigMgr = new DataListMgr<GowPrizeConfig>();
        private DataListMgr<GowTimeConfig> m_GowTimeConfigMgr = new DataListMgr<GowTimeConfig>();

        public void LoadForClient()
        {
            m_GowPrizeConfigMgr.CollectDataFromDBC(FilePathDefine_Client.C_GowPrizeConfig, "GowPrize");
            m_GowTimeConfigMgr.CollectDataFromDBC(FilePathDefine_Client.C_GowTimeConfig, "GowTime");
        }

        public static GowConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static GowConfigProvider s_Instance = new GowConfigProvider();
    }
}
