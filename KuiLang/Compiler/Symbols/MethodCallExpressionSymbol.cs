using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallExpressionSymbol : IExpression, ISymbol
    {
        public MethodCallExpressionSymbol(ISymbol parent,  Ast.Expression.MethodCall symbolAst)
        {
            Parent = parent;
            Ast = symbolAst;
        }

        public Ast.Expression.MethodCall Ast { get; }
        public IReadOnlyList<IExpression> Arguments { get; internal set; } = null!;
        public MethodSymbol TargetMethod { get; internal set; } = null!; // resolve member step.

        public TypeSymbol ReturnType => TargetMethod.ReturnType;

        public ISymbol Parent { get; }
    }
}
