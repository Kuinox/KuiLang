using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public class NumberLiteralSymbol : ISymbol, IExpressionSymbol
    {
        public NumberLiteralSymbol( ISymbol parent, Ast.Expression.Literal.Number symbolAst )
        {
            Value = symbolAst.Value;
            Parent = parent;
            SymbolAst = symbolAst;
            ReturnType = parent.GetRoot().HardcodedSymbols.NumberType;
        }

        public decimal Value { get; }
        public ISymbol Parent { get; }
        public Ast.Expression.Literal.Number SymbolAst { get; }

        public TypeSymbol ReturnType { get; }

        public override string ToString() => Value.ToString();
    }
}
