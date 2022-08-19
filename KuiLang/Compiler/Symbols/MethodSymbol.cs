using KuiLang.Semantic;
using KuiLang.Syntax;
using System.Collections.Generic;
using System.Diagnostics;

namespace KuiLang.Compiler.Symbols
{
    public class MethodSymbol : IMethodSymbol, ISymbolWithAStatement
    {

        public MethodSymbol( ISymbolWithMethods parent, Ast.Statement.Definition.Typed.Method ast )
        {
            Debug.Assert( parent is not null );
            Name = ast.Name;
            Parent = parent;
            Ast = ast;
        }

        public ISymbolWithMethods Parent { get; }
        public Ast.Statement.Definition.Typed.Method? Ast { get; }
        public string Name { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public OrderedDictionary<string, MethodParameterSymbol> ParameterSymbols { get; } = new();

        public StatementSymbol Statement { get; set; } = null!;

        ISymbol? ISymbol.Parent => Parent;
    }
}
