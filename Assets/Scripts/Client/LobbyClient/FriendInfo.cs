using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class FriendInfo
    {
        public const int c_Friend_Max = 40;
        private ulong m_Guid = 0;
        private string m_Nickname = null;
        private int m_Level = 1;
        private int m_FightingScore = 0;
        private bool m_IsBlack = false;
        private ItemDataInfo[] m_EquipInfo = new ItemDataInfo[EquipmentStateInfo.c_EquipmentCapacity];
        private List<SkillInfo> m_SkillInfo = new List<SkillInfo>();

        public FriendInfo()
        {
        }

        public ulong Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
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
        public bool IsBlack
        {
            get { return m_IsBlack; }
            set { m_IsBlack = value; }
        }
        public ItemDataInfo[] Equips
        {
            get { return m_EquipInfo; }
        }
        public List<SkillInfo> Skills
        {
            get { return m_SkillInfo; }
        }
    }
}
