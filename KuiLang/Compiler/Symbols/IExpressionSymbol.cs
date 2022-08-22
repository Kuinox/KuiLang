using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public interface IExpressionSymbol : ISymbol
    {
        public TypeSymbol ReturnType { get; }
    }
}
