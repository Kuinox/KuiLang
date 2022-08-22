using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public class VariableSymbol : StatementSymbol, ITypedSymbol
    {
        public VariableSymbol(
            ISymbol parent,
            StatementBlockSymbol statementScope,
            Ast.Statement.Definition.Typed.Field ast ) : base( parent )
        {
            StatementScope = statementScope;
            Ast = ast;
        }

        public IExpressionSymbol? InitValue { get; internal set; } = null!;
        public string Name => Ast.Name;
        public TypeSymbol Type { get; internal set; } = null!; // Ordrered Resolution pass.
        public StatementBlockSymbol StatementScope { get; }
        public Ast.Statement.Definition.Typed.Field Ast { get; }
    }
}
