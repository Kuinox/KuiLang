using System.Collections.Generic;

namespace KuiLang
{
    public class FunctionCall
    {
        public FunctionCall(FieldLocation functionRef, List<Expression> args)
        {
            FunctionRef = functionRef;
            Args = args;
        }

        public FieldLocation FunctionRef { get; }
        public List<Expression> Args { get; }
    }
}