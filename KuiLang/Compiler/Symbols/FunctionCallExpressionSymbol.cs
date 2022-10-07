using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FunctionCallExpressionSymbol : IExpressionSymbol
    {
        public FunctionCallExpressionSymbol( ISymbol parent, Ast.Expression.FuncCall ast )
        {
            Parent = parent;
            Ast = ast;
        }

        public ISymbol Parent { get; }
        public Ast.Expression.FuncCall Ast { get; }
        public IReadOnlyList<IExpressionSymbol> Arguments { get; internal set; } = null!;

        public TypeSymbol ReturnType => TargetMethod?.ReturnType!;
        public FunctionExpressionSymbol TargetMethod { get; internal set; } = null!; // resolve member step.
        public IExpressionSymbol CallTarget { get; internal set; }

        public override string ToString() =>$"Call {CallTarget?.ToString()}({string.Join( ",", Arguments )})";

    }
}
