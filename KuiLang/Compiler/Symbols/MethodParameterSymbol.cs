using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodParameterSymbol : ISymbol
    {
        public MethodParameterSymbol( TypeSymbol type, string name, Ast.Statement.Definition.Parameter symbolAst, MethodSymbol parent )
        {
            SymbolAst = symbolAst;
            Parent = parent;
            Type = type;
            Name = name;
        }
        public TypeSymbol Type { get; internal set; } = null!; // Ordrered Resolution pass.
        public string Name { get; }
        public Ast.Statement.Definition.Parameter SymbolAst { get; }
        public MethodSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;
    }
}
