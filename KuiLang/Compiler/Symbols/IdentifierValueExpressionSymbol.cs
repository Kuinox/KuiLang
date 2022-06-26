using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class IdentifierValueExpressionSymbol : IExpression, ISymbol
    {
        public IdentifierValueExpressionSymbol( ISymbol parent, Ast.Expression.IdentifierValue symbolAst )
        {
            Parent = parent;
            Ast = symbolAst;
        }

        public Ast.Expression.IdentifierValue Ast { get; }
        public ISymbol Parent { get; }

        public ITypedSymbol Field { get; internal set; } = null!;
        public TypeSymbol ReturnType => Field?.Type!;
    }
}
