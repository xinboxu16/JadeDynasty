using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
 * 整个文件就是解析txt文本数据文件
 * 将第一行的头放入Header表中
 * 将数据文件放入m_DataBuf(List)和m_HashData(MyDictionary)
 */
namespace DashFire
{
    public class DBC_Row
    {

        /**
         * @brief 数据
         */
        private List<string> m_Data;
        public List<string> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        /**
         * @brief 返回行号
         */
        public int RowIndex
        {
            get { return m_RowIndex; }
        }

        /**
         * @brief 对数据文件的引用
         */
        private DBC m_DBC;

        /**
         * @brief 当前行所处行号
         */
        private int m_RowIndex;

        public DBC_Row(DBC dbc, int rowIndex = -1)
        {
            m_Data = new List<string>();
            m_DBC = dbc;
            m_RowIndex = -1;
        }

        /**
         * @brief 析构函数 
         */
        ~DBC_Row()
        {
            m_Data.Clear();
            m_Data = null;
            m_DBC = null;
        }

        /**
         * @brief 是否包含数据
         */
        public bool HasFields
        {
            get { return (m_Data != null && m_Data.Count > 0); }
        }

        /**
         * @brief 按标题列名字读取数据
         *
         * @param name
         *
         * @return 
         */
        public string SelectFieldByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (m_DBC == null || m_DBC.Header == null || m_DBC.Header.Count == 0)
                return null;

            int index = m_DBC.GetHeaderIndexByName(name);

            if (index >= 0 && index < m_Data.Count)
            {
                return m_Data[index];
            }

            return null;
        }

