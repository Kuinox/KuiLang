using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallExpressionSymbol : IExpressionSymbol, ISymbol<Ast.Expression.MethodCall>
    {
        public MethodCallExpressionSymbol( Ast.Expression.MethodCall symbolAst, IReadOnlyList<IExpressionSymbol> arguments)
        {
            SymbolAst = symbolAst;
            Arguments = arguments;
        }

        public Ast.Expression.MethodCall SymbolAst { get; }
        public IReadOnlyList<IExpressionSymbol> Arguments { get; }
        public MethodSymbol TargetMethod { get; internal set; } = null!;

        public TypeSymbol ReturnType => TargetMethod.ReturnType;
    }
}
