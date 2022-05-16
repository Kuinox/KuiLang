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
    class ReturnStatementSymbol : StatementSymbolBase<Ast.Statement.Return>
    {
        public ReturnStatementSymbol( OneOf<ISymbolWithAStatement, StatementBlockSymbol> parent, Ast.Statement.Return symbolAst ) : base( parent, symbolAst )
        {
        }

        public TypeSymbol ReturnType { get; internal set; } = null!;
    }
}
