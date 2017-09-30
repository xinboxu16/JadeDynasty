using DashFire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableData.Parser
{
    class DslError
    {
        private DslToken tokens;
        private bool mHasError;

        internal DslError(DslToken tokens)
        {
            this.tokens = tokens;
        }

        internal bool HasError
        {
            get { return mHasError; }
        }

        internal void message(string message)
        {
            LogSystem.Error("{0}", message);
        }

        internal short no_entry(short nonterminal, short token, int level)
        {
            mHasError = true;
            //TODO：未实现
            //LogSystem.Error(" syntax error: skipping input {0}", DslString.GetSymbolName(token));
            token = tokens.get(); // advance the input
            return token;
        }

        internal short mismatch(short terminal, short token)
        {
            mHasError = true;
            //TODO：未实现
            //LogSystem.Error(" expecting {0} but found {1}", DslString.GetSymbolName(terminal), DslString.GetSymbolName(token));
            return token;
        }

        internal void input_left()
        {
            LogSystem.Error("input left.");
        }
    }
}
