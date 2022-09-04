using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class ReturnStatementSymbol : StatementSymbol
    {
        public ReturnStatementSymbol( ISymbol parent, Ast.Statement.Return? ast )
            : base( parent )
        {
            Ast = ast;
        }

        public IExpressionSymbol? ReturnedValue { get; internal set; }
        public Ast.Statement.Return? Ast { get; }

        protected override string ValueString => @$"{{
            ""Expression"": {ReturnedValue?.ToString() ?? "null"}
        }}";
    }
}
