using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using OneOf;
using System.Diagnostics;

namespace KuiLang.Semantic
{
    public class FieldSymbol : ITypedSymbol
    {
        public FieldSymbol( Ast.Statement.Definition.Typed.Field ast, TypeSymbol parent )
        {
            Debug.Assert( ast != null );
            Name = ast.Name;
            Ast = ast;
            Parent = parent;
        }

        public string Name { get; }
        public TypeSymbol Type { get; internal set; } = null!;
        public IExpression? InitValue { get; internal set; }
        public Ast.Statement.Definition.Typed.Field Ast { get; }
        public TypeSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;
    }
}
