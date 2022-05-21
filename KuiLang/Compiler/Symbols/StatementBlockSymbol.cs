using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public sealed class StatementBlockSymbol : StatementSymbolBase<Ast.Statement.Block>
    {
        public StatementBlockSymbol( SingleOrMultiStatementSymbol parent, Ast.Statement.Block ast )
            : base( parent, ast )
        {
        }

        public List<IStatementSymbol> Statements { get; } = new();
    }
}
