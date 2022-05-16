using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class ConstantExpressionSymbol : ExpressionBaseSymbol<Ast.Expression.Constant>
    {
        public ConstantExpressionSymbol( Ast.Expression.Constant symbolAst ) : base( symbolAst )
        {
        }
        //TODO, see Constant Ast notes, this should eventually disapear.
    }
}
