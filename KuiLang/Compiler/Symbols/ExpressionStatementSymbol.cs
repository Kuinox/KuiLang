using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class ExpressionStatementSymbol : StatementSymbolBase<Ast.Statement.ExpressionStatement>, IExpressionSymbol
    {
        public ExpressionStatementSymbol(
            SingleOrMultiStatementSymbol parent,
            Ast.Statement.ExpressionStatement symbolAst ): base(parent, symbolAst)
        {
        }

        public IExpressionSymbol Expression { get; internal set; } = null;
        public TypeSymbol ReturnType => Expression.ReturnType;

    }
}
