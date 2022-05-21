using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodParameterSymbol : ISymbol<Ast.Statement.Definition.Parameter>
    {
        public MethodParameterSymbol( Ast.Statement.Definition.Parameter symbolAst, MethodSymbol parent )
        {
            Name = symbolAst.Name;
            SymbolAst = symbolAst;
            Parent = parent;
        }

        public Ast.Statement.Definition.Parameter SymbolAst { get; }
        public MethodSymbol Parent { get; }
        public TypeSymbol Type { get; internal set; } = null!;
        public string Name { get; }
    }
}
