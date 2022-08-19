using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class InstantiateObjectExpression : IExpression
    {
        public InstantiateObjectExpression( ReturnStatementSymbol parent )
        {
            Parent = parent;
        }
        public TypeSymbol ReturnType => (TypeSymbol)Parent.Parent.Parent!;

        public ReturnStatementSymbol Parent { get; }

        ISymbol ISymbol.Parent => Parent;
    }
}
