using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableData.Parser
{
    class DslToken
    {
        private string mInput;
        private int mIterator;
        private string mCurToken;
        private string mLastToken;

        private int mLineNumber;
        private int mLastLineNumber;

        private const string mWhiteSpaces = " \t\r\n";
        private const string mDelimiters = "{}()[],;";//分隔符
        private const string mOperators = "~`!%^&*-+=|:<>?/";//操作符
        private Dictionary<string, short> mKeywords = new Dictionary<string, short>();
        private StringBuilder mTokenBuilder = new StringBuilder();

        //在同一程序集内访问
        internal DslToken(string input)
        {
            mInput = input;
            mIterator = 0;

            mLineNumber = 1;
            mLastLineNumber = 1;

            mKeywords["true"] = DslConstants.TRUE_;
            mKeywords["false"] = DslConstants.FALSE_;
        }

        internal short get()
        {
            mTokenBuilder.Length = 0;
            mLastToken = mCurToken;
            mLastLineNumber = mLineNumber;
            bool isSkip = true;
            //跳过注释与白空格
            for(;isSkip && CurChar!=0;)
            {
                isSkip = false;
                //查找是否有空格 换行
                for(;CurChar!=0 && mWhiteSpaces.IndexOf(CurChar)>=0;++mIterator)
                {
                    if(CurChar=='\n')
                    {
                        ++mLineNumber;
                    }
                    isSkip = true;
                }
                //#引导的单行注释
                if(CurChar!=0 && CurChar=='#')
                {
                    for (; CurChar != 0 && CurChar != '\n'; ++mIterator) ;
                    isSkip = true;
                }
                //C++风格的单行注释与多行注释
                if(CurChar!=0 && CurChar=='/' && (NextChar=='/' || NextChar=='*'))
                {
                    ++mIterator;
                    if(CurChar!=0 && CurChar=='/')
                    {
                        ++mIterator;
                        for (; CurChar != 0 && CurChar != '\n'; ++mIterator) ;
                        isSkip = true;
                    }
                    else if (CurChar != 0 && CurChar == '*')
                    {
                        ++mIterator;
                        for (; CurChar != 0; ++mIterator)
                        {
                            if (CurChar == '\n') ++mLineNumber;
                            if(CurChar=='*' && NextChar=='/')
                            {
                                ++mIterator;
                                ++mIterator;
                                break;
                            }
                        }
                        isSkip = true;
                    }
                }
            }
            if(CurChar==0)//输入结束
            {
                mCurToken = "<<eof>>";
                return DslConstants.END_OF_SLK_INPUT_;
            }
            else if(mOperators.IndexOf(CurChar) >= 0)//操作符
            {
                getOperatorToken();
                return getOperatorTokenValue();
            }
            else if(CurChar=='.' && NextChar!=0 && !myisdigit(NextChar,false))
            {
                char c = CurChar;
                ++mIterator;

                mTokenBuilder.Append(c);
                mCurToken = mTokenBuilder.ToString();
                return DslConstants.DOT_;
            }
            else if(mDelimiters.IndexOf(CurChar)>=0)//分隔符
            {
                char c = CurChar;
                ++mIterator;

                mTokenBuilder.Append(c);
                mCurToken = mTokenBuilder.ToString();
                switch (c)
                {
                    case '{':
                        return DslConstants.LBRACE_;
                    case '}':
                        return DslConstants.RBRACE_;
                    case '[':
                        return DslConstants.LBRACK_;
                    case ']':
                        return DslConstants.RBRACK_;
                    case '(':
                        return DslConstants.LPAREN_;
                    case ')':
                        return DslConstants.RPAREN_;
                    case ',':
                        return DslConstants.COMMA_;
                    case ';':
                        return DslConstants.SEMI_;
                    default:
                        return (short)c;
                }
            }
            else//关键字、标识符或常数
            {
                if (CurChar == '"' || CurChar == '\'')//引号括起来的名称或关键字
                {
                    int line = mLineNumber;
                    char c = CurChar;
                    for (++mIterator; CurChar != 0 && CurChar != c; ++mIterator)
                    {
                        if (CurChar == '\n') ++mLineNumber;
                        if (CurChar == '\\')
                        {
                            mTokenBuilder.Append(CurChar);
                            ++mIterator;
                            if (CurChar == 0)
                                break;
                        }
                        mTokenBuilder.Append(CurChar);
                    }
                    if (CurChar != 0)
                    {
                        ++mIterator;
                    }
                    else
                    {
                        //::printf("[行 %d ]：字符串无法结束！\n",line);
                    }
                    mCurToken = mTokenBuilder.ToString();
                    return DslConstants.STRING_;
                }
                else
                {
                    bool isNum = true;
                    bool isHex = false;
                    if (CurChar == '0' && NextChar == 'x')
                    {
                        isHex = true;
                        mTokenBuilder.Append(CurChar);
                        ++mIterator;
                        mTokenBuilder.Append(CurChar);
                        ++mIterator;
                    }
                    for (; CurChar != 0 && (mDelimiters.IndexOf(CurChar) < 0 && mWhiteSpaces.IndexOf(CurChar) < 0 && mOperators.IndexOf(CurChar) < 0); ++mIterator)
                    {
                        if (CurChar == '#')
                            break;
                        else if(CurChar=='/')
                        {
                            if (NextChar == '/' || NextChar == '*')
                            {
                                break;
                            }
                        }
                        else if(CurChar=='.')
                        {
                            if (!isNum)
                            {
                                break;
                            }
                            else
                            {
                                if (NextChar != 0 && !myisdigit(NextChar, isHex))
                                {
                                    break;
                                }
                            }
                        }
                        else if (!myisdigit(CurChar, isHex))
                        {
                            isNum = false;
                        }
                        mTokenBuilder.Append(CurChar);
                    }
                    mCurToken = mTokenBuilder.ToString();	
                    if(mKeywords.ContainsKey(mCurToken))
                    {
                        return mKeywords[mCurToken];
                    }
                    else
                    {
                        if(isNum)
                            return DslConstants.NUMBER_;
                        else
                            return DslConstants.IDENTIFIER_;
                    }
                }
            }
        }

        private short getOperatorTokenValue()
        {
            string curOperator = mCurToken;
            string lastToken = mLastToken;
            bool lastIsOperator = true;
            if (lastToken.Length>0)
            {
                if (isDelimiter(lastToken[0]))//分隔符
                {
                    lastIsOperator = true;
                }
                else
                {
                    lastIsOperator = isOperator(lastToken[0]);
                }
            }
            short val = DslConstants.OP_TOKEN_0_;
             if (curOperator.Length>0)
             {
                 char c0 = curOperator[0];
                 char c1 = (char)0;
                 char c2 = (char)0;
                 if (curOperator.Length > 1)
                     c1 = curOperator[1];
                 if (curOperator.Length > 2)
                     c2 = curOperator[2];
                 if ((c0 == '?' || c0 == ':') && curOperator.Length == 1)
                 {
                     val = DslConstants.OP_TOKEN_1_;
                 }
                 else if (c0 == '|' && c1 == '|')
                 {
                     val = DslConstants.OP_TOKEN_2_;
                 }
                 else if (c0 == '&' && c1 == '&')
                 {
                     val = DslConstants.OP_TOKEN_3_;
                 }
                 else if (c0 == '|' && c1 == 0)
                 {
                     val = DslConstants.OP_TOKEN_4_;
                 }
                 else if (c0 == '^' && c1 == 0)
                 {
                     val = DslConstants.OP_TOKEN_5_;
                 }
                 else if (c0 == '&' && c1 == 0)
                 {
                     val = DslConstants.OP_TOKEN_6_;
                 }
                 else if ((c0 == '=' || c0 == '!') && c1 == '=')
                 {
                     val = DslConstants.OP_TOKEN_7_;
                 }
                 else if ((c0 == '<' || c0 == '>') && (c1 == '=' || c1 == 0))
                 {
                     val = DslConstants.OP_TOKEN_8_;
                 }
                 else if ((c0 == '<' && c1 == '<') || (c0 == '>' && c1 == '>') || (c0 == '>' && c1 == '>' && c2 == '>'))
                 {
                     val = DslConstants.OP_TOKEN_9_;
                 }
                 else if ((c0 == '+' || c0 == '-') && c1 == 0)
                 {
                     if (lastIsOperator)
                         val = DslConstants.OP_TOKEN_12_;
                     else
                         val = DslConstants.OP_TOKEN_10_;
                 }
                 else if ((c0 == '*' || c0 == '/' || c0 == '%') && c1 == 0)
                 {
                     val = DslConstants.OP_TOKEN_11_;
                 }
                 else if ((c0 == '+' && c1 == '+') || (c0 == '-' && c1 == '-') || (c0 == '~' && c1 == 0) || (c0 == '!' && c1 == 0))
                 {
                     val = DslConstants.OP_TOKEN_12_;
                 }
                 else
                 {
                     val = DslConstants.OP_TOKEN_0_;
                 }
             }
             return val;
        }

        internal short peek(int level) // scan next token without consuming it
        {
            short token = 0;
            LogSystem.Info("peek_token is not called in an LL(1) grammar\n");
            return token;
        }

        private void getOperatorToken()
        {
            int st = mIterator;
            switch(CurChar)
            {
                case '+':
                    {
                        ++mIterator;
                        if (CurChar == '+' || CurChar == '=')
                        {
                            ++mIterator;
                        }
                    }
                    break;
                case '-':
                    {
                        ++mIterator;
                        if (CurChar == '-' || CurChar == '=')
                        {
                            ++mIterator;
                        }
                    }
                    break;
                case '>':
                    {
                        ++mIterator;
                        if (CurChar == '=')
                        {
                            ++mIterator;
                        }
                        else if (CurChar == '>')
                        {
                            ++mIterator;
                            if (CurChar == '>')
                            {
                                ++mIterator;
                            }
                            if (CurChar == '=')
                            {
                                ++mIterator;
                            }
                        }
                    }
                    break;
                case '<':
                    {
                        ++mIterator;
                        if (CurChar == '=')
                        {
                            ++mIterator;
                        }
                        else if (CurChar == '<')
                        {
                            ++mIterator;
                            if (CurChar == '=')
                            {
                                ++mIterator;
                            }
                        }
                    }
                    break;
                case '&':
                    {
                        ++mIterator;
                        if (CurChar == '=')
                        {
                            ++mIterator;
                        }
                        else if (CurChar == '&')
                        {
                            ++mIterator;
                            if (CurChar == '=')
                            {
                                ++mIterator;
                            }
                        }
                    }
                    break;
                case '|':
                    {
                        ++mIterator;
                        if (CurChar == '=')
                        {
                            ++mIterator;
                        }
                        else if (CurChar == '|')
                        {
                            ++mIterator;
                            if (CurChar == '=')
                            {
                                ++mIterator;
                            }
                        }
                    }
                    break;
                case '=':
                case '!':
                case '^':
                case '*':
                case '/':
                case '%':
                    {
                        ++mIterator;
                        if (CurChar == '=')
                        {
                            ++mIterator;
                        }
                    }
                    break;
                default:
                    {
                        ++mIterator;
                    }
                    break;
            }
            int ed = mIterator;
            mCurToken = mInput.Substring(st, ed - st);
        }

        private char NextChar
        {
            get
            {
                char c = (char)0;
                if (mIterator + 1 < mInput.Length)
                    c = mInput[mIterator + 1];
                return c;
            }
        }

        private char CurChar
        {
            get
            {
                char c = (char)0;
                if (mIterator < mInput.Length)
                    c = mInput[mIterator];
                return c;
            }
        }

        internal string getLastToken()
        {
            return mLastToken;
        }

        internal int getLineNumber()
        {
            return mLineNumber;
        }

        internal int getLastLineNumber()
        {
            return mLastLineNumber;
        }

        private bool isDelimiter(char c)
        {
            if (0 == c)
                return false;
            else
                return mDelimiters.IndexOf(c) >= 0;
        }

        private bool isOperator(char c)
        {
            if (0 == c)
                return false;
            else
                return mOperators.IndexOf(c) >= 0;
        }

        private static bool myisdigit(char c, bool isHex)
        {
            bool ret = false;
            if (true == isHex)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                    ret = true;
                else
                    ret = false;
            }
            else
            {
                if ((c >= '0' && c <= '9'))
                    ret = true;
                else
                    ret = false;
            }
            return ret;
        }
    }
}
