using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public interface IExpression
    {
        public TypeSymbol ReturnType { get; }
        ISymbol Parent { get; }

    }
}
