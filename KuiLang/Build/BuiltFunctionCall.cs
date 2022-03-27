using System.Collections.Generic;

namespace KuiLang.Build
{
    public class BuiltFunctionCall
    {
        public BuiltFunctionCall(BuiltMethod builtMethod, BuiltExpression[] builtExpressions)
        {
            BuiltMethod = builtMethod;
            BuiltExpressions = builtExpressions;
        }

        public BuiltMethod BuiltMethod { get; }
        public IReadOnlyCollection<BuiltExpression> BuiltExpressions { get; }
    }
}