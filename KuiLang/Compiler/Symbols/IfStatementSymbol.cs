using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    class IfStatementSymbol : StatementSymbolBase<Ast.Statement.If>, ISymbolWithAStatement
    {
        public IfStatementSymbol( OneOf<ISymbolWithAStatement, StatementBlockSymbol> parent, Ast.Statement.If symbolAst )
            : base( parent, symbolAst )
        {
        }

        public StatementSymbolBase<Ast.Statement> Statement { get; set; } = null!;
    }
}
