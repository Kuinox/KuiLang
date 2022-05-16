using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public abstract class ExpressionBaseSymbol<T> : SymbolBase<T> where T : Ast.Expression
    {
        public ExpressionBaseSymbol( T symbolAst ) : base( symbolAst )
        {
        }
    }
}
