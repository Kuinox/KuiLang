using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    class FunctionCallExpressionSymbol : ExpressionBaseSymbol<Ast.Expression.MethodCall>
    {
        public FunctionCallExpressionSymbol( Ast.Expression.MethodCall symbolAst ) : base( symbolAst )
        {
        }

        public MethodSymbol MethodToCall { get; set; } = null!;

        public List<ExpressionBaseSymbol<Ast.Expression>> Arguments { get; } = new List<ExpressionBaseSymbol<Ast.Expression>>();
    }
}
