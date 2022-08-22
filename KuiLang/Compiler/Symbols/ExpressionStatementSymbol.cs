using KuiLang.Semantic;

namespace KuiLang.Compiler.Symbols
{
    public class ExpressionStatementSymbol : StatementSymbol
    {
        public ExpressionStatementSymbol( ISymbol parent ) : base( parent )
        {
        }

        public IExpressionSymbol Expression { get; internal set; }
    }
}
