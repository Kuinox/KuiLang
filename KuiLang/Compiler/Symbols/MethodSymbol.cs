using KuiLang.Semantic;
using KuiLang.Syntax;
using System.Collections.Generic;

namespace KuiLang.Compiler.Symbols
{
    public class MethodSymbol : ISymbol, ISymbolWithAStatement
    {

        public MethodSymbol( ISymbolWithMethods parent, Ast.Statement.Definition.MethodDeclaration ast )
        {
            Name = ast.Name;
            Parent = parent;
            Ast = ast;
        }

        public ISymbolWithMethods Parent { get; }
        public Ast.Statement.Definition.MethodDeclaration Ast { get; }
        public string Name { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public List<KeyValuePair<string, MethodParameterSymbol>> ParameterSymbols { get; } = new();

        public StatementSymbol Statement { get; set; } = null!;

        ISymbol? ISymbol.Parent => Parent.Parent;
    }
}
