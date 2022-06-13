using KuiLang.Semantic;
using KuiLang.Syntax;
using System.Collections.Generic;

namespace KuiLang.Compiler.Symbols
{
    public class MethodSymbol : ISymbol, ISymbolWithAStatement
    {
        readonly List<KeyValuePair<string, MethodParameterSymbol>> _parameterSymbols = new();

        public MethodSymbol( TypeSymbol parent, Ast.Statement.Definition.MethodDeclaration symbolAst )
        {
            Name = symbolAst.Name;
            Parent = parent;
            SymbolAst = symbolAst;
        }

        public TypeSymbol Parent { get; }
        public Ast.Statement.Definition.MethodDeclaration SymbolAst { get; }
        public string Name { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public IReadOnlyList<KeyValuePair<string, MethodParameterSymbol>> ParameterSymbols => _parameterSymbols;

        public IStatementSymbol Statement { get; set; } = null!;

        ISymbol? ISymbol.Parent => Parent;
    }
}
