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
    public sealed class StatementBlockSymbol : StatementSymbol
    {
        public StatementBlockSymbol( ISymbol parent, Ast.Statement.Block? ast )
            : base( parent )
        {
            Ast = ast;
        }

        public List<StatementSymbol> Statements { get; } = new();
        public Ast.Statement.Block? Ast { get; }
    }
}
