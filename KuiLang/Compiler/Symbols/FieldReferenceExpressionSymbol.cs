using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FieldReferenceExpressionSymbol : ExpressionBaseSymbol<Ast.Expression.FieldReference>
    {
        public FieldReferenceExpressionSymbol( Ast.Expression.FieldReference symbolAst ) : base( symbolAst )
        {
        }

        public FieldSymbol Field { get; internal set; } = null!;
    }
}
