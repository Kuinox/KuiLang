using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using OneOf;

namespace KuiLang.Semantic
{
    public class FieldSymbol : ISymbol
    {
        public FieldSymbol( Ast.Statement.Definition.FieldDeclaration symbolAst, TypeSymbol parent )
        {
            Name = symbolAst.Name;
            SymbolAst = symbolAst;
            Parent = parent;
        }

        public string Name { get; }
        public TypeSymbol Type { get; internal set; } = null!;
        public IExpressionSymbol? InitValue { get; internal set; }
        public Ast.Statement.Definition.FieldDeclaration SymbolAst { get; }
        public TypeSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;
    }
}
