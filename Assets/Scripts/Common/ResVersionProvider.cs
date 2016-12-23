using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DashFire
{
    public class ResVersionData : IData
    {
        public int m_Id = -1;
        public string m_AssetBundleName = string.Empty;
        public string m_AssetName = string.Empty;
        public List<int> m_Depends = null;
        public string m_MD5 = string.Empty;
        public int m_Chapter = 0;
        public long m_Size = 0L;
        public bool CollectDataFromDBC(DBC_Row node)
        {
            m_Id = DBCUtil.ExtractNumeric<int>(node, "Id", -1, true);
            m_AssetBundleName = DBCUtil.ExtractString(node, "AssetBundleName", "", true);
            m_AssetName = DBCUtil.ExtractString(node, "AssetName", "", true);
            m_Depends = DBCUtil.ExtractNumericList<int>(node, "Depends", 0, false);
            m_MD5 = DBCUtil.ExtractString(node, "MD5", "", true);
            m_Chapter = DBCUtil.ExtractNumeric<int>(node, "Chapter", 0, true);
            m_Size = DBCUtil.ExtractNumeric<long>(node, "Size", 0, true);
            return true;
        }
        public int GetId()
        {
            return m_Id;
        }
    }

    public class ResVersionProvider
    {
        #region Singleton
        private static ResVersionProvider s_instance_ = new ResVersionProvider();
        public static ResVersionProvider Instance
        {
            get { return s_instance_; }
        }
        #endregion

        Dictionary<int, ResVersionData> m_ConfigDict = new Dictionary<int, ResVersionData>();
        Dictionary<string, int> m_AssetBundleDict = new Dictionary<string, int>();
        Dictionary<string, int> m_AssetDict = new Dictionary<string, int>();

        public void Clear()
        {
            m_ConfigDict.Clear();
            m_AssetBundleDict.Clear();
            m_AssetDict.Clear();
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
                    ResVersionData data = new ResVersionData();
                    bool ret = data.CollectDataFromDBC(node);
                    if (ret)
                    {
                        m_ConfigDict.Add(data.GetId(), data);
                        m_AssetBundleDict.Add(data.m_AssetBundleName, data.GetId());
                        m_AssetDict.Add(data.m_AssetName, data.GetId());
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
                        ResVersionData data = new ResVersionData();
                        bool ret = data.CollectDataFromDBC(node);
                        if (ret)
                        {
                            m_ConfigDict.Add(data.GetId(), data);
                            m_AssetBundleDict.Add(data.m_AssetBundleName, data.GetId());
                            m_AssetDict.Add(data.m_AssetName, data.GetId());
                        }
                        else
                        {
                            string info = string.Format("DataTempalteMgr.CollectDataFromXml collectData Row:{0} failed!", index);
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

        public ResVersionData GetDataById(int id)
        {
            if (m_ConfigDict.ContainsKey(id))
            {
                return m_ConfigDict[id];
            }
            return null;
        }
        public ResVersionData GetDataByAssetBundleName(string abname)
        {
            if (m_AssetBundleDict.ContainsKey(abname))
            {
                return GetDataById(m_AssetBundleDict[abname]);
            }
            return null;
        }
        public ResVersionData GetDataByAssetName(string name)
        {
            if (m_AssetDict.ContainsKey(name))
            {
                return GetDataById(m_AssetDict[name]);
            }
            return null;
        }
        public Dictionary<int, ResVersionData> GetData()
        {
            return m_ConfigDict;
        }
    }
}
