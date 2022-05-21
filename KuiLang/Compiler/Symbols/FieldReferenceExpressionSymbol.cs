using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FieldReferenceExpressionSymbol : IExpressionSymbol, ISymbol<Ast.Expression.FieldReference>
    {
        public FieldReferenceExpressionSymbol( Ast.Expression.FieldReference symbolAst )
        {
            SymbolAst = symbolAst;
        }

        public FieldSymbol Field { get; internal set; } = null!;
        public Ast.Expression.FieldReference SymbolAst { get; }

        public TypeSymbol ReturnType { get; internal set; } = null!;
    }
}
