using KuiLang.Syntax;
using OneOf;

namespace KuiLang.Semantic
{
    public class FieldSymbol : SymbolBase<Ast.Statement.Definition.FieldDeclaration>
    {
        public FieldSymbol( Ast.Statement.Definition.FieldDeclaration symbolAst, TypeSymbol parent ) : base( symbolAst )
        {
            Name = symbolAst.Name;
            Parent = parent;
        }

        public string Name { get; }
        public TypeSymbol Type { get; internal set; } = null!;

        public TypeSymbol Parent { get; }
    }
}
