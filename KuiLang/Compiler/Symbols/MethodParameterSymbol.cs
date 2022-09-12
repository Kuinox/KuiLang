using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class ParameterSymbol : ITypedSymbol
    {
        public ParameterSymbol( Ast.Statement.Definition.Typed.Parameter ast, FunctionExpressionSymbol parent )
        {
            Debug.Assert( ast is not null );
            Ast = ast;
            Parent = parent;
        }
        public TypeSymbol Type { get; internal set; } = null!;
        public Ast.Statement.Definition.Typed.Parameter Ast { get; }
        public FunctionExpressionSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;
    }
}