        /**
         * @brief 按标题列名字前缀读取数据
         *
         * @param namePrefix
         *
         * @return 
         */
        public List<string> SelectFieldsByPrefix(string namePrefix)
        {
            List<string> list = new List<string>();

            if (string.IsNullOrEmpty(namePrefix))
                return list;

            if (m_DBC == null || m_DBC.Header == null || m_DBC.Header.Count == 0)
                return null;

            foreach (string name in m_DBC.Header)
            {
                if (!name.StartsWith(namePrefix))
                    continue;

                int index = m_DBC.GetHeaderIndexByName(name);
                if (index >= 0 && index < m_Data.Count)
                {
                    list.Add(m_Data[index]);
                }
            }

            return list;
        }
    }
    public class DBC
    {
        /**
         * @brief 标题名列表
         */
        private List<string> m_Header;

        /**
         * @brief 文件名
         */
        private string m_FileName;

        /**
         * @brief 文件大小
         */
        private long m_FileSize;

        /**
         * @brief 返回标题名列表
         */
        public List<string> Header
        {
            get { return m_Header; }
        }

        /**
         * @brief 数据池
         */
        private List<DBC_Row> m_DataBuf;

        /**
         * @brief 数据池，用于快速按关键字检索
         */
        private MyDictionary<string, DBC_Row> m_HashData;

        /**
         * @brief 行数
         */
        private int m_RowNum;
        /**
         * @brief 返回行数
         */
        public int RowNum
        {
            get { return m_RowNum; }
        }

        /**
         * @brief 列数
         */
        private int m_ColumNum;
        /**
         * @brief 返回列数
         */
        public int ColumnNum
        {
            get { return m_ColumNum; }
        }

        /**
     * @brief 构造函数
     *
     * @return 
     */
        public DBC()
        {
            m_Header = new List<string>();
            m_DataBuf = new List<DBC_Row>();
            m_HashData = new MyDictionary<string, DBC_Row>();
            m_FileName = "";
            m_FileSize = 0;
            m_RowNum = 0;
            m_ColumNum = 0;
        }

        /**
         * @brief 析构函数
         */
        ~DBC()
        {
            m_Header = null;
            m_DataBuf.Clear();
            m_HashData.Clear();
            m_FileName = "";
        }

        /**
         * @brief 返回标题所在列序号，根据标题名字
         *
         * @param name
         *
         * @return 
         */
        public int GetHeaderIndexByName(string name)
        {
            int ret = -1;
            if (m_Header == null || m_Header.Count == 0)
            {
                return ret;
            }

            if (name == "")
            {
                return ret;
            }

            for (int index = 0; index < m_Header.Count; index++)
            {
                if (name == m_Header[index])
                {
                    ret = index;
                    break;
                }
            }

            return ret;
        }

        /**
         * @brief 返回指定行，根据行号
         *
         * @param index
         *
         * @return 
         */
        public DBC_Row GetRowByIndex(int index)
        {
            if (index < 0 || index >= m_RowNum)
                return null;

            if (m_DataBuf != null && index >= m_DataBuf.Count)
                return null;

            return m_DataBuf[index];
        }

        /**
         * @brief 载入文件
         *
         * @param path
         *
         * @return 
         */
        public bool Load(string path)
        {
            bool ret = false;

            if (path == "" || !FileReaderProxy.Exists(path))
            {
                return ret;
            }
            
            Stream ms = null;
            StreamReader sr = null;
            try
            {
                ms = FileReaderProxy.ReadFileAsMemoryStream(path);
                if (ms == null)
                {
                    LogSystem.Debug("DBC, Warning, Load file error or file is empty: {0}", path);
                    return false;
                }

                m_FileName = path;
                ms.Seek(0, SeekOrigin.Begin);
                m_FileSize = ms.Length;
                if (m_FileSize <= 0 || m_FileSize >= int.MaxValue)
                    return ret;

                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                sr = new StreamReader(ms, encoding);

                ret = LoadFromStream(sr);
                ret = true;
            }
            catch (Exception e)
            {
                string err = "Exception:" + e.Message + "\n" + e.StackTrace + "\n";
                System.Diagnostics.Debug.WriteLine(err);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (ms != null)
                {
                    ms.Close();
                }
            }

            return ret;
        }

        // 私有方法

        /**
         * @brief 创建索引，默认第一列作为关键字
         *
         * @return 
         */
        private void CreateIndex()
        {
            foreach (DBC_Row row in m_DataBuf)
            {
                if (row.Data != null && row.Data.Count > 0)
                {
                    string key = row.Data[0];
                    if (!m_HashData.ContainsKey(key))
                    {
                        m_HashData.Add(key, row);
                    }
                    else
                    {
                        string err = string.Format("DBC.CreateIndex FileName:{0} SameKey:{1}", m_FileName, key);
                        System.Diagnostics.Debug.Assert(false, err);
                    }
                }
            }
        }

        /**
           * @brief 将字符串解析为字符串数组
           *
           * @param vec 字符串,类似于"100,200,200"
           *
           * @return 
           */
        private static List<string> ConvertStringList(string vec, string[] split)
        {
            string[] resut = vec.Split(split, StringSplitOptions.None);
            List<string> list = new List<string>();
            foreach (string str in resut)
            {
                list.Add(str);
            }

            return list;
        }

        /**
         * @brief 从文本文件txt流中读取
         *
         * @param sr
         *
         * @return 
         */
        private bool LoadFromStream_Text(StreamReader sr)
        {
            //--------------------------------------------------------------
            //临时变量
            List<string> vRet = null;
            string strLine = "";

            //读第一行,标题行
            strLine = sr.ReadLine();
            //读取失败，即认为读取结束
            if (strLine == null)
                return false;

            vRet = ConvertStringList(strLine, new string[] { "\t" });
            if (vRet == null || vRet.Count == 0)
                return false;
            m_Header = vRet;

            //--------------------------------------------------------------
            //初始化
            int nRecordsNum = 0;
            int nFieldsNum = vRet.Count;

            //--------------------------------------------------------------
            //开始读取
            DBC_Row dbcRow = null;
            do
            {
                vRet = null;
                dbcRow = null;

                //读取一行
                strLine = sr.ReadLine();
                //读取失败，即认为读取结束
                if (strLine == null) break;

                //是否是注释行
                if (strLine.StartsWith("#")) continue;

                //分解
                vRet = ConvertStringList(strLine, new string[] { "\t" });

                //列数不对
                if (vRet.Count == 0) continue;
                if (vRet.Count != nFieldsNum)
                {
                    //补上空格
                    if (vRet.Count < nFieldsNum)
                    {
                        int nSubNum = nFieldsNum - vRet.Count;
                        for (int i = 0; i < nSubNum; i++)
                        {
                            vRet.Add("");
                        }
                    }
                }

                //第一列不能为空
                if (string.IsNullOrEmpty(vRet[0])) continue;

                dbcRow = new DBC_Row(this, nRecordsNum);
                dbcRow.Data = vRet;

                m_DataBuf.Add(dbcRow);

                nRecordsNum++;
            } while (true);

            //--------------------------------------------------------
            //生成正式数据库
            m_ColumNum = nFieldsNum;
            m_RowNum = nRecordsNum;

            //--------------------------------------------------------
            //创建索引
            CreateIndex();

            return true;
        }

        /**
         * @brief 从文件流中读取
         *
         * @param sr
         *
         * @return 
         */
        public bool LoadFromStream(StreamReader sr)
        {
            return LoadFromStream_Text(sr);
        }
    }
}
