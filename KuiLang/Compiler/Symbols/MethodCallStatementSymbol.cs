 using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallStatementSymbol : StatementSymbol
    {
        public MethodCallStatementSymbol( ISymbol parent,
            Ast.Statement.MethodCallStatement ast )
            : base( parent )
        {
            Ast = ast;
        }

        public FunctionCallExpressionSymbol MethodCallExpression { get; internal set; } = null!; // POST INIT

        public Ast.Statement.MethodCallStatement Ast { get; }
    }
}
