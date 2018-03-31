using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSystem
{
    public sealed class SkillConfigManager
    {
        private object m_Lock = new object();
        private Dictionary<int, SkillInstance> m_SkillInstances = new Dictionary<int, SkillInstance>();

        private SkillConfigManager() { }

        public void LoadSkillIfNotExist(int id, string file)
        {
            if (!ExistSkill(id))
            {
                LoadSkill(file);
            }
        }

        public void LoadSkill(string file)
        {
            if(!string.IsNullOrEmpty(file))
            {
                ScriptableData.ScriptableDataFile dataFile = new ScriptableData.ScriptableDataFile();
                if(dataFile.Load(file))
                {
                    Load(dataFile);
                }
            }
        }

        private void Load(ScriptableData.ScriptableDataFile dataFile)
        {
            lock (m_Lock)
            {
                foreach (ScriptableData.ScriptableDataInfo info in dataFile.ScriptableDatas)
                {
                    if (info.GetId() == "skill")
                    {
                        ScriptableData.FunctionData funcData = info.First;
                        if (null != funcData)
                        {
                            ScriptableData.CallData callData = funcData.Call;
                            if (null != callData && callData.HaveParam())
                            {
                                int id = int.Parse(callData.GetParamId(0));
                                if (!m_SkillInstances.ContainsKey(id))
                                {
                                    SkillInstance instance = new SkillInstance();
                                    instance.Init(info);
                                    m_SkillInstances.Add(id, instance);

                                    LogSystem.Debug("ParseSkill {0}", id);
                                }
                            }
                        }
                    }
                }
            }
        }

        public SkillInstance NewSkillInstance(int id)
        {
            SkillInstance instance = null;
            SkillInstance temp = GetSkillInstanceResource(id);
            if(null != temp)
            {
                instance = temp.Clone();
            }
            return instance;
        }

        public bool ExistSkill(int id)
        {
            return null != GetSkillInstanceResource(id);
        }

        private SkillInstance GetSkillInstanceResource(int id)
        {
            SkillInstance instance = null;
            lock (m_Lock)
            {
                if (m_SkillInstances.ContainsKey(id))
                {
                    instance = m_SkillInstances[id];
                }
            }
            return instance;
        }

        public static SkillConfigManager Instance
        {
            get { return s_Instance; }
        }
        private static SkillConfigManager s_Instance = new SkillConfigManager();
    }
}
