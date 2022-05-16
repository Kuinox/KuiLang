using KuiLang.Semantic;
using KuiLang.Syntax;
using System.Collections.Generic;

namespace KuiLang.Compiler.Symbols
{
    public class MethodSymbol : SymbolBase<Ast.Statement.Definition.MethodDeclaration>, ISymbolWithAStatement
    {
        readonly Dictionary<string, MethodParameterSymbol> _parameterSymbols = new();

        public MethodSymbol( TypeSymbol parent, Ast.Statement.Definition.MethodDeclaration method ) : base( method )
        {
            Name = method.Name;
            Parent = parent;
        }

        public TypeSymbol Parent { get; }
        public string Name { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public IReadOnlyDictionary<string, MethodParameterSymbol> ParameterSymbols => _parameterSymbols;

       public StatementSymbolBase<Ast.Statement> Statement { get; set; } = null!;

    }
}
