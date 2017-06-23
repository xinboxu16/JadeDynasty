using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class StrDictionary : IData
    {
        public int Id = 0;
        public string m_String = "";

        public bool CollectDataFromDBC(DBC_Row node)
        {
            Id = DBCUtil.ExtractNumeric<int>(node, "Id", 0, true);
            m_String = DBCUtil.ExtractString(node, "String", "", true);
            return true;
        }
        public int GetId()
        {
            return Id;
        }
    }

    public class StrDictionaryProvider
    {
        private DataDictionaryMgr<StrDictionary> m_StrDictionaryMgr = new DataDictionaryMgr<StrDictionary>();

        public void Load(string file, string root)
        {
            m_StrDictionaryMgr.CollectDataFromDBC(file, root);
        }

        public StrDictionary GetDataById(int id)
        {
            return m_StrDictionaryMgr.GetDataById(id);
        }

        public string GetDictString(int id)
        {
            string ret;
            StrDictionary dict = GetDataById(id);
            if(null != dict && null != dict.m_String)
            {
                ret = dict.m_String;
            }
            else
            {
                ret = "";
            }
            return ret;
        }

        public static StrDictionaryProvider Instance
        {
            get { return s_Instance; }
        }
        private static StrDictionaryProvider s_Instance = new StrDictionaryProvider();
    }

    public static class Dict
    {
        public static string Get(int id)
        {
            return StrDictionaryProvider.Instance.GetDictString(id);
        }
    }
}
