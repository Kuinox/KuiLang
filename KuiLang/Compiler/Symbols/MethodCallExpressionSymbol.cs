using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallExpressionSymbol : IExpressionSymbol
    {
        public MethodCallExpressionSymbol( ISymbol parent, IExpressionSymbol callTarget, Ast.Expression.FuncCall.MethodCall ast )
        {
            Parent = parent;
            Ast = ast;
            CallTarget = callTarget;
        }

        public ISymbol Parent { get; }
        public Ast.Expression.FuncCall.MethodCall Ast { get; }
        public IReadOnlyList<IExpressionSymbol> Arguments { get; internal set; } = null!;

        public TypeSymbol ReturnType => TargetMethod?.ReturnType!;
        public MethodSymbol TargetMethod { get; internal set; } = null!; // resolve member step.
        public IExpressionSymbol CallTarget { get; } = null!;

        public override string ToString() =>
$@"{{
    ""{nameof( MethodCallExpressionSymbol )}"": {{
        ""Target"": {CallTarget?.ToString() ?? "null"},
        ""Arguments"": [
    {string.Join( ",\n", Arguments )}
    ],
        ""TargetMethod"": {(TargetMethod?.Ast.Name is null ? "null" : $@"""{TargetMethod?.Ast.Name}""")},
        ""Type"": ""{ReturnType?.Identifier.ToString() ?? "null"}""
    }}
}}";

    }
}
