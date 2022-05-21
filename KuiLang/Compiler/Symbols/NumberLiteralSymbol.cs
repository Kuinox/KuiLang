using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class NumberLiteralSymbol : ISymbol<Ast.Expression.Literal.Number>, IExpressionSymbol
    {
        public NumberLiteralSymbol( Ast.Expression.Literal.Number symbolAst )
        {
            Value = symbolAst.Value;
            SymbolAst = symbolAst;
        }

        public decimal Value { get; }
        public Ast.Expression.Literal.Number SymbolAst { get; }

        public TypeSymbol ReturnType => TODO; //Can be a constant type.
    }
}
