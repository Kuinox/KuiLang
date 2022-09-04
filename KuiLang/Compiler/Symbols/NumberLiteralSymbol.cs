using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class NumberLiteralSymbol : ISymbol, IExpressionSymbol
    {
        public NumberLiteralSymbol( ISymbol parent, Ast.Expression.Literal.Number symbolAst )
        {
            Value = symbolAst.Value;
            Parent = parent;
            SymbolAst = symbolAst;
        }

        public decimal Value { get; }
        public ISymbol Parent { get; }
        public Ast.Expression.Literal.Number SymbolAst { get; }

        public TypeSymbol ReturnType => HardcodedSymbols.NumberType;

        public override string ToString() => Value.ToString();
    }
}
