using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class Data_ActionConfig : IData
    {
        public class Data_ActionInfo
        {
            public string m_AnimName;
            public string m_SoundId;
        }

        public int m_ModelId;
        public Data_ActionInfo m_Default;
        public Dictionary<Animation_Type, List<Data_ActionInfo>> m_ActionContainer;
        public float m_CombatStdSpeed;
        public float m_ForwardStdSpeed;
        public float m_SlowStdSpeed;
        public float m_FastStdSpeed;
        public string m_ActionPrefix;

        /**
         * @brief 提取数据
         *
         * @param node
         *
         * @return 
         */
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_ModelId = DBCUtil.ExtractNumeric<int>(node, "ModelId", 0, false);
            m_CombatStdSpeed = DBCUtil.ExtractNumeric<float>(node, "CombatStdSpeed", 3.0f, false);
            m_ForwardStdSpeed = DBCUtil.ExtractNumeric<float>(node, "ForwardStdSpeed", 3.0f, true);
            m_SlowStdSpeed = DBCUtil.ExtractNumeric<float>(node, "SlowStdSpeed", 3.0f, false);
            m_FastStdSpeed = DBCUtil.ExtractNumeric<float>(node, "FastStdSpeed", 3.0f, false);

            m_ActionPrefix = DBCUtil.ExtractString(node, "ActionPrefix", "", false);

            m_ActionContainer = new Dictionary<Animation_Type, List<Data_ActionInfo>>();

            m_ActionContainer[Animation_Type.AT_SLEEP] = ExtractAction(node, "Sleep");
            m_ActionContainer[Animation_Type.AT_Stand] = ExtractAction(node, "Stand_0");
            m_ActionContainer[Animation_Type.AT_IdelStand] = ExtractAction(node, "Idle_Stand");
            m_ActionContainer[Animation_Type.AT_Idle0] = ExtractAction(node, "Idle_0");
            m_ActionContainer[Animation_Type.AT_Idle1] = ExtractAction(node, "Idle_1");
            m_ActionContainer[Animation_Type.AT_Idle2] = ExtractAction(node, "Idle_2");
            m_ActionContainer[Animation_Type.AT_FlyUp] = ExtractAction(node, "FlyUp");
            m_ActionContainer[Animation_Type.AT_FlyDown] = ExtractAction(node, "FlyDown");
            m_ActionContainer[Animation_Type.AT_FlyHurt] = ExtractAction(node, "FlyHurt");
            m_ActionContainer[Animation_Type.AT_FlyDownGround] = ExtractAction(node, "FlyDownGround");
            m_ActionContainer[Animation_Type.AT_OnGround] = ExtractAction(node, "OnGround");
            m_ActionContainer[Animation_Type.AT_Grab] = ExtractAction(node, "Grab");

            m_ActionContainer[Animation_Type.AT_CombatStand] = ExtractAction(node, "CombatStand");
            m_ActionContainer[Animation_Type.AT_CombatRun] = ExtractAction(node, "CombatRun");

            m_ActionContainer[Animation_Type.AT_SlowMove] = ExtractAction(node, "SlowMove");
            m_ActionContainer[Animation_Type.AT_FastMove] = ExtractAction(node, "FastMove");
            m_ActionContainer[Animation_Type.AT_RunForward] = ExtractAction(node, "RunForward_0");

            m_ActionContainer[Animation_Type.AT_Born] = ExtractAction(node, "Born");

            m_ActionContainer[Animation_Type.AT_Hurt0] = ExtractAction(node, "Hurt_0");
            m_ActionContainer[Animation_Type.AT_Hurt1] = ExtractAction(node, "Hurt_1");
            m_ActionContainer[Animation_Type.AT_Hurt2] = ExtractAction(node, "Hurt_2");
            m_ActionContainer[Animation_Type.AT_Dead] = ExtractAction(node, "Dead");
            m_ActionContainer[Animation_Type.AT_Taunt] = ExtractAction(node, "Taunt");
            m_ActionContainer[Animation_Type.AT_PostDead] = ExtractAction(node, "PostDead");

            m_ActionContainer[Animation_Type.AT_GetUp1] = ExtractAction(node, "GetUp_0");
            m_ActionContainer[Animation_Type.AT_GetUp2] = ExtractAction(node, "GetUp_1");

            m_ActionContainer[Animation_Type.AT_Celebrate] = ExtractAction(node, "Celebrate_0");
            m_ActionContainer[Animation_Type.AT_Depressed] = ExtractAction(node, "Depressed_0");
            m_ActionContainer[Animation_Type.AT_SkillSection1] = ExtractAction(node, "SkillSection01");
            m_ActionContainer[Animation_Type.AT_SkillSection2] = ExtractAction(node, "SkillSection02");
            m_ActionContainer[Animation_Type.AT_SkillSection3] = ExtractAction(node, "SkillSection03");
            m_ActionContainer[Animation_Type.AT_SkillSection4] = ExtractAction(node, "SkillSection04");
            m_ActionContainer[Animation_Type.AT_SkillSection5] = ExtractAction(node, "SkillSection05");
            m_ActionContainer[Animation_Type.AT_SkillSection6] = ExtractAction(node, "SkillSection06");
            m_ActionContainer[Animation_Type.AT_SkillSection7] = ExtractAction(node, "SkillSection07");
            m_ActionContainer[Animation_Type.AT_SkillSection8] = ExtractAction(node, "SkillSection08");
            m_ActionContainer[Animation_Type.AT_SkillSection9] = ExtractAction(node, "SkillSection09");
            m_ActionContainer[Animation_Type.AT_SkillSection10] = ExtractAction(node, "SkillSection10");
            m_ActionContainer[Animation_Type.AT_SkillSection11] = ExtractAction(node, "SkillSection11");
            m_ActionContainer[Animation_Type.AT_SkillSection12] = ExtractAction(node, "SkillSection12");
            m_ActionContainer[Animation_Type.AT_SkillSection13] = ExtractAction(node, "SkillSection13");
            m_ActionContainer[Animation_Type.AT_SkillSection14] = ExtractAction(node, "SkillSection14");
            m_ActionContainer[Animation_Type.AT_SkillSection15] = ExtractAction(node, "SkillSection15");
            m_ActionContainer[Animation_Type.AT_SkillSection16] = ExtractAction(node, "SkillSection16");
            m_ActionContainer[Animation_Type.AT_SkillSection17] = ExtractAction(node, "SkillSection17");
            m_ActionContainer[Animation_Type.AT_SkillSection18] = ExtractAction(node, "SkillSection18");
            m_ActionContainer[Animation_Type.AT_SkillSection19] = ExtractAction(node, "SkillSection19");
            m_ActionContainer[Animation_Type.AT_SkillSection20] = ExtractAction(node, "SkillSection20");
            return true;
        }

        /**
         * @brief 私有提取函数
         *
         * @return 
         */
        private List<Data_ActionInfo> ExtractAction(DBC_Row node, string prefix)
        {
            List<Data_ActionInfo> data = new List<Data_ActionInfo>();

            //获取prefix为前缀的数据集
            List<string> childList = DBCUtil.ExtractNodeByPrefix(node, prefix);

            if (childList.Count == 0)
                return data;

            foreach (string child in childList)
            {
                if (string.IsNullOrEmpty(child))
                {
                    continue;
                }

                string outModelPath;
                string outSoundId;
                if (!_ParseModelPath(child, out outModelPath, out outSoundId))
                {
                    string info = "[Err]:ActionConfigProvider.ExtractAction anim name error:" + child;
                    throw new Exception(info);
                }

                Data_ActionInfo action = new Data_ActionInfo();
                action.m_AnimName = m_ActionPrefix + outModelPath;
                action.m_SoundId = outSoundId;
                data.Add(action);
            }

            return data;
        }

        private static bool _ParseModelPath(string path, out string outModelPath, out string outSoundId)
        {
            string[] resut = path.Split(new String[] { "@" }, StringSplitOptions.None);
            if (resut != null)
            {
                outModelPath = (resut.Length > 0) ? resut[0] : "";
                outSoundId = (resut.Length > 1) ? resut[1] : "";
            }
            else
            {
                outModelPath = "";
                outSoundId = "";
            }
            return true;
        }

        /**
         * @brief 获取动作数量
         *
         * @return 
         */
        public int GetActionCountByType(Animation_Type type)
        {
            if (!m_ActionContainer.ContainsKey(type))
            {
                return 0;
            }

            return m_ActionContainer[type].Count;
        }

        /**
         * @brief 获取随即动作
         *
         * @return 
         */
        public Data_ActionInfo GetRandomActionByType(Animation_Type type)
        {
            int count = GetActionCountByType(type);
            if (count > 0)
            {
                int randIndex = Helper.Random.Next(count);
                return m_ActionContainer[type][randIndex];
            }

            return null;
        }

        /**
         * @brief 获取数据ID
         *
         * @return 
         */
        public virtual int GetId()
        {
            return m_ModelId;
        }
    }
    public class ActionConfigProvider
    {
        private DataDictionaryMgr<Data_ActionConfig> m_ActionConfigMgr = new DataDictionaryMgr<Data_ActionConfig>();

        public void Load(string file, string root)
        {
            m_ActionConfigMgr.CollectDataFromDBC(file, root);
        }

        public Data_ActionConfig GetDataById(int id)
        {
            return m_ActionConfigMgr.GetDataById(id);
        }

        public DataDictionaryMgr<Data_ActionConfig> ActionConfigMgr
        {
            get { return m_ActionConfigMgr; }
        }

        public static ActionConfigProvider Instance
        {
            get { return s_Instance; }
        }
        private static ActionConfigProvider s_Instance = new ActionConfigProvider();
    }
}
