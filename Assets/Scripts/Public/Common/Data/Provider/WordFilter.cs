using System;
using System.Collections.Generic;
using System.Text;

namespace DashFire
{
    public class WordFilter
    {
        public static WordFilter Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static WordFilter s_Instance = new WordFilter();
        public void Load(string dictPath)
        {
            string path = HomePath.GetAbsolutePath(dictPath);
            if (path != string.Empty)
            {
                List<string> wordList = new List<string>();
                Array.Clear(MEMORYLEXICON, 0, MEMORYLEXICON.Length);
                string[] words = System.IO.File.ReadAllLines(path, System.Text.Encoding.UTF8);
                foreach (string word in words)
                {
                    string key = this.ToDBC(word);
                    wordList.Add(key);
                    //繁体转简体，暂不考虑  
                    //wordList.Add(Microsoft.VisualBasic.Strings.StrConv(key, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0));
                }
                Comparison<string> cmp = delegate(string key1, string key2)
                {
                    return key1.CompareTo(key2);
                };
                wordList.Sort(cmp);
                for (int i = wordList.Count - 1; i > 0; i--)
                {
                    if (wordList[i].ToString() == wordList[i - 1].ToString())
                    {
                        wordList.RemoveAt(i);
                    }
                }
                foreach (var word in wordList)
                {
                    WordGroup group = MEMORYLEXICON[(int)word[0]];
                    if (group == null)
                    {
                        group = new WordGroup();
                        MEMORYLEXICON[(int)word[0]] = group;
                    }
                    group.Add(word.Substring(1));
                }
            }
        }
        //检查是否含有敏感词
        public bool Check(string inputText)
        {
            m_Cursor = 0;
            string str = inputText.Trim();
            if (str == string.Empty)
            {
                return false;
            }
            m_SourceText = ToDBC(str);
            List<string> illegalWords = new List<string>();    //检测到的非法字符集
            char[] tempString = m_SourceText.ToCharArray();
            for (int i = 0; i < m_SourceText.Length; i++)
            {
                //查询以该字为首字符的词组
                WordGroup group = MEMORYLEXICON[(int)ToDBC(m_SourceText)[i]];
                if (group != null)
                {
                    for (int z = 0; z < group.Count(); z++)
                    {
                        string word = group.GetWord(z);
                        if (word.Length == 0 || Examine(word))
                        {
                            string blackword = string.Empty;
                            for (int pos = 0; pos < m_WordLengh + 1; pos++)
                            {
                                blackword += tempString[pos + m_Cursor].ToString();
                                //tempString[pos + m_Cursor] = replaceChar;
                            }
                            illegalWords.Add(blackword);
                            m_Cursor = m_Cursor + m_WordLengh;
                            i = i + m_WordLengh;
                        }
                    }
                }
                m_Cursor++;
            }
            if (illegalWords.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string Filter(string inputText, char replaceChar = '*')
        {
            m_Cursor = 0;
            string str = inputText.Trim();
            m_SourceText = ToDBC(str);
            if (m_SourceText != string.Empty)
            {
                List<string> illegalWords = new List<string>();    //检测到的非法字符集
                char[] tempString = m_SourceText.ToCharArray();
                ;
                for (int i = 0; i < m_SourceText.Length; i++)
                {
                    //查询以该字为首字符的词组
                    WordGroup group = MEMORYLEXICON[(int)ToDBC(m_SourceText)[i]];
                    if (group != null)
                    {
                        for (int z = 0; z < group.Count(); z++)
                        {
                            string word = group.GetWord(z);
                            if (word.Length == 0 || Examine(word))
                            {
                                string blackword = string.Empty;
                                for (int pos = 0; pos < m_WordLengh + 1; pos++)
                                {
                                    blackword += tempString[pos + m_Cursor].ToString();
                                    tempString[pos + m_Cursor] = replaceChar;
                                }
                                illegalWords.Add(blackword);
                                m_Cursor = m_Cursor + m_WordLengh;
                                i = i + m_WordLengh;
                            }
                        }
                    }
                    m_Cursor++;
                }
                return new string(tempString);
            }
            else
            {
                return string.Empty;
            }
        }
        // 检测   
        private bool Examine(string blackWord)
        {
            m_WordLengh = 0;
            //检测源下一位游标
            m_NextCursor = m_Cursor + 1;
            bool found = false;
            //遍历词的每一位做匹配
            for (int i = 0; i < blackWord.Length; i++)
            {
                //特殊字符偏移游标
                int offset = 0;
                if (m_NextCursor >= m_SourceText.Length)
                {
                    break;
                }
                else
                {
                    //检测下位字符如果不是汉字 数字 字符 偏移量加1
                    for (int y = m_NextCursor; y < m_SourceText.Length; y++)
                    {
                        if (!isCHS(m_SourceText[y]) && !isNum(m_SourceText[y]) && !isAlphabet(m_SourceText[y]))
                        {
                            offset++;
                            //避让特殊字符，下位游标如果>=字符串长度 跳出
                            if (m_NextCursor + offset >= m_SourceText.Length)
                                break;
                            m_WordLengh++;
                        }
                        else
                            break;
                    }
                    if ((int)blackWord[i] == (int)m_SourceText[m_NextCursor + offset])
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }
                m_NextCursor = m_NextCursor + 1 + offset;
                m_WordLengh++;
            }
            //待检查的词仅是敏感词的一部分
            if (m_WordLengh < blackWord.Length)
            {
                found = false;
            }
            return found;
        }
        // 判断是否是中文
        private bool isCHS(char character)
        {
            //  中文表意字符的范围 4E00-9FA5
            int charVal = (int)character;
            return (charVal >= 0x4e00 && charVal <= 0x9fa5);
        }
        // 判断是否是数字        
        private bool isNum(char character)
        {
            int charVal = (int)character;
            return (charVal >= 48 && charVal <= 57);
        }
        // 判断是否是字母
        private bool isAlphabet(char character)
        {
            int charVal = (int)character;
            return ((charVal >= 97 && charVal <= 122) || (charVal >= 65 && charVal <= 90));
        }
        /// <summary>
        /// 转半角小写的函数(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        private string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c).ToLower();
        }

        private string m_SourceText = string.Empty;
        private WordGroup[] MEMORYLEXICON = new WordGroup[(int)char.MaxValue];
        private int m_Cursor = 0;     // 检测源游标
        private int m_WordLengh = 0; // 匹配成功后偏移量
        private int m_NextCursor = 0; // 检测词游标
    }

    /// <summary>
    /// 具有相同首字符的词组集合
    /// </summary>
    class WordGroup
    {
        // 集合
        private List<string> groupList;

        public WordGroup()
        {
            groupList = new List<string>();
        }

        // 添加词
        public void Add(string word)
        {
            groupList.Add(word);
        }

        // 获取总数
        public int Count()
        {
            return groupList.Count;
        }

        // 根据下标获取词
        public string GetWord(int index)
        {
            return groupList[index];
        }
    }
}

