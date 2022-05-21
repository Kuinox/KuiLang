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
    class IfStatementSymbol : StatementSymbolBase<Ast.Statement.If>, ISymbolWithAStatement<Ast.Statement.If>
    {
        public IfStatementSymbol( SingleOrMultiStatementSymbol parent, Ast.Statement.If symbolAst )
            : base( parent, symbolAst )
        {
        }

        public StatementSymbolBase<Ast.Statement> Statement { get; set; } = null!;
    }
}
