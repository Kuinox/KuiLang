 using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class MethodCallStatementSymbol : StatementSymbolBase<Ast.Statement.MethodCallStatement>
    {
        public MethodCallStatementSymbol(
            SingleOrMultiStatementSymbol parent,
            Ast.Statement.MethodCallStatement symbolAst ): base(parent, symbolAst)
        {
        }

        public MethodCallExpressionSymbol MethodCallExpression { get; internal set; } = null!; // POST INIT

    }
}
