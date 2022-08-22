using KuiLang.Semantic;
using KuiLang.Syntax;
using System.Collections.Generic;

namespace KuiLang.Compiler.Symbols
{
    public class FunctionCallExpressionSymbol : IExpressionSymbol
    {
        public FunctionCallExpressionSymbol( ISymbol parent, Ast.Expression.FuncCall ast )
        {
            Parent = parent;
            Ast = ast;
        }

        public Ast.Expression.FuncCall Ast { get; }
        public IReadOnlyList<IExpressionSymbol> Arguments { get; internal set; } = null!;
        public MethodSymbol TargetMethod { get; internal set; } = null!; // resolve member step.
        public TypeSymbol ReturnType => TargetMethod.ReturnType;

        public ISymbol Parent { get; }
    }
}
