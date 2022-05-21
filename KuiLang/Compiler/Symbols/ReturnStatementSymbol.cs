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
    public class ReturnStatementSymbol : StatementSymbolBase<Ast.Statement.Return>
    {
        public ReturnStatementSymbol( IExpressionSymbol? returnedValue, SingleOrMultiStatementSymbol parent, Ast.Statement.Return symbolAst ) : base( parent, symbolAst )
        {
            ReturnedValue = returnedValue;
        }

        public IExpressionSymbol? ReturnedValue { get; }
        public TypeSymbol ReturnType { get; internal set; } = null!;
    }
}
