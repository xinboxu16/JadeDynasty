using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum EnemyType : int
    {
        ET_Monster = 0,
        ET_Boss,
        ET_OnePlayer,
        ET_TwoPlayer,
    }

    public class ExpeditionImageInfo
    {
        public ExpeditionImageInfo()
        {
        }
        public ulong Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
        }
        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }
        public string Nickname
        {
            get { return m_Nickname; }
            set { m_Nickname = value; }
        }
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        public int FightingScore
        {
            get { return m_FightingScore; }
            set { m_FightingScore = value; }
        }
        //public ItemDataInfo[] Equips
        //{
        //    get { return m_EquipInfo; }
        //}
        //public List<SkillInfo> Skills
        //{
        //    get { return m_SkillInfo; }
        //}
        //public ItemDataInfo[] Legacys
        //{
        //    get { return m_LegacyInfo; }
        //}
        private ulong m_Guid = 0;
        private int m_HeroId = 0;
        private string m_Nickname = "";
        private int m_Level = 1;
        private int m_FightingScore = 0;
        //private ItemDataInfo[] m_EquipInfo = new ItemDataInfo[EquipmentStateInfo.c_EquipmentCapacity];
        //private List<SkillInfo> m_SkillInfo = new List<SkillInfo>();
        //private ItemDataInfo[] m_LegacyInfo = new ItemDataInfo[LegacyStateInfo.c_LegacyCapacity];
    }

    public class ExpeditionPlayerInfo
    {
        const int c_MaxExpeditionNum = 12;
        private int m_ActiveTollgate = 0;
        private TollgateData[] m_Tollgates = new TollgateData[c_MaxExpeditionNum];

        public const int c_UnlockLevel = 18;

        public TollgateData[] Tollgates
        {
            get { return m_Tollgates; }
        }

        public int ActiveTollgate
        {
            get { return m_ActiveTollgate; }
            set { m_ActiveTollgate = value; }
        }

        //关卡
        public class TollgateData
        {
            public TollgateData()
            {
                Type = EnemyType.ET_Monster;
                this.m_FlushNum = -1;
                this.m_EnemyAttrList.Clear();
                this.m_EnemyList.Clear();
                this.m_UserImageList.Clear();
                this.m_IsFinish = false;
                this.m_IsPostResult = false;
                this.m_IsPlayAnim = false;
                this.m_IsAcceptedAward = true;
            }
            public void Reset()
            {
                Type = EnemyType.ET_Monster;
                this.m_FlushNum = -1;
                this.m_EnemyAttrList.Clear();
                this.m_EnemyList.Clear();
                this.m_UserImageList.Clear();
                this.m_IsFinish = false;
                this.m_IsPostResult = false;
                this.m_IsPlayAnim = false;
                this.m_IsAcceptedAward = true;
            }
            public EnemyType Type
            {
                get { return m_Type; }
                set
                {
                    m_Type = value;
                    if (EnemyType.ET_Monster == m_Type)
                    {
                        m_FlushNum = 2;
                    }
                    else
                    {
                        m_FlushNum = 1;
                    }
                }
            }
            public List<ExpeditionImageInfo> UserImageList
            {
                get { return m_UserImageList; }
                set { m_UserImageList = value; }
            }
            public List<int> EnemyList
            {
                get { return m_EnemyList; }
            }
            public List<int> EnemyAttrList
            {
                get { return m_EnemyAttrList; }
            }
            public bool IsFinish
            {
                get { return m_IsFinish; }
                set { m_IsFinish = value; }
            }
            public bool IsAcceptedAward
            {
                get { return m_IsAcceptedAward; }
                set { m_IsAcceptedAward = value; }
            }
            public int FlushNum
            {
                get { return m_FlushNum; }
                set { m_FlushNum = value; }
            }
            public bool IsPostResult
            {
                get { return m_IsPostResult; }
                set { m_IsPostResult = value; }
            }
            public bool IsPlayAnim
            {
                get { return m_IsPlayAnim; }
                set { m_IsPlayAnim = value; }
            }
            private EnemyType m_Type = EnemyType.ET_Monster;
            private List<int> m_EnemyList = new List<int>();
            private List<int> m_EnemyAttrList = new List<int>();
            private List<ExpeditionImageInfo> m_UserImageList = new List<ExpeditionImageInfo>();
            private bool m_IsFinish = false;
            private bool m_IsAcceptedAward = true;
            private int m_FlushNum = -1;
            private bool m_IsPostResult = false;
            private bool m_IsPlayAnim = false;
        }
    }
}
