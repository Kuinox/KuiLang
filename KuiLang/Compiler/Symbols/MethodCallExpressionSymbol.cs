using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallExpressionSymbol : IExpressionSymbol, ISymbol
    {
        public MethodCallExpressionSymbol(ISymbol parent,  Ast.Expression.MethodCall symbolAst)
        {
            Parent = parent;
            SymbolAst = symbolAst;
        }

        public Ast.Expression.MethodCall SymbolAst { get; }
        public IReadOnlyList<IExpressionSymbol> Arguments { get; internal set; } = null!;
        public MethodSymbol TargetMethod { get; internal set; } = null!; // resolve member step.

        public TypeSymbol ReturnType => TargetMethod.ReturnType;

        public ISymbol Parent { get; }
    }
}
