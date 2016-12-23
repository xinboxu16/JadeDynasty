using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DashFire
{
    public class ClientResVersionData : IData
    {
        public string m_Name;
        public string m_MD5;
        public bool m_IsBuildIn = false;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Name = DBCUtil.ExtractString(node, "Name", "", true);
            m_MD5 = DBCUtil.ExtractString(node, "MD5", "", true);
            m_IsBuildIn = DBCUtil.ExtractBool(node, "IsBuildIn", false, true);
            return true;
        }

        public int GetId()
        {
            return 0;
        }
    }

    public class ClientResVersionProvider
    {
        #region Singleton
        private static ClientResVersionProvider s_instance_ = new ClientResVersionProvider();
        public static ClientResVersionProvider Instance
        {
            get { return s_instance_; }
        }
        #endregion

        Dictionary<string, ClientResVersionData> m_ConfigDict = new Dictionary<string, ClientResVersionData>();
        public void Clear()
        {
            m_ConfigDict.Clear();
        }

        public bool CollectDataFromDBC(string file)
        {
            bool result = true;
            DBC document = new DBC();
            document.Load(file);
            for (int index = 0; index < document.RowNum; index++)
            {
                DBC_Row node = document.GetRowByIndex(index);
                if (node != null)
                {
                    ClientResVersionData data = new ClientResVersionData();
                    bool ret = data.CollectDataFromDBC(node);
                    if (ret)
                    {
                        if (m_ConfigDict.ContainsKey(data.m_Name))
                        {
                            m_ConfigDict.Remove(data.m_Name);
                        }
                        m_ConfigDict.Add(data.m_Name, data);
                    }
                    else
                    {
                        string info = string.Format("DataTempalteMgr.CollectDataFromXml collectData Row:{0} failed!", index);
                        LogSystem.Assert(ret, info);
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool CollectDataFromDBC(byte[] bytes)
        {
            bool result = true;
            MemoryStream ms = null;
            StreamReader sr = null;
            try
            {
                DBC document = new DBC();
                ms = new MemoryStream(bytes);
                ms.Seek(0, SeekOrigin.Begin);
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                sr = new StreamReader(ms, encoding);
                document.LoadFromStream(sr);
                for (int index = 0; index < document.RowNum; index++)
                {
                    DBC_Row node = document.GetRowByIndex(index);
                    if (node != null)
                    {
                        ClientResVersionData data = new ClientResVersionData();
                        bool ret = data.CollectDataFromDBC(node);
                        if (ret)
                        {
                            if (m_ConfigDict.ContainsKey(data.m_Name))
                            {
                                m_ConfigDict.Remove(data.m_Name);
                            }
                            m_ConfigDict.Add(data.m_Name, data);
                        }
                        else
                        {
                            string info = string.Format("ClCollectDataFromDBC collectData Row:{0} failed!", index);
                            LogSystem.Assert(ret, info);
                            result = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                string info = string.Format("DataTempalteMgr.CollectDataFromXml exception ex:", ex);
                LogSystem.Assert(false, info);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return result;
        }

        public ClientResVersionData GetDataByName(string name)
        {
            if (m_ConfigDict.ContainsKey(name))
            {
                return m_ConfigDict[name];
            }
            return null;
        }
        public void AddData(ClientResVersionData data)
        {
            if (!m_ConfigDict.ContainsKey(data.m_Name))
            {
                m_ConfigDict.Add(data.m_Name, data);
            }
        }
        public Dictionary<string, ClientResVersionData> GetArray()
        {
            return m_ConfigDict;
        }
    }
}
