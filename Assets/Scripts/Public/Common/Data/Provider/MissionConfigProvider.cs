using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class MissionConfig : IData
    {
        public int Id;
        public int MissionType;
        public string Name;
        public string Description;
        public int FollowId;
        public int LevelLimit;
        public int SceneId;
        public int Condition;
        public int Args0;
        public int Args1;
        public int DropId;
        public List<int> TriggerGuides;
        public int UnlockLegacyId;
        public bool IsBornAccept;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            MissionType = DBCUtil.ExtractNumeric<int>(node, "Type", 0, true);
            Name = DBCUtil.ExtractString(node, "Name", "", true);
            Description = DBCUtil.ExtractString(node, "Description", "", false);
            FollowId = DBCUtil.ExtractNumeric<int>(node, "FollowId", 0, false);
            LevelLimit = DBCUtil.ExtractNumeric<int>(node, "LevelLimit", 0, false);
            SceneId = DBCUtil.ExtractNumeric<int>(node, "SceneId", 0, true);
            Condition = DBCUtil.ExtractNumeric<int>(node, "Condition", 0, true);
            Args0 = DBCUtil.ExtractNumeric<int>(node, "Args0", 0, false);
            Args1 = DBCUtil.ExtractNumeric<int>(node, "Args1", 0, false);
            DropId = DBCUtil.ExtractNumeric<int>(node, "DropId", 0, false);
            TriggerGuides = DBCUtil.ExtractNumericList<int>(node, "TriggerGuide", 0, false);
            UnlockLegacyId = DBCUtil.ExtractNumeric<int>(node, "UnlockLegacyId", 0, false);
            IsBornAccept = DBCUtil.ExtractBool(node, "IsBornMission", false, false);

            return true;
        }

        public int GetId()
        {
            return Id;
        }
    }

    public class MissionConfigProvider
    {
        private DataDictionaryMgr<MissionConfig> m_MissionConfigMgr = new DataDictionaryMgr<MissionConfig>();

        public void Load(string file, string root)
        {
            m_MissionConfigMgr.CollectDataFromDBC(file, root);
        }

        public MissionConfig GetDataById(int Id)
        {
            return m_MissionConfigMgr.GetDataById(Id);
        }

        public static MissionConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static MissionConfigProvider s_Instance = new MissionConfigProvider();
    }
}
