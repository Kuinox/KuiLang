using KuiLang.Syntax;
using OneOf;

namespace KuiLang.Semantic
{
    public class FieldSymbol : ISymbol<Ast.Statement.Definition.FieldDeclaration>
    {
        public FieldSymbol( Ast.Statement.Definition.FieldDeclaration symbolAst, TypeSymbol parent )
        {
            Name = symbolAst.Name;
            SymbolAst = symbolAst;
            Parent = parent;
        }

        public string Name { get; }
        public TypeSymbol Type { get; internal set; } = null!;
        public Ast.Statement.Definition.FieldDeclaration SymbolAst { get; }
        public TypeSymbol Parent { get; }
    }
}
