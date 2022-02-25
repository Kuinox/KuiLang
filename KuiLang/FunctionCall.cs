using System.Collections.Generic;

namespace KuiLang
{
    internal class FunctionCall
    {
        public FunctionCall()
        {
        }

        public FunctionCall(FullName functionRef, List<Expression> args)
        {
            FunctionRef = functionRef;
            Args = args;
        }

        public FullName FunctionRef { get; }
        public List<Expression> Args { get; }
    }
}