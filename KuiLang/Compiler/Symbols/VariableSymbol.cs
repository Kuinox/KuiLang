using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public class VariableSymbol : StatementSymbolBase<Ast.Statement.Definition.FieldDeclaration>
    {
        public VariableSymbol(
            SingleOrMultiStatementSymbol parent,
            StatementBlockSymbol statementScope,
            Ast.Statement.Definition.FieldDeclaration symbolAst ) : base( parent, symbolAst )
        {
            StatementScope = statementScope;
        }

        public IExpressionSymbol? InitValue { get; internal set; } = null!;

        public TypeSymbol Type { get; internal set; } = null!; // Ordrered Resolution pass.
        public StatementBlockSymbol StatementScope { get; }
    }
}
