using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class RoleInfo
    {
        // 角色GUID
        private ulong m_Guid = 0;
        // 角色昵称               
        private string m_Nickname;
        // 角色职业               
        private int m_HeroId = 0;
        // 角色等级      
        private int m_Level = 0;
        // 新手指导场景
        private int m_NewBieGuideScene;
        // 金钱数
        private int m_Money = 0;
        // 钻石数
        private int m_Gold = 0;
        // 体力
        private int m_CurStamina = 0;
        // 经验
        private int m_Exp = 0;
        // vip等级
        private int m_Vip = 0;
        // 战斗分数
        public float FightingScore { get; set; }
        // 所在的主城场景ID
        private int m_CitySceneId = 0;
        // 购买体力计数
        private int m_BuyStaminaCount = 0;
        // 兑换金币计数
        private int m_BuyMoneyCount = 0;
        // 出售物品收益
        private int m_SellItemGoldIncome = 0;
        // 通关信息
        private Dictionary<int, int> m_SceneInfo = new Dictionary<int, int>();
        // 通关次数
        private Dictionary<int, int> m_ScenesCompletedCountData = new Dictionary<int, int>();
        // 教学信息
        private List<int> m_NewbieGuides = new List<int>();
        // 角色物品信息
        private List<ItemDataInfo> m_Items = new List<ItemDataInfo>();
        // 角色装备信息
        private ItemDataInfo[] m_Equips = new ItemDataInfo[EquipmentStateInfo.c_EquipmentCapacity];
        // 角色技能预设信息
        private List<SkillInfo> m_SkillInfos = new List<SkillInfo>();
        // 角色任务信息
        private MissionStateInfo m_MissionStateInfo = new MissionStateInfo();
        // 神器信息
        private ItemDataInfo[] m_Legacys = new ItemDataInfo[LegacyStateInfo.c_LegacyCapacity];
        // 战神赛信息
        private GowInfo m_GowInfo = new GowInfo();
        // 好友信息
        private Dictionary<ulong, FriendInfo> m_Friends = new Dictionary<ulong, FriendInfo>();

        // 远征信息
        private ExpeditionPlayerInfo m_Expeditioninfo = new ExpeditionPlayerInfo();

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

        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public int BuyStaminaCount
        {
            get { return m_BuyStaminaCount; }
            set { m_BuyStaminaCount = value; }
        }

        public int Money
        {
            get { return m_Money; }
            set { m_Money = value; }
        }

        public int Exp
        {
            get { return m_Exp; }
            set { m_Exp = value; }
        }

        public int Vip
        {
            get { return m_Vip; }
            set { m_Vip = value; }
        }

        public int Gold
        {
            get { return m_Gold; }
            set { m_Gold = value; }
        }

        public int CitySceneId
        {
            get { return m_CitySceneId; }
            set { m_CitySceneId = value; }
        }

        public int BuyMoneyCount
        {
            get { return m_BuyMoneyCount; }
            set { m_BuyMoneyCount = value; }
        }

        // 出售物品钻石收益
        public int SellItemGoldIncome
        {
            get { return m_SellItemGoldIncome; }
            set { m_SellItemGoldIncome = value; }
        }

        public int NewBieGuideScene
        {
            get { return m_NewBieGuideScene; }
            set { m_NewBieGuideScene = value; }
        }

        public UserInfo GetPlayerSelfInfo()
        {
            return WorldSystem.Instance.GetPlayerSelf();
        }

        public Dictionary<int, int> SceneInfo
        {
            get { return m_SceneInfo; }
        }

        public int GetSceneInfo(int sceneId)
        {
            if (m_SceneInfo.ContainsKey(sceneId))
            {
                return m_SceneInfo[sceneId];
            }
            return 0;
        }

        public List<int> NewbieGuides
        {
            get { return m_NewbieGuides; }
            set { m_NewbieGuides = value; }
        }

        public List<ItemDataInfo> Items
        {
            get { return m_Items; }
        }

        public ItemDataInfo[] Equips
        {
            get { return m_Equips; }
        }

        public List<SkillInfo> SkillInfos
        {
            get { return m_SkillInfos; }
        }

        public ItemDataInfo[] Legacys
        {
            get { return m_Legacys; }
        }

        public GowInfo Gow
        {
            get { return m_GowInfo; }
        }

        public Dictionary<ulong, FriendInfo> Friends
        {
            get { return m_Friends; }
        }

        // 任务
        public MissionStateInfo GetMissionStateInfo()
        {
            return m_MissionStateInfo;
        }

        public void SetEquip(int pos, ItemDataInfo info)
        {
            if (null != m_Equips && m_Equips.Length > 0)
            {
                for (int i = 0; i < m_Equips.Length; i++)
                {
                    if (i == pos)
                    {
                        m_Equips[i] = info;
                        break;
                    }
                }
            }
        }

        public int CurStamina
        {
            get { return m_CurStamina; }
            set
            {
                m_CurStamina = value;
            }
        }

        public void SetSceneInfo(int sceneId, int grade)
        {
            if(m_SceneInfo.ContainsKey(sceneId))
            {
                if(m_SceneInfo[sceneId] < grade)
                {
                    m_SceneInfo[sceneId] = grade;
                }
            }
            else
            {
                m_SceneInfo.Add(sceneId, grade);
            }
        }

        public void AddCompletedSceneCount(int sceneId, int count = 1)
        {
            if(m_ScenesCompletedCountData.ContainsKey(sceneId))
            {
                m_ScenesCompletedCountData[sceneId]++;
            }
            else
            {
                m_ScenesCompletedCountData.Add(sceneId, 1);
            }
        }

        public ExpeditionPlayerInfo GetExpeditionInfo()
        {
            return m_Expeditioninfo;
        }

        public void FightingScoreChangeCB(float score)
        {
            if (!Geometry.IsSameFloat(FightingScore, score))
            {
                FightingScore = score;
                Network.LobbyNetworkSystem.Instance.UpdateFightingScore(score);
            }
        }
    }
}
