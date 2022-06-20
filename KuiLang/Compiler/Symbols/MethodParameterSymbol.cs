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
        public MethodParameterSymbol( string name, Ast.Statement.Definition.Parameter symbolAst, MethodSymbol parent )
        {
            Ast = symbolAst;
            Parent = parent;
            parent.ParameterSymbols.Add( new KeyValuePair<string, MethodParameterSymbol>( name, this ) );
            Name = name;
        }
        public TypeSymbol Type { get; internal set; } = null!;
        public string Name { get; }
        public Ast.Statement.Definition.Parameter Ast { get; }
        public MethodSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;
    }
}
