using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public class VariableSymbol : StatementSymbol
    {
        public VariableSymbol(
            ISymbol parent,
            StatementBlockSymbol statementScope,
            Ast.Statement.Definition.FieldDeclaration ast ) :base(parent)
        {
            StatementScope = statementScope;
            Ast = ast;
        }

        public IExpressionSymbol? InitValue { get; internal set; } = null!;

        public TypeSymbol Type { get; internal set; } = null!; // Ordrered Resolution pass.
        public StatementBlockSymbol StatementScope { get; }
        public Ast.Statement.Definition.FieldDeclaration Ast { get; }
    }
}
