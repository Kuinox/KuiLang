using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class StatementBlockSymbol : StatementSymbolBase<Ast.Statement.Block>
    {
        public StatementBlockSymbol( OneOf<ISymbolWithAStatement, StatementBlockSymbol> parent, Ast.Statement.Block ast )
            : base( parent, ast )
        {
        }

        public List<Ast.Statement> Statements { get; } = new();
    }
}
