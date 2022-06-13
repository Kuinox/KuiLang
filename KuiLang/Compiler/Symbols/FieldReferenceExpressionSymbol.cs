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
        public FieldReferenceExpressionSymbol( ISymbol parent, Ast.Expression.FieldReference symbolAst )
        {
            Parent = parent;
            SymbolAst = symbolAst;
        }

        public FieldSymbol Field { get; internal set; } = null!;
        public Ast.Expression.FieldReference SymbolAst { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
        public ISymbol? Parent { get; }
    }
}
