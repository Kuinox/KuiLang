using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FieldReferenceExpressionSymbol : IExpressionSymbol, ISymbol
    {
        public FieldReferenceExpressionSymbol( ISymbol parent, Ast.Expression.FieldReference ast )
        {
            Parent = parent;
            Ast = ast;
        }

        public FieldSymbol Field { get; internal set; } = null!;
        public Ast.Expression.FieldReference Ast { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public ISymbol? Parent { get; }
    }
}
