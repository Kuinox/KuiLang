using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    class FunctionCallExpressionSymbol : IExpressionSymbol, ISymbol<Ast.Expression.MethodCall>
    {
        public FunctionCallExpressionSymbol( Ast.Expression.MethodCall symbolAst )
        {
            SymbolAst = symbolAst;
        }

        public MethodSymbol MethodToCall { get; set; } = null!;

        public List<IExpressionSymbol> Arguments { get; } = new List<IExpressionSymbol>();
        public Ast.Expression.MethodCall SymbolAst { get; }

        public TypeSymbol ReturnType => MethodToCall.ReturnType;
    }
}
